import {Entrance} from '../../../shared/entrance';
import {IAdministrationUnit} from './iadministration-unit';
import {YearMonth} from '../../shared/year-month';
import {Variant} from '../../../shared/variant';
import {IAdministrationUnitProperty} from './iadministration-unit-property';
import {IUnboundSubunit} from '../../subunit/iunboundsubunit';
import {ISubunit} from '../../subunit/isubunit';
import {IAdministrationUnitSubunit} from './i-administration-unit-subunit';


export class AdministrationUnit implements IAdministrationUnit {
  constructor(
    public Id: string,
    public UserKey: string,
    public Title: string,
    public Edit: Date,
    public Version: number,
    public Entrances?: Entrance[],
    public YearOfConstruction?: YearMonth,
    public AdministrationUnitProperties?: IAdministrationUnitProperty[],
    public UnboundSubUnits?: IUnboundSubunit[],
    public SubUnits?: IAdministrationUnitSubunit[]
  ) { }
}
