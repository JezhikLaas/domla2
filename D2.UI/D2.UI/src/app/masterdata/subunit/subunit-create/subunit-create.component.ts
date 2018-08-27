import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormArray, FormControl} from '@angular/forms';
import {UnboundSubUnitType} from '../iunboundsubunit';
import {Entrance} from '../../../shared/entrance';

@Component({
  selector: 'ui-subinit-create',
  templateUrl: './subunit-create.component.html',
  styles: []
})
export class SubunitCreateComponent implements OnInit {
  @Input() Title: FormControl;
  @Input() Number: FormControl;
  @Input() Type: FormControl;
  @Input() Entrances: Array<Entrance>;
  @Input() Floor: FormControl;
  @Input() ArrayLastElement: number;
  @Input() Entrance: FormControl;
  @Output() addSubUnit = new EventEmitter<any>();
  @Output() selectedType = new EventEmitter<any>();
  @Output() removeSubUnitControl = new EventEmitter<any>();
  UnboundSubUnitType =  UnboundSubUnitType;
  constructor() { }

  ngOnInit() {
  }

  onAddSubUnitControl () {
    this.addSubUnit.emit();
  }
  onRemoveSubUnitControl() {
    this.removeSubUnitControl.emit();
  }
  onSelectedType(event: any) {
    this.selectedType.emit(event);
  }
}
