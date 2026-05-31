import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse } from '../../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5248/api/auth';

  constructor(private http: HttpClient) {}

 login(request: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request).pipe(
    tap(response => {
      if (response.success) {
        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem('username', request.username);
        localStorage.setItem('password', request.password);
      }
    })
  );
}

 logout(): void {
  localStorage.removeItem('isLoggedIn');
  localStorage.removeItem('username');
  localStorage.removeItem('password');
}

  isLoggedIn(): boolean {
    return localStorage.getItem('isLoggedIn') === 'true';
  }
}