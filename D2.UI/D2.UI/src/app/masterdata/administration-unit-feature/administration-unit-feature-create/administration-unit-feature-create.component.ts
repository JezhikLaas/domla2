import {AfterViewChecked, Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {DataType} from '../../shared/data-type';
import {AdministrationUnitFeatureService} from '../../shared/administration-unit-feature.service';
import {AdministrationUnitFeatureCreateErrorMessages} from './administration-unit-feature-create-error-messages';
import {MatDialog, MatSnackBar, MatTableDataSource} from '@angular/material';
import {DialogAdministrationUnitsListComponent} from '../dialog-administration-units-list/dialog-administration-units-list.component';
import {AdministrationUnit} from '../../administration-unit/shared/administration-unit';
import {AdministrationUnitsListViewComponent} from '../../administration-unit/administration-units-list-view/administration-units-list-view.component';
import {AdministrationUnitService} from '../../administration-unit/shared/administration-unit.service';
import {SelectedAdministrationUnitsPropertyParameter} from '../../administration-unit/shared/selected-administration-units-property-parameter';

@Component({
  selector: 'ui-administration-unit-feature-create',
  templateUrl: './administration-unit-feature-create.component.html',
  styles: []
})
export class AdministrationUnitFeatureCreateComponent implements OnInit, AfterViewChecked {
  EditForm: FormGroup;
  AdministrationUnitFeatureGroup: FormGroup;
  DataType = DataType;
  @Output() refreshProperty = new EventEmitter<any>();
  Errors: { [key: string]: string } = {};
  AdministrationUnits: AdministrationUnit [] = [];
  @ViewChild (AdministrationUnitsListViewComponent) AdministrationUnitsListView: AdministrationUnitsListViewComponent;
  Message = String();

  constructor(private fb: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private administrationUnitFeatureService: AdministrationUnitFeatureService,
              private administrationUnitService: AdministrationUnitService,
              public dialog: MatDialog,
              public confirm: MatSnackBar) {
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
    if (this.AdministrationUnits.length > 0 ) {
      const  ids = [];
      this.Message = 'Zusatzfeld wurde den ausgewählten Objekten hinzugefügt.';
      this.AdministrationUnits.forEach((adminUnit) => {
        ids.push(adminUnit.Id);
      });
      let parameter: SelectedAdministrationUnitsPropertyParameter;
      parameter = {
        AdministrationUnitsFeatureParameters: this.AdministrationUnitFeatureGroup.value,
        AdministrationUnitIds: ids
      };
      this.administrationUnitService.addPropertiesSelectedAdministrationUnits(parameter).subscribe(res => this.onRefreshProperty());
    } else {
      this.administrationUnitFeatureService.createAdministrationUnitFeature(this.AdministrationUnitFeatureGroup.value).subscribe(res => this.onRefreshProperty());
      this.Message = 'Zusatzfeld wurden allen Objekte hinzugefügt.';
    }
      this.EditForm.reset();
      this.AdministrationUnitFeatureGroup.controls.Tag.setValue(3);
      this.AdministrationUnitFeatureGroup.controls.TypedValueDecimalPlace.setValue(0);
      this.confirm.open(
        this.Message,
        null,
        {
          duration: 2500,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        },
        );
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
        height: '800px',
        disableClose: false,
        hasBackdrop: true
      });
      dialogRef.afterClosed().subscribe(result => {
        if (result) { this.AdministrationUnits = result; }
      });
    }
    if (selected.value === 'allAdministrationUnits') {
      this.AdministrationUnits = [];
    }
  }

  ngAfterViewChecked() {
    if (this.AdministrationUnitsListView) {
      this.AdministrationUnitsListView.DataSource  = new MatTableDataSource<AdministrationUnit>(this.AdministrationUnits);
      this.AdministrationUnitsListView.DisableSelectRow = true;
    }
  }
}


