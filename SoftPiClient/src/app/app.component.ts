import { Component } from '@angular/core';
import {LogService} from './log.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers : [LogService]
})
export class AppComponent {
  logs = [];
  searchStr = '';

  constructor(private logService: LogService) {}
    // this.users = this.userService.users;

    ngOnInit() {
      this.logService.getLog().subscribe(logs => {
       this.logs = logs;
      });

    }
  }

