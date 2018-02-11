import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';
import { CookieService } from 'ngx-cookie-service';
import { UserLogin } from './user-login';
import { API_URL } from '../../url';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/retry';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import * as parseUri from 'parse-uri';

interface AuthorizeResult {
  redirectUri: string;
  state: string;
  scope: string;
  identityToken: string;
  accessToken: string;
  accessTokenLifetime: number;
  code: string;
  sessionState: string;
  error: string;
  errorDescritpion: string;
  isError: boolean;
}

@Injectable()
export class AccountServiceService {

  private static readonly Login_Url = '/account/login';
  private static readonly Logout_Url = '/account/logout';

  constructor(
    @Inject(API_URL) private api: string,
    @Inject(DOCUMENT) private document: any,
    private http: HttpClient,
    private cookieService: CookieService
  ) { }

  login(login: UserLogin, completed: () => void, failed: (message: string) => void) {
    const token = this.cookieService.get('XSRF-TOKEN');
    const httpHeaders = (token) ? new HttpHeaders({ 'X-XSRF-TOKEN': token }) : null;

    const loginData: FormData = new FormData();
    loginData.append('Username', login.login);
    loginData.append('Password', login.password);
    loginData.append('ReturnUrl', login.returnUrl);

    this.http.post<AuthorizeResult>(`${this.api}${AccountServiceService.Login_Url}`, loginData, { headers: httpHeaders })
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe(data => {
        if (!data.isError) {
          completed();

          const uri = parseUri(data.redirectUri);
          let target = uri.protocol + '://' + uri.host
          if (uri.port) {
            target = target + ':' + uri.port;
          }
          target = target + '/';

          this.document.location.href = target;
        } else {
          failed(data.error);
        }
      });
  }

  logout(id: string, failed: (message: string) => void) {
    const token = this.cookieService.get('XSRF-TOKEN');
    const httpHeaders = (token) ? new HttpHeaders({ 'X-XSRF-TOKEN': token }) : null;

    const loginData: FormData = new FormData();
    loginData.append('LogoutId', id);
    this.http.post(`${this.api}${AccountServiceService.Logout_Url}`, loginData, { headers: httpHeaders })
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe();
  }
}
