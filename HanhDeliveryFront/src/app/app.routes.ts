import { Routes } from '@angular/router';
import {LoginComponent} from './pages/login/login.component'
import {DeliveryCenterComponent} from './pages/delivery-center/delivery-center.component'
import { authGuard } from './guards/auth/auth.guard';

export const routes: Routes = [
    {
        path:'login',
        component:LoginComponent
    },
    {
        path:'delivery-center',
        component:DeliveryCenterComponent,
        canActivate:[authGuard]
    },
    {
        path:'**',
        redirectTo:'login'
    }
];
