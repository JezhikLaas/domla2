import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../../shared/account.service';
import {IAdministrationUnit} from './iadministration-unit';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
import { ServiceInfo } from '../../../shared/account.service';
import {Entrance} from '../../../shared/entrance';
import {AdministrationUnit} from './administration-unit';

@Injectable()
export class AdministrationUnitService {
  private topic = 'administrationunits';

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) { }

  listAdministrationUnits(): Observable<Array<IAdministrationUnit>> {
    return this.accountService.fetchService(this.topic)
      .switchMap(info => {
        const endPoint = info.EndPoints.find(ep => ep.Name === 'list_administrationunits');
        return this.http.get<Array<IAdministrationUnit>>(`${info.BaseUrl}${endPoint.Uri}`);
      });
  }
  getSingle(id: string) {
    return new AdministrationUnit ('123', '123', 'Ramker Weg');
  }
}

