# State Management with NgRx

## Learning Objectives

By the end of this lesson, students will be able to:

- [ ] Understand state management concepts and patterns
- [ ] Implement NgRx store for application state
- [ ] Create actions, reducers, effects, and selectors
- [ ] Use NgRx Entity for collections
- [ ] Apply NgRx Signal Store for modern state management

## Conceptual Explanation

**Visual Analogy**: Think of state management as a **centralized warehouse** for your application's data. In a small store, you can keep inventory in each room (local component state). But in a large department store, you need a central warehouse where all departments (components) get their inventory from and report sales to. NgRx is that warehouse management system - it tracks what's in stock (state), processes orders (actions), updates inventory (reducers), and coordinates deliveries (effects)!

### NgRx Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                    NgRx Architecture Flow                            │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│   Component ──▶ Action ──▶ Reducer ──▶ State ──▶ Selector ──▶ Component│
│                           │                                          │
│                           ▼                                          │
│                          Effects                                    │
│                           │                                          │
│                           ▼                                          │
│                        Services (API)                               │
│                                                                     │
│   1. Component dispatches Action                                    │
│   2. Reducer updates State based on Action                         │
│   3. Effects handle side effects (API calls)                       │
│   4. Selector updates Component with new State                     │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Real-World Application Context

### Why State Management Matters

1. **Predictable State**: Single source of truth
2. **Debugging**: Time-travel debugging with Redux DevTools
3. **Scalability**: Manage complex state across many components
4. **Team Development**: Clear patterns for collaboration

### Industry Use Cases

- **E-commerce**: Cart management, product catalogs
- **Dashboards**: Real-time data, filters
- **CRUD Apps**: Complex form state, list management

## Step-by-Step Walkthrough

### Setting Up NgRx

#### Step 1: Install Dependencies

```bash
npm install @ngrx/store @ngrx/effects @ngrx/entity @ngrx/store-devtools
```

#### Step 2: Configure Store

```typescript
// app.config.ts
import { ApplicationConfig } from '@angular/core';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideRouter } from '@angular/router';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideStore(),  // Enable NgRx store
    provideEffects(),
    provideStoreDevtools({
      maxAge: 25,
      logOnly: false
    })
  ]
};
```

### Creating State Structure

#### Step 1: Define Models

```typescript
// models/product.model.ts
export interface Product {
  id: string;
  name: string;
  price: number;
  category: string;
  description: string;
  imageUrl: string;
}
```

#### Step 2: Create Actions

```typescript
// store/product.actions.ts
import { createAction, props } from '@ngrx/store';
import { Product } from '../../models/product.model';

// Load products
export const loadProducts = createAction('[Product] Load Products');
export const loadProductsSuccess = createAction(
  '[Product] Load Products Success',
  props<{ products: Product[] }>()
);
export const loadProductsFailure = createAction(
  '[Product] Load Products Failure',
  props<{ error: string }>()
);

// Add product
export const addProduct = createAction(
  '[Product] Add Product',
  props<{ product: Product }>()
);

// Update product
export const updateProduct = createAction(
  '[Product] Update Product',
  props<{ product: Product }>()
);

// Delete product
export const deleteProduct = createAction(
  '[Product] Delete Product',
  props<{ id: string }>()
);

// Select product
export const selectProduct = createAction(
  '[Product] Select Product',
  props<{ id: string | null }>()
);
```

#### Step 3: Create Reducer

```typescript
// store/product.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Product } from '../../models/product.model';
import * as ProductActions from './product.actions';

// Entity adapter for better performance
export interface ProductState extends EntityState<Product> {
  selectedProductId: string | null;
  loading: boolean;
  error: string | null;
}

// Configure adapter
export const adapter: EntityAdapter<Product> = createEntityAdapter<Product>();

// Initial state
export const initialState: ProductState = adapter.getInitialState({
  selectedProductId: null,
  loading: false,
  error: null
});

// Create reducer
export const productReducer = createReducer(
  initialState,
  
  // Load products
  on(ProductActions.loadProducts, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  
  on(ProductActions.loadProductsSuccess, (state, { products }) =>
    adapter.setAll(products, { ...state, loading: false })
  ),
  
  on(ProductActions.loadProductsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  
  // Add product
  on(ProductActions.addProduct, (state, { product }) =>
    adapter.addOne(product, state)
  ),
  
  // Update product
  on(ProductActions.updateProduct, (state, { product }) =>
    adapter.updateOne({ id: product.id, changes: product }, state)
  ),
  
  // Delete product
  on(ProductActions.deleteProduct, (state, { id }) =>
    adapter.removeOne(id, state)
  ),
  
  // Select product
  on(ProductActions.selectProduct, (state, { id }) => ({
    ...state,
    selectedProductId: id
  }))
);

// Export selectors
export const { selectIds, selectEntities, selectAll, selectTotal } =
  adapter.getSelectors();
```

#### Step 4: Create Selectors

```typescript
// store/product.selectors.ts
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProductState, selectAll, selectEntities } from './product.reducer';

// Feature selector
export const selectProductState = createFeatureSelector<ProductState>('products');

// Select all products
export const selectAllProducts = createSelector(
  selectProductState,
  selectAll
);

// Select product entities
export const selectProductEntities = createSelector(
  selectProductState,
  selectEntities
);

// Select loading state
export const selectProductLoading = createSelector(
  selectProductState,
  (state) => state.loading
);

// Select error
export const selectProductError = createSelector(
  selectProductState,
  (state) => state.error
);

// Select selected product
export const selectSelectedProduct = createSelector(
  selectProductState,
  selectProductEntities,
  (state, entities) => state.selectedProductId 
    ? entities[state.selectedProductId] 
    : null
);

// Select products by category
export const selectProductsByCategory = (category: string) =>
  createSelector(
    selectAllProducts,
    (products) => products.filter((p) => p.category === category)
  );

// Select product count
export const selectProductTotal = createSelector(
  selectProductState,
  (state) => state.ids.length
);
```

#### Step 5: Create Effects

```typescript
// store/product.effects.ts
import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { map, mergeMap, catchError } from 'rxjs/operators';
import { ProductService } from '../../services/product.service';
import * as ProductActions from './product.actions';

@Injectable()
export class ProductEffects {
  private actions$ = inject(Actions);
  private productService = inject(ProductService);
  
  // Load products effect
  loadProducts$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.loadProducts),
      mergeMap(() =>
        this.productService.getProducts().pipe(
          map((products) => ProductActions.loadProductsSuccess({ products })),
          catchError((error) =>
            of(ProductActions.loadProductsFailure({ error: error.message }))
          )
        )
      )
    )
  );
  
  // Add product effect
  addProduct$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.addProduct),
      mergeMap(({ product }) =>
        this.productService.createProduct(product).pipe(
          map((newProduct) => ProductActions.addProduct({ product: newProduct })),
          catchError((error) =>
            of(ProductActions.loadProductsFailure({ error: error.message }))
          )
        )
      )
    )
  );
  
  // Update product effect
  updateProduct$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.updateProduct),
      mergeMap(({ product }) =>
        this.productService.updateProduct(product).pipe(
          map(() => ProductActions.updateProduct({ product })),
          catchError((error) =>
            of(ProductActions.loadProductsFailure({ error: error.message }))
          )
        )
      )
    )
  );
  
  // Delete product effect
  deleteProduct$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.deleteProduct),
      mergeMap(({ id }) =>
        this.productService.deleteProduct(id).pipe(
          map(() => ProductActions.deleteProduct({ id })),
          catchError((error) =>
            of(ProductActions.loadProductsFailure({ error: error.message }))
          )
        )
      )
    )
  );
}
```

#### Step 6: Register in App Config

```typescript
// app.config.ts
import { ApplicationConfig, isDevMode } from '@angular/core';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideRouter } from '@angular/router';
import { productReducer } from './store/product.reducer';
import { ProductEffects } from './store/product.effects';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideStore({ products: productReducer }),
    provideEffects([ProductEffects]),
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !isDevMode(),
      autoPause: true
    })
  ]
};
```

### Using Store in Components

```typescript
// product-list.component.ts
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngrx/store';
import { Product } from '../../models/product.model';
import * as ProductActions from '../../store/product.actions';
import * as ProductSelectors from '../../store/product.selectors';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="product-list">
      <h1>Products</h1>
      
      <button (click)="loadProducts()">Refresh</button>
      
      @if (loading$ | async) {
        <p>Loading...</p>
      }
      
      @if (error$ | async; as error) {
        <p class="error">{{ error }}</p>
      }
      
      <div class="products">
        @for (product of products$ | async; track product.id) {
          <div class="product-card" (click)="selectProduct(product.id)">
            <img [src]="product.imageUrl" [alt]="product.name">
            <h3>{{ product.name }}</h3>
            <p>{{ product.price | currency }}</p>
            <p>{{ product.category }}</p>
            <button (click)="deleteProduct($event, product.id)">Delete</button>
          </div>
        }
      </div>
    </div>
  `
})
export class ProductListComponent implements OnInit {
  private store = inject(Store);
  
  // Selectors
  products$ = this.store.select(ProductSelectors.selectAllProducts);
  loading$ = this.store.select(ProductSelectors.selectProductLoading);
  error$ = this.store.select(ProductSelectors.selectProductError);
  
  ngOnInit(): void {
    this.loadProducts();
  }
  
  loadProducts(): void {
    this.store.dispatch(ProductActions.loadProducts());
  }
  
  selectProduct(id: string): void {
    this.store.dispatch(ProductActions.selectProduct({ id }));
  }
  
  deleteProduct(event: Event, id: string): void {
    event.stopPropagation();
    if (confirm('Delete this product?')) {
      this.store.dispatch(ProductActions.deleteProduct({ id }));
    }
  }
}
```

## NgRx Signal Store (Modern Approach)

Angular 16+ introduces a simpler alternative:

```typescript
// todo.store.ts
import { patchState, signalStore, withState, withMethods, withComputed } from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import { TodoService, Todo } from '../services/todo.service';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { pipe, tap, switchMap, mergeMap } from 'rxjs';

type TodoState = {
  todos: Todo[];
  filter: 'all' | 'active' | 'completed';
  loading: boolean;
};

const initialState: TodoState = {
  todos: [],
  filter: 'all',
  loading: false
};

export const TodoStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withMethods((store, todoService = inject(TodoService)) => ({
    // Computed signals
    $filteredTodos: computed(() => {
      const { todos, filter } = store;
      if (filter === 'active') return todos.filter(t => !t.completed);
      if (filter === 'completed') return todos.filter(t => t.completed);
      return todos;
    }),
    $activeCount: computed(() => store.todos.filter(t => !t.completed).length),
    $completedCount: computed(() => store.todos.filter(t => t.completed).length),
    
    // Methods
    setFilter(filter: TodoState['filter']) {
      patchState(store, { filter });
    },
    
    // Rx methods
    loadTodos: rxMethod<void>(pipe(
      tap(() => patchState(store, { loading: true })),
      switchMap(() => todoService.getTodos()),
      tap(todos => patchState(store, { todos, loading: false }))
    )),
    
    addTodo: rxMethod<Omit<Todo, 'id'>>(pipe(
      mergeMap(todo => todoService.createTodo(todo)),
      tap(todo => patchState(store, { todos: [...store.todos, todo] }))
    ))
  }))
);

// Using in component
@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button (click)="todoStore.loadTodos()">Load</button>
    @for (todo of todoStore.$filteredTodos(); track todo.id) {
      {{ todo.title }}
    }
  `
})
export class TodoComponent {
  todoStore = inject(TodoStore);
}
```

## Best Practices

### 1. Use Entity Adapter

```typescript
// Good: Entity adapter for collections
export interface State extends EntityState<Product> {}
export const adapter = createEntityAdapter<Product>();

// Avoid: Array operations for large collections
this.products = this.products.filter(p => p.id !== id); // Less efficient
```

### 2. Keep Effects Pure

```typescript
// Good: Effects only dispatch
loadProducts$ = createEffect(() =>
  actions$.pipe(
    ofType(loadProducts),
    switchMap(() => service.getProducts().pipe(
      map(products => loadProductsSuccess({ products }))
    ))
  )
);

// Avoid: Side effects in components
ngOnInit() {
  this.service.getProducts().subscribe(products => {
    this.products = products; // Don't do this with NgRx
  });
}
```

### 3. Use Selectors for Derived State

```typescript
// Good: Computed selectors
export const selectActiveCount = createSelector(
  selectAllTodos,
  (todos) => todos.filter(t => !t.completed).length
);

// In component
activeCount$ = this.store.select(selectActiveCount);
```

## Hands-On Exercise

### Exercise 3.1: NgRx Implementation

**Objective**: Implement state management for a shopping cart

**Requirements**:
1. Set up NgRx store
2. Create actions for add/remove/update cart items
3. Implement reducer with entity adapter
4. Create selectors for cart totals
5. Use store in component

**Deliverable**: Complete cart state management

**Assessment Criteria**:
- [ ] Store configured properly
- [ ] All CRUD actions implemented
- [ ] Selectors for computed values
- [ ] Effects for async operations
- [ ] Components using store correctly

## Summary

- **NgRx** provides predictable state management
- **Actions** describe events
- **Reducers** update state
- **Effects** handle side effects
- **Selectors** derive computed state
- **NgRx Signal Store** offers a modern, simpler alternative

## Suggested Reading

- [NgRx Documentation](https://ngrx.io/guide/store)
- "Reactive Programming with Angular and NgRx"

## Next Steps

In the next lecture, we'll explore testing strategies for Angular applications.
