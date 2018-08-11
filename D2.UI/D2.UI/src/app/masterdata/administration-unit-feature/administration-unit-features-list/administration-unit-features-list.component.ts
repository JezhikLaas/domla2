import { Component, OnInit } from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import {MatDialogRef, MatTableDataSource} from '@angular/material';
import { MenuItem } from '../../../shared/menu-item';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IAdministrationUnitFeature } from '../../shared/IAdministrationUnitFeature';
import {AdministrationUnitFeatureService} from '../../shared/administration-unit-feature.service';
import {DataType} from '../../shared/data-type';

@Component({
  selector: 'ui-administration-unit-features-list',
  templateUrl: './administration-unit-features-list.component.html'
})
export class AdministrationUnitFeaturesListComponent implements OnInit {
  displayedColumns = ['Title', 'Description', 'Tag', 'TypedValueDecimalPlace', 'TypedValueUnit'];
  dataSource: MatTableDataSource<IAdministrationUnitFeature>;
  DataType = DataType;

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute,
    private bs: AdministrationUnitFeatureService
  ) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<IAdministrationUnitFeature>(this.route.snapshot.data['AdministrationUnitFeatures']);
  }

  refreshProperties() {
    this.bs.listAdministrationUnitFeature().subscribe(res => this.dataSource = new MatTableDataSource<IAdministrationUnitFeature>(res));
  }
}

