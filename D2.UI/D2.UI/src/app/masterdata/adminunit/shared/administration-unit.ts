import {Entrance} from '../../../shared/entrance';
import {IAdministrationUnit} from './iadministration-unit';
import {YearMonth} from '../../shared/year-month';
import {AdministrationUnitPropertyValue} from '../../../shared/administration-unit-property-value';


export class AdministrationUnit implements IAdministrationUnit {
  constructor(
    public Id: string,
    public UserKey: string,
    public Title: string,
    public Edit: Date,
    public Version: number,
    public Entrances?: Entrance[],
    public YearOfConstruction?: YearMonth,
    public AdministrationUnitProperties?: {
      Title: string;
      Description: string;
      Value: AdministrationUnitPropertyValue;
    }[]
  ) { }
}
