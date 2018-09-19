import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { AdministrationUnit } from '../../shared/administration-unit';
import { SelectionModel } from '@angular/cdk/collections';
import { MenuDisplayService } from '../../../../shared/menu-display.service';
import { ActivatedRoute } from '@angular/router';
import { Entrance } from '../../../../shared/entrance';
import { EntranceEditComponent } from '../entrance-edit/entrance-edit.component';
import { AdminUnitFactory } from '../../shared/admin-unit-factory';

@Component({
  selector: 'ui-entrances-list',
  templateUrl: './entrances-list.component.html',
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
export class EntrancesListComponent implements OnInit {
  DisplayedColumns = ['title', 'country', 'postalCode', 'city', 'street', 'number'];
  DataSource = new MatTableDataSource<Entrance>();
  InitialSelection = [];
  AllowMultiSelect = false;
  Selection = new SelectionModel<Entrance>(this.AllowMultiSelect, this.InitialSelection);
  @Input() Entrances: Entrance[];
  @Output() IsModified: EventEmitter <any> = new EventEmitter <any>();
  Entrance = AdminUnitFactory.emptyEntrance();
  DisableSelectRow = false;
  constructor(
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.DataSource = new MatTableDataSource<Entrance>(this.Entrances);
  }

  selectRow (entrance: any, index: number) {
    this.openDialog(entrance, index);
  }

  openDialog (selectedEntrance: any, index: number) {
    const dialogRef = this.dialog.open(EntranceEditComponent, {
      width: '1000px',
      height: '800px',
      disableClose: false,
      hasBackdrop: true,
      data: selectedEntrance
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        if (result !== selectedEntrance && typeof(index) === 'number') {
          this.DataSource.data[index] = result;
        } else if (!index) {
          this.DataSource.data.push(result);
        }
        this.DataSource._updateChangeSubscription();
        this.IsModified.emit();
      }
    });
  }
}
