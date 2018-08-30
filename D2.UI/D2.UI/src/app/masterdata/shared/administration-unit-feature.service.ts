import {throwError as observableThrowError, Observable} from 'rxjs';
import { Injectable } from '@angular/core';
import {IAdministrationUnitRaws} from '../administration-unit/shared/iadministration-unit-raws';
import {map} from 'rxjs/operators';
import {AdminUnitFactory} from '../administration-unit/shared/admin-unit-factory';
import {AccountService} from '../../shared/account.service';
import {HttpClient} from '@angular/common/http';
import {IpostalCodeInfo} from './ipostalcodeinfo';
import {CountryInfo} from '../../shared/country-info';
import {catchError, switchMap} from 'rxjs/internal/operators';
import {IAdministrationUnitFeature} from './IAdministrationUnitFeature';
import {IAdministrationUnit} from '../administration-unit/shared/iadministration-unit';

@Injectable({
  providedIn: 'root'
})
export class AdministrationUnitFeatureService {

  private topic = 'BaseSettings';
  private brokerUrl: string;

  constructor( private http: HttpClient,
               private accountService: AccountService ) { }

  listAdministrationUnitFeature(): Observable<Array<IAdministrationUnitFeature>> {
    if (this.brokerUrl) {
      return this.http
        .get<IAdministrationUnitFeature []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .get<IAdministrationUnitFeature []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  getSingleAdministrationUnitFeature (id: string): Observable <IAdministrationUnitFeature> {
    if (id !== '0') {
      if (this.brokerUrl) {
        return this.http
          .get<IAdministrationUnitFeature>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`);
      } else {
        return this.accountService.fetchServices()
          .pipe(
            switchMap(data => {
              this.brokerUrl = data.Broker;
              return this.http
                .get<IAdministrationUnitFeature>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`);
            }),
            catchError(error => observableThrowError(error))
          );
      }
    }
  }

  createAdministrationUnitFeature(BaseSettings: IAdministrationUnitFeature): Observable<any> {
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

  editAdministrationUnitFeature (BaseSettings: IAdministrationUnitFeature): Observable<any> {
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
