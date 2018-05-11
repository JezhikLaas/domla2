import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import {NgModelGroup} from '@angular/forms';

import { AdminunitRoutingModule } from './adminunit-routing.module';
import { AdministrationUnitEditComponent } from './administration-unit-edit/administration-unit-edit.component';
import { AdministrationUnitsListComponent } from './administration-units-list/administration-units-list.component';
import { SharedModule } from '../../shared/shared.module';



@NgModule({
  imports: [
    CommonModule,
    AdminunitRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    SharedModule

  ],
  declarations: [
    AdministrationUnitEditComponent,
    AdministrationUnitsListComponent
  ],

  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AdminunitModule { }
