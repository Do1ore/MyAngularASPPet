import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ProductComponent} from 'src/app/product/product.component';
import {ProductDetailsComponent} from './product-details/product-details.component';
import {HomeComponent} from 'src/app/home/home.component';
import {LoginComponent} from "./auth-components/login/login.component";
import {RegisterComponent} from "./auth-components/register-component/register.component";


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'home', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'product', component: ProductComponent},
  {path: 'product/:id', component: ProductDetailsComponent}
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]

})
export class RoutingModule {
}
