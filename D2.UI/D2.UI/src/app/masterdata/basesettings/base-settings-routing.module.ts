import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BaseSettingsListComponent } from './basesettingslist/base-settings-list.component';
import { BaseSettingsResolver } from '../shared/base-settings-resolver.service';
import { BaseSettingEditComponent } from './basesettingedit/base-setting-edit.component';
import { BaseSettingResolver } from '../shared/base-setting-resolver.service';

const routes: Routes = [
  {
    path: '',
    component: BaseSettingsListComponent,
    resolve: {
      BaseSettings: BaseSettingsResolver
    }
  },
  {
    path: 'id',
    component: BaseSettingEditComponent,
    resolve: {
      BaseSetting : BaseSettingResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BaseSettingsRoutingModule { }
