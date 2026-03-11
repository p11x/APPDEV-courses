# Section 8: Angular Components & Templates

## Component Basics

Components are the main building block of Angular applications. Each component consists of:
- TypeScript class with logic
- HTML template for the view
- CSS styles for presentation

### Template Syntax

**Interpolation:**
```html
<h1>{{ product.name }}</h1>
<p>Price: {{ product.price | currency }}</p>
```

**Property Binding:**
```html
<img [src]="product.imageUrl" [alt]="product.name">
<button [disabled]="!product.inStock">Add to Cart</button>
```

**Event Binding:**
```html
<button (click)="addToCart(product)">Add</button>
<input (input)="onSearch($event)">
```

**Two-way Binding:**
```html
<input [(ngModel)]="searchTerm">
```

### Directives

**ngFor - Loop:**
```html
<div *ngFor="let product of products">
    {{ product.name }}
</div>
```

**ngIf - Conditional:**
```html
<div *ngIf="product.inStock">
    In Stock
</div>
<div *ngIf="!product.inStock">
    Out of Stock
</div>
```

**ngSwitch - Multiple conditions:**
```html
<div [ngSwitch]="product.category">
    <div *ngSwitchCase="'Electronics'">⚡</div>
    <div *ngSwitchCase="'Furniture'">🪑</div>
    <div *ngSwitchDefault>📦</div>
</div>
```

### Pipes

```html
<!-- Date pipe -->
{{ today | date:'shortDate' }}

<!-- Currency pipe -->
{{ price | currency:'USD' }}

<!-- Uppercase -->
{{ name | uppercase }}

<!-- JSON (for debugging) -->
{{ product | json }}
```

---

## Summary

Components + Templates = Angular UI
