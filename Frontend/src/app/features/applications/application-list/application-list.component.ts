import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApplicationService } from '../../../core/services/application.service';
const NEXT: Record<string,string> = {
 Applied: 'HRScreen', HRScreen: 'TechRound1', TechRound1: 'TechRound2',
 TechRound2: 'Final', Final: 'Offered'
};
@Component({ selector: 'app-application-list', standalone: true, imports: [FormsModule],
 template: `
 <div class="space-y-6">
 <div class="flex items-center justify-between">
 <h1 class="text-2xl font-bold text-gray-800">Applications</h1>
 <button (click)="showForm.set(!showForm())"
 class="bg-blue-600 text-white px-4 py-2 rounded-lg text-sm hover:bg-blue-700">+
Add Application</button>
 </div>
 @if (showForm()) {
 <div class="bg-white border border-gray-200 rounded-xl p-6 space-y-3">
 <input [(ngModel)]="jobRoleId" placeholder="Job Role ID"
 class="w-full border rounded-lg px-3 py-2 text-sm" />
 <button (click)="create()" class="bg-green-600 text-white px-4 py-2 rounded-lg 
text-sm">Save</button>
 </div>
 }
 @if (svc.applications.isLoading()) { <p class="text-gray-400 
text-sm">Loading...</p> }
 @else if (svc.applications.hasValue()) {
 <div class="space-y-3">
 @for (a of svc.applications.value(); track a.id) {
 <div class="bg-white border border-gray-200 rounded-xl p-4 flex items-center 
justify-between">
 <div>
 <span [class]="statusBadge(a.status)">{{ a.status }}</span>
 <p class="text-xs text-gray-400 mt-1">{{ a.jobRoleId }}</p>
 </div>
 <div class="flex gap-2">
 @if (next(a.status)) {
 <button (click)="move(a.id, next(a.status)!)"
 class="text-xs bg-blue-50 text-blue-700 px-3 py-1 rounded hover:bg blue-100">
 Move to {{ next(a.status) }}
 </button>
 }
 <button (click)="move(a.id, 'Rejected')"
 class="text-xs bg-red-50 text-red-600 px-3 py-1 rounded hover:bg-red 100">Reject</button>
 <button (click)="delete(a.id)" class="text-xs text-gray-400 hover:text red-500">Delete</button>
 </div>
 </div>
 }
 </div>
 }
 </div>
 `
})
export class ApplicationListComponent {
 svc = inject(ApplicationService);
 showForm = signal(false); jobRoleId = '';
 create() {
 this.svc.create(this.jobRoleId)
 .subscribe(() => { this.svc.applications.reload(); this.showForm.set(false); 
this.jobRoleId = ''; });
 }
 move(id: string, status: string) { this.svc.move(id, status).subscribe(() => 
this.svc.applications.reload()); }
 delete(id: string) { this.svc.delete(id).subscribe(() => 
this.svc.applications.reload()); }
 next(s: string): string | null { return NEXT[s] ?? null; }
 statusBadge(s: string) {
  const badges: Record<string, string> = {
    Applied:    'bg-blue-100 text-blue-700 px-2 py-0.5 rounded text-xs',
    HRScreen:   'bg-yellow-100 text-yellow-700 px-2 py-0.5 rounded text-xs',
    TechRound1: 'bg-orange-100 text-orange-700 px-2 py-0.5 rounded text-xs',
    TechRound2: 'bg-purple-100 text-purple-700 px-2 py-0.5 rounded text-xs',
    Final:      'bg-indigo-100 text-indigo-700 px-2 py-0.5 rounded text-xs',
    Offered:    'bg-green-100 text-green-700 px-2 py-0.5 rounded text-xs',
    Rejected:   'bg-red-100 text-red-700 px-2 py-0.5 rounded text-xs',
  };
  return badges[s] ?? 'bg-gray-100 text-gray-600 px-2 py-0.5 rounded text-xs';
}
}