import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { httpResource } from '@angular/common/http';
@Injectable({ providedIn: 'root' })
export class ApplicationService {
 private http = inject(HttpClient);
 private base = 'https://localhost:7288/api/applications';
 applications = httpResource(this.base);
 summary = httpResource(this.base + '/summary');
 create(jobRoleId: string) { return this.http.post(this.base, { jobRoleId }); }
 move(applicationId: string, status: string) {
 return this.http.patch(this.base + '/move', { applicationId, status });
 }
 delete(id: string) { return this.http.delete(this.base + '/' + id); }
}
