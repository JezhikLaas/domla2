import {Entrance} from '../../../shared/entrance';
import {Address} from '../../../shared/address';
import {CountryInfo} from '../../../shared/country-info';

export interface AdministrationUnitRaws {
  Id: string;
  UserKey: string;
  Title: string;
  Edit: string;
  Version: number;
  YearOfConstruction?: {
    Year: string;
    Month: string;
  };
  Entrances?: {
    Id: string;
    Title: string;
    Address: {
      Country?: {
        Iso2: string;
        Iso3: string;
        Name: string;
      };
      City: string;
      Street: string;
      Number: string;
      PostalCode: string;
    }
    Edit: string;
    SubUnits?: string;
    AdministrationUnitId: string;
  }[];
}
