import {Component, OnInit, HostListener, Output, AfterViewInit, ViewChild} from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MenuItem } from '../../../shared/menu-item';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { AdministrationUnitService } from '../shared/administration-unit.service';
import { IAdministrationUnit } from '../shared/iadministration-unit';
import { AdminUnitFactory} from '../shared/admin-unit-factory';
import { AdministrationUnitValidators } from '../shared/administration-unit.validators';
import {
  AdministrationUnitFormErrorMessages,
  AddressErrorMessages,
  EntranceErrorMessages,
  PropertiesErrorMessages, PropertyValueErrorMessages
} from './administration-form-error-messages';
import { CountryInfo } from '../../../shared/country-info';
import { DatePipe } from '@angular/common';
import { AddressService } from '../../shared/address.service';
import { YearMonth } from '../../shared/year-month';
import { DataType } from '../../shared/data-type';
import {AdministrationUnitPropertyValue} from '../../../shared/administration-unit-property-value';
import {AdministrationUnitPropertyValidator} from './AdministrationUnitPropertyValidator';
import {BaseSettingsListComponent} from '../../basesettings/basesettingslist/base-settings-list.component';
import {BaseSettingsService} from '../../shared/basesettings.service';
import {IBaseSettings} from '../../shared/Ibasesettings';
import {MatTableDataSource} from '@angular/material';

export enum KEY_CODE {
  RIGHT_ARROW = 39,
  LEFT_ARROW = 37
}


@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styleUrls: [ './administration-unit-edit.component.css'],
  providers: [DatePipe],
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => this.submitForm(), () => {
      if (this.editForm.valid && (this.editForm.touched || this.editForm.dirty) ) {
        return true;
      } else { return false; }
    }),
    new MenuItem('Schließen', () => this.doCancel(), () => true)
  ];
  editForm: FormGroup;
  isUpdatingAdminUnit = false;
  AdminUnit = AdminUnitFactory.empty();
  errors: { [key: string]: string } = {};
  entrances: FormArray;
  properties: FormArray;
  Address: FormGroup;
  Country: FormGroup;
  Value: FormGroup;
  Countries: CountryInfo[];
  CountryDefaultIso2: string;
  PostalCode: string;
  DataType;
  ShowPropertiesForAllAdministrationUnits: boolean;
  PropertiesForSelectedAdministrationUnitsShow: boolean;
  @ViewChild(BaseSettingsListComponent)subject: BaseSettingsListComponent;
  baseSettings: IBaseSettings[];

  constructor(private fb: FormBuilder,
              private menuDisplay: MenuDisplayService,
              private confirmDialog: ConfirmDialogComponent,
              private router: Router,
              private route: ActivatedRoute,
              private datepipe: DatePipe,
              private as: AdministrationUnitService,
              private ads: AddressService,
              private baseSettingsService: BaseSettingsService) {
  }
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    if (event.keyCode === KEY_CODE.RIGHT_ARROW) {
      this.addEntrancesControl();
    }

    if (event.keyCode === KEY_CODE.LEFT_ARROW) {
      this.removeEntrancesControl(this.entrances.length - 1);
    }
  }


  ngOnInit() {
    this.ads.getCountries().subscribe(res =>
      this.Countries = res);
    const id = this.route.snapshot.params ['id'];
    if (id !== '0') {
      this.isUpdatingAdminUnit = true;
      this.AdminUnit = this.route.snapshot.data['AdministrationUnit'];
      for (let i = 0; i < this.AdminUnit.Entrances.length; i++) {
        this.CountryDefaultIso2 = this.AdminUnit.Entrances[i].Address.Country.Iso2;
        this.PostalCode = this.AdminUnit.Entrances[i].Address.PostalCode;
      }
    } else {
              this.CountryDefaultIso2 = 'DE';
              this.PostalCode = '';
            }
    this.initAdminUnit();
  }

  initAdminUnit () {
    this.buildEntrancesArray();
    this.buildPropertiesArray();
    this.editForm = this.fb.group({
      UserKey: this.fb.control(
        this.AdminUnit.UserKey,
        [
          Validators.required
        ]
      ),
      Title: this.fb.control(
        this.AdminUnit.Title,
        [
          Validators.required
        ]
      ),
      YearOfConstruction: this.fb.control(
        this.AdminUnit.YearOfConstruction ?
                    new Date(this.AdminUnit.YearOfConstruction.Year, this.AdminUnit.YearOfConstruction.Month - 1, 1) :
                    this.AdminUnit.YearOfConstruction
      ),
      Entrances: this.entrances
    });
    if (this.isUpdatingAdminUnit) {
      this.addPropertiesArray();
    }
    this.editForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.MenuButtons[0].isActive = () => {
      if (this.editForm.valid && (this.editForm.touched || this.editForm.dirty)) {
        return true;
      } else { return false; }
    };
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
    this.DataType = DataType;
  }

  buildEntrancesArray() {
    this.entrances = this.fb.array(
      this.AdminUnit.Entrances.map(
        t => this.fb.group({
          Title: this.fb.control(t.Title, [ Validators.required]),
          Address: this.Address = this.fb.group(
            {
              City: this.fb.control(t.Address.City, [Validators.required]),
              Street: this.fb.control(t.Address.Street, [Validators.required]),
              Number: this.fb.control(t.Address.Number, [Validators.required]),
              Country: this.Country = this.fb.group(
                {
                  Iso2: this.fb.control(t.Address.Country.Iso2), Name: this.fb.control(t.Address.Country.Name)
                }, { validator: Validators.required}
              ),
              PostalCode: this.fb.control(t.Address.PostalCode, [Validators.required])
            }
          )
        } )
      ),
      AdministrationUnitValidators.atLeastOneEntrance
    );
  }

  addPropertiesArray() {
    this.editForm.addControl('AdministrationUnitProperties', this.properties);
    if (this.properties.length === 0 ) {
      this.addPropertiesControl();
    }
  }

  buildPropertiesArray() {
    this.properties = this.fb.array(
      this.AdminUnit.AdministrationUnitProperties.map(
        t => this.fb.group({
          Title: this.fb.control(t.Title, [Validators.required]),
          Description: this.fb.control((t.Description)),
          Value: this.Value =  this.fb.group(
            {
              Tag: this.fb.control(this.setDefaultTagValueByNewAdminUnit(t), [Validators.required] ),
              Raw: this.fb.control(
                  this.formatRaw(t.Value), [Validators.required]
                )
            }
          )
        }, AdministrationUnitPropertyValidator)
      )
    );
  }

  formatRaw(value: AdministrationUnitPropertyValue) {
    if (value.Tag === '1') {
      return new Date(value.Raw);
    } else { return value.Raw; }
  }

  setDefaultTagValueByNewAdminUnit (property: any) {
    if (property.Title === '') {
      return 3;
    } else { return property.Value.Tag; }
  }

  submitForm() {
    this.editForm.value.Entrances = this.editForm.value.Entrances.filter(entrance => entrance);
    this.editForm.value.AdministrationUnitProperties = this.editForm.value.AdministrationUnitProperties.filter(properties => properties);
    const AdminUnit: IAdministrationUnit = AdminUnitFactory.fromObject(this.editForm.value);
     if (AdminUnit.YearOfConstruction) {
       const date: Date = new Date(AdminUnit.YearOfConstruction.toString());
       const yearMonth: YearMonth = new YearMonth(date.getFullYear(), date.getMonth() + 1);
       AdminUnit.YearOfConstruction = yearMonth;
     }
    const formArray = this.editForm.get('Entrances') as FormArray;
    if (this.isUpdatingAdminUnit) {
      AdminUnit.Id = this.AdminUnit.Id;
      AdminUnit.Edit = this.AdminUnit.Edit;
      AdminUnit.Version = this.AdminUnit.Version;
      for ( let i = 0; i < formArray.length && i < this.AdminUnit.Entrances.length; i++ ) {
        AdminUnit.Entrances[i].Id = this.AdminUnit.Entrances[i].Id;
        AdminUnit.Entrances[i].Edit = this.AdminUnit.Entrances[i].Edit;
        AdminUnit.Entrances[i].Version = this.AdminUnit.Entrances[i].Version;
      }
      for (let i = 0; i < AdminUnit.AdministrationUnitProperties.length; i++) {
        if (AdminUnit.AdministrationUnitProperties[i].Value.Tag === '1'
          &&  typeof AdminUnit.AdministrationUnitProperties[i].Value.Raw === 'string' ) {
          AdminUnit.AdministrationUnitProperties[i].Value.Raw =
            new Date (AdminUnit.AdministrationUnitProperties[i].Value.Raw).toISOString();
        }
      }
      this.as.edit(AdminUnit).subscribe(res => {
        this.router.navigate(['../../administrationUnits']);
      });
    } else {
      this.as.create(AdminUnit).subscribe(res => {
        this.AdminUnit = AdminUnitFactory.empty();
        this.editForm.reset(AdminUnitFactory.empty());
      });
    }
  }
  updateErrorMessages() {
    this.errors = {};
    for (const message of AdministrationUnitFormErrorMessages) {
      const control = this.editForm.get(message.forControl);
      if (control &&
        control.dirty &&
        control.invalid &&
        control.errors &&
        control.errors[message.forValidator] &&
        !this.errors['AdminUnit' + message.forControl]) {
        this.errors['AdminUnit' + message.forControl] = message.text;
      }
    }
    this.updateErrorMessagesEntrance();
    this.updateErrorMessagesAddress();
    if (this.editForm.controls.AdministrationUnitProperties) {
      this.updateErrorMessagesProperties();
      this.updateErrorMessagesPropertyValue();
    }
  }

  updateErrorMessagesEntrance() {
    const formArray = this.editForm.get('Entrances') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of EntranceErrorMessages) {
        const control = this.editForm.get(['Entrances', i, message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.errors['Entrance' + message.forControl]) {
          this.errors['Entrance' + message.forControl] = message.text;
        }
      }
    }
  }

  updateErrorMessagesProperties() {
    const formArray = this.editForm.get('AdministrationUnitProperties') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of PropertiesErrorMessages) {
        const control = this.editForm.get(['AdministrationUnitProperties', i, message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.errors['Property' + message.forControl]) {
          this.errors['Property' + message.forControl] = message.text;
        }
      }
    }
  }

  updateErrorMessagesPropertyValue() {
    const formArray = this.editForm.get('AdministrationUnitProperties') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of PropertyValueErrorMessages) {
        const control = this.editForm.get(['AdministrationUnitProperties', i, 'Value', message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.errors['PropertyValue' + message.forControl]) {
          this.errors['PropertyValue' + message.forControl] = message.text;
        }
      }
    }
  }

  updateErrorMessagesAddress() {
    const formArray = this.editForm.get('Entrances') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      for (const message of AddressErrorMessages) {
        const control = this.editForm.get(['Entrances', i, 'Address', message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.errors['Address' + message.forControl]) {
          this.errors['Address' + message.forControl] = message.text;
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


  addEntrancesControl() {
    this.entrances.push(this.fb.group({
      Title: this.fb.control (null, [Validators.required]),
      Address: this.fb.group({
        City: this.fb.control (null, [Validators.required]),
        Street: this.fb.control (null, [Validators.required]),
        Number: this.fb.control (null, [Validators.required]),
        Country:
          this.fb.group({Iso2: null, Name: null}, { validator: Validators.required}),
        PostalCode: this.fb.control (null, [Validators.required])
      })
    }));
  }

  addPropertiesControl() {
    this.properties.push(this.fb.group({
        Title: this.fb.control(null, [ Validators.required]),
        Description: this.fb.control((null)),
        Value: this.Value =  this.fb.group(
          {
            Tag: this.fb.control(3, [Validators.required]),
            Raw: this.fb.control(null, [Validators.required])
          }
        )
      }, AdministrationUnitPropertyValidator)
    );
  }

  removeEntrancesControl(index: number) {
    this.entrances.removeAt(index);
  }

  removePropertiesControl(index: number) {
    this.properties.removeAt(index);
    if (this.properties.length === 0 ) {
      this.editForm.removeControl('AdministrationUnitProperties');
    }
  }

  onShowPropertiesForAllAdministrationUnits() {
    this.ShowPropertiesForAllAdministrationUnits = true;
    this.baseSettingsService.listBaseSettings().subscribe(bs => this.baseSettings = bs);
    this.subject.dataSource = new MatTableDataSource<IBaseSettings>(this.baseSettings);
  }


  onDataTypeSelected( val: any) {
    // this.Value.patchValue({Tag: val});
  }
}


