import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdministrationUnitFeaturesListComponent } from './administration-unit-features-list/administration-unit-features-list.component';
import { AdministrationUnitFeaturesResolver } from '../shared/administration-unit-features-resolver.service';
import { AdministrationUnitFeatureCreateComponent } from './administration-unit-feature-create/administration-unit-feature-create.component';
import { AdministrationUnitFeatureResolver } from '../shared/administration-unit-feature-resolver.service';
import {AdministrationUnitsResolver} from '../administration-unit/shared/administration-units-resolver.service';
import {DialogAdministrationUnitsListComponent} from './dialog-administration-units-list/dialog-administration-units-list.component';

const routes: Routes = [
  {
    path: '',
    component: AdministrationUnitFeaturesListComponent,
    resolve: {
      AdministrationUnitFeatures: AdministrationUnitFeaturesResolver
    }
  },
  {
    path: 'id',
    component: AdministrationUnitFeatureCreateComponent,
    resolve: {
      AdministrationUnitFeature : AdministrationUnitFeatureResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdministrationUnitFeatureRoutingModule { }
