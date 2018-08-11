import { Injectable } from '@angular/core';
import { IAdministrationUnitFeature } from './IAdministrationUnitFeature';
import { AdministrationUnitFeatureService } from './administration-unit-feature.service';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AdministrationUnitFeatureResolver implements Resolve<IAdministrationUnitFeature> {

  constructor(private administrationUnitFeatureService: AdministrationUnitFeatureService ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<IAdministrationUnitFeature> {
    return this.administrationUnitFeatureService.getSingleAdministrationUnitFeature(route.params['id']);
  }
}
