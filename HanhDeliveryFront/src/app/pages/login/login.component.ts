import { Component, HostBinding } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from '../../services/login/login.service';

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

  constructor(private router: Router, private loginService:LoginService) {}

  login(): void {
    this.loginService.login(this.username, this.password)
    .then(() => {
      this.submitted = true;
      setTimeout(()=>{
        this.router.navigate(['/delivery-center'])
      },1000)
    })
    .catch(error => {
      alert(error.message);
      this.username='';
      this.password='';
    });  
  }
}
