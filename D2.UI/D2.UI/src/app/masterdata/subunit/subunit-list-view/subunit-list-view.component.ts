import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { AdministrationUnitSubunit } from '../../administration-unit/shared/administration-unit-subunit';
import { UnboundSubUnitType } from '../unbound-subunit-type';
import { AdminUnitFactory } from '../../administration-unit/shared/admin-unit-factory';
import { EntranceEditComponent } from '../../administration-unit/administration-unit-edit/entrance-edit/entrance-edit.component';
import { SubunitCreateComponent } from '../subunit-create/subunit-create.component';
import { SelectionModel } from '@angular/cdk/collections';
import { Entrance } from '../../../shared/entrance';

@Component({
  selector: 'ui-subinit-list-view',
  templateUrl: './subunit-list-view.component.html',
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
export class SubunitListViewComponent implements OnInit {
  displayedColumns = ['Title', 'Number', 'Floor', 'Entrance', 'Type'];
  InboundSubUnitType = UnboundSubUnitType;
  @Input() SubUnits: AdministrationUnitSubunit[];
  @Input() Entrances: Entrance[];
  DataSource: MatTableDataSource<AdministrationUnitSubunit>;
  initialSelection = [];
  allowMultiSelect = false;
  selection = new SelectionModel<Entrance>(this.allowMultiSelect, this.initialSelection);
  SubUnit = AdminUnitFactory.emptySubUnit();
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    public dialog: MatDialog
  ) {
  }

  ngOnInit() {
    this.DataSource = new MatTableDataSource<AdministrationUnitSubunit>(this.SubUnits);
    this.DataSource.sort = this.sort;
    this.DataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Entrance':
          if (item.Entrance.Address) {
            return item.Entrance.Title + ' ' + item.Entrance.Address.Street + ' ' + item.Entrance.Address.Number;
          } else {
            return '';
          }
        case 'Number':
          return item.Number;
        case 'Title':
          return item.Title;
        case 'Floor':
          return item.Floor;
        case 'Type':
          return this.InboundSubUnitType[item.Type - 1];
        default:
            return item + '.' +  property;
      }
    };
  }
  applyFilter(filterValue: string) {
    this.DataSource.filter = filterValue.trim().replace(/\s/g, '' ).replace(',', '' ).toLowerCase();
  }

  selectRow (entrance: any, index: number) {
    this.openDialog(entrance, index, this.Entrances);
  }


  openDialog (selectedSubUnit: any, index: number, entrances: Entrance []) {
    const dialogRef = this.dialog.open(SubunitCreateComponent, {
      width: '1000px',
      height: '800px',
      disableClose: false,
      hasBackdrop: true,
      data: {
        selectedSubUnit,
        entrances
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result && result !== selectedSubUnit && typeof(index) === 'number') {
        this.DataSource.data[index] = result;
      } else if (result && !index) {
        this.DataSource.data.push(result);
      }
      this.DataSource._updateChangeSubscription();
    });
  }
}
