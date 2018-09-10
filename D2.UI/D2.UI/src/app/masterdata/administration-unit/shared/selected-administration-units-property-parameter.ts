import {AdministrationUnitFeature} from '../../shared/administration-unit-feature';

export class SelectedAdministrationUnitsPropertyParameter {
  constructor (
    public AdministrationUnitsFeatureParameters: AdministrationUnitFeature,
    public AdministrationUnitIds: string[]
  )  {}
}
