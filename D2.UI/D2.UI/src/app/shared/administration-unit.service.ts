import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './account.service';
import {AdministrationUnit} from './administration-unit';

@Injectable()
export class AdministrationUnitService {

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) { }

  listAdministrationUnits(result: (units: AdministrationUnit[]) => void) {
    this.accountService.fetchService('administrationunits')
      .subscribe(info => {
        const endPoint = info.EndPoints.find(ep => ep.Name === 'list_administrationunits');
        this.http.get<AdministrationUnit[]>(`${info.BaseUrl}${endPoint.Uri}`)
          .subscribe(list => result(list));
      });
  }
}
