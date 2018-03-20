import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './account.service';
import {AdministrationUnit} from './administration-unit';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import { ServiceInfo } from './account.service';

@Injectable()
export class AdministrationUnitService {
  private topic = 'administrationunits';

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) { }

  listAdministrationUnits(): Observable<Array<AdministrationUnit>> {
    return this.accountService.fetchService(this.topic)
      .switchMap(info => {
        const endPoint = info.EndPoints.find(ep => ep.Name === 'list_administrationunits');
        return this.http.get<Array<AdministrationUnit>>(`${info.BaseUrl}${endPoint.Uri}`);
      });
  }
}
