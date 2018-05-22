import { throwError as observableThrowError,  Observable } from 'rxjs';
import { Inject, Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { UserRegistration } from './user-registration';
import {catchError} from 'rxjs/operators';

@Injectable()
export class RegisterUserService {
  private registerApi = '/users/register';
  private headers: HttpHeaders = new HttpHeaders();

  constructor(
    private http: HttpClient,
    @Inject('API_URL') private api: string,
  ) {
    this.headers.append('Content-Type', 'application/json');
  }

  registerUser(
    registration: UserRegistration,
    succeeded: () => void,
    failed: (code: number, message: string) => void
  ) {
    const requestData = JSON.stringify(registration);
    return this.http.put(`${this.api}${this.registerApi}`, requestData, { headers: this.headers })
      .pipe(catchError(error => {
        failed(error.status, error.message);
        return observableThrowError(error);
      }))
      .subscribe(data =>
        succeeded()
      );
  }
}
