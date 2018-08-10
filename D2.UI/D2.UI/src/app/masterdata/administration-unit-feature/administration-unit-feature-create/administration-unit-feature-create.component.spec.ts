import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseSettingEditComponent } from './base-setting-edit.component';

describe('BaseSettingEditComponent', () => {
  let component: BaseSettingEditComponent;
  let fixture: ComponentFixture<BaseSettingEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaseSettingEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseSettingEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
