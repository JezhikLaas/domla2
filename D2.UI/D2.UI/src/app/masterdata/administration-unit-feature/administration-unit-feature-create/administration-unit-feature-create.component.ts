import {AfterViewChecked, Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {IBaseSetting} from '../../shared/ibasesetting';
import {DataType} from '../../shared/data-type';
import {BaseSettingsService} from '../../shared/basesettings.service';
import {BaseSettingsErrorMessages} from './base-settings-error-messages';

@Component({
  selector: 'ui-base-setting-edit',
  templateUrl: './base-setting-edit.component.html',
  styles: []
})
export class BaseSettingEditComponent implements OnInit {
  EditForm: FormGroup;
  BaseSettingsGroup: FormGroup;
  DataType = DataType;
  @Output() refreshProperty = new EventEmitter<any>();
  Errors: { [key: string]: string } = {};

  constructor(private fb: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private bsService: BaseSettingsService) {
  }

  ngOnInit() {
    this.buildBaseSettingsGroup();
    this.EditForm = this.fb.group({
      BaseSettings: this.BaseSettingsGroup
    });
    this.EditForm.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  buildBaseSettingsGroup() {
    this.BaseSettingsGroup = this.fb.group({
      Title: this.fb.control(null, [Validators.required]),
      Description: this.fb.control(null),
      Tag: this.fb.control(3, [Validators.required]),
      TypedValueDecimalPlace: this.fb.control(0),
      TypedValueUnit: this.fb.control(null)
    });
  }

  onAddBaseControl() {
    this.bsService.createBaseSettings(this.BaseSettingsGroup.value).subscribe(res => this.onRefreshProperty());
    this.EditForm.reset();
    this.BaseSettingsGroup.controls.Tag.setValue(3);
    this.BaseSettingsGroup.controls.TypedValueDecimalPlace.setValue(0);
  }

  onRefreshProperty() {
    this.refreshProperty.emit();
  }

  updateErrorMessages() {
  this.Errors = {};
  for (const message of BaseSettingsErrorMessages) {
    const control = this.EditForm.get(['BaseSettings', message.forControl]);
    if (control &&
      control.dirty &&
      control.invalid &&
      control.errors &&
      control.errors[message.forValidator] &&
      !this.Errors['BaseSettings' + message.forControl]) {
      this.Errors['BaseSettings' + message.forControl] = message.text;
    }
  }
  }
}

