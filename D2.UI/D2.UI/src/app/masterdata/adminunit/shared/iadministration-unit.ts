import {Entrance} from './../../../shared/entrance';
import {YearMonth} from '../../shared/year-month';
import {AdministrationUnitPropertyValue} from '../../../shared/administration-unit-property-value';

export interface IAdministrationUnit {
  Id: string;
  UserKey: string;
  Title: string;
  Version: number;
  Entrances?: Entrance[];
  YearOfConstruction?: YearMonth;
  Edit: Date;
  AdministrationUnitProperty?: {
    Id: string;
    Title: string;
    Description: string;
    Value?: AdministrationUnitPropertyValue;
  }[];
}
