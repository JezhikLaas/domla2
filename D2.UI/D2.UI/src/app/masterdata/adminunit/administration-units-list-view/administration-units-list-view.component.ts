import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IAdministrationUnit } from '../shared/iadministration-unit';
import { ActivatedRoute, Router } from '@angular/router';

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
  dataSource: MatTableDataSource<IAdministrationUnit>;
  initialSelection = [];
  allowMultiSelect = false;
  selection = new SelectionModel<IAdministrationUnit>(this.allowMultiSelect, this.initialSelection);
  @Output() administrationUnitSelected = new EventEmitter<any>();
  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<IAdministrationUnit>(this.route.snapshot.data['AdministrationUnits']);
  }

  selectRow (administratinUnit: any) {
    this.administrationUnitSelected.emit(administratinUnit);
  }
}
