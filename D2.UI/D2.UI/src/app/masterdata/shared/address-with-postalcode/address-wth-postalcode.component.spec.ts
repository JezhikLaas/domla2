import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressWithPostalcodeComponent } from './address-with-postalcode.components';

describe('AddressWithPostalcode', () => {
  let component: AddressWithPostalcodeComponent;
  let fixture: ComponentFixture<AddressWithPostalcodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddressWithPostalcodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddressWithPostalcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
