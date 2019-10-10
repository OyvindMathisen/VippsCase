import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PurchasePageComponent } from './views/purchase-page/purchase-page.component';


const routes: Routes = [{
  path: "/shopping-cart", component: PurchasePageComponent
}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
