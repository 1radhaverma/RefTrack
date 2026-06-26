import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReferrerService } from '../../../core/services/referrer.service';
 
@Component({
  selector: 'app-referrer-list',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold text-gray-800">Referrers</h1>
        <button (click)="showForm.set(!showForm())"
          class="bg-blue-600 text-white px-4 py-2 rounded-lg text-sm hover:bg-blue-700">
          + Add Referrer
        </button>
      </div>
 
      @if (showForm()) {
        <div class="bg-white border border-gray-200 rounded-xl p-6 space-y-3">
          <input [(ngModel)]="name" placeholder="Name"
            class="w-full border rounded-lg px-3 py-2 text-sm" />
          <input [(ngModel)]="linkedInUrl" placeholder="LinkedIn URL"
            class="w-full border rounded-lg px-3 py-2 text-sm" />
          <input [(ngModel)]="designation" placeholder="Designation"
            class="w-full border rounded-lg px-3 py-2 text-sm" />
          <input [(ngModel)]="jobRoleId" placeholder="Job Role ID"
            class="w-full border rounded-lg px-3 py-2 text-sm" />
          <button (click)="create()"
            class="bg-green-600 text-white px-4 py-2 rounded-lg text-sm">Save</button>
        </div>
      }
 
      @if (svc.referrers.isLoading()) {
        <p class="text-gray-400 text-sm">Loading...</p>
      } @else if (svc.referrers.hasValue()) {
        <div class="space-y-3">
          @for (r of svc.referrers.value() ?? []; track r.id) {
            <div class="bg-white border border-gray-200 rounded-xl p-4 flex items-center justify-between">
              <div>
                <p class="font-medium text-gray-800">{{ r.name }}</p>
                <p class="text-xs text-gray-400">{{ r.designation }}</p>
              </div>
              <div class="flex items-center gap-2">
                <span [class]="statusBadge(r.status)">{{ r.status }}</span>
                <button (click)="contact(r.id)"
                  class="text-xs bg-blue-50 text-blue-700 px-2 py-1 rounded hover:bg-blue-100">
                  Contact
                </button>
                <button (click)="replied(r.id)"
                  class="text-xs bg-green-50 text-green-700 px-2 py-1 rounded hover:bg-green-100">
                  Replied
                </button>
                <button (click)="referred(r.id)"
                  class="text-xs bg-purple-50 text-purple-700 px-2 py-1 rounded hover:bg-purple-100">
                  Referred
                </button>
                <button (click)="ghosted(r.id)"
                  class="text-xs bg-gray-50 text-gray-500 px-2 py-1 rounded hover:bg-gray-100">
                  Ghosted
                </button>
              </div>
            </div>
          }
        </div>
      }
    </div>
  `
})
export class ReferrerListComponent {
  svc = inject(ReferrerService);
 
  showForm = signal(false);
  name = '';
  linkedInUrl = '';
  designation = '';
  jobRoleId = '';
 
  create() {
    this.svc.create({
      name: this.name,
      linkedInUrl: this.linkedInUrl,
      designation: this.designation,
      jobRoleId: this.jobRoleId
    }).subscribe(() => {
      this.svc.referrers.reload();
      this.showForm.set(false);
      this.name = '';
      this.linkedInUrl = '';
      this.designation = '';
      this.jobRoleId = '';
    });
  }
 
  contact(id: string)  { this.svc.contact(id).subscribe(() => this.svc.referrers.reload()); }
  replied(id: string)  { this.svc.replied(id).subscribe(() => this.svc.referrers.reload()); }
  referred(id: string) { this.svc.referred(id).subscribe(() => this.svc.referrers.reload()); }
  ghosted(id: string)  { this.svc.ghosted(id).subscribe(() => this.svc.referrers.reload()); }
 
  statusBadge(s: string): string {
    if (s === 'NotContacted') return 'bg-gray-100 text-gray-600 px-2 py-0.5 rounded text-xs';
    if (s === 'Sent')         return 'bg-blue-100 text-blue-700 px-2 py-0.5 rounded text-xs';
    if (s === 'Replied')      return 'bg-green-100 text-green-700 px-2 py-0.5 rounded text-xs';
    if (s === 'Referred')     return 'bg-purple-100 text-purple-700 px-2 py-0.5 rounded text-xs';
    if (s === 'Ghosted')      return 'bg-red-100 text-red-700 px-2 py-0.5 rounded text-xs';
    return 'bg-gray-100 text-gray-600 px-2 py-0.5 rounded text-xs';
  }
}