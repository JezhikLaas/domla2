import { TestBed, inject } from '@angular/core/testing';

import { MenuDisplayService } from './menu-display.service';

describe('MenuDisplayService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MenuDisplayService]
    });
  });

  it('should be created', inject([MenuDisplayService], (service: MenuDisplayService) => {
    expect(service).toBeTruthy();
  }));
});
