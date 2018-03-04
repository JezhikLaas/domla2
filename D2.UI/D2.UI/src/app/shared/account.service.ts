import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { DOCUMENT } from '@angular/common';
import 'rxjs/add/operator/retry';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import {StorageService} from './storage.service';

interface LogoutUrl {
  url: string;
}

interface BrokerUrl {
  Broker: string;
}

interface EndPointInfo {
  Name: string;
  Uri: string;
}

interface ServiceInfo {
  Name: string;
  BaseUrl: string;
  Version: number;
  Patch: number;
  EndPoints: EndPointInfo[];
}

@Injectable()
export class AccountService {

  private static readonly Logout_Url = '/home/logout';
  private static readonly Services_Url = '/home/services';
  private brokerUrl: string;
  private services: ServiceInfo[];

  constructor(
    private http: HttpClient,
    private storage: StorageService,
    @Inject('API_URL') private api: string,
    @Inject(DOCUMENT) private document: any
  ) { }

  fetchServices(succeeded: () => void, failed: (message: string) => void) {
    this.http.get<BrokerUrl>(`${this.api}${AccountService.Services_Url}`)
      .catch(error => {
        failed(error.message);
        return Observable.throw(error);
      })
      .subscribe(
        data => {
          console.log('received: ' + data);
          this.brokerUrl = data.Broker;
          this.http.get<ServiceInfo>(`${this.brokerUrl}/apps/Domla2/01/`)
            .catch(error => {
              failed(error.message);
              return Observable.throw(error);
            })
            .subscribe(answer => {
              this.services = answer;
              succeeded();
            });
        }
      );
  }

  fetchService(topic: string): Observable<ServiceInfo> {
    return this.http.get<ServiceInfo>(`${this.brokerUrl}/apps/Domla2/01/${topic}`)
      .catch(error => {
        return Observable.throw(error);
      })
      ;
  }

  logout(id: string, failed: (message: string) => void) {
    this.storage.set('access_token', null);
    this.storage.set('refresh_token', null);
    this.document.location.href = `${this.api}${AccountService.Logout_Url}`;
  }
}
