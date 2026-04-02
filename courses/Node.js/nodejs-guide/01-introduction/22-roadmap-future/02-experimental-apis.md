# Experimental API Exploration

## What You'll Learn

- Using Node.js experimental features
- Permission model hands-on
- Built-in test runner advanced features
- Testing experimental APIs safely

## Permission Model (Node.js 20+)

```bash
# Enable permission model
node --experimental-permission --allow-fs-read=/app --allow-fs-write=/app/data app.js
```

```javascript
// Check if running with permissions
if (process.permission) {
    console.log('Permission model active');
    console.log('Can read /etc:', process.permission.has('fs.read', '/etc'));
}
```

## Built-in Watch Mode

```bash
# Watch all files
node --watch app.js

# Watch specific paths
node --watch-path=./src --watch-path=./config app.js
```

## Test Runner Advanced Features

```javascript
import { describe, it, mock, snapshot } from 'node:test';

describe('Advanced testing', () => {
    it('snapshot testing', (t) => {
        const result = { name: 'test', value: 42 };
        t.assert.snapshot(result);
    });
    
    it('mock timers', async (t) => {
        t.mock.timers.enable({ apis: ['Date'] });
        t.mock.timers.setTime(1000);
        assert.strictEqual(Date.now(), 1000);
    });
    
    it('subtests', async (t) => {
        await t.test('subtest 1', () => {
            assert.strictEqual(1 + 1, 2);
        });
        await t.test('subtest 2', () => {
            assert.strictEqual(2 + 2, 4);
        });
    });
});
```

## Best Practices Checklist

- [ ] Use experimental features only in development
- [ ] Document experimental flag usage
- [ ] Monitor Node.js changelogs for stability changes
- [ ] Have fallback for experimental features
- [ ] Test experimental features across Node.js versions

## Cross-References

- See [Upcoming Features](./01-upcoming-features.md) for roadmap
- See [Community Contributions](./03-community-future.md) for governance
- See [Testing Frameworks](../20-modern-workflows/03-testing-frameworks.md) for test setup

## Next Steps

Continue to [Community and Future](./03-community-future.md) for governance insights.
