import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { OverviewComponent } from './overview/overview.component';
import { GearComponent } from './gear/gear.component';

import { HttpClientModule } from '@angular/common/http';

import { KeyboardShortcutsModule } from 'ng-keyboard-shortcuts';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    OverviewComponent,
    GearComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    KeyboardShortcutsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
