import {Entrance} from './../../../shared/entrance';
import {YearMonth} from '../../shared/year-month';
import {Variant} from '../../../shared/variant';
import {ISubunit} from '../../subunit/isubunit';
import {IUnboundSubunit} from '../../subunit/iunboundsubunit';

export interface IAdministrationUnit {
  Id: string;
  UserKey: string;
  Title: string;
  Version: number;
  Entrances?: Entrance[];
  SubUnits?: ISubunit[];
  UnboundSubUnits?: IUnboundSubunit[];
  YearOfConstruction?: YearMonth;
  Edit: Date;
  AdministrationUnitProperties?: {
    Title: string;
    Description: string;
    Value?: Variant;
    Version: number;
    Id: string;
  }[];
}
