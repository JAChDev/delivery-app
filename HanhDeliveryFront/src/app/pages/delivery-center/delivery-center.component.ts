import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delivery-center',
  standalone: true,
  imports: [],
  templateUrl: './delivery-center.component.html',
  styleUrl: './delivery-center.component.css'
})
export class DeliveryCenterComponent {
  constructor(private router: Router) {}
}
