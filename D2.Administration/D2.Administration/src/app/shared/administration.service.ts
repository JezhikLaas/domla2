
import {throwError as observableThrowError,  Observable } from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { StorageService } from './storage.service';
import { Registration } from './registration';

interface LogoutUrl {
  url: string;
}

interface OidcConfiguration {
  issuer: string;
  redirectUri: string;
  clientId: string;
  scope: string;
  silentRefreshRedirectUri: string;
}

@Injectable()
export class AdministrationService {

  private static readonly List_Registrations_Url = '/registrations/list';
  private static readonly Confirm_Registrations_Url = '/registrations/confirm';
  private static readonly Logout_Url = '/home/logout';
  private static readonly OidcConfiguration_Url = '/home/loadconfiguration';

  constructor(
    private http: HttpClient,
    private storage: StorageService,
    @Inject('API_URL') private api: string,
    @Inject(DOCUMENT) private document: any
  ) { }

  loadOidcConfiguration(): Observable<OidcConfiguration> {
    return this.http.get<OidcConfiguration>(AdministrationService.OidcConfiguration_Url);
  }

  fetchRegistrations(): Observable<Array<Registration>> {
    return this.http.get<Registration[]>(`${this.api}${AdministrationService.List_Registrations_Url}`);
  }

  confirmRegistrations(registrationIds: Array<string>): Observable<any> {
    return this.http.post(`${this.api}${AdministrationService.Confirm_Registrations_Url}`, registrationIds);
  }

  logout(failed: (message: string) => void) {
    this.storage.set('access_token', null);
    this.storage.set('refresh_token', null);
    this.document.location.href = `${this.api}${AdministrationService.Logout_Url}`;
  }
}
