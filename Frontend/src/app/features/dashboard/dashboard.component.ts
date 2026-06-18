import { Component, inject, computed } from '@angular/core';
import { ApplicationService } from '../../core/services/application.service';
import { CompanyService } from '../../core/services/company.service';
@Component({ selector: 'app-dashboard', standalone: true,
 template: `
 <div class="space-y-6">
 <h1 class="text-2xl font-bold text-gray-800">Dashboard</h1>
 <div class="bg-white rounded-xl border border-gray-200 p-6">
 <h2 class="text-lg font-semibold text-gray-700 mb-4">Application Pipeline</h2>
 @if (appSvc.summary.isLoading()) {
 <p class="text-gray-400 text-sm">Loading...</p>
 } @else if (appSvc.summary.hasValue()) {
 <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
 @for (entry of summaryEntries(); track entry.status) {
 <div class="bg-blue-50 rounded-lg p-4 text-center">
 <p class="text-2xl font-bold text-blue-700">{{ entry.count }}</p>
 <p class="text-sm text-gray-500 mt-1">{{ entry.status }}</p>
 </div>
 }
 </div>
 }
 </div>
 <div class="bg-white rounded-xl border border-gray-200 p-6">
 <p class="text-sm text-gray-500">Companies tracked</p>
 <p class="text-3xl font-bold text-gray-800 mt-1">
 {{ companySvc.companies.hasValue() ? (companySvc.companies.value()?.length ?? 
0) : 0 }}
 </p>
 </div>
 </div>
 `
})
export class DashboardComponent {
 appSvc = inject(ApplicationService);
 companySvc = inject(CompanyService);
 summaryEntries = computed(() => {
 const s = (this.appSvc.summary.value() ?? {}) as Record<string,number>;
 return Object.entries(s).map(([status, count]) => ({ status, count }));
 });
}
