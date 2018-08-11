import { TestBed, inject } from '@angular/core/testing';

import { BasesettingsService } from './administration-unit-feature.service';

describe('BasesettingsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BasesettingsService]
    });
  });

  it('should be created', inject([BasesettingsService], (service: BasesettingsService) => {
    expect(service).toBeTruthy();
  }));
});
