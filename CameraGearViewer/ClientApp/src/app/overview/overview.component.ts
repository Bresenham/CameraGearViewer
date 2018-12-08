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
      icon: "/assets/imgs/name-32.png"
    },
    {
      descriptor: "Description",
      icon: "/assets/imgs/description-32.png"
    },
    {
      descriptor: "Date Added",
      icon: "/assets/imgs/timer-32.png"
    }
  ];

  constructor(private gearService: GearService) {}

  loadGearComponents() {
    this.gearPieces = this.gearService.getGearComponents();
  }

  ngOnInit() {

  }
}
