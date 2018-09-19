import {Component, Inject, Input, OnInit, EventEmitter, Output} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from '@angular/material';
import {FormArray, FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {DataType} from '../../../shared/data-type';
import { AdministrationUnitPropertyValidator } from '../administration-unit-property-validator';
import { PropertiesErrorMessages, PropertyValueErrorMessages } from '../../administration-unit-edit/administration-form-error-messages';


@Component({
  selector: 'ui-administration-unit-property-edit',
  templateUrl: './administration-unit-property-edit.component.html',
  styles: []
})
export class AdministrationUnitPropertyEditComponent implements OnInit {
  @Output() selectedValueTag = new EventEmitter<any>();
  DataType = DataType;
  AdministrationUnitProperty: FormGroup;
  Value: FormGroup;
  Errors: { [key: string]: string } = {};

  constructor(
    public dialogRef: MatDialogRef<AdministrationUnitPropertyEditComponent>,
    public fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public adminUnitPropertyData: any
  ) { }

  ngOnInit() {
    this.initProperties();
    this.AdministrationUnitProperty.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  initProperties() {
    this.buildValue();
    this.AdministrationUnitProperty = this.fb.group({
          Title: this.fb.control(this.adminUnitPropertyData.Title, [Validators.required, Validators.maxLength(256)]),
          Description: this.fb.control(this.adminUnitPropertyData.Description, [Validators.maxLength(1024)]),
          Value: this.Value,
          Id: this.fb.control(this.adminUnitPropertyData.Id),
        }, );
  }

  buildValue () {
    this.Value =
      this.fb.group({
        Tag: this.fb.control(this.adminUnitPropertyData.Value.Tag),
        Raw: this.fb.control(this.checkValueRaw(this.adminUnitPropertyData.Value)),
        RawNumber: this.fb.group({
          _decimalPlaces: this.fb.control( this.adminUnitPropertyData.Value.Raw ? this.adminUnitPropertyData.Value.Raw._decimalPlaces : null),
          _unit: this.fb.control(this.adminUnitPropertyData.Value.Raw ? this.adminUnitPropertyData.Value.Raw._unit : null),
          _value: this.fb.control(this.adminUnitPropertyData.Value.Raw ? this.adminUnitPropertyData.Value.Raw._value : null)
        })
      }, {validator: AdministrationUnitPropertyValidator.checkPropertyValue });
  }

  checkValueRaw (value: any): any {
    let raw_value = value.Raw;
    if (value.Raw && value.Tag === 1) {
      raw_value = new Date(value.Raw._value);
    } else if (value.Raw) {
        raw_value = value.Raw._value;
    } else {
      raw_value = value.Raw;
    }
    return raw_value;
  }

  onSelectedValueTag (event: any) {
    this.Value.get('Raw').reset();
    this.Value.get('RawNumber').reset();
  }

  updateErrorMessages() {
    this.Errors = {};
    this.updateErrorMessagesPropertyValue();
    this.updateErrorMessagesProperties();
  }

  updateErrorMessagesProperties() {
      for (const message of PropertiesErrorMessages) {
        const control = this.AdministrationUnitProperty.get([message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['Property' + message.forControl + message.forValidator]) {
          this.Errors['Property' + message.forControl + message.forValidator] = message.text;
        }
      }
  }

  updateErrorMessagesPropertyValue() {
      for (const message of PropertyValueErrorMessages) {
        if (message.forControl === 'Value') {
          const control = this.Value;
          if (control &&
            control.dirty &&
            control.invalid &&
            control.errors &&
            control.errors[message.forValidator] &&
            !this.Errors['PropertyValue' + message.forValidator]) {
            this.Errors['PropertyValue' + message.forValidator] = message.text;
          }
        }
      }
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}

