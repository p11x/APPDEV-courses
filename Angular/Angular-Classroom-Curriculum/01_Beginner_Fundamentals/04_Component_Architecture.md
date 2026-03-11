# Component Architecture

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand the component-based architecture in Angular
- [ ] Create and configure Angular components
- [ ] Implement component communication with @Input and @Output
- [ ] Use component lifecycle hooks effectively
- [ ] Structure components for maintainability and reusability

## Conceptual Explanation

**Visual Analogy**: Think of Angular components as **LEGO blocks**. Each component is a self-contained piece that can be combined with others to build complex applications. Just as LEGO blocks have specific shapes and connection points, Angular components have inputs, outputs, and templates that define how they connect to other components!

### What is a Component?

A component controls a patch of screen called a **view**. It consists of:
- A TypeScript class with application logic
- An HTML template that defines the view
- CSS styles that define the appearance
- Optional testing specifications

```
┌─────────────────────────────────────────────────────────────┐
│                    Angular Component                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌─────────────────────────────────────────────────────┐   │
│  │                 @Component()                         │   │
│  │  ├── selector: 'app-hero'                           │   │
│  │  ├── templateUrl: './hero.component.html'           │   │
│  │  ├── styleUrls: ['./hero.component.scss']          │   │
│  │  └── standalone: true                               │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                              │
│  ┌─────────────────────────────────────────────────────┐   │
│  │               HeroComponent Class                    │   │
│  │  ├── @Input() name: string                          │   │
│  │  ├── @Output() selected = new EventEmitter()        │   │
│  │  ├── onSelect() { this.selected.emit(this.hero) }  │   │
│  │  └── lifecycle hooks: ngOnInit(), ngOnChanges()    │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why Component Architecture Matters

1. **Modularity**: Complex apps broken into manageable pieces
2. **Reusability**: Components can be used across the application
3. **Testability**: Each component can be tested in isolation
4. **Team Collaboration**: Different teams can work on different components
5. **Maintainability**: Easy to update and debug

### Industry Use Cases

- **UI Libraries**: Material Design, PrimeNG, NGX Bootstrap
- **Design Systems**: Build consistent company-wide UI
- **Micro-frontends**: Independent deployable components

## Step-by-Step Walkthrough

### Creating a Component

#### Step 1: Generate a Component

```bash
ng generate component components/hero
# or shorthand
ng g c components/hero
```

Expected output:
```
CREATE src/app/components/hero/hero.component.html (x bytes)
CREATE src/app/components/hero/hero.component.ts (x bytes)
CREATE src/app/components/hero/hero.component.scss (x bytes)
CREATE src/app/components/hero/hero.component.spec.ts (x bytes)
```

#### Step 2: Define the Component Class

```typescript
// hero.component.ts
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Hero {
  id: number;
  name: string;
  power: string;
}

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './hero.component.html',
  styleUrls: ['./hero.component.scss']
})
export class HeroComponent implements OnInit {
  // Input from parent component
  @Input() hero: Hero | null = null;
  @Input() isSelected: boolean = false;
  
  // Output to parent component
  @Output() heroSelected = new EventEmitter<Hero>();
  @Output() heroDeleted = new EventEmitter<number>();
  
  ngOnInit(): void {
    console.log('Hero component initialized');
  }
  
  onSelect(): void {
    if (this.hero) {
      this.heroSelected.emit(this.hero);
    }
  }
  
  onDelete(event: Event): void {
    event.stopPropagation();
    if (this.hero) {
      this.heroDeleted.emit(this.hero.id);
    }
  }
}
```

#### Step 3: Create the Template

```html
<!-- hero.component.html -->
<div class="hero-card" 
     [class.selected]="isSelected"
     (click)="onSelect()">
  
  @if (hero) {
    <div class="hero-header">
      <h3>{{ hero.name }}</h3>
      <span class="hero-power">{{ hero.power }}</span>
    </div>
    
    <div class="hero-actions">
      <button (click)="onDelete($event)" class="delete-btn">
        Delete
      </button>
    </div>
  } @else {
    <p class="no-hero">No hero data available</p>
  }
</div>
```

#### Step 4: Add Styles

```scss
/* hero.component.scss */
.hero-card {
  padding: 1rem;
  border: 2px solid #ddd;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  
  &:hover {
    border-color: #1976d2;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
  
  &.selected {
    border-color: #1976d2;
    background-color: #e3f2fd;
  }
  
  .hero-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    
    h3 {
      margin: 0;
      color: #333;
    }
    
    .hero-power {
      background: #ff9800;
      color: white;
      padding: 0.25rem 0.5rem;
      border-radius: 4px;
      font-size: 0.875rem;
    }
  }
  
  .hero-actions {
    margin-top: 1rem;
    
    .delete-btn {
      background: #f44336;
      color: white;
      border: none;
      padding: 0.5rem 1rem;
      border-radius: 4px;
      cursor: pointer;
      
      &:hover {
        background: #d32f2f;
      }
    }
  }
}
```

#### Step 5: Use the Component

```typescript
// app.component.ts
import { Component } from '@angular/core';
import { HeroComponent, Hero } from './components/hero/hero.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HeroComponent],
  template: `
    <h1>Hero Dashboard</h1>
    
    <div class="heroes-list">
      @for (hero of heroes; track hero.id) {
        <app-hero 
          [hero]="hero"
          [isSelected]="selectedHero?.id === hero.id"
          (heroSelected)="onHeroSelected($event)"
          (heroDeleted)="onHeroDeleted($event)">
        </app-hero>
      }
    </div>
  `
})
export class AppComponent {
  heroes: Hero[] = [
    { id: 1, name: 'Spiderman', power: 'Super Strength' },
    { id: 2, name: 'Iron Man', power: 'Technology' },
    { id: 3, name: 'Thor', power: 'Thunder' }
  ];
  
  selectedHero: Hero | null = null;
  
  onHeroSelected(hero: Hero): void {
    this.selectedHero = hero;
    console.log('Selected:', hero);
  }
  
  onHeroDeleted(id: number): void {
    this.heroes = this.heroes.filter(h => h.id !== id);
    console.log('Deleted:', id);
  }
}
```

## Component Communication Patterns

### 1. Parent to Child: @Input()

```typescript
// Parent component
@Component({
  template: `<app-child [message]="parentMessage"></app-child>`
})
export class ParentComponent {
  parentMessage = 'Hello from parent!';
}

// Child component
@Component({
  selector: 'app-child',
  standalone: true,
  template: `<p>{{ message }}</p>`
})
export class ChildComponent {
  @Input() message: string = '';
}
```

### 2. Child to Parent: @Output()

```typescript
// Child component
@Component({
  selector: 'app-child',
  standalone: true,
  template: `<button (click)="sendMessage()">Send</button>`
})
export class ChildComponent {
  @Output() messageSent = new EventEmitter<string>();
  
  sendMessage(): void {
    this.messageSent.emit('Hello from child!');
  }
}

// Parent component
@Component({
  template: `<app-child (messageSent)="onMessage($event)"></app-child>`
})
export class ParentComponent {
  onMessage(msg: string): void {
    console.log(msg);
  }
}
```

### 3. Parent Calls Child: ViewChild

```typescript
import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { ChildComponent } from './child.component';

@Component({
  selector: 'app-parent',
  standalone: true,
  imports: [ChildComponent],
  template: `
    <app-child #childRef></app-child>
    <button (click)="callChild()">Call Child</button>
  `
})
export class ParentComponent implements AfterViewInit {
  @ViewChild('childRef') childComponent!: ChildComponent;
  
  ngAfterViewInit(): void {
    // Child is now available
  }
  
  callChild(): void {
    this.childComponent.childMethod();
  }
}
```

## Lifecycle Hooks

| Hook | When Called | Use Case |
|------|-------------|----------|
| `ngOnInit()` | After first property check | Initialize component data |
| `ngOnChanges()` | When @Input changes | React to input changes |
| `ngDoCheck()` | Every change detection | Custom change detection |
| `ngAfterViewInit()` | After view initializes | DOM manipulation |
| `ngOnDestroy()` | Before component destroyed | Cleanup subscriptions |

### Lifecycle Example

```typescript
import { Component, OnInit, OnChanges, OnDestroy, 
         SimpleChanges, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-lifecycle',
  standalone: true,
  template: `<p>Check console for lifecycle events</p>`
})
export class LifecycleComponent implements OnInit, OnChanges, 
                                       AfterViewInit, OnDestroy {
  @Input() data: string = '';
  
  ngOnInit(): void {
    console.log('ngOnInit - Component initialized');
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    console.log('ngOnChanges - Input changed:', changes);
  }
  
  ngAfterViewInit(): void {
    console.log('ngAfterViewInit - View is ready');
  }
  
  ngOnDestroy(): void {
    console.log('ngOnDestroy - Cleaning up!');
  }
}
```

## Best Practices

### 1. Single Responsibility

```typescript
// Good: Focused component
@Component({ selector: 'app-user-avatar', ... })
export class UserAvatarComponent {
  @Input() src: string = '';
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
}

// Bad: Too many responsibilities
@Component({ selector: 'app-user-profile-form-display-edit-validate', ... })
export class UserProfileComponent {
  // Does everything - hard to maintain
}
```

### 2. Smart vs Dumb Components

```typescript
// Smart (Container) Component - Has logic, calls services
@Component({
  selector: 'app-user-list-container',
  standalone: true,
  imports: [UserCardComponent],
  template: `
    <app-user-card 
      *ngFor="let user of users$ | async"
      [user]="user"
      (edit)="editUser($event)">
    </app-user-card>
  `
})
export class UserListContainerComponent {
  private userService = inject(UserService);
  users$ = this.userService.getUsers();
  
  editUser(user: User): void { /* ... */ }
}

// Dumb (Presentational) Component - Just displays data
@Component({
  selector: 'app-user-card',
  standalone: true,
  inputs: ['user'],
  outputs: ['edit'],
  template: `
    <div class="card">
      <img [src]="user.avatar" [alt]="user.name">
      <h3>{{ user.name }}</h3>
      <button (click)="edit.emit(user)">Edit</button>
    </div>
  `
})
export class UserCardComponent {
  user!: User;
  edit = new EventEmitter<User>();
}
```

### 3. Use Standalone Components (Angular 17+)

```typescript
// Good: Standalone component
@Component({
  standalone: true,
  imports: [CommonModule],
  selector: 'app-hero',
  template: `...`
})
export class HeroComponent { }

// Avoid: NgModule (unless necessary)
@NgModule({
  declarations: [HeroComponent],
  exports: [HeroComponent]
})
export class HeroModule { }
```

## Common Pitfalls and Debugging

### Pitfall 1: Circular Dependency

```typescript
// Problem: A imports B, B imports A
// Solution: Use forwardRef
@Component({...})
export class AComponent {
  @ViewChild(BComponent) b!: BComponent;
}

@Component({...})
export class BComponent {
  @ViewChild(AComponent) a!: AComponent;
}
```

### Pitfall 2: Memory Leaks

```typescript
// Problem: Subscription not cleaned up
export class MyComponent implements OnInit {
  ngOnInit() {
    this.dataService.getData().subscribe(data => {
      this.data = data;
    }); // Memory leak!
  }
}

// Solution: Use takeUntilDestroyed or async pipe
export class MyComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  
  ngOnInit() {
    this.dataService.getData()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(data => this.data = data);
  }
}
```

### Pitfall 3: Change Detection Issues

```typescript
// Problem: Object mutated but Angular doesn't detect
export class MyComponent {
  user = { name: 'John' };
  
  updateName(): void {
    this.user.name = 'Jane'; // Angular might not detect this
  }
}

// Solution: Create new reference
export class MyComponent {
  user = { name: 'John' };
  
  updateName(): void {
    this.user = { ...this.user, name: 'Jane' }; // New object
  }
}
```

## Hands-On Exercise

### Exercise 1.5: Component Communication

**Objective**: Build a task list with parent-child component communication

**Requirements**:
1. Create a `TaskListComponent` (parent)
2. Create a `TaskItemComponent` (child)
3. Implement @Input for passing task data to child
4. Implement @Output for task completion/deletion
5. Use lifecycle hooks to log events

**Deliverable**: Working task list with component communication

**Assessment Criteria**:
- [ ] Parent passes task data to child via @Input
- [ ] Child emits events to parent via @Output
- [ ] At least one lifecycle hook implemented
- [ ] Clean component architecture
- [ ] Proper TypeScript typing

## Extension Challenge

**Challenge**: Add a task edit feature

```typescript
// Add edit mode to TaskItemComponent
@Component({...})
export class TaskItemComponent {
  @Input() task!: Task;
  @Output() taskUpdated = new EventEmitter<Task>();
  
  isEditing = false;
  editForm: FormGroup;
  
  // Implement edit functionality
}
```

## Summary

- Components are the building blocks of Angular applications
- Use @Input for parent-to-child communication
- Use @Output for child-to-parent events
- Implement lifecycle hooks for initialization and cleanup
- Follow single responsibility and smart/dumb component patterns
- Use standalone components in Angular 17+

## Next Steps

In the next lecture, we'll cover data binding techniques in detail.
