import { Component, OnInit, Input } from '@angular/core';
import { CartService } from 'src/app/services/cart.service';


@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss']
})
export class OrderListComponent implements OnInit {

  @Input() orders: any;

  constructor(private cartService: CartService) {
  }

  ngOnInit() {
    this.cartService.getOrdersByUserId(23).subscribe((data) => {
        this.orders = data;
    })
  }

}
