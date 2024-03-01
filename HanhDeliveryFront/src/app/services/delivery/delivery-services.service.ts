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
        this.grid = response;
      })
      .catch(e => {
        throw e;
      })
  };

  getGrid():any {
    return this.grid;
  }
}
