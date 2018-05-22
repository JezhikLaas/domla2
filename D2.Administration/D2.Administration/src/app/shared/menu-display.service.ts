import {EventEmitter, Injectable, Output} from '@angular/core';
import {Observable} from 'rxjs';
import {MenuItem} from './menu-item';

@Injectable()
export class MenuDisplayService {
  @Output() menuNeeded = new EventEmitter<Array<MenuItem>>();

  constructor() { }
}
