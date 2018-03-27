import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MenuItem } from '../../../shared/menu-item';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import {ActivatedRoute, Router} from '@angular/router';
import { ConfirmDialogComponent } from '../../../shared/confirm-dialog/confirm-dialog.component';
import { AdministrationUnitService } from '../shared/administration-unit.service';
import { IAdministrationUnit } from '../shared/iadministration-unit';
import {AdminUnitFactory} from '../shared/admin-unit-factory';

@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styles: []
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => console.log('Save'), () => false),
    new MenuItem('Schließen', () => this.doCancel(), () => true)
  ];
  editForm: FormGroup;
  isUpdatingAdminUnit = false;
  AdminUnit = AdminUnitFactory.empty();

  constructor(private fb: FormBuilder,
              private menuDisplay: MenuDisplayService,
              private confirmDialog: ConfirmDialogComponent,
              private router: Router,
              private route: ActivatedRoute,
              private AUdata: AdministrationUnitService) {
  }

  ngOnInit() {
    const id = this.route.snapshot.params ['id'];
    if (id !== 0) {
      this.isUpdatingAdminUnit = true;
      this.AdminUnit = this.route.snapshot.data['AdministrationUnit'];
    }
    this.initAdminUnit();
  }

  initAdminUnit() {
    this.editForm = this.fb.group({
      userKey: this.fb.control(
        this.AdminUnit.UserKey,
        [
          Validators.required
        ]
      ),
      title: this.fb.control(
        this.AdminUnit.Title,
        [
          Validators.required
        ]
      )
    });
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
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
