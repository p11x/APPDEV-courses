# Child Process Testing Strategies

## What You'll Learn

- Unit testing child process code
- Mocking child processes
- Integration testing with real processes
- CI/CD integration
- Error scenario testing

## Mocking Child Processes

```js
// test/process-mock.js — Mock child processes for testing
import { test, describe, beforeEach } from 'node:test';
import assert from 'node:assert/strict';

// Mock execFile for unit tests
function mockExecFile(expectedOutput) {
    return async (command, args, options) => {
        if (expectedOutput.error) throw expectedOutput.error;
        return {
            stdout: expectedOutput.stdout || '',
            stderr: expectedOutput.stderr || '',
        };
    };
}

// Test a function that uses execFile
describe('GitService', () => {
    let originalExecFile;

    beforeEach(() => {
        // Store original
        originalExecFile = globalThis.execFile;
    });

    test('getStatus returns parsed output', async () => {
        // Mock git status output
        const mockExec = mockExecFile({
            stdout: 'M file1.js\nA file2.js\n',
        });

        const service = new GitService(mockExec);
        const status = await service.getStatus();

        assert.deepEqual(status.modified, ['file1.js']);
        assert.deepEqual(status.added, ['file2.js']);
    });

    test('handles git not found', async () => {
        const mockExec = mockExecFile({
            error: Object.assign(new Error('ENOENT'), { code: 'ENOENT' }),
        });

        const service = new GitService(mockExec);
        await assert.rejects(
            () => service.getStatus(),
            { message: 'git not found' }
        );
    });

    test('handles non-zero exit code', async () => {
        const mockExec = mockExecFile({
            error: Object.assign(new Error('not a git repo'), {
                code: 128,
                stderr: 'fatal: not a git repository',
            }),
        });

        const service = new GitService(mockExec);
        await assert.rejects(
            () => service.getStatus(),
            { message: 'not a git repo' }
        );
    });
});
```

## Integration Testing with Real Processes

```js
// test/process-integration.test.js
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';
import { ProcessErrorHandler } from '../lib/process-error-handler.js';

const execFileAsync = promisify(execFile);

describe('Child Process Integration', () => {
    test('runs node --version successfully', async () => {
        const { stdout } = await execFileAsync('node', ['--version']);
        assert.ok(stdout.startsWith('v'));
    });

    test('handles command not found', async () => {
        await assert.rejects(
            () => execFileAsync('nonexistent-command', []),
            { code: 'ENOENT' }
        );
    });

    test('handles non-zero exit', async () => {
        const err = await assert.rejects(
            () => execFileAsync('node', ['-e', 'process.exit(42)']),
        );
        assert.equal(err.code, 42);
    });

    test('captures stderr', async () => {
        const err = await assert.rejects(
            () => execFileAsync('node', ['-e', 'console.error("error")']),
        );
        assert.ok(err.stderr.includes('error'));
    });

    test('timeout kills process', async () => {
        const handler = new ProcessErrorHandler({ timeout: 1000 });
        await assert.rejects(
            () => handler.execute('sleep', ['10']),
            { message: /timed out/i }
        );
    });

    test('retry on failure', async () => {
        let attempts = 0;
        const flakyHandler = new ProcessErrorHandler({
            maxRetries: 3,
            retryDelay: 100,
            onRetry: () => { attempts++; },
        });

        // This will always fail, but should retry
        try {
            await flakyHandler.execute('node', ['-e', 'process.exit(1)']);
        } catch {
            // Expected
        }

        assert.equal(attempts, 2); // 3 total attempts, 2 retries
    });
});
```

## Performance Testing

```js
// test/process-performance.test.js
import { test, describe } from 'node:test';
import assert from 'node:assert/strict';
import { performance } from 'node:perf_hooks';
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

describe('Process Performance', () => {
    test('execFile completes within 100ms', async () => {
        const start = performance.now();
        await execFileAsync('node', ['--version']);
        const elapsed = performance.now() - start;

        assert.ok(elapsed < 100, `Took ${elapsed.toFixed(1)}ms, expected <100ms`);
    });

    test('100 sequential processes complete within 10s', async () => {
        const start = performance.now();

        for (let i = 0; i < 100; i++) {
            await execFileAsync('node', ['-e', 'process.exit(0)']);
        }

        const elapsed = performance.now() - start;
        assert.ok(elapsed < 10000, `Took ${elapsed.toFixed(0)}ms for 100 processes`);
    });
});
```

## Common Mistakes

- Not mocking child processes in unit tests (slow, flaky)
- Not testing error scenarios (ENOENT, timeout, non-zero exit)
- Not running integration tests in CI
- Not testing cross-platform differences

## Try It Yourself

### Exercise 1: Mock Tests
Write unit tests for a git wrapper using mocked execFile.

### Exercise 2: Integration Tests
Test all error scenarios with real child processes.

### Exercise 3: CI Integration
Add child process tests to your GitHub Actions pipeline.

## Next Steps

Continue to [Deployment](../10-deployment/01-production.md).
