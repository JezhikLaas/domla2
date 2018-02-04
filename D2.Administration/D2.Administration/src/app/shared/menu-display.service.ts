import {EventEmitter, Injectable, Output} from '@angular/core';
import {Observable} from 'rxjs/Observable';

@Injectable()
export class MenuDisplayService {
  @Output() menuNeeded = new EventEmitter<Array<string>>();

  constructor() { }
}
