import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';
import { AuthenticationWelcomeComponent } from './authentication-welcome/authentication-welcome.component';
import { AuthenticationErrorComponent } from './authentication-error/authentication-error.component';
import { AccountServiceService } from './shared/account-service.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginUserComponent,
    LogoutUserComponent,
    AuthenticationWelcomeComponent,
    AuthenticationErrorComponent,
    ErrorDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    AccountServiceService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
