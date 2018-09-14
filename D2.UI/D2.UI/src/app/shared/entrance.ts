import {Address} from './address';
import {BoundSubunit} from '../masterdata/subunit/boundsubunit';

export class Entrance {
  constructor (
  public Id: string,
  public Title: string,
  public Address: Address,
  public Edit: string,
  public Version: number,
  public AdministrationUnitId: string,
  public SubUnits?: BoundSubunit[]
  ) {}
}
