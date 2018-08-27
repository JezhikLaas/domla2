import {SubUnit} from './subunit';
import {IUnboundSubunit, UnboundSubUnitType} from './iunboundsubunit';

export class UnboundSubunit extends SubUnit implements IUnboundSubunit {
  public Type?: UnboundSubUnitType;
  constructor (id: string, title: string, version: number, usage: string, number: number, type: UnboundSubUnitType) {
    super();
    this.Id = id;
    this.Title = title;
    this.Version = version;
    this.Number = number;
    this.Type = type;
  }
}
