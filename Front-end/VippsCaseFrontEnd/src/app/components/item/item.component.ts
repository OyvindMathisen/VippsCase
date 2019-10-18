import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})
export class ItemComponent implements OnInit {
  items: any;

  constructor(private cartService: CartService) { }

  ngOnInit() {
    this.cartService.getItem().subscribe((data) => {
      this.items = data;
    });
  }

}
