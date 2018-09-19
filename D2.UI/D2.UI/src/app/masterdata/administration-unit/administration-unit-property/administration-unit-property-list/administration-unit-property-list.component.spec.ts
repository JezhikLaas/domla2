import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitPropertyListComponent } from './administration-unit-property-list.component';

describe('AdministrationUnitPropertyListComponent', () => {
  let component: AdministrationUnitPropertyListComponent;
  let fixture: ComponentFixture<AdministrationUnitPropertyListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitPropertyListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitPropertyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
