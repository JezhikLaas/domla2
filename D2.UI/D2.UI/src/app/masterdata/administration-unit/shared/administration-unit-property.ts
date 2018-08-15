import {Variant} from '../../../shared/variant';

export class AdministrationUnitProperty {
  constructor(
    public Id: string,
    public Edit: Date,
    public Version: number,
    public Title: string,
    public Description: string,
    public Value: Variant
  ) { }
}

