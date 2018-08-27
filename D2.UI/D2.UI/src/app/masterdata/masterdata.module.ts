import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule} from '../shared/shared.module';
import { SubunitCreateComponent } from './subunit/subunit-create/subunit-create.component';
import { SubunitListComponent } from './subunit/subunit-list/subunit-list.component';
import { SubunitListViewComponent } from './subunit/subunit-list-view/subunit-list-view.component';
@NgModule({
  imports: [
    CommonModule,
    SharedModule
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  declarations: []
})
export class MasterdataModule { }
