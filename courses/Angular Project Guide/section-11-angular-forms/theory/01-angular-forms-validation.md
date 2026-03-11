# Section 11: Angular Forms & Validation

## Two Approaches

### 1. Template-Driven Forms
```typescript
import { NgModule } from '@angular/forms';
// Add FormsModule to imports

// In component
import { NgForm } from '@angular/forms';

onSubmit(form: NgForm) {
  console.log(form.value);
  console.log(form.valid);
}
```

```html
<form #productForm="ngForm" (ngSubmit)="onSubmit(productForm)">
  <div class="form-group">
    <label>Name</label>
    <input type="text" name="name" [(ngModel)]="product.name" required minlength="3">
  </div>
  <div class="form-group">
    <label>Price</label>
    <input type="number" name="price" [(ngModel)]="product.price" required min="0">
  </div>
  <button type="submit" [disabled]="!productForm.valid">Submit</button>
</form>
```

### 2. Reactive Forms (Recommended)
```typescript
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

export class ProductFormComponent implements OnInit {
  productForm!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      category: ['Electronics', Validators.required],
      inStock: [true]
    });
  }

  onSubmit() {
    if (this.productForm.valid) {
      console.log(this.productForm.value);
    }
  }
}
```

### Form Validation Messages
```html
<div *ngIf="name.invalid && (name.dirty || name.touched)">
  <small *ngIf="name.errors?.required">Name is required.</small>
  <small *ngIf="name.errors?.minlength">Name must be at least 3 characters.</small>
</div>
```

---

## Summary

Template Forms = Quick and simple
Reactive Forms = Powerful and scalable
