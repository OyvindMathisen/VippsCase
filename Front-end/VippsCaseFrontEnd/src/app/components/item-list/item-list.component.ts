import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.scss']
})
export class ItemListComponent implements OnInit {

  //@Input() anyClassNameYouWant: string = 'any text or no text';
  @Input() displayType: string = '';

  items = [
    {img: "url.jpg", name: "Macbook", price: 125},
    {img: "url.jpg", name: "Alienware", price: 250}
  ];
  constructor(
    // Init the service here
  ) { }

  ngOnInit() {
    // Get items here.
  }

}
