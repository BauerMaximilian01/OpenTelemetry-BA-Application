import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InventoryProduct } from '../shared/inventory-product';
import { Order } from '../shared/order';
import { OrderService } from '../shared/order.service';

@Component({
  selector: 'div.ba-product-list-item',
  templateUrl: './product-list-item.component.html',
  styles: [
  ]
})
export class ProductListItemComponent {
  @Input() product: InventoryProduct = new InventoryProduct();
  amount: number = 1;
  userForm!: FormGroup;

  constructor(private orderService: OrderService, private fb: FormBuilder, private addedSnackBar: MatSnackBar) {
    this.userForm = this.fb.group({
      username: ['', Validators.required]
    });
    this.userForm.statusChanges.subscribe();
  }

  createOrder() {
    let order = new Order("1", new Date(), this.userForm.value.username,this.product, this.amount, this.amount * this.product.price!);
    
    this.orderService.createOrder(order).subscribe(res => {
      if (res instanceof HttpErrorResponse) {
        this.openSnackBar(res.status + " " + res.error);
        return;
      }

      let addedOrder = res as Order;
      this.openSnackBar('Created Order. Order ID: ' + addedOrder.id);
    });
  }

  increment() {
    this.amount = this.amount + 1;
  }

  decrement() {
    if (this.amount - 1 > 0) {
      this.amount = this.amount - 1;
    }
  }

  openSnackBar(message: string) {
    this.addedSnackBar.open(message, 'ok', {
      horizontalPosition: 'end',
      verticalPosition: 'top',
      duration: 3000
    });
  }
}
