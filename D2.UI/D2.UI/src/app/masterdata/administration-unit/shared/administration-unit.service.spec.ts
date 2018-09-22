import { TestBed, inject, async } from '@angular/core/testing';
import { AdministrationUnitService } from './administration-unit.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AccountService } from '../../../shared/account.service';
import { StorageService } from '../../../shared/storage.service';

describe('AdministrationUnitService', () => {
  /*
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdministrationUnitService]
    });
  });
  */
  beforeEach(() => {
    // 0. set up the test environment
    TestBed.configureTestingModule({
      providers: [
        AdministrationUnitService,
        AccountService,
        StorageService
      ],
      imports: [
        // no more boilerplate code w/ custom providers needed :-)
        HttpClientModule,
        HttpClientTestingModule
      ]
    });
  });


  it('should be created', inject([AdministrationUnitService], (service: AdministrationUnitService) => {
    expect(service).toBeTruthy();
  }));
  it(`should issue a request`,
    // 1. declare as async test since the HttpClient works with Observables
    async(
      // 2. inject HttpClient and HttpTestingController into the test
      inject([HttpClient, HttpTestingController], (http: HttpClient, backend: HttpTestingController) => {
        // 3. send a simple request
        http.get('/foo/bar').subscribe();

        // 4. HttpTestingController supersedes `MockBackend` from the "old" Http package
        // here two, it's significantly less boilerplate code needed to verify an expected request
        backend.expectOne({
          url: '/foo/bar',
          method: 'GET'
        });
      })
    )
  );
});
