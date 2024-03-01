import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { SignalRService } from '../../services/signalR/signal-r.service';
import { DeliveryServicesService } from '../../services/delivery/delivery-services.service';
import { GraphcomponentComponent } from '../../components/graphcomponent/graphcomponent.component';

@Component({
  selector: 'app-delivery-center',
  standalone: true,
  imports: [CommonModule, GraphcomponentComponent],
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

    });
    this.signalRService.addOrderAcceptedNotificationListener((orderId: number, earnings: number, cost: number) => {

    });
  }



}
