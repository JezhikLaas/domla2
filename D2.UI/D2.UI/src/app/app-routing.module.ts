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
    loadChildren: 'app/masterdata/administration-unit/administration-unit.module#AdministrationUnitModule'
  },
  {
    path: 'datePicker',
    loadChildren: 'app/shared/shared.module#SharedModule'
  },
  {
    path: 'baseSettings',
    loadChildren: 'app/masterdata/administration-unit-feature/administration-unit-feature.module#AdministrationUnitFeatureModule'
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
