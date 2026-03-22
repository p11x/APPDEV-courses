# Example204.py
# Topic: Stack Implementation & Applications

# This file demonstrates stack data structure implementation and applications
# including balanced parentheses, expression evaluation, and undo mechanisms.


# ============================================================
# Example 1: Basic Stack with List
# ============================================================
print("=== Basic Stack ===")

stack = []
stack.append(1)
stack.append(2)
stack.append(3)
print(f"Push 1,2,3: {stack}")

top = stack.pop()
print(f"Pop: {top}")    # 3
print(f"After pop: {stack}")    # [1, 2]


# ============================================================
# Example 2: Stack Class
# ============================================================
print("\n=== Stack Class ===")

class Stack:
    def __init__(self):
        self._items = []
    
    def push(self, item):
        self._items.append(item)
    
    def pop(self):
        return self._items.pop() if self._items else None
    
    def peek(self):
        return self._items[-1] if self._items else None
    
    def is_empty(self):
        return len(self._items) == 0

s = Stack()
s.push(10)
s.push(20)
print(f"Peek: {s.peek()}")    # 20
print(f"Pop: {s.pop()}")    # 20


# ============================================================
# Example 3: Balanced Parentheses
# ============================================================
print("\n=== Balanced Parentheses ===")

def is_balanced(s):
    stack = []
    pairs = {")": "(", "]": "[", "}": "{"}
    
    for char in s:
        if char in "([{":
            stack.append(char)
        elif char in ")]}":
            if not stack or stack[-1] != pairs[char]:
                return False
            stack.pop()
    
    return len(stack) == 0

tests = ["()", "()[]{}", "(]", "([)]", "{[]}"]
for t in tests:
    print(f"{t}: {is_balanced(t)}")


# ============================================================
# Example 4: Reverse String
# ============================================================
print("\n=== Reverse String ===")

def reverse_string(s):
    stack = list(s)
    result = []
    while stack:
        result.append(stack.pop())
    return "".join(result)

print(f"Reverse: {reverse_string('hello')}")    # "olleh"


# ============================================================
# Example 5: Postfix Evaluation
# ============================================================
print("\n=== Postfix Evaluation ===")

def eval_postfix(expr):
    stack = []
    for token in expr.split():
        if token.isdigit():
            stack.append(int(token))
        else:
            b = stack.pop()
            a = stack.pop()
            if token == "+": stack.append(a + b)
            elif token == "-": stack.append(a - b)
            elif token == "*": stack.append(a * b)
            elif token == "/": stack.append(a // b)
    return stack[0]

print(f"3 4 + = {eval_postfix('3 4 +')}")    # 7


# ============================================================
# Example 6: Undo Mechanism
# ============================================================
print("\n=== Undo Mechanism ===")

class TextEditor:
    def __init__(self):
        self._text = ""
        self._history = Stack()
    
    def write(self, text):
        self._history.push(self._text)
        self._text += text
    
    def undo(self):
        if not self._history.is_empty():
            self._text = self._history.pop()

editor = TextEditor()
editor.write("Hello")
editor.write(" World")
print(f"Text: {editor._text}")    # "Hello World"
editor.undo()
print(f"After undo: {editor._text}")    # "Hello"


# ============================================================
# Example 7: Function Call Stack
# ============================================================
print("\n=== Function Call Stack ===")

def fibonacci(n, call_stack=[]):
    call_stack.append(n)
    if n <= 1:
        call_stack.pop()
        return n
    result = fibonacci(n-1, call_stack) + fibonacci(n-2, call_stack)
    call_stack.pop()
    return result

print(f"Fib(5): {fibonacci(5)}")


# ============================================================
# Example 8: Stock Span Problem
# ============================================================
print("\n=== Stock Span ===")

def stock_span(prices):
    stack = []
    spans = []
    
    for price in prices:
        span = 1
        while stack and stack[-1][0] <= price:
            span += stack.pop()[1]
        stack.append((price, span))
        spans.append(span)
    
    return spans

prices = [100, 80, 60, 70, 60, 85, 100]
print(f"Spans: {stock_span(prices)}")


# ============================================================
# Example 9: Infix to Postfix
# ============================================================
print("\n=== Infix to Postfix ===")

def infix_to_postfix(expr):
    prec = {"+": 1, "-": 1, "*": 2, "/": 2}
    stack = []
    result = []
    
    for token in expr.split():
        if token.isalnum():
            result.append(token)
        elif token in "+-*/":
            while stack and stack[-1] in prec and prec[stack[-1]] >= prec[token]:
                result.append(stack.pop())
            stack.append(token)
        elif token == "(":
            stack.append(token)
        elif token == ")":
            while stack and stack[-1] != "(":
                result.append(stack.pop())
            stack.pop()
    
    while stack:
        result.append(stack.pop())
    
    return " ".join(result)

print(f"Infix to Postfix: {infix_to_postfix('a + b * c')}")


# ============================================================
# Example 10: Backtracking
# ============================================================
print("\n=== Backtracking ===")

def solve_maze(maze, start, end, path=[]):
    path = path + [start]
    if start == end:
        return path
    for neighbor in get_neighbors(maze, start):
        if neighbor not in path:
            result = solve_maze(maze, neighbor, end, path)
            if result:
                return result
    return None

def get_neighbors(maze, pos):
    return [(pos[0]+1, pos[1]), (pos[0], pos[1]+1)]

print("Backtracking available")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
STACK (LIFO):
- push: Add to top
- pop: Remove from top
- peek: View top
- O(1) operations

USE CASES:
- Balanced parentheses
- Expression evaluation
- Undo mechanisms
- Backtracking
""")
