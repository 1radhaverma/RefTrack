import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { httpResource } from '@angular/common/http';
import { CreateCompanyRequest } from '../models/company.model';
@Injectable({ providedIn: 'root' })
export class CompanyService {
 private http = inject(HttpClient);
 private base = 'https://localhost:7288/api/companies';
 // httpResource: auto-fetches on init, 
exposes .value() .isLoading() .hasValue() .reload()
 companies = httpResource(this.base);
 create(req: CreateCompanyRequest) { return this.http.post(this.base, req); }
 blacklist(id: string) { return this.http.patch(this.base + '/' + id + '/blacklist', 
{}); }
 delete(id: string) { return this.http.delete(this.base + '/' + id); 
 }
}