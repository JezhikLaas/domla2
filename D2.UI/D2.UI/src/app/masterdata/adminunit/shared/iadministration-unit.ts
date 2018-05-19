import {Entrance} from './../../../shared/entrance';
import {YearMonth} from '../../shared/year-month';

export interface IAdministrationUnit {
  Id: string;
  UserKey: string;
  Title: string;
  Version: number;
  Entrances?: Entrance[];
  YearOfConstruction?: YearMonth;
  Edit: Date;
}
