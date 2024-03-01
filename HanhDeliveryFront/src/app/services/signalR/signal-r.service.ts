import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  constructor() { }

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7147/notificationHub")
    .build();

    this.hubConnection.start()
    .then(() => console.log('Connection with SignalR server started'))
    .catch(err => console.error('Error starting connection with SignalR:', err));
  }

  addNodesNotificationListener(callback: (currentNodeId: number, currentCost: number) => void) {
    this.hubConnection.on('NodesNotification', (currentNodeId: number, currentCost: number) => {
      callback(currentNodeId, currentCost);
    });
  }

  addOrderAcceptedNotificationListener(callback: (orderId: number, earnings: number, cost: number) => void) {
    this.hubConnection.on('OrderAcceptedNotification', (orderId: number, earnings: number, cost: number) => {
      callback(orderId, earnings, cost);
    });
  }
  
}
