import {Component, OnInit, ViewChild, Inject} from '@angular/core';
import {AdministrationUnitsListViewComponent} from '../../adminunit/administration-units-list-view/administration-units-list-view.component';
import {MAT_DIALOG_DATA, MatDialogRef, MatTableDataSource} from '@angular/material';
import {IAdministrationUnit} from '../../adminunit/shared/iadministration-unit';
import {SelectionModel} from '@angular/cdk/collections';
import {MenuDisplayService} from '../../../shared/menu-display.service';
import {ActivatedRoute, Router} from '@angular/router';
import {AdministrationUnitService} from '../../adminunit/shared/administration-unit.service';
import {IAdministrationUnitFeature} from '../../shared/IAdministrationUnitFeature';

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
  AdministrationUnitsId: String [] = [];
  displayedColumns = ['select', 'userKey', 'title', 'country', 'postalCode', 'city', 'street', 'number'];
  dataSource: MatTableDataSource<IAdministrationUnit>;
  initialSelection = [];
  allowMultiSelect = true;
  selection = new SelectionModel<IAdministrationUnit>(this.allowMultiSelect, this.initialSelection);
  constructor(
    public dialogRef: MatDialogRef<DialogAdministrationUnitsListComponent>,
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute,
    private as: AdministrationUnitService
  ) { }

  ngOnInit() {
    this.as.listAdministrationUnits().subscribe(res => this.dataSource =
      new MatTableDataSource<IAdministrationUnit>(res));
  }

  AdministrationUnitSelected(administrationUnit: any) {
    this.AdministrationUnitsId.push(administrationUnit.Id);
    console.log(this.AdministrationUnitsId[0]);
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  selectRow (administratinUnit: any) {
    this.AdministrationUnitsId.push(administratinUnit.Id);
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }
}
