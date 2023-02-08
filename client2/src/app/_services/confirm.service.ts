import { Injectable } from '@angular/core';
import {BsModalRef, BsModalService} from "ngx-bootstrap/modal";
import {config, map, Observable} from "rxjs";
import {ConfirmDialogComponent} from "../modals/confirm-dialog/confirm-dialog.component";

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {

  bsModelRef?: BsModalRef<ConfirmDialogComponent>;
  constructor(private modalService:BsModalService) {

  }

  confirm(
    title = 'Confimation',
    message = 'Are u sure u want to do this',
    btnOkText = 'Ok',
    btnCancelText = 'Cancel'
  ): Observable<boolean> {
    const config = {
      initialState:{
        title,
        message,
        btnOkText,
        btnCancelText
      }
    }

    this.bsModelRef = this.modalService.show(ConfirmDialogComponent, config);
    return this.bsModelRef.onHidden!.pipe(map(()=>{
      return this.bsModelRef!.content!.result
    }))
  }


}
