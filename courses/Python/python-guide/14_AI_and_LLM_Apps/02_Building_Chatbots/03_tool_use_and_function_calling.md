# 🔧 Tool Use and Function Calling

## 🎯 What You'll Learn

- What tool use is and how it works
- Defining tools with JSON Schema
- The tool use loop: request → execute → respond
- Building practical tools: weather, search, file reading
- Handling errors in tool results

## 📦 Prerequisites

- Completed multi-turn conversations
- Understanding of JSON Schema basics

---

## What is Tool Use?

Tool use lets Claude call YOUR Python functions. Here's how it works:

```
1. You define tools (functions) Claude can call
2. Claude decides WHEN to use them based on user request
3. Claude returns a "tool_use" block with the function and inputs
4. You execute the function
5. You send the result back to Claude
6. Claude provides the final response
```

---

## Defining Tools

### Tool Schema

```python
from anthropic import Anthropic
from typing import Any

client = Anthropic()

# Define tools as a list
tools = [
    {
        "name": "get_weather",
        "description": "Get the current weather for a city",
        "input_schema": {
            "type": "object",
            "properties": {
                "city": {
                    "type": "string",
                    "description": "The city name (e.g., 'San Francisco', 'London')"
                },
                "units": {
                    "type": "string",
                    "enum": ["celsius", "fahrenheit"],
                    "description": "Temperature units",
                    "default": "celsius"
                }
            },
            "required": ["city"]
        }
    },
    {
        "name": "search_web",
        "description": "Search the web for information",
        "input_schema": {
            "type": "object",
            "properties": {
                "query": {
                    "type": "string",
                    "description": "The search query"
                }
            },
            "required": ["query"]
        }
    }
]

# That's it! Tools are just dictionaries with name, description, and input_schema
```

### 💡 Line-by-Line Breakdown

```python
tools = [  # List of tool definitions
    {
        "name": "get_weather",           # Function name Claude will call
        "description": "Get weather...", # What the tool does - be specific!
        "input_schema": {                 # JSON Schema for arguments
            "type": "object",
            "properties": {              # Available parameters
                "city": {
                    "type": "string",
                    "description": "City name"
                }
            },
            "required": ["city"]          # Required parameters
        }
    }
]
```

---

## The Tool Use Loop

```python
import os
from anthropic import Anthropic

client = Anthropic(api_key=os.environ.get("ANTHROPIC_API_KEY"))

# Tool definitions
tools = [
    {
        "name": "get_weather",
        "description": "Get current weather for a city",
        "input_schema": {
            "type": "object",
            "properties": {
                "city": {"type": "string", "description": "City name"}
            },
            "required": ["city"]
        }
    }
]

# Implementations
def get_weather(city: str, units: str = "celsius") -> dict:
    """Get weather for a city (mock implementation)."""
    # In real code, call a weather API!
    return {
        "city": city,
        "temperature": 22 if units == "celsius" else 72,
        "conditions": "Partly cloudy",
        "units": units
    }

# Tool use loop
def chat_with_tools(user_message: str) -> str:
    """Chat with Claude, allowing tool calls."""
    
    messages = [{"role": "user", "content": user_message}]
    
    while True:
        # 1. Send message to Claude with tools available
        response = client.messages.create(
            model="claude-sonnet-4-5",
            max_tokens=1000,
            messages=messages,
            tools=tools
        )
        
        # 2. Check if Claude wants to use a tool
        if response.stop_reason == "tool_use":
            # Get the tool use block
            tool_use = response.content[0]
            tool_name = tool_use.name
            tool_input = tool_use.input
            
            # 3. Execute the tool
            if tool_name == "get_weather":
                result = get_weather(**tool_input)
            
            # 4. Add tool result to messages
            messages.append({
                "role": "assistant",
                "content": response.content
            })
            messages.append({
                "role": "user",
                "content": [{
                    "type": "tool_result",
                    "tool_use_id": tool_use.id,
                    "content": str(result)
                }]
            })
        
        else:
            # No tool use - return the response
            return response.content[0].text

# Test it!
print(chat_with_tools("What's the weather in Tokyo?"))
```

### 💡 Line-by-Line Breakdown

```python
# 1. Send message WITH tools available
response = client.messages.create(
    model="claude-sonnet-4-5",
    messages=messages,
    tools=tools  # ← Tell Claude what tools exist
)

# 2. Check if tool was used
if response.stop_reason == "tool_use":
    tool_use = response.content[0]  # Get tool call info
    tool_name = tool_use.name       # "get_weather"
    tool_input = tool_use.input     # {"city": "Tokyo"}

# 3. Execute your function
result = get_weather(**tool_input)

# 4. Send result back to Claude
messages.append({
    "role": "user",
    "content": [{
        "type": "tool_result",
        "tool_use_id": tool_use.id,  # Must include this!
        "content": str(result)         # Result as string
    }]
})

# Loop continues - Claude will now respond with final answer
```

---

## Practical Tools

### Tool 1: Read File

```python
from pathlib import Path

def read_file(path: str) -> str:
    """Read contents of a file."""
    try:
        file_path = Path(path).expanduser().resolve()
        
        # Security: prevent directory traversal
        if not str(file_path).startswith(str(Path.cwd())):
            return "Error: Access denied - must be in current directory"
        
        if not file_path.exists():
            return f"Error: File not found: {path}"
        
        # Limit file size (1MB)
        if file_path.stat().st_size > 1_000_000:
            return "Error: File too large (max 1MB)"
        
        return file_path.read_text(encoding="utf-8")
    
    except Exception as e:
        return f"Error reading file: {e}"

# Add to tools list
file_tools = [
    {
        "name": "read_file",
        "description": "Read contents of a text file. Use for reading code, config files, or any text content.",
        "input_schema": {
            "type": "object",
            "properties": {
                "path": {
                    "type": "string",
                    "description": "Path to the file to read"
                }
            },
            "required": ["path"]
        }
    }
]
```

### Tool 2: Run Python (Sandboxed!)

```python
import subprocess
import tempfile

def run_python(code: str) -> str:
    """Execute Python code in a sandboxed environment."""
    # ⚠️ SECURITY: This is dangerous! Only run in proper sandbox!
    # For production, use Docker containers or ephemeral environments
    
    try:
        with tempfile.NamedTemporaryFile(mode="w", suffix=".py", delete=False) as f:
            f.write(code)
            f.flush()
        
        result = subprocess.run(
            ["python", f.name],
            capture_output=True,
            text=True,
            timeout=10  # 10 second timeout
        )
        
        output = result.stdout if result.stdout else result.stderr
        return output[:5000]  # Limit output size
    
    except subprocess.TimeoutExpired:
        return "Error: Code execution timed out (10 seconds)"
    except Exception as e:
        return f"Error: {e}"

# Add to tools
python_tools = [
    {
        "name": "run_python",
        "description": "Execute Python code and return the output. Useful for calculations, testing code snippets, or running small programs.",
        "input_schema": {
            "type": "object",
            "properties": {
                "code": {
                    "type": "string",
                    "description": "Python code to execute"
                }
            },
            "required": ["code"]
        }
    }
]
```

### Tool 3: Search Web (Mock)

```python
def search_web(query: str) -> str:
    """Search the web (mock - replace with real API)."""
    # In production, use Bing API, Google Custom Search, or DuckDuckGo
    mock_results = {
        "python": "Python is a high-level programming language...",
        "ai": "Artificial intelligence is intelligence demonstrated by machines...",
        "claude": "Claude is an AI assistant created by Anthropic..."
    }
    
    query_lower = query.lower()
    for key, result in mock_results.items():
        if key in query_lower:
            return f"Search results for '{query}':\n\n{result}"
    
    return f"No mock results for '{query}'. Try searching for: python, ai, or claude."

# Add to tools
search_tools = [
    {
        "name": "search_web",
        "description": "Search the web for information. Use for current events, facts, or topics you want to look up.",
        "input_schema": {
            "type": "object",
            "properties": {
                "query": {
                    "type": "string",
                    "description": "Search query"
                }
            },
            "required": ["query"]
        }
    }
]
```

---

## Error Handling

```python
def safe_tool_execution(tool_name: str, tool_input: dict, tools: list) -> str:
    """Execute a tool with error handling."""
    
    # Find the tool
    tool_def = next((t for t in tools if t["name"] == tool_name), None)
    if not tool_def:
        return f"Error: Unknown tool '{tool_name}'"
    
    # Get the implementation
    implementations = {
        "get_weather": get_weather,
        "read_file": read_file,
        "run_python": run_python,
        "search_web": search_web,
    }
    
    func = implementations.get(tool_name)
    if not func:
        return f"Error: No implementation for '{tool_name}'"
    
    # Execute with error handling
    try:
        result = func(**tool_input)
        return str(result)
    except TypeError as e:
        return f"Error: Invalid arguments - {e}"
    except Exception as e:
        return f"Error: {type(e).__name__}: {e}"
```

---

## ✅ Summary

- Tool use lets Claude call your Python functions
- Define tools with name, description, and JSON Schema input_schema
- The tool use loop: create → check stop_reason → execute → send result
- Always implement error handling in tool results
- Security: sandbox any code execution, limit file access

## ➡️ Next Steps

Continue to [../03_LLM_App_Patterns/01_prompt_engineering.md](../03_LLM_App_Patterns/01_prompt_engineering.md) to learn advanced prompt engineering techniques.

## 🔗 Further Reading

- [Tool Use Documentation](https://docs.anthropic.com/en/docs/claude-code/tool-use)
- [JSON Schema](https://json-schema.org/)
