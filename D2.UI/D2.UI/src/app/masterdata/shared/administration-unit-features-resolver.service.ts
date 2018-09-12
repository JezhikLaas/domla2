import { Injectable } from '@angular/core';
import { AdministrationUnitFeatureService } from './administration-unit-feature.service';
import { AdministrationUnitFeature } from './administration-unit-feature';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable()
export class AdministrationUnitFeaturesResolver implements Resolve<Array<AdministrationUnitFeature>> {

  constructor( private administrationUnitFeatureService: AdministrationUnitFeatureService) { }

  resolve(): Observable<Array<AdministrationUnitFeature>> {
    return this.administrationUnitFeatureService.listAdministrationUnitFeature();
  }
}
