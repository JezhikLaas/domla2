import { Component, OnInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import { MatTableDataSource } from '@angular/material';
import { MenuItem } from '../../../shared/menu-item';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IBaseSetting } from '../../shared/ibasesetting';
import {BaseSettingsService} from '../../shared/basesettings.service';
import {DataType} from '../../shared/data-type';

@Component({
  selector: 'ui-base-settings-list',
  templateUrl: './base-settings-list.component.html'
})
export class BaseSettingsListComponent implements OnInit {
  displayedColumns = ['Title', 'Description', 'Tag', 'TypedValueDecimalPlace', 'TypedValueUnit'];
  dataSource: MatTableDataSource<IBaseSetting>;
  DataType = DataType;

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute,
    private bs: BaseSettingsService
  ) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<IBaseSetting>(this.route.snapshot.data['BaseSettings']);
  }

  refreshProperties() {
    this.bs.listBaseSettings().subscribe(res => this.dataSource = new MatTableDataSource<IBaseSetting>(res));
  }
}

