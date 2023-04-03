import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ProductComponent} from 'src/app/product/product.component';
import {ProductDetailsComponent} from './product-details/product-details.component';
import {HomeComponent} from 'src/app/home/home.component';


const routes: Routes = [
  {path: '', component: HomeComponent},
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
