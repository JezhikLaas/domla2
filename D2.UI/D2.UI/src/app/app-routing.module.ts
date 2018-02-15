import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitsComponent } from './administration-units/administration-units.component';

const routes: Routes = [
  { path: 'administrationUnits', component: AdministrationUnitsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
