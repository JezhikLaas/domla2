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

  private static readonly List_Registrations_Url = 'http://localhost:8133/registrations/list';
  private static readonly Logout_Url = 'http://localhost:8133/home/logout';

  constructor(
    private http: HttpClient,
    private storage: StorageService,
    @Inject(DOCUMENT) private document: any
  ) { }

  fetchRegistrations(succeeded: () => void, failed: (message: string) => void): Observable<Array<Registration>> {
    return this.http.get<Registration[]>(AdministrationService.List_Registrations_Url)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      });
  }

  logout(failed: (message: string) => void) {
    this.http.get<LogoutUrl>(AdministrationService.Logout_Url)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe(
        data => {
          this.document.location.href = data.url;
        }
      );
  }
}
