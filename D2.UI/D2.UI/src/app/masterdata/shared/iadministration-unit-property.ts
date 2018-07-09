import {AdministrationUnitPropertyValue} from '../../shared/administration-unit-property-value';

export interface IAdministrationUnitProperty {
  Id: string;
  Title: string;
  Description: string;
  Value: AdministrationUnitPropertyValue;
  Version: number;
  Edit: Date;
}
