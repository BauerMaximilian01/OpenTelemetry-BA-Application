import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Error } from '../shared/error';
import { Order } from '../shared/order';
import { OrderService } from '../shared/order.service';

@Component({
  selector: 'ba-get-order',
  templateUrl: './get-order.component.html',
  styles: [
  ]
})
export class GetOrderComponent implements OnInit {
  order: Order = new Order();
  getOrderForm!: FormGroup;
  error!: Error;

  constructor(private orderService: OrderService, private fb: FormBuilder, private router: Router) {}

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.getOrderForm = this.fb.group({
      idinput: ['', Validators.required]
    });
  }

  Submit() {
    this.orderService.getOrder(this.getOrderForm.value.idinput).subscribe(res => {
      if (res instanceof HttpErrorResponse) {
        this.error = new Error(res.status, res.error);
        return;
      }

      this.order = res as Order;
    });
  }

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

      this.router.navigateByUrl("/orders");
    });
  }
}
