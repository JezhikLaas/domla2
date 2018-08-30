import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {MatSort, MatTableDataSource} from '@angular/material';
import {IAdministrationUnitSubunit} from '../../administration-unit/shared/i-administration-unit-subunit';
import {UnboundSubUnitType} from '../iunboundsubunit';

@Component({
  selector: 'ui-subinit-list-view',
  templateUrl: './subunit-list-view.component.html',
  styles: []
})
export class SubunitListViewComponent implements OnInit {
  displayedColumns = ['Title', 'Number', 'Floor', 'Entrance', 'Type'];
  InboundSubUnitType = UnboundSubUnitType;
  @Input() SubUnits: IAdministrationUnitSubunit[];
  dataSource: MatTableDataSource<IAdministrationUnitSubunit>;
  @ViewChild(MatSort) sort: MatSort;
  constructor() {
  }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<IAdministrationUnitSubunit>(this.SubUnits);
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Entrance':
          if (item.Entrance.Address) {
            return item.Entrance.Title + ' ' + item.Entrance.Address.Street + ' ' + item.Entrance.Address.Number;
          } else {
            return '';
          }
        case 'Number':
          return item.Number;
        case 'Title':
          return item.Title;
        case 'Floor':
          return item.Floor;
        case 'Type':
          return this.InboundSubUnitType[item.Type - 1];
        default:
            return item + '.' +  property;
      }
    };
  }
  applyFilter(filterValue: string) {
    /*this.dataSource.filterPredicate = (data, filter) =>
      data.Title.trim().toLowerCase().indexOf(filter) !== -1 ||
      data.Number.toString().trim().toLowerCase().indexOf(filter) !== -1;
      this.InboundSubUnitType[data.Type + 1].toString().trim().toLowerCase().indexOf(filter) !== -1; */

    this.dataSource.filter = filterValue.trim().replace(/\s/g, '' ).replace(',', '' ).toLowerCase();
  }
}
