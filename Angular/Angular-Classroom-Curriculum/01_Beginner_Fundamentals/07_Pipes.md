# Angular Pipes

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand pipes and their role in Angular
- [ ] Use built-in pipes effectively
- [ ] Create custom pipes with parameters
- [ ] Implement pure and impure pipes
- [ ] Apply pipes for data formatting

## Conceptual Explanation

**Visual Analogy**: Think of pipes as **water treatment plants** in a city. Raw water (raw data) enters, gets processed through various filters and treatments (pipe transformations), and clean water (formatted data) comes out. Pipes take raw data, transform it, and present it beautifully!

### What are Pipes?

A pipe is a way to transform data in your templates. Pipes take in data and output transformed data.

```
┌─────────────────────────────────────────────────────────────────────┐
│                         Pipe Transformation                          │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   Raw Data        Pipe           Transformed Data                   │
│                                                                     │
│   "hello"   ────▶  uppercase  ────▶  "HELLO"                       │
│                                                                     │
│   1234.56   ────▶  currency    ────▶  "$1,234.56"                   │
│                                                                     │
│   Date      ────▶  date        ────▶  "Jan 15, 2024"               │
│                                                                     │
│   Array     ────▶  json        ────▶  "[1,2,3]"                    │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why Pipes Matter

1. **Template Simplicity**: Keep templates clean from transformation logic
2. **Reusability**: Use same pipe across the application
3. **Separation of Concerns**: Data transformation separate from components
4. **Performance**: Angular caches pipe results

### Industry Use Cases

- **Internationalization**: Date, currency, and text formatting
- **Data Display**: JSON visualization, number formatting
- **Text Processing**: Uppercase, lowercase, title case
- **Filtering**: Custom pipes for array filtering

## Step-by-Step Walkthrough

### Built-in Pipes

#### 1. String Pipes

```typescript
@Component({
  selector: 'app-string-pipes',
  standalone: true,
  template: `
    <h2>String Pipes</h2>
    
    <p>Original: {{ text }}</p>
    <p>Uppercase: {{ text | uppercase }}</p>
    <p>Lowercase: {{ text | lowercase }}</p>
    <p>Titlecase: {{ text | titlecase }}</p>
    <p>Slice (0-10): {{ text | slice:0:10 }}</p>
  `
})
export class StringPipesComponent {
  text = 'hello world from angular';
}
```

#### 2. Number Pipes

```typescript
@Component({
  selector: 'app-number-pipes',
  standalone: true,
  template: `
    <h2>Number Pipes</h2>
    
    <p>Original: {{ value }}</p>
    <p>Decimal (3.2-2): {{ value | number:'3.2-2' }}</p>
    <p>Currency: {{ value | currency }}</p>
    <p>Currency (EUR): {{ value | currency:'EUR' }}</p>
    <p>Currency (USD, symbol): {{ value | currency:'USD':'symbol' }}</p>
    <p>Percent: {{ decimal | percent:'1.0-0' }}</p>
  `
})
export class NumberPipesComponent {
  value = 1234.5678;
  decimal = 0.567;
}
```

#### 3. Date Pipes

```typescript
@Component({
  selector: 'app-date-pipes',
  standalone: true,
  template: `
    <h2>Date Pipes</h2>
    
    <p>Original: {{ date }}</p>
    <p>Short: {{ date | date:'short' }}</p>
    <p>Medium: {{ date | date:'medium' }}</p>
    <p>Long: {{ date | date:'long' }}</p>
    <p>Full: {{ date | date:'full' }}</p>
    <p>Custom: {{ date | date:'MMM dd, yyyy HH:mm' }}</p>
    <p>Time only: {{ date | date:'HH:mm' }}</p>
    <p>Relative: {{ date | date:'relative' }}</p>
  `
})
export: DatePipesComponent {
  date = new Date('2024-01-15T10:30:00');
}
```

#### 4. Object Pipes

```typescript
@Component({
  selector: 'app-object-pipes',
  standalone: true,
  template: `
    <h2>Object Pipes</h2>
    
    <h3>JSON Pipe</h3>
    <pre>{{ user | json }}</pre>
    
    <h3>KeyValue Pipe</h3>
    <ul>
      @for (item of user | keyvalue; track item.key) {
        <li>{{ item.key }}: {{ item.value }}</li>
      }
    </ul>
  `
})
export class ObjectPipesComponent {
  user = {
    name: 'John Doe',
    email: 'john@example.com',
    age: 30,
    role: 'admin'
  };
}
```

### Pipe Parameters

Pipes can accept parameters for customization:

```typescript
@Component({
  selector: 'app-parameterized-pipes',
  standalone: true,
  template: `
    <p>Currency with symbol: {{ amount | currency:'USD':'symbol' }}</p>
    <p>Date with format: {{ date | date:'MMM/dd/yyyy' }}</p>
    <p>Slice string: {{ text | slice:0:5 }}</p>
    <p>Multiple pipes: {{ text | uppercase | slice:0:5 }}</p>
  `
})
export class ParameterizedPipesComponent {
  amount = 1234.56;
  date = new Date();
  text = 'Hello World';
}
```

### Chaining Pipes

Multiple pipes can be chained:

```typescript
@Component({
  selector: 'app-chained-pipes',
  standalone: true,
  template: `
    <!-- Chained pipes: left to right -->
    {{ message | uppercase | slice:0:5 }}
    <!-- Output: "HELLO" -->
    
    {{ today | date:'fullDate' | uppercase }}
    <!-- Output: "MONDAY, JANUARY 15, 2024" -->
    
    {{ users | json | slice:0:50 }}
    <!-- Output truncated JSON -->
  `
})
export class ChainedPipesComponent {
  message = 'hello angular';
  today = new Date();
  users = [{ name: 'John' }, { name: 'Jane' }];
}
```

## Creating Custom Pipes

### Simple Custom Pipe

```typescript
// greeting.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'greeting',
  standalone: true
})
export class GreetingPipe implements PipeTransform {
  transform(value: string, greeting: string = 'Hello'): string {
    return `${greeting}, ${value}!`;
  }
}

// Usage: {{ 'World' | greeting }}
// Output: "Hello, World!"

// Usage: {{ 'World' | greeting:'Welcome' }}
// Output: "Welcome, World!"
```

### Custom Pipe with Parameters

```typescript
// truncate.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate',
  standalone: true
})
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number = 10, trail: string = '...'): string {
    if (!value) return '';
    if (value.length <= limit) return value;
    return value.substring(0, limit) + trail;
  }
}

// Usage: {{ longText | truncate:20 }}
// If longText = "This is a very long text", output: "This is a very ..."
```

### Pipe with Complex Logic

```typescript
// filter.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter',
  standalone: true
})
export class FilterPipe implements PipeTransform {
  transform<T>(items: T[], searchTerm: string, ...keys: string[]): T[] {
    if (!items || !searchTerm) return items;
    
    const lowerSearch = searchTerm.toLowerCase();
    
    return items.filter(item => {
      return keys.some(key => {
        const value = this.getNestedValue(item, key);
        return value && value.toString().toLowerCase().includes(lowerSearch);
      });
    });
  }
  
  private getNestedValue(obj: any, path: string): any {
    return path.split('.').reduce((o, p) => o?.[p], obj);
  }
}

// Usage in template:
// @for (user of users | filter:'john':'name':'email'; track user.id) {
//   {{ user.name }}
// }
```

### Impure Pipes

By default, pipes are **pure**. For arrays, Angular doesn't re-run the pipe when the array reference changes (but its contents change). Use `pure: false` for impure pipes:

```typescript
// impure-filter.pipe.ts
@Pipe({
  name: 'impureFilter',
  standalone: true,
  pure: false  // Runs on every change detection
})
export class ImpureFilterPipe implements PipeTransform {
  transform<T>(items: T[], filter: string): T[] {
    if (!items) return [];
    if (!filter) return items;
    return items.filter(item => item.toString().includes(filter));
  }
}

// Warning: Impure pipes run on every change detection cycle
// Use sparingly for performance reasons
```

## Complete Example: Product Table with Pipes

```typescript
// product.model.ts
export interface Product {
  id: number;
  name: string;
  price: number;
  category: string;
  releaseDate: Date;
  inStock: boolean;
}

// custom-pipes.ts
@Pipe({ name: 'stockStatus', standalone: true })
export class StockStatusPipe implements PipeTransform {
  transform(inStock: boolean): string {
    return inStock ? '✓ In Stock' : '✗ Out of Stock';
  }
}

@Pipe({ name: 'discount', standalone: true })
export class DiscountPipe implements PipeTransform {
  transform(price: number, discountPercent: number = 0): string {
    if (discountPercent <= 0) return '';
    const discounted = price * (1 - discountPercent / 100);
    return `(-${discountPercent}%) $${discounted.toFixed(2)}`;
  }
}

// product-table.component.ts
import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from './product.model';
import { StockStatusPipe, DiscountPipe } from './custom-pipes';

@Component({
  selector: 'app-product-table',
  standalone: true,
  imports: [CommonModule, StockStatusPipe, DiscountPipe],
  template: `
    <div class="product-table">
      <h1>Product Catalog</h1>
      
      <!-- Search -->
      <div class="search-bar">
        <input 
          [(ngModel)]="searchTerm" 
          placeholder="Search products..."
          (input)="filterProducts()">
      </div>
      
      <!-- Category Filter with @switch -->
      <div class="category-filter">
        @switch (selectedCategory) {
          @case ('electronics') { <p>Electronics</p> }
          @case ('clothing') { <p>Clothing</p> }
          @case ('books') { <p>Books</p> }
          @default { <p>All Categories</p> }
        }
        <button (click)="filterByCategory('')">All</button>
        <button (click)="filterByCategory('electronics')">Electronics</button>
        <button (click)="filterByCategory('clothing')">Clothing</button>
        <button (click)="filterByCategory('books')">Books</button>
      </div>
      
      <!-- Product Table -->
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Product</th>
            <th>Category</th>
            <th>Price</th>
            <th>Discounted</th>
            <th>Date</th>
            <th>Stock</th>
          </tr>
        </thead>
        <tbody>
          @for (product of filteredProducts(); track product.id) {
            <tr [class.out-of-stock]="!product.inStock">
              <td>{{ product.id }}</td>
              <td>{{ product.name | titlecase }}</td>
              <td>{{ product.category | titlecase }}</td>
              <td>{{ product.price | currency:'USD':'symbol':'1.2-2' }}</td>
              <td>
                {{ product.price | currency:'USD':'symbol':'1.2-2' }}
                @if (product.category === 'electronics') {
                  <span class="discount">-10%</span>
                }
              </td>
              <td>{{ product.releaseDate | date:'MMM dd, yyyy' }}</td>
              <td [class.in-stock]="product.inStock">
                {{ product.inStock | stockStatus }}
              </td>
            </tr>
          } @empty {
            <tr>
              <td colspan="7">No products found</td>
            </tr>
          }
        </tbody>
      </table>
      
      <!-- Summary -->
      <div class="summary">
        <p>Total Products: {{ filteredProducts().length }}</p>
        <p>Total Value: {{ totalValue() | currency:'USD':'symbol':'1.2-2' }}</p>
        <p>Last Updated: {{ lastUpdated | date:'mediumTime' }}</p>
      </div>
    </div>
  `,
  styles: [`
    .product-table {
      padding: 1rem;
    }
    table {
      width: 100%;
      border-collapse: collapse;
    }
    th, td {
      padding: 0.75rem;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }
    .discount {
      background: #f44336;
      color: white;
      padding: 0.125rem 0.25rem;
      border-radius: 3px;
      font-size: 0.75rem;
    }
    .in-stock { color: #4caf50; }
    .out-of-stock { opacity: 0.6; }
  `]
})
export class ProductTableComponent {
  products = signal<Product[]>([
    { id: 1, name: 'Laptop Pro', price: 1299.99, category: 'electronics', releaseDate: new Date('2024-01-10'), inStock: true },
    { id: 2, name: 'Running Shoes', price: 89.99, category: 'clothing', releaseDate: new Date('2024-01-05'), inStock: true },
    { id: 3, name: 'TypeScript Guide', price: 49.99, category: 'books', releaseDate: new Date('2024-01-01'), inStock: false },
    { id: 4, name: 'Wireless Mouse', price: 29.99, category: 'electronics', releaseDate: new Date('2024-01-12'), inStock: true },
    { id: 5, name: 'Winter Jacket', price: 199.99, category: 'clothing', releaseDate: new Date('2023-12-15'), inStock: true }
  ]);
  
  filteredProducts = signal<Product[]>([]);
  searchTerm = '';
  selectedCategory = '';
  lastUpdated = new Date();
  
  constructor() {
    this.filterProducts();
  }
  
  filterProducts(): void {
    let result = this.products();
    
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      result = result.filter(p => 
        p.name.toLowerCase().includes(term) ||
        p.category.toLowerCase().includes(term)
      );
    }
    
    if (this.selectedCategory) {
      result = result.filter(p => p.category === this.selectedCategory);
    }
    
    this.filteredProducts.set(result);
  }
  
  filterByCategory(category: string): void {
    this.selectedCategory = category;
    this.filterProducts();
  }
  
  totalValue = () => this.filteredProducts().reduce((sum, p) => sum + p.price, 0);
}
```

## Best Practices

### 1. Use Built-in Pipes When Possible

```typescript
// Good: Built-in pipe
{{ name | titlecase }}

// Avoid: Custom pipe for simple transformations
{{ name | customTitlecase }}
```

### 2. Keep Pipes Pure When Possible

```typescript
// Good: Pure pipe (default)
@Pipe({ name: 'myPipe' })
export class MyPipe implements PipeTransform {
  transform(value: string): string { ... }
}

// Only use impure when necessary
@Pipe({ name: 'myImpurePipe', pure: false })
export class MyImpurePipe implements PipeTransform { ... }
```

### 3. Don't Over-Complicate Pipes

```typescript
// Good: Simple transformation
@Pipe({ name: 'abbreviate' })
export class AbbreviatePipe implements PipeTransform {
  transform(value: string): string {
    return value.split(' ').map(w => w[0]).join('');
  }
}

// Better: Move complex logic to service
// Use pipe only for presentation
```

## Common Pitfalls and Debugging

### Pitfall 1: Pipe Not Found

```typescript
// Error: The 'myPipe' pipe could not be found
// Solution: Import the pipe in the component
@Component({
  standalone: true,
  imports: [MyCustomPipe],  // Import here
  template: `{{ value | myCustomPipe }}`
})
export class MyComponent {}
```

### Pitfall 2: Null Values

```typescript
// Problem: Error with null value
{{ user.name | uppercase }}  // Error if user is null

// Solution: Use optional chaining
{{ user?.name | uppercase }}
// or ngIf
@if (user) { {{ user.name | uppercase }} }
```

### Pitfall 3: Pure Pipe Not Updating

```typescript
// Problem: Pure pipe doesn't detect array changes
@Component({...})
export class MyComponent {
  items = [1, 2, 3];
  
  addItem() {
    this.items.push(4);  // Won't trigger pipe!
  }
}

// Solution: Create new array reference
addItem() {
  this.items = [...this.items, 4];  // Triggers pipe
}
```

## Hands-On Exercise

### Exercise 1.8: Pipe Implementation

**Objective**: Create a data dashboard with custom pipes

**Requirements**:
1. Use at least 5 built-in pipes
2. Create 2 custom pipes
3. Chain multiple pipes in a single expression
4. Handle null values gracefully

**Deliverable**: Dashboard with formatted data

**Assessment Criteria**:
- [ ] 5+ built-in pipes used correctly
- [ ] Custom pipe with parameter
- [ ] Custom pipe for array filtering
- [ ] Null safety implemented
- [ ] Clean template organization

## Summary

- **Pipes** transform data in templates
- **Built-in pipes**: uppercase, lowercase, date, currency, number, json, slice, keyvalue
- **Custom pipes**: Create with @Pipe decorator
- **Parameters**: Pass with colon syntax
- **Impure pipes**: Set `pure: false` for array mutations
- **Best practice**: Keep pipes simple and pure

## Suggested Reading

- [Angular Pipes Documentation](https://angular.io/guide/pipes)
- "Angular: Up and Running" - Pipes Chapter

## Next Steps

This concludes the Beginner Level. In the next section, we'll cover Intermediate Level topics including Services, Dependency Injection, and Routing.
