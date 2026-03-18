# Debugging Tools

## What You'll Learn

- Debugging with print statements (the basics)
- Using the Python debugger (pdb)
- IDE debugging (VS Code, PyCharm)
- Logging best practices
- Debugging web applications
- Error tracking tools

## Prerequisites

- Completed `06-environment-managers.md`

## The Debugging Mindset

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DEBUGGING PROCESS                                         │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  1. REPRODUCE:                                                             │
│  Can you make the bug happen consistently?                                │
│                                                                             │
│  2. LOCATE:                                                                │
│  Where does the bug occur?                                                 │
│                                                                             │
│  3. HYPOTHESIZE:                                                            │
│  Why does it happen?                                                       │
│                                                                             │
│  4. TEST:                                                                  │
│  Verify your hypothesis                                                    │
│                                                                             │
│  5. FIX:                                                                   │
│  Make the change                                                           │
│                                                                             │
│  6. VERIFY:                                                                │
│  Does the bug still happen?                                                │
│                                                                             │
│  KEY SKILL: Reading error messages and stack traces                       │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Print Debugging

The simplest approach:

```python
def calculate_total(items: list[dict]) -> float:
    print(f"Input: {items}")  # Debug: see what we received
    
    total = 0
    for item in items:
        print(f"Processing item: {item}")  # Debug: each iteration
        price = item.get("price", 0)
        quantity = item.get("quantity", 1)
        print(f"Price: {price}, Quantity: {quantity}")  # Debug
        line_total = price * quantity
        print(f"Line total: {line_total}")  # Debug
        total += line_total
    
    print(f"Final total: {total}")  # Debug
    return total
```

🔍 **What this does:**
- Shows the flow of data
- Lets you see variable values
- Simple but effective

## The Python Debugger (pdb)

Interactive debugging:

```python
import pdb

def calculate_total(items: list[dict]) -> float:
    pdb.set_trace()  # Stop here!
    
    total = 0
    for item in items:
        price = item.get("price", 0)
        quantity = item.get("quantity", 1)
        total += price * quantity
    
    return total
```

### pdb Commands

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PDB COMMANDS                                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  n (next)        → Execute next line                                       │
│  s (step)        → Step into function                                      │
│  c (continue)    → Continue until next breakpoint                         │
│  p variable      → Print variable value                                    │
│  pp variable     → Pretty print                                            │
│  l (list)        → Show code around current line                          │
│  w (where)       → Show stack trace                                        │
│  u (up)          → Move up stack                                           │
│  d (down)        → Move down stack                                         │
│  q (quit)        → Quit debugger                                           │
│  b line#         → Set breakpoint                                          │
│  cl line#        → Clear breakpoint                                        │
│                                                                             │
│  Interactive example:                                                       │
│  > /app.py(10)calculate_total()                                            │
│  -> total = 0                                                              │
│  (Pdb) p items                                                             │
│  [{'price': 10, 'quantity': 2}]                                           │
│  (Pdb) n                                                                   │
│  > /app.py(12)calculate_total()                                            │
│  (Pdb)                                                                     │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Modern pdb: pdb++

```bash
# Install pdb++
pip install pdbpp

# Now you get:
# - Syntax highlighting
# - Tab completion
# - Better UI
```

## IDE Debugging (VS Code)

VS Code has excellent debugging:

### Setting Up

```json
// .vscode/launch.json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Python: Current File",
            "type": "python",
            "request": "launch",
            "program": "${file}",
            "console": "integratedTerminal"
        },
        {
            "name": "Python: FastAPI",
            "type": "python",
            "request": "launch",
            "module": "uvicorn",
            "args": ["main:app"],
            "jinja": true,
            "justMyCode": true
        }
    ]
}
```

### Using the Debugger

1. Set breakpoint (click in margin or `F9`)
2. Start debug (`F5`)
3. Use Debug toolbar:
   - Continue (`F5`)
   - Step Over (`F10`)
   - Step Into (`F11`)
   - Step Out (`Shift+F11`)
   - Stop (`Shift+F5`)

### Debug Panel

The Debug panel shows:
- Variables (local and global)
- Watch (custom expressions)
- Call Stack
- Breakpoints

## Logging

Better than print for production:

```python
import logging

# Configure logging
logging.basicConfig(
    level=logging.DEBUG,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)

logger = logging.getLogger(__name__)

def calculate_total(items: list[dict]) -> float:
    logger.debug(f"Input: {items}")
    
    total = 0
    for item in items:
        price = item.get("price", 0)
        quantity = item.get("quantity", 1)
        logger.debug(f"Item: price={price}, qty={quantity}")
        total += price * quantity
    
    logger.debug(f"Total: {total}")
    return total
```

### Logging Levels

```python
logger.debug("Detailed debug info")
logger.info("General info")
logger.warning("Warning")
logger.error("Error")
logger.critical("Critical failure")
```

## Debugging Web Applications

### Flask Debugging

```python
# Enable debug mode
app.run(debug=True)

# Then you get:
# - Interactive debugger on errors
# - Auto-reload on code changes
# - Detailed error pages
```

### FastAPI Debugging

```python
# Use uvicorn with reload
# Command line:
# uvicorn main:app --reload

# In code:
if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", reload=True)
```

### Debugging with curl

```bash
# Test endpoint and see response
curl -v http://localhost:8000/api/endpoint

# With headers
curl -v -H "Authorization: Bearer token" http://localhost:8000/api/
```

## Error Tracking

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    ERROR TRACKING SERVICES                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  SENTRY:                                                                    │
│  • Most popular                                                             │
│  • Python SDK: sentry-sdk                                                  │
│  • Tracks exceptions, provides context                                     │
│                                                                             │
│  INSTALLATION:                                                             │
│  pip install sentry-sdk                                                    │
│                                                                             │
│  USAGE:                                                                    │
│  import sentry_sdk                                                         │
│  sentry_sdk.init(dsn="your-dsn")                                           │
│                                                                             │
│  BENEFITS:                                                                  │
│  • Stack traces with local variables                                       │
│  • User context                                                            │
│  • Release tracking                                                        │
│  • Alerting                                                                │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

## Summary

- Print debugging is simple but limited
- pdb provides interactive debugging
- IDE debuggers are most powerful
- Use logging for production debugging
- Use Sentry for production error tracking

## Next Steps

→ Continue to `08-productivity-tools.md` to learn about tools that boost your productivity.
