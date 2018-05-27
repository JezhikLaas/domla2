import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { PrivacyStatementComponent } from './privacy-statement/privacy-statement.component';
import { ProvidePasswordComponent } from './provide-password/provide-password.component';
import { LoaderComponent } from './shared/loader/loader.component';

@NgModule({
  declarations: [
    AppComponent,
    ErrorDialogComponent,
    PrivacyStatementComponent,
    ProvidePasswordComponent,
    LoaderComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    ErrorDialogComponent
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
