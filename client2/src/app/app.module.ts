import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {FormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import {SharedModule} from "./_modules/shared/shared.module";
import { TestErrorComponent } from './errors/test-error/test-error.component';
import {ErrorInterceptor} from "./_interceptors/error.interceptor";
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import {JwtInterceptor} from "./_interceptors/jwt.interceptor";
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import {LoadingInterceptor} from "./_interceptors/loading.interceptor";

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailsComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BrowserModule,
    AppRoutingModule,
    NgIf,
    SharedModule
  ],
  providers: [
    {provide:HTTP_INTERCEPTORS, useClass:ErrorInterceptor, multi:true},
    {provide:HTTP_INTERCEPTORS, useClass:JwtInterceptor, multi:true},
    {provide:HTTP_INTERCEPTORS, useClass:LoadingInterceptor, multi:true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
