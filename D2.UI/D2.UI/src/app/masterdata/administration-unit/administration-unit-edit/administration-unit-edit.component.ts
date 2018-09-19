import { Component, OnInit, HostListener, ViewChild, AfterViewChecked, AfterViewInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MenuItem } from '../../../shared/menu-item';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { AdministrationUnitService } from '../shared/administration-unit.service';
import { AdministrationUnit } from '../shared/administration-unit';
import { AdminUnitFactory } from '../shared/admin-unit-factory';
import {
  AdministrationUnitFormErrorMessages
} from './administration-form-error-messages';
import { CountryInfo } from '../../../shared/country-info';
import { DatePipe } from '@angular/common';
import { AddressService } from '../../shared/address.service';
import { DataType } from '../../shared/data-type';
import { List } from 'linqts';
import { AdministrationUnitFeatureService } from '../../shared/administration-unit-feature.service';
import { MatTableDataSource } from '@angular/material';
import { AdministrationUnitFeature } from '../../shared/administration-unit-feature';
import { AdministrationUnitFeaturesListViewComponent } from '../../administration-unit-feature/administration-unit-features-list-view/administration-unit-features-list-view.component';
import { AdministrationUnitPropertyEditComponent } from '../administration-unit-property/administration-unit-property-edit/administration-unit-property-edit.component';
import { EntranceEditComponent } from './entrance-edit/entrance-edit.component';
import { SubunitCreateComponent } from '../../subunit/subunit-create/subunit-create.component';
import { AdministrationUnitPropertyListComponent } from '../administration-unit-property/administration-unit-property-list/administration-unit-property-list.component';
import { EntrancesListComponent } from './entrances-list/entrances-list.component';
import { SubunitListViewComponent } from '../../subunit/subunit-list-view/subunit-list-view.component';


@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styleUrls: [ './administration-unit-edit.component.css'],
  providers: [DatePipe]
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => this.submitForm(), () => {
      return this.EditForm.valid && (this.EditForm.touched || this.EditForm.dirty || this.AdminUnit.IsModified);
    }),
    new MenuItem('Schließen', () => this.doCancel(), () => true)
  ];
  EditForm: FormGroup;
  IsUpdatingAdminUnit = false;
  AdminUnit = AdminUnitFactory.emptyAdministrationUnit();
  Errors: { [key: string]: string } = {};
  EntrancesFormArray: FormArray;
  Address: FormGroup;
  Country: FormGroup;
  Value: FormGroup;
  Countries: CountryInfo[];
  DataType;
  ShowPropertiesForAllAdministrationUnits: boolean;
  @ViewChild (AdministrationUnitFeaturesListViewComponent) AdministrationUnitFeatures: AdministrationUnitFeaturesListViewComponent;

  constructor(private fb: FormBuilder,
              private menuDisplay: MenuDisplayService,
              private confirmDialog: ConfirmDialogComponent,
              private router: Router,
              private route: ActivatedRoute,
              private datepipe: DatePipe,
              private as: AdministrationUnitService,
              private ads: AddressService,
              private bs: AdministrationUnitFeatureService) {
  }

  ngOnInit() {
    this.ads.getCountries().subscribe(res =>
      this.Countries = res);
    const id = this.route.snapshot.params ['id'];
    if (id !== '0') {
      this.IsUpdatingAdminUnit = true;
      this.AdminUnit = this.route.snapshot.data['AdministrationUnit'];
    }
    this.initAdminUnit();
  }

  initAdminUnit () {
    this.EditForm = this.fb.group({
      UserKey: this.fb.control(
        this.AdminUnit.UserKey,
        [
          Validators.required,
          Validators.maxLength(256)
        ]
      ),
      Title: this.fb.control(
        this.AdminUnit.Title,
        [
          Validators.required,
          Validators.maxLength(256)
        ]
      ),
      YearOfConstruction: this.fb.control(
        this.AdminUnit.YearOfConstruction ?
                    new Date(this.AdminUnit.YearOfConstruction.Year, this.AdminUnit.YearOfConstruction.Month - 1, 1) :
                    this.AdminUnit.YearOfConstruction
      ),
      Id: this.fb.control(this.AdminUnit.Id),
      Version: this.fb.control(this.AdminUnit.Version),
      Edit: this.fb.control(this.AdminUnit.Edit)
    });
    this.EditForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.MenuButtons[0].isActive = () => {
      return this.EditForm.valid && (this.EditForm.touched || this.EditForm.dirty || this.AdminUnit.IsModified);
    };
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
    this.DataType = DataType;
  }

  modifiedChildComponent () {
    this.AdminUnit.IsModified = true;
  }

  submitForm() {
    if (this.EditForm.value.AdministrationUnitProperties) {
      this.EditForm.value.AdministrationUnitProperties = this.EditForm.value.AdministrationUnitProperties.filter(properties => properties);
    }
    this.AdminUnit.Title = this.EditForm.value.Title;
    this.AdminUnit.UserKey = this.EditForm.value.UserKey;
    this.AdminUnit.YearOfConstruction = this.EditForm.value.YearOfConstruction;
    this.divideSubUnitsInBoundAndUnbound(this.AdminUnit.SubUnits);
    const AdminUnit: AdministrationUnit = AdminUnitFactory.toObject(this.AdminUnit);
    if (this.IsUpdatingAdminUnit) {
      this.as.edit(AdminUnit).subscribe(res => {
        this.router.navigate(['../../administrationUnits']);
      });
    } else {
      this.as.create(AdminUnit).subscribe(res => {
        this.router.navigate([`administrationUnits/${res.newId}`]);
        this.as.getSingle(res.newId).subscribe(au => au);
        this.IsUpdatingAdminUnit = true;
      });
    }
  }

  divideSubUnitsInBoundAndUnbound(subUnits: any[]) {
    for (const entrance of this.AdminUnit.Entrances) {
      entrance.SubUnits = [];
    }
    this.AdminUnit.UnboundSubUnits = [];
    for (const subUnit of subUnits) {
      if (!subUnit.Entrance) {
        this.AdminUnit.UnboundSubUnits.push(subUnit);
      } else if (subUnit.Entrance) {
        const entranceValue = subUnit.Entrance;
        const boundSubUnitsEntrances = new List<any>(this.AdminUnit.Entrances);
        const resultEntrances = boundSubUnitsEntrances
          .Where( x => entranceValue.Id === x.Id
                    && entranceValue.Address.City === x.Address.City
                    && entranceValue.Address.PostalCode === x.Address.PostalCode
                    && entranceValue.Title === x.Title
                    && entranceValue.Address.Number === x.Address.Number)
          .FirstOrDefault();
        delete subUnit.Entrance;

        resultEntrances.SubUnits.push(subUnit);
      }
    }
  }

  onShowPropertiesForAllAdministrationUnits() {
    this.ShowPropertiesForAllAdministrationUnits = !this.ShowPropertiesForAllAdministrationUnits;

  }

  refreshProperties() {
    const id = this.AdminUnit.Id;
    this.as.getSingle(id).subscribe(res => {
      this.EditForm.patchValue({'Version': res.Version});
    });
    this.bs.listAdministrationUnitFeature().subscribe(res => this.AdministrationUnitFeatures.DataSource =
      new MatTableDataSource<AdministrationUnitFeature>(res));
  }

  updateErrorMessages() {
    this.Errors = {};
    for (const message of AdministrationUnitFormErrorMessages) {
      const control = this.EditForm.get(message.forControl);
      if (control &&
        control.dirty &&
        control.invalid &&
        control.errors &&
        control.errors[message.forValidator] &&
        !this.Errors['AdminUnit' + message.forControl + message.forValidator]) {
        this.Errors['AdminUnit' + message.forControl + message.forValidator] = message.text;
      }
    }
  }

  doCancel() {
    this.confirmDialog.show(
      'Bestätigung',
      'Möchten Sie wirklich abbrechen?',
      value => {
        if (value) {
          this.router.navigate(['administrationUnits']);
        }
      }
    );
  }
}


