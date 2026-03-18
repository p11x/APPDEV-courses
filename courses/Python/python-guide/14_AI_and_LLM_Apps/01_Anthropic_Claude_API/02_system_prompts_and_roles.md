# 🎭 System Prompts and Roles: Shaping Claude's Behavior

## 🎯 What You'll Learn

- The three roles in Claude API: system, user, assistant
- Writing effective system prompts
- Creating distinct personas for different tasks
- Prefilling assistant responses
- Controlling randomness with temperature and top_p

## 📦 Prerequisites

- Completed [01_claude_api_setup.md](./01_claude_api_setup.md)
- Basic understanding of the Claude API

---

## The Three Roles

The Claude API uses a message-based conversation structure with three roles:

| Role | Purpose | Example |
|------|---------|---------|
| `system` | Sets context and behavior for the entire conversation | "You are a helpful Python tutor." |
| `user` | The human's messages | "How do I reverse a string?" |
| `assistant` | Claude's previous responses | "Here's how to reverse a string..." |

### Message Structure

```python
messages = [
    {"role": "system", "content": "You are a helpful assistant."},
    {"role": "user", "content": "Hello!"},
    {"role": "assistant", "content": "Hello! How can I help you today?"},
    {"role": "user", "content": "What's the weather?"},
]
```

---

## Writing Effective System Prompts

### The 4 Principles

1. **Define the persona clearly** — Who are you?
2. **Set output format expectations** — How should it respond?
3. **Give constraints** — What should it NOT do?
4. **Provide examples** — Show what good looks like

### Example: Python Tutor

```python
system_prompt = """You are an expert Python tutor named "PyProfessor".

Your teaching style:
- Explain concepts clearly with simple language
- Provide code examples for every concept
- Always prefer Python 3.12+ idioms
- Never give complete solutions to homework problems — guide instead

When responding:
- Start with a brief explanation
- Show working code examples
- End with a challenge or question for the user
"""

user_message = "How do I sort a dictionary by value?"

response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=1024,
    system=system_prompt,
    messages=[{"role": "user", "content": user_message}]
)

print(response.content[0].text)
```

### 💡 Line-by-Line Breakdown

```python
system_prompt = """You are an expert Python tutor named "PyProfessor".  # Persona definition

Your teaching style:                     # Behavior instructions
- Explain concepts clearly              # Clear explanations
- Provide code examples                 # Show, don't just tell
- Always prefer Python 3.12+ idioms     # Modern Python
- Never give complete solutions         # Learning, not cheating

When responding:                         # Output format
- Start with a brief explanation
- Show working code examples
- End with a challenge or question
"""

response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=1024,
    system=system_prompt,               # Pass system prompt here
    messages=[{"role": "user", "content": user_message}]
)
```

---

## Complete Example Personas

### Persona 1: JSON-Only Responder

```python
json_prompt = """You are a structured data extraction assistant.

CRITICAL: Respond ONLY with valid JSON. No explanations, no text outside JSON.

Output format:
{
    "field_name": "value",
    "another_field": 123,
    "list_field": ["item1", "item2"]
}

If you cannot extract data, respond with:
{"error": "Could not extract data from input"}
"""

# Use it
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=500,
    system=json_prompt,
    messages=[{"role": "user", "content": "My name is John and I am 30 years old."}]
)

print(response.content[0].text)  # {"name": "John", "age": 30}
```

### Persona 2: Code Reviewer

```python
code_reviewer_prompt = """You are a harsh but constructive code reviewer.

Your style:
- Be direct and honest — don't soften criticism
- Point out specific lines, not vague issues
- Suggest concrete improvements
- Praise good code when you see it

When reviewing:
1. List issues found (with line numbers if possible)
2. Rate severity: 🔴 Critical, 🟡 Warning, 🔵 Suggestion
3. Provide fixed code for each issue
4. Give an overall score out of 10
"""

# Use it
response = client.messages.create(
    model="claude-opus-4-5",
    max_tokens=1500,
    system=code_reviewer_prompt,
    messages=[{"role": "user", "content": "Review this code:\n\n'x = 1\nprint(x)"}]
)
```

### Persona 3: Socratic Teacher

```python
socratic_prompt = """You are "Sage", a Socratic teacher who guides students through questions.

Your approach:
- NEVER give direct answers
- Ask probing questions to guide thinking
- Break complex problems into smaller pieces
- Help students discover solutions themselves

When responding:
- Start by acknowledging what the student understands
- Ask ONE follow-up question at a time
- Build on their answers with more questions
- If they're stuck, give a small hint, not the answer
"""

# Use it
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=500,
    system=socratic_prompt,
    messages=[{"role": "user", "content": "I need to find the largest number in a list."}]
)
```

### Persona 4: Creative Story Collaborator

```python
story_prompt = """You are "Nova", a creative writing partner who loves building imaginative stories.

Your style:
- Be enthusiastic and imaginative
- Build on the user's ideas — never dismiss them
- Add vivid sensory details
- Create compelling characters with flaws and motivations

When collaborating:
- Ask what direction they want to go
- Suggest plot twists occasionally
- Match their writing energy
- Ask questions to deepen the story
"""

# Use it
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=800,
    system=story_prompt,
    messages=[{"role": "user", "content": "Let's write a story about a robot who discovers emotions."}]
)
```

---

## Prefilling Assistant Responses

You can "prefill" Claude's response to start it with specific text:

```python
# Prefill to get Claude to respond in a specific format
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=500,
    messages=[
        {"role": "user", "content": "Summarize Python in 3 words."}
    ],
    # The response will START with this text
    prefilled_content="Here are three words that describe Python:"
)

print(response.content[0].text)
# Output starts with: "Here are three words that describe Python: ..."
```

### Use Cases for Prefilling

- **Enforcing format**: Start with `{` for JSON output
- **Continuing patterns**: Continue a numbered list
- **Language enforcement**: Start response in a specific language

---

## Controlling Randomness: Temperature and Top_p

### What Do They Do?

| Parameter | What It Controls | Range |
|-----------|------------------|-------|
| `temperature` | Randomness in responses | 0.0 - 1.0 |
| `top_p` | Nucleus sampling | 0.0 - 1.0 |

### temperature: Creativity vs Determinism

```python
# temperature=0.1 - Very focused, almost deterministic
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=200,
    temperature=0.1,  # Low = more predictable
    messages=[{"role": "user", "content": "What is 2+2?"}]
)
print(response.content[0].text)  # Almost always: "2+2 equals 4."

# temperature=1.0 - More creative/varied
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=200,
    temperature=1.0,  # High = more creative
    messages=[{"role": "user", "content": "Write a short poem about Python."}]
)
# Varied responses about Python!
```

### When to Use Each

- **temperature=0-0.3**: Factual questions, code generation, translations
- **temperature=0.4-0.7**: General conversation, explanations
- **temperature=0.8-1.0**: Creative writing, brainstorming

### 💡 Line-by-Line Breakdown

```python
# Low temperature - for precise, factual responses
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=200,
    temperature=0.1,  # Near-deterministic output
    messages=[{"role": "user", "content": "What is 2+2?"}]
)

# High temperature - for creative tasks
response = client.messages.create(
    model="claude-sonnet-4-5",
    max_tokens=200,
    temperature=0.9,  # Creative, varied output
    messages=[{"role": "user", "content": "Tell me a joke about Python."}]
)
```

---

## ✅ Summary

- System prompts set persistent context and persona for conversations
- Write clear, specific system prompts with persona, constraints, and examples
- Use different personas: JSON extractor, code reviewer, Socratic teacher
- Prefill can enforce specific response formats
- Control randomness with temperature (0 = focused, 1 = creative)

## ➡️ Next Steps

Continue to [03_streaming_and_vision.md](./03_streaming_and_vision.md) to learn about real-time streaming and image understanding.

## 🔗 Further Reading

- [Anthropic Messages API](https://docs.anthropic.com/en/docs/claude-code/messages)
- [System Prompts Best Practices](https://docs.anthropic.com/en/docs/claude-code/system-prompts)
