import { TestBed, inject } from '@angular/core/testing';

import { DomlaResolverService } from './domla-resolver.service';

describe('DomlaResolverService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DomlaResolverService]
    });
  });

  it('should be created', inject([DomlaResolverService], (service: DomlaResolverService) => {
    expect(service).toBeTruthy();
  }));
});
