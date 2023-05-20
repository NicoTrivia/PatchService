import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestPatchComponent } from './request-patch.component';

describe('RequestPatchComponent', () => {
  let component: RequestPatchComponent;
  let fixture: ComponentFixture<RequestPatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RequestPatchComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequestPatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
