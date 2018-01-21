import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import {StorageService} from './storage.service';


@Injectable()
export class BearerInterceptor implements HttpInterceptor {
  constructor(
    private storage: StorageService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const token = this.storage.get('access_token');
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return next.handle(authReq)
      .catch((error, caught) => {
        return Observable.throw(error);
      }) as any;
  }
}
