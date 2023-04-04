import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import  {RoutingModule} from './app-routing.module';
import { AppComponent } from './app.component';
import { ProductComponent } from './product/product.component';
import { FormsModule } from "@angular/forms";
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductFilterPipe } from './filter/product-filter.pipe';
import { FooterComponent } from './footer/footer.component';
import { LoginComponent } from './auth-components/login/login.component';
import { RegisterComponent } from './auth-components/register-component/register.component';


@NgModule({
  declarations: [
    AppComponent,
      ProductComponent,
      NavbarComponent,
      HomeComponent,
      ProductDetailsComponent,
      ProductFilterPipe,
      FooterComponent,
      LoginComponent,
      RegisterComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
