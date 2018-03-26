import {Entrance} from '../../../shared/entrance';
import {Address} from '../../../shared/address';
import {CountryInfo} from '../../../shared/country-info';

export interface AdministrationUnitRaws {
  Id: string;
  UserKey: string;
  Title: string;
  Edit: string;
  YearOfConstruction?: string;
  Entrances: Entrance[];
}
