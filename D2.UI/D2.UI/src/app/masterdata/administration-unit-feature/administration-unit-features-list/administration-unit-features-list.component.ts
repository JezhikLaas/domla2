import {Component, OnInit, ViewChild} from '@angular/core';
import { SelectionModel } from '@angular/cdk/collections';
import {MatDialogRef, MatSnackBar, MatTableDataSource} from '@angular/material';
import { MenuItem } from '../../../shared/menu-item';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { IAdministrationUnitFeature } from '../../shared/IAdministrationUnitFeature';
import {AdministrationUnitFeatureService} from '../../shared/administration-unit-feature.service';
import {DataType} from '../../shared/data-type';
import {AdministrationUnitFeaturesListViewComponent} from '../administration-unit-features-list-view/administration-unit-features-list-view.component';

@Component({
  selector: 'ui-administration-unit-features-list',
  templateUrl: './administration-unit-features-list.component.html'
})
export class AdministrationUnitFeaturesListComponent implements OnInit {
  @ViewChild(AdministrationUnitFeaturesListViewComponent) AdministrationUnitFeaturesListViewComponent: AdministrationUnitFeaturesListViewComponent;
  MenuButtons = [ ];
  constructor(
    private administrationUnitFeatureService: AdministrationUnitFeatureService,
    private menuDisplay: MenuDisplayService
  ) { }

  ngOnInit() {
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
  }

  refreshProperties() {
    this.administrationUnitFeatureService.listAdministrationUnitFeature().subscribe(res => this.AdministrationUnitFeaturesListViewComponent.dataSource
      = new MatTableDataSource<IAdministrationUnitFeature>(res));
  }
}

