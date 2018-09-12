import { Injectable } from '@angular/core';
import { AdministrationUnitFeature } from './administration-unit-feature';
import { AdministrationUnitFeatureService } from './administration-unit-feature.service';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AdministrationUnitFeatureResolver implements Resolve<AdministrationUnitFeature> {

  constructor(private administrationUnitFeatureService: AdministrationUnitFeatureService ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<AdministrationUnitFeature> {
    return this.administrationUnitFeatureService.getSingleAdministrationUnitFeature(route.params['id']);
  }
}
