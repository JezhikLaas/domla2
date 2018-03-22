import { TestBed, inject } from '@angular/core/testing';

import { AdministrationUnitService } from './administration-unit.service';

describe('AdministrationUnitService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdministrationUnitService]
    });
  });

  it('should be created', inject([AdministrationUnitService], (service: AdministrationUnitService) => {
    expect(service).toBeTruthy();
  }));
});
