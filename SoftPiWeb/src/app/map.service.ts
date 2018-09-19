import {Http} from '@angular/http';
import {Injectable} from '@angular/core';
import {map} from 'rxjs/operators/';

@Injectable()
export  class MapService {
  constructor (private  http: Http) {}
   getLocation(evenId) {
     return this.http.get('');
        }
}
