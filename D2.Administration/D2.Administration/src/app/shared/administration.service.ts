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
  private static readonly Logout_Url = '/home/logout';

  constructor(
    private http: HttpClient,
    private storage: StorageService,
    @Inject('API_URL') private api: string,
    @Inject(DOCUMENT) private document: any
  ) { }

  fetchRegistrations(succeeded: () => void, failed: (message: string) => void): Observable<Array<Registration>> {
    return this.http.get<Registration[]>(`${this.api}${AdministrationService.List_Registrations_Url}`)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      });
  }

  logout(failed: (message: string) => void) {
    this.document.location.href = `${this.api}${AdministrationService.Logout_Url}`;
  }
}
