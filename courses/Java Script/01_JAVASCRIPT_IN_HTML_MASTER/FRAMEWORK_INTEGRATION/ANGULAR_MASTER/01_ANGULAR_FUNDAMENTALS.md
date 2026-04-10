# 🔷 Angular JavaScript Master Guide

## Comprehensive Angular Development

---

## Table of Contents

1. [Introduction to Angular](#introduction-to-angular)
2. [Angular Setup](#angular-setup)
3. [Components](#components)
4. [Directives](#directives)
5. [Services](#services)
6. [Dependency Injection](#dependency-injection)
7. [Pipes](#pipes)
8. [Forms](#forms)
9. [HTTP](#http)
10. [Best Practices](#best-practices)

---

## Introduction to Angular

### What is Angular?

Angular is a platform and framework for building client applications in HTML, TypeScript, and CSS. Developed and maintained by Google.

```
┌─────────────────────────────────────────────────────────────┐
│              ANGULAR ECOSYSTEM                             │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐    │
│  │  Angular  │  │   Angular │  │    Angular   │    │
│  │    CLI    │  │   Router │  │     Forms     │    │
│  └─────────────┘  └─────────────┘  └─────────────────┘    │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────┐    │
│  │    RxJS   │  │   NgRx    │  │    Angular    │    │
│  │ (Reactive)│  │  (State)  │  │    Material   │    │
│  └─────────────┘  └─────────────┘  └─────────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

### Why Angular?

- **TypeScript**: Type-safe development
- **Structure**: Opinionated architecture
- **Features**: Built-in solutions
- **Enterprise**: Scales well

---

## Angular Setup

### Installation

```bash
npm install -g @angular/cli
ng new my-app
cd my-app
ng serve
```

### Project Structure

```
src/
├── app/
│   ├── app.component.ts
│   ├── app.config.ts
│   └── app.routes.ts
├── main.ts
└── index.html
```

---

## Components

### Creating Components

```bash
ng generate component components/my-component
```

### Component Definition

```typescript
// components/my-component.ts
import { Component } from '@angular/core'

@Component({
  selector: 'app-my-component',
  template: `
    <div>
      <p>My Component</p>
    </div>
  `
})
export class MyComponentComponent {}
```

### Component with State

```typescript
import { Component } from '@angular/core'

@Component({
  selector: 'app-counter',
  template: `
    <div>
      <p>Count: {{ count }}</p>
      <button (click)="increment()">Increment</button>
    </div>
  `
})
export class CounterComponent {
  count = 0
  
  increment() {
    this.count++
  }
}
```

---

## Directives

### Structural Directives

```html
<!-- *ngIf -->
<div *ngIf="show">Content</div>

<!-- *ngFor -->
<ul>
  <li *ngFor="let item of items">{{ item.name }}</li>
</ul>

<!-- *ngSwitch -->
<div [ngSwitch]="type">
  <p *ngSwitchCase="'a'">Type A</p>
  <p *ngSwitchCase="'b'">Type B</p>
  <p *ngSwitchDefault>Default</p>
</div>
```

### Attribute Directives

```typescript
import { Directive, ElementRef, Input } from '@angular/core'

@Directive({
  selector: '[appHighlight]'
})
export class HighlightDirective {
  @Input() set appHighlight(color: string) {
    this.el.nativeElement.style.backgroundColor = color
  }
  
  constructor(private el: ElementRef) {}
}
```

---

## Services

### Creating Services

```bash
ng generate service services/data
```

### Service Definition

```typescript
import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root'
})
export class DataService {
  getData() {
    return ['Item 1', 'Item 2', 'Item 3']
  }
}
```

---

## Dependency Injection

### Injecting Services

```typescript
import { Component, inject } from '@angular/core'
import { DataService } from '../services/data'

@Component({
  selector: 'app-my-component',
  template: `<div>{{ data }}</div>`
})
export class MyComponentComponent {
  private dataService = inject(DataService)
  data = this.dataService.getData()
}
```

---

## Pipes

### Built-in Pipes

```html
<p>{{ name | uppercase }}</p>
<p>{{ date | date:'shortDate' }}</p>
<p>{{ price | currency }}</p>
<p>{{ name | slice:0:3 }}</p>
```

### Custom Pipes

```bash
ng generate pipe pipes/my-pipe
```

```typescript
import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'myPipe'
})
export class MyPipePipe implements PipeTransform {
  transform(value: string): string {
    return `Prefix: ${value}`
  }
}
```

---

## Forms

### Template-driven Forms

```html
<form #form="ngForm" (ngSubmit)="onSubmit(form)">
  <input name="name" ngModel #name="ngModel">
  <button type="submit">Submit</button>
</form>
```

### Reactive Forms

```typescript
import { Component } from '@angular/core'
import { FormBuilder, ReactiveFormsModule } from '@angular/forms'

@Component({
  imports: [ReactiveFormsModule],
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <input formControlName="name">
      <button type="submit">Submit</button>
    </form>
  `
})
export class MyFormComponent {
  private fb = inject(FormBuilder)
  form = this.fb.group({
    name: ['']
  })
  
  onSubmit() {
    console.log(this.form.value)
  }
}
```

---

## HTTP

### HTTP Client

```typescript
import { HttpClient } from '@angular/common/http'
import { inject } from '@angular/core'

@Component({})
export class MyComponent {
  private http = inject(HttpClient)
  
  getData() {
    this.http.get('/api/data').subscribe(data => {
      console.log(data)
    })
  }
}
```

---

## Best Practices

### Folder Structure

```
src/
├── app/
│   ├── core/
│   │   ├── services/
│   │   └── guards/
│   ├── features/
│   │   ├── components/
│   │   └── pages/
│   ├── shared/
│   │   ├── components/
│   │   ├── directives/
│   │   └── pipes/
│   └── app.component.ts
```

### Coding Standards

- Use TypeScriptstrict mode
- Follow Angular style guide
- Use lazy loading
- Implement OnPush change detection

---

## Summary

### Key Takeaways

1. **Angular CLI**: Project scaffolding
2. **Components**: Building blocks
3. **Services**: Business logic
4. **Dependency Injection**: Loose coupling
5. **Forms**: User input
6. **HTTP**: API communication

### Next Steps

- Continue with: [02_ANGULAR_COMPONENTS_MASTER.md](02_ANGULAR_COMPONENTS_MASTER.md)
- Learn Angular Material
- Study NgRx for state

---

## Cross-References

- **Previous**: [05_VUE_TESTING_GUIDE.md](../VUE_MASTER/05_VUE_TESTING_GUIDE.md)
- **Next**: [02_ANGULAR_COMPONENTS_MASTER.md](02_ANGULAR_COMPONENTS_MASTER.md)

---

*Last updated: 2024*