import { IAdministrationUnit } from './iadministration-unit';
import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitRaws } from './administration-unit-raws';
import { Entrance } from '../../../shared/entrance';

export class AdminUnitFactory {
  static empty(): AdministrationUnit {
    return new AdministrationUnit('', '', '', new Date(), <Entrance[]>[] );
  }

  static fromObject (rawAdministrationUnit: AdministrationUnitRaws | any): AdministrationUnit {
    return new AdministrationUnit(
      rawAdministrationUnit.Id,
      rawAdministrationUnit.UserKey,
      rawAdministrationUnit.Title,
      typeof (rawAdministrationUnit.Edit) === 'string' ?
            new Date (rawAdministrationUnit.Edit) : rawAdministrationUnit.Edit,
      rawAdministrationUnit.Entrances,
      typeof(rawAdministrationUnit.YearOfConstruction) === 'string' ?
                       new Date (rawAdministrationUnit.YearOfConstruction) : rawAdministrationUnit.YearOfConstruction,
    );
  }
}
