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
  private readonly AUTH_STATE= "unauthorized";
  public authToken!: string|null;
  public authorized!:boolean;
  private readonly AUTH_TOKEN_KEY='authToken';

  constructor(private http: HttpClient) {}

  async login(username: string, password: string):Promise<void> {
    const body = { username, password };
    return await firstValueFrom(this.http.post<any>(this.url, body))
      .then(response => {
        this.setState("authorized");
        this.setAuthToken(response.token);
      })
      .catch(e => {
        throw e;
      })
  };

  logout() {
    this.setAuthToken("");
    this.setState("unauthorized");
  }

  private setAuthToken(token: string): void {
    if(typeof localStorage !== 'undefined')
    {
      localStorage.setItem(this.AUTH_TOKEN_KEY, token);
    }
  }

  private setState(state: string): void {
    if(typeof localStorage !== 'undefined'){
      localStorage.setItem(this.AUTH_STATE, state);
    }
  }

  getAuthToken(): string | null {
    if (typeof localStorage !== undefined){
      return localStorage.getItem(this.AUTH_TOKEN_KEY)
    } else {
      return null
    }
  }

  getAuthState(): string | null {
    if (typeof localStorage !== undefined){
      return localStorage.getItem(this.AUTH_STATE)
    } else {
      return null
    }
  }

  isAuthenticatedUser():boolean {
    this.authorized = (this.getAuthState() == "authorized" && typeof localStorage !== undefined) ? true : false;
    return this.authorized;
  }

  clearLocalStorage():void{
    localStorage.removeItem("AUTH_TOKEN_KEY")
    localStorage.removeItem("AUTH_STATE")
    localStorage.removeItem("graphData")


  }

}
