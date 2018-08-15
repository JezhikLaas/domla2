import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitsListViewComponent } from './administration-units-list-view.component';

describe('AdministrationUnitsListViewComponent', () => {
  let component: AdministrationUnitsListViewComponent;
  let fixture: ComponentFixture<AdministrationUnitsListViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitsListViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitsListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
