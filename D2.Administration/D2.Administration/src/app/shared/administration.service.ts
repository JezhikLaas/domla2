import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { StorageService } from './storage.service';
import { Registration } from './registration';
import { Observable } from 'rxjs/Observable';

interface LogoutUrl {
  url: string;
}

@Injectable()
export class AdministrationService {

  private static readonly List_Registrations_Url = '/registrations/list';
  private static readonly Confirm_Registrations_Url = '/registrations/confirm';
  private static readonly Logout_Url = '/home/logout';

  constructor(
    private http: HttpClient,
    private storage: StorageService,
    @Inject('API_URL') private api: string,
    @Inject(DOCUMENT) private document: any
  ) { }

  fetchRegistrations(): Observable<Array<Registration>> {
    return this.http.get<Registration[]>(`${this.api}${AdministrationService.List_Registrations_Url}`);
  }

  confirmRegistrations(registrationIds: Array<string>): Observable<Array<Registration>> {
    return this.http.post(`${this.api}${AdministrationService.Confirm_Registrations_Url}`, registrationIds)
      .catch(error => {
        return Observable.throw(error);
      });
  }

  logout(failed: (message: string) => void) {
    this.storage.set('access_token', null);
    this.storage.set('refresh_token', null);
    this.document.location.href = `${this.api}${AdministrationService.Logout_Url}`;
  }
}
