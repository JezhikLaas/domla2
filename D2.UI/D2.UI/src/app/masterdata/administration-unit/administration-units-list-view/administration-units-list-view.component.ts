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
  displayedColumns = ['userKey', 'title', 'country', 'postalCode', 'city', 'street', 'number'];
  dataSource: MatTableDataSource<AdministrationUnit>;
  initialSelection = [];
  allowMultiSelect = false;
  selection = new SelectionModel<AdministrationUnit>(this.allowMultiSelect, this.initialSelection);
  @Output() administrationUnitSelected = new EventEmitter<any>();
  disableSelectRow = false;
  constructor(
    private menuDisplay: MenuDisplayService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<AdministrationUnit>(this.route.snapshot.data['AdministrationUnits']);
  }

  selectRow (administratinUnit: any) {
    this.administrationUnitSelected.emit(administratinUnit);
  }
}
