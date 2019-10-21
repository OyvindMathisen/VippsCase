import { Component, OnInit, Input } from '@angular/core';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.scss']
})
export class ItemListComponent implements OnInit {

  // @Input() anyClassNameYouWant: string = 'any text or no text';
  @Input() displayType = '';
  @Input() items: any;

  constructor(private cartService: CartService) {
  }

  ngOnInit() {
    this.items = [];
  }

  getCart() {
    // Get new cart:
    this.cartService.newCart(parseInt(localStorage.getItem('user_id'), 10)).subscribe((data) => {
      localStorage.setItem('order_id', data.orderId);
      this.items = data.items;
    });
  }

}
