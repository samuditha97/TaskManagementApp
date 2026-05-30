import { Routes } from '@angular/router';
import { Login } from './features/login/login';
import { Tasks } from './features/tasks/tasks';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'tasks', component: Tasks, canActivate: [authGuard] },
  { path: '**', redirectTo: 'login' }
];