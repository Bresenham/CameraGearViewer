import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gear',
  templateUrl: './gear.component.html'
})
export class GearComponent implements OnInit {

  name: string;
  price: number;
  forumLink: string;
  date: Date;

  constructor(name: string, price: number, forumLink: string, date: Date) {
    this.name = name;
    this.price = price;
    this.forumLink = forumLink;
    this.date = date;
  }

  ngOnInit() {
  }

}
