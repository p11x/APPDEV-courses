# RxJS Best Practices

## What You'll Learn

- Memory leak prevention
- Subscription management
- Operator selection
- Performance optimization

---

## Best Practices

```typescript
import { Subscription } from 'rxjs';

class MyComponent {
  private subscriptions: Subscription[] = [];

  ngOnInit() {
    // Track subscriptions
    this.subscriptions.push(
      this.data$.subscribe(handleData)
    );
  }

  ngOnDestroy() {
    // Clean up
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}

// Or use takeUntil
destroy$ = new Subject<void>();

ngOnInit() {
  this.data$
    .pipe(takeUntil(this.destroy$))
    .subscribe();
}

ngOnDestroy() {
  this.destroy$.next();
  this.destroy$.complete();
}
```
