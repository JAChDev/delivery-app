import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeliveryCenterComponent } from './delivery-center.component';

describe('DeliveryCenterComponent', () => {
  let component: DeliveryCenterComponent;
  let fixture: ComponentFixture<DeliveryCenterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeliveryCenterComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeliveryCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
