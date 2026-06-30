import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CompanyService } from '../../../core/services/company.service';
 
@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold text-gray-800">Companies</h1>
        <button (click)="showForm.set(!showForm())"
          class="bg-blue-600 text-white px-4 py-2 rounded-lg text-sm hover:bg-blue-700">
          + Add Company
        </button>
      </div>
 
      @if (showForm()) {
        <div class="bg-white border border-gray-200 rounded-xl p-6 space-y-3">
          <input [(ngModel)]="name" placeholder="Company name"
            class="w-full border rounded-lg px-3 py-2 text-sm" />
          <input [(ngModel)]="careerPageUrl" placeholder="Career page URL"
            class="w-full border rounded-lg px-3 py-2 text-sm" />
          <select [(ngModel)]="tier" class="w-full border rounded-lg px-3 py-2 text-sm">
            <option>Dream</option>
            <option>Stretch</option>
            <option>Safe</option>
          </select>
          <button (click)="create()"
            class="bg-green-600 text-white px-4 py-2 rounded-lg text-sm">Save</button>
        </div>
      }
 
      @if (svc.companies.isLoading()) {
        <p class="text-gray-400 text-sm">Loading...</p>
      } @else if (svc.companies.hasValue()) {
        <div class="bg-white rounded-xl border border-gray-200 overflow-hidden">
          <table class="w-full text-sm">
            <thead class="bg-gray-50 border-b border-gray-200">
              <tr>
                <th class="text-left px-4 py-3 text-gray-600 font-medium">Name</th>
                <th class="text-left px-4 py-3 text-gray-600 font-medium">Tier</th>
                <th class="text-left px-4 py-3 text-gray-600 font-medium">Status</th>
                <th class="text-left px-4 py-3 text-gray-600 font-medium">Actions</th>
              </tr>
            </thead>
            <tbody>
              @for (c of svc.companies.value(); track c.id) {
                <tr class="border-b border-gray-100 hover:bg-gray-50">
                  <td class="px-4 py-3 font-medium">{{ c.name }}</td>
                  <td class="px-4 py-3">
                    <span [class]="tierBadge(c.tier)">{{ c.tier }}</span>
                  </td>
                  <td class="px-4 py-3">
                    @if (c.isBlacklisted) {
                      <span class="bg-red-100 text-red-700 px-2 py-0.5 rounded text-xs">Blacklisted</span>
                    } @else {
                      <span class="bg-green-100 text-green-700 px-2 py-0.5 rounded text-xs">Active</span>
                    }
                  </td>
                  <td class="px-4 py-3 space-x-2">
                    @if (!c.isBlacklisted) {
                      <button (click)="blacklist(c.id)"
                        class="text-xs text-orange-600 hover:underline">Blacklist</button>
                    }
                    <button (click)="delete(c.id)"
                      class="text-xs text-red-600 hover:underline">Delete</button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }
    </div>
  `
})
export class CompanyListComponent {
  svc = inject(CompanyService);
 
  showForm = signal(false);
  name = '';
  careerPageUrl = '';
  tier = 'Stretch';
 
  create() {
    this.svc.create({
      name: this.name,
      careerPageUrl: this.careerPageUrl,
      tier: this.tier
    }).subscribe(() => {
      this.svc.companies.reload();
      this.showForm.set(false);
      this.name = '';
      this.careerPageUrl = '';
    });
  }
 
  blacklist(id: string) {
    this.svc.blacklist(id).subscribe(() => this.svc.companies.reload());
  }
 
  delete(id: string) {
    this.svc.delete(id).subscribe(() => this.svc.companies.reload());
  }
 
  tierBadge(t: string): string {
    if (t === 'Dream')   return 'bg-purple-100 text-purple-700 px-2 py-0.5 rounded text-xs';
    if (t === 'Stretch') return 'bg-blue-100 text-blue-700 px-2 py-0.5 rounded text-xs';
    if (t === 'Safe')    return 'bg-green-100 text-green-700 px-2 py-0.5 rounded text-xs';
    return 'bg-gray-100 text-gray-700 px-2 py-0.5 rounded text-xs';
  }
}
