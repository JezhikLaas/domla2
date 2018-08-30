
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

@Injectable()
export class AddressService {

  private topic = 'PostalCodeInfo';
  private brokerUrl: string;

  constructor( private http: HttpClient,
              private accountService: AccountService ) { }

  listPostalCodeInfo(): Observable<Array<IpostalCodeInfo>> {
    if (this.brokerUrl) {
      return this.http
        .get<IpostalCodeInfo []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .get<IpostalCodeInfo []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  getCountries (): Observable <Array<CountryInfo>> {
    return this.http.get<Array<CountryInfo>>('./assets/Countries.json')
      .pipe(catchError(error => observableThrowError(error)));
  }
}
