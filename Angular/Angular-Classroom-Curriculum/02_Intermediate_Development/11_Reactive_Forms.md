# Reactive Forms

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Create reactive forms with FormControl, FormGroup, and FormArray
- [ ] Implement form validation with built-in and custom validators
- [ ] Create nested forms and form arrays
- [ ] Handle form submission and data flow
- [ ] Optimize form performance with custom validators

## Conceptual Explanation

**Visual Analogy**: Think of Reactive Forms as a **smart form builder** that knows everything about itself at all times. Unlike template-driven forms (where the template does all the work), reactive forms maintain a real-time model of the form state. It's like having a **form expert** that can tell you at any moment: "This field is invalid", "These fields have errors", and "Here's what the user typed!"

### Reactive Forms vs Template-Driven Forms

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Form Comparison                                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   TEMPLATE-DRIVEN                    REACTIVE FORMS                 │
│   ─────────────────                  ────────────────              │
│   • HTML-centric                     • Code-centric                  │
│   • Directives manage form          • FormControl/Group models     │
│   • Hard to test                    • Easy to test                  │
│   • Limited validation              • Complex validation            │
│   • Two-way binding (ngModel)       • Programmatic control         │
│   • Good for simple forms           • Good for complex forms       │
│                                                                     │
│   Example:                           Example:                        │
│   <form #f="ngForm">                this.form = new FormGroup({    │
│     <input [(ngModel)]="name">        name: new FormControl('',    │
│   </form>                                  Validators.required)     │
│                                       });                            │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why Reactive Forms Matter

1. **Complex Forms**: Dynamic fields, arrays, nested groups
2. **Real-time Validation**: Instant feedback as user types
3. **Testability**: Easy to unit test form behavior
4. **Reactivity**: React to form changes anywhere
5. **Custom Validation**: Build complex business rules

### Industry Use Cases

- **Registration Forms**: Multi-step wizard forms
- **Settings Pages**: Configuration with many options
- **Data Entry**: Complex data collection
- **Search Filters**: Dynamic query builders

## Step-by-Step Walkthrough

### Creating a Basic Reactive Form

#### Step 1: Import ReactiveFormsModule

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideReactiveForms } from '@angular/forms';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    provideReactiveForms()
  ]
};
```

#### Step 2: Create Form in Component

```typescript
// user-form.component.ts
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { 
  ReactiveFormsModule, 
  FormGroup, 
  FormControl, 
  Validators 
} from '@angular/forms';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <form [formGroup]="userForm" (ngSubmit)="onSubmit()">
      <div class="form-group">
        <label for="name">Name</label>
        <input 
          id="name" 
          type="text" 
          formControlName="name"
          [class.error]="isFieldInvalid('name')">
        
        @if (isFieldInvalid('name')) {
          <span class="error-msg">
            @if (userForm.get('name')?.hasError('required')) {
              Name is required
            } @else if (userForm.get('name')?.hasError('minlength')) {
              Name must be at least 3 characters
            }
          </span>
        }
      </div>
      
      <div class="form-group">
        <label for="email">Email</label>
        <input 
          id="email" 
          type="email" 
          formControlName="email"
          [class.error]="isFieldInvalid('email')">
        
        @if (isFieldInvalid('email')) {
          <span class="error-msg">Valid email is required</span>
        }
      </div>
      
      <div class="form-group">
        <label for="phone">Phone</label>
        <input 
          id="phone" 
          type="tel" 
          formControlName="phone">
      </div>
      
      <button type="submit" [disabled]="userForm.invalid">Submit</button>
      <button type="button" (click)="resetForm()">Reset</button>
    </form>
    
    <div class="form-value">
      <h3>Form Value:</h3>
      <pre>{{ userForm.value | json }}</pre>
    </div>
  `,
  styles: [`
    form { max-width: 500px; margin: 2rem auto; }
    .form-group { margin-bottom: 1rem; }
    label { display: block; margin-bottom: 0.5rem; font-weight: bold; }
    input { 
      width: 100%; 
      padding: 0.5rem; 
      border: 1px solid #ddd; 
      border-radius: 4px; 
    }
    input.error { border-color: #f44336; }
    .error-msg { color: #f44336; font-size: 0.875rem; }
    button { 
      padding: 0.75rem 1.5rem; 
      margin-right: 0.5rem; 
      cursor: pointer; 
    }
    button:disabled { opacity: 0.5; cursor: not-allowed; }
  `]
})
export class UserFormComponent {
  // Create the form
  userForm = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50)
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),
    phone: new FormControl('', [
      Validators.pattern(/^\+?[\d\s-()]+$/)
    ])
  });
  
  // Check if field is invalid
  isFieldInvalid(fieldName: string): boolean {
    const field = this.userForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }
  
  // Submit form
  onSubmit(): void {
    if (this.userForm.valid) {
      console.log('Form Submitted:', this.userForm.value);
      // Call API to save data
    } else {
      // Mark all fields as touched to show errors
      this.userForm.markAllAsTouched();
    }
  }
  
  // Reset form
  resetForm(): void {
    this.userForm.reset({
      name: '',
      email: '',
      phone: ''
    });
  }
}
```

### FormBuilder (Cleaner Syntax)

```typescript
// Using FormBuilder for cleaner syntax
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `...`
})
export class UserFormComponent {
  private fb = inject(FormBuilder);
  
  // Using FormBuilder
  userForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    phone: ['']
  });
}
```

### Nested Forms

```typescript
// Creating nested form groups
@Component({...})
export class ProfileFormComponent {
  private fb = inject(FormBuilder);
  
  profileForm = this.fb.group({
    personal: this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['']
    }),
    address: this.fb.group({
      street: [''],
      city: [''],
      state: [''],
      zipCode: ['', Validators.pattern(/^\d{5}$/)]
    }),
    // Nested form for preferences
    preferences: this.fb.group({
      notifications: [true],
      newsletter: [false],
      theme: ['light']
    })
  });
  
  // Template usage
  // <div formGroupName="personal">
  //   <input formControlName="firstName">
  // </div>
  // <div formGroupName="address">
  //   <input formControlName="city">
  // </div>
}
```

### FormArray (Dynamic Fields)

```typescript
// FormArray for dynamic fields
@Component({...})
export class SkillsFormComponent {
  private fb = inject(FormBuilder);
  
  skillsForm = this.fb.group({
    skills: this.fb.array([])  // Start with empty array
  });
  
  // Getter for easy access
  get skills(): FormArray {
    return this.skillsForm.get('skills') as FormArray;
  }
  
  // Add a new skill
  addSkill(): void {
    const skillGroup = this.fb.group({
      name: ['', Validators.required],
      level: ['intermediate']
    });
    this.skills.push(skillGroup);
  }
  
  // Remove a skill
  removeSkill(index: number): void {
    this.skills.removeAt(index);
  }
  
  // Template
  // <div formArrayName="skills">
  //   @for (skill of skills.controls; track skill; let i = $index) {
  //     <div [formGroupName]="i">
  //       <input formControlName="name">
  //       <input formControlName="level">
  //       <button (click)="removeSkill(i)">Remove</button>
  //     </div>
  //   }
  // </div>
  // <button (click)="addSkill()">Add Skill</button>
}
```

### Custom Validators

#### Custom Validator Function

```typescript
// custom-validators.ts
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

// Custom validator for matching fields
export function matchingFieldsValidator(
  field1: string, 
  field2: string
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as FormGroup;
    const value1 = formGroup.get(field1)?.value;
    const value2 = formGroup.get(field2)?.value;
    
    if (value1 !== value2) {
      return { mismatch: { field1, field2 } };
    }
    return null;
  };
}

// Custom validator for forbidden values
export function forbiddenValueValidator(forbiddenValues: string[]): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (forbiddenValues.includes(control.value.toLowerCase())) {
      return { forbidden: { value: control.value } };
    }
    return null;
  };
}

// Async validator example
export function asyncUsernameValidator(): ValidatorFn {
  return (control: AbstractControl): Observable<ValidationErrors | null> => {
    return this.http.get(`/api/users/exists?username=${control.value}`).pipe(
      map(exists => exists ? { usernameTaken: true } : null),
      catchError(() => of(null))
    );
  };
}
```

#### Using Custom Validators

```typescript
// Component using custom validators
@Component({...})
export class RegistrationFormComponent {
  private fb = inject(FormBuilder);
  
  registrationForm = this.fb.group({
    username: ['', [
      Validators.required,
      Validators.minLength(3),
      forbiddenValueValidator(['admin', 'root', 'system'])
    ]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required,
      Validators.minLength(8)
    ]],
    confirmPassword: ['', Validators.required]
  }, {
    validators: matchingFieldsValidator('password', 'confirmPassword')
  });
}
```

### Form Events and Status Changes

```typescript
// Listening to form changes
@Component({...})
export class MyComponent {
  private fb = inject(FormBuilder);
  
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });
  
  ngOnInit(): void {
    // Value changes
    this.form.valueChanges.subscribe(value => {
      console.log('Form value:', value);
    });
    
    // Status changes
    this.form.statusChanges.subscribe(status => {
      console.log('Form status:', status); // VALID, INVALID, PENDING
    });
    
    // Individual field changes
    this.form.get('email')?.valueChanges.subscribe(value => {
      console.log('Email changed:', value);
    });
  }
}
```

## Complete Example: Complete User Registration

```typescript
// registration.component.ts
import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { 
  ReactiveFormsModule, 
  FormBuilder, 
  FormGroup, 
  FormArray, 
  Validators,
  AbstractControl 
} from '@angular/forms';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="registration-container">
      <h1>User Registration</h1>
      
      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <!-- Personal Information -->
        <fieldset formGroupName="personal">
          <legend>Personal Information</legend>
          
          <div class="form-group">
            <label>First Name *</label>
            <input formControlName="firstName" [class.error]="isInvalid('personal.firstName')">
            @if (isInvalid('personal.firstName')) {
              <span class="error">First name is required</span>
            }
          </div>
          
          <div class="form-group">
            <label>Last Name *</label>
            <input formControlName="lastName" [class.error]="isInvalid('personal.lastName')">
            @if (isInvalid('personal.lastName')) {
              <span class="error">Last name is required</span>
            }
          </div>
          
          <div class="form-group">
            <label>Email *</label>
            <input formControlName="email" type="email" [class.error]="isInvalid('personal.email')">
            @if (isInvalid('personal.email')) {
              <span class="error">Valid email is required</span>
            }
          </div>
        </fieldset>
        
        <!-- Address -->
        <fieldset formGroupName="address">
          <legend>Address</legend>
          
          <div class="form-group">
            <label>Street</label>
            <input formControlName="street">
          </div>
          
          <div class="form-group">
            <label>City</label>
            <input formControlName="city">
          </div>
          
          <div class="form-row">
            <div class="form-group">
              <label>State</label>
              <input formControlName="state">
            </div>
            <div class="form-group">
              <label>Zip Code</label>
              <input formControlName="zipCode" [class.error]="isInvalid('address.zipCode')">
              @if (isInvalid('address.zipCode')) {
                <span class="error">Invalid zip code</span>
              }
            </div>
          </div>
        </fieldset>
        
        <!-- Dynamic Skills -->
        <fieldset>
          <legend>Skills</legend>
          
          <div formArrayName="skills">
            @for (skill of skillsArray.controls; track skill; let i = $index) {
              <div class="skill-row" [formGroupName]="i">
                <input formControlName="name" placeholder="Skill name">
                <select formControlName="level">
                  <option value="beginner">Beginner</option>
                  <option value="intermediate">Intermediate</option>
                  <option value="advanced">Advanced</option>
                </select>
                <button type="button" (click)="removeSkill(i)">Remove</button>
              </div>
            }
          </div>
          
          <button type="button" (click)="addSkill()" class="add-btn">+ Add Skill</button>
        </fieldset>
        
        <!-- Terms -->
        <div class="form-group checkbox">
          <input type="checkbox" formControlName="acceptTerms" id="terms">
          <label for="terms">I accept the terms and conditions *</label>
        </div>
        @if (isInvalid('acceptTerms')) {
          <span class="error">You must accept the terms</span>
        }
        
        <!-- Submit -->
        <div class="form-actions">
          <button type="submit" [disabled]="form.invalid || isSubmitting()">
            {{ isSubmitting() ? 'Submitting...' : 'Register' }}
          </button>
          <button type="button" (click)="resetForm()">Reset</button>
        </div>
      </form>
      
      <!-- Debug -->
      <div class="debug">
        <h3>Form Status</h3>
        <p>Valid: {{ form.valid }}</p>
        <p>Touched: {{ form.touched }}</p>
        <p>Value: {{ form.value | json }}</p>
      </div>
    </div>
  `,
  styles: [`
    .registration-container { max-width: 600px; margin: 2rem auto; }
    fieldset { border: 1px solid #ddd; padding: 1rem; margin-bottom: 1rem; }
    legend { font-weight: bold; padding: 0 0.5rem; }
    .form-group { margin-bottom: 1rem; }
    .form-row { display: flex; gap: 1rem; }
    .form-row .form-group { flex: 1; }
    label { display: block; margin-bottom: 0.25rem; }
    input, select { 
      width: 100%; 
      padding: 0.5rem; 
      border: 1px solid #ddd; 
      border-radius: 4px; 
    }
    input.error { border-color: #f44336; }
    .error { color: #f44336; font-size: 0.875rem; }
    .checkbox { display: flex; align-items: center; gap: 0.5rem; }
    .checkbox label { margin: 0; }
    .form-actions { display: flex; gap: 1rem; margin-top: 1rem; }
    .form-actions button { 
      padding: 0.75rem 1.5rem; 
      cursor: pointer; 
    }
    .skill-row { display: flex; gap: 0.5rem; margin-bottom: 0.5rem; }
    .add-btn { background: #4caf50; color: white; border: none; padding: 0.5rem 1rem; }
    .debug { margin-top: 2rem; padding: 1rem; background: #f5f5f5; }
  `]
})
export class RegistrationComponent {
  private fb = inject(FormBuilder);
  
  isSubmitting = signal(false);
  
  form = this.fb.group({
    personal: this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    }),
    address: this.fb.group({
      street: [''],
      city: [''],
      state: [''],
      zipCode: ['', Validators.pattern(/^\d{5}(-\d{4})?$/)]
    }),
    skills: this.fb.array([]),
    acceptTerms: [false, Validators.requiredTrue]
  });
  
  get skillsArray(): FormArray {
    return this.form.get('skills') as FormArray;
  }
  
  addSkill(): void {
    const skillGroup = this.fb.group({
      name: ['', Validators.required],
      level: ['intermediate']
    });
    this.skillsArray.push(skillGroup);
  }
  
  removeSkill(index: number): void {
    this.skillsArray.removeAt(index);
  }
  
  isInvalid(path: string): boolean {
    const control = this.form.get(path);
    return control ? control.invalid && control.touched : false;
  }
  
  onSubmit(): void {
    if (this.form.valid) {
      this.isSubmitting.set(true);
      console.log('Form submitted:', this.form.value);
      
      // Simulate API call
      setTimeout(() => {
        this.isSubmitting.set(false);
        alert('Registration successful!');
      }, 1500);
    } else {
      this.form.markAllAsTouched();
    }
  }
  
  resetForm(): void {
    this.form.reset();
    this.skillsArray.clear();
  }
}
```

## Best Practices

### 1. Use FormBuilder

```typescript
// Good: Clean syntax with FormBuilder
form = this.fb.group({
  name: ['', Validators.required],
  email: ['', [Validators.required, Validators.email]]
});

// Avoid: Verbose FormControl creation
name = new FormControl('', Validators.required);
```

### 2. Use Typed Forms (Angular 14+)

```typescript
// Good: Fully typed forms
interface UserForm {
  name: FormControl<string>;
  email: FormControl<string>;
  age: FormControl<number>;
}

form = new FormGroup<UserForm>({
  name: new FormControl('', { nonNullable: true }),
  email: new FormControl('', { nonNullable: true }),
  age: new FormControl(0, { nonNullable: true })
});
```

### 3. Handle Form State Properly

```typescript
// Good: Mark as touched to show errors
onSubmit(): void {
  if (this.form.invalid) {
    this.form.markAllAsTouched();
    return;
  }
  // Process valid form
}
```

### 4. Use Async Pipe with Observables

```typescript
// Good: Automatic subscription management
@Component({
  template: `
    <div *ngFor="let user of users$ | async">{{ user.name }}</div>
  `
})
export class MyComponent {
  users$ = this.userService.getUsers();
}
```

## Common Pitfalls and Debugging

### Pitfall 1: Form Not Resetting

```typescript
// Problem: Form values persist after reset
this.form.reset(); // Sometimes doesn't clear all values

// Solution: Pass default values to reset
this.form.reset({
  name: '',
  email: '',
  age: 0
});
```

### Pitfall 2: Validation Not Triggering

```typescript
// Problem: Validation not showing
<input formControlName="name">

// Solution: Mark as touched when losing focus
<input formControlName="name" (blur)="form.get('name')?.markAsTouched()">
```

### Pitfall 3: Nested Form Validation

```typescript
// Problem: Can't access nested form controls
this.form.get('address.city')?.valid // undefined sometimes

// Solution: Use get with full path or typed access
this.form.get('address.city')?.valid;
// or
(this.form.get('address') as FormGroup).get('city')?.valid;
```

## Hands-On Exercise

### Exercise 2.4: Reactive Form Implementation

**Objective**: Build a complete form with validation

**Requirements**:
1. Create user registration form
2. Add nested form groups (personal, address)
3. Implement custom validators
4. Add dynamic FormArray for skills/interests
5. Handle form submission

**Deliverable**: Complete registration form

**Assessment Criteria**:
- [ ] All fields properly validated
- [ ] Custom validators working
- [ ] FormArray for dynamic fields
- [ ] Error messages displayed
- [ ] Form reset functionality

## Summary

- **Reactive Forms** provide programmatic form management
- **FormBuilder** simplifies form creation
- **FormGroup** groups related controls
- **FormArray** handles dynamic fields
- **Validators** enforce data integrity
- Use **FormBuilder** and **typed forms** for best practices

## Suggested Reading

- [Angular Reactive Forms Documentation](https://angular.io/guide/reactive-forms)
- "Angular Forms" - Official Guide

## Next Steps

In the next lecture, we'll explore RxJS Observables for reactive programming.
