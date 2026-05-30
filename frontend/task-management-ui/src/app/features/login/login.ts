import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  errorMessage = '';
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login(): void {
    if (this.loginForm.invalid) {
      this.errorMessage = 'Please enter username and password.';
      return;
    }

    this.authService.login({
      username: this.loginForm.value.username ?? '',
      password: this.loginForm.value.password ?? ''
    }).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: () => this.errorMessage = 'Invalid username or password.'
    });
  }
}