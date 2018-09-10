import { Injectable } from '@angular/core';
import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitService } from './administration-unit.service';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class AdministrationUnitResolver implements Resolve<AdministrationUnit> {

  constructor(private audata: AdministrationUnitService) { }
  resolve(route: ActivatedRouteSnapshot): Observable<AdministrationUnit> {
    return this.audata.getSingle(route.params['id']);
  }
}
