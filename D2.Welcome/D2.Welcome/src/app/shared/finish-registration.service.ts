import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FinishRegistration } from './finish-registration';

@Injectable({
  providedIn: 'root'
})
export class FinishRegistrationService {
  constructor(
    private http: HttpClient
  ) { }

  finishRegistration(info: FinishRegistration) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');

    return this.http.post('/welcome/finish', info, { headers: headers});
  }
}
