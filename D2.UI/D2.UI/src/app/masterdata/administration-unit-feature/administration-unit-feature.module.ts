import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdministrationUnitFeatureRoutingModule } from './administration-unit-feature-routing.module';
import { AdministrationUnitFeaturesListComponent } from './administration-unit-features-list/administration-unit-features-list.component';
import { AdministrationUnitFeatureCreateComponent } from './administration-unit-feature-create/administration-unit-feature-create.component';
import { SharedModule} from '../../shared/shared.module';
import { DialogAdministrationUnitsListComponent} from './dialog-administration-units-list/dialog-administration-units-list.component';
import { AdministrationUnitsListComponent} from '../administration-unit/administration-units-list/administration-units-list.component';
import { AdministrationUnitFeaturesListViewComponent } from './administration-unit-features-list-view/administration-unit-features-list-view.component';

@NgModule({
  imports: [
    AdministrationUnitFeatureRoutingModule,
    SharedModule
  ],
  entryComponents: [DialogAdministrationUnitsListComponent],
  declarations: [
    AdministrationUnitFeaturesListComponent,
    AdministrationUnitFeatureCreateComponent,
    DialogAdministrationUnitsListComponent,
    AdministrationUnitFeaturesListViewComponent
  ],

  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],

  exports: [
    AdministrationUnitFeaturesListComponent,
    AdministrationUnitFeatureCreateComponent,
    AdministrationUnitFeatureRoutingModule,
    DialogAdministrationUnitsListComponent,
    AdministrationUnitFeaturesListViewComponent
  ]
})
export class AdministrationUnitFeatureModule { }
