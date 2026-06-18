import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { httpResource } from '@angular/common/http';
import { CreateReferrerRequest } from '../models/referrer.model';
@Injectable({ providedIn: 'root' })
export class ReferrerService {
 private http = inject(HttpClient);
 private base = 'https://localhost:7288/api/referrers';
 selectedJobRoleId = signal<string | null>(null);
 referrers = httpResource(() => {
 const id = this.selectedJobRoleId();
 return id ? this.base + '?jobRoleId=' + id : this.base + '/all';
 });
 create(req: CreateReferrerRequest) { return this.http.post(this.base, req); }
 contact(id: string) { return this.http.patch(this.base + '/' + id + '/contact', {}); }
 replied(id: string) { return this.http.patch(this.base + '/' + id + '/replied', {}); }
 referred(id: string) { return this.http.patch(this.base + '/' + id + '/referred', 
{}); }
 ghosted(id: string) { return this.http.patch(this.base + '/' + id + '/ghosted', {}); }
 delete(id: string) { return this.http.delete(this.base + '/' + id); }
 }
