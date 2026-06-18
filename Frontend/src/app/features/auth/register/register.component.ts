import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
@Component({ selector: 'app-register', standalone: true, imports: [FormsModule, 
RouterLink],
 template: `
 <div class="min-h-screen bg-gray-50 flex items-center justify-center">
 <div class="bg-white p-8 rounded-xl shadow w-full max-w-md">
 <h1 class="text-2xl font-bold text-gray-800 mb-6">Create account</h1>
 @if (error()) {
 <div class="bg-red-50 text-red-600 px-4 py-2 rounded mb-4 textsm">{{ error() }}</div>
 }
 <form (ngSubmit)="register()">
 <input [(ngModel)]="displayName" name="displayName" placeholder="Display Name"
 class="w-full border rounded-lg px-3 py-2 text-sm mb-3" />
 <input type="email" [(ngModel)]="email" name="email" placeholder="Email"
 class="w-full border rounded-lg px-3 py-2 text-sm mb-3" />
 <input type="password" [(ngModel)]="password" name="password" 
placeholder="Password"
 class="w-full border rounded-lg px-3 py-2 text-sm mb-6" />
 <button type="submit" [disabled]="loading()"
 class="w-full bg-blue-600 text-white py-2 rounded-lg font-medium hover:bgblue-700 disabled:opacity-50">
 {{ loading() ? "Creating..." : "Create account" }}
 </button>
 </form>
 <p class="text-center text-sm text-gray-500 mt-4">
 Have account? <a routerLink="/login" class="text-blue-600 hover:underline">Sign
in</a>
 </p>
 </div>
 </div>
 `
})
export class RegisterComponent {
 private auth = inject(AuthService); private router = inject(Router);
 displayName = ''; email = ''; password = '';
 loading = signal(false); error = signal('');
 register() {
 this.loading.set(true); this.error.set('');
 this.auth.register({ email: this.email, password: this.password, displayName: 
this.displayName }).subscribe({
 next: res => { this.auth.saveToken(res); this.router.navigate(['/dashboard']); },
 error: () => { this.error.set('Registration failed.'); this.loading.set(false); }
 });
 }
}
