import { Component } from '@angular/core';
import {UsersService} from './users.service';
import {MapService} from './map.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [UsersService]
})
export class AppComponent {
  users = [];
  location =[];
  searchStr = '' ;
  constructor(private userService: UsersService) {
   // this.users = this.userService.users;
  }
    ngOnInit() {
    this.userService.getUsers().subscribe(users => {
      this.users = users;
    });

  }

}


