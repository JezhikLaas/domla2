import { Component, OnInit} from '@angular/core';
import { MatDialogRef, MatTableDataSource } from '@angular/material';
import { AdministrationUnit } from '../../administration-unit/shared/administration-unit';
import { SelectionModel } from '@angular/cdk/collections';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AdministrationUnitService } from '../../administration-unit/shared/administration-unit.service';

@Component({
  selector: 'ui-dialog-administration-units-list',
  templateUrl: './dialog-administration-units-list.component.html',
  styles: [`
    .mat-column-select {
      overflow: initial;
    }
    .mat-row.selected {
        background-color: lightblue;
        cursor: pointer;
    }
  `]
})
export class DialogAdministrationUnitsListComponent implements OnInit {
  displayedColumns = ['select', 'userKey', 'title', 'country', 'postalCode', 'city', 'street', 'number'];
  dataSource: MatTableDataSource<AdministrationUnit>;
  initialSelection = [];
  allowMultiSelect = true;
  selection = new SelectionModel<AdministrationUnit>(this.allowMultiSelect, this.initialSelection);
  constructor(
    public dialogRef: MatDialogRef<DialogAdministrationUnitsListComponent>,
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute,
    private as: AdministrationUnitService
  ) { }

  ngOnInit() {
    this.as.listAdministrationUnits().subscribe(res => this.dataSource =
      new MatTableDataSource<AdministrationUnit>(res));
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear Selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }
}
