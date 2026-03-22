# Example181.py
# Topic: Stack Implementation

# This file demonstrates stack data structure implementation.
# Stack is LIFO (Last In, First Out) - like a stack of plates.
# Operations: push, pop, peek, is_empty.


# ============================================================
# Example 1: Basic Stack with List
# ============================================================
print("=== Basic Stack with List ===")

# Stack using list - append/pop from end
stack = []    # list — stack implementation

stack.append(1)    # Push 1
stack.append(2)    # Push 2
stack.append(3)    # Push 3
print(f"After pushes: {stack}")    # [1, 2, 3]

top = stack.pop()    # int — pop top element
print(f"Popped: {top}")    # Popped: 3
print(f"After pop: {stack}")    # After pop: [1, 2]


# ============================================================
# Example 2: Stack Class Implementation
# ============================================================
print("\n=== Stack Class ===")

class Stack:
    def __init__(self):
        self._items = []
    
    def push(self, item):
        self._items.append(item)
    
    def pop(self):
        if self.is_empty():
            raise IndexError("pop from empty stack")
        return self._items.pop()
    
    def peek(self):
        if self.is_empty():
            raise IndexError("peek from empty stack")
        return self._items[-1]
    
    def is_empty(self) -> bool:
        return len(self._items) == 0
    
    def size(self) -> int:
        return len(self._items)
    
    def __repr__(self):
        return f"Stack({self._items})"

s = Stack()
s.push(10)
s.push(20)
s.push(30)

print(f"Stack: {s}")    # Stack([10, 20, 30])
print(f"Peek: {s.peek()}")    # Peek: 30
print(f"Size: {s.size()}")    # Size: 3

s.pop()    # Remove top
print(f"After pop: {s}")    # Stack([10, 20])


# ============================================================
# Example 3: Balanced Parentheses
# ============================================================
print("\n=== Balanced Parentheses ===")

def is_balanced(expression: str) -> bool:
    stack = []
    pairs = {")": "(", "]": "[", "}": "{"}
    
    for char in expression:
        if char in "([{":
            stack.append(char)
        elif char in ")]}":
            if not stack or stack[-1] != pairs[char]:
                return False
            stack.pop()
    
    return len(stack) == 0

tests = ["()", "()[]{}", "(]", "([)]", "{[]}"]
for test in tests:
    result = is_balanced(test)    # bool — balanced or not
    print(f"{test}: {result}")    # True/False


# ============================================================
# Example 4: Reverse String with Stack
# ============================================================
print("\n=== Reverse String ===")

def reverse_string(s: str) -> str:
    stack = list(s)
    result = []
    
    while stack:
        result.append(stack.pop())
    
    return "".join(result)

text = "Hello, World!"
reversed_text = reverse_string(text)    # str — reversed
print(f"Original: {text}")    # Original: Hello, World!
print(f"Reversed: {reversed_text}")    # Reversed: !dlroW ,olleH


# ============================================================
# Example 5: Stack for Expression Evaluation
# ============================================================
print("\n=== Postfix Evaluation ===")

def evaluate_postfix(expression: str) -> int:
    stack = []
    tokens = expression.split()
    
    for token in tokens:
        if token.isdigit():
            stack.append(int(token))
        else:
            b = stack.pop()
            a = stack.pop()
            if token == "+":
                stack.append(a + b)
            elif token == "-":
                stack.append(a - b)
            elif token == "*":
                stack.append(a * b)
            elif token == "/":
                stack.append(a // b)
    
    return stack[0]

expr = "3 4 + 2 *"
result = evaluate_postfix(expr)    # int — evaluated result
print(f"{expr} = {result}")    # 14


# ============================================================
# Example 6: Undo Mechanism with Stack
# ============================================================
print("\n=== Undo Mechanism ===")

class TextEditor:
    def __init__(self):
        self._text = ""
        self._history = Stack()
    
    def write(self, text: str):
        self._history.push(self._text)
        self._text += text
    
    def undo(self) -> bool:
        if self._history.is_empty():
            return False
        self._text = self._history.pop()
        return True
    
    def __repr__(self):
        return f'Text: "{self._text}"'

editor = TextEditor()
editor.write("Hello")
print(f"After 'Hello': {editor}")    # Text: "Hello"
editor.write(" World")
print(f"After ' World': {editor}")    # Text: "Hello World"
editor.undo()
print(f"After undo: {editor}")    # Text: "Hello"
editor.undo()
print(f"After undo: {editor}")    # Text: ""


# ============================================================
# Example 7: Function Call Stack Simulation
# ============================================================
print("\n=== Call Stack Simulation ===")

class CallStack:
    def __init__(self):
        self._frames = Stack()
    
    def push_frame(self, function_name: str):
        self._frames.push(function_name)
        print(f"Calling: {function_name}")
    
    def pop_frame(self):
        func = self._frames.pop()
        print(f"Returning from: {func}")
    
    def __repr__(self):
        return str(self._frames._items)

call_stack = CallStack()
call_stack.push_frame("main")
call_stack.push_frame("calculate")
call_stack.push_frame("process")
print(f"Stack: {call_stack}")
call_stack.pop_frame()
call_stack.pop_frame()
call_stack.pop_frame()


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
STACK (LIFO):
- push(item): Add to top
- pop(): Remove from top
- peek(): View top without removing
- is_empty(): Check if empty

USES:
- Undo/redo functionality
- Expression evaluation
- Backtracking algorithms
- Function call management
- Balanced parentheses checking

COMPLEXITY:
- push/pop/peek: O(1)
- space: O(n) for n elements
""")
