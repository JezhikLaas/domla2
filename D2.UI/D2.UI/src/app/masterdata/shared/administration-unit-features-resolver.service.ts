import { Injectable } from '@angular/core';
import { AdministrationUnitFeatureService } from './administration-unit-feature.service';
import { IAdministrationUnitFeature } from './IAdministrationUnitFeature';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable()
export class AdministrationUnitFeaturesResolver implements Resolve<Array<IAdministrationUnitFeature>> {

  constructor( private administrationUnitFeatureService: AdministrationUnitFeatureService) { }

  resolve(): Observable<Array<IAdministrationUnitFeature>> {
    return this.administrationUnitFeatureService.listAdministrationUnitFeature();
  }
}
