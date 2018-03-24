import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import {DomlaResolver} from '../../shared/domla-resolver.service';

const routes: Routes = [
  {
    path: '',
    component: AdministrationUnitsListComponent,
    resolve: {
      AdministrationUnit: DomlaResolver
    }
  },
  { path: ':id', component: AdministrationUnitEditComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminunitRoutingModule { }
