<!-- FILE: 09_testing/02_unit_tests/03_mocking_dependencies.md -->

## Overview

Mocking is a technique used in testing to replace external dependencies with fake objects. This allows you to test your code in isolation without relying on external services like APIs, databases, or file systems.

## Prerequisites

- Basic testing knowledge
- Familiarity with the `unittest.mock` library (included in Python 3.3+)

## Core Concepts

### Why Mock?

- Isolate the unit under test
- Avoid side effects (e.g., sending emails, making real API calls)
- Control the behavior of dependencies
- Test error conditions

### The `unittest.mock` Library

The `unittest.mock` library provides:
- `Mock` and `MagicMock` classes
- `patch` decorator and context manager
- `PropertyMock` for properties

## Code Walkthrough

### Basic Mocking

```python
# tests/test_mocking.py
from unittest.mock import Mock, patch
import pytest

def test_external_api_call():
    """Test a function that calls an external API."""
    # Mock the requests.get function
    with patch('requests.get') as mock_get:
        # Configure the mock to return a specific response
        mock_response = Mock()
        mock_response.status_code = 200
        mock_response.json.return_value = {'data': 'value'}
        mock_get.return_value = mock_response
        
        # Call the function that uses requests.get
        result = get_data_from_api()
        
        # Assert the function behaved correctly
        assert result == {'data': 'value'}
        mock_get.assert_called_once_with('https://api.example.com/data')
```

### Mocking a Class

```python
def test_email_service():
    """Test a function that sends an email."""
    with patch('app.services.EmailService') as MockEmailService:
        # Configure the mock instance
        mock_instance = MockEmailService.return_value
        mock_instance.send.return_value = True
        
        # Call the function that uses EmailService
        result = send_notification()
        
        # Assert
        assert result is True
        mock_instance.send.assert_called_once()
```

### Mocking Database Queries

```python
def test_user_lookup():
    """Test a function that queries the database."""
    with patch('app.models.User.query') as mock_query:
        # Configure the mock to return a specific user
        mock_user = Mock()
        mock_user.username = 'testuser'
        mock_query.filter_by.return_value.first.return_value = mock_user
        
        # Call the function
        user = get_user_by_username('testuser')
        
        # Assert
        assert user.username == 'testuser'
        mock_query.filter_by.assert_called_once_with(username='testuser')
```

### Using `side_effect` for Dynamic Responses

```python
def test_various_inputs():
    """Test a function with different inputs."""
    with patch('app.utils.process_data') as mock_process:
        # Configure side effect to return different values based on input
        mock_process.side_effect = lambda x: x * 2
        
        # Test
        assert process_and_double(5) == 10
        assert process_and_double(10) == 20
        
        # Assert calls
        assert mock_process.call_count == 2
        mock_process.assert_any_call(5)
        mock_process.assert_any_call(10)
```

### Mocking Flask Extensions

```python
def test_email_sent():
    """Test that an email is sent after user registration."""
    with patch('flask_mail.Message') as MockMessage, \
         patch('flask_mail.Mail.send') as mock_send:
        
        # Configure the mock
        mock_instance = MockMessage.return_value
        mock_send.return_value = True
        
        # Call the registration function
        result = register_user('test@example.com', 'password')
        
        # Assert
        assert result is True
        mock_send.assert_called_once()
        # Check that the message was created with correct arguments
        MockMessage.assert_called_once()
        args, kwargs = MockMessage.call_args
        assert 'Welcome' in kwargs['subject'] or args[0]  # Depending on how it's called
```

## Common Mistakes

❌ **Mocking the wrong thing**
```python
# WRONG — Mocking at the wrong level
with patch('requests') as mock_requests:  # Too broad
    mock_requests.get.return_value = Mock()
```

✅ **Correct — Mock the specific function you use**
```python
# CORRECT
with patch('module.requests.get') as mock_get:
    mock_get.return_value = Mock()
```

❌ **Not resetting mocks between tests**
```python
# WRONG — Mock state persists
def test_one():
    with patch('requests.get') as mock_get:
        mock_get.return_value = Mock()
        # ... test ...

def test_two():
    # mock_get might still have the return value from test_one
    with patch('requests.get') as mock_get:
        # ... test ...
```

✅ **Correct — Use fresh mocks in each test**
```python
# CORRECT — Each test gets its own mock
def test_one():
    with patch('requests.get') as mock_get:
        # ... test ...

def test_two():
    with patch('requests.get') as mock_get:
        # ... test ...
```

## Quick Reference

| Technique | Code |
|-----------|------|
| Basic mock | `with patch('module.function') as mock:` |
| Mock return value | `mock.return_value = value` |
| Mock side effect | `mock.side_effect = lambda x: x*2` |
| Assert calls | `mock.assert_called_once_with(arg)` |
| Mock class | `with patch('module.Class') as MockClass:` |

## Next Steps

Now you can mock dependencies. Continue to [01_testing_with_database.md](../03_integration_tests/01_testing_with_database.md) to learn about integration testing with databases.