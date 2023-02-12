import {Component, OnInit} from '@angular/core';
import {Photo} from "../../_models/photo";
import {AdminService} from "../../_services/admin.service";
import {BsModalService} from "ngx-bootstrap/modal";

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {

  photos: Photo[] = [];

  constructor(private adminService: AdminService, private modalService:BsModalService) {
  }
  ngOnInit(): void {
      this.getPhotosToModerate();
  }

  getPhotosToModerate(){
        this.adminService.getPhotosForApproval().subscribe({
      next: photos => {
        this.photos = photos;
        console.log(this.photos);
      }
    })
  }

  approvePhoto(photo: Photo) {
    this.adminService.approvePhoto(photo.id).subscribe({
      next:()=>{
        this.getPhotosToModerate();
      }
    })
  }

  rejectPhoto(photo: Photo) {
    this.adminService.rejectPhoto(photo.id).subscribe({
      next:()=>{
        this.getPhotosToModerate();
      }
    })
  }
}
