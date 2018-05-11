import { Injectable } from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {AdministrationUnitRaws} from '../adminunit/shared/administration-unit-raws';
import {map} from 'rxjs/operators';
import {AdminUnitFactory} from '../adminunit/shared/admin-unit-factory';
import {AccountService} from '../../shared/account.service';
import {HttpClient} from '@angular/common/http';
import {IpostalCodeInfo} from './ipostalcodeinfo';
import {CountryInfo} from '../../shared/country-info';

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
        .switchMap(data => {
          this.brokerUrl = data.Broker;
          return this.http
            .get<IpostalCodeInfo []>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=List`);
        }).catch(error => Observable.throw(error));
    }
  }
  getCountries (): Observable <Array<CountryInfo>> {
    return this.http.get('./assets/Countries.json')
      .catch(error => Observable.throw(error));
  }

}
