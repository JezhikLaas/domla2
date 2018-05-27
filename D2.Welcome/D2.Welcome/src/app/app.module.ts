import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { PrivacyStatementComponent } from './privacy-statement/privacy-statement.component';
import { ProvidePasswordComponent } from './provide-password/provide-password.component';

@NgModule({
  declarations: [
    AppComponent,
    ErrorDialogComponent,
    PrivacyStatementComponent,
    ProvidePasswordComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [
    ErrorDialogComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
