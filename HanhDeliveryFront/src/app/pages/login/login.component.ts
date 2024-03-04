import { Component, HostBinding } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';
import { DeliveryServicesService } from '../../services/delivery/delivery-services.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  submitted: boolean = false;

  constructor(private router: Router, private loginService:LoginService, private deliveryServices:DeliveryServicesService) {}

  async login(): Promise<void> {
    await this.loginService.login(this.username, this.password)
    .then(async () => {
      this.submitted = true;
      const token:any = this.loginService.getAuthToken();
      const grid = await this.deliveryServices.sendTokenReceiveGrid(token);
      setTimeout(()=>{
        this.router.navigate(['/delivery-center'])
      },500)
      
    })
    .catch(error => {
      alert(error.message);
      this.username='';
      this.password='';
    });
    
  }
}
