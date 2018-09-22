import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule, LOCALE_ID } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CdkTableModule } from '@angular/cdk/table';
import { SharedModule } from './shared/shared.module';
import { HomeComponent } from './home/home.component';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { StorageService } from './shared/storage.service';
import { MenuDisplayService } from './shared/menu-display.service';
import { AccountService } from './shared/account.service';
import { CookieService } from 'ngx-cookie-service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { AdministrationUnitService } from './masterdata/administration-unit/shared/administration-unit.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdministrationUnitResolver} from './masterdata/administration-unit/shared/administration-unit-resolver.service';
import { AdministrationUnitsResolver } from './masterdata/administration-unit/shared/administration-units-resolver.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { registerLocaleData } from '@angular/common';
import localeDe from '@angular/common/locales/de';
import localeDeExtra from '@angular/common/locales/extra/de';
import {AddressService} from './masterdata/shared/address.service';
import {AdministrationUnitFeatureResolver} from './masterdata/shared/administration-unit-feature-resolver.service';
import {AdministrationUnitFeaturesResolver} from './masterdata/shared/administration-unit-features-resolver.service';
import {MAT_DIALOG_DEFAULT_OPTIONS} from '@angular/material';
import {AdministrationUnitFeatureService} from './masterdata/shared/administration-unit-feature.service';
import { OAuthModule } from 'angular-oauth2-oidc';

registerLocaleData(localeDe, 'de-DE', localeDeExtra);

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['http', '/home'],
        sendAccessToken: true
      }
    })
  ],
  providers: [
    ErrorDialogComponent,
    ConfirmDialogComponent,
    AccountService,
    StorageService,
    MenuDisplayService,
    CookieService,
    AdministrationUnitsResolver,
    AdministrationUnitResolver,
    AdministrationUnitService,
    AddressService,
    AdministrationUnitFeatureResolver,
    AdministrationUnitFeaturesResolver,
    AdministrationUnitFeatureService,
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false}},
    {
      provide: LOCALE_ID,
      useValue: 'de'
    }
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
})
export class AppModule {
  constructor() {
    registerLocaleData(localeDe);
  }
}
