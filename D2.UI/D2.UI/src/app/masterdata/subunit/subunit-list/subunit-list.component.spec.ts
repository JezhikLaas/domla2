import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubunitListComponent } from './subunit-list.component';

describe('SubunitListComponent', () => {
  let component: SubunitListComponent;
  let fixture: ComponentFixture<SubunitListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubunitListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubunitListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
