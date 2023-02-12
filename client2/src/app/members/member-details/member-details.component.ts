  import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Member} from "../../_models/member";
import {MembersService} from "../../_services/members.service";
import {ActivatedRoute, Router} from "@angular/router";
import {NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions} from "@kolkov/ngx-gallery";
  import {TabDirective, TabsetComponent} from "ngx-bootstrap/tabs";
  import {MessageService} from "../../_services/message.service";
  import {Message} from "../../_models/message";
  import {PresenceService} from "../../_services/presence.service";
  import {AccountService} from "../../_services/account.service";
  import {User} from "../../_models/user";

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs',{static: true}) memberTabs?: TabsetComponent;

  member:Member = {} as Member;
  galleryOptions: NgxGalleryOptions[]  = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];
  user?:User;

  constructor(private accountService:AccountService,
              private  messagesService: MessageService,
              private route:ActivatedRoute,
              public presenceService:PresenceService,
              private router: Router)
  {
          this.accountService.currentUser$.subscribe({
              next: user =>{
                if(user){
                  this.user = user;
                }
              }
          });
          // код для того чтобы таб перезагружался
          this.router.routeReuseStrategy.shouldReuseRoute = ()=>false;
  }


  ngOnInit(): void {
    //this.loadMember();

    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
       next: params =>{
          params['tab'] && this.selectTab(params['tab'])
       }
    })

    this.galleryOptions = [
      {
        width:'500px',
        height:'500px',
        imagePercent:100,
        thumbnailsColumns:4,
        imageAnimation:NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();

  }


  ngOnDestroy(): void {
      this.messagesService.stopHubConnection();
  }
  // loadMember(){
  //   var username = this.route.snapshot.paramMap.get('username');
  //   if(!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: member => {this.member = member;
  //       this.galleryImages = this.getImages();
  //     }
  //   })
  // }

  selectTab(heading: string){
    if(this.memberTabs){
      this.memberTabs.tabs.find(x=>x.heading == heading)!.active = true
    }
  }

  loadMessages(){
    if(this.member){
      this.messagesService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.user){
        this.messagesService.createHubConnection(this.user, this.member.userName);
    } else {
      this.messagesService.stopHubConnection();
    }
  }

  private getImages() {
    if(!this.member) return [];
    const imageUrls = [];
    for (const photo of this.member.photos.filter(x=>x.isApproved)){
      imageUrls.push({
          small: photo.url,
          medium:photo.url,
          big: photo.url
      })
    }

    return imageUrls;
  }

}
