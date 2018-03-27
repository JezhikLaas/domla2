import { TestBed, inject } from '@angular/core/testing';

import { AdminUnitResolver } from './administration-unit-resolver.service';

describe('AdminUnitResolver', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdminUnitResolver]
    });
  });

  it('should be created', inject([AdminUnitResolver], (service: AdminUnitResolver) => {
    expect(service).toBeTruthy();
  }));
});
