import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { SignalRService } from '../../../core/services/signalr.service';
@Component({ selector: 'app-login', standalone: true, imports: [FormsModule, RouterLink],
 template: `
 <div class="min-h-screen bg-gray-50 flex items-center justify-center">
 <div class="bg-white p-8 rounded-xl shadow w-full max-w-md">
 <h1 class="text-2xl font-bold text-gray-800 mb-6">Sign in to RefTrack</h1>
 @if (error()) {
 <div class="bg-red-50 text-red-600 px-4 py-2 rounded mb-4 text sm">{{ error() }}</div>
 }
 <form (ngSubmit)="login()">
 <label class="block text-sm font-medium text-gray-700 mb-1">Email</label>
 <input type="email" [(ngModel)]="email" name="email" required
 class="w-full border rounded-lg px-3 py-2 text-sm mb-4 focus:outline-none 
focus:ring-2 focus:ring-blue-500" />
 <label class="block text-sm font-medium text-gray-700 mb-1">Password</label>
 <input type="password" [(ngModel)]="password" name="password" required
 class="w-full border rounded-lg px-3 py-2 text-sm mb-6 focus:outline-none 
focus:ring-2 focus:ring-blue-500" />
 <button type="submit" [disabled]="loading()"
 class="w-full bg-blue-600 text-white py-2 rounded-lg font-medium hover:bgblue-700 disabled:opacity-50">
 {{ loading() ? "Signing in..." : "Sign in" }}
 </button>
 </form>
 <p class="text-center text-sm text-gray-500 mt-4">
 No account? <a routerLink="/register" class="text-blue-600 
hover:underline">Register</a>
 </p>
 </div>
 </div>
 `
})
export class LoginComponent {
 private auth = inject(AuthService);
 private signalR = inject(SignalRService);
 private router = inject(Router);
 email = ''; password = '';
 loading = signal(false); error = signal('');
 login() {
 this.loading.set(true); this.error.set('');
 this.auth.login({ email: this.email, password: this.password }).subscribe({
 next: res => { this.auth.saveToken(res); this.signalR.connect(res.token); 
this.router.navigate(['/dashboard']); },
 error: () => { this.error.set('Invalid email or password.'); 
this.loading.set(false); }
 });
 }
}