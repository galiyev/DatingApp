import { Component, OnInit } from '@angular/core';
import {AccountService} from "../_services/account.service";
import {Observable, of} from "rxjs";
import {User} from "../_model/user";
import {Router} from "@angular/router";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any ={}
  loggedIn = false;
  //currentUser$:Observable<User | null> = of(null);

  constructor(public accountService:AccountService, private router:Router) { }

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
    this.router.navigateByUrl('/');
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => this.router.navigateByUrl('/members'),
      error: error=>console.log(error)
    });
  }
}
