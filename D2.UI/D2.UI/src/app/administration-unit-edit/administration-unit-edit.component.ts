import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import {MenuItem} from '../shared/menu-item';
import {MenuDisplayService} from '../shared/menu-display.service';
import {Router} from '@angular/router';
import {ConfirmDialogComponent} from '../shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'ui-administration-unit-edit',
  templateUrl: './administration-unit-edit.component.html',
  styles: []
})

export class AdministrationUnitEditComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Speichern', () => console.log('Save')),
    new MenuItem('Schließen', () => this.doCancel())
  ];
  editForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private menuDisplay: MenuDisplayService,
    private confirmDialog: ConfirmDialogComponent,
    private router: Router
  ) { }

  ngOnInit() {
    this.editForm = this.fb.group({
      userKey: this.fb.control(
        null,
        [
          Validators.required
        ]
      ),
      title: this.fb.control(
        null,
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
        if (value) { this.router.navigate(['administrationUnits']); }
      }
    );
  }
}
