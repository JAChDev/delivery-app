import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeliveryServicesService {

  public grid:object|null=null;

  constructor(private http: HttpClient) {}

  async sendTokenReceiveGrid(token: string):Promise<void> {
    const url:string = `https://localhost:7147/DeliveryAutomation/TokenAndBuild`;
    const body = { token }
    return firstValueFrom(this.http.post<any>(url, body))
      .then(response => {
        if(typeof localStorage !== 'undefined')
        {
          localStorage.setItem('graphData', JSON.stringify(response))
        }
      })
      .catch(e => {
        throw e;
      })
  };

  getGrid():any {
    if(typeof localStorage !== 'undefined')
        {
          const graph:any = localStorage.getItem('graphData');
          return JSON.parse(graph);
        }
  }
}
