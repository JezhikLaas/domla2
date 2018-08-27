import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubunitCreateComponent } from './subunit-create.component';

describe('SubunitCreateComponent', () => {
  let component: SubunitCreateComponent;
  let fixture: ComponentFixture<SubunitCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubunitCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubunitCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
