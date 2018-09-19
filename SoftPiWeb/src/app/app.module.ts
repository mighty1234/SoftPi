import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import {UserComponent} from './user/user.component';
import {HttpModule} from '@angular/http';
import {HoverDirective} from './hover.directive';
import {FooterComponent} from './footer/footer.component';
import {MenuComponent} from './menu/menu.component';
import {HeaderComponent} from './header/header.component';
import {FormsModule} from '@angular/forms';
import {SearchPipe} from './search.pipe';
import { AgmCoreModule } from '@agm/core';
import {EventComponent} from './event/event.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    FooterComponent,
    MenuComponent,
    HeaderComponent,
    EventComponent,
    HoverDirective,
    SearchPipe
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    // google maps
    AgmCoreModule.forRoot({
      apiKey: 'YOUR_KEY'
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
