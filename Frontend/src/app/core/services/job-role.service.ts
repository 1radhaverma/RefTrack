import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { httpResource } from '@angular/common/http';
import { CreateJobRoleRequest } from '../models/job-role.model';
@Injectable({ providedIn: 'root' })
export class JobRoleService {
 private http = inject(HttpClient);
 private base = 'https://localhost:7288/api/jobroles';
 selectedCompanyId = signal<string | null>(null);
 jobRoles = httpResource(() => {
 const id = this.selectedCompanyId();
 return id ? this.base + '?companyId=' + id : this.base + '/all';
 });
 create(req: CreateJobRoleRequest) { return this.http.post(this.base, req); }
 apply(id: string) { return this.http.patch(this.base + '/' + id + '/apply', {}); }
 delete(id: string) { return this.http.delete(this.base + '/' + id); }}