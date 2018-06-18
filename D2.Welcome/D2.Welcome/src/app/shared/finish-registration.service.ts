import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FinishRegistration } from './finish-registration';
import { Observable } from 'rxjs';
import { RegistrationResult } from './registration-result';

@Injectable({
  providedIn: 'root'
})
export class FinishRegistrationService {
  constructor(
    private http: HttpClient
  ) { }

  finishRegistration(info: FinishRegistration): Observable<RegistrationResult> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    return this.http.post<RegistrationResult>('/welcome/finish', info, { headers: headers});
  }
}
