# Unit Testing with Jasmine/Karma

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Write unit tests for Angular components
- [ ] Test services with dependencies
- [ ] Test HTTP requests with HttpClientTestingModule
- [ ] Use TestBed for component testing
- [ ] Apply testing best practices

## Conceptual Explanation

**Visual Analogy**: Think of testing as **quality assurance** in a car factory. Before a car goes to customers, it undergoes rigorous testing - engines are tested, brakes are checked, safety features are validated. Similarly, Angular applications need testing to ensure every component works correctly. Tests catch bugs early, prevent regressions, and give developers confidence when making changes!

### Testing Pyramid

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Testing Pyramid                                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│                         E2E Tests                                   │
│                    (End-to-End)                                     │
│                   ┌───────────────┐                                 │
│                  /   Integration   \                                │
│                 /     Tests         \                               │
│                /  ┌───────────────┐ \                             │
│               /   /    Unit Tests  \  \                            │
│              /   /                  \   \                          │
│             /   /                    \   \                         │
│            ▼   ▼                      ▼   ▼                       │
│         Faster                        Slower                        │
│         More                         More                           │
│         Tests                        Comprehensive                │
│                                                                     │
│   Unit Tests: 70% - Fast, isolated, many tests                      │
│   Integration Tests: 20% - Component interaction                    │
│   E2E Tests: 10% - Full application flow                           │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Step-by-Step Walkthrough

### Setting Up Testing Environment

Angular projects come with testing configured by default:

```bash
# Angular uses Jasmine + Karma
# Test files end with .spec.ts
ng generate component my-component  # Creates component.spec.ts
```

### Testing Components

#### Basic Component Test

```typescript
// hello.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-hello',
  standalone: true,
  template: `
    <h1>{{ message }}</h1>
    <button (click)="onClick()">Click me</button>
  `
})
export class HelloComponent {
  message = 'Hello, World!';
  
  onClick(): void {
    this.message = 'Button clicked!';
  }
}

// hello.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HelloComponent } from './hello.component';

describe('HelloComponent', () => {
  let component: HelloComponent;
  let fixture: ComponentFixture<HelloComponent>;
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HelloComponent]
    }).compileComponents();
    
    fixture = TestBed.createComponent(HelloComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should display initial message', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, World!');
  });
  
  it('should update message on click', () => {
    const button = fixture.nativeElement.querySelector('button');
    button.click();
    fixture.detectChanges();
    
    const h1 = fixture.nativeElement.querySelector('h1');
    expect(h1?.textContent).toContain('Button clicked!');
  });
});
```

#### Testing Components with Inputs/Outputs

```typescript
// user-card.component.ts
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="user-card" [class.selected]="selected">
      <h3>{{ user?.name }}</h3>
      <p>{{ user?.email }}</p>
      <button (click)="onDelete()">Delete</button>
    </div>
  `
})
export class UserCardComponent {
  @Input() user: { name: string; email: string } | null = null;
  @Input() selected = false;
  @Output() deleted = new EventEmitter<void>();
  
  onDelete(): void {
    this.deleted.emit();
  }
}

// user-card.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserCardComponent } from './user-card.component';

describe('UserCardComponent', () => {
  let component: UserCardComponent;
  let fixture: ComponentFixture<UserCardComponent>;
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserCardComponent]
    }).compileComponents();
    
    fixture = TestBed.createComponent(UserCardComponent);
    component = fixture.componentInstance;
  });
  
  it('should display user information', () => {
    component.user = { name: 'John', email: 'john@test.com' };
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h3')?.textContent).toContain('John');
    expect(compiled.querySelector('p')?.textContent).toContain('john@test.com');
  });
  
  it('should apply selected class when selected', () => {
    component.user = { name: 'John', email: 'john@test.com' };
    component.selected = true;
    fixture.detectChanges();
    
    const card = fixture.nativeElement.querySelector('.user-card');
    expect(card.classList.contains('selected')).toBeTrue();
  });
  
  it('should emit deleted event on button click', () => {
    spyOn(component.deleted, 'emit');
    component.user = { name: 'John', email: 'john@test.com' };
    fixture.detectChanges();
    
    const button = fixture.nativeElement.querySelector('button');
    button.click();
    
    expect(component.deleted.emit).toHaveBeenCalled();
  });
});
```

### Testing Services

```typescript
// user.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { User } from './user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private http = inject(HttpClient);
  
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>('/api/users');
  }
}

// user.service.spec.ts
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService } from './user.service';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });
    
    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });
  
  afterEach(() => {
    httpMock.verify();
  });
  
  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  
  it('should return users from API', () => {
    const mockUsers = [
      { id: 1, name: 'John', email: 'john@test.com' },
      { id: 2, name: 'Jane', email: 'jane@test.com' }
    ];
    
    service.getUsers().subscribe(users => {
      expect(users.length).toBe(2);
      expect(users).toEqual(mockUsers);
    });
    
    const req = httpMock.expectOne('/api/users');
    expect(req.request.method).toBe('GET');
    req.flush(mockUsers);
  });
  
  it('should handle HTTP error', () => {
    service.getUsers().subscribe({
      error: (err) => {
        expect(err.status).toBe(500);
      }
    });
    
    const req = httpMock.expectOne('/api/users');
    req.flush('Error', { status: 500, statusText: 'Server Error' });
  });
});
```

### Testing with Dependencies

```typescript
// calculator.service.ts
@Injectable({ providedIn: 'root' })
export class CalculatorService {
  add(a: number, b: number): number {
    return a + b;
  }
  
  subtract(a: number, b: number): number {
    return a - b;
  }
  
  multiply(a: number, b: number): number {
    return a * b;
  }
}

// calculator.service.spec.ts
describe('CalculatorService', () => {
  let service: CalculatorService;
  
  beforeEach(() => {
    service = new CalculatorService();
  });
  
  describe('add', () => {
    it('should add two numbers', () => {
      expect(service.add(2, 3)).toBe(5);
    });
    
    it('should handle negative numbers', () => {
      expect(service.add(-1, 1)).toBe(0);
    });
    
    it('should handle decimals', () => {
      expect(service.add(0.1, 0.2)).toBeCloseTo(0.3, 10);
    });
  });
  
  describe('subtract', () => {
    it('should subtract two numbers', () => {
      expect(service.subtract(5, 3)).toBe(2);
    });
  });
});
```

### Testing Reactive Forms

```typescript
// user-form.component.ts
@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <form [formGroup]="form">
      <input formControlName="name">
      <input formControlName="email">
      <button type="submit" [disabled]="form.invalid">Submit</button>
    </form>
  `
})
export class UserFormComponent {
  form = new FormGroup({
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email])
  });
  
  onSubmit(): void {
    if (this.form.valid) {
      console.log(this.form.value);
    }
  }
}

// user-form.component.spec.ts
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { UserFormComponent } from './user-form.component';

describe('UserFormComponent', () => {
  let component: UserFormComponent;
  let fixture: ComponentFixture<UserFormComponent>;
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, UserFormComponent]
    }).compileComponents();
    
    fixture = TestBed.createComponent(UserFormComponent);
    component = fixture.componentInstance;
  });
  
  it('should create form with invalid initial state', () => {
    expect(component.form.valid).toBeFalse();
  });
  
  it('should validate name field is required', () => {
    const nameControl = component.form.get('name');
    nameControl?.setValue('');
    expect(nameControl?.hasError('required')).toBeTrue();
  });
  
  it('should validate email field format', () => {
    const emailControl = component.form.get('email');
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTrue();
  });
  
  it('should be valid when all fields are filled correctly', () => {
    component.form.setValue({
      name: 'John',
      email: 'john@test.com'
    });
    expect(component.form.valid).toBeTrue();
  });
  
  it('should disable submit button when form is invalid', () => {
    fixture.detectChanges();
    const button = fixture.nativeElement.querySelector('button');
    expect(button.disabled).toBeTrue();
  });
});
```

## Running Tests

```bash
# Run tests once
ng test --watch=false

# Run tests with coverage
ng test --code-coverage

# Run specific test file
ng test --include='**/user.service.spec.ts'

# Run in CI mode
ng test --browsers=ChromeHeadless --watch=false
```

## Best Practices

### 1. Follow AAA Pattern

```typescript
it('should add user', () => {
  // Arrange
  const service = new UserService();
  const user = { name: 'John' };
  
  // Act
  const result = service.addUser(user);
  
  // Assert
  expect(result).toEqual(user);
});
```

### 2. Use Descriptive Test Names

```typescript
// Good: Descriptive names
it('should return empty array when no users exist');
it('should throw error when user is not found');

// Avoid: Vague names
it('should work');
it('test1');
```

### 3. Test One Thing Per Test

```typescript
// Good: One assertion per test
it('should return user by id', () => {
  const user = service.getUser(1);
  expect(user.id).toBe(1);
});

it('should return null for non-existent id', () => {
  const user = service.getUser(999);
  expect(user).toBeNull();
});
```

### 4. Mock Dependencies

```typescript
// Good: Use mock objects
const mockUserService = {
  getUser: jasmine.createSpy('getUser').and.returnValue(of(user)),
  saveUser: jasmine.createSpy('saveUser').and.returnValue(of(true))
};

// Avoid: Real dependencies
const service = new RealUserService(); // May fail without backend
```

## Hands-On Exercise

### Exercise 3.2: Testing Implementation

**Objective**: Write tests for a task management feature

**Requirements**:
1. Write component tests for task list
2. Write service tests with mocked HTTP
3. Test form validation
4. Achieve 70%+ code coverage

**Deliverable**: Complete test suite

**Assessment Criteria**:
- [ ] Component tests passing
- [ ] Service tests with mocks
- [ ] Form validation tests
- [ ] Tests follow AAA pattern

## Summary

- **Jasmine** provides test framework
- **Karma** runs tests in browsers
- **TestBed** configures Angular testing module
- **HttpTestingModule** mocks HTTP requests
- Follow **AAA pattern**: Arrange, Act, Assert
- Test one thing per test case

## Suggested Reading

- [Angular Testing Guide](https://angular.io/guide/testing)
- "Testing Angular Applications"
