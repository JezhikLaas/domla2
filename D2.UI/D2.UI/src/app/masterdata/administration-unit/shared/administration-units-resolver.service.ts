import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitService } from './administration-unit.service';

@Injectable()
export class AdministrationUnitsResolver implements Resolve<Array<AdministrationUnit>> {

  constructor( private as: AdministrationUnitService) { }
  resolve(): Observable<Array<AdministrationUnit>> {
    return this.as.listAdministrationUnits();
  }

}


