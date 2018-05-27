import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvidePasswordComponent } from './provide-password.component';

describe('ProvidePasswordComponent', () => {
  let component: ProvidePasswordComponent;
  let fixture: ComponentFixture<ProvidePasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProvidePasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvidePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
