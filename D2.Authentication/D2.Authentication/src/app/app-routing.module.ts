import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';
import { AuthenticationWelcomeComponent} from './authentication-welcome/authentication-welcome.component';

const routes: Routes = [
  { path: '', component: AuthenticationWelcomeComponent, pathMatch: 'full' },
  { path: 'app/login', component: LoginUserComponent },
  { path: 'app/logout', component: LogoutUserComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule { }
