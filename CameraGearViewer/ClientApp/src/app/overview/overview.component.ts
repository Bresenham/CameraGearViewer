import { Component, OnInit, ElementRef } from '@angular/core';
import { GearComponent } from '../gear/gear.component';
import { formatDate } from '@angular/common';
import { GearService } from '../services/gear.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'overview',
  styleUrls: ['./overview.component.css'],
  templateUrl: './overview.component.html'
})
export class OverviewComponent implements OnInit {

  headerElements = [
    {
      descriptor: "Name",
      icon: "/assets/imgs/name-32.png",
      style: "pointer",
      event: "name"
    },
    {
      descriptor: "Price",
      icon: "/assets/imgs/description-32.png",
      style: "pointer",
      event: "price"
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
      event: "date"
    }
  ];

  isSortedAsc = false;
  lastUpdate: Date;
  searchFilterText: string;
  componentList: GearComponent[];
  filteredComponents: GearComponent[];

  constructor(private gearService: GearService, private element: ElementRef) {
    window.addEventListener("keydown", function (e) {
      /* Catch CTRL-F Shortcut */
      if (e.keyCode === 114 || (e.ctrlKey && e.keyCode === 70)) {
        element.nativeElement.querySelector(".form-control").focus();
        event.preventDefault();
      }
    });
  }

  onSearchInputChange() {
    if (this.searchFilterText)
      this.filteredComponents = this.componentList.filter(p => p.name.toLowerCase().includes(this.searchFilterText.toLowerCase()));
    else
      this.filteredComponents = this.componentList;
  }

  loadGearComponents() {
    this.gearService.getGearComponents().subscribe(pieces => {
      var internalList: GearComponent[] = [];
      pieces.forEach(piece => {
        if (!this.componentList || this.componentList.length == 0)
          piece.isNew = true;
        else if (this.componentList.map(itm => itm.forumLink).indexOf(piece.forumLink) == -1)
          piece.isNew = true;
        else
          piece.isNew = false;
        internalList.push(piece);
      });
      this.componentList = internalList;
      this.filteredComponents = this.componentList;
      this.onSearchInputChange();
      this.lastUpdate = new Date;
    });
  }

  orderBy(property : string) {
    this.isSortedAsc = !this.isSortedAsc;
    if (this.isSortedAsc)
      this.filteredComponents.sort(this.dynamicSort(property, 1))
    else
      this.filteredComponents.sort(this.dynamicSort(property, -1))
  }

  dynamicSort(property : string, order : number) {
    var sortOrder = order;
    if (property[0] === "-") {
      sortOrder = -order;
      property = property.substr(1);
    }
    return function (a, b) {
      var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
      return result * sortOrder;
    }
  }

  ngOnInit() {
    this.loadGearComponents();
  }
}
