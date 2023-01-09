import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Error } from '../shared/error';
import { Order } from '../shared/order';
import { OrderService } from '../shared/order.service';

@Component({
  selector: 'div.ba-order-list-item',
  templateUrl: './order-list-item.component.html',
  styles: [
  ]
})
export class OrderListItemComponent {
  @Input() order: Order = new Order();
  error!: Error;

  constructor(private orderService: OrderService, private router: Router) {}

  date() {
    let pipe = new DatePipe('en-US');
    return pipe.transform(this.order.date, 'fullDate');
  }

  deleteOrder() {
    this.orderService.deleteOrder(Number(this.order.id)).subscribe(res => {
      if (res instanceof HttpErrorResponse) {
        this.error = new Error(res.status, res.error);
        return;
      }

      this.router.navigateByUrl("/products");
    });
  }
}
