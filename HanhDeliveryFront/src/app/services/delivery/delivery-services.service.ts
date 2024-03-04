import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { response } from 'express';
import { Observable, firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { environment as environmentDev } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class DeliveryServicesService {

  public grid:object|null=null;
  public coins:number|null=null;
  private apiUrlAutomation:string;
  private apiUrlSimulation:string;

  constructor(private http: HttpClient) {
    this.apiUrlAutomation = environment.production ? environment.apiUrlAutomation : environmentDev.apiUrlAutomation
    this.apiUrlSimulation = environment.production ? environment.apiUrlSimulation : environmentDev.apiUrlSimulation
  }

  async sendTokenReceiveGrid(token: string|null):Promise<void> {
    const url:string = `${this.apiUrlAutomation}/DeliveryAutomation/TokenAndBuild`;
    const body = { token }
    return await firstValueFrom(this.http.post<any>(url, body))
      .then(response => {
        return response
      })
      .catch(e => {
        throw e;
      })
  };

  async getCredits(token: string|null): Promise<number> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
    const url = `${this.apiUrlSimulation}/User/CoinAmount`;
    try {
      const response = await firstValueFrom(this.http.get<number>(url, httpOptions));
      return response;
    } catch (error) {
      throw error; 
    }
  }

  async startSim(token: string|null):Promise<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
    const url = `${this.apiUrlSimulation}/Sim/Start`;
    await firstValueFrom(this.http.post(url,null,httpOptions))
          .then(() => {console.log("Simulation Started")})
          .catch(e => { throw e })
  }

  async stopSim(token: string|null):Promise<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
    const url = `${this.apiUrlSimulation}/Sim/Stop`;
    await firstValueFrom(this.http.post(url,null,httpOptions))
          .then(() => {console.log("Simulation Stopped")})
          .catch(e => { throw e })
  }

  async getAcceptedOrders(token: string|null):Promise<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
    };
    const url = `${this.apiUrlSimulation}/Order/GetAllAccepted`;
    try {
      const response = await firstValueFrom(this.http.get<number>(url, httpOptions));
      return response;
    } catch (error) {
      throw error; 
    }
  }
}
