import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { IAdministrationUnit } from '../masterdata/adminunit/shared/iadministration-unit';
import { AdministrationUnitService } from '../masterdata/adminunit/shared/administration-unit.service';

@Injectable()
export class DomlaResolver implements Resolve<Array<IAdministrationUnit>> {

  constructor( private as: AdministrationUnitService) { }
  resolve(): Observable<Array<IAdministrationUnit>> {
    return this.as.listAdministrationUnits();
  }

}
