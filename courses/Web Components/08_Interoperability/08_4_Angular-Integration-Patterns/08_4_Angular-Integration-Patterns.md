# Angular Integration Patterns

## OVERVIEW

Angular integration uses custom elements schema and reactive forms. This guide covers NgElement, form controls, and event binding.

## IMPLEMENTATION DETAILS

### Angular Custom Elements

```typescript
// Enable custom elements in app.module.ts
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  // ...
})
export class AppModule {}
```

### Using in Angular Templates

```typescript
// my-element.component.ts
import { Component, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-my-wrapper',
  template: `
    <my-element
      #element
      [value]="value"
      (change)="onChange($event)"
    ></my-element>
  `
})
export class MyWrapperComponent {
  @ViewChild('element') element!: ElementRef;
  
  value = '';
  
  onChange(event: CustomEvent) {
    console.log('Changed:', event.detail);
  }
}
```

## NEXT STEPS

Proceed to **08_Interoperability/08_5_Svelte-Integration-Methods**.