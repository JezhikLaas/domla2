import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import { AdministrationUnitsResolver } from './shared/administration-units-resolver.service';
import { AdministrationUnitResolver } from './shared/administration-unit-resolver.service';
import {AdministrationUnitFeaturesResolver} from '../shared/administration-unit-features-resolver.service';
import {AdministrationUnitsListComponent} from './administration-units-list/administration-units-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdministrationUnitsListComponent,
    resolve: {
      AdministrationUnits: AdministrationUnitsResolver
    }
  },
  {
    path: ':id',
    component: AdministrationUnitEditComponent,
    resolve: {
      AdministrationUnit: AdministrationUnitResolver,
      AdministrationUnitFeatures: AdministrationUnitFeaturesResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminunitRoutingModule { }
