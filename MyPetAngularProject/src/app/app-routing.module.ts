import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from 'src/app/home/home.component';
import {LoginComponent} from "./auth-components/login/login.component";
import {RegisterComponent} from "./auth-components/register-component/register.component";
import {UserProfileComponent} from "./profile-components/user-profile/user-profile.component";
import {ChatComponent} from "./messanger-components/chat/chat.component";
import {ChatDomainComponent} from "./messanger-components/chat-domain/chat-domain.component";


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'profile', component: UserProfileComponent},
  {path: 'chat', component: ChatDomainComponent}
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]

})
export class RoutingModule {
}
