import { Component, OnInit, HostListener, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MenuItem } from '../../../shared/menu-item';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { AdministrationUnitService } from '../shared/administration-unit.service';
import { AdministrationUnit } from '../shared/administration-unit';
import { AdminUnitFactory } from '../shared/admin-unit-factory';
import { AdministrationUnitValidators } from '../shared/administration-unit.validators';
import {
  AdministrationUnitFormErrorMessages,
  AddressErrorMessages,
  EntranceErrorMessages,
  PropertiesErrorMessages, PropertyValueErrorMessages, SubUnitsErrorMessages
} from './administration-form-error-messages';
import { CountryInfo } from '../../../shared/country-info';
import { DatePipe } from '@angular/common';
import { AddressService } from '../../shared/address.service';
import { DataType } from '../../shared/data-type';
import { AdministrationUnitPropertyValidator } from '../administration-unit-property/administration-unit-property-validator';
import { List } from 'linqts';
import { AdministrationUnitFeatureService } from '../../shared/administration-unit-feature.service';
import { MatTableDataSource } from '@angular/material';
import { AdministrationUnitFeature } from '../../shared/administration-unit-feature';
import { AdministrationUnitFeaturesListViewComponent } from '../../administration-unit-feature/administration-unit-features-list-view/administration-unit-features-list-view.component';
import { SubUnit } from '../../subunit/subunit';
import { SubunitListViewComponent } from '../../subunit/subunit-list-view/subunit-list-view.component';
import { AdministrationUnitSubunit } from '../shared/administration-unit-subunit';


export enum KEY_CODE {
  RIGHT_ARROW = 39,
  LEFT_ARROW = 37
}


@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styleUrls: [ './administration-unit-edit.component.css'],
  providers: [DatePipe]
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => this.submitForm(), () => {
      return this.EditForm.valid && (this.EditForm.touched || this.EditForm.dirty) &&
        (this.SubUnitsArray.valid && (this.SubUnitsArray.touched || this.SubUnitsArray.dirty));
    }),
    new MenuItem('Schließen', () => this.doCancel(), () => true)
  ];
  EditForm: FormGroup;
  IsUpdatingAdminUnit = false;
  AdminUnit = AdminUnitFactory.empty();
  Errors: { [key: string]: string } = {};
  Entrances: FormArray;
  SubUnitsArray: FormArray;
  SubUnits: SubUnit[];
  Properties: FormArray = this.fb.array([]);
  Address: FormGroup;
  Country: FormGroup;
  Value: FormGroup;
  Countries: CountryInfo[];
  DataType;
  ShowPropertiesForAllAdministrationUnits: boolean;
  @ViewChild (AdministrationUnitFeaturesListViewComponent) AdministrationUnitFeatures: AdministrationUnitFeaturesListViewComponent;
  @ViewChild (SubunitListViewComponent) SubUnitList: SubunitListViewComponent;
  private CounterEntrance = 0;

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
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    if (event.keyCode === KEY_CODE.RIGHT_ARROW) {
      this.addEntrancesControl();
    }

    if (event.keyCode === KEY_CODE.LEFT_ARROW) {
      this.removeEntrancesControl(this.Entrances.length - 1);
    }
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
    this.buildEntrancesArray();
    this.buildSubUnitsArray();
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
      Entrances: this.Entrances,
      UnboundSubUnits: this.fb.array(this.AdminUnit.UnboundSubUnits),
      SubUnitsControls: this.SubUnitsArray,
      Id: this.fb.control(this.AdminUnit.Id),
      Version: this.fb.control(this.AdminUnit.Version),
      Edit: this.fb.control(this.AdminUnit.Edit)
    });
    if (this.IsUpdatingAdminUnit && this.AdminUnit.AdministrationUnitProperties && this.AdminUnit.AdministrationUnitProperties.length > 0) {
      this.buildPropertiesArray();
      this.EditForm.addControl('AdministrationUnitProperties', this.Properties);
    }
    this.EditForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.MenuButtons[0].isActive = () => {
      return this.EditForm.valid && (this.EditForm.touched || this.EditForm.dirty) &&
        (this.SubUnitsArray.valid && (this.SubUnitsArray.touched || this.SubUnitsArray.dirty));
    };
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
    this.DataType = DataType;
    this.SubUnits = this.AdminUnit.SubUnits;
  }

  buildEntrancesArray() {
    this.Entrances = this.fb.array(
      this.AdminUnit.Entrances.map(
        t => this.fb.group({
          Title: this.fb.control(t.Title, [ Validators.required, Validators.maxLength(256)]),
          Address: this.Address = this.fb.group(
            {
              City: this.fb.control(t.Address.City, [Validators.required, Validators.maxLength(100)]),
              Street: this.fb.control(t.Address.Street, [Validators.required, Validators.maxLength(150)]),
              Number: this.fb.control(t.Address.Number, [Validators.required, Validators.maxLength(10)]),
              Country: this.Country = this.fb.group(
                {
                  Iso2: this.fb.control(t.Address.Country.Iso2), Name: this.fb.control(t.Address.Country.Name)
                }, { validator: Validators.required}
              ),
              PostalCode: this.fb.control(t.Address.PostalCode, [Validators.required, Validators.maxLength(20)])
            }
          ),
          Id: this.fb.control(t.Id),
          Version: this.fb.control(t.Version),
          Edit: this.fb.control(t.Edit),
          SubUnits: this.fb.array(t.SubUnits),
          InternID: this.fb.control(0)
        } )
      ),
      AdministrationUnitValidators.atLeastOneEntrance
    );
    for (const entrance of this.Entrances.controls) {
      entrance.patchValue(  { 'InternID': this.getCounterEntrance()} ) ;
    }
  }

  getCounterEntrance(): number {
    this.CounterEntrance = this.CounterEntrance + 1;
    return this.CounterEntrance;
  }

  buildPropertiesArray() {
      this.Properties = this.fb.array(
        this.AdminUnit.AdministrationUnitProperties.map(
          t => this.fb.group({
            Title: this.fb.control(t.Title, [Validators.required, Validators.maxLength(256)]),
            Description: this.fb.control(t.Description, [Validators.maxLength(1024)]),
            Value: this.fb.group({
              Tag: this.fb.control(t.Value.Tag),
              Raw: this.fb.control(t.Value.Raw),
              RawNumber: this.fb.group({
                _decimalPlaces: this.fb.control( t.Value.Raw ? t.Value.Raw._decimalPlaces : null),
                _unit: this.fb.control(t.Value.Raw ? t.Value.Raw._unit : null),
                _value: this.fb.control(t.Value.Raw ? t.Value.Raw._value : null)
              })
            }),
            Id: this.fb.control(t.Id),
            Version: this.fb.control(t.Version),
            Edit: this.fb.control(t.Edit)
          }, AdministrationUnitPropertyValidator)
        )
      );
  }

  buildSubUnitsArray() {
    this.SubUnitsArray = this.fb.array([
      this.fb.group({
        Type: this.fb.control(1, [Validators.required]),
        Title: this.fb.control(null, [Validators.required, Validators.maxLength(256)]),
        Number: this.fb.control(null, [Validators.required]),
        Version: this.fb.control(0),
        Id: this.fb.control('00000000-0000-0000-0000-000000000000'),
        Entrance: this.fb.control(null),
        Floor: this.fb.control(null)
      })]
    );
  }

  submitForm() {
    this.addFormValueFromSunUnit();
    if (this.EditForm.value.AdministrationUnitProperties) {
      this.EditForm.value.AdministrationUnitProperties = this.EditForm.value.AdministrationUnitProperties.filter(properties => properties);
    }
    this.EditForm.value.Entrances = this.EditForm.value.Entrances.filter(entrance => entrance);
    const AdminUnit: AdministrationUnit = AdminUnitFactory.toObject(this.EditForm.value);
    AdminUnit.SubUnits = this.AdminUnit.SubUnits;
    if (this.IsUpdatingAdminUnit) {
      this.as.edit(AdminUnit).subscribe(res => {
        this.router.navigate(['../../administrationUnits']);
      });
    } else {
      this.as.create(AdminUnit).subscribe(res => {
        this.router.navigate([`administrationUnits/${res.newId}`]);
        this.as.getSingle(res.newId).subscribe(au => this.afterCreateNewAdministrationUnit(au));
        this.IsUpdatingAdminUnit = true;
        this.buildSubUnitsArray();
      });
    }
  }

  addFormValueFromSunUnit() {
    const unboundSubUnitsArray: FormArray = this.EditForm.get(['UnboundSubUnits']) as FormArray;
    for (const i of this.SubUnitsArray.controls) {
      const subUnitFormGroup: FormGroup = i as FormGroup;
      if (!subUnitFormGroup.controls.Entrance.value) {
        unboundSubUnitsArray.push(i);
      }
      if (subUnitFormGroup.controls.Entrance.value) {
        const entranceValue = subUnitFormGroup.controls.Entrance.value;
        const entries = new List<any>(this.EditForm.value.Entrances);
        const result = entries
          .Where( x => entranceValue.InternID === x.InternID)
          .FirstOrDefault();
        const entranceControl = subUnitFormGroup.get('Entrance');
        const typeControl = subUnitFormGroup.get('Type');
        subUnitFormGroup.removeControl('Entrance');
        subUnitFormGroup.removeControl('Type');
        result.SubUnits.push(subUnitFormGroup.value);
        subUnitFormGroup.addControl('Entrance', entranceControl);
        subUnitFormGroup.addControl('Type', typeControl );
      }
    }
  }

  afterCreateNewAdministrationUnit(adminUnit: any) {
    this.AdminUnit = adminUnit;
    if (this.AdminUnit.SubUnits) {
      this.SubUnits = this.AdminUnit.SubUnits;
      this.SubUnitList.dataSource = new MatTableDataSource<AdministrationUnitSubunit>(this.SubUnits);
    }
  }

  addEntrancesControl() {
    this.Entrances.push(this.fb.group({
      Title: this.fb.control (null, [Validators.required, Validators.maxLength(256)]),
      Address: this.fb.group({
        City: this.fb.control (null, [Validators.required, Validators.maxLength(100)]),
        Street: this.fb.control (null, [Validators.required, Validators.maxLength(150)]),
        Number: this.fb.control (null, [Validators.required, Validators.maxLength(10)]),
        Country:
          this.fb.group({Iso2: 'DE', Name: null}, { validator: Validators.required}),
        PostalCode: this.fb.control (null, [Validators.required, Validators.maxLength(20)])
      }),
      Id: this.fb.control('00000000-0000-0000-0000-000000000000'),
      Version: this.fb.control(0),
      Edit: this.fb.control(''),
      SubUnits: this.fb.array([]),
      InternID: this.fb.control(this.CounterEntrance + 1)
    }));
    this.getCounterEntrance();
  }

  addPropertiesArray() {
    this.addPropertiesControl();
    this.EditForm.addControl('AdministrationUnitProperties', this.Properties);
  }

  addPropertiesControl() {
    this.Properties.push(this.fb.group({
      Title: this.fb.control(null, [Validators.required, Validators.maxLength(256)]),
      Description: this.fb.control(null, [Validators.maxLength(1024)]),
      Value: this.fb.group({
        Tag: this.fb.control(3),
        Raw: this.fb.control(null),
        RawNumber: this.fb.group({
          _decimalPlaces: this.fb.control( 0),
          _unit: this.fb.control(null),
          _value: this.fb.control(null)
        })
      }),
      Id: this.fb.control('00000000-0000-0000-0000-000000000000'),
      Version: this.fb.control(0),
      Edit: this.fb.control(null)
      }, AdministrationUnitPropertyValidator)
    );
  }

  addSubUnitsArrayControl() {
    this.SubUnitsArray.push(this.fb.group({
      Type: this.fb.control(1, [Validators.required]),
      Title: this.fb.control(null, [Validators.required, Validators.maxLength(256)]),
      Number: this.fb.control(null, [Validators.required]),
      Version: this.fb.control(0),
      Id: this.fb.control('00000000-0000-0000-0000-000000000000'),
      Entrance: this.fb.control(null),
      Floor: this.fb.control(null)
      })
    );
  }

  removeEntrancesControl(index: number) {
    this.Entrances.removeAt(index);
    this.EditForm.markAsTouched();
  }

  removePropertiesControl(index: number) {
    this.Properties.removeAt(index);
    if (this.Properties.length === 0 ) {
      this.EditForm.removeControl('AdministrationUnitProperties');
    }
    this.EditForm.markAsTouched();
  }

  removeSubUnitsArryControl(index: number) {
    this.SubUnitsArray.removeAt(index);
    this.EditForm.markAsTouched();
  }

  onShowPropertiesForAllAdministrationUnits() {
    this.ShowPropertiesForAllAdministrationUnits = !this.ShowPropertiesForAllAdministrationUnits;

  }

  refreshProperties() {
    const id = this.AdminUnit.Id;
    this.as.getSingle(id).subscribe(res => {
      this.refreshNewProperty(res.AdministrationUnitProperties);
      this.EditForm.patchValue({'Version': res.Version});
    });
    this.bs.listAdministrationUnitFeature().subscribe(res => this.AdministrationUnitFeatures.dataSource =
      new MatTableDataSource<AdministrationUnitFeature>(res));
  }

  refreshNewProperty(properties: any) {
    if (this.AdminUnit.AdministrationUnitProperties) {
      const propertiesArrayNew = new List<any>(properties);
      const propertyArrayCurrent = new List <any>(this.AdminUnit.AdministrationUnitProperties);
      const propertyNew = propertiesArrayNew
        .Where(x => propertyArrayCurrent.Any(y => y.Id === x.Id) === false)
        .FirstOrDefault();
      this.Properties.push(this.fb.group({
        Title: this.fb.control(propertyNew.Title, [ Validators.required]),
        Description: this.fb.control(propertyNew.Description),
        Value: this.fb.group({
          Tag: this.fb.control(propertyNew.Value.Tag),
          Raw: this.fb.control(propertyNew.Value.Raw),
          RawNumber: this.fb.group({
            _decimalPlaces: this.fb.control( propertyNew.Value.Raw._decimalPlaces),
            _unit: this.fb.control(propertyNew.Value.Raw._unit),
            _value: this.fb.control(propertyNew.Value.Raw._value)
          })
        }),
        Id: this.fb.control(propertyNew.Id),
        Version: this.fb.control(propertyNew.Version),
        Edit: this.fb.control(propertyNew.Edit)
      }, AdministrationUnitPropertyValidator));
    }
    if (!this.EditForm.get(['AdministrationUnitProperties'])) {
      this.EditForm.addControl('AdministrationUnitProperties', this.Properties);
    }
    this.AdminUnit.AdministrationUnitProperties = properties;
  }

  selectedValueTag(event: any, i: number) {
    const valueRawNumberValue =
      this.EditForm.get(['AdministrationUnitProperties', i, 'Value', 'RawNumber', '_value']) as FormControl;
    const valueRawNumberDecimalPlaces =
      this.EditForm.get(['AdministrationUnitProperties', i, 'Value', 'RawNumber', '_decimalPlaces']) as FormControl;
    const valueRawNumberUnit =
      this.EditForm.get(['AdministrationUnitProperties', i, 'Value', 'RawNumber', '_unit']) as FormControl;
    const valueRaw =
      this.EditForm.get(['AdministrationUnitProperties', i, 'Value', 'Raw']) as FormControl;
    if (valueRawNumberValue) { valueRawNumberValue.setValue(null); }
    if (valueRawNumberDecimalPlaces) { valueRawNumberDecimalPlaces.setValue(null); }
    if (valueRawNumberUnit) { valueRawNumberUnit.setValue(null); }
    if (valueRaw) { valueRaw.setValue(null); }
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
    this.updateErrorMessagesEntrance();
    this.updateErrorMessagesAddress();
    this.updateErrorMessagesSubUnit();
    if (this.EditForm.controls.AdministrationUnitProperties) {
      this.updateErrorMessagesProperties();
      this.updateErrorMessagesPropertyValue();
    }
  }

  updateErrorMessagesEntrance() {
    const formArray = this.EditForm.get('Entrances') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of EntranceErrorMessages) {
        const control = this.EditForm.get(['Entrances', i, message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['Entrance' + message.forControl +  message.forValidator]) {
          this.Errors['Entrance' + message.forControl + message.forValidator] = message.text;
        }
      }
    }
  }

  updateErrorMessagesProperties() {
    const formArray = this.EditForm.get('AdministrationUnitProperties') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of PropertiesErrorMessages) {
        const control = this.EditForm.get(['AdministrationUnitProperties', i, message.forControl]);
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
  }

  updateErrorMessagesPropertyValue() {
    const formArray = this.EditForm.get('AdministrationUnitProperties') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of PropertyValueErrorMessages) {
        const control = this.EditForm.get(['AdministrationUnitProperties', i, 'Value', message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['PropertyValue' + message.forControl + message.forValidator]) {
          this.Errors['PropertyValue' + message.forControl + message.forValidator] = message.text;
        }
      }
    }
  }

  updateErrorMessagesAddress() {
    const formArray = this.EditForm.get('Entrances') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of AddressErrorMessages) {
        const control = this.EditForm.get(['Entrances', i, 'Address', message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['Address' + message.forControl + message.forValidator]) {
          this.Errors['Address' + message.forControl + message.forValidator] = message.text;
        }
      }
    }
  }

  updateErrorMessagesSubUnit() {
    const formArray = this.EditForm.get('SubUnitsControls') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of SubUnitsErrorMessages) {
        const control = this.EditForm.get(['SubUnitsControls', i, message.forControl]);
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


