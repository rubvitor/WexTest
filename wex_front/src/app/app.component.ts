import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <main class="shell">
      <header><h1>WEX Purchase Currency</h1><nav><a routerLink="/purchase">Create Purchase</a><a routerLink="/conversion">Convert Purchase</a></nav></header>
      <router-outlet />
    </main>`
})
export class AppComponent {}
