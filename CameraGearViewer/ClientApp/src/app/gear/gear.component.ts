import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gear',
  templateUrl: './gear.component.html'
})
export class GearComponent implements OnInit {

  name: string;
  description: string;
  date: string;

  constructor(name: string, description: string, date: string) {
    this.name = name;
    this.description = description;
    this.date = date;
  }

  ngOnInit() {
  }

}
