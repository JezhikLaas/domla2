import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import { AdminunitRoutingModule } from './adminunit-routing.module';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';
import { SharedModule } from '../../shared/shared.module';
import {
  AdministrationUnitPropertyComponent
} from './administration-unit-property/administration-unit-property.component';
import { BaseSettingsModule} from '../basesettings/basesettings.module';


@NgModule({
  imports: [
    AdminunitRoutingModule,
    SharedModule,
    BaseSettingsModule

  ],
  declarations: [
    AdministrationUnitEditComponent,
    AdministrationUnitsListComponent,
    AdministrationUnitPropertyComponent
],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AdminunitModule { }
