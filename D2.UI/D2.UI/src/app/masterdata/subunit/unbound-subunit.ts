import { SubUnit } from './subunit';
import { UnboundSubUnitType } from './unbound-subunit-type';

export class UnboundSubunit extends SubUnit {
  public Type?: UnboundSubUnitType;
  constructor (id: string, title: string, version: number, usage: string, number: number, type: UnboundSubUnitType) {
    super(id, title, version, number);
    this.Type = type;
  }
}
