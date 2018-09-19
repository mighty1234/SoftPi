import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import {HttpModule} from '@angular/http';
import {LogComponent} from './log/log.component';
import {FormsModule} from '@angular/forms';
import {SearchPipe} from './search.pipe';

@NgModule({
  declarations: [
    AppComponent,
    LogComponent,
    SearchPipe
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
