import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MenuItem } from '../../../shared/menu-item';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { AdministrationUnitService } from '../shared/administration-unit.service';
import { IAdministrationUnit } from '../shared/iadministration-unit';
import { AdminUnitFactory} from '../shared/admin-unit-factory';
import { Entrance } from '../../../shared/entrance';
import { forEach } from '@angular/router/src/utils/collection';
import { AdministrationUnitValidators} from '../shared/administration-unit.validators';
import {AdministrationUnitFormErrorMessages, AddressErrorMessages, EntranceErrorMessages} from './administration-form-error-messages';

@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styles: []
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => this.submitForm(), () => {
        if (this.editForm.valid && this.editForm.touched ) {
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
  Address: FormGroup;
  Country: FormGroup;

  constructor(private fb: FormBuilder,
              private menuDisplay: MenuDisplayService,
              private confirmDialog: ConfirmDialogComponent,
              private router: Router,
              private route: ActivatedRoute,
              private AUdata: AdministrationUnitService) {
  }

  ngOnInit() {
    const id = this.route.snapshot.params ['id'];
    if (id !== '0') {
      this.isUpdatingAdminUnit = true;
      this.AdminUnit = this.route.snapshot.data['AdministrationUnit'];
    }
    this.initAdminUnit();
  }

  initAdminUnit () {
    this.buildEntrancesArray();
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
      Entrances: this.entrances
    });
    this.editForm.statusChanges.subscribe(() => this.updateErrorMessages());
    this.MenuButtons[0].isActive = () => {
      if (this.editForm.valid && this.editForm.touched ) {
        return true;
      } else { return false; }
    };
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
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
                  Iso2: this.fb.control(t.Address.Country.Iso2),
                  Iso3: this.fb.control(t.Address.Country.Iso3),
                  Name: this.fb.control(t.Address.Country.Name)
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

  submitForm() {
    this.editForm.value.Entrances = this.editForm.value.Entrances.filter(entrance => entrance);
    const AdminUnit: IAdministrationUnit = AdminUnitFactory.fromObject(this.editForm.value);
    if (this.isUpdatingAdminUnit) {
      AdminUnit.Id = this.AdminUnit.Id;
      AdminUnit.Edit = this.AdminUnit.Edit;
      AdminUnit.Version = this.AdminUnit.Version;
      for ( let i = 0; i < this.AdminUnit.Entrances.length; i++ ) {
        AdminUnit.Entrances[i].Id = this.AdminUnit.Entrances[i].Id;
        AdminUnit.Entrances[i].Edit = this.AdminUnit.Entrances[i].Edit;
        AdminUnit.Entrances[i].Version = this.AdminUnit.Entrances[i].Version;
      }
      this.AUdata.edit(AdminUnit).subscribe(res => {
        this.router.navigate(['../../administrationUnits']);
      });
    } else {
      this.AUdata.create(AdminUnit).subscribe(res => {
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
  }

  updateErrorMessagesEntrance() {
    const formArray = this.editForm.get('Entrances') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      const entrance = formArray.controls[i] as FormGroup;
      const address = entrance.controls.Address as FormGroup;
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

  updateErrorMessagesAddress() {
    const formArray = this.editForm.get('Entrances') as FormArray;
    for (let i = 0; i < formArray.length; i++) {
      const entrance = formArray.controls[i] as FormGroup;
      const address = entrance.controls.Address as FormGroup;
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
          this.fb.group({Iso2: null, Iso3: null, Name: null}, { validator: Validators.required}),
        PostalCode: this.fb.control (null, [Validators.required])
      })
    }));
    // this.editForm.statusChanges.subscribe(() => this.updateErrorMessagesAddress());
  }

  removeEntrancesControl(index: number) {
    this.entrances.removeAt(index);
  }
}
