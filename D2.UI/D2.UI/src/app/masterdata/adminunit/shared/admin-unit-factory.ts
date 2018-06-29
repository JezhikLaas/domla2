import { IAdministrationUnit } from './iadministration-unit';
import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitRaws } from './administration-unit-raws';
import { Entrance } from '../../../shared/entrance';
import { Address } from '../../../shared/address';
import { CountryInfo } from '../../../shared/country-info';
import {YearMonth} from '../../shared/year-month';

export class AdminUnitFactory {
  static empty(): AdministrationUnit {
    return new AdministrationUnit('', '', '', new Date(), 0, [
      {
        Id: '',
        Title: '',
        Version: 0,
        Address: {
          Country:
            {Iso2: 'DE', Iso3: '', Name: ''},
          City: '',
          Street: '',
          Number: '',
          PostalCode: ''
        },
        Edit: '',
        AdministrationUnitId: ''
      }],
      new YearMonth(0, 0 ),
      [
        {
          Title: '',
          Description: '',
          Value: {
            Tag: '3',
            Raw: ''
          }
        }
        ]
    );
  }

  static fromObject (rawAdministrationUnit: AdministrationUnitRaws | any): AdministrationUnit {
    return new AdministrationUnit(
      rawAdministrationUnit.Id,
      rawAdministrationUnit.UserKey,
      rawAdministrationUnit.Title,
      typeof (rawAdministrationUnit.Edit) === 'string' ?
            new Date (rawAdministrationUnit.Edit) : rawAdministrationUnit.Edit,
      rawAdministrationUnit.Version,
      rawAdministrationUnit.Entrances,
      typeof (rawAdministrationUnit.YearOfConstruction) === 'string' ?
            new YearMonth(rawAdministrationUnit.YearOfConstruction.Year, rawAdministrationUnit.YearOfConstruction.Month) :
              rawAdministrationUnit.YearOfConstruction,
      rawAdministrationUnit.AdministrationUnitProperties
    );
  }
}
