import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginUserComponent } from './login-user/login-user.component';
import { LogoutUserComponent } from './logout-user/logout-user.component';

const routes: Routes = [
  { path: '', component: LoginUserComponent, pathMatch: 'full' },
  { path: 'app/login', component: LoginUserComponent },
  { path: 'app/logout', component: LogoutUserComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: []
})

export class AppRoutingModule { }
