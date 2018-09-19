import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'search'

})
export class SearchPipe implements  PipeTransform {

  transform (logs, value) {
    return logs.filter(log => {
      return log.size.includes(value);
    });

  }
}
