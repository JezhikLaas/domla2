import { Injectable } from '@angular/core';
import {IBaseSettings} from './Ibasesettings';
import {BaseSettingsService} from './basesettings.service';
import {ActivatedRouteSnapshot, Resolve} from '@angular/router';
import {Observable} from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class BaseSettingResolver implements Resolve<IBaseSettings> {

  constructor(private baseSettingsService: BaseSettingsService ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<IBaseSettings> {
    return this.baseSettingsService.getSingleBaseSetting(route.params['id']);
  }
}
