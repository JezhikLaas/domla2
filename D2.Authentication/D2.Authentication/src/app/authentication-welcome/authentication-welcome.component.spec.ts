import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthenticationWelcomeComponent } from './authentication-welcome.component';

describe('AuthenticationWelcomeComponent', () => {
  let component: AuthenticationWelcomeComponent;
  let fixture: ComponentFixture<AuthenticationWelcomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthenticationWelcomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthenticationWelcomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
