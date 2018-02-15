import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { Registration } from '../shared/registration';
import { AdministrationService } from '../shared/administration.service';
import { SelectionModel } from '@angular/cdk/collections';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import { environment } from '../../environments/environment';
import { MenuDisplayService } from '../shared/menu-display.service';

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
  MenuButtons = ['Akzeptieren', 'Ablehnen'];

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
    this.service.fetchRegistrations(
      () => {},
      (message: string) => {
        if (environment.production) {
          this.errorDialog.show('Fehler', message);
        }
      }
    )
    .subscribe(data => {
      this.dataSource = new MatTableDataSource<Registration>(data);
    });

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
}
