import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.scss']
})
export class ItemListComponent implements OnInit {

  // @Input() anyClassNameYouWant: string = 'any text or no text';
  @Input() displayType = '';
  @Input() items: any;

  constructor() {
  }

  ngOnInit() {
    this.items = [];
    // Get items here.
    // this.cartService.getItem().subscribe((data) => {
    //   this.items = data;
    // });
  }

}
