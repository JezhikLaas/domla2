import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoutUserComponent } from './logout-user.component';

describe('LogoutUserComponent', () => {
  let component: LogoutUserComponent;
  let fixture: ComponentFixture<LogoutUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogoutUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogoutUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
