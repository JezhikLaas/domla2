import { EventEmitter, Injectable, Output } from '@angular/core';
import { MenuItem } from './menu-item';
import { Observable } from 'rxjs';

@Injectable()
export class MenuDisplayService {
  @Output() menuNeeded = new EventEmitter<Array<MenuItem>>();

  constructor() { }
}
