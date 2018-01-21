import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { LoaderComponent } from './shared/loader/loader.component';
import { AccountService } from './shared/account.service';
import { StorageService } from './shared/storage.service';

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
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
