# Understanding Contribution Guidelines

## What You'll Learn

- Reading CONTRIBUTING files
- Code style requirements
- Testing requirements

## Prerequisites

- Completed `04-making-your-first-contribution.md`

## Reading CONTRIBUTING.md

Most projects have a CONTRIBUTING file that outlines:

- How to set up development environment
- Code style requirements
- Testing requirements
- How to submit changes

Example sections:

```markdown
## Development Setup

1. Fork and clone the repository
2. Install dependencies: `pip install -e .[dev]`
3. Run tests: `pytest`

## Code Style

- We use Black for formatting
- We use flake8 for linting
- We use mypy for type checking

## Testing

- All new code must include tests
- Run `pytest` before submitting
- Target 100% coverage for new code
```

## Common Requirements

### Type Hints

```python
def process_user(user_id: int, name: str) -> dict[str, Any]:
    """Process user data with type hints."""
    return {"id": user_id, "name": name}
```

### Docstrings

```python
def calculate_total(items: list[dict]) -> float:
    """Calculate the total price of items.
    
    Args:
        items: List of item dictionaries with 'price' key
        
    Returns:
        Total price as a float
    """
    return sum(item["price"] for item in items)
```

### Tests

```python
def test_calculate_total():
    """Test total calculation."""
    items = [{"price": 10.0}, {"price": 20.0}]
    assert calculate_total(items) == 30.0
```

## Summary

- Always read CONTRIBUTING.md first
- Follow code style guidelines
- Include tests with your changes

## Next Steps

Continue to `06-code-review-process.md`.
