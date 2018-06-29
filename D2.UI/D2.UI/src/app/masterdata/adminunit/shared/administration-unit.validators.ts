import { FormControl, FormArray, FormGroup, Validators} from '@angular/forms';
import { Address } from '../../../shared/address';

export class AdministrationUnitValidators  {
  static atLeastOneEntrance(controlArray: FormArray): { [error: string]: any } {
    const check = controlArray.controls.some (el => {
      // return  (AdministrationUnitValidators.EntranceNotEmpty(el.value.Address, el.value.Title));
      return (el.value) ? true : false;
    });
    return check ? null : {
      atLeastOneEntrance: { valid: false }
    };
  }

  static  EntranceNotEmpty(address: Address, title: string ): boolean {
    const check = (
      address.Street
      && address.Number
      && address.City
      && address.PostalCode
      && address.Country
      && address.Street
      && title) ? true : false;
    return check ? true :  false;
  }

  static EntranceOnceValidator (controlGroup: FormGroup): { [error: string]: any } {
    const check = (AdministrationUnitValidators.EntranceNotEmpty(controlGroup.value.Address, controlGroup.value.Title)) ? true : false;
    return check ? null : {
      EntranceOnceValidator: { valid: false }
    };
}
}
