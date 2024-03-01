import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { error } from 'console';
import { response } from 'express';
import { Observable, firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService{
  private url:string = "https://localhost:7115/User/Login";
  private authenticated: boolean = false;
  public authToken:string|null=null;

  constructor(private http: HttpClient) {}

  async login(username: string, password: string):Promise<void> {
    const body = { username, password };
    return firstValueFrom(this.http.post<any>(this.url, body))
      .then(response => {
        this.authenticated=true;
        this.authToken = response.token;
      })
      .catch(e => {
        throw e;
      })
  };

  logout() {
    this.authenticated = false;
    this.authToken = null;
  }

  isAuthenticatedUser():boolean {
    return this.authenticated;
  }

  getAuthToken():string|null {
    return this.authToken;
  }

}
