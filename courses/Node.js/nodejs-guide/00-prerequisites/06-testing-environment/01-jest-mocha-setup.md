# Testing Environment Configuration

## What You'll Learn

- Setting up Jest for Node.js testing
- Mocha and Chai as alternatives
- Test runner configuration and scripts
- Mock testing environment setup

## Jest Setup

### Installation

```bash
# Install Jest
npm install --save-dev jest

# Or with specific version
npm install --save-dev jest@29

# For TypeScript projects
npm install --save-dev jest @types/jest ts-jest
```

### Basic Configuration

```javascript
// jest.config.js
module.exports = {
    // Test environment
    testEnvironment: 'node',
    
    // Test file patterns
    testMatch: [
        '**/__tests__/**/*.js',
        '**/*.test.js',
        '**/*.spec.js'
    ],
    
    // Coverage configuration
    collectCoverage: true,
    coverageDirectory: 'coverage',
    coverageReporters: ['text', 'lcov', 'html'],
    collectCoverageFrom: [
        'src/**/*.js',
        '!src/**/*.test.js',
        '!src/**/*.spec.js'
    ],
    
    // Setup files
    setupFilesAfterEnv: ['<rootDir>/jest.setup.js'],
    
    // Module paths
    moduleNameMapper: {
        '^@/(.*)$': '<rootDir>/src/$1'
    },
    
    // Transform files
    transform: {
        '^.+\\.js$': 'babel-jest'
    },
    
    // Ignore patterns
    testPathIgnorePatterns: [
        '/node_modules/',
        '/dist/'
    ]
};
```

### Jest Scripts

```json
// package.json
{
    "scripts": {
        "test": "jest",
        "test:watch": "jest --watch",
        "test:coverage": "jest --coverage",
        "test:ci": "jest --ci --coverage",
        "test:debug": "node --inspect-brk node_modules/.bin/jest --runInBand"
    }
}
```

### Writing Jest Tests

```javascript
// math.test.js
const { add, subtract, multiply, divide } = require('./math');

describe('Math functions', () => {
    test('adds two numbers', () => {
        expect(add(2, 3)).toBe(5);
    });
    
    test('subtracts two numbers', () => {
        expect(subtract(5, 3)).toBe(2);
    });
    
    test('multiplies two numbers', () => {
        expect(multiply(2, 3)).toBe(6);
    });
    
    test('divides two numbers', () => {
        expect(divide(6, 3)).toBe(2);
    });
    
    test('throws error on division by zero', () => {
        expect(() => divide(5, 0)).toThrow('Cannot divide by zero');
    });
});

// Async testing
describe('Async functions', () => {
    test('fetches user data', async () => {
        const user = await fetchUser(1);
        expect(user).toHaveProperty('id', 1);
        expect(user).toHaveProperty('name');
    });
    
    test('handles fetch error', async () => {
        await expect(fetchUser(999)).rejects.toThrow('User not found');
    });
});

// Mocking
describe('Mocking examples', () => {
    test('mocks a function', () => {
        const mockFn = jest.fn();
        mockFn.mockReturnValue('mocked');
        
        expect(mockFn()).toBe('mocked');
        expect(mockFn).toHaveBeenCalled();
    });
    
    test('mocks a module', () => {
        jest.mock('./database', () => ({
            query: jest.fn().mockResolvedValue([{ id: 1 }])
        }));
        
        const { query } = require('./database');
        expect(query).toBeDefined();
    });
});
```

## Mocha and Chai Setup

### Installation

```bash
# Install Mocha and Chai
npm install --save-dev mocha chai

# For async testing
npm install --save-dev chai-as-promised

# For mocking
npm install --save-dev sinon

# For TypeScript
npm install --save-dev @types/mocha @types/chai @types/sinon
```

### Basic Configuration

```javascript
// .mocharc.js
module.exports = {
    // Test files
    spec: 'test/**/*.test.js',
    
    // Reporter
    reporter: 'spec',
    
    // Timeout
    timeout: 5000,
    
    // Recursive search
    recursive: true,
    
    // Require modules
    require: ['chai'],
    
    // Watch files
    watch: false,
    watchFiles: ['src/**/*.js', 'test/**/*.js']
};
```

### Mocha Scripts

```json
// package.json
{
    "scripts": {
        "test": "mocha",
        "test:watch": "mocha --watch",
        "test:coverage": "nyc mocha",
        "test:debug": "mocha --inspect-brk"
    }
}
```

### Writing Mocha/Chai Tests

```javascript
// math.test.js
const { expect } = require('chai');
const { add, subtract, multiply, divide } = require('../src/math');

describe('Math functions', function() {
    describe('add()', function() {
        it('should add two positive numbers', function() {
            expect(add(2, 3)).to.equal(5);
        });
        
        it('should add negative numbers', function() {
            expect(add(-2, -3)).to.equal(-5);
        });
        
        it('should add zero', function() {
            expect(add(5, 0)).to.equal(5);
        });
    });
    
    describe('divide()', function() {
        it('should divide two numbers', function() {
            expect(divide(6, 3)).to.equal(2);
        });
        
        it('should throw error on division by zero', function() {
            expect(() => divide(5, 0)).to.throw('Cannot divide by zero');
        });
    });
});

// Async testing with chai-as-promised
const chaiAsPromised = require('chai-as-promised');
chai.use(chaiAsPromised);

describe('Async functions', function() {
    it('should resolve with user data', async function() {
        await expect(fetchUser(1)).to.eventually.have.property('id', 1);
    });
    
    it('should reject with error', async function() {
        await expect(fetchUser(999)).to.be.rejectedWith('User not found');
    });
});
```

### Sinon for Mocking

```javascript
const sinon = require('sinon');
const { expect } = require('chai');
const userService = require('../src/userService');
const database = require('../src/database');

describe('UserService', function() {
    let queryStub;
    
    beforeEach(function() {
        // Create stub before each test
        queryStub = sinon.stub(database, 'query');
    });
    
    afterEach(function() {
        // Restore stub after each test
        sinon.restore();
    });
    
    it('should get user by id', async function() {
        // Arrange
        queryStub.resolves([{ id: 1, name: 'Alice' }]);
        
        // Act
        const user = await userService.getUser(1);
        
        // Assert
        expect(user).to.deep.equal({ id: 1, name: 'Alice' });
        expect(queryStub).to.have.been.calledOnce;
    });
    
    it('should handle database error', async function() {
        // Arrange
        queryStub.rejects(new Error('Database error'));
        
        // Act & Assert
        await expect(userService.getUser(1)).to.be.rejectedWith('Database error');
    });
});
```

## Test Runner Configuration

### npm Test Script

```json
// package.json
{
    "scripts": {
        "test": "jest",
        "test:watch": "jest --watch",
        "test:coverage": "jest --coverage",
        "test:ci": "jest --ci --coverage --forceExit",
        "test:unit": "jest --testPathPattern=unit",
        "test:integration": "jest --testPathPattern=integration",
        "test:e2e": "jest --testPathPattern=e2e"
    }
}
```

### Test Organization

```
project/
├── src/
│   ├── math.js
│   └── utils.js
├── test/
│   ├── unit/
│   │   ├── math.test.js
│   │   └── utils.test.js
│   ├── integration/
│   │   └── api.test.js
│   └── e2e/
│       └── user-flow.test.js
├── jest.config.js
└── package.json
```

### Environment Variables for Testing

```javascript
// jest.setup.js
process.env.NODE_ENV = 'test';
process.env.DATABASE_URL = 'postgresql://localhost:5432/test_db';
process.env.API_KEY = 'test-api-key';

// Global test timeout
jest.setTimeout(10000);
```

## Mock Testing Environment

### Mocking External Services

```javascript
// __mocks__/axios.js
module.exports = {
    get: jest.fn(() => Promise.resolve({ data: {} })),
    post: jest.fn(() => Promise.resolve({ data: {} })),
    put: jest.fn(() => Promise.resolve({ data: {} })),
    delete: jest.fn(() => Promise.resolve({ data: {} }))
};
```

### Mocking Database

```javascript
// __mocks__/database.js
const mockData = {
    users: [
        { id: 1, name: 'Alice' },
        { id: 2, name: 'Bob' }
    ]
};

module.exports = {
    query: jest.fn((sql, params) => {
        if (sql.includes('SELECT * FROM users')) {
            return Promise.resolve(mockData.users);
        }
        return Promise.resolve([]);
    }),
    connect: jest.fn(),
    disconnect: jest.fn()
};
```

### Mocking File System

```javascript
const fs = require('fs');

// Mock fs module
jest.mock('fs');

describe('File operations', () => {
    beforeEach(() => {
        fs.readFileSync.mockClear();
    });
    
    test('reads file content', () => {
        fs.readFileSync.mockReturnValue('file content');
        
        const content = fs.readFileSync('test.txt', 'utf8');
        expect(content).toBe('file content');
        expect(fs.readFileSync).toHaveBeenCalledWith('test.txt', 'utf8');
    });
});
```

### Mocking Date and Time

```javascript
describe('Date-dependent tests', () => {
    let dateSpy;
    
    beforeEach(() => {
        // Mock Date to return fixed time
        dateSpy = jest.spyOn(Date, 'now').mockReturnValue(
            new Date('2024-01-01T00:00:00Z').getTime()
        );
    });
    
    afterEach(() => {
        dateSpy.mockRestore();
    });
    
    test('uses mocked date', () => {
        const result = getFormattedDate();
        expect(result).toBe('2024-01-01');
    });
});
```

## Coverage Configuration

### Jest Coverage

```javascript
// jest.config.js
module.exports = {
    collectCoverage: true,
    coverageDirectory: 'coverage',
    coverageReporters: ['text', 'lcov', 'html', 'json'],
    coverageThreshold: {
        global: {
            branches: 80,
            functions: 80,
            lines: 80,
            statements: 80
        }
    },
    collectCoverageFrom: [
        'src/**/*.js',
        '!src/**/*.test.js',
        '!src/**/index.js'
    ]
};
```

### NYC/Istanbul Coverage

```json
// package.json
{
    "nyc": {
        "include": ["src/**/*.js"],
        "exclude": ["src/**/*.test.js"],
        "reporter": ["text", "lcov", "html"],
        "require": ["@babel/register"],
        "all": true
    }
}
```

## Troubleshooting Common Issues

### Jest Not Finding Tests

```javascript
// Problem: No tests found
// Solution: Check testMatch pattern

// jest.config.js
module.exports = {
    testMatch: [
        '**/__tests__/**/*.js',
        '**/*.test.js',
        '**/*.spec.js'
    ]
};
```

### Async Tests Timing Out

```javascript
// Problem: Timeout - Async callback was not invoked
// Solution: Increase timeout or use async/await

// Option 1: Increase timeout
test('long running test', async () => {
    // test code
}, 30000); // 30 second timeout

// Option 2: Use async/await properly
test('fetches data', async () => {
    const data = await fetchData();
    expect(data).toBeDefined();
});
```

### Mock Not Working

```javascript
// Problem: Mock not being called
// Solution: Ensure mock is set up before import

// Must mock before requiring the module
jest.mock('./database');
const { getUser } = require('./userService');
```

### Coverage Not Collecting

```javascript
// Problem: Coverage shows 0%
// Solution: Check collectCoverageFrom pattern

// jest.config.js
module.exports = {
    collectCoverageFrom: [
        'src/**/*.js',
        '!**/node_modules/**',
        '!**/vendor/**'
    ]
};
```

## Best Practices Checklist

- [ ] Choose appropriate test framework (Jest or Mocha)
- [ ] Configure test runner properly
- [ ] Organize tests by type (unit, integration, e2e)
- [ ] Use descriptive test names
- [ ] Mock external dependencies
- [ ] Set up coverage thresholds
- [ ] Run tests in CI/CD pipeline
- [ ] Use test fixtures for consistent data
- [ ] Clean up after tests
- [ ] Test edge cases and error conditions

## Performance Optimization Tips

- Run tests in parallel with Jest
- Use `--runInBand` for debugging
- Cache test results
- Mock expensive operations
- Use test-specific database
- Avoid testing implementation details
- Use test fixtures for large datasets
- Profile slow tests

## Cross-References

- See [Code Quality Toolchain](../07-code-quality-toolchain-setup/) for linting setup
- See [Git Workflow](../08-git-workflow/) for pre-commit hooks
- See [Virtual Environments](../11-virtual-environments/) for test isolation
- See [Node.js Installation](../05-nodejs-installation/) for environment setup

## Next Steps

Now that testing is configured, let's set up code quality tools. Continue to [Code Quality Toolchain Setup](../07-code-quality-toolchain-setup/).