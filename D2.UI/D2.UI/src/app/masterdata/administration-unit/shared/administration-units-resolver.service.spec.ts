import { TestBed, inject } from '@angular/core/testing';

import { AdministrationUnitsResolver } from './administration-units-resolver.service';

describe('DomlaResolverService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdministrationUnitsResolver]
    });
  });

  it('should be created', inject([AdministrationUnitsResolver], (service: AdministrationUnitsResolver) => {
    expect(service).toBeTruthy();
  }));
});
