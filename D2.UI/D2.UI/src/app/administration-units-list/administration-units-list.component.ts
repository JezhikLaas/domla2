import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { MenuDisplayService } from '../shared/menu-display.service';
import { AdministrationUnit } from '../shared/administration-unit';
import { MenuItem } from '../shared/menu-item';
import { Router } from '@angular/router';

@Component({
  selector: 'ui-administration-units',
  templateUrl: './administration-units-list.component.html',
  styles: [`
    .mat-column-select {
      overflow: initial;
    }
  `]
})
export class AdministrationUnitsListComponent implements OnInit {
  MenuButtons = [
    new MenuItem('Neu', () => this.router.navigate(['editAdministrationUnit/0'])),
    new MenuItem('Bearbeiten', () => console.log('Edit'))
  ];
  displayedColumns = ['select', 'userKey', 'title', 'country', 'postalCode', 'city', 'street', 'number'];
  dataSource: MatTableDataSource<AdministrationUnit>;
  initialSelection = [];
  allowMultiSelect = true;
  selection = new SelectionModel<AdministrationUnit>(this.allowMultiSelect, this.initialSelection);

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router
  ) { }

  ngOnInit() {
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
