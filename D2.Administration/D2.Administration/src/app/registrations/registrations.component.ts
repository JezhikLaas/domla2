import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { Registration } from '../shared/registration';
import { AdministrationService } from '../shared/administration.service';
import { SelectionModel } from '@angular/cdk/collections';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import { MenuDisplayService } from '../shared/menu-display.service';
import { MenuItem } from '../shared/menu-item';

@Component({
  selector: 'am-registrations',
  templateUrl: './registrations.component.html',
  styles: [`
    .mat-column-select {
      overflow: initial;
    }
  `]
})
export class RegistrationsComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Akzeptieren', () => this.acceptRegistrations(), () => true),
    new MenuItem('Ablehnen', () => console.log('Ablehnen'), () => true)
  ];

  displayedColumns = ['select', 'login', 'salutation', 'title', 'firstName', 'lastName', 'email'];
  dataSource: MatTableDataSource<Registration>;
  initialSelection = [];
  allowMultiSelect = true;
  selection = new SelectionModel<Registration>(this.allowMultiSelect, this.initialSelection);

  constructor(
    private service: AdministrationService,
    private errorDialog: ErrorDialogComponent,
    private menuDisplay: MenuDisplayService
  ) { }

  ngOnInit() {
    this.service.fetchRegistrations()
    .subscribe(
      data => this.dataSource = new MatTableDataSource<Registration>(data),
      error => this.errorDialog.show('Fehler', error.message)
    );

    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  acceptRegistrations() {
    const registrationIds = this.selection.selected.map((value, index, values) => value.id);
    this.service.confirmRegistrations(registrationIds)
      .subscribe(() => {
        this.service.fetchRegistrations()
          .subscribe(
            data => this.dataSource = new MatTableDataSource<Registration>(data),
            error => this.errorDialog.show('Fehler', error.message)
          );
      },
     error => this.errorDialog.show('Fehler', error.message));
  }
}
