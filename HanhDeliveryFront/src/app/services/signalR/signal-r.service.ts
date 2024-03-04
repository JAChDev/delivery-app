import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { environment } from '../../../environments/environment';
import { environment as environmentDev } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  private apiUrlAutomation: string;

  constructor() { 
    this.apiUrlAutomation = environment.production ? environment.apiUrlAutomation : environmentDev.apiUrlAutomation
  }

  startConnection() {
    const url = `${this.apiUrlAutomation}/notificationHub`
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(url)
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
