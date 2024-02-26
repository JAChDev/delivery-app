import { CanActivateFn,  Router } from '@angular/router';
import { inject } from '@angular/core';
import { LoginService } from '../../services/login/login.service';



export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(LoginService);
  const router = inject(Router);
  if(authService.isAuthenticatedUser()) {
    return true;
  }else{
    router.navigate(['/login']);
    return false;
  }
};
