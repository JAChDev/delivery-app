import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersListComponentComponent } from './orders-list-component.component';

describe('OrdersListComponentComponent', () => {
  let component: OrdersListComponentComponent;
  let fixture: ComponentFixture<OrdersListComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdersListComponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OrdersListComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
