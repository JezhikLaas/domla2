import { throwError, Observable } from 'rxjs';
import { Inject, Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { StorageService } from './storage.service';
import { DOCUMENT } from '@angular/common';
import { catchError } from 'rxjs/operators';

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
      .pipe(catchError(error => throwError(error))) as any;
  }
}
