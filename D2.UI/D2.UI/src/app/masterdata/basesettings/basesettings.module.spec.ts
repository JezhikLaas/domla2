import { BaseSettingsModule } from './basesettings.module';

describe('BasesettingsModule', () => {
  let basesettingsModule: BaseSettingsModule;

  beforeEach(() => {
    basesettingsModule = new BaseSettingsModule();
  });

  it('should create an instance', () => {
    expect(basesettingsModule).toBeTruthy();
  });
});
