import { Injectable } from '@angular/core';
import {IBaseSetting} from './ibasesetting';
import {BaseSettingsService} from './basesettings.service';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {Observable} from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class BaseSettingResolver implements Resolve<IBaseSetting> {

  constructor(private baseSettingsService: BaseSettingsService ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<IBaseSetting> {
    return this.baseSettingsService.getSingleBaseSetting(route.params['id']);
  }
}
