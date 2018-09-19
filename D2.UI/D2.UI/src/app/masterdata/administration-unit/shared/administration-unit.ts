import { Entrance } from '../../../shared/entrance';
import { YearMonth } from '../../shared/year-month';
import { AdministrationUnitProperty } from './administration-unit-property';
import { UnboundSubunit } from '../../subunit/unbound-subunit';
import { AdministrationUnitSubunit } from './administration-unit-subunit';


export class AdministrationUnit {
  constructor(
    public Id: string,
    public UserKey: string,
    public Title: string,
    public Edit: Date,
    public Version: number,
    public Entrances?: Entrance[],
    public YearOfConstruction?: YearMonth,
    public AdministrationUnitProperties?: AdministrationUnitProperty[],
    public UnboundSubUnits?: UnboundSubunit[],
    public SubUnits?: AdministrationUnitSubunit[],
    public IsModified: boolean = false
  ) { }
}
