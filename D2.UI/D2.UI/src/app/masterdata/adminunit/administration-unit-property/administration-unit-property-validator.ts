import {FormArray, FormGroup} from '@angular/forms';

export class AdministrationUnitPropertyValidator {
  static checkProperty(controlPropertyArray: FormArray): { [error: string]: any } {
    const Length = controlPropertyArray.length;
    const Title = controlPropertyArray.get(['Title']).value;
    const ValueTag = controlPropertyArray.get(['_value', 'Tag']).value;
    const ValueRaw = controlPropertyArray.get(['_value', 'Raw']).value;
    return (Length > 0 && Title !== '' && ValueTag !== '') ? null : {
      checkProperty: {valid: false}
    };
  }
}
