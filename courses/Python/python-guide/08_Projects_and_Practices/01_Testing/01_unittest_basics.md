# unittest Basics

## What You'll Learn

- unittest.TestCase
- setUp, tearDown
- assert methods
- Running tests

## Prerequisites

- Read [03_overload_and_protocols.md](../07_Advanced_Python/03_Typing_Advanced/03_overload_and_protocols.md) first

## Basic Test

```python
import unittest

class TestMath(unittest.TestCase):
    def test_add(self):
        self.assertEqual(1 + 1, 2)
    
    def test_divide(self):
        with self.assertRaises(ZeroDivisionError):
            1 / 0

if __name__ == "__main__":
    unittest.main()
```

## setUp and tearDown

```python
class TestDatabase(unittest.TestCase):
    def setUp(self):
        self.conn = create_connection()
    
    def tearDown(self):
        self.conn.close()
    
    def test_query(self):
        # Use self.conn
        pass
```

## Summary

- **unittest**: Built-in testing framework
- **TestCase**: Base test class
- **assertEqual**: Check equality

## Next Steps

Continue to **[02_pytest_guide.md](./02_pytest_guide.md)**
