import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { JobRoleService } from '../../../core/services/job-role.service';
@Component({ selector: 'app-job-role-list', standalone: true, imports: [FormsModule],
 template: `
 <div class="space-y-6">
 <div class="flex items-center justify-between">
 <h1 class="text-2xl font-bold text-gray-800">Job Roles</h1>
 <button (click)="showForm.set(!showForm())"
 class="bg-blue-600 text-white px-4 py-2 rounded-lg text-sm hover:bg-blue-700">+
Add Job Role</button>
 </div>
 @if (showForm()) {  
 <div class="bg-white border border-gray-200 rounded-xl p-6 space-y-3">
 <input [(ngModel)]="title" placeholder="Job title"
 class="w-full border rounded-lg px-3 py-2 text-sm" />
 <input [(ngModel)]="jobUrl" placeholder="Job URL"
 class="w-full border rounded-lg px-3 py-2 text-sm" />
 <input [(ngModel)]="jobDescription" placeholder="Job description"
 class="w-full border rounded-lg px-3 py-2 text-sm" />
 <input [(ngModel)]="companyId" placeholder="Company ID"
 class="w-full border rounded-lg px-3 py-2 text-sm" />
 <select [(ngModel)]="tier" class="w-full border rounded-lg px-3 py-2 text-sm">
 <option>Dream</option><option>Stretch</option><option>Safe</option>
 </select>
 <button (click)="create()"
 class="bg-green-600 text-white px-4 py-2 rounded-lg text-sm">Save</button>
 </div>
 }
 @if (svc.jobRoles.isLoading()) { <p class="text-gray-400 text-sm">Loading...</p> }
 @else if (svc.jobRoles.hasValue()) {
 <div class="space-y-3">
 @for (r of svc.jobRoles.value(); track r.id) {
 <div class="bg-white border border-gray-200 rounded-xl p-4 flex items-center 
justify-between">
 <div>
 <p class="font-medium text-gray-800">{{ r.title }}</p>
 <p class="text-xs text-gray-400">ATS Score: {{ r.atsScore }}</p>
 </div>
 <div class="flex gap-2 items-center">
 <span [class]="tierBadge(r.tier)">{{ r.tier }}</span>
 @if (!r.isApplied) {
 <button (click)="apply(r.id)"
 class="text-xs bg-green-50 text-green-700 px-3 py-1 rounded hover:bg green-100">Apply</button>
 } @else {
 <span class="text-xs bg-green-100 text-green-700 px-2 py-0.5 
rounded">Applied</span>
 }
 <button (click)="delete(r.id)" class="text-xs text-red-600 
hover:underline">Delete</button>
 </div>
 </div>
 }
 </div>
 }
 </div>
 `
})
export class JobRoleListComponent {

 svc = inject(JobRoleService);
 showForm = signal(false); title = ''; jobUrl = ''; jobDescription = ''; companyId = '';
tier = 'Stretch';
 
create() {
 this.svc.create({ title: this.title, jobUrl: this.jobUrl, jobDescription: 
this.jobDescription, tier: this.tier, companyId: this.companyId })
 .subscribe(() => { this.svc.jobRoles.reload(); this.showForm.set(false); });
 }
 apply(id: string) { this.svc.apply(id).subscribe(() => this.svc.jobRoles.reload()); }
 delete(id: string) { this.svc.delete(id).subscribe(() => this.svc.jobRoles.reload()); }

 tierBadge(t: string) {
  const badges: Record<string, string> = {
    Dream:   'bg-purple-100 text-purple-700 px-2 py-0.5 rounded text-xs',
    Stretch: 'bg-blue-100 text-blue-700 px-2 py-0.5 rounded text-xs',
    Safe:    'bg-green-100 text-green-700 px-2 py-0.5 rounded text-xs',
  };
  return badges[t] ?? 'bg-gray-100 text-gray-700 px-2 py-0.5 rounded text-xs';
}
}
