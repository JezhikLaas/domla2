
import {throwError as observableThrowError, Observable} from 'rxjs';
import { Injectable } from '@angular/core';
import {AccountService} from '../../shared/account.service';
import {HttpClient} from '@angular/common/http';
import {PostalCodeInfo} from './postal-code-info';
import {CountryInfo} from '../../shared/country-info';
import {catchError, switchMap} from 'rxjs/internal/operators';

@Injectable()
export class AddressService {

  private topic = 'PostalCodeInfo';
  private brokerUrl: string;

  constructor( private http: HttpClient,
              private accountService: AccountService ) { }

  listPostalCodeInfo(): Observable<Array<PostalCodeInfo>> {
    if (this.brokerUrl) {
      return this.http
        .get<PostalCodeInfo []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .get<PostalCodeInfo []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
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
