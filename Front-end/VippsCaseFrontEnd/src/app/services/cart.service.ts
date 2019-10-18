import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private http: HttpClient) { }

  getItems(){
    const token = localStorage.getItem('id_token');
    const headerDict = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Access-Control-Allow-Headers': 'Content-Type',
      'Authorization': `Bearer ${token}`
    }
    
    const requestOptions = {                                                                                                                                                                                 
      headers: new HttpHeaders(headerDict), 
    };
    
    return this.http.get("https://vippscaseapi20191011124052.azurewebsites.net/api/items", requestOptions);
  }

  getItem(){
    const token = localStorage.getItem('id_token');
    const headerDict = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Access-Control-Allow-Headers': 'Content-Type',
      'Authorization': `Bearer ${token}`
    }
    
    const requestOptions = {                                                                                                                                                                                 
      headers: new HttpHeaders(headerDict), 
    };
    
    return this.http.get("https://vippscaseapi20191011124052.azurewebsites.net/api/items/1", requestOptions);
  }

  newCart(userId: number): Observable<any>{
    const token = localStorage.getItem('id_token');

    return this.http.post("https://vippscaseapi20191011124052.azurewebsites.net/api/orders/newCart", {userId: userId}, 
    {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Authorization': `Bearer ${token}`
          }), 
      responseType: 'json'
    }).pipe(
        data => data,
        error => error
    );
  }
}
