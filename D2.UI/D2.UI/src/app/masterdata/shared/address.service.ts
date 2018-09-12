
import { throwError as observableThrowError, Observable, of } from 'rxjs';
import { Injectable } from '@angular/core';
import {AccountService} from '../../shared/account.service';
import {HttpClient} from '@angular/common/http';
import {PostalCodeInfo} from './postal-code-info';
import {CountryInfo} from '../../shared/country-info';
import {catchError} from 'rxjs/internal/operators';

@Injectable()
export class AddressService {
  private postalCodes: any [];

  constructor( private http: HttpClient,
              private accountService: AccountService ) {
    this.postalCodes =
      [
        {
          Iso2: 'DE',
          PostalCode: '32051',
          City: 'Herford',
          Version: 1,
          Id: '21ca0e73-50ea-4c3f-9df8-a94300d2a26e',
          Edit: '0001-01-01T00:00:00'
        },
        {
          Iso2: 'DE',
          PostalCode: '32602',
          City: 'Vlotho',
          Version: 1,
          Id: '55d8a3ea-104c-447c-becd-a94a00ac6c61',
          Edit: '0001-01-01T00:00:00'
        }
      ];
  }

  listPostalCodeInfo(): Observable<Array<PostalCodeInfo>> {
    return of(this.postalCodes);
  }

  getCountries (): Observable <Array<CountryInfo>> {
    return this.http.get<Array<CountryInfo>>('./assets/Countries.json')
      .pipe(catchError(error => observableThrowError(error)));
  }
}
