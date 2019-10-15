import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private http: HttpClient) { }

  getItem(){
    return this.http.get("https://vippscaseapi20191011124052.azurewebsites.net/api/items");
  }
}
