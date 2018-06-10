import { BrowserModule } from '@angular/platform-browser';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { PrivacyStatementComponent } from './privacy-statement/privacy-statement.component';
import { LoaderComponent } from './shared/loader/loader.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { InfoDialogComponent } from './shared/info-dialog/info-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    ErrorDialogComponent,
    PrivacyStatementComponent,
    LoaderComponent,
    InfoDialogComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot([])
  ],
  providers: [
    ErrorDialogComponent,
    InfoDialogComponent
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
