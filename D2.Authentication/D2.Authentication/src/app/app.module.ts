import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';
import { AuthenticationWelcomeComponent } from './authentication-welcome/authentication-welcome.component';
import { AuthenticationErrorComponent } from './authentication-error/authentication-error.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginUserComponent,
    LogoutUserComponent,
    AuthenticationWelcomeComponent,
    AuthenticationErrorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
