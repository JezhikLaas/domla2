import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { UnboundSubUnitType } from '../../../subunit/unbound-subunit-type';
import { Entrance } from '../../../../shared/entrance';
import { MatDialog, MatSort, MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { AdminUnitFactory } from '../../shared/admin-unit-factory';
import { AdministrationUnitProperty } from '../../shared/administration-unit-property';
import { AdministrationUnitPropertyEditComponent} from '../administration-unit-property-edit/administration-unit-property-edit.component';
import { Variant } from '../../../../shared/variant';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'ui-administration-unit-property-list',
  templateUrl: './administration-unit-property-list.component.html',
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
export class AdministrationUnitPropertyListComponent implements OnInit {
  DisplayedColumns = ['Title', 'Description', 'Value.Tag', 'Value.Raw._unit', 'Value.Raw._decimalPlaces', 'Value.Raw._value'];
  @Input() AdministrationUnitProperties: AdministrationUnitProperty[];
  @Output() IsModified: EventEmitter <any> = new EventEmitter <any>();
  DataSource: MatTableDataSource<AdministrationUnitProperty>;
  InitialSelection = [];
  AllowMultiSelect = false;
  Selection = new SelectionModel<Entrance>(this.AllowMultiSelect, this.InitialSelection);
  emptyProperty = AdminUnitFactory.emptyProperty();
  @ViewChild(MatSort) sort: MatSort;
  pipe = new DatePipe('de');
  constructor(
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.DataSource = new MatTableDataSource<AdministrationUnitProperty>(this.AdministrationUnitProperties);
    this.DataSource.data.forEach(x => this.transformForDisplay(x.Value));
  }

  transformForDisplay(Value: Variant) {
    if (Value.Raw && typeof(Value.Raw._value) === 'undefined') {
      Value.Raw = {_value: Value.Raw};
    }
    if ( Value.Tag === 1 && Value.Raw) {
      Value.Raw._value = this.pipe.transform(Value.Raw._value, 'fullDate');
    }
  }

  transformFromEdit(Value: any)  {
    if (Value.Tag === 3 ) {
      Value.Raw = {_value: Value.Raw};
    } else if (Value.Tag === 2) {
        Value.Raw = Value.RawNumber;
      } else if (Value.Tag === 1) {
        Value.Raw = {_value: this.pipe.transform(Value.Raw, 'fullDate')};
    }
  }

  selectRow (adminUnitProperty: any, index: number) {
    this.openDialog(adminUnitProperty, index);
  }


  openDialog (selectedAdminUnitProperty: any, index: number) {
    const dialogRef = this.dialog.open(AdministrationUnitPropertyEditComponent, {
      width: '1000px',
      height: '800px',
      disableClose: false,
      hasBackdrop: true,
      data: selectedAdminUnitProperty
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result !== selectedAdminUnitProperty && typeof(index) === 'number') {
          this.transformFromEdit(result.Value);
          this.DataSource.data[index] = result;
        } else if (!index) {
          this.transformFromEdit(result.Value)
          this.DataSource.data.push(result);
        }
        this.DataSource._updateChangeSubscription();
        this.IsModified.emit();
      }
    }
    );
  }

}
