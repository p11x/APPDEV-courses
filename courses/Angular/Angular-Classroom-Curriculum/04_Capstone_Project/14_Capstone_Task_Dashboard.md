# Capstone Project: Task Management Dashboard

## Project Overview

This capstone project integrates all concepts learned throughout the Angular curriculum into a complete, production-ready application. Students will build a comprehensive **Task Management Dashboard** with authentication, CRUD operations, Material Design UI, and state management.

## Learning Objectives

By completing this project, students will demonstrate:

- [ ] Full-stack application development skills
- [ ] Authentication and route protection
- [ ] Complete CRUD operations
- [ ] Material Design component integration
- [ ] State management implementation
- [ ] Responsive design
- [ ] Best practices and code organization

## Project Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Task Management Dashboard Architecture             │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   ┌─────────────────────────────────────────────────────────────┐  │
│   │                    Angular Application                       │  │
│   ├─────────────────────────────────────────────────────────────┤  │
│   │  Pages:                                                     │  │
│   │  ├── Login/Register (Auth)                                 │  │
│   │  ├── Dashboard (Overview)                                  │  │
│   │  ├── Tasks List (with filters)                             │  │
│   │  ├── Task Detail/Edit                                       │  │
│   │  ├── Users Management (Admin)                              │  │
│   │  └── Settings                                               │  │
│   ├─────────────────────────────────────────────────────────────┤  │
│   │  Features:                                                  │  │
│   │  ├── Authentication (JWT)                                  │  │
│   │  ├── Task CRUD                                              │  │
│   │  ├── Pagination & Search                                    │  │
│   │  ├── State Management (NgRx)                               │  │
│   │  ├── Responsive Design                                      │  │
│   │  └── Material Design                                         │  │
│   ├─────────────────────────────────────────────────────────────┤  │
│   │  Services:                                                  │  │
│   │  ├── AuthService                                           │  │
│   │  ├── TaskService                                           │  │
│   │  ├── UserService                                           │  │
│   │  └── HttpInterceptor                                       │  │
│   └─────────────────────────────────────────────────────────────┘  │
│                                                                     │
│   ┌─────────────────────────────────────────────────────────────┐  │
│   │                    REST API (Mock/Simulated)                │  │
│   └─────────────────────────────────────────────────────────────┘  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Prerequisites

Before starting this project, ensure you have completed:

- [ ] All Beginner Level topics
- [ ] All Intermediate Level topics
- [ ] All Advanced Level topics
- [ ] Angular 17+ development environment set up

## Project Setup

### Step 1: Create Project

```bash
ng new task-dashboard --routing --style=scss --standalone
cd task-dashboard
```

### Step 2: Install Dependencies

```bash
# Angular Material
ng add @angular/material

# State Management
npm install @ngrx/store @ngrx/effects @ngrx/entity @ngrx/store-devtools

# Additional utilities
npm install uuid date-fns
```

### Step 3: Configure Application

```typescript
// app.config.ts
import { ApplicationConfig, isDevMode } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';

import { routes } from './app.routes';
import { taskReducer } from './store/task/task.reducer';
import { TaskEffects } from './store/task/task.effects';
import { authReducer } from './store/auth/auth.reducer';
import { AuthEffects } from './store/auth/auth.effects';
import { authInterceptor } from './core/interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(
      withInterceptorsFromDi(),
    ),
    provideAnimations(),
    provideStore({
      tasks: taskReducer,
      auth: authReducer
    }),
    provideEffects([TaskEffects, AuthEffects]),
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !isDevMode()
    }),
    {
      provide: HTTP_INTERCEPTORS,
      useValue: authInterceptor,
      multi: true
    }
  ]
};
```

## Feature Implementation

### 1. Authentication Module

#### Auth Service

```typescript
// services/auth.service.ts
import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, of, BehaviorSubject } from 'rxjs';

export interface User {
  id: string;
  email: string;
  name: string;
  role: 'admin' | 'user';
  avatar?: string;
}

export interface LoginResponse {
  user: User;
  token: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  
  // Reactive state with signals
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  
  private isAuthenticated = signal(false);
  isLoggedIn = computed(() => this.isAuthenticated());
  
  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/auth/login', { email, password })
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('user', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
          this.isAuthenticated.set(true);
        })
      );
  }
  
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.isAuthenticated.set(false);
    this.router.navigate(['/login']);
  }
  
  getToken(): string | null {
    return localStorage.getItem('token');
  }
  
  getCurrentUser(): User | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
  
  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
```

#### Auth Guard

```typescript
// guards/auth.guard.ts
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  
  if (authService.isLoggedIn()) {
    return true;
  }
  
  return router.createUrlTree(['/login']);
};

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const user = authService.getCurrentUser();
  
  if (user?.role === 'admin') {
    return true;
  }
  
  return router.createUrlTree(['/dashboard']);
};
```

### 2. Task Management Module

#### Task Model

```typescript
// models/task.model.ts
export interface Task {
  id: string;
  title: string;
  description: string;
  status: 'todo' | 'in-progress' | 'review' | 'done';
  priority: 'low' | 'medium' | 'high' | 'urgent';
  assigneeId?: string;
  assigneeName?: string;
  dueDate: Date;
  createdAt: Date;
  updatedAt: Date;
  tags: string[];
  comments: TaskComment[];
}

export interface TaskComment {
  id: string;
  userId: string;
  userName: string;
  content: string;
  createdAt: Date;
}

export interface TaskFilter {
  status?: Task['status'];
  priority?: Task['priority'];
  assigneeId?: string;
  search?: string;
  page: number;
  limit: number;
}
```

#### Task Store (NgRx)

```typescript
// store/task/task.actions.ts
import { createAction, props } from '@ngrx/store';
import { Task, TaskFilter } from '../../models/task.model';

export const loadTasks = createAction('[Task] Load Tasks', props<{ filter?: TaskFilter }>());
export const loadTasksSuccess = createAction('[Task] Load Tasks Success', props<{ tasks: Task[]; total: number }>());
export const loadTasksFailure = createAction('[Task] Load Tasks Failure', props<{ error: string }>());

export const loadTask = createAction('[Task] Load Task', props<{ id: string }>());
export const loadTaskSuccess = createAction('[Task] Load Task Success', props<{ task: Task }>());
export const loadTaskFailure = createAction('[Task] Load Task Failure', props<{ error: string }>());

export const createTask = createAction('[Task] Create Task', props<{ task: Partial<Task> }>());
export const createTaskSuccess = createAction('[Task] Create Task Success', props<{ task: Task }>());
export const createTaskFailure = createAction('[Task] Create Task Failure', props<{ error: string }>());

export const updateTask = createAction('[Task] Update Task', props<{ task: Task }>());
export const updateTaskSuccess = createAction('[Task] Update Task Success', props<{ task: Task }>());
export const updateTaskFailure = createAction('[Task] Update Task Failure', props<{ error: string }>());

export const deleteTask = createAction('[Task] Delete Task', props<{ id: string }>());
export const deleteTaskSuccess = createAction('[Task] Delete Task Success', props<{ id: string }>());
export const deleteTaskFailure = createAction('[Task] Delete Task Failure', props<{ error: string }>());

export const setFilter = createAction('[Task] Set Filter', props<{ filter: TaskFilter }>());

// store/task/task.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Task, TaskFilter } from '../../models/task.model';
import * as TaskActions from './task.actions';

export interface TaskState extends EntityState<Task> {
  selectedTaskId: string | null;
  loading: boolean;
  error: string | null;
  filter: TaskFilter;
  total: number;
}

export const adapter: EntityAdapter<Task> = createEntityAdapter<Task>({
  selectId: (task: Task) => task.id,
  sortComparer: (a, b) => new Date(b.updatedAt).getTime() - new Date(a.updatedAt).getTime()
});

export const initialState: TaskState = adapter.getInitialState({
  selectedTaskId: null,
  loading: false,
  error: null,
  filter: { page: 1, limit: 10 },
  total: 0
});

export const taskReducer = createReducer(
  initialState,
  
  on(TaskActions.loadTasks, (state) => ({ ...state, loading: true, error: null })),
  on(TaskActions.loadTasksSuccess, (state, { tasks, total }) => 
    adapter.setAll(tasks, { ...state, loading: false, total })
  ),
  on(TaskActions.loadTasksFailure, (state, { error }) => ({ ...state, loading: false, error })),
  
  on(TaskActions.createTaskSuccess, (state, { task }) => adapter.addOne(task, state)),
  on(TaskActions.updateTaskSuccess, (state, { task }) => adapter.updateOne({ id: task.id, changes: task }, state)),
  on(TaskActions.deleteTaskSuccess, (state, { id }) => adapter.removeOne(id, state)),
  
  on(TaskActions.setFilter, (state, { filter }) => ({ ...state, filter }))
);
```

### 3. Task List Component

```typescript
// components/task-list/task-list.component.ts
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Store } from '@ngrx/store';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';

import { Task } from '../../models/task.model';
import * as TaskActions from '../../store/task/task.actions';
import * as TaskSelectors from '../../store/task/task.selectors';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule, 
    RouterLink,
    MatCardModule, 
    MatButtonModule, 
    MatIconModule,
    MatChipsModule,
    MatPaginatorModule,
    MatInputModule,
    MatSelectModule,
    FormsModule
  ],
  template: `
    <div class="task-list-container">
      <div class="header">
        <h1>Task Dashboard</h1>
        <button mat-raised-button color="primary" routerLink="/tasks/new">
          <mat-icon>add</mat-icon> New Task
        </button>
      </div>
      
      <!-- Filters -->
      <div class="filters">
        <mat-form-field appearance="outline">
          <mat-label>Search</mat-label>
          <input matInput [(ngModel)]="searchTerm" (input)="onSearch()" placeholder="Search tasks...">
          <mat-icon matSuffix>search</mat-icon>
        </mat-form-field>
        
        <mat-form-field appearance="outline">
          <mat-label>Status</mat-label>
          <mat-select [(ngModel)]="statusFilter" (selectionChange)="onFilterChange()">
            <mat-option value="">All</mat-option>
            <mat-option value="todo">To Do</mat-option>
            <mat-option value="in-progress">In Progress</mat-option>
            <mat-option value="review">Review</mat-option>
            <mat-option value="done">Done</mat-option>
          </mat-select>
        </mat-form-field>
        
        <mat-form-field appearance="outline">
          <mat-label>Priority</mat-label>
          <mat-select [(ngModel)]="priorityFilter" (selectionChange)="onFilterChange()">
            <mat-option value="">All</mat-option>
            <mat-option value="low">Low</mat-option>
            <mat-option value="medium">Medium</mat-option>
            <mat-option value="high">High</mat-option>
            <mat-option value="urgent">Urgent</mat-option>
          </mat-select>
        </mat-form-field>
      </div>
      
      <!-- Task Stats -->
      <div class="stats">
        <mat-card class="stat-card">
          <mat-card-content>
            <span class="stat-value">{{ todoCount() }}</span>
            <span class="stat-label">To Do</span>
          </mat-card-content>
        </mat-card>
        <mat-card class="stat-card">
          <mat-card-content>
            <span class="stat-value">{{ inProgressCount() }}</span>
            <span class="stat-label">In Progress</span>
          </mat-card-content>
        </mat-card>
        <mat-card class="stat-card">
          <mat-card-content>
            <span class="stat-value">{{ reviewCount() }}</span>
            <span class="stat-label">Review</span>
          </mat-card-content>
        </mat-card>
        <mat-card class="stat-card">
          <mat-card-content>
            <span class="stat-value">{{ doneCount() }}</span>
            <span class="stat-label">Done</span>
          </mat-card-content>
        </mat-card>
      </div>
      
      <!-- Loading -->
      @if (loading()) {
        <div class="loading">Loading tasks...</div>
      }
      
      <!-- Task Grid -->
      @if (!loading()) {
        <div class="task-grid">
          @for (task of tasks(); track task.id) {
            <mat-card class="task-card" [class.priority-urgent]="task.priority === 'urgent'">
              <mat-card-header>
                <mat-card-title>{{ task.title }}</mat-card-title>
                <mat-card-subtitle>
                  <mat-chip [class]="task.status">{{ task.status }}</mat-chip>
                  <mat-chip [class]="'priority-' + task.priority">{{ task.priority }}</mat-chip>
                </mat-card-subtitle>
              </mat-card-header>
              
              <mat-card-content>
                <p>{{ task.description | slice:0:100 }}...</p>
                <div class="task-meta">
                  <span><mat-icon>calendar_today</mat-icon> {{ task.dueDate | date:'mediumDate' }}</span>
                  <span><mat-icon>person</mat-icon> {{ task.assigneeName || 'Unassigned' }}</span>
                </div>
                <div class="tags">
                  @for (tag of task.tags; track tag) {
                    <mat-chip>{{ tag }}</mat-chip>
                  }
                </div>
              </mat-card-content>
              
              <mat-card-actions>
                <button mat-button [routerLink]="['/tasks', task.id]">View</button>
                <button mat-button [routerLink]="['/tasks', task.id, 'edit']">Edit</button>
                <button mat-button color="warn" (click)="deleteTask(task.id)">Delete</button>
              </mat-card-actions>
            </mat-card>
          } @empty {
            <div class="empty-state">
              <mat-icon>assignment</mat-icon>
              <p>No tasks found</p>
              <button mat-raised-button color="primary" routerLink="/tasks/new">
                Create your first task
              </button>
            </div>
          }
        </div>
      }
      
      <!-- Pagination -->
      <mat-paginator
        [length]="total()"
        [pageSize]="pageSize()"
        [pageIndex]="pageIndex()"
        [pageSizeOptions]="[5, 10, 25, 50]"
        (page)="onPageChange($event)"
        aria-label="Select page">
      </mat-paginator>
    </div>
  `,
  styles: [`
    .task-list-container { padding: 2rem; }
    .header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; }
    .filters { display: flex; gap: 1rem; margin-bottom: 2rem; flex-wrap: wrap; }
    .filters mat-form-field { flex: 1; min-width: 200px; }
    .stats { display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); gap: 1rem; margin-bottom: 2rem; }
    .stat-card { text-align: center; }
    .stat-value { font-size: 2rem; font-weight: bold; display: block; }
    .stat-label { color: #666; }
    .task-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; margin-bottom: 2rem; }
    .task-card { transition: transform 0.2s; }
    .task-card:hover { transform: translateY(-4px); box-shadow: 0 4px 12px rgba(0,0,0,0.15); }
    .task-card.priority-urgent { border-left: 4px solid #f44336; }
    .task-meta { display: flex; gap: 1rem; margin: 1rem 0; color: #666; font-size: 0.875rem; }
    .task-meta mat-icon { font-size: 1rem; width: 1rem; height: 1rem; vertical-align: middle; }
    .tags { display: flex; gap: 0.5rem; flex-wrap: wrap; }
    .mat-mdc-chip.status-todo { background: #e3f2fd; }
    .mat-mdc-chip.status-in-progress { background: #fff3e0; }
    .mat-mdc-chip.status-review { background: #f3e5f5; }
    .mat-mdc-chip.status-done { background: #e8f5e9; }
    .mat-mdc-chip.priority-low { background: #4caf50; color: white; }
    .mat-mdc-chip.priority-medium { background: #ff9800; color: white; }
    .mat-mdc-chip.priority-high { background: #f44336; color: white; }
    .mat-mdc-chip.priority-urgent { background: #9c27b0; color: white; }
    .empty-state { grid-column: 1 / -1; text-align: center; padding: 3rem; }
    .empty-state mat-icon { font-size: 4rem; width: 4rem; height: 4rem; color: #ccc; }
    .loading { text-align: center; padding: 2rem; }
  `]
})
export class TaskListComponent implements OnInit {
  private store = inject(Store);
  
  // Signals from store
  tasks = this.store.selectSignal(TaskSelectors.selectAllTasks);
  loading = this.store.selectSignal(TaskSelectors.selectTaskLoading);
  total = this.store.selectSignal(TaskSelectors.selectTaskTotal);
  
  // Computed stats
  todoCount = computed(() => this.tasks().filter(t => t.status === 'todo').length);
  inProgressCount = computed(() => this.tasks().filter(t => t.status === 'in-progress').length);
  reviewCount = computed(() => this.tasks().filter(t => t.status === 'review').length);
  doneCount = computed(() => this.tasks().filter(t => t.status === 'done').length);
  
  // Local state
  searchTerm = '';
  statusFilter = '';
  priorityFilter = '';
  pageIndex = signal(0);
  pageSize = signal(10);
  
  ngOnInit(): void {
    this.loadTasks();
  }
  
  loadTasks(): void {
    this.store.dispatch(TaskActions.loadTasks({
      filter: {
        search: this.searchTerm || undefined,
        status: this.statusFilter as Task['status'] || undefined,
        priority: this.priorityFilter as Task['priority'] || undefined,
        page: this.pageIndex() + 1,
        limit: this.pageSize()
      }
    }));
  }
  
  onSearch(): void {
    this.pageIndex.set(0);
    this.loadTasks();
  }
  
  onFilterChange(): void {
    this.pageIndex.set(0);
    this.loadTasks();
  }
  
  onPageChange(event: PageEvent): void {
    this.pageIndex.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.loadTasks();
  }
  
  deleteTask(id: string): void {
    if (confirm('Are you sure you want to delete this task?')) {
      this.store.dispatch(TaskActions.deleteTask({ id }));
    }
  }
}
```

### 4. Task Form Component

```typescript
// components/task-form/task-form.component.ts
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators, FormArray } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';

import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatChipsModule,
    MatIconModule,
    RouterLink
  ],
  template: `
    <div class="task-form-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>{{ isEditMode() ? 'Edit Task' : 'New Task' }}</mat-card-title>
        </mat-card-header>
        
        <mat-card-content>
          <form [formGroup]="taskForm" (ngSubmit)="onSubmit()">
            <!-- Title -->
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Title</mat-label>
              <input matInput formControlName="title" placeholder="Enter task title">
              @if (taskForm.get('title')?.hasError('required')) {
                <mat-error>Title is required</mat-error>
              }
            </mat-form-field>
            
            <!-- Description -->
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Description</mat-label>
              <textarea matInput formControlName="description" rows="4" placeholder="Describe the task..."></textarea>
            </mat-form-field>
            
            <!-- Status & Priority -->
            <div class="form-row">
              <mat-form-field appearance="outline">
                <mat-label>Status</mat-label>
                <mat-select formControlName="status">
                  <mat-option value="todo">To Do</mat-option>
                  <mat-option value="in-progress">In Progress</mat-option>
                  <mat-option value="review">Review</mat-option>
                  <mat-option value="done">Done</mat-option>
                </mat-select>
              </mat-form-field>
              
              <mat-form-field appearance="outline">
                <mat-label>Priority</mat-label>
                <mat-select formControlName="priority">
                  <mat-option value="low">Low</mat-option>
                  <mat-option value="medium">Medium</mat-option>
                  <mat-option value="high">High</mat-option>
                  <mat-option value="urgent">Urgent</mat-option>
                </mat-select>
              </mat-form-field>
            </div>
            
            <!-- Due Date -->
            <mat-form-field appearance="outline">
              <mat-label>Due Date</mat-label>
              <input matInput [matDatepicker]="picker" formControlName="dueDate">
              <mat-datepicker-toggle matIconSuffix [for]="picker"></mat-datepicker-toggle>
              <mat-datepicker #picker></mat-datepicker>
            </mat-form-field>
            
            <!-- Tags -->
            <mat-form-field appearance="outline" class="full-width">
              <mat-label>Tags (comma separated)</mat-label>
              <input matInput formControlName="tagsInput" placeholder="work, urgent, review">
            </mat-form-field>
            
            <!-- Actions -->
            <div class="form-actions">
              <button mat-button type="button" routerLink="/tasks">Cancel</button>
              <button mat-raised-button color="primary" type="submit" [disabled]="taskForm.invalid">
                {{ isEditMode() ? 'Update' : 'Create' }} Task
              </button>
            </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .task-form-container { max-width: 600px; margin: 2rem auto; padding: 0 1rem; }
    .full-width { width: 100%; }
    .form-row { display: flex; gap: 1rem; }
    .form-row mat-form-field { flex: 1; }
    .form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1rem; }
  `]
})
export class TaskFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private taskService = inject(TaskService);
  
  taskId: string | null = null;
  isEditMode = signal(false);
  
  taskForm = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(3)]],
    description: [''],
    status: ['todo', Validators.required],
    priority: ['medium', Validators.required],
    dueDate: [new Date(), Validators.required],
    tagsInput: ['']
  });
  
  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id');
    if (this.taskId) {
      this.isEditMode.set(true);
      this.loadTask(this.taskId);
    }
  }
  
  loadTask(id: string): void {
    this.taskService.getTask(id).subscribe(task => {
      if (task) {
        this.taskForm.patchValue({
          title: task.title,
          description: task.description,
          status: task.status,
          priority: task.priority,
          dueDate: new Date(task.dueDate),
          tagsInput: task.tags.join(', ')
        });
      }
    });
  }
  
  onSubmit(): void {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const taskData: Partial<Task> = {
        title: formValue.title!,
        description: formValue.description || '',
        status: formValue.status as Task['status'],
        priority: formValue.priority as Task['priority'],
        dueDate: formValue.dueDate!,
        tags: formValue.tagsInput ? formValue.tagsInput.split(',').map(t => t.trim()) : []
      };
      
      if (this.isEditMode() && this.taskId) {
        this.taskService.updateTask({ ...taskData, id: this.taskId } as Task).subscribe(() => {
          this.router.navigate(['/tasks']);
        });
      } else {
        this.taskService.createTask(taskData).subscribe(() => {
          this.router.navigate(['/tasks']);
        });
      }
    }
  }
}
```

## Project Routes

```typescript
// app.routes.ts
import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  
  // Public routes
  { path: 'login', loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent) },
  
  // Protected routes
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'tasks',
    loadComponent: () => import('./pages/task-list/task-list.component').then(m => m.TaskListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'tasks/new',
    loadComponent: () => import('./pages/task-form/task-form.component').then(m => m.TaskFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'tasks/:id',
    loadComponent: () => import('./pages/task-detail/task-detail.component').then(m => m.TaskDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'tasks/:id/edit',
    loadComponent: () => import('./pages/task-form/task-form.component').then(m => m.TaskFormComponent),
    canActivate: [authGuard]
  },
  
  // Admin routes
  {
    path: 'users',
    loadComponent: () => import('./pages/users/users.component').then(m => m.UsersComponent),
    canActivate: [authGuard]
  },
  
  // 404
  { path: '**', loadComponent: () => import('./pages/not-found/not-found.component').then(m => m.NotFoundComponent) }
];
```

## Deliverables

### Minimum Requirements

1. **Authentication**
   - Login form with email/password
   - JWT token storage
   - Route guards
   - Logout functionality

2. **Task Management**
   - Create, Read, Update, Delete tasks
   - Task status workflow (todo → in-progress → review → done)
   - Priority levels (low, medium, high, urgent)
   - Due dates

3. **Search & Filter**
   - Text search
   - Status filter
   - Priority filter
   - Pagination

4. **UI/UX**
   - Material Design components
   - Responsive layout
   - Loading states
   - Error handling

### Extension Features

1. **Advanced Features**
   - Task comments
   - File attachments
   - Activity log
   - Task categories/tags

2. **Admin Features**
   - User management
   - Role-based access
   - Analytics dashboard

3. **Technical Features**
   - Unit tests
   - E2E tests
   - Production build optimization

## Assessment Criteria

| Feature | Points | Criteria |
|---------|--------|----------|
| Authentication | 20 | Login, logout, guards, token handling |
| Task CRUD | 25 | Create, read, update, delete operations |
| Search & Filter | 15 | Search, filters, pagination work correctly |
| UI/UX | 15 | Material Design, responsive, error handling |
| State Management | 15 | NgRx store implemented correctly |
| Code Quality | 10 | Best practices, TypeScript, organization |

## Project Timeline

- **Week 1**: Setup, authentication, routing
- **Week 2**: Task CRUD, state management
- **Week 3**: Search, filters, UI polish
- **Week 4**: Testing, optimization, extensions

## Conclusion

This capstone project demonstrates comprehensive Angular skills including:
- Modern Angular 17+ features (signals, standalone components, control flow)
- State management with NgRx
- Material Design UI
- Authentication and security
- Best practices and code organization

Completing this project prepares students for professional Angular development roles.
