import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/retry';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class AccountService {

  private static readonly Login_Url = 'account/login';
  private static readonly Logout_Url = 'http://localhost:8130/home/logout';

  constructor(
    private http: HttpClient,
  ) { }

  logout(id: string, failed: (message: string) => void) {
    // const token = this.cookieService.get('XSRF-TOKEN');
    // const httpHeaders = (token) ? new HttpHeaders({ 'X-XSRF-TOKEN': token }) : null;

    // const loginData: FormData = new FormData();
    // loginData.append('LogoutId', id);
    this.http.get(AccountService.Logout_Url)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe();
  }
}
