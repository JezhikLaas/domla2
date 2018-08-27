import {Component, Inject, Input, OnInit, EventEmitter, Output} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from '@angular/material';
import {FormArray, FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {DataType} from '../../shared/data-type';


@Component({
  selector: 'ui-administration-unit-property',
  templateUrl: './administration-unit-property.component.html',
  styles: []
})
export class AdministrationUnitPropertyComponent implements OnInit {
  @Input() Value: FormGroup;
  @Input() Title: FormControl;
  @Input() Raw: FormControl;
  @Input() RawNumber: FormGroup;
  @Input() RawNumberValue: FormControl;
  @Input() RawNumberUnit: FormControl;
  @Input() RawNumberDecimalPlaces: FormControl;
  @Input() Tag: FormControl;
  @Input() Description: FormControl;
  @Input() ArrayLastElement: number;
  @Output() addPropertiesControl = new EventEmitter<any>();
  @Output() removePropertiesControl = new EventEmitter<any>();
  @Output() selectedValueTag = new EventEmitter<any>();
  DataType = DataType;

  constructor() { }

  ngOnInit() {
  }

  onAddPropertiesControl() {
    this.addPropertiesControl.emit();
  }

  onRemovePropertiesControl () {
    this.removePropertiesControl.emit();
  }

  onSelectedValueTag (event: any) {
    this.selectedValueTag.emit(event);
  }
}

