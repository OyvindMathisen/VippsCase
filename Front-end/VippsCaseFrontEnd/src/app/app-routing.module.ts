import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent } from './views/login-form/login-form.component';
import { PurchasePageComponent } from './views/purchase-page/purchase-page.component';
import { PurchaseHistoryComponent } from './views/purchase-history/purchase-history.component';
import { PurchaseConfirmedComponent } from './views/purchase-confirmed/purchase-confirmed.component';


const routes: Routes = [
  { path: "", component: LoginFormComponent },
  { path: "purchase", component: PurchasePageComponent },
  { path: "confirmation", component: PurchaseConfirmedComponent },
  { path: "profile", component: PurchaseHistoryComponent },
  { path: '**', component: PurchasePageComponent } // TODO: Replace with 404 page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
