import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoaderComponent } from './shared/components/loader/loader.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, LoaderComponent],
  template: `
    <router-outlet></router-outlet>
    <app-loader></app-loader>
  `
})
export class AppComponent {
  title = 'Ordering System Dashboard';
}

