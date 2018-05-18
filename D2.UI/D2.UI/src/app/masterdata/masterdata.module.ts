import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule} from '../shared/shared.module';


@NgModule({
  imports: [
    CommonModule,
    SharedModule
  ],
  declarations: [],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class MasterdataModule { }
