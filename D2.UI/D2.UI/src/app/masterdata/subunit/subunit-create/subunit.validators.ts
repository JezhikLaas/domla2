import { FormGroup } from '@angular/forms';

export class SubUnitValidators {
  static FloorIfWithEntrance(subUnitGroup: FormGroup): { [error: string]: any } {
    let check = true;
    if (subUnitGroup.value.Entrance) {
      check = subUnitGroup.value.Floor ? true : false;
    }
     return check ? null : {
        floorRequired: { valid: false }
      };
  }

  static TypeIfWithEntrance(subUnitGroup: FormGroup): { [error: string]: any } {
    let check = true;
    if (!subUnitGroup.value.Entrance) {
      check = subUnitGroup.value.Type ? true : false;
    }
    return check ? null : {
      typeRequired: { valid: false }
    };
  }
}
