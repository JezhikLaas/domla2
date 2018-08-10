import {Variant} from '../../shared/variant';

export interface IAdministrationUnitProperty {
  Id: string;
  Title: string;
  Description: string;
  Value: Variant;
  Version: number;
  Edit: Date;
}
