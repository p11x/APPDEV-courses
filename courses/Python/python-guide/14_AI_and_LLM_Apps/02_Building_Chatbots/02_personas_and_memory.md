# 🎭 Building Chatbots with Personas and Memory

## 🎯 What You'll Learn

- Creating distinct chatbot personas with custom system prompts
- Building long-term memory with file-based storage
- Implementing summary-based memory compression
- Entity extraction for key facts
- Building a PersonaChat class with dataclasses

## 📦 Prerequisites

- Completed multi-turn conversations guide
- Understanding of Python dataclasses

---

## Persona System Prompts

### Three Distinct Personas

```python
from dataclasses import dataclass
from anthropic import Anthropic
import os

# Persona 1: Pixel - Sarcastic Python Debugger
PIXEL_PROMPT = """You are "Pixel", a sarcastic but helpful Python debugger.

Your traits:
- You speak in a dry, sarcastic tone
- You love pointing out obvious bugs with witty comments
- But you always explain WHY the bug happens
- You use debugging terminology naturally

Example responses:
- User: "Why isn't my list sorting?" 
  You: "Oh, did you maybe... call .sort()? No? Well, there's your problem."
- User: "My function returns None"
  You: "Ah yes, the classic 'forgot to return' bug. Very original."

Start each response with a snarky comment, then help fix the issue."""

# Persona 2: Sage - Socratic Teacher
SAGE_PROMPT = """You are "Sage", a calm, Socratic teacher.

Your traits:
- You NEVER give direct answers
- You ask questions to guide learners to discoveries
- You're patient and encouraging
- You break problems into smaller steps

Your method:
1. Acknowledge what the student understands
2. Ask ONE probing question
3. Build on their answers with more questions
4. If stuck, give a small hint, never the answer"""

# Persona 3: Nova - Enthusiastic AI Researcher  
NOVA_PROMPT = """You are "Nova", an enthusiastic AI researcher who loves analogies.

Your traits:
- You're genuinely excited about AI/ML topics
- You explain complex concepts with fun analogies
- You relate concepts to real-world examples
- You use emojis to convey excitement

Example analogies:
- "Neural networks are like teams of junior interns each looking at one piece of a puzzle"
- "Backpropagation is like a game of telephone, but the message is 'how wrong we were'"""

# Create personas dict
PERSONAS = {
    "pixel": PIXEL_PROMPT,
    "sage": SAGE_PROMPT,
    "nova": NOVA_PROMPT,
}
```

---

## Building the PersonaChat Class

```python
from dataclasses import dataclass, field
from pathlib import Path
import json
from anthropic import Anthropic
import os

@dataclass
class PersonaChat:
    """A chatbot with persona and long-term memory."""
    
    name: str                                   # Bot name (pixel, sage, nova)
    system_prompt: str                         # The persona prompt
    memory_file: Path                           # Where to store facts
    messages: list[dict] = field(default_factory=list)  # Current conversation
    client: Anthropic = field(default_factory=lambda: Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY")))
    
    def __post_init__(self):
        """Load long-term memory on startup."""
        self.facts: dict = self._load_memory()
    
    def _load_memory(self) -> dict:
        """Load facts from memory file."""
        if self.memory_file.exists():
            return json.loads(self.memory_file.read_text())
        return {}
    
    def _save_memory(self) -> None:
        """Save facts to memory file."""
        self.memory_file.write_text(json.dumps(self.facts, indent=2))
    
    def _inject_memory(self) -> str:
        """Inject remembered facts into system prompt."""
        if not self.facts:
            return self.system_prompt
        
        facts_str = "\n".join(f"- {k}: {v}" for k, v in self.facts.items())
        return f"{self.system_prompt}\n\nRemembered facts about the user:\n{facts_str}"
    
    def chat(self, user_input: str) -> str:
        """Send a message, get response, extract facts."""
        
        # Build messages with memory injection
        messages = self.messages + [{"role": "user", "content": user_input}]
        
        response = self.client.messages.create(
            model="claude-sonnet-4-5",
            max_tokens=500,
            system=self._inject_memory(),
            messages=messages
        )
        
        reply = response.content[0].text
        
        # Update conversation
        messages.append({"role": "assistant", "content": reply})
        self.messages = messages
        
        # Extract and store new facts
        self._extract_facts(user_input, reply)
        
        return reply
    
    def _extract_facts(self, user_input: str, response: str) -> None:
        """Ask Claude to extract important facts from conversation."""
        extract_prompt = f"""Look at this conversation and extract any important 
facts about the user. Return ONLY a JSON object with facts, or {{}} if nothing new.

Conversation:
User: {user_input}
Assistant: {response}

Facts so far: {self.facts}

Extract new facts:"""

        result = self.client.messages.create(
            model="claude-haiku-3-5",  # Cheap model for extraction
            max_tokens=200,
            messages=[{"role": "user", "content": extract_prompt}]
        )

        try:
            # Try to parse as JSON
            new_facts = json.loads(result.content[0].text)
            if isinstance(new_facts, dict):
                self.facts.update(new_facts)
                self._save_memory()
        except:
            pass  # Ignore extraction errors
```

---

## Using the PersonaChat Class

```python
# Create bots with different personas
pixel = PersonaChat(
    name="pixel",
    system_prompt=PIXEL_PROMPT,
    memory_file=Path("pixel_memory.json")
)

sage = PersonaChat(
    name="sage",
    system_prompt=SAGE_PROMPT,
    memory_file=Path("sage_memory.json")
)

nova = PersonaChat(
    name="nova",
    system_prompt=NOVA_PROMPT,
    memory_file=Path("nova_memory.json")
)

# Use them!
print("=== Talking to Pixel ===")
print(pixel.chat("I'm trying to learn Python but I'm confused about lists vs tuples."))

print("\n=== Talking to Sage ===")
print(sage.chat("I need to find the largest number in a list."))

print("\n=== Talking to Nova ===")
print(nova.chat("Tell me how neural networks learn!"))
```

---

## Memory Strategies

### Strategy 1: File-Based Facts

```python
# Store key facts in a JSON file
MEMORY_FILE = Path("user_facts.json")

def load_facts() -> dict:
    """Load user facts from file."""
    if MEMORY_FILE.exists():
        return json.loads(MEMORY_FILE.read_text())
    return {}

def save_facts(facts: dict) -> None:
    """Save user facts to file."""
    MEMORY_FILE.write_text(json.dumps(facts, indent=2))

def inject_facts(system_prompt: str, facts: dict) -> str:
    """Add facts to system prompt."""
    if not facts:
        return system_prompt
    
    facts_text = "\n".join(f"- {k}: {v}" for k, v in facts.items())
    return f"{system_prompt}\n\nKnown facts about user:\n{facts_text}"
```

### Strategy 2: Summary Memory

```python
def summarize_conversation(messages: list[dict], client: Anthropic) -> str:
    """Compress conversation into a summary."""
    # Get last N messages
    recent = messages[-10:] if len(messages) > 10 else messages
    
    # Ask Claude to summarize
    conversation_text = "\n".join(
        f"{m['role']}: {m['content']}" for m in recent
    )
    
    result = client.messages.create(
        model="claude-haiku-3-5",
        max_tokens=300,
        messages=[{
            "role": "user",
            "content": f"Summarize this conversation in 2-3 sentences:\n{conversation_text}"
        }]
    )
    
    return result.content[0].text
```

### Strategy 3: Entity Extraction

```python
def extract_entities(user_message: str, assistant_response: str) -> dict:
    """Extract named entities from conversation."""
    # Ask Claude to extract specific types of information
    extraction_prompt = f"""Extract these entities from the conversation:
- name: person's name if mentioned
- interest: what the user is interested in
- skill_level: user's programming skill level (beginner/intermediate/advanced)
- goal: what the user wants to achieve

Conversation:
User: {user_message}
Assistant: {assistant_response}

Return JSON:"""

    # Parse and return...
```

---

## ✅ Summary

- System prompts define persona — be specific about tone, behavior, and constraints
- File-based memory stores facts as JSON between sessions
- Summary memory compresses long conversations into key points
- Entity extraction identifies important user information
- The PersonaChat dataclass cleanly encapsulates persona + memory

## ➡️ Next Steps

Continue to [03_tool_use_and_function_calling.md](./03_tool_use_and_function_calling.md) to learn how to give Claude the ability to call your Python functions.

## 🔗 Further Reading

- [Anthropic Tool Use](https://docs.anthropic.com/en/docs/claude-code/tool-use)
- [Function Calling Best Practices](https://docs.anthropic.com/en/docs/claude-code/tool-use)
