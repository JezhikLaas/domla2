import {IAdministrationUnit} from './iadministration-unit';
import { AdministrationUnit } from './administration-unit';

export class AdminUnitFactory {
  static empty (): AdministrationUnit {
    return new AdministrationUnit ('', '', '');
  }
}
