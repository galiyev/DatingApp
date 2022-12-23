import { Component, OnInit } from '@angular/core';
import {AccountService} from "../_services/account.service";
import {Observable, of} from "rxjs";
import {User} from "../_model/user";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any ={}
  loggedIn = false;
  //currentUser$:Observable<User | null> = of(null);

  constructor(public accountService:AccountService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$;
  }

  // избавляемся от вызова этого метода
  getCurrentUser(){
    this.accountService.currentUser$.subscribe({
        next: user => this.loggedIn = !!user,
       error: error => console.log(error)
    })
  }

  logout(){
    this.accountService.logout();
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        this.loggedIn = true;
      },
      error: error=>console.log(error)
    });
  }
}
