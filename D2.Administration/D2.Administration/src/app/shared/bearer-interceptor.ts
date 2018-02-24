import { Inject, Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { StorageService } from './storage.service';
import { DOCUMENT } from '@angular/common';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';

@Injectable()
export class BearerInterceptor implements HttpInterceptor {
  constructor(
    private storage: StorageService,
    @Inject(DOCUMENT) private document: any
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const token = this.storage.get('access_token');
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return next
      .handle(authReq)
      .catch((error, caught) => {
        return Observable.throw(error);
      }) as any;
  }
}
