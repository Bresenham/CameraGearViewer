import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { GearComponent } from '../gear/gear.component';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GearService {

  connectionString = "api/controller";

  constructor(private http: HttpClient) { }

  getGearComponents(): Observable<GearComponent[]> {
    return this.http.get<GearComponent[]>(this.connectionString);
  }

  getGearComponentsOrderBy(property : string, direction : string): Observable<GearComponent[]> {
    return this.http.get<GearComponent[]>(this.connectionString + "/sorted?orderBy=" + property + "&direction=" + direction);
  }
}
