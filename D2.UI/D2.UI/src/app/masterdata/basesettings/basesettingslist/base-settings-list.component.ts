import { Component, OnInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material';
import { MenuItem } from '../../../shared/menu-item';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IBaseSettings } from '../../shared/Ibasesettings';
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
  dataSource: MatTableDataSource<IBaseSettings>;
  initialSelection = [];
  allowMultiSelect = false;
  selection = new SelectionModel<IBaseSettings>(this.allowMultiSelect, this.initialSelection);
  receivedBaseSettings: IBaseSettings [];

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
    this.dataSource = new MatTableDataSource<IBaseSettings>(this.route.snapshot.data['BaseSettings']);
  }
  selectRow (BaseSettings) {
    this.router.navigate([`baseSettings/${BaseSettings.Id}`]);
  }
}

