import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitFeatureCreateComponent } from './administration-unit-feature-create.component';

describe('AdministrationUnitFeatureCreateComponent', () => {
  let component: AdministrationUnitFeatureCreateComponent;
  let fixture: ComponentFixture<AdministrationUnitFeatureCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitFeatureCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitFeatureCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
