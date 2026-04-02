---
title: "Angular NG-Bootstrap Setup"
topic: "Framework Integration"
difficulty: 2
duration: "30 minutes"
prerequisites: ["Angular fundamentals", "TypeScript", "Angular CLI"]
tags: ["angular", "ng-bootstrap", "bootstrap", "directives", "components"]
---

## Overview

NG-Bootstrap provides native Angular widgets for Bootstrap 5, built from scratch using Angular directives and components without any dependency on jQuery or Bootstrap's JavaScript. Each Bootstrap component (datepicker, modal, accordion, carousel, etc.) is implemented as an Angular component with full TypeScript support, dependency injection integration, and ARIA-compliant accessibility.

The library requires only Bootstrap's CSS (no JS bundle) and uses Angular's `@angular/animations` for transitions. Components use Angular's `@Input()`/`@Output()` pattern, `ng-template` for content projection, and `NgbModal` service for programmatic modal creation.

## Basic Installation

```bash
ng new my-ng-bootstrap-app
cd my-ng-bootstrap-app
ng add @ng-bootstrap/ng-bootstrap
npm install bootstrap @popperjs/core
```

Add Bootstrap CSS to `angular.json`:

```json
{
  "projects": {
    "my-ng-bootstrap-app": {
      "architect": {
        "build": {
          "options": {
            "styles": [
              "node_modules/bootstrap/dist/css/bootstrap.min.css",
              "src/styles.scss"
            ]
          }
        }
      }
    }
  }
}
```

Or use SCSS customization:

```scss
// src/styles.scss
$primary: #8b5cf6;
@import 'bootstrap/scss/bootstrap';
```

Basic component usage:

```typescript
// src/app/app.component.ts
import { Component } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from './confirm-modal.component';

@Component({
  selector: 'app-root',
  template: `
    <div class="container py-5">
      <div class="row">
        <div class="col-md-8 mx-auto">
          <div class="card">
            <div class="card-header">
              <h5 class="mb-0">NG-Bootstrap</h5>
            </div>
            <div class="card-body">
              <ngb-accordion [closeOthers]="true">
                <ngb-panel id="panel-1" title="First Panel">
                  <ng-template ngbPanelContent>
                    Content of the first panel.
                  </ng-template>
                </ngb-panel>
                <ngb-panel id="panel-2" title="Second Panel">
                  <ng-template ngbPanelContent>
                    Content of the second panel.
                  </ng-template>
                </ngb-panel>
              </ngb-accordion>

              <button class="btn btn-primary mt-3" (click)="openModal()">
                Open Modal
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class AppComponent {
  constructor(private modalService: NgbModal) {}

  async openModal() {
    const { ConfirmModalComponent } = await import('./confirm-modal.component');
    const modalRef = this.modalService.open(ConfirmModalComponent);
    modalRef.componentInstance.title = 'Confirm Action';
    const result = await modalRef.result.catch(() => 'dismiss');
    console.log('Modal result:', result);
  }
}
```

## Advanced Variations

### Custom Modal Component

```typescript
// src/app/confirm-modal.component.ts
import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirm-modal',
  template: `
    <div class="modal-header">
      <h4 class="modal-title">{{ title }}</h4>
      <button type="button" class="btn-close" (click)="modal.dismiss()"></button>
    </div>
    <div class="modal-body">
      <ng-content></ng-content>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="modal.dismiss()">
        Cancel
      </button>
      <button type="button" class="btn btn-danger" (click)="modal.close('confirmed')">
        Confirm
      </button>
    </div>
  `,
})
export class ConfirmModalComponent {
  @Input() title = 'Confirm';
  constructor(public modal: NgbActiveModal) {}
}
```

### Datepicker with Form Integration

```typescript
// src/app/booking-form.component.ts
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-booking-form',
  template: `
    <form [formGroup]="form" (ngSubmit)="submit()">
      <div class="mb-3">
        <label class="form-label">Check-in Date</label>
        <div class="input-group">
          <input class="form-control" placeholder="yyyy-mm-dd"
                 ngbDatepicker #checkin="ngbDatepicker"
                 formControlName="checkinDate"
                 (dateSelect)="validateDates()">
          <button class="btn btn-outline-secondary" (click)="checkin.toggle()" type="button">
            <i class="bi bi-calendar"></i>
          </button>
        </div>
        @if (form.get('checkinDate')?.invalid && form.get('checkinDate')?.touched) {
          <div class="text-danger small mt-1">Check-in date is required</div>
        }
      </div>

      <ngb-timepicker formControlName="checkinTime" [meridian]="true" />

      <button type="submit" class="btn btn-primary" [disabled]="form.invalid">
        Book
      </button>
    </form>
  `,
})
export class BookingFormComponent {
  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      checkinDate: [null, Validators.required],
      checkinTime: [{ hour: 14, minute: 0 }, Validators.required],
    });
  }

  validateDates() {
    // Custom date validation logic
  }

  submit() {
    if (this.form.valid) {
      console.log('Booking:', this.form.value);
    }
  }
}
```

### Module-based Configuration

```typescript
// src/app/app.module.ts
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    NgbModule,
    // Or import individual modules:
    // NgbAccordionModule,
    // NgbModalModule,
    // NgbDatepickerModule,
  ],
})
export class AppModule {}
```

## Best Practices

1. **Use `ng add @ng-bootstrap/ng-bootstrap`** for automatic project configuration and dependency installation.
2. **Do not include Bootstrap's JavaScript** (`bootstrap.bundle.min.js`) — NG-Bootstrap replaces it entirely.
3. **Use `NgbModal` service** for programmatic modal creation rather than template-driven approaches for complex modals.
4. **Import only needed modules** (`NgbAccordionModule`, `NgbModalModule`) for optimal tree-shaking in production.
5. **Use `ng-template`** for content projection into NG-Bootstrap components (accordion panels, tab content, etc.).
6. **Leverage `NgbDateAdapter`** to integrate datepickers with your preferred date library (Luxon, date-fns).
7. **Use `@angular/forms` integration** — NG-Bootstrap components implement `ControlValueAccessor` for reactive form binding.
8. **Configure `NgbDatepickerConfig`** globally in a service for consistent datepicker defaults across the application.
9. **Use lazy loading** with dynamic `import()` for modal components to reduce initial bundle size.
10. **Set `NgbTooltip` `placement` and `triggers`** inputs for consistent tooltip behavior application-wide.

## Common Pitfalls

1. **Including `bootstrap.js` or `bootstrap.bundle.js`** alongside NG-Bootstrap causes duplicate event handlers and conflicts.
2. **Missing `@popperjs/core`** breaks tooltip/popover positioning in NG-Bootstrap's dropdown and popover components.
3. **Not adding Bootstrap CSS to `angular.json` styles** results in unstyled components.
4. **Using `ngbDatepicker` without `NgbDateAdapter`** for custom date formats causes parsing errors.
5. **Forgetting `[closeOthers]="true"`** on accordions allows multiple panels open simultaneously, which may be unintended.

## Accessibility Considerations

NG-Bootstrap implements WAI-ARIA patterns for all interactive components. Accordions use `role="region"` with `aria-labelledby`. Modals manage focus trapping, `aria-modal`, and `aria-describedby`. Datepickers provide keyboard navigation with arrow keys and screen reader announcements. Use `NgbTypeahead` with `aria-label` for accessible autocomplete. Components respect `prefers-reduced-motion` through Angular's animation system.

## Responsive Behavior

NG-Bootstrap's grid components rely on Bootstrap's CSS classes for responsive layouts. Use standard Bootstrap classes (`col-md-6`, `d-lg-flex`) in Angular templates. The `NgbOffcanvas` component accepts a `position` prop (`start`, `end`, `bottom`) and works responsively with Bootstrap's breakpoint utilities. Datepicker and timepicker components adapt their layout to container width. The `NgbDropdownMenu` repositions automatically based on viewport constraints via Popper.js.