# Angular Directives

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand the three types of directives in Angular
- [ ] Use built-in structural directives (@if, @for, @switch)
- [ ] Use built-in attribute directives (ngClass, ngStyle, ngModel)
- [ ] Create custom attribute directives
- [ ] Create custom structural directives
- [ ] Apply directives for DOM manipulation

## Conceptual Explanation

**Visual Analogy**: Think of directives as **special instructions** you give to Angular about how to handle elements in your DOM. Just like a director gives instructions to actors on a stage, directives tell Angular how to display, hide, repeat, or style elements!

### Types of Directives

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Angular Directive Types                           │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  1. COMPONENT DIRECTIVES                                            │
│     ┌─────────────────────────────────────────────────────────┐    │
│     │  @Component()                                          │    │
│     │  Template + Logic + Styles                             │    │
│     │  Example: <app-user-profile></app-user-profile>       │    │
│     └─────────────────────────────────────────────────────────┘    │
│                                                                     │
│  2. ATTRIBUTE DIRECTIVES                                            │
│     ┌─────────────────────────────────────────────────────────┐    │
│     │  Modify element appearance/behavior                    │    │
│     │  Examples: [class], [style], [ngClass], [ngStyle]     │    │
│     └─────────────────────────────────────────────────────────┘    │
│                                                                     │
│  3. STRUCTURAL DIRECTIVES                                           │
│     ┌─────────────────────────────────────────────────────────┐    │
│     │  Add/Remove elements from DOM                         │    │
│     │  Examples: @if, @for, @switch, *ngIf, *ngFor         │    │
│     └─────────────────────────────────────────────────────────┘    │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why Directives Matter

1. **DOM Manipulation**: Safely modify the DOM without direct access
2. **Reusability**: Create reusable UI behaviors
3. **Separation of Concerns**: Separate view logic from business logic
4. **Performance**: Optimized change detection

### Industry Use Cases

- **UI Frameworks**: Material Design, Bootstrap Angular
- **Form Validation**: Custom validation directives
- **Permissions**: Show/hide elements based on user roles
- **Lazy Loading**: Conditional content loading

## Step-by-Step Walkthrough

### 1. Built-in Structural Directives (Angular 17+ Control Flow)

#### @if - Conditional Rendering

```typescript
@Component({
  selector: 'app-conditional',
  standalone: true,
  template: `
    <!-- Basic if -->
    @if (isLoggedIn) {
      <p>Welcome back!</p>
    }
    
    <!-- if-else with else block -->
    @if (hasPremiumAccess) {
      <button>Premium Content</button>
    } @else {
      <button>Upgrade to Premium</button>
    }
    
    <!-- if-else-if-else chain -->
    @if (userRole === 'admin') {
      <p>Admin Dashboard</p>
    } @else if (userRole === 'user') {
      <p>User Dashboard</p>
    } @else {
      <p>Guest Dashboard</p>
    }
  `
})
export class ConditionalComponent {
  isLoggedIn = true;
  hasPremiumAccess = false;
  userRole = 'admin';
}
```

#### @for - Loop Rendering

```typescript
@Component({
  selector: 'app-loop',
  standalone: true,
  template: `
    <!-- Basic for loop -->
    @for (item of items; track item.id) {
      <div>{{ item.name }}</div>
    }
    
    <!-- For loop with index -->
    @for (item of items; track item.id; let i = $index) {
      <div>{{ i + 1 }}. {{ item.name }}</div>
    }
    
    <!-- Empty collection fallback -->
    @for (item of items; track item.id) {
      <div>{{ item.name }}</div>
    } @empty {
      <p>No items found</p>
    }
    
    <!-- For loop with count -->
    @for (item of items; track item.id; let count = $count) {
      <p>Total items: {{ count }}</p>
    }
    
    <!-- First and last -->
    @for (item of items; track item.id; let first = $first; let last = $last) {
      <div [class.first]="first" [class.last]="last">
        {{ item.name }}
      </div>
    }
  `
})
export class LoopComponent {
  items = [
    { id: 1, name: 'Apple' },
    { id: 2, name: 'Banana' },
    { id: 3, name: 'Orange' }
  ];
}
```

#### @switch - Multiple Conditions

```typescript
@Component({
  selector: 'app-switch-demo',
  standalone: true,
  template: `
    @switch (status) {
      @case ('loading') {
        <p>Loading...</p>
      }
      @case ('success') {
        <p>Data loaded successfully!</p>
      }
      @case ('error') {
        <p>Error loading data</p>
      }
      @default {
        <p>Unknown status</p>
      }
    }
  `
})
export class SwitchDemoComponent {
  status = 'success';
}
```

### 2. Built-in Attribute Directives

#### ngClass - Dynamic Classes

```typescript
@Component({
  selector: 'app-class-demo',
  standalone: true,
  template: `
    <!-- Object syntax -->
    <div [ngClass]="{
      'active': isActive,
      'disabled': isDisabled,
      'highlighted': hasHighlight
    }">
      Object binding
    </div>
    
    <!-- Array syntax -->
    <div [ngClass]="['class-one', 'class-two']">
      Array binding
    </div>
    
    <!-- String syntax -->
    <div [ngClass]="'class-one class-two'">
      String binding
    </div>
  `
})
export class ClassDemoComponent {
  isActive = true;
  isDisabled = false;
  hasHighlight = true;
}
```

#### ngStyle - Dynamic Styles

```typescript
@Component({
  selector: 'app-style-demo',
  standalone: true,
  template: `
    <!-- Object syntax -->
    <div [ngStyle]="{
      'color': textColor,
      'font-size.px': fontSize,
      'background-color': bgColor
    }">
      Styled content
    </div>
  `
})
export class StyleDemoComponent {
  textColor = 'white';
  fontSize = 18;
  bgColor = '#1976d2';
}
```

### 3. Custom Attribute Directive

#### Creating a Highlight Directive

```typescript
// highlight.directive.ts
import { Directive, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appHighlight]',
  standalone: true
})
export class HighlightDirective implements OnInit {
  @Input('appHighlight') highlightColor: string = 'yellow';
  @Input() defaultColor: string = 'transparent';
  
  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}
  
  ngOnInit(): void {
    this.renderer.setStyle(
      this.el.nativeElement,
      'background-color',
      this.defaultColor
    );
  }
  
  // Method to apply highlight
  highlight(color?: string): void {
    this.renderer.setStyle(
      this.el.nativeElement,
      'background-color',
      color || this.highlightColor
    );
  }
  
  // Method to remove highlight
  removeHighlight(): void {
    this.renderer.setStyle(
      this.el.nativeElement,
      'background-color',
      this.defaultColor
    );
  }
}

// Using the directive
@Component({
  selector: 'app-highlight-demo',
  standalone: true,
  imports: [HighlightDirective],
  template: `
    <p appHighlight="lightblue">Highlighted text</p>
    <p appHighlight="lightgreen" defaultColor="lightgray">Custom highlight</p>
    <p appHighlight (mouseenter)="onHover()" (mouseleave)="onLeave()">
      Hover to highlight
    </p>
  `
})
export class HighlightDemoComponent {
  onHover(): void {
    // Would need ViewChild access to call directive methods
  }
}
```

### 4. Custom Structural Directive

#### Creating an IfNot Directive (Reverse ngIf)

```typescript
// if-not.directive.ts
import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appIfNot]',
  standalone: true
})
export class IfNotDirective {
  @Input() set appIfNot(condition: boolean) {
    if (!condition) {
      // Create view if condition is false
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      // Clear view if condition is true
      this.viewContainer.clear();
    }
  }
  
  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef
  ) {}
}

// Using the directive
@Component({
  selector: 'app-if-not-demo',
  standalone: true,
  imports: [IfNotDirective],
  template: `
    <div *appIfNot="isHidden">
      This shows when isHidden is false
    </div>
  `
})
export class IfNotDemoComponent {
  isHidden = false;
}
```

### Complete Example: User List with Directives

```typescript
// user.model.ts
export interface User {
  id: number;
  name: string;
  email: string;
  role: 'admin' | 'user' | 'guest';
  status: 'active' | 'inactive';
  avatar?: string;
}

// role-badge.directive.ts
import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appRoleBadge]',
  standalone: true
})
export class RoleBadgeDirective {
  @Input() set appRoleBadge(role: string) {
    const colors: Record<string, string> = {
      admin: '#d32f2f',
      user: '#1976d2',
      guest: '#757575'
    };
    
    this.renderer.setStyle(
      this.el.nativeElement,
      'background-color',
      colors[role] || '#999'
    );
  }
  
  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}
}

// user-list.component.ts
import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { User, UserService } from './user.service';
import { RoleBadgeDirective } from './role-badge.directive';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RoleBadgeDirective],
  template: `
    <div class="user-list-container">
      <h1>User Management</h1>
      
      <!-- Filter Controls -->
      <div class="filters">
        <input 
          [(ngModel)]="searchTerm" 
          placeholder="Search users..."
          (input)="filterUsers()">
        
        <select [(ngModel)]="roleFilter" (change)="filterUsers()">
          <option value="">All Roles</option>
          <option value="admin">Admin</option>
          <option value="user">User</option>
          <option value="guest">Guest</option>
        </select>
        
        <label>
          <input type="checkbox" [(ngModel)]="showActiveOnly" (change)="filterUsers()">
          Show Active Only
        </label>
      </div>
      
      <!-- User Table with @for -->
      <table class="user-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          @for (user of filteredUsers(); track user.id; let i = $index) {
            <tr [class.inactive]="user.status === 'inactive'"
                [class.admin]="user.role === 'admin'">
              <td>{{ i + 1 }}</td>
              <td>{{ user.name }}</td>
              <td>{{ user.email }}</td>
              <td>
                <span 
                  class="role-badge" 
                  [appRoleBadge]="user.role">
                  {{ user.role }}
                </span>
              </td>
              <td>
                <span [class.status-active]="user.status === 'active'"
                      [class.status-inactive]="user.status === 'inactive'">
                  {{ user.status }}
                </span>
              </td>
            </tr>
          } @empty {
            <tr>
              <td colspan="5" class="no-users">No users found</td>
            </tr>
          }
        </tbody>
      </table>
      
      <!-- Status summary -->
      <div class="summary">
        @switch (roleFilter) {
          @case ('admin') {
            <p>Showing {{ adminCount() }} admins</p>
          }
          @case ('user') {
            <p>Showing {{ userCount() }} users</p>
          }
          @case ('guest') {
            <p>Showing {{ guestCount() }} guests</p>
          }
          @default {
            <p>Total: {{ filteredUsers().length }} users</p>
          }
        }
      </div>
    </div>
  `,
  styles: [`
    .user-list-container {
      padding: 1rem;
      max-width: 800px;
      margin: 0 auto;
    }
    
    .filters {
      display: flex;
      gap: 1rem;
      margin-bottom: 1rem;
      padding: 1rem;
      background: #f5f5f5;
      
      input, select {
        padding: 0.5rem;
      }
    }
    
    .user-table {
      width: 100%;
      border-collapse: collapse;
      
      th, td {
        padding: 0.75rem;
        text-align: left;
        border-bottom: 1px solid #ddd;
      }
      
      th {
        background: #fafafa;
        font-weight: 600;
      }
      
      tr:hover {
        background: #f9f9f9;
      }
      
      tr.inactive {
        opacity: 0.6;
      }
      
      tr.admin {
        border-left: 3px solid #d32f2f;
      }
    }
    
    .role-badge {
      padding: 0.25rem 0.5rem;
      border-radius: 4px;
      color: white;
      font-size: 0.75rem;
      text-transform: uppercase;
    }
    
    .status-active { color: #4caf50; }
    .status-inactive { color: #f44336; }
    
    .no-users {
      text-align: center;
      color: #888;
      padding: 2rem;
    }
    
    .summary {
      margin-top: 1rem;
      padding: 1rem;
      background: #e3f2fd;
    }
  `]
})
export class UserListComponent {
  users = signal<User[]>([
    { id: 1, name: 'John Doe', email: 'john@example.com', role: 'admin', status: 'active' },
    { id: 2, name: 'Jane Smith', email: 'jane@example.com', role: 'user', status: 'active' },
    { id: 3, name: 'Bob Wilson', email: 'bob@example.com', role: 'user', status: 'inactive' },
    { id: 4, name: 'Alice Brown', email: 'alice@example.com', role: 'guest', status: 'active' },
    { id: 5, name: 'Charlie Davis', email: 'charlie@example.com', role: 'admin', status: 'active' }
  ]);
  
  filteredUsers = signal<User[]>([]);
  searchTerm = '';
  roleFilter = '';
  showActiveOnly = false;
  
  adminCount = () => this.users().filter(u => u.role === 'admin').length;
  userCount = () => this.users().filter(u => u.role === 'user').length;
  guestCount = () => this.users().filter(u => u.role === 'guest').length;
  
  constructor() {
    this.filterUsers();
  }
  
  filterUsers(): void {
    let result = this.users();
    
    // Search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      result = result.filter(u => 
        u.name.toLowerCase().includes(term) || 
        u.email.toLowerCase().includes(term)
      );
    }
    
    // Role filter
    if (this.roleFilter) {
      result = result.filter(u => u.role === this.roleFilter);
    }
    
    // Status filter
    if (this.showActiveOnly) {
      result = result.filter(u => u.status === 'active');
    }
    
    this.filteredUsers.set(result);
  }
}
```

## Best Practices

### 1. Use Control Flow Syntax (Angular 17+)

```typescript
// Good: New control flow
@if (condition) { <div>Content</div> }
@for (item of items; track item.id) { <div>{{ item.name }}</div> }

// Avoid: Old *ngIf and *ngFor
<div *ngIf="condition">Content</div>
<div *ngFor="let item of items">{{ item.name }}</div>
```

### 2. Always Use track in @for

```typescript
// Good: track by unique identifier
@for (item of items; track item.id) { ... }

// Avoid: No track or track by index (performance issue)
@for (item of items) { ... }
```

### 3. Prefer Signals for Complex Logic

```typescript
// Good: Using computed signals
@Component({...})
export class MyComponent {
  items = signal<Item[]>([]);
  filteredItems = computed(() => 
    this.items().filter(i => i.active)
  );
}
```

## Common Pitfalls and Debugging

### Pitfall 1: Missing Import

```typescript
// Error: Can't bind to 'ngModel' since it isn't a known property
// Solution: Import FormsModule
@Component({
  standalone: true,
  imports: [FormsModule],  // Required for ngModel
  template: `<input [(ngModel)]="value">`
})
export class MyComponent {}
```

### Pitfall 2: Null Checks in @for

```typescript
// Problem: Error if items is null
@for (item of items; track item.id) { }

// Solution: Use empty fallback or null coalescing
@for (item of items ?? []; track item.id) { }
```

### Pitfall 3: Incorrect Directive Selector

```typescript
// Problem: Selector without brackets for attribute
@Directive({ selector: 'appHighlight' })  // Looks for <appHighlight>

// Solution: Add brackets for attribute selector
@Directive({ selector: '[appHighlight]' })  // Looks for <element appHighlight>
```

## Hands-On Exercise

### Exercise 1.7: Directive Implementation

**Objective**: Create a feature-rich product grid

**Requirements**:
1. Use @if for loading/error states
2. Use @for with track for product list
3. Use @switch for category display
4. Create a custom directive for product highlighting
5. Use ngClass for dynamic styling

**Deliverable**: Product grid with custom directives

**Assessment Criteria**:
- [ ] All three control flow directives used
- [ ] Custom attribute directive created
- [ ] Track by unique identifier in @for
- [ ] Empty state handled
- [ ] Responsive grid layout

## Summary

- **Three types**: Components, Attribute directives, Structural directives
- **Control flow**: Use @if, @for, @switch (Angular 17+)
- **Built-in attribute**: ngClass, ngStyle, ngModel
- **Custom directives**: Create with @Directive decorator
- **Best practice**: Always use `track` in @for loops

## Next Steps

In the next lecture, we'll explore Angular Pipes for data transformation.
