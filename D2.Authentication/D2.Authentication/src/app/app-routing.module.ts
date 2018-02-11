import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';
import { LoggedOutComponent } from './logged-out/logged-out.component';
import { AuthenticationWelcomeComponent} from './authentication-welcome/authentication-welcome.component';
import { AuthenticationErrorComponent } from './authentication-error/authentication-error.component';

const routes: Routes = [
  { path: '', component: AuthenticationWelcomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginUserComponent },
  { path: '_auth/login', component: LoginUserComponent },
  { path: 'logout', component: LogoutUserComponent },
  { path: '_auth/logout', component: LogoutUserComponent },
  { path: 'goodbye', component: LoggedOutComponent },
  { path: '_auth/goodbye', component: LoggedOutComponent },
  { path: 'error', component: AuthenticationErrorComponent },
  { path: '_auth/error', component: AuthenticationErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule { }
