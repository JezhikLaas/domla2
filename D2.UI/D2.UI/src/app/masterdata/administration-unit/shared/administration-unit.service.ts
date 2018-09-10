import { throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../../shared/account.service';
import { AdministrationUnit } from './administration-unit';
import { IAdministrationUnitRaws} from './iadministration-unit-raws';
import { AdminUnitFactory } from './admin-unit-factory';
import { map, switchMap, catchError } from 'rxjs/operators';
import { SelectedAdministrationUnitsPropertyParameter } from './selected-administration-units-property-parameter';

@Injectable()
export class AdministrationUnitService {
  private topic = 'AdministrationUnit';
  private brokerUrl: string;

  constructor(private http: HttpClient,
              private accountService: AccountService) {
  }

  listAdministrationUnits(): Observable<Array<AdministrationUnit>> {
    if (this.brokerUrl) {
      return this.http
        .get<IAdministrationUnitRaws []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`)
        .pipe(
          map(rawAdministrationUnits => rawAdministrationUnits
            .map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit)))
        );
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .get<IAdministrationUnitRaws []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`)
              .pipe(
                map(rawAdministrationUnits => rawAdministrationUnits
                  .map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit)))
              );
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  getSingle(id: string): Observable<AdministrationUnit> {
    if (id !== '0') {
      if (this.brokerUrl) {
        return this.http
          .get<IAdministrationUnitRaws>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`)
          .pipe(
            map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
          );
      } else {
        return this.accountService.fetchServices()
          .pipe(
            switchMap(data => {
              this.brokerUrl = data.Broker;
              return this.http
                .get<IAdministrationUnitRaws>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`)
                .pipe(
                  map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
                );
            }),
            catchError(error => observableThrowError(error))
          );
      }
    }
  }
  create(AdminUnit: AdministrationUnit): Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  edit (AdminUnit: AdministrationUnit): Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, AdminUnit);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, AdminUnit);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  addPropertiesSelectedAdministrationUnits(selectedAdministrationUnitsPropertyParameter: SelectedAdministrationUnitsPropertyParameter) {
    if (this.brokerUrl) {
      return this.http
        .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=AddProperty`, selectedAdministrationUnitsPropertyParameter);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=AddProperty`, selectedAdministrationUnitsPropertyParameter);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }
}

