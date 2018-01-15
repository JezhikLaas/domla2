import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';
import { AuthenticationWelcomeComponent } from './authentication-welcome/authentication-welcome.component';
import { AuthenticationErrorComponent } from './authentication-error/authentication-error.component';
import { AccountServiceService } from './shared/account-service.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { LoaderComponent } from './shared/loader/loader.component';
import { LoggedOutComponent } from './logged-out/logged-out.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginUserComponent,
    LogoutUserComponent,
    AuthenticationWelcomeComponent,
    AuthenticationErrorComponent,
    ErrorDialogComponent,
    LoaderComponent,
    LoggedOutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    AccountServiceService,
    ErrorDialogComponent,
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
