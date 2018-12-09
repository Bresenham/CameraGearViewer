import { Component, OnInit } from '@angular/core';
import { GearComponent } from '../gear/gear.component';
import { formatDate } from '@angular/common';
import { GearService } from '../services/gear.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'overview',
  templateUrl: './overview.component.html'
})
export class OverviewComponent implements OnInit {

  createdOn = formatDate(Date.now(), "dd.MM.yyyy HH:mm", "en-US");

  gearPieces: Observable<GearComponent[]>;

  headerElements = [
    {
      descriptor: "Name",
      icon: "/assets/imgs/name-32.png",
      style: "default",
      event: ""
    },
    {
      descriptor: "Price",
      icon: "/assets/imgs/description-32.png",
      style: "pointer",
      event: "Price"
    },
    {
      descriptor: "Link",
      icon: "/assets/imgs/timer-32.png",
      style: "default",
      event: ""
    },
    {
      descriptor: "Datum",
      icon: "/assets/imgs/timer-32.png",
      style: "pointer",
      event: "Date"
    }
  ];

  constructor(private gearService: GearService) {}

  loadGearComponents() {
    this.gearPieces = this.gearService.getGearComponents();
  }

  isSortedAsc = false;

  orderBy(property : string) {
    this.isSortedAsc = !this.isSortedAsc;
    const direction = this.isSortedAsc ? "desc" : "asc";
    this.gearPieces = this.gearService.getGearComponentsOrderBy(property, direction);
  }

  ngOnInit() {

  }
}
