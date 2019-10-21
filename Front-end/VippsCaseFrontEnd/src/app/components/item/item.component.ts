import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})
export class ItemComponent implements OnInit {
  @Input() item: any;

  constructor() { }

  ngOnInit() {
    this.cartService.getItems().subscribe((data) => {
      this.items = data;
    });
  }

  getCart(){
    // Get new cart:
    this.cartService.newCart(parseInt(localStorage.getItem('user_id'), 10)).subscribe((data) => {
      localStorage.setItem('order_id', data['orderId']);
      this.items = data['items'];
      console.log(data);
      console.log(this.items);
      
    });
  }

}
