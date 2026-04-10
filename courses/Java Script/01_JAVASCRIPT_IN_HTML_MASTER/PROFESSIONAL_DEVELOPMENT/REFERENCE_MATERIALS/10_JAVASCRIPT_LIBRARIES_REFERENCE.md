# 📚 JavaScript Libraries Reference

## Essential Library Ecosystem

---

## Table of Contents

1. [Utility Libraries](#utility-libraries)
2. [UI Libraries](#ui-libraries)
3. [Data Visualization](#data-visualization)
4. [Testing Libraries](#testing-libraries)

---

## Utility Libraries

### Lodash

```javascript
import _ from 'lodash';

// Collection operations
_.groupBy(users, 'role');
_.orderBy(users, ['age'], ['desc']);
_.chunk(array, 4);

// Object operations
_.pick(obj, ['name', 'age']);
_.omit(obj, ['password']);
_.merge(obj1, obj2);

// Utility
_.debounce(func, 300);
_.throttle(func, 300);
```

### Moment.js / Day.js

```javascript
import dayjs from 'dayjs';

// Parse
dayjs('2024-01-01');

// Format
dayjs().format('YYYY-MM-DD');

// Add/subtract
dayjs().add(1, 'week');
dayjs().subtract(1, 'month');
```

---

## UI Libraries

### Bootstrap

```html
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

### Tailwind CSS

```bash
npm install -D tailwindcss
npx tailwindcss init
```

```javascript
// tailwind.config.js
module.exports = {
  content: ['./src/**/*.{html,js}'],
  theme: { extend: {} },
  plugins: []
};
```

---

## Data Visualization

### Chart.js

```javascript
import Chart from 'chart.js/auto';

const chart = new Chart(ctx, {
  type: 'line',
  data: {
    labels: ['Jan', 'Feb', 'Mar'],
    datasets: [{
      label: 'Sales',
      data: [100, 200, 150]
    }]
  }
});
```

### D3.js

```javascript
import * as d3 from 'd3';

const svg = d3.select('svg');
const circle = svg.append('circle')
  .attr('cx', 50)
  .attr('cy', 50)
  .attr('r', 40)
  .style('fill', 'blue');
```

---

## Testing Libraries

### Jest

```javascript
test('adds numbers', () => {
  expect(add(2, 3)).toBe(5);
});
```

### Vitest

```javascript
import { describe, it, expect } from 'vitest';

describe('math', () => {
  it('adds', () => {
    expect(1 + 1).toBe(2);
  });
});
```

---

## Summary

### Key Takeaways

1. **Lodash**: Utility functions
2. **UI**: Bootstrap, Tailwind
3. **Charts**: Chart.js, D3
4. **Testing**: Jest, Vitest

### Next Steps

- Continue with: [11_JAVASCRIPT_ADVANCED_TECHNIQUES.md](11_JAVASCRIPT_ADVANCED_TECHNIQUES.md)
- Choose libraries carefully
- Keep dependencies updated

---

## Cross-References

- **Previous**: [09_JAVASCRIPT_FRAMEWORK_COMPARISON.md](09_JAVASCRIPT_FRAMEWORK_COMPARISON.md)
- **Next**: [11_JAVASCRIPT_ADVANCED_TECHNIQUES.md](11_JAVASCRIPT_ADVANCED_TECHNIQUES.md)

---

*Last updated: 2024*