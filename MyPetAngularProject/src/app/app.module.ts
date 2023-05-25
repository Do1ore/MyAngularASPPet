import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NavbarComponent} from './navbar/navbar.component';
import {HomeComponent} from './home/home.component';
import {FooterComponent} from './footer/footer.component';
import {LoginComponent} from './auth-components/login/login.component';
import {RegisterComponent} from './auth-components/register-component/register.component';
import {AuthInterceptor} from "./services/auth.interceptor";

import {ToastrModule, GlobalConfig} from "ngx-toastr";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {UserProfileComponent} from './profile-components/user-profile/user-profile.component';
import {ChatComponent} from './messanger-components/chat/chat.component';
import {SignalRMessageService} from "./services/signal-r-message.service";


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    UserProfileComponent,
    ChatComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RoutingModule,
    ToastrModule.forRoot({
      timeOut: 4000,
      progressBar: true,
      positionClass: 'toast-bottom-right',
    } as GlobalConfig),
    BrowserAnimationsModule,
    ReactiveFormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true,
  },
    SignalRMessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
