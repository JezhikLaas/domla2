import {Component, OnInit} from '@angular/core';
import {MatTableDataSource} from '@angular/material';
import {AdministrationUnitFeature} from '../../shared/administration-unit-feature';
import {DataType} from '../../shared/data-type';
import {MenuDisplayService} from '../../../shared/menu-display.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'ui-administration-unit-features-list-view',
  templateUrl: './administration-unit-features-list-view.component.html',
  styles: []
})
export class AdministrationUnitFeaturesListViewComponent implements OnInit {

  displayedColumns = ['Title', 'Description', 'Tag', 'TypedValueDecimalPlace', 'TypedValueUnit'];
  dataSource: MatTableDataSource<AdministrationUnitFeature>;
  DataType = DataType;

  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<AdministrationUnitFeature>(this.route.snapshot.data['AdministrationUnitFeatures']);
  }
}
