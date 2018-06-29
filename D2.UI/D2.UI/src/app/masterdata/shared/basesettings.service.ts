import {throwError as observableThrowError, Observable} from 'rxjs';
import { Injectable } from '@angular/core';
import {AdministrationUnitRaws} from '../adminunit/shared/administration-unit-raws';
import {map} from 'rxjs/operators';
import {AdminUnitFactory} from '../adminunit/shared/admin-unit-factory';
import {AccountService} from '../../shared/account.service';
import {HttpClient} from '@angular/common/http';
import {IpostalCodeInfo} from './ipostalcodeinfo';
import {CountryInfo} from '../../shared/country-info';
import {catchError, switchMap} from 'rxjs/internal/operators';
import {IBaseSettings} from './Ibasesettings';
import {IAdministrationUnit} from '../adminunit/shared/iadministration-unit';

@Injectable({
  providedIn: 'root'
})
export class BaseSettingsService {

  private topic = 'BaseSettings';
  private brokerUrl: string;

  constructor( private http: HttpClient,
               private accountService: AccountService ) { }

  listBaseSettings(): Observable<Array<IBaseSettings>> {
    if (this.brokerUrl) {
      return this.http
        .get<IBaseSettings []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .get<IBaseSettings []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  getSingleBaseSetting (id: string): Observable <IBaseSettings> {
    if (id !== '0') {
      if (this.brokerUrl) {
        return this.http
          .get<IBaseSettings>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`);
      } else {
        return this.accountService.fetchServices()
          .pipe(
            switchMap(data => {
              this.brokerUrl = data.Broker;
              return this.http
                .get<IBaseSettings>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`);
            }),
            catchError(error => observableThrowError(error))
          );
      }
    }
  }

  createBaseSettings(BaseSettings: IBaseSettings): Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, BaseSettings);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, BaseSettings);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  editBaseSettings (BaseSettings: IBaseSettings): Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, BaseSettings);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, BaseSettings);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }
}
