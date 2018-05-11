import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../../shared/account.service';
import { IAdministrationUnit } from './iadministration-unit';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitRaws} from './administration-unit-raws';
import { AdminUnitFactory } from './admin-unit-factory';
import { map } from 'rxjs/operators';
import { CountryInfo } from '../../../shared/country-info';

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
        .get<AdministrationUnitRaws []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`)
        .pipe(
          map(rawAdministrationUnits => rawAdministrationUnits
            .map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit)))
        );
    } else {
      return this.accountService.fetchServices()
        .switchMap(data => {
          this.brokerUrl = data.Broker;
          return this.http
            .get<AdministrationUnitRaws []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`)
            .pipe(
              map(rawAdministrationUnits => rawAdministrationUnits
                .map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit)))
            );
        }).catch(error => Observable.throw(error));
    }
  }

  getSingle(id: string): Observable<AdministrationUnit> {
    if (id !== '0') {
      if (this.brokerUrl) {
        return this.http
          .get<AdministrationUnitRaws>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`)
          .pipe(
            map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
          );
      } else {
        return this.accountService.fetchServices()
          .switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .get<AdministrationUnitRaws>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`)
              .pipe(
                map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
              );
          }).catch(error => Observable.throw(error));
      }
    }
  }
  create(AdminUnit: IAdministrationUnit): Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
    } else {
      return this.accountService.fetchServices()
        .switchMap(data => {
          this.brokerUrl = data.Broker;
          return this.http
            .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
        }).catch(error => Observable.throw(error));
    }
  }

  edit (AdminUnit: IAdministrationUnit): Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, AdminUnit);
    } else {
      return this.accountService.fetchServices()
        .switchMap(data => {
          this.brokerUrl = data.Broker;
          return this.http
            .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, AdminUnit);
        }).catch(error => Observable.throw(error));
    }
  }
}

