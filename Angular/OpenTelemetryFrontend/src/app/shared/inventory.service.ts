import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { environment } from 'src/environment/environment';
import { Product } from './product';
import { InventoryProduct } from './inventory-product';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  constructor(private http: HttpClient) { }

  private errorHandler(error: Error | any): Observable<any> {
    console.log(error);
    return of(error);
  } 

  getInventory(): Observable<InventoryProduct[]> {
    return this.http.get(`${environment.inventoryServer}/products`)
      .pipe(map<any, InventoryProduct[]>(res => res), catchError(this.errorHandler))
  }
}
