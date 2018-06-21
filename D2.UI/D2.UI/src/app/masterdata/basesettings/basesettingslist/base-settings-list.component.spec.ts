import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseSettingsListComponent } from './base-settings-list.component';

describe('BaseSettingsListComponent', () => {
  let component: BaseSettingsListComponent;
  let fixture: ComponentFixture<BaseSettingsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaseSettingsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseSettingsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
