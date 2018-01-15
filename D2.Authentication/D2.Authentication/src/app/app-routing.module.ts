import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';
import { LoggedOutComponent } from './logged-out/logged-out.component';
import { AuthenticationWelcomeComponent} from './authentication-welcome/authentication-welcome.component';
import { AuthenticationErrorComponent } from './authentication-error/authentication-error.component';

const routes: Routes = [
  { path: '', component: AuthenticationWelcomeComponent, pathMatch: 'full' },
  { path: 'app/login', component: LoginUserComponent },
  { path: 'app/logout', component: LogoutUserComponent },
  { path: 'app/goodbye', component: LoggedOutComponent },
  { path: 'app/error', component: AuthenticationErrorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule { }
