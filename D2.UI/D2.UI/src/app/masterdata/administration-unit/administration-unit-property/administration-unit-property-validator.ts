import {FormArray, FormGroup} from '@angular/forms';

export class AdministrationUnitPropertyValidator {
  static checkPropertyValue (value: FormGroup): { [error: string]: any } {
    return value.get(['Tag']).value === 2 &&
        (!value.get(['RawNumber', '_decimalPlaces']).value || !value.get(['RawNumber', '_unit']).value) ?
     {CheckProperty: {valid: false}} : null;
  }
}
