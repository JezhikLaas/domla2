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
    return this.accountService.fetchService('adminstrationunits')
      .subscribe(info => {
        const endPoint = info.endPoints.find(ep => ep.name === 'list_administrationunits');
        this.http.get<AdministrationUnit[]>(`${info.baseUrl}${endPoint.uri}`)
          .subscribe(list => result(list));
      });
  }
}
