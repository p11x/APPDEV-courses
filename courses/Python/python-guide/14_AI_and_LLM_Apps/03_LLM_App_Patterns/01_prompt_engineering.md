# 🎨 The Art of Prompt Engineering

## 🎯 What You'll Learn

- The 6 core principles of effective prompting
- Zero-shot vs one-shot vs few-shot prompting
- Building reusable prompt templates
- Common anti-patterns and how to fix them

## 📦 Prerequisites

- Completed Claude API basics
- Understanding of conversation structure

---

## The 6 Principles of Effective Prompting

### 1. Be Specific

```python
# ❌ Vague prompt - vague results
prompt = "Tell me about Python."

# ✅ Specific prompt - targeted results
prompt = """Explain Python's list comprehension syntax to a beginner.
Cover:
- Basic syntax
- Why it's useful vs for loops
- One simple example
Keep it under 100 words."""
```

### 2. Give Context

```python
# ❌ No context
prompt = "Write a function."

# ✅ Rich context
prompt = """Write a Python function to calculate factorial.
Target audience: beginner Python developer
Use: recursion (not iteration)
Include: type hints and docstring
Avoid: using math.factorial"""
```

### 3. Specify Format

```python
# ❌ No format specified
prompt = "List some Python tips."

# ✅ Explicit format
prompt = """List 5 Python tips for beginners.
Format: numbered list
Each tip: one line with brief explanation
Style: conversational but informative"""
```

### 4. Show Examples (Few-Shot)

```python
# ❌ No examples
prompt = "Convert this to uppercase: hello"

# ✅ With examples
prompt = """Convert text to uppercase. Examples:
- "hello" → "HELLO"
- "World" → "WORLD"
- "Python" → "PYTHON"

Now convert: "machine learning"
"""
```

### 5. Chain of Thought

```python
# ❌ Direct answer request
prompt = "What is 234 * 567?"

# ✅ Ask for reasoning
prompt = """Solve this step by step, showing your work:
What is 234 * 567?"""
```

### 6. Set Constraints

```python
# ❌ No constraints
prompt = "Write about Python."

# ✅ Clear constraints
prompt = """Write about Python in exactly 50 words.
- Use simple language (avoid jargon)
- Include one code example
- Do not mention JavaScript"""
```

---

## Zero-Shot vs One-Shot vs Few-Shot

### Zero-Shot

```python
# No examples - Claude figures it out from the prompt
response = client.messages.create(
    model="claude-sonnet-4-5",
    messages=[{
        "role": "user",
        "content": "Classify this sentiment: 'I love this product!'"
    }]
)
# Claude guesses: positive sentiment
```

### One-Shot

```python
# One example
response = client.messages.create(
    model="claude-sonnet-4-5",
    messages=[{
        "role": "user",
        "content": """Classify sentiment as positive, negative, or neutral.

Example:
Text: 'This is terrible' → negative

Now classify: 'I love this product!'"""
    }]
)
```

### Few-Shot

```python
# Multiple examples
response = client.messages.create(
    model="claude-sonnet-4-5",
    messages=[{
        "role": "user",
        "content": """Classify sentiment as positive, negative, or neutral.

Examples:
- 'This is amazing!' → positive
- 'This is terrible' → negative  
- 'It is okay' → neutral

Now classify:
1. 'I love this product!'
2. 'Very disappointed'
3. 'Does the job'"""
    }]
)
```

---

## Building Prompt Templates

```python
from dataclasses import dataclass
from string import Template

@dataclass
class PromptTemplate:
    """A reusable prompt template."""
    
    template: str
    description: str
    
    def render(self, **kwargs) -> str:
        """Fill in template variables."""
        return Template(self.template).substitute(**kwargs)

# Create templates
EXTRACT_JSON = PromptTemplate(
    template="""Extract the following fields from the text below:
$fields

Text:
$text

Return as JSON:""",
    description="Extract structured data from text"
)

SUMMARIZE = PromptTemplate(
    template="""Summarize the following in $style style:
- Maximum $max_words words
- Include key points: $key_points

Text:
$text""",
    description="Summarize text with constraints"
)

# Use them
prompt = EXTRACT_JSON.render(
    fields="name, email, phone",
    text="Contact John at john@example.com or 555-1234"
)

prompt2 = SUMMARIZE.render(
    style="casual",
    max_words=50,
    key_points="main argument, conclusion",
    text="..."  # Long text
)
```

---

## Common Anti-Patterns

### Anti-Pattern 1: Conflicting Instructions

```python
# ❌ Conflicting - don't do this!
prompt = """Be very concise. Also, provide extensive detail."""

# ✅ Fixed - pick one approach
prompt = """Be concise, but complete enough to be useful."""
```

### Anti-Pattern 2: Ambiguous Role

```python
# ❌ Unclear who Claude should be
prompt = "Explain neural networks."

# ✅ Clear persona
prompt = """As a patient teacher, explain neural networks 
to someone with no math background. Use analogies."""
```

### Anti-Pattern 3: Missing Output Format

```python
# ❌ What format should the answer be in?
prompt = "What are the benefits of Python?"

# ✅ Explicit format
prompt = """List 5 benefits of Python.
Format: numbered list with brief (one sentence) explanations."""
```

### Anti-Pattern 4: Too Many Tasks at Once

```python
# ❌ Multiple unrelated tasks
prompt = "Explain Python, write a haiku about coding, and recommend books."

# ✅ One task per prompt (or clearly separated)
prompt = """Complete these tasks:
1. Explain Python in one sentence
2. Write a haiku about coding
3. Recommend one beginner book

Separate each with ---"""
```

---

## ✅ Summary

- Be specific: vague in = vague out
- Give context: who, what, why, for whom
- Specify format: JSON, bullet points, etc.
- Show examples: few-shot beats zero-shot
- Chain of thought: "think step by step"
- Set constraints: word limits, topics to avoid

## ➡️ Next Steps

Continue to [02_structured_outputs.md](./02_structured_outputs.md) to learn how to get Claude to return clean, parseable JSON.

## 🔗 Further Reading

- [Anthropic Prompt Engineering Guide](https://docs.anthropic.com/en/docs/claude-code/prompt-engineering)
- [OpenAI Prompt Engineering Guide](https://platform.openai.com/docs/guides/prompt-engineering)
