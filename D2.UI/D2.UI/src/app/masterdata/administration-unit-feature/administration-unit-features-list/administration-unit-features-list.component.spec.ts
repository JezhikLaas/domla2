import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitFeaturesListComponent } from './administration-unit-features-list.component';

describe('AdministrationUnitFeaturesListComponent', () => {
  let component: AdministrationUnitFeaturesListComponent;
  let fixture: ComponentFixture<AdministrationUnitFeaturesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitFeaturesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitFeaturesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
