import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import { AdminunitRoutingModule } from './adminunit-routing.module';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import { SharedModule } from '../../shared/shared.module';
import {
  AdministrationUnitPropertyComponent
} from './administration-unit-property/administration-unit-property.component';
import { AdministrationUnitFeatureModule} from '../administration-unit-feature/administration-unit-feature.module';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';


@NgModule({
  imports: [
    AdminunitRoutingModule,
    SharedModule,
    AdministrationUnitFeatureModule

  ],
  declarations: [
    AdministrationUnitEditComponent,
    AdministrationUnitPropertyComponent,
    AdministrationUnitsListComponent
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
})
export class AdminunitModule { }
