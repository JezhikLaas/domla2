import { Injectable } from '@angular/core';
import {IAdministrationUnit} from './iadministration-unit';
import {AdministrationUnitService} from './administration-unit.service';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import {Observable} from 'rxjs';

@Injectable()
export class AdministrationUnitResolver implements Resolve<IAdministrationUnit> {

  constructor(private audata: AdministrationUnitService) { }
  resolve(route: ActivatedRouteSnapshot): Observable<IAdministrationUnit> {
    return this.audata.getSingle(route.params['id']);
  }

}
