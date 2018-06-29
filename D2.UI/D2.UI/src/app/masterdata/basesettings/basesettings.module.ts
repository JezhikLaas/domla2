import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';

import { BaseSettingsRoutingModule } from './base-settings-routing.module';
import { BaseSettingsListComponent } from './basesettingslist/base-settings-list.component';
import { BaseSettingEditComponent } from './basesettingedit/base-setting-edit.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  imports: [
    BaseSettingsRoutingModule,
    SharedModule
  ],

  declarations: [
    BaseSettingsListComponent,
    BaseSettingEditComponent
  ],

  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],

  exports: [
    BaseSettingsListComponent,
    BaseSettingEditComponent,
    BaseSettingsRoutingModule
  ]
})
export class BaseSettingsModule { }
