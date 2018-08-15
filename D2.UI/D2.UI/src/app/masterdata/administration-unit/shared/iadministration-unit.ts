import {Entrance} from './../../../shared/entrance';
import {YearMonth} from '../../shared/year-month';
import {Variant} from '../../../shared/variant';

export interface IAdministrationUnit {
  Id: string;
  UserKey: string;
  Title: string;
  Version: number;
  Entrances?: Entrance[];
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
