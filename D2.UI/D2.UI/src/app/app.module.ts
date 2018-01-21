import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { LoaderComponent } from './shared/loader/loader.component';
import { AccountService } from './shared/account.service';
import { StorageService } from './shared/storage.service';
import { BearerInterceptor } from './shared/bearer-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ErrorDialogComponent,
    LoaderComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [
    ErrorDialogComponent,
    AccountService,
    StorageService,
    CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BearerInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
