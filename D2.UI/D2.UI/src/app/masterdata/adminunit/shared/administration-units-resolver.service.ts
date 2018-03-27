import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { IAdministrationUnit } from './iadministration-unit';
import { AdministrationUnitService } from './administration-unit.service';

@Injectable()
export class AdministrationUnitsResolver implements Resolve<Array<IAdministrationUnit>> {

  constructor( private as: AdministrationUnitService) { }
  resolve(): Observable<Array<IAdministrationUnit>> {
    return this.as.listAdministrationUnits();
  }

}


