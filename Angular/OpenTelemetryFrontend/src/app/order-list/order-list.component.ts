import { Component, OnInit } from '@angular/core';
import { Order } from '../shared/order';
import { OrderService } from '../shared/order.service';

@Component({
  selector: 'ba-order-list',
  templateUrl: './order-list.component.html',
  styles: [
  ]
})
export class OrderListComponent implements OnInit{
  orders: Order[] = [];

  constructor(private orderService: OrderService){}

  ngOnInit() {
    this.orderService.getOrders().subscribe(res => this.orders = res);
  }
}
