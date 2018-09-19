import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntrancesListComponent } from './entrances-list.component';

describe('EntrancesListComponent', () => {
  let component: EntrancesListComponent;
  let fixture: ComponentFixture<EntrancesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntrancesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntrancesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
