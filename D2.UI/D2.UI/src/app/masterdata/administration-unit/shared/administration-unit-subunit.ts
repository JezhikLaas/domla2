import {IAdministrationUnitSubunit} from './i-administration-unit-subunit';
import {Entrance} from '../../../shared/entrance';

export class AdministrationUnitSubunit implements IAdministrationUnitSubunit {
  constructor (
    public Id: string,
    public Title: string,
    public Version: number,
    public Number?: number,
    public Floor?: string,
    public Type?: number,
    public Entrance?: Entrance
  ) {}
}
