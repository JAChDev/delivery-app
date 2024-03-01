import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraphcomponentComponent } from './graphcomponent.component';

describe('GraphcomponentComponent', () => {
  let component: GraphcomponentComponent;
  let fixture: ComponentFixture<GraphcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraphcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GraphcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
