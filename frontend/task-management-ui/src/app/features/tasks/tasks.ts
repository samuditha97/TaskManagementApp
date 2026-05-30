import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskService } from '../../core/services/task.service';
import { AuthService } from '../../core/services/auth.service';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-tasks',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './tasks.html',
  styleUrl: './tasks.css'
})
export class Tasks implements OnInit {
  tasks: Task[] = [];
  filteredTasks: Task[] = [];

  taskForm: FormGroup;
  editingTaskId: number | null = null;

  searchText = '';
  filterStatus = 'all';
  sortBy = 'created';

  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private authService: AuthService,
    private router: Router
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.maxLength(500)],
      dueDate: [''],
      isCompleted: [false]
    });
  }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.taskService.getAll().subscribe({
      next: (response) => {
        this.tasks = response;
        this.applyFilters();
      },
      error: () => this.errorMessage = 'Failed to load tasks.'
    });
  }

  saveTask(): void {
    if (this.taskForm.invalid) {
      this.errorMessage = 'Please enter a valid task title.';
      return;
    }

    const formValue = this.taskForm.value;

    if (this.editingTaskId) {
      this.taskService.update(this.editingTaskId, formValue).subscribe({
        next: () => {
          this.successMessage = 'Task updated successfully.';
          this.resetForm();
          this.loadTasks();
        },
        error: () => this.errorMessage = 'Failed to update task.'
      });
    } else {
      this.taskService.create(formValue).subscribe({
        next: () => {
          this.successMessage = 'Task created successfully.';
          this.resetForm();
          this.loadTasks();
        },
        error: () => this.errorMessage = 'Failed to create task.'
      });
    }
  }

  editTask(task: Task): void {
    this.editingTaskId = task.id;

    this.taskForm.patchValue({
      title: task.title,
      description: task.description,
      dueDate: task.dueDate ? task.dueDate.substring(0, 10) : '',
      isCompleted: task.isCompleted
    });
  }

  deleteTask(id: number): void {
    if (!confirm('Are you sure you want to delete this task?')) return;

    this.taskService.delete(id).subscribe({
      next: () => {
        this.successMessage = 'Task deleted successfully.';
        this.loadTasks();
      },
      error: () => this.errorMessage = 'Failed to delete task.'
    });
  }

  markComplete(id: number): void {
    this.taskService.markAsCompleted(id).subscribe({
      next: () => {
        this.successMessage = 'Task marked as completed.';
        this.loadTasks();
      },
      error: () => this.errorMessage = 'Failed to update task.'
    });
  }

  applyFilters(): void {
    let result = [...this.tasks];

    if (this.searchText.trim()) {
      const search = this.searchText.toLowerCase();
      result = result.filter(task =>
        task.title.toLowerCase().includes(search) ||
        task.description?.toLowerCase().includes(search)
      );
    }

    if (this.filterStatus === 'completed') {
      result = result.filter(task => task.isCompleted);
    }

    if (this.filterStatus === 'pending') {
      result = result.filter(task => !task.isCompleted);
    }

    if (this.sortBy === 'title') {
      result.sort((a, b) => a.title.localeCompare(b.title));
    }

    if (this.sortBy === 'dueDate') {
      result.sort((a, b) =>
        new Date(a.dueDate || '').getTime() - new Date(b.dueDate || '').getTime()
      );
    }

    this.filteredTasks = result;
  }

  resetForm(): void {
    this.editingTaskId = null;
    this.taskForm.reset({
      title: '',
      description: '',
      dueDate: '',
      isCompleted: false
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  get completedCount(): number {
    return this.tasks.filter(x => x.isCompleted).length;
  }

  get pendingCount(): number {
    return this.tasks.filter(x => !x.isCompleted).length;
  }
}