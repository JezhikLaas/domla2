import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IAdministrationUnit } from '../shared/iadministration-unit';
import { MenuItem } from '../../../shared/menu-item';
import { ActivatedRoute, Router } from '@angular/router';
import { AdministrationUnitService } from '../shared/administration-unit.service';

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
    new MenuItem('Neu', () => this.router.navigate(['administrationUnits/0']), () => true),
    new MenuItem('Bearbeiten', () => this.router.navigate(['administrationUnits/123']), () => true),
  ];
  displayedColumns = ['userKey', 'title', 'country', 'postalCode', 'city', 'street', 'number'];
  dataSource: MatTableDataSource<IAdministrationUnit>;
  initialSelection = [];
  allowMultiSelect = true;

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private units: AdministrationUnitService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
    this.dataSource = new MatTableDataSource<IAdministrationUnit>(this.route.snapshot.data['AdministrationUnit']);
  }
}
