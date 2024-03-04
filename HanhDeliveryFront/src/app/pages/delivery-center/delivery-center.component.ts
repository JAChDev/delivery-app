import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';
import { SignalRService } from '../../services/signalR/signal-r.service';
import { DeliveryServicesService } from '../../services/delivery/delivery-services.service';
import { GraphcomponentComponent } from '../../components/graphcomponent/graphcomponent.component';

@Component({
  selector: 'app-delivery-center',
  standalone: true,
  imports: [CommonModule, FormsModule, GraphcomponentComponent],
  templateUrl: './delivery-center.component.html',
  styleUrl: './delivery-center.component.css'
})
export class DeliveryCenterComponent implements OnInit, OnDestroy{

  @ViewChild(GraphcomponentComponent) graphComponent!:GraphcomponentComponent;

  private grid!:any;
  activeSim:boolean=false;
  notificationQueue: any[] = [];
  coins!:number|null;


  constructor(private router: Router, private signalRService: SignalRService, 
              private deliveryServices: DeliveryServicesService, private loginServices:LoginService) {}

  ngOnInit():void {    
    this.deliveryServices.getCredits(this.loginServices.getAuthToken()).then(r => {
      this.coins = r
    });
    this.signalRService.startConnection();
    this.subscribeListener();
  }

  ngOnDestroy():void{
    this.loginServices.clearLocalStorage();
  }

  startSim = () => {
    this.deliveryServices.startSim(this.loginServices.getAuthToken()).then(()=>{
      this.activeSim = true;
    })
  }

  stopSim = () => {
    this.deliveryServices.stopSim(this.loginServices.getAuthToken()).then(()=>{
      this.activeSim = false;
    })
  }

  private subscribeListener() {
    this.signalRService.addNodesNotificationListener((bestPath:any[], orderId:number, cost:number, earnings:number) => {
      this.notificationQueue.push({orderId,bestPath,cost,earnings});
    });
  }

  highLightGraph(id:any): void {
    const highlightPath = this.notificationQueue.find((e:any)=>e.orderId == id)
    this.graphComponent.highlightPath(highlightPath.bestPath);
    this.deliveryServices.getCredits(this.loginServices.getAuthToken()).then(r => {
      this.coins = r
    });
  }

}
