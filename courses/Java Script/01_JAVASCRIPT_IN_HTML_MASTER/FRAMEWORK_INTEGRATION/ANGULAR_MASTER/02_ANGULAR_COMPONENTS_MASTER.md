# 🔷 Angular Components Complete Guide

## Component Architecture in Angular

---

## Table of Contents

1. [Component Basics](#component-basics)
2. [Component Lifecycle](#component-lifecycle)
3. [Input and Output](#input-and-output)
4. [View Child](#view-child)
5. [Content Projection](#content-projection)
6. [Dynamic Components](#dynamic-components)

---

## Component Basics

### Component Decorator

```typescript
import { Component } from '@angular/core'

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [],
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})
export class ButtonComponent {
  label = 'Click'
}
```

---

## Component Lifecycle

### Lifecycle Hooks

```typescript
import { 
  Component, 
  OnInit, 
  OnDestroy,
  OnChanges,
  AfterViewInit,
  AfterViewChecked,
  AfterContentInit,
  AfterContentChecked
} from '@angular/core'

@Component({})
export class MyComponent implements 
  OnInit, 
  OnDestroy,
  OnChanges,
  AfterViewInit,
  AfterViewChecked,
  AfterContentInit,
  AfterContentChecked {
  
  ngOnInit() {
    console.log('OnInit')
  }
  
  ngOnDestroy() {
    console.log('OnDestroy')
  }
  
  ngOnChanges() {
    console.log('OnChanges')
  }
  
  ngAfterViewInit() {
    console.log('AfterViewInit')
  }
}
```

---

## Input and Output

### @Input

```typescript
import { Component, Input } from '@angular/core'

@Component({
  selector: 'app-user',
  template: `<p>{{ name }}</p>`
})
export class UserComponent {
  @Input() name = ''
  @Input() age = 0
}
```

### @Output

```typescript
import { Component, Output, EventEmitter } from '@angular/core'

@Component({
  selector: 'app-counter',
  template: `
    <button (click)="increment()">{{ count }}</button>
  `
})
export class CounterComponent {
  @Output() countChange = new EventEmitter<number>()
  count = 0
  
  increment() {
    this.count++
    this.countChange.emit(this.count)
  }
}
```

---

## View Child

### Accessing Elements

```typescript
import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core'

@Component({
  selector: 'app-input',
  template: `<input #myInput>`
})
export class InputComponent implements AfterViewInit {
  @ViewChild('myInput') input!: ElementRef
  
  ngAfterViewInit() {
    this.input.nativeElement.focus()
  }
}
```

---

## Content Projection

### Single Slot

```typescript
@Component({
  selector: 'app-card',
  template: `
    <div class="card">
      <ng-content></ng-content>
    </div>
  `
})
export class CardComponent {}
```

### Multi Slot

```typescript
@Component({
  selector: 'app-panel',
  template: `
    <div class="header">
      <ng-content select="header"></ng-content>
    </div>
    <div class="body">
      <ng-content></ng-content>
    </div>
  `
})
export class PanelComponent {}
```

---

## Dynamic Components

### Loading Components

```typescript
import { Component, ViewContainerRef, ComponentRef } from '@angular/core'

@Component({
  selector: 'app-dynamic',
  template: `<ng-container></ng-container>`
})
export class DynamicComponent {
  private vcr = inject(ViewContainerRef)
  
  async loadComponent(component: Type<any>) {
    this.vcr.clear()
    const ref = this.vcr.createComponent(component)
    return ref
  }
}
```

---

## Summary

### Key Takeaways

1. **Decorators**: Component configuration
2. **Lifecycle**: Hooks for initialization/death
3. **Input/Output**: Parent-child communication
4. **ViewChild**: DOM access
5. **Projection**: Content distribution

### Next Steps

- Continue with: [03_ANGULAR_ROUTING_MASTER.md](03_ANGULAR_ROUTING_MASTER.md)
- Practice component patterns
- Implement complex components

---

## Cross-References

- **Previous**: [01_ANGULAR_FUNDAMENTALS.md](01_ANGULAR_FUNDAMENTALS.md)
- **Next**: [03_ANGULAR_ROUTING_MASTER.md](03_ANGULAR_ROUTING_MASTER.md)

---

*Last updated: 2024*