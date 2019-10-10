import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginFormComponent } from './views/login-form/login-form.component';
import { PurchasePageComponent } from './views/purchase-page/purchase-page.component';
import { PurchaseHistoryComponent } from './views/purchase-history/purchase-history.component';
import { PurchaseConfirmedComponent } from './views/purchase-confirmed/purchase-confirmed.component';
import { ItemListComponent } from './components/item-list/item-list.component';
import { ItemComponent } from './components/item/item.component';
import { BillComponent } from './components/bill/bill.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    PurchasePageComponent,
    PurchaseHistoryComponent,
    PurchaseConfirmedComponent,
    ItemListComponent,
    ItemComponent,
    BillComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
