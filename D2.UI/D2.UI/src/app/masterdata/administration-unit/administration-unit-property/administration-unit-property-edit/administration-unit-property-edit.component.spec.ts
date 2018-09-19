import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitPropertyEditComponent } from './administration-unit-property-edit.component';

describe('AdministrationUnitPropertyEditComponent', () => {
  let component: AdministrationUnitPropertyEditComponent;
  let fixture: ComponentFixture<AdministrationUnitPropertyEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitPropertyEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitPropertyEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
