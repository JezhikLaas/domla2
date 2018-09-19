import {Variant} from '../../../shared/variant';

export class AdministrationUnitProperty {
  constructor(
    public Id: string,
    public Edit: Date | string,
    public Version: number,
    public Title: string,
    public Value: Variant,
    public Description?: string
  ) { }
}

