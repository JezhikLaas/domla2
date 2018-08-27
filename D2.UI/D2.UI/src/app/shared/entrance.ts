import {Address} from './address';
import {IBoundSubunit} from '../masterdata/subunit/iboundsubunit';

export class Entrance {
  Id: string;
  Title: string;
  Address: Address;
  Edit: string;
  Version: number;
  SubUnits?: IBoundSubunit[];
  AdministrationUnitId: string;
}
