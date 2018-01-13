import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserLogin } from './user-login';
import { API_URL } from '../../url';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/retry';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class AccountServiceService {

  private static readonly Login_Url = 'account/login';

  constructor(
    @Inject(API_URL) private api: string,
    private http: HttpClient
  ) { }

  apiUrl(): string {
    if (this.api.slice(-1) === '/') {
      return this.api;
    }
    return this.api + '/';
  }

  login(login: UserLogin, completed: () => void, failed: (message: string) => void) {
    this.http.post(this.apiUrl() + AccountServiceService.Login_Url, login)
      .retry(3)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe(() => completed());
  }
}
