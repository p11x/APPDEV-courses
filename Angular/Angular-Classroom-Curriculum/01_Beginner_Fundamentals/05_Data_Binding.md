# Data Binding in Angular

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand all types of data binding in Angular
- [ ] Implement interpolation, property binding, and event binding
- [ ] Use two-way binding with ngModel
- [ ] Apply attribute binding and class/style binding
- [ ] Understand Angular's change detection mechanism

## Conceptual Explanation

**Visual Analogy**: Think of data binding as a **two-way bridge** between your component logic (TypeScript) and the template (HTML). Just like a bridge allows vehicles (data) to travel in both directions, Angular's data binding allows information to flow between your code and the UI!

### Types of Data Binding

```
┌─────────────────────────────────────────────────────────────────────┐
│                    Angular Data Binding Types                       │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  ┌─────────────────┐          ┌─────────────────┐                 │
│  │   Component     │          │      View       │                 │
│  │   (TypeScript)  │◄────────►│     (HTML)      │                 │
│  └─────────────────┘          └─────────────────┘                 │
│                                                                     │
│  1. Interpolation     {{ value }}      Component → View            │
│  2. Property Binding  [property]       Component → View            │
│  3. Event Binding     (event)          View → Component           │
│  4. Two-Way Binding   [(ngModel)]      Both Directions             │
│  5. Attribute Binding [attr.xxx]       Component → View            │
│  6. Class Binding     [class.xxx]      Component → View            │
│  7. Style Binding     [style.xxx]      Component → View            │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why Data Binding Matters

1. **Declarative UI**: Focus on what to display, not how to update
2. **Reduced Boilerplate**: No manual DOM manipulation
3. **Maintainability**: Clear data flow is easier to debug
4. **Reactivity**: UI automatically updates when data changes

### Industry Use Cases

- **Forms**: Real-time validation and feedback
- **Dashboards**: Live data visualization
- **User Profiles**: Dynamic UI updates
- **E-commerce**: Cart updates, price calculations

## Step-by-Step Walkthrough

### 1. Interpolation

Displays component data in the template.

```typescript
// component.ts
@Component({
  selector: 'app-user',
  standalone: true,
  template: `
    <h1>Welcome, {{ userName }}!</h1>
    <p>Your score is: {{ score }}</p>
    <p>Double score: {{ score * 2 }}</p>
    <p>Status: {{ isActive ? 'Active' : 'Inactive' }}</p>
    <p>Full name: {{ getFullName() }}</p>
  `
})
export class UserComponent {
  userName: string = 'John';
  score: number = 100;
  isActive: boolean = true;
  
  getFullName(): string {
    return 'John Doe';
  }
}
```

### 2. Property Binding

Binds component properties to HTML element properties.

```typescript
// component.ts
@Component({
  selector: 'app-image',
  standalone: true,
  template: `
    <!-- Property binding -->
    <img [src]="imageUrl" [alt]="imageAlt">
    <button [disabled]="isDisabled">Click Me</button>
    <div [hidden]="isHidden">I'm visible!</div>
    <a [href]="linkUrl">External Link</a>
    <input [value]="inputValue">
  `
})
export class ImageComponent {
  imageUrl: string = 'https://picsum.photos/200';
  imageAlt: string = 'Random image';
  isDisabled: boolean = false;
  isHidden: boolean = false;
  linkUrl: string = 'https://angular.io';
  inputValue: string = 'Hello';
}
```

### 3. Event Binding

Handles user interactions by binding to DOM events.

```typescript
// component.ts
@Component({
  selector: 'app-button',
  standalone: true,
  template: `
    <button (click)="onClick()">Click Me</button>
    <button (click)="onClickWithEvent($event)">Click with Event</button>
    <input (input)="onInput($event)">
    <div (mousedown)="onMouseDown()">Mouse here</div>
    <form (submit)="onSubmit($event)">
      <button type="submit">Submit</button>
    </form>
  `
})
export class ButtonComponent {
  onClick(): void {
    console.log('Button clicked!');
  }
  
  onClickWithEvent(event: MouseEvent): void {
    console.log('Event:', event);
    console.log('Clicked element:', event.target);
  }
  
  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    console.log('Input value:', input.value);
  }
  
  onMouseDown(): void {
    console.log('Mouse down!');
  }
  
  onSubmit(event: Event): void {
    event.preventDefault();
    console.log('Form submitted!');
  }
}
```

### 4. Two-Way Binding

Synchronizes data between component and template.

```typescript
// component.ts
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h1>Two-Way Binding Demo</h1>
    
    <!-- Using ngModel -->
    <input [(ngModel)]="username" placeholder="Username">
    <p>Hello, {{ username }}!</p>
    
    <!-- Full syntax (equivalent) -->
    <input [ngModel]="email" (ngModelChange)="email = $event" placeholder="Email">
    <p>Your email: {{ email }}</p>
  `
})
export class FormComponent {
  username: string = '';
  email: string = '';
}
```

### 5. Attribute Binding

Binds to HTML attributes (not properties).

```typescript
// component.ts
@Component({
  selector: 'app-table',
  standalone: true,
  template: `
    <!-- Attribute binding -->
    <table>
      <tr>
        <td [attr.colspan]="colspanValue">Merged Cell</td>
      </tr>
    </table>
    
    <!-- ARIA attributes -->
    <button [attr.aria-label]="buttonLabel">Click</button>
    
    <!-- Data attributes -->
    <div [attr.data-id]="itemId">Item</div>
  `
})
export class TableComponent {
  colspanValue: number = 2;
  buttonLabel: string = 'Click to submit';
  itemId: string = '12345';
}
```

### 6. Class and Style Binding

Dynamically add classes and styles.

```typescript
// component.ts
@Component({
  selector: 'app-styles',
  standalone: true,
  template: `
    <!-- Class binding -->
    <div [class.active]="isActive" 
         [class.highlighted]="isHighlighted"
         [class.disabled]="isDisabled">
      Class Binding Demo
    </div>
    
    <!-- Multiple classes with ngClass -->
    <div [ngClass]="{
      'active': isActive,
      'highlighted': isHighlighted,
      'disabled': isDisabled
    }">
      ngClass Demo
    </div>
    
    <!-- Style binding -->
    <div [style.color]="textColor" 
         [style.font-size.px]="fontSize"
         [style.background-color]="bgColor">
      Style Binding Demo
    </div>
    
    <!-- Multiple styles with ngStyle -->
    <div [ngStyle]="{
      'color': textColor,
      'font-size.px': fontSize,
      'background-color': bgColor
    }">
      ngStyle Demo
    </div>
  `
})
export class StylesComponent {
  isActive: boolean = true;
  isHighlighted: boolean = true;
  isDisabled: boolean = false;
  
  textColor: string = 'white';
  fontSize: number = 18;
  bgColor: string = '#1976d2';
}
```

## Complete Example: Todo List with All Binding Types

```typescript
// todo.model.ts
export interface Todo {
  id: number;
  title: string;
  completed: boolean;
  priority: 'low' | 'medium' | 'high';
}

// todo.component.ts
import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Todo } from './todo.model';

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="todo-container">
      <h1>Todo List</h1>
      
      <!-- Input section with two-way binding -->
      <div class="add-todo">
        <input 
          [(ngModel)]="newTodoTitle" 
          (keyup.enter)="addTodo()"
          placeholder="Add a new todo...">
        <select [(ngModel)]="newTodoPriority">
          <option value="low">Low</option>
          <option value="medium">Medium</option>
          <option value="high">High</option>
        </select>
        <button (click)="addTodo()">Add</button>
      </div>
      
      <!-- Statistics with interpolation -->
      <div class="stats">
        <span>Total: {{ todos().length }}</span>
        <span>Completed: {{ completedCount() }}</span>
        <span>Pending: {{ pendingCount() }}</span>
      </div>
      
      <!-- Todo list with class binding and event binding -->
      <ul class="todo-list">
        @for (todo of todos(); track todo.id) {
          <li 
            [class.completed]="todo.completed"
            [class.high-priority]="todo.priority === 'high'"
            [class.medium-priority]="todo.priority === 'medium'">
            
            <!-- Checkbox with property and event binding -->
            <input 
              type="checkbox"
              [checked]="todo.completed"
              (change)="toggleTodo(todo.id)">
            
            <!-- Interpolation -->
            <span class="todo-title">{{ todo.title }}</span>
            
            <!-- Style binding -->
            <span 
              class="priority-badge"
              [style.background-color]="getPriorityColor(todo.priority)">
              {{ todo.priority }}
            </span>
            
            <!-- Event binding -->
            <button (click)="deleteTodo(todo.id)" class="delete-btn">
              Delete
            </button>
          </li>
        }
      </ul>
      
      <!-- Conditional display with interpolation -->
      @if (todos().length === 0) {
        <p class="empty-message">No todos yet. Add one above!</p>
      }
    </div>
  `,
  styles: [`
    .todo-container {
      max-width: 600px;
      margin: 2rem auto;
      padding: 1rem;
    }
    .add-todo {
      display: flex;
      gap: 0.5rem;
      margin-bottom: 1rem;
      
      input {
        flex: 1;
        padding: 0.5rem;
      }
    }
    .stats {
      display: flex;
      gap: 1rem;
      margin-bottom: 1rem;
      padding: 0.5rem;
      background: #f5f5f5;
    }
    .todo-list {
      list-style: none;
      padding: 0;
      
      li {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        padding: 0.75rem;
        border: 1px solid #ddd;
        margin-bottom: 0.5rem;
        
        &.completed .todo-title {
          text-decoration: line-through;
          color: #888;
        }
        
        &.high-priority {
          border-left: 4px solid #f44336;
        }
        
        &.medium-priority {
          border-left: 4px solid #ff9800;
        }
        
        .todo-title {
          flex: 1;
        }
        
        .priority-badge {
          padding: 0.25rem 0.5rem;
          border-radius: 4px;
          font-size: 0.75rem;
          color: white;
        }
      }
    }
    .delete-btn {
      background: #f44336;
      color: white;
      border: none;
      padding: 0.5rem;
      cursor: pointer;
    }
  `]
})
export class TodoComponent {
  // Using Angular signals
  todos = signal<Todo[]>([
    { id: 1, title: 'Learn Angular', completed: true, priority: 'high' },
    { id: 2, title: 'Build a project', completed: false, priority: 'medium' },
    { id: 3, title: 'Read documentation', completed: false, priority: 'low' }
  ]);
  
  newTodoTitle: string = '';
  newTodoPriority: 'low' | 'medium' | 'high' = 'medium';
  
  // Computed values
  completedCount = computed(() => 
    this.todos().filter(t => t.completed).length
  );
  
  pendingCount = computed(() => 
    this.todos().filter(t => !t.completed).length
  );
  
  addTodo(): void {
    if (this.newTodoTitle.trim()) {
      const newTodo: Todo = {
        id: Date.now(),
        title: this.newTodoTitle,
        completed: false,
        priority: this.newTodoPriority
      };
      
      this.todos.update(todos => [...todos, newTodo]);
      this.newTodoTitle = '';
    }
  }
  
  toggleTodo(id: number): void {
    this.todos.update(todos => 
      todos.map(todo => 
        todo.id === id ? { ...todo, completed: !todo.completed } : todo
      )
    );
  }
  
  deleteTodo(id: number): void {
    this.todos.update(todos => todos.filter(todo => todo.id !== id));
  }
  
  getPriorityColor(priority: string): string {
    const colors: Record<string, string> = {
      high: '#f44336',
      medium: '#ff9800',
      low: '#4caf50'
    };
    return colors[priority] || '#999';
  }
}
```

## Best Practices

### 1. Use Signals for Reactive State (Angular 16+)

```typescript
// Good: Using signals
count = signal(0);
doubleCount = computed(() => this.count() * 2);

increment() {
  this.count.update(c => c + 1);
}

// Avoid: Manual subscriptions for simple cases
count: number = 0;
ngOnInit() {
  this.dataService.getData().subscribe(data => {
    this.count = data.count; // More boilerplate
  });
}
```

### 2. Use Async Pipe

```typescript
// Good: Async pipe handles subscription automatically
@Component({
  template: `
    <div *ngFor="let user of users$ | async">
      {{ user.name }}
    </div>
  `
})
export class GoodComponent {
  users$ = this.userService.getUsers();
}

// Bad: Manual subscription (memory leak risk)
@Component({
  template: `
    <div *ngFor="let user of users">
      {{ user.name }}
    </div>
  `
})
export class BadComponent implements OnDestroy {
  users: User[] = [];
  
  ngOnInit() {
    this.userService.getUsers().subscribe(users => {
      this.users = users;
    });
  }
}
```

### 3. Avoid Complex Expressions in Templates

```typescript
// Good: Computed property
@Component({...})
export class MyComponent {
  user = signal({ firstName: 'John', lastName: 'Doe' });
  
  fullName = computed(() => `${this.user().firstName} ${this.user().lastName}`);
}

// Template
<h1>{{ fullName() }}</h1>

// Avoid: Complex expressions in template
// <h1>{{ user.firstName + ' ' + user.lastName }}</h1>
```

## Common Pitfalls and Debugging

### Pitfall 1: Expression Changed After Check

```typescript
// Problem
ngOnInit() {
  setTimeout(() => {
    this.value = 'new value'; // Error!
  }, 0);
}

// Solution: Use proper change detection
ngAfterViewInit() {
  setTimeout(() => {
    this.value = 'new value';
  }, 0);
}
```

### Pitfall 2: Null Reference Errors

```typescript
// Problem
@Component({
  template: `{{ user.name }}` // Error if user is null
})
export class MyComponent {
  user: User | null = null;
}

// Solution: Use optional chaining or ngIf
@Component({
  template: `
    {{ user?.name }}
    <!-- or -->
    @if (user) { {{ user.name }} }
  `
})
export class MyComponent {
  user: User | null = null;
}
```

### Pitfall 3: Change Detection Not Triggering

```typescript
// Problem: Mutating object
this.user.name = 'Jane'; // Not detected

// Solution: Create new reference
this.user = { ...this.user, name: 'Jane' };
// or with signals
this.user.update(u => ({ ...u, name: 'Jane' }));
```

## Hands-On Exercise

### Exercise 1.6: Data Binding Mastery

**Objective**: Create a product catalog with full data binding

**Requirements**:
1. Display product list using interpolation
2. Use property binding for product images
3. Add event handlers for add-to-cart
4. Use two-way binding for quantity selection
5. Use class binding for stock status
6. Use style binding for price display

**Deliverable**: Product catalog with all binding types

**Assessment Criteria**:
- [ ] At least 5 products displayed
- [ ] All 4 main binding types used
- [ ] Quantity selection with two-way binding
- [ ] Dynamic classes based on product properties
- [ ] Interactive shopping cart functionality

## Summary

- **Interpolation** `{{ }}`: Display values from component
- **Property Binding** `[property]`: Set element properties
- **Event Binding** `(event)`: Handle user interactions
- **Two-Way Binding** `[(ngModel)]`: Synchronize data both ways
- Use signals for reactive state management
- Use async pipe for Observable handling
- Avoid complex expressions in templates

## Suggested Reading

- [Angular Data Binding Documentation](https://angular.io/guide/binding-overview)
- "Angular Design Patterns" by Mathieu

## Next Steps

In the next lecture, we'll explore Angular Directives.
