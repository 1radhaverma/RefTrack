import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';


export const routes: Routes = [
 { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
 { path: 'register', loadComponent: () => 
import('./features/auth/register/register.component').then(m => m.RegisterComponent) },
 { path: 'dashboard', canActivate: [authGuard], loadComponent: () => 
import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
 { path: 'companies', canActivate: [authGuard], loadComponent: () => 
import('./features/companies/company-list/company-list.component').then(m => 
m.CompanyListComponent) },
 { path: 'jobroles', canActivate: [authGuard], loadComponent: () => 
import('./features/job-roles/job-role-list/job-role-list.component').then(m => 
m.JobRoleListComponent) },
 { path: 'referrers', canActivate: [authGuard], loadComponent: () => 
import('./features/referrers/referrer-list/referrer-list.component').then(m => 
m.ReferrerListComponent) },
 { path: 'applications', canActivate: [authGuard], loadComponent: () => 
import('./features/applications/application-list/application-list.component').then(m => 
m.ApplicationListComponent) },
 { path: '**', redirectTo: 'dashboard' }
];