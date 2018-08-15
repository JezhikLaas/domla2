import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitFeaturesListViewComponent } from './administration-unit-features-list-view.component';

describe('AdministrationUnitFeaturesListViewComponent', () => {
  let component: AdministrationUnitFeaturesListViewComponent;
  let fixture: ComponentFixture<AdministrationUnitFeaturesListViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitFeaturesListViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitFeaturesListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
