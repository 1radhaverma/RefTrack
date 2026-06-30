import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { httpResource } from '@angular/common/http';
import { CreateReferrerRequest } from '../models/referrer.model';
import { rxResource } from '@angular/core/rxjs-interop'; 

export interface Referrer {
  id: string;
  name: string;
  designation: string;
  status: string;
  linkedInUrl?: string;
  jobRoleId?: string;
}
@Injectable({ providedIn: 'root' })
export class ReferrerService {

 private http = inject(HttpClient);
 private base = 'https://localhost:7288/api/referrers';
 selectedJobRoleId = signal<string | null>(null);
//  referrers = httpResource(() => {
//  const id = this.selectedJobRoleId();
//  return id ? this.base + '?jobRoleId=' + id : this.base + '/all';
//  });

referrers = rxResource<Referrer[], string>({
    // 1. 'request' is now 'params'
    params: () => {
      const id = this.selectedJobRoleId();
      return id ? `${this.base}?jobRoleId=${id}` : `${this.base}/all`;
    },
    // 2. 'loader' is now 'stream', and context is explicitly typed
    stream: ({ params: url }) => {
      return this.http.get<Referrer[]>(url);
    }
  });
  
 create(req: CreateReferrerRequest) { return this.http.post(this.base, req); }
 contact(id: string) { return this.http.patch(this.base + '/' + id + '/contact', {}); }
 replied(id: string) { return this.http.patch(this.base + '/' + id + '/replied', {}); }
 referred(id: string) { return this.http.patch(this.base + '/' + id + '/referred', 
{}); }
 ghosted(id: string) { return this.http.patch(this.base + '/' + id + '/ghosted', {}); }
 delete(id: string) { return this.http.delete(this.base + '/' + id); }
 }
