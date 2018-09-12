import { throwError as observableThrowError, Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import { AccountService } from '../../shared/account.service';
import { HttpClient } from '@angular/common/http';
import { catchError, switchMap } from 'rxjs/internal/operators';
import { IAdministrationUnitFeature } from './IAdministrationUnitFeature';
import { List } from 'linqts';
import { map } from 'rxjs/operators';
import { AdminUnitFactory } from '../administration-unit/shared/admin-unit-factory';
import { Guid } from 'guid-typescript';
import { AdministrationUnitService } from '../administration-unit/shared/administration-unit.service';
import { AdministrationUnitProperty } from '../administration-unit/shared/administration-unit-property';
import { IAdministrationUnit } from '../administration-unit/shared/iadministration-unit';

@Injectable({
  providedIn: 'root'
})
export class AdministrationUnitFeatureService {
  private topic = 'BaseSettings';
  private brokerUrl: string;
  private administrationUnitsFeatures: any [];

  constructor( private http: HttpClient,
               private accountService: AccountService,
               private administrationUnitsService: AdministrationUnitService) {
    this.administrationUnitsFeatures = [
      {
        Title: 'Farbe',
        Description: 'Aussenfarbe',
        Tag: 3,
        TypedValueDecimalPlace: 0,
        TypedValueUnit: null,
        Version: 1,
        Id: 'a9390372-208b-4855-9453-a95000b95267',
        Edit: '0001-01-01T00:00:00'
      },
      {
        Title: 'Wohnfläche',
        Description: 'Wohnfläche',
        Tag: 2,
        TypedValueDecimalPlace: 2,
        TypedValueUnit: 'qm',
        Version: 1,
        Id: 'EBD064EB-A561-404C-9846-E2FDF45E0632',
        Edit: '0001-01-01T00:00:00'
      }
    ];
  }

  listAdministrationUnitFeature(): Observable<Array<IAdministrationUnitFeature>> {
    return of (this.administrationUnitsFeatures);
  }

  getSingleAdministrationUnitFeature (id: string): Observable <IAdministrationUnitFeature> {
    if (id !== '0') {
      const list = new List <any>(this.administrationUnitsFeatures) ;
      const feature = list
        .Where(x  => x.Id === id )
        .FirstOrDefault();
      return of(feature);
    }
  }

  createAdministrationUnitFeature(baseSettings: IAdministrationUnitFeature): Observable<any> {
    const baseSetting = baseSettings;
    baseSetting.Id = Guid.create().toString();
    this.administrationUnitsFeatures.push(baseSetting);
    for (const adminUnit of this.administrationUnitsService.getListNoObservable()) {
      const adminUnitProperty = new AdministrationUnitProperty(
        Guid.create().toString(),
        new Date(),
        1,
        baseSettings.Title,
        baseSettings.Description,
        this.administrationUnitsService.buildPropertyValue(baseSettings)
      );
      adminUnit.AdministrationUnitProperties.push(adminUnitProperty);
    }
    return of([]);
  }

  editAdministrationUnitFeature (baseSettings: IAdministrationUnitFeature): Observable<any> {
    const featureIndex = this.administrationUnitsFeatures.findIndex(item => item.Id === baseSettings.Id);
    this.administrationUnitsFeatures[featureIndex] = baseSettings;
    return of([])
      .pipe(
        catchError(error => observableThrowError(error))
      );
  }
}
