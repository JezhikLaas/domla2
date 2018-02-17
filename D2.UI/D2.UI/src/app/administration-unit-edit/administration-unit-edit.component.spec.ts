import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationUnitEditComponent } from './administration-unit-edit.component';

describe('AdministrationUnitEditComponent', () => {
  let component: AdministrationUnitEditComponent;
  let fixture: ComponentFixture<AdministrationUnitEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationUnitEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationUnitEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
