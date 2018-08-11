import { AdministrationUnitFeatureModule } from './administration-unit-feature.module';

describe('BasesettingsModule', () => {
  let basesettingsModule: AdministrationUnitFeatureModule;

  beforeEach(() => {
    basesettingsModule = new AdministrationUnitFeatureModule();
  });

  it('should create an instance', () => {
    expect(basesettingsModule).toBeTruthy();
  });
});
