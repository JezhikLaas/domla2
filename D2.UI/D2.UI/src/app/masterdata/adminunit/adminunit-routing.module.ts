import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import { AdministrationUnitsResolver } from './shared/administration-units-resolver.service';
import { AdministrationUnitResolver } from './shared/administration-unit-resolver.service';

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
    AdministrationUnit: AdministrationUnitResolver
  }}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminunitRoutingModule { }
