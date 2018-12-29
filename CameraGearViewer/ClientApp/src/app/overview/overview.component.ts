import { Component, OnInit, ElementRef } from '@angular/core';
import { GearComponent } from '../gear/gear.component';
import { formatDate } from '@angular/common';
import { GearService } from '../services/gear.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'overview',
  templateUrl: './overview.component.html'
})
export class OverviewComponent implements OnInit {

  gearPieces: Observable<GearComponent[]>;

  headerElements = [
    {
      descriptor: "Name",
      icon: "/assets/imgs/name-32.png",
      style: "pointer",
      event: "Name"
    },
    {
      descriptor: "Price",
      icon: "/assets/imgs/description-32.png",
      style: "pointer",
      event: "Price"
    },
    {
      descriptor: "Link",
      icon: "/assets/imgs/link-32.png",
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

  isSortedAsc = false;

  constructor(private gearService: GearService, private element: ElementRef) {
    window.addEventListener("keydown", function (e) {
      /* Catch CTRL-F Shortcut */
      if (e.keyCode === 114 || (e.ctrlKey && e.keyCode === 70)) {
        element.nativeElement.querySelector(".form-control").focus();
        event.preventDefault();
      }
    })
  }

  onSearchInputChange(searchInput: string) {
    if (searchInput.length == 0)
      this.loadGearComponents();
    else
      this.gearPieces = this.gearService.getGearComponentsFiltered(searchInput);
  }

  loadGearComponents() {
    this.gearPieces = this.gearService.getGearComponents();
  }

  orderBy(property : string) {
    this.isSortedAsc = !this.isSortedAsc;
    const direction = this.isSortedAsc ? "desc" : "asc";
    this.gearPieces = this.gearService.getGearComponentsOrderBy(property, direction);
  }

  ngOnInit() {

  }
}
