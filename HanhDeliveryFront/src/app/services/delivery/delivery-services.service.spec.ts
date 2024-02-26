import { TestBed } from '@angular/core/testing';

import { DeliveryServicesService } from './delivery-services.service';

describe('DeliveryServicesService', () => {
  let service: DeliveryServicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeliveryServicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
