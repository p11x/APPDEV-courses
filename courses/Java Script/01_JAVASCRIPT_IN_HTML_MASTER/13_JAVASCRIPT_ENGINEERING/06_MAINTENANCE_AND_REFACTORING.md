# Maintenance and Refactoring

> Master code quality, technical debt management, and refactoring techniques for sustainable JavaScript development

## Table of Contents

1. [Introduction to Code Maintenance](#introduction)
2. [Code Quality](#code-quality)
3. [Technical Debt](#technical-debt)
4. [Refactoring Patterns](#refactoring-patterns)
5. [Code Smells](#code-smells)
6. [Maintainability](#maintainability)
7. [Key Takeaways](#key-takeaways)
8. [Common Pitfalls](#common-pitfalls)
9. [Related Files](#related-files)

---

## 1. Introduction to Code Maintenance

[anchor](#1-introduction-to-code-maintenance)

Code maintenance is the ongoing process of keeping software functional, secure, and maintainable. It encompasses bug fixes, performance improvements, documentation updates, and refactoring.

### Why Maintenance Matters

- **Business Value**: Well-maintained code delivers value consistently
- **Team Velocity**: Clean code enables faster development
- **Technical Health**: Prevents accumulation of technical debt
- **Security**: Regular maintenance addresses vulnerabilities

### Maintenance Costs Over Time

| Code Quality | Annual Maintenance Cost |
|--------------|------------------------|
| Excellent | 10-15% of original |
| Good | 15-25% of original |
| Poor | 40-60% of original |

---

## 2. Code Quality

[anchor](#2-code-quality)

### Quality Metrics

```javascript
// file: quality/metrics.js
// Code quality measurement utilities

class CodeQualityAnalyzer {
  #complexity = 0;
  #lines = 0;
  #functions = 0;

  analyze(code) {
    this.#complexity = this.#calculateComplexity(code);
    this.#lines = this.#countLines(code);
    this.#functions = this.#countFunctions(code);

    return {
      cyclomaticComplexity: this.#complexity,
      linesOfCode: this.#lines,
      functionCount: this.#functions,
      maintainabilityIndex: this.#calculateMaintainabilityIndex(),
      linesPerFunction: this.#functions > 0 ? this.#lines / this.#functions : 0
    };
  }

  #calculateComplexity(code) {
    let complexity = 1;
    const patterns = [
      /if\s*\(/g,
      /else\s+if\s*\(/g,
      /for\s*\(/g,
      /while\s*\(/g,
      /\?\s*[^:]+:/g,
      /case\s+/g,
      /catch\s*\(/g,
      /&&/g,
      /\|\|/g
    ];

    patterns.forEach(pattern => {
      const matches = code.match(pattern);
      if (matches) complexity += matches.length;
    });

    return complexity;
  }

  #countLines(code) {
    return code.split('\n').filter(line => line.trim().length > 0).length;
  }

  #countFunctions(code) {
    const functionPatterns = [
      /function\s+\w+\s*\(/g,
      /\w+\s*=\s*(?:async\s+)?\(/g,
      /const\s+\w+\s*=\s*(?:async\s+)?\(/g
    ];

    let count = 0;
    functionPatterns.forEach(pattern => {
      const matches = code.match(pattern);
      if (matches) count += matches.length;
    });

    return count;
  }

  #calculateMaintainabilityIndex() {
    const mi = 171 - 5.2 * Math.log(this.#lines) - 0.23 * this.#complexity - 16.2 * Math.log(this.#functions);
    return Math.max(0, Math.min(100, mi));
  }
}

const analyzer = new CodeQualityAnalyzer();
const code = `
function processData(items) {
  if (items && items.length > 0) {
    return items.filter(i => i.active).map(i => i.value);
  }
  return [];
}
`;

console.log(analyzer.analyze(code));
```

### Code Review Checklist

```javascript
// file: quality/checklist.js
// Automated code review checks

class CodeReviewer {
  #issues = [];

  review(code, filename) {
    this.#issues = [];

    this.#checkComplexity(code);
    this.#checkNaming(code);
    this.#checkErrorHandling(code);
    this.#checkSecurity(code);
    this.#checkPerformance(code);

    return {
      filename,
      issues: this.#issues,
      score: this.#calculateScore()
    };
  }

  #checkComplexity(code) {
    const ifCount = (code.match(/if\s*\(/g) || []).length;
    const forCount = (code.match(/for\s*\(/g) || []).length;
    const whileCount = (code.match(/while\s*\(/g) || []).length;

    const total = ifCount + forCount + whileCount;

    if (total > 10) {
      this.#issues.push({
        type: 'complexity',
        severity: 'high',
        message: `High complexity: ${total} control structures`
      });
    }
  }

  #checkNaming(code) {
    const singleCharVars = code.match(/\b[a-z]\b(?!\s*[=({])/g);
    if (singleCharVars && singleCharVars.length > 3) {
      this.#issues.push({
        type: 'naming',
        severity: 'medium',
        message: 'Multiple single-character variable names detected'
      });
    }
  }

  #checkErrorHandling(code) {
    const hasTryCatch = code.includes('try') && code.includes('catch');
    const hasThrow = code.includes('throw');

    if (hasThrow && !hasTryCatch) {
      this.#issues.push({
        type: 'error-handling',
        severity: 'high',
        message: 'Errors are thrown but not caught'
      });
    }
  }

  #checkSecurity(code) {
    if (code.includes('eval(')) {
      this.#issues.push({
        type: 'security',
        severity: 'critical',
        message: 'eval() usage detected - potential code injection'
      });
    }

    if (code.includes('innerHTML')) {
      this.#issues.push({
        type: 'security',
        severity: 'high',
        message: 'innerHTML usage - potential XSS vulnerability'
      });
    }
  }

  #checkPerformance(code) {
    if (code.includes('document.write')) {
      this.#issues.push({
        type: 'performance',
        severity: 'medium',
        message: 'document.write() - can cause performance issues'
      });
    }
  }

  #calculateScore() {
    let score = 100;
    this.#issues.forEach(issue => {
      switch (issue.severity) {
        case 'critical': score -= 20; break;
        case 'high': score -= 10; break;
        case 'medium': score -= 5; break;
        case 'low': score -= 2; break;
      }
    });
    return Math.max(0, score);
  }
}

const reviewer = new CodeReviewer();
const code = `
function x(a, b) {
  eval(a);
  document.write(b);
}
`;
console.log(reviewer.review(code, 'example.js'));
```

---

## 3. Technical Debt

[anchor](#3-technical-debt)

### Tracking Technical Debt

```javascript
// file: debt/tracker.js
// Technical debt tracking system

class DebtTracker {
  #debts = [];

  addDebt(debt) {
    this.#debts.push({
      id: crypto.randomUUID(),
      ...debt,
      createdAt: new Date().toISOString(),
      interest: 0
    });
  }

  updateInterest() {
    this.#debts.forEach(debt => {
      const daysSinceCreated = Math.floor(
        (Date.now() - new Date(debt.createdAt)) / (1000 * 60 * 60 * 24)
      );
      debt.interest = daysSinceCreated * debt.dailyInterestRate;
    });
  }

  getTotalDebt() {
    this.updateInterest();
    return this.#debts.reduce((total, debt) => {
      return total + debt.estimatedHours + debt.interest;
    }, 0);
  }

  getDebtsByCategory() {
    return this.#debts.reduce((acc, debt) => {
      acc[debt.category] = (acc[debt.category] || 0) + debt.estimatedHours;
      return acc;
    }, {});
  }

  getDebtsByPriority() {
    return this.#debts.sort((a, b) => {
      const priorityOrder = { critical: 0, high: 1, medium: 2, low: 3 };
      return priorityOrder[a.priority] - priorityOrder[b.priority];
    });
  }

  recordPayment(debtId, hours) {
    const debt = this.#debts.find(d => d.id === debtId);
    if (debt) {
      debt.estimatedHours -= hours;
      if (debt.estimatedHours <= 0) {
        this.#removeDebt(debtId);
      }
    }
  }

  #removeDebt(id) {
    this.#debts = this.#debts.filter(d => d.id !== id);
  }
}

const tracker = new DebtTracker();

tracker.addDebt({
  title: 'Authentication system refactor',
  category: 'security',
  priority: 'high',
  estimatedHours: 40,
  dailyInterestRate: 0.5,
  relatedFiles: ['auth/*.js']
});

tracker.addDebt({
  title: 'Legacy jQuery dependencies',
  category: 'dependencies',
  priority: 'medium',
  estimatedHours: 20,
  dailyInterestRate: 0.25,
  relatedFiles: ['src/ui/**/*.js']
});

console.log('Total debt hours:', tracker.getTotalDebt());
console.log('By category:', tracker.getDebtsByCategory());
```

### Managing Debt Strategically

```javascript
// file: debt/strategy.js
// Strategic debt management

class DebtStrategy {
  #capacity;
  #tracker;

  constructor(weeklyCapacityHours, tracker) {
    this.#capacity = weeklyCapacityHours;
    this.#tracker = tracker;
  }

  getSprintRecommendation() {
    const totalDebt = this.#tracker.getDebtsByPriority();
    const categoryBreakdown = this.#tracker.getDebtsByCategory();

    const criticalDebt = totalDebt.filter(d => d.priority === 'critical');
    const highDebt = totalDebt.filter(d => d.priority === 'high');

    const recommendedAllocation = {
      critical: Math.min(this.#capacity * 0.3, criticalDebt.length * 8),
      high: Math.min(this.#capacity * 0.4, highDebt.length * 4),
      maintenance: this.#capacity * 0.3
    };

    return {
      recommended: recommendedAllocation,
      debtByCategory: categoryBreakdown,
      paydownSuggestion: this.#generatePaydownPlan(totalDebt)
    };
  }

  #generatePaydownPlan(debts) {
    const plan = [];
    let remainingCapacity = this.#capacity * 0.3;

    for (const debt of debts) {
      if (remainingCapacity <= 0) break;

      const allocation = Math.min(debt.estimatedHours, remainingCapacity);
      if (allocation > 0) {
        plan.push({
          title: debt.title,
          allocatedHours: allocation,
          priority: debt.priority
        });
        remainingCapacity -= allocation;
      }
    }

    return plan;
  }
}
```

---

## 4. Refactoring Patterns

[anchor](#4-refactoring-patterns)

### Extract Method

```javascript
// file: refactoring/extract-method.js
// Before: Monolithic function
function processUserRegistration(userData) {
  const errors = [];

  if (!userData.email || !userData.email.includes('@')) {
    errors.push('Invalid email');
  }

  if (!userData.password || userData.password.length < 8) {
    errors.push('Password too short');
  }

  if (!userData.name || userData.name.trim().length < 2) {
    errors.push('Name required');
  }

  if (errors.length > 0) {
    return { success: false, errors };
  }

  const existingUser = database.findByEmail(userData.email);
  if (existingUser) {
    return { success: false, errors: ['Email already exists'] };
  }

  const passwordHash = await bcrypt.hash(userData.password, 10);
  const user = {
    id: crypto.randomUUID(),
    email: userData.email.toLowerCase(),
    name: userData.name.trim(),
    passwordHash,
    createdAt: new Date().toISOString()
  };

  await database.save(user);
  await sendWelcomeEmail(user);

  return { success: true, user };
}

// After: Extracted methods
class UserRegistrationService {
  #database;
  #emailService;

  constructor(database, emailService) {
    this.#database = database;
    this.#emailService = emailService;
  }

  async register(userData) {
    const validation = this.#validateUserData(userData);
    if (!validation.valid) {
      return { success: false, errors: validation.errors };
    }

    const existingUser = await this.#checkExistingUser(userData.email);
    if (existingUser) {
      return { success: false, errors: ['Email already exists'] };
    }

    const user = await this.#createUser(userData);
    await this.#emailService.sendWelcome(user);

    return { success: true, user };
  }

  #validateUserData(userData) {
    const errors = [];

    if (!this.#isValidEmail(userData.email)) {
      errors.push('Invalid email');
    }

    if (!this.#isValidPassword(userData.password)) {
      errors.push('Password too short');
    }

    if (!this.#isValidName(userData.name)) {
      errors.push('Name required');
    }

    return { valid: errors.length === 0, errors };
  }

  #isValidEmail(email) {
    return email && email.includes('@');
  }

  #isValidPassword(password) {
    return password && password.length >= 8;
  }

  #isValidName(name) {
    return name && name.trim().length >= 2;
  }

  async #checkExistingUser(email) {
    return this.#database.findByEmail(email.toLowerCase());
  }

  async #createUser(userData) {
    const passwordHash = await bcrypt.hash(userData.password, 10);
    const user = {
      id: crypto.randomUUID(),
      email: userData.email.toLowerCase(),
      name: userData.name.trim(),
      passwordHash,
      createdAt: new Date().toISOString()
    };

    await this.#database.save(user);
    return user;
  }
}
```

### Replace Conditional with Polymorphism

```javascript
// file: refactoring/polymorphism.js
// Before: Complex conditionals
class PaymentCalculator {
  calculate(payment) {
    let discount = 0;

    if (payment.type === 'standard') {
      if (payment.customer.tier === 'gold') {
        discount = 0.2;
      } else if (payment.customer.tier === 'silver') {
        discount = 0.1;
      }
    } else if (payment.type === 'premium') {
      if (payment.customer.tier === 'gold') {
        discount = 0.3;
      }
    } else if (payment.type === 'enterprise') {
      discount = 0.4;
    }

    if (payment.isHoliday) {
      discount += 0.1;
    }

    return payment.amount * (1 - discount);
  }
}

// After: Polymorphic approach
class PaymentStrategy {
  calculate(payment) {
    throw new Error('Not implemented');
  }
}

class StandardPayment extends PaymentStrategy {
  calculate(payment) {
    const tierDiscount = {
      gold: 0.2,
      silver: 0.1,
      bronze: 0
    }[payment.customer.tier] || 0;

    return payment.amount * (1 - tierDiscount);
  }
}

class PremiumPayment extends PaymentStrategy {
  calculate(payment) {
    const discount = payment.customer.tier === 'gold' ? 0.3 : 0.15;
    return payment.amount * (1 - discount);
  }
}

class EnterprisePayment extends PaymentStrategy {
  calculate(payment) {
    return payment.amount * 0.6;
  }
}

class PaymentCalculator {
  #strategies = {
    standard: new StandardPayment(),
    premium: new PremiumPayment(),
    enterprise: new EnterprisePayment()
  };

  calculate(payment) {
    const strategy = this.#strategies[payment.type];
    let amount = strategy.calculate(payment);

    if (payment.isHoliday) {
      amount *= 0.9;
    }

    return amount;
  }
}
```

### Introduce Parameter Object

```javascript
// file: refactoring/parameter-object.js
// Before: Many parameters
function createOrder(
  customerId,
  productId,
  quantity,
  shippingAddress,
  billingAddress,
  paymentMethod,
  couponCode,
  notes
) {
  // Implementation
}

// After: Parameter object
class OrderRequest {
  #data;

  constructor(data) {
    this.#data = {
      customerId: data.customerId,
      productId: data.productId,
      quantity: data.quantity || 1,
      shippingAddress: data.shippingAddress,
      billingAddress: data.billingAddress,
      paymentMethod: data.paymentMethod,
      couponCode: data.couponCode,
      notes: data.notes,
      createdAt: new Date().toISOString()
    };
  }

  get customerId() { return this.#data.customerId; }
  get productId() { return this.#data.productId; }
  get quantity() { return this.#data.quantity; }
  get shippingAddress() { return this.#data.shippingAddress; }
  get hasCoupon() { return !!this.#data.couponCode; }
}

function createOrder(request) {
  const orderRequest = request instanceof OrderRequest 
    ? request 
    : new OrderRequest(request);

  // Implementation using orderRequest
  return { success: true, orderId: crypto.randomUUID() };
}
```

---

## 5. Code Smells

[anchor](#5-code-smells)

### Identifying Code Smells

```javascript
// file: smells/detector.js
// Code smell detection

class CodeSmellDetector {
  detectSmells(code) {
    const smells = [];

    smells.push(...this.#detectLongMethods(code));
    smells.push(...this.#detectDuplicateCode(code));
    smells.push(...this.#detectGodClasses(code));
    smells.push(...this.#detectDataClumps(code));
    smells.push(...this.#detectFeatureEnvy(code));

    return smells;
  }

  #detectLongMethods(code) {
    const lines = code.split('\n');
    if (lines.length > 50) {
      return [{
        type: 'Long Method',
        severity: 'medium',
        message: `Method has ${lines.length} lines (recommended: < 50)`
      }];
    }
    return [];
  }

  #detectDuplicateCode(code) {
    const lines = code.split('\n').filter(l => l.trim().length > 0);
    const duplicates = [];

    for (let i = 0; i < lines.length; i++) {
      for (let j = i + 1; j < lines.length; j++) {
        if (lines[i] === lines[j]) {
          duplicates.push({
            type: 'Duplicate Code',
            severity: 'medium',
            message: `Duplicate line found at ${i} and ${j}`
          });
        }
      }
    }

    return duplicates.slice(0, 3);
  }

  #detectGodClasses(code) {
    const classMatches = code.match(/class\s+\w+/g) || [];
    const methodMatches = code.match(/\s+\w+\s*\([^)]*\)\s*{/g) || [];

    if (methodMatches.length > 10) {
      return [{
        type: 'God Class',
        severity: 'high',
        message: `Class has ${methodMatches.length} methods (recommended: < 10)`
      }];
    }

    return [];
  }

  #detectDataClumps(code) {
    const params = code.match(/\(([^)]+)\)/g);
    if (params && params.length > 3) {
      return [{
        type: 'Data Clumps',
        severity: 'low',
        message: 'Multiple parameters could be grouped into objects'
      }];
    }
    return [];
  }

  #detectFeatureEnvy(code) {
    const getMatches = code.match(/\.get\w+\(/g) || [];
    if (getMatches.length > 5) {
      return [{
        type: 'Feature Envy',
        severity: 'medium',
        message: 'Method seems to overuse external object access'
      }];
    }
    return [];
  }
}

const detector = new CodeSmellDetector();
const code = `
function processEverything(
  a, b, c, d, e, f, g, h, i, j,
  k, l, m, n, o, p, q, r, s, t
) {
  // ... 100 lines of code
  const x = obj.getA();
  const y = obj.getB();
  const z = obj.getC();
  // ... more code
}
`;

console.log(detector.detectSmells(code));
```

---

## 6. Maintainability

[anchor](#6-maintainability)

### Measuring Maintainability

```javascript
// file: maintainability/index.js
// Maintainability score calculation

class MaintainabilityIndex {
  calculate(code, complexity, lines, functions) {
    const mi = this.#calculateMi(lines, complexity, functions);
    const a = this.#calculateAccessibility(code);
    const d = this.#calculateDocumentation(code);
    const c = this.#calculateConsistency(code);

    return {
      maintainabilityIndex: Math.round(mi),
      accessibility: Math.round(a),
      documentation: Math.round(d),
      consistency: Math.round(c),
      grade: this.#getGrade(mi)
    };
  }

  #calculateMi(lines, complexity, functions) {
    if (lines === 0 || functions === 0) return 0;

    const mi = 171 - 5.2 * Math.log(lines) - 0.23 * complexity - 16.2 * Math.log(functions);
    return Math.max(0, Math.min(100, mi));
  }

  #calculateAccessibility(code) {
    const hasErrorHandling = code.includes('try') ? 20 : 0;
    const hasValidation = code.includes('validate') || code.includes('check') ? 20 : 0;
    const hasTypes = code.includes(': ') ? 20 : 0;
    const hasDefaults = code.includes('??') || code.includes('||') ? 20 : 0;
    const hasComments = (code.match(/\/\//g) || []).length > 0 ? 20 : 0;

    return Math.min(100, hasErrorHandling + hasValidation + hasTypes + hasDefaults + hasComments);
  }

  #calculateDocumentation(code) {
    const lines = code.split('\n').filter(l => l.trim().length > 0).length;
    const jdocs = (code.match(/\/\*\*[\s\S]*?\*\//g) || []).length;
    const moduleExports = code.includes('export') ? 20 : 0;

    const score = (jdocs * 10 + moduleExports + (lines > 10 ? 10 : 0));
    return Math.min(100, score);
  }

  #calculateConsistency(code) {
    const hasConst = code.includes('const ');
    const hasLet = code.includes('let ');
    const hasVar = code.includes('var ');

    let score = 50;
    if (!hasVar) score += 20;
    if (hasConst && !hasLet) score += 20;
    if (code.includes('=>') !== code.includes('function')) score += 10;

    return score;
  }

  #getGrade(mi) {
    if (mi >= 80) return 'A - Excellent';
    if (mi >= 65) return 'B - Good';
    if (mi >= 50) return 'C - Moderate';
    if (mi >= 35) return 'D - Low';
    return 'F - Poor';
  }
}

const mi = new MaintainabilityIndex();
console.log(mi.calculate(code, 5, 100, 3));
```

---

## Key Takeaways

- **Quality Metrics**: Use cyclomatic complexity and maintainability index
- **Technical Debt**: Track and prioritize systematically
- **Refactoring**: Use proven patterns like Extract Method, Introduce Parameter Object
- **Code Smells**: Detect and address early
- **Maintainability**: Measure continuously

### Refactoring Best Practices

1. **Test First**: Ensure tests pass before and after
2. **Small Steps**: Make incremental changes
3. **Version Control**: Commit frequently
4. **Review**: Get peer feedback during refactoring

### Security Considerations

- **Never introduce vulnerabilities during refactoring
- **Preserve input validation
- **Keep security checks intact

---

## Common Pitfalls

1. **Refactoring Without Tests**: Increases risk significantly
2. **Big Bang Changes**: Hard to review and debug
3. **Ignoring Documentation**: Makes future maintenance harder
4. **Breaking Existing APIs**: Causes breaking changes
5. **Not Tracking Debt**: Leads to accumulated technical debt

---

## Related Files

- [Design Patterns in JavaScript](./01_DESIGN_PATTERNS_JAVASCRIPT.md) - Refactoring patterns
- [SOLID Principles in JavaScript](./02_SOLID_PRINCIPLES_JAVASCRIPT.md) - Code quality principles
- [Code Organization and Structure](./04_CODE_ORGANIZATION_AND_STRUCTURE.md) - Maintainable structure

---

## Practice Exercises

1. **Beginner**: Refactor a long function using Extract Method
2. **Intermediate**: Apply Strategy pattern to replace conditionals
3. **Advanced**: Build automated technical debt tracking system