import { Component, OnInit } from '@angular/core';
import {AccountService} from "../_services/account.service";
import {Observable, of} from "rxjs";
import {User} from "../_models/user";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {MembersService} from "../_services/members.service";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any ={}
  loggedIn = false;
  //currentUser$:Observable<User | null> = of(null);

  constructor(public accountService:AccountService, 
              private router:Router,
              private toastr:ToastrService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$;
  }

  // избавляемся от вызова этого метода
  getCurrentUser(){
    this.accountService.currentUser$.subscribe({
        next: user => this.loggedIn = !!user,
       error: error => this.toastr.error(error.error)
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => {
        this.router.navigateByUrl('/members')
        // this.memberService.resetUserParams();
      }
      // error: error => this.toastr.error(error.error)
    });
  }
}
