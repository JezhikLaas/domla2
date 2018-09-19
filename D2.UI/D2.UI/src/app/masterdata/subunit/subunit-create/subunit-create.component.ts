import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {UnboundSubUnitType} from '../unbound-subunit-type';
import {Entrance} from '../../../shared/entrance';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { SubUnit } from '../subunit';
import { UnboundSubunit} from '../unbound-subunit';
import { SubUnitsErrorMessages } from '../../administration-unit/administration-unit-edit/administration-form-error-messages';
import { BoundSubunit } from '../boundsubunit';
import { SubUnitValidators } from './subunit.validators';

@Component({
  selector: 'ui-subinit-create',
  templateUrl: './subunit-create.component.html',
  styles: []
})
export class SubunitCreateComponent implements OnInit {
  UnboundSubUnitType =  UnboundSubUnitType;
  SubUnit: FormGroup;
  Errors: { [key: string]: string } = {};

  constructor(
    public dialogRef: MatDialogRef<SubunitCreateComponent>,
    public fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public subUnitData: any
  ) { }

  ngOnInit() {
    this.initSubUnits();
  }

  initSubUnits() {
    this.SubUnit =  this.fb.group({
        Type: this.fb.control(1),
        Title: this.fb.control(this.subUnitData.selectedSubUnit.Title, [Validators.required, Validators.maxLength(256)]),
        Number: this.fb.control(this.subUnitData.selectedSubUnit.Number, [Validators.required]),
        Version: this.fb.control(this.subUnitData.selectedSubUnit.Version),
        Id: this.fb.control(this.subUnitData.selectedSubUnit.Id),
        Entrance: this.fb.control(this.subUnitData.selectedSubUnit.Entrance ? this.subUnitData.selectedSubUnit.Entrance : null),
        Floor: this.fb.control(this.subUnitData.selectedSubUnit.Floor ? this.subUnitData.selectedSubUnit.Floor : null )
      }, { validator: Validators.compose([SubUnitValidators.FloorIfWithEntrance, SubUnitValidators.TypeIfWithEntrance]) });
    this.SubUnit.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  updateErrorMessages() {
    this.Errors = {};
    for (const message of SubUnitsErrorMessages) {
      if (message.forControl === 'SubUnit') {
        const control = this.SubUnit;
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['SubUnit' + message.forValidator]) {
          this.Errors['SubUnit' +  message.forValidator] = message.text;
        }
      }
    }
    this.updateErrorMessagesSubUnit();
  }

  updateErrorMessagesSubUnit() {
    for (const message of SubUnitsErrorMessages) {
      const control = this.SubUnit.get([message.forControl]);
       if (control &&
         control.dirty &&
         control.invalid &&
         control.errors &&
         control.errors[message.forValidator] &&
         !this.Errors['SubUnit' + message.forControl + message.forValidator]) {
         this.Errors['SubUnit' + message.forControl + message.forValidator] = message.text;
       }
    }
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  compareEntrance(e1: Entrance, e2: Entrance): boolean {
    return e1 && e2 ?
            e1.Title === e2.Title &&
            e1.Address.Street === e2.Address.Street &&
            e1.Address.Number && e2.Address.Number &&
            e1.Id === e2.Id :
          e1 === e2;
  }

  onSelectedEntrance(event: any) {
    if (!event) {
       this.SubUnit.get('Floor').patchValue(undefined);
       this.SubUnit.get('Type').setValue(1);
    } else  {
      this.SubUnit.get('Type').patchValue(undefined);
    }
  }
}
