import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
 private http = inject(HttpClient);
 private router = inject(Router);
 private base = 'https://localhost:7288/api/auth';
 isLoggedIn = signal(!!localStorage.getItem('reftrack_token'));
 login(req: LoginRequest) {
 return this.http.post<AuthResponse>(this.base + '/login', req);
 }
 register(req: RegisterRequest) {
 return this.http.post<AuthResponse>(this.base + '/register', req);
 }
 saveToken(res: AuthResponse) {
    console.log('reftrack_token', res.token);
 localStorage.setItem('reftrack_token', res.token);
 localStorage.setItem('reftrack_userId', res.userId);
 this.isLoggedIn.set(true);
 }
 logout() {
 localStorage.removeItem('reftrack_token');
 localStorage.removeItem('reftrack_userId');
 this.isLoggedIn.set(false);
 this.router.navigate(['/login']);
 }
 getUserId() { return localStorage.getItem('reftrack_userId') ?? ''; }
}