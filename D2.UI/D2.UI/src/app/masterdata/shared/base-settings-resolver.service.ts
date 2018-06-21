import { Injectable } from '@angular/core';
import { BaseSettingsService } from './basesettings.service';
import { IBaseSettings } from './Ibasesettings';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable()
export class BaseSettingsResolver implements Resolve<Array<IBaseSettings>> {

  constructor( private baseSettingsService: BaseSettingsService) { }

  resolve(): Observable<Array<IBaseSettings>> {
    return this.baseSettingsService.listBaseSetings();
  }
}
