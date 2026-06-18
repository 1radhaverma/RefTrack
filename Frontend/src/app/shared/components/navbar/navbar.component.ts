import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { SignalRService } from '../../../core/services/signalr.service';
@Component({ selector: 'app-navbar', standalone: true, imports: [RouterLink, 
RouterLinkActive],
 template: `
 <nav class="bg-white border-b border-gray-200 px-6 py-3 flex items-center justifybetween">
 <span class="text-blue-700 font-bold text-lg">RefTrack</span>
 @if (auth.isLoggedIn()) {
 <div class="flex gap-6 text-sm font-medium text-gray-600">
 <a routerLink="/dashboard" routerLinkActive="text-blue-600" 
class="hover:text-blue-600">Dashboard</a>
 <a routerLink="/companies" routerLinkActive="text-blue-600" 
class="hover:text-blue-600">Companies</a>
 <a routerLink="/jobroles" routerLinkActive="text-blue-600" 
class="hover:text-blue-600">Job Roles</a>
 <a routerLink="/referrers" routerLinkActive="text-blue-600" 
class="hover:text-blue-600">Referrers</a>
 <a routerLink="/applications" routerLinkActive="text-blue-600" 
class="hover:text-blue-600">Applications</a>
 </div>
 <button (click)="auth.logout()" class="text-sm text-gray-500 hover:text-red500">Logout</button>
 }
 </nav>
 @if (signalR.reminder()) {
 <div class="fixed top-4 right-4 bg-yellow-50 border border-yellow-300 text-yellow800 px-4 py-3 rounded-lg shadow z-50 text-sm max-w-xs">
 <p class="font-medium">Follow-up Reminder</p>
 <p>{{ signalR.reminder()!.message }}</p>
 <button (click)="signalR.reminder.set(null)" class="text-xs underline mt-
1">Dismiss</button>
 </div>
 }
 `
})
export class NavbarComponent {
 auth = inject(AuthService);
 signalR = inject(SignalRService);
}
