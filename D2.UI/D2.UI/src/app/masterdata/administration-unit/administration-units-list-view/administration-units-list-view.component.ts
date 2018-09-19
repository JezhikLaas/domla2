import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { AdministrationUnit } from '../shared/administration-unit';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'ui-administration-units-view',
  templateUrl: './administration-units-list-view.component.html',
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
export class AdministrationUnitsListViewComponent implements OnInit {
  DisplayedColumns = ['userKey', 'title', 'country', 'postalCode', 'city', 'street', 'number'];
  DataSource: MatTableDataSource<AdministrationUnit>;
  InitialSelection = [];
  AllowMultiSelect = false;
  Selection = new SelectionModel<AdministrationUnit>(this.AllowMultiSelect, this.InitialSelection);
  @Output() administrationUnitSelected = new EventEmitter<any>();
  DisableSelectRow = false;
  constructor(
    private menuDisplay: MenuDisplayService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.DataSource = new MatTableDataSource<AdministrationUnit>(this.route.snapshot.data['AdministrationUnits']);
  }

  selectRow (administratinUnit: any) {
    this.administrationUnitSelected.emit(administratinUnit);
  }
}
