import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitsListComponent } from './administration-units.component';

describe('AdministrationUnitsListComponent', () => {
  let component: AdministrationUnitsListComponent;
  let fixture: ComponentFixture<AdministrationUnitsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
