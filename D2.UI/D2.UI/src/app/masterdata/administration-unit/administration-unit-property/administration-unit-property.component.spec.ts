import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitPropertyComponent } from './administration-unit-property.component';

describe('AdministrationUnitPropertyComponent', () => {
  let component: AdministrationUnitPropertyComponent;
  let fixture: ComponentFixture<AdministrationUnitPropertyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitPropertyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
