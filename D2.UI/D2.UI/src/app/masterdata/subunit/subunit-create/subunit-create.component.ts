import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {UnboundSubUnitType} from '../unbound-subunit-type';
import {Entrance} from '../../../shared/entrance';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { SubUnit } from '../subunit';
import { UnboundSubunit} from '../unbound-subunit';
import { SubUnitsErrorMessages } from '../../administration-unit/administration-unit-edit/administration-form-error-messages';
import { BoundSubunit } from '../boundsubunit';

@Component({
  selector: 'ui-subinit-create',
  templateUrl: './subunit-create.component.html',
  styles: []
})
export class SubunitCreateComponent implements OnInit {
  UnboundSubUnitType =  UnboundSubUnitType;
  SubUnit: FormGroup;
  // Entrance: FormGroup;
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
        Type: this.fb.control(1, [Validators.required]),
        Title: this.fb.control(this.subUnitData.selectedSubUnit.Title, [Validators.required, Validators.maxLength(256)]),
        Number: this.fb.control(this.subUnitData.selectedSubUnit.Number, [Validators.required]),
        Version: this.fb.control(this.subUnitData.selectedSubUnit.Version),
        Id: this.fb.control(this.subUnitData.Id),
        Entrance: this.fb.control(this.subUnitData.selectedSubUnit.Entrance ? this.subUnitData.selectedSubUnit.Entrance : null),
        Floor: this.fb.control(this.subUnitData.selectedSubUnit.Floor ? this.subUnitData.selectedSubUnit.Floor : null )
      });
    this.SubUnit.statusChanges.subscribe(() => this.updateErrorMessages());
  }

 /*  buildEntrance() {
   this.Entrance =  this.fb.group({
     Title: this.fb.control (this.subUnitData.selectedSubUnit.Entrance.Title),
     Street: this.fb.control(this.subUnitData.selectedSubUnit.Entrance.Address.Street),
     Number: this.fb.control(this.subUnitData.selectedSubUnit.Entrance.Address.Number),
   });
  }
  */

  updateErrorMessages() {
    this.Errors = {};
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
    return e1 && e2 ? e1.Title === e2.Title && e1.Address.Street === e2.Address.Street && e1.Address.Number && e2.Address.Number && e1.Id === e2.Id : e1 === e2;
  }
  onSelectedType(event: any) {
    // this.selectedType.emit(event);
  }

  onSelectedEntrance(event: any) {
    if (!event) {
       this.SubUnit.get('Floor').patchValue(undefined);
    } else  {
      this.SubUnit.get('Type').patchValue(undefined);
    }
    console.log(event);
  }
}
