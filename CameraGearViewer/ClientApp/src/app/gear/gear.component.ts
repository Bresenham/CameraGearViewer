import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-gear',
  templateUrl: './gear.component.html'
})
export class GearComponent implements OnInit {

  name: string;
  description: string;
  dateAdded: string;

  constructor(name: string, description: string, dateAdded: string) {
    this.name = name;
    this.description = description;
    this.dateAdded = dateAdded;
  }

  ngOnInit() {
  }

}
