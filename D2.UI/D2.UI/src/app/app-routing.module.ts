import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';

const routes: Routes = [
  { path: 'administrationUnits', component: AdministrationUnitsListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
