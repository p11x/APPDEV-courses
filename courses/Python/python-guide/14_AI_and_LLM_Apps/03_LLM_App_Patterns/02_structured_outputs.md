# 📋 Getting Structured Outputs from Claude

## 🎯 What You'll Learn

- Why structured output matters for programming
- Methods for getting clean JSON from Claude
- Using Pydantic for validation
- Building robust extraction functions

## 📦 Prerequisites

- Understanding of prompt engineering basics

---

## Why Structured Output?

```python
# Instead of parsing this:
response = "Sure! The user asked about Python. Based on the text, I can extract..."

# You want this:
{
    "name": "John",
    "email": "john@example.com",
    "sentiment": "positive"
}
```

---

## Method 1: Prompt for JSON

```python
import json

def extract_json(client, prompt: str) -> dict:
    """Extract JSON from Claude's response."""
    
    response = client.messages.create(
        model="claude-sonnet-4-5",
        max_tokens=1000,
        messages=[{
            "role": "user",
            "content": f"""{prompt}

Respond ONLY with valid JSON. No explanations."""
        }]
    )
    
    text = response.content[0].text.strip()
    
    # Remove markdown fences if present
    if text.startswith("```json"):
        text = text[7:]
    if text.startswith("```"):
        text = text[3:]
    if text.endswith("```"):
        text = text[:-3]
    
    return json.loads(text.strip())

# Use it
result = extract_json(client, "Extract name and age from: John is 25 years old.")
print(result)  # {"name": "John", "age": "25"}
```

---

## Method 2: Pydantic Validation

```python
from pydantic import BaseModel
from typing import Optional

class Person(BaseModel):
    """A person extracted from text."""
    name: str
    age: int
    email: Optional[str] = None

def extract_person(client, text: str) -> Person:
    """Extract person info with Pydantic validation."""
    
    prompt = f"""Extract person information from this text:
{text}

Return as JSON with fields: name (string), age (integer), email (string or null)"""
    
    response = client.messages.create(
        model="claude-sonnet-4-5",
        max_tokens=500,
        messages=[{"role": "user", "content": prompt}]
    )
    
    text = response.content[0].text.strip()
    # Clean up markdown
    text = text.strip("```json").strip("```").strip()
    
    return Person.model_validate_json(text)

# Use it
person = extract_person(client, "John Smith is 30 years old, email: john@example.com")
print(person.name)  # John Smith
print(person.email)  # john@example.com
```

---

## Robust Extraction Function

```python
from pydantic import BaseModel, ValidationError
from typing import Type
import json
import re

def extract_structured(
    client,
    text: str,
    model: Type[BaseModel],
    prompt_hint: str = None,
    max_retries: int = 2
) -> BaseModel:
    """Extract structured data with automatic retry."""
    
    prompt = f"""Extract from this text:
{text}

{prompt_hint or 'Return as JSON matching this schema: ' + str(model.model_json_schema())}

Respond ONLY with valid JSON. No markdown, no explanation."""
    
    for attempt in range(max_retries):
        try:
            response = client.messages.create(
                model="claude-sonnet-4-5",
                max_tokens=1000,
                messages=[{"role": "user", "content": prompt}]
            )
            
            raw = response.content[0].text
            
            # Clean up
            cleaned = re.sub(r'^```json', '', raw)
            cleaned = re.sub(r'^```', '', cleaned)
            cleaned = re.sub(r'```$', '', cleaned)
            cleaned = cleaned.strip()
            
            return model.model_validate_json(cleaned)
        
        except (json.JSONDecodeError, ValidationError) as e:
            if attempt == max_retries - 1:
                raise ValueError(f"Failed to parse: {e}")
    
    raise ValueError("Max retries exceeded")
```

---

## Real Examples

### Example 1: Invoice Extraction

```python
from pydantic import BaseModel
from datetime import date
from typing import List

class InvoiceItem(BaseModel):
    description: str
    quantity: int
    unit_price: float

class Invoice(BaseModel):
    vendor: str
    date: date
    total: float
    items: List[InvoiceItem]

invoice_text = """
From: Acme Corp
Date: 2024-01-15
Items:
- Widgets: 10 @ $5.00 each
- Gadgets: 5 @ $10.00 each
Total: $100.00
"""

invoice = extract_structured(client, invoice_text, Invoice)
print(invoice.vendor)  # Acme Corp
print(invoice.total)   # 100.0
```

### Example 2: Feedback Classification

```python
from pydantic import BaseModel
from enum import Enum

class Sentiment(str, Enum):
    POSITIVE = "positive"
    NEGATIVE = "negative"
    NEUTRAL = "neutral"

class Category(str, Enum):
    BUG = "bug"
    FEATURE = "feature"
    QUESTION = "question"
    OTHER = "other"

class Feedback(BaseModel):
    sentiment: Sentiment
    category: Category
    summary: str
    priority: int  # 1-5

feedback = extract_structured(
    client,
    "The app crashes when I try to upload a file. Very frustrating!",
    Feedback
)
print(feedback.sentiment)  # negative
print(feedback.category)   # bug
print(feedback.priority)   # 4
```

---

## ✅ Summary

- Always request JSON explicitly in prompts
- Use Pydantic for automatic validation
- Handle parsing errors with retry logic
- Clean up markdown from responses before parsing

## ➡️ Next Steps

Continue to [03_rag_basics.md](./03_rag_basics.md) to learn about Retrieval-Augmented Generation.

## 🔗 Further Reading

- [Pydantic](https://docs.pydantic.dev/)
- [JSON Schema](https://json-schema.org/)
