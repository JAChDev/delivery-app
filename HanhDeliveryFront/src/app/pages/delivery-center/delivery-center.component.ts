import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SignalRService } from '../../services/signalR/signal-r.service';
import { DeliveryServicesService } from '../../services/delivery/delivery-services.service';

@Component({
  selector: 'app-delivery-center',
  standalone: true,
  imports: [],
  templateUrl: './delivery-center.component.html',
  styleUrl: './delivery-center.component.css'
})
export class DeliveryCenterComponent {
  private grid!:any;
  constructor(private router: Router, private signalRService: SignalRService, private deliveryServices: DeliveryServicesService) {}
  ngOnInit():void {

    this.grid = this.deliveryServices.getGrid();

    this.signalRService.startConnection();
    this.signalRService.addNodesNotificationListener((currentNodeId: number, currentCost: number) => {
      console.log(currentCost + " + " + currentNodeId);
    });
    this.signalRService.addOrderAcceptedNotificationListener((orderId: number, earnings: number, cost: number) => {
      console.log(orderId + " + " + earnings);
    });
  }
}
