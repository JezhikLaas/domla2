import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {MatTableDataSource} from '@angular/material';
import {IAdministrationUnitFeature} from '../../shared/IAdministrationUnitFeature';
import {DataType} from '../../shared/data-type';
import {MenuDisplayService} from '../../../shared/menu-display.service';
import {ActivatedRoute, Router} from '@angular/router';
import {AdministrationUnitFeatureService} from '../../shared/administration-unit-feature.service';
import {OuterSubscriber} from 'rxjs/internal-compatibility';

@Component({
  selector: 'ui-administration-unit-features-list-view',
  templateUrl: './administration-unit-features-list-view.component.html',
  styles: []
})
export class AdministrationUnitFeaturesListViewComponent implements OnInit {

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
}
