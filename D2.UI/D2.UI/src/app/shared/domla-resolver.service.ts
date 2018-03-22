import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AdministrationUnit } from '../masterdata/adminunit/shared/administration-unit';
import { AdministrationUnitService } from '../masterdata/adminunit/shared/administration-unit.service';

@Injectable()
export class DomlaResolver implements Resolve<Array<AdministrationUnit>> {

  constructor( private as: AdministrationUnitService) { }
  resolve(): Observable<Array<AdministrationUnit>> {
    return this.as.listAdministrationUnits();
  }

}
