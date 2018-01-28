import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { UserRegistration } from './user-registration';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/retry';

@Injectable()
export class RegisterUserService {
  private registerApi = 'http://localhost:8132/users/register';
  private headers: HttpHeaders = new HttpHeaders();

  constructor(private http: HttpClient) {
    this.headers.append('Content-Type', 'application/json');
  }

  registerUser(
    registration: UserRegistration,
    succeeded: () => void,
    failed: (code: number, message: string) => void
  ) {
    const requestData = JSON.stringify(registration);
    return this.http.put(`${this.registerApi}`, requestData, { headers: this.headers })
      .catch(error => {
        failed(error.status, error.message);
        return Observable.throw(error);
      })
      .subscribe(data =>
        succeeded()
      );
  }
}
