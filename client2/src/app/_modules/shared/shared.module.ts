import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {ToastrModule} from "ngx-toastr";
import {BsDropdownModule} from "ngx-bootstrap/dropdown";
import {TabsModule} from "ngx-bootstrap/tabs";
import {NgxGalleryModule} from "@kolkov/ngx-gallery";
import {NgxSpinner, NgxSpinnerModule} from "ngx-spinner";
import {FileUploadModule} from "ng2-file-upload";

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:"toast-bottom-right"
    }),
    NgxGalleryModule,
    FileUploadModule,
    NgxSpinnerModule.forRoot({
      type:'line-scale-party'
    })
  ],
  exports:[
    BsDropdownModule,
    TabsModule,
    ToastrModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    FileUploadModule
  ]
})
export class SharedModule { }
