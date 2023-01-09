import { Component, OnInit } from '@angular/core';
import { InventoryProduct } from '../shared/inventory-product';
import { InventoryService } from '../shared/inventory.service';

@Component({
  selector: 'ba-product-list',
  templateUrl: './product-list.component.html',
  styles: [
  ]
})
export class ProductListComponent implements OnInit {
  products: InventoryProduct[] = [];

  constructor(private inventoryService: InventoryService) {}

  ngOnInit() {
    this.inventoryService.getInventory().subscribe(res => this.products = res);
  }
}
