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
import { BearerInterceptor } from './shared/bearer-interceptor';
import { StorageService } from './shared/storage.service';
import { MenuDisplayService } from './shared/menu-display.service';
import { AccountService } from './shared/account.service';
import { CookieService } from 'ngx-cookie-service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { AdministrationUnitService } from './masterdata/adminunit/shared/administration-unit.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdministrationUnitResolver} from './masterdata/adminunit/shared/administration-unit-resolver.service';
import { AdministrationUnitsResolver } from './masterdata/adminunit/shared/administration-units-resolver.service';
import { NgSelectModule } from '@ng-select/ng-select';


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
    NgSelectModule
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
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BearerInterceptor,
      multi: true
    },
    {
      provide: LOCALE_ID,
      useValue: 'de'
    }
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AppModule { }
