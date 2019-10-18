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

  items: any;

  constructor(
    // Init the service here
    private cartService: CartService
  ) { }

  ngOnInit() {
    this.items = [];
    // Get items here.
    // this.cartService.getItem().subscribe((data) => {
    //   this.items = data;
    // });
  }

}
