import { Component, OnInit } from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import { MenuItem } from '../../../shared/menu-item';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import {ActivatedRoute, Router} from '@angular/router';
import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { AdministrationUnitService } from '../shared/administration-unit.service';
import { IAdministrationUnit } from '../shared/iadministration-unit';
import { AdminUnitFactory} from '../shared/admin-unit-factory';
import {Entrance} from '../../../shared/entrance';
import {forEach} from '@angular/router/src/utils/collection';

@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styles: []
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => this.submitForm(), () => true),
    new MenuItem('Schließen', () => this.doCancel(), () => true)
  ];
  editForm: FormGroup;
  isUpdatingAdminUnit = false;
  AdminUnit = AdminUnitFactory.empty();
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
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
  }

  buildEntrancesArray() {
    this.entrances = this.fb.array(
      this.AdminUnit.Entrances.map(
        t => this.fb.group({
          Title: this.fb.control(t.Title),
          Address: this.Address = this.fb.group(
            {
              City: this.fb.control(t.Address.City),
              Street: this.fb.control(t.Address.Street),
              Number: this.fb.control(t.Address.Number),
              Country: this.Country = this.fb.group(
                {
                  Iso2: this.fb.control(t.Address.Country.Iso2),
                  Iso3: this.fb.control(t.Address.Country.Iso3),
                  Name: this.fb.control(t.Address.Country.Name)
                }
              ),
              PostalCode: this.fb.control(t.Address.PostalCode)
            }
          )
        })
      )
    );
  }

  submitForm() {
    const AdminUnit: IAdministrationUnit = AdminUnitFactory.fromObject(this.editForm.value);
    if (this.isUpdatingAdminUnit) {
      AdminUnit.Id = this.AdminUnit.Id;
      AdminUnit.Edit = this.AdminUnit.Edit;
      for ( let i = 0; i < this.AdminUnit.Entrances.length; i++ ) {
        AdminUnit.Entrances[i].Id = this.AdminUnit.Entrances[i].Id;
        AdminUnit.Entrances[i].Edit = this.AdminUnit.Entrances[i].Edit;
      }
      this.AUdata.edit(AdminUnit).subscribe(res => {
        this.router.navigate(['../../administrationUnits', AdminUnit.Id], { relativeTo: this.route});
      });
    } else {
      this.AUdata.create(AdminUnit).subscribe(res => {
        this.AdminUnit = AdminUnitFactory.empty();
        this.editForm.reset(AdminUnitFactory.empty());
      });
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
    this.entrances.push(this.fb.group({ title: null, city: null, street: null, postalCode: null }));
  }
}
