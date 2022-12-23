import {Component, OnInit} from '@angular/core';
import {AccountService} from "./_services/account.service";
import {User} from "./_model/user";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  users: any;
  title= 'The Dating App 2';

  constructor(private accountService: AccountService) {

  }

  ngOnInit() {
    // this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }

  // getUsers(){
  //   this.http.get('https://localhost:5001/api/users').subscribe({
  //     next: response => this.users = response,
  //     error: error => console.log(error)
  //   });
  // }


}
