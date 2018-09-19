import {Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { MenuDisplayService } from '../../../shared/menu-display.service';
import { AdministrationUnitFeature } from '../../shared/administration-unit-feature';
import { AdministrationUnitFeatureService } from '../../shared/administration-unit-feature.service';
import { AdministrationUnitFeaturesListViewComponent } from '../administration-unit-features-list-view/administration-unit-features-list-view.component';

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
    this.administrationUnitFeatureService.listAdministrationUnitFeature().subscribe(res => this.AdministrationUnitFeaturesListViewComponent.DataSource
      = new MatTableDataSource<AdministrationUnitFeature>(res));
  }
}

