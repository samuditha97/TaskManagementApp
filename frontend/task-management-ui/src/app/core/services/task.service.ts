import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Task, TaskCreate, TaskUpdate } from '../../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = 'http://localhost:5248/api/tasks';

  constructor(private http: HttpClient) {}

    private getAuthHeaders() {
  const username = localStorage.getItem('username') ?? '';
  const password = localStorage.getItem('password') ?? '';

  return {
    Authorization: 'Basic ' + btoa(`${username}:${password}`)
  };
}

 getAll(): Observable<Task[]> {
  return this.http.get<Task[]>(this.apiUrl, {
    headers: this.getAuthHeaders()
  });
}

getById(id: number): Observable<Task> {
  return this.http.get<Task>(`${this.apiUrl}/${id}`, {
    headers: this.getAuthHeaders()
  });
}

create(task: TaskCreate): Observable<Task> {
  return this.http.post<Task>(this.apiUrl, task, {
    headers: this.getAuthHeaders()
  });
}

update(id: number, task: TaskUpdate): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/${id}`, task, {
    headers: this.getAuthHeaders()
  });
}

delete(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/${id}`, {
    headers: this.getAuthHeaders()
  });
}

markAsCompleted(id: number): Observable<void> {
  return this.http.patch<void>(`${this.apiUrl}/${id}/complete`, {}, {
    headers: this.getAuthHeaders()
  });
}

}