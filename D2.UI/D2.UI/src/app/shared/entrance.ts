import {Address} from './address';
import {BoundSubunit} from '../masterdata/subunit/boundsubunit';

export class Entrance {
  Id: string;
  Title: string;
  Address: Address;
  Edit: string;
  Version: number;
  SubUnits?: BoundSubunit[];
  AdministrationUnitId: string;
}
