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

  addNodesNotificationListener(callback: (bestPath:any[], orderId:number, cost:number, earnings:number) => void) {
    this.hubConnection.on('NodesNotification', (bestPath:any[],orderId:number,cost:number, earnings:number) => {
      callback(bestPath, orderId,cost,earnings);
    });
  }
  
}
