import {Component,  OnInit} from '@angular/core';
import {MenuItem} from '../../../shared/menu-item';
import {MenuDisplayService} from '../../../shared/menu-display.service';
import {Router} from '@angular/router';

@Component({
  selector: 'ui-administration-units-list',
  templateUrl: './administration-units-list.component.html',
  styles: []
})
export class AdministrationUnitsListComponent implements OnInit {

  MenuButtons = [
    new MenuItem('New', () => this.router.navigate([`administrationUnits/0`]), () => true),
  ];
  constructor(
    private menuDisplay: MenuDisplayService,
    private router: Router
  ) { }

  ngOnInit() {
    this.menuDisplay.menuNeeded.emit(this.MenuButtons);
  }

  selectRow (AdminUnit) {
      this.router.navigate([`administrationUnits/${AdminUnit.Id}`]);
  }

}
