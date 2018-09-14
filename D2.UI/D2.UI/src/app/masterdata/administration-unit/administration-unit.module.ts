import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import { AdministrationUnitRoutingModule } from './administration-unit-routing.module';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import { SharedModule } from '../../shared/shared.module';
import {
  AdministrationUnitPropertyComponent
} from './administration-unit-property/administration-unit-property.component';
import { AdministrationUnitFeatureModule} from '../administration-unit-feature/administration-unit-feature.module';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';
import {SubunitCreateComponent} from '../subunit/subunit-create/subunit-create.component';
import {SubunitListComponent} from '../subunit/subunit-list/subunit-list.component';
import {SubunitListViewComponent} from '../subunit/subunit-list-view/subunit-list-view.component';
import { EntrancesListComponent } from './administration-unit-edit/entrances-list/entrances-list.component';
import { EntranceEditComponent } from './administration-unit-edit/entrance-edit/entrance-edit.component';

@NgModule({
  imports: [
    AdministrationUnitRoutingModule,
    SharedModule,
    AdministrationUnitFeatureModule
  ],
  entryComponents: [
    EntranceEditComponent, SubunitCreateComponent],
  declarations: [
    AdministrationUnitEditComponent,
    AdministrationUnitPropertyComponent,
    AdministrationUnitsListComponent,
    SubunitCreateComponent,
    SubunitListComponent,
    SubunitListViewComponent,
    EntrancesListComponent,
    EntranceEditComponent
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
})
export class AdministrationUnitModule { }
