import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogAdministrationUnitsListComponent } from './dialog-administration-units-list.component';

describe('DialogAdministrationUnitsListComponent', () => {
  let component: DialogAdministrationUnitsListComponent;
  let fixture: ComponentFixture<DialogAdministrationUnitsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogAdministrationUnitsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogAdministrationUnitsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
