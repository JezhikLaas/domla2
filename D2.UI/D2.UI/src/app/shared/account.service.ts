
import {throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DOCUMENT } from '@angular/common';



import {StorageService} from './storage.service';
import {environment} from '../../environments/environment';
import {catchError, switchMap} from 'rxjs/internal/operators';


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

interface OidcConfiguration {
  issuer: string;
  redirectUri: string;
  clientId: string;
  scope: string;
  silentRefreshRedirectUri: string;
}

export interface ServiceInfo {
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
  private static readonly OidcConfiguration_Url = '/home/loadconfiguration';
  private brokerUrl: string;
  private services: ServiceInfo[];

  constructor(
    private http: HttpClient,
    private storage: StorageService,
    @Inject('API_URL') private api: string,
    @Inject(DOCUMENT) private document: any
  ) { }

  loadOidcConfiguration(): Observable<OidcConfiguration> {
    return this.http.get<OidcConfiguration>(AccountService.OidcConfiguration_Url);
  }

  fetchServices(): Observable<BrokerUrl> {
    return this.http.get<BrokerUrl>(`${this.api}${AccountService.Services_Url}`)
      .pipe(catchError(error => {
        return observableThrowError(error);
      }));
  }

  fetchService(topic: string): Observable<ServiceInfo> {
    if (this.brokerUrl) {
      return this.http.get<ServiceInfo>(`${this.brokerUrl}/apps/Domla2/01/${topic}`);
    } else {
      return this.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http.get<ServiceInfo>(`${this.brokerUrl}/apps/Domla2/01/${topic}`);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }
}
