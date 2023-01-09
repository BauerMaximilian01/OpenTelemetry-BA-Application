import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { environment } from 'src/environment/environment';
import { Order } from './order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }

  private errorHandler(error: Error | any): Observable<any> {
    console.log(error);
    return of(error);
  } 

  getOrders(): Observable<Order[]> {
    return this.http.get(`${environment.orderServer}/orders`)
      .pipe(map<any, Order[]>(res => res), catchError(this.errorHandler));
  }

  createOrder(order: Order) {
    return this.http.post(`${environment.orderServer}/orders`, order)
      .pipe(catchError(this.errorHandler));
  }

  getOrder(id: number): Observable<Order | Error> {
    return this.http.get(`${environment.orderServer}/orders/${id}`)
      .pipe(catchError(this.errorHandler));
  }

  deleteOrder(id: number) {
    return this.http.delete(`${environment.orderServer}/orders/${id}`)
      .pipe(catchError(this.errorHandler));
  }
}
