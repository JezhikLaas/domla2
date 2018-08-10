import { Injectable } from '@angular/core';
import { BaseSettingsService } from './basesettings.service';
import { IBaseSetting } from './ibasesetting';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable()
export class BaseSettingsResolver implements Resolve<Array<IBaseSetting>> {

  constructor( private baseSettingsService: BaseSettingsService) { }

  resolve(): Observable<Array<IBaseSetting>> {
    return this.baseSettingsService.listBaseSettings();
  }
}
