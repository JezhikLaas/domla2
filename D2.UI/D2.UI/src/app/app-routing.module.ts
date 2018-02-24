import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';

const routes: Routes = [
  { path: 'administrationUnits', component: AdministrationUnitsListComponent },
  { path: 'editAdministrationUnit/:id', component: AdministrationUnitEditComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
