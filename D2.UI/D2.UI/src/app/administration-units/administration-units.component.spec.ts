import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitsComponent } from './administration-units.component';

describe('AdministrationUnitsComponent', () => {
  let component: AdministrationUnitsComponent;
  let fixture: ComponentFixture<AdministrationUnitsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
