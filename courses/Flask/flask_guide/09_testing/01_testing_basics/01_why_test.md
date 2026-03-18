<!-- FILE: 09_testing/01_testing_basics/01_why_test.md -->

## Overview

Testing is essential for building reliable applications. This file explains why testing matters, the different types of tests, and how testing improves code quality and developer confidence.

## Core Concepts

### Why Test?

- Catch bugs early
- Prevent regressions
- Document expected behavior
- Enable refactoring with confidence
- Improve code design

### Types of Tests

| Type | Purpose | Speed |
|------|---------|-------|
| Unit | Test individual functions | Fast |
| Integration | Test component interactions | Medium |
| End-to-End | Test full user flows | Slow |

## Code Walkthrough

### Testing Example

```python
# test_app.py — Simple test example
import unittest
from app import app

class TestApp(unittest.TestCase):
    def setUp(self):
        """Set up test client before each test."""
        self.app = app.test_client()
        self.app.testing = True
    
    def test_home_page(self):
        """Test that home page returns 200."""
        response = self.app.get('/')
        self.assertEqual(response.status_code, 200)
    
    def test_hello_endpoint(self):
        """Test hello endpoint."""
        response = self.app.get('/hello')
        self.assertEqual(response.status_code, 200)
        self.assertIn(b'Hello', response.data)

if __name__ == '__main__':
    unittest.main()
```

## Next Steps

Now you understand why testing matters. Continue to [02_pytest_setup.md](02_pytest_setup.md) to learn about pytest setup.