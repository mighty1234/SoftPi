import {Http} from '@angular/http';
import {Injectable} from '@angular/core';
import {map} from 'rxjs/operators/';

@Injectable()
export class LogService {

  constructor(private  http: Http) {
  }
logs = [];
  getLog() {
    return this.http.get('http://localhost:58613/Api/Log/GetAll')
      .pipe(map(response => response.json())).pipe(map(logs => {
      return logs.map(log => {
        return {
          path: log.File.Path,
          fileName: log.File.Name,
          size: log.File.Size,
          company: log.IP.Company,
          country: log.IP.Country,
          requestType: log.requestType,
          requestTime: log.requestTime,
          ip: log.IP.Ip1,
          result: log.result
      };
      });
    }));
  }
}
