import { Component, OnInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material';
import { MenuItem } from '../../../shared/menu-item';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IBaseSetting } from '../../shared/ibasesetting';
import {BaseSettingsService} from '../../shared/basesettings.service';

@Component({
  selector: 'ui-base-settings-list',
  templateUrl: './base-settings-list.component.html',
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
export class BaseSettingsListComponent implements OnInit {
  MenuButtons = [
    new MenuItem('New', () => this.router.navigate([`baseSettings/0`]), () => true),
  ];
  displayedColumns = ['Title', 'Description', 'Tag', 'TypedValueDecimalPlace', 'TypedValueUnit'];
  dataSource: MatTableDataSource<IBaseSetting>;
  initialSelection = [];
  allowMultiSelect = false;
  selection = new SelectionModel<IBaseSetting>(this.allowMultiSelect, this.initialSelection);
  receivedBaseSettings: IBaseSetting [];

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    // this.menuDisplay.menuNeeded.emit(this.MenuButtons);
    this.dataSource = new MatTableDataSource<IBaseSetting>(this.route.snapshot.data['BaseSettings']);
  }
  selectRow (BaseSettings) {
    this.router.navigate([`baseSettings/${BaseSettings.Id}`]);
  }
}

