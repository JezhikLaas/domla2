import {AfterViewChecked, Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {DataType} from '../../shared/data-type';
import {AdministrationUnitFeatureService} from '../../shared/administration-unit-feature.service';
import {AdministrationUnitFeatureCreateErrorMessages} from './administration-unit-feature-create-error-messages';
import {MatDialog} from '@angular/material';
import {DialogAdministrationUnitsListComponent} from '../dialog-administration-units-list/dialog-administration-units-list.component';

@Component({
  selector: 'ui-administration-unit-feature-create',
  templateUrl: './administration-unit-feature-create.component.html',
  styles: []
})
export class AdministrationUnitFeatureCreateComponent implements OnInit {
  EditForm: FormGroup;
  AdministrationUnitFeatureGroup: FormGroup;
  DataType = DataType;
  @Output() refreshProperty = new EventEmitter<any>();
  Errors: { [key: string]: string } = {};

  constructor(private fb: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private bsService: AdministrationUnitFeatureService,
              public dialog: MatDialog) {
  }

  ngOnInit() {
    this.buildBaseSettingsGroup();
    this.EditForm = this.fb.group({
      AdministrationUnitFeatures: this.AdministrationUnitFeatureGroup
    });
    this.EditForm.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  buildBaseSettingsGroup() {
    this.AdministrationUnitFeatureGroup = this.fb.group({
      Title: this.fb.control(null, [Validators.required]),
      Description: this.fb.control(null),
      Tag: this.fb.control(3, [Validators.required]),
      TypedValueDecimalPlace: this.fb.control(0),
      TypedValueUnit: this.fb.control(null)
    });
  }

  onAddBaseControl() {
    this.bsService.createAdministrationUnitFeature(this.AdministrationUnitFeatureGroup.value).subscribe(res => this.onRefreshProperty());
    this.EditForm.reset();
    this.AdministrationUnitFeatureGroup.controls.Tag.setValue(3);
    this.AdministrationUnitFeatureGroup.controls.TypedValueDecimalPlace.setValue(0);
  }

  onRefreshProperty() {
    this.refreshProperty.emit();
  }

  updateErrorMessages() {
  this.Errors = {};
  for (const message of AdministrationUnitFeatureCreateErrorMessages) {
    const control = this.EditForm.get(['AdministrationUnitFeatures', message.forControl]);
    if (control &&
      control.dirty &&
      control.invalid &&
      control.errors &&
      control.errors[message.forValidator] &&
      !this.Errors['AdministrationUnitFeatures' + message.forControl]) {
      this.Errors['AdministrationUnitFeatures' + message.forControl] = message.text;
    }
  }
  }

  openDialog (selected: any) {
    if (selected.value === 'selectedAdministrationUnits') {
      const dialogRef = this.dialog.open (DialogAdministrationUnitsListComponent, {
        width: '1000px',
        height: '800px'
      });
    }
  }
}


