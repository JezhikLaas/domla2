import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntranceEditComponent } from './entrance-edit.component';

describe('EntranceEditComponent', () => {
  let component: EntranceEditComponent;
  let fixture: ComponentFixture<EntranceEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntranceEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntranceEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
