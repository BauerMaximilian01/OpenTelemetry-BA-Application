import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GetOrderComponent } from './get-order/get-order.component';
import { OrderListComponent } from './order-list/order-list.component';
import { ProductListComponent } from './product-list/product-list.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full'
  },
  {
    path: 'index.html',
    redirectTo: 'products',
    pathMatch: 'full'
  },
  {
    path: 'products',
    component: ProductListComponent
  },
  {
    path: 'orders',
    component: OrderListComponent
  },
  {
    path: 'search/orders',
    component: GetOrderComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
