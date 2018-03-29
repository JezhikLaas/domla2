import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../../shared/account.service';
import { IAdministrationUnit } from './iadministration-unit';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import { ServiceInfo } from '../../../shared/account.service';
import {Entrance} from '../../../shared/entrance';
import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitRaws} from './administration-unit-raws';
import { AdminUnitFactory } from './admin-unit-factory';
import { map } from 'rxjs/operators';

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
    return this.accountService.fetchServices()
      .switchMap(data => {
        this.brokerUrl = data.Broker;
        return this.http
          .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
      }).catch(error => Observable.throw(error));
  }
}

