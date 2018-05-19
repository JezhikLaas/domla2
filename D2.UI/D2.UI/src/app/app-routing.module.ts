import { NgModule } from '@angular/core';
import {Routes, RouterModule, PreloadAllModules} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {SharedModule} from './shared/shared.module';


const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'administrationUnits',
    loadChildren: 'app/masterdata/adminunit/adminunit.module#AdminunitModule'
  },
  {
    path: 'datePicker',
    loadChildren: 'app/shared/shared.module#SharedModule'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
