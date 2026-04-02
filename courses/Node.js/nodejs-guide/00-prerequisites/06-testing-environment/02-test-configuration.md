# Test Runner Configuration and Mock Setup

## What You'll Learn

- Configuring test runners for different scenarios
- Setting up mock testing environments
- Writing effective test scripts
- Performance testing setup

## Test Runner Configuration

### Jest Configuration File

Create `jest.config.js` in your project root:

```javascript
// jest.config.js
module.exports = {
    // Test environment
    testEnvironment: 'node',
    
    // Test file patterns
    testMatch: [
        '**/__tests__/**/*.test.js',
        '**/*.test.js',
        '**/*.spec.js'
    ],
    
    // Coverage settings
    collectCoverage: true,
    coverageDirectory: 'coverage',
    coverageReporters: ['text', 'lcov', 'html'],
    collectCoverageFrom: [
        'src/**/*.js',
        '!src/**/*.test.js',
        '!src/**/index.js'
    ],
    
    // Setup files
    setupFilesAfterEnv: ['./tests/setup.js'],
    
    // Transform files
    transform: {
        '^.+\\.js$': 'babel-jest'
    },
    
    // Module paths
    moduleNameMapper: {
        '^@/(.*)$': '<rootDir>/src/$1'
    },
    
    // Test timeout
    testTimeout: 10000,
    
    // Verbose output
    verbose: true
};
```

### Mocha Configuration

Create `.mocharc.js`:

```javascript
// .mocharc.js
module.exports = {
    // Test files
    spec: 'tests/**/*.test.js',
    
    // Reporter
    reporter: 'spec',
    
    // Timeout
    timeout: 5000,
    
    // Recursive
    recursive: true,
    
    // Require
    require: ['@babel/register'],
    
    // Watch
    watch: false,
    watchFiles: ['src/**/*.js', 'tests/**/*.js']
};
```

## Test Scripts Configuration

### package.json Scripts

```json
{
    "scripts": {
        "test": "jest",
        "test:watch": "jest --watch",
        "test:coverage": "jest --coverage",
        "test:ci": "jest --ci --coverage --forceExit",
        "test:unit": "jest --testPathPattern=unit",
        "test:integration": "jest --testPathPattern=integration",
        "test:e2e": "jest --testPathPattern=e2e",
        "test:debug": "node --inspect-brk node_modules/.bin/jest --runInBand"
    }
}
```

## Mock Testing Environment

### Setting Up Test Fixtures

```javascript
// tests/fixtures/users.js
export const mockUsers = [
    {
        id: 1,
        name: 'Alice Johnson',
        email: 'alice@example.com',
        role: 'admin',
        createdAt: new Date('2024-01-01')
    },
    {
        id: 2,
        name: 'Bob Smith',
        email: 'bob@example.com',
        role: 'user',
        createdAt: new Date('2024-01-15')
    }
];

export const mockUser = mockUsers[0];
```

### Mock Database Setup

```javascript
// tests/setup.js
import { jest } from '@jest/globals';

// Mock database module
jest.mock('../src/database', () => ({
    query: jest.fn(),
    connect: jest.fn(),
    disconnect: jest.fn(),
    transaction: jest.fn()
}));

// Mock external APIs
jest.mock('axios');

// Global test setup
beforeAll(() => {
    // Setup code before all tests
    console.log('Starting test suite...');
});

afterAll(() => {
    // Cleanup code after all tests
    console.log('Test suite completed.');
});

beforeEach(() => {
    // Reset mocks before each test
    jest.clearAllMocks();
});

afterEach(() => {
    // Cleanup after each test
});
```

### Mock Service Example

```javascript
// tests/__mocks__/userService.js
export const mockUserService = {
    getUser: jest.fn(),
    createUser: jest.fn(),
    updateUser: jest.fn(),
    deleteUser: jest.fn(),
    getAllUsers: jest.fn()
};

// Default implementations
mockUserService.getUser.mockResolvedValue({
    id: 1,
    name: 'Test User',
    email: 'test@example.com'
});

mockUserService.createUser.mockImplementation((userData) => 
    Promise.resolve({ id: 3, ...userData })
);
```

## Testing Different Scenarios

### Unit Test Example

```javascript
// tests/unit/math.test.js
import { add, subtract, multiply, divide } from '../../src/utils/math.js';

describe('Math Utilities', () => {
    describe('add()', () => {
        test('adds two positive numbers', () => {
            expect(add(2, 3)).toBe(5);
        });
        
        test('adds negative numbers', () => {
            expect(add(-2, -3)).toBe(-5);
        });
        
        test('adds zero', () => {
            expect(add(5, 0)).toBe(5);
        });
    });
    
    describe('divide()', () => {
        test('divides two numbers', () => {
            expect(divide(10, 2)).toBe(5);
        });
        
        test('throws error when dividing by zero', () => {
            expect(() => divide(10, 0)).toThrow('Cannot divide by zero');
        });
    });
});
```

### Integration Test Example

```javascript
// tests/integration/api.test.js
import request from 'supertest';
import app from '../../src/app.js';
import { mockUsers } from '../fixtures/users.js';

describe('User API', () => {
    describe('GET /api/users', () => {
        test('returns list of users', async () => {
            const response = await request(app)
                .get('/api/users')
                .expect('Content-Type', /json/)
                .expect(200);
            
            expect(response.body).toHaveProperty('users');
            expect(Array.isArray(response.body.users)).toBe(true);
        });
    });
    
    describe('POST /api/users', () => {
        test('creates a new user', async () => {
            const newUser = {
                name: 'John Doe',
                email: 'john@example.com'
            };
            
            const response = await request(app)
                .post('/api/users')
                .send(newUser)
                .expect('Content-Type', /json/)
                .expect(201);
            
            expect(response.body).toHaveProperty('id');
            expect(response.body.name).toBe(newUser.name);
        });
        
        test('validates required fields', async () => {
            const invalidUser = { name: '' };
            
            await request(app)
                .post('/api/users')
                .send(invalidUser)
                .expect(400);
        });
    });
});
```

## Performance Testing Setup

### Load Testing with Artillery

```yaml
# tests/performance/load-test.yml
config:
  target: "http://localhost:3000"
  phases:
    - duration: 60
      arrivalRate: 10
      name: "Warm up"
    - duration: 120
      arrivalRate: 50
      name: "Load test"
    - duration: 60
      arrivalRate: 100
      name: "Stress test"

scenarios:
  - name: "User API Load Test"
    flow:
      - get:
          url: "/api/users"
      - think: 1
      - post:
          url: "/api/users"
          json:
            name: "Load Test User"
            email: "loadtest@example.com"
```

### Running Performance Tests

```bash
# Install Artillery
npm install -g artillery

# Run load test
artillery run tests/performance/load-test.yml

# Generate HTML report
artillery run tests/performance/load-test.yml -o report.json
artillery report report.json
```

## Troubleshooting Common Issues

### Tests Not Running

```bash
# Problem: No tests found
# Solution: Check test file patterns

# In package.json
"test": "jest --testPathPattern='.*\\.test\\.js$'"

# Or in jest.config.js
testMatch: ['**/__tests__/**/*.test.js', '**/*.test.js']
```

### Async Tests Timing Out

```javascript
// Problem: Async callback not invoked within timeout
// Solution: Increase timeout or use async/await

test('long running operation', async () => {
    // Increase timeout for this test
    jest.setTimeout(30000);
    
    const result = await longRunningOperation();
    expect(result).toBeDefined();
}, 30000); // Alternative: pass timeout as third argument
```

### Mock Not Working

```javascript
// Problem: Mock not being called
// Solution: Ensure mock is hoisted

// BAD - mock may not be hoisted
import { service } from '../service';
jest.mock('../service');

// GOOD - mock is hoisted
jest.mock('../service');
import { service } from '../service';
```

## Best Practices Checklist

- [ ] Configure test runner for your project needs
- [ ] Set up test fixtures for consistent data
- [ ] Create mock services for external dependencies
- [ ] Write both unit and integration tests
- [ ] Set up performance testing for critical paths
- [ ] Configure code coverage thresholds
- [ ] Use meaningful test descriptions
- [ ] Clean up test data after tests
- [ ] Run tests in CI/CD pipeline
- [ ] Monitor test performance and flakiness

## Performance Optimization Tips

- Use `--runInBand` for debugging, parallel for speed
- Mock expensive operations (database, network)
- Use test-specific configuration
- Cache test dependencies
- Run only affected tests with `--changedSince`
- Use snapshot testing for UI components
- Profile slow tests and optimize

## Cross-References

- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for linting integration
- See [Debugging Setup](../09-debugging-setup/) for test debugging
- See [Git Workflow](../08-git-workflow/) for pre-commit hooks
- See [Virtual Environments](../11-virtual-environments/) for test isolation

## Next Steps

Now that testing is configured, let's set up code quality tools. Continue to [Code Quality Toolchain Setup](../07-code-quality-toolchain-setup/).