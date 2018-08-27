import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubunitListViewComponent } from './subunit-list-view.component';

describe('SubunitListViewComponent', () => {
  let component: SubunitListViewComponent;
  let fixture: ComponentFixture<SubunitListViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubunitListViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubunitListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
