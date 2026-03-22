# Example219.py
# Topic: Stack Applications

# This file demonstrates more stack applications.


# ============================================================
# Example 1: Evaluate Reverse Polish
# ============================================================
print("=== RPN Evaluation ===")

def eval_rpn(tokens):
    stack = []
    for t in tokens:
        if t.lstrip('-').isdigit():
            stack.append(int(t))
        else:
            b = stack.pop()
            a = stack.pop()
            if t == '+': stack.append(a + b)
            elif t == '-': stack.append(a - b)
            elif t == '*': stack.append(a * b)
            elif t == '/': stack.append(int(a / b))
    return stack[0]

print(f"3 4 +: {eval_rpn(['3', '4', '+'])}")
print(f"10 2 - 3 *: {eval_rpn(['10', '2', '-', '3', '*'])}")


# ============================================================
# Example 2: Valid Parentheses
# ============================================================
print("\n=== Valid Parentheses ===")

def is_valid(s):
    stack = []
    pairs = {')': '(', ']': '[', '}': '{'}
    for c in s:
        if c in '([{':
            stack.append(c)
        elif c in ')]}':
            if not stack or stack[-1] != pairs[c]:
                return False
            stack.pop()
    return not stack

print(f"'()': {is_valid('()')}")
print(f"'([)]': {is_valid('([)]')}")


# ============================================================
# Example 3: Simplify Path
# ============================================================
print("\n=== Simplify Path ===")

def simplify_path(path):
    stack = []
    for part in path.split('/'):
        if part == '..':
            if stack: stack.pop()
        elif part and part != '.':
            stack.append(part)
    return '/' + '/'.join(stack)

print(f"/home/:/../root: {simplify_path('/home/user/Documents')}")


# ============================================================
# Example 4: Make Valid String
# ============================================================
print("\n=== Make Valid ===")

def make_valid(s):
    stack = []
    for c in s:
        if c == '#':
            if stack: stack.pop()
        else:
            stack.append(c)
    return ''.join(stack)

print(f"abc##d: {make_valid('abc##d')}")


# ============================================================
# Example 5: Decode String
# ============================================================
print("\n=== Decode String ===")

def decode(s):
    stack = []
    for c in s:
        if c == ']':
            sub = ''
            while stack and stack[-1] != '[':
                sub = stack.pop() + sub
            stack.pop()
            num = ''
            while stack and stack[-1].isdigit():
                num = stack.pop() + num
            stack.append(int(num) * sub)
        elif c.isdigit() or c == '[' or c.isalpha():
            stack.append(c)
    return ''.join(stack)

print(f"3[a]2[bc]: {decode('3[a]2[bc]')}")


# ============================================================
# Example 6: Remove Adjacent Duplicates
# ============================================================
print("\n=== Remove Duplicates ===")

def remove_duplicates(s):
    stack = []
    for c in s:
        if stack and stack[-1] == c:
            stack.pop()
        else:
            stack.append(c)
    return ''.join(stack)

print(f"aabbc: {remove_duplicates('aabbc')}")


# ============================================================
# Example 7: Next Greater Element
# ============================================================
print("\n=== Next Greater ===")

def next_greater(arr):
    stack = []
    result = [-1] * len(arr)
    for i in range(len(arr)):
        while stack and arr[stack[-1]] < arr[i]:
            result[stack.pop()] = arr[i]
        stack.append(i)
    return result

arr = [1, 3, 2, 4]
print(f"Next greater: {next_greater(arr)}")


# ============================================================
# Example 8: Daily Temperatures
# ============================================================
print("\n=== Daily Temperatures ===")

def daily_temps(t):
    stack = []
    result = [0] * len(t)
    for i in range(len(t)):
        while stack and t[stack[-1]] < t[i]:
            result[stack.pop()] = i - stack[-1] if stack else i
        stack.append(i)
    return result

t = [73, 74, 75, 71, 69]
print(f"Days: {daily_temps(t)}")


# ============================================================
# Example 9: Asteroid Collision
# ============================================================
print("\n=== Asteroid ===")

def asteroid(asteroids):
    stack = []
    for a in asteroids:
        while stack and a < 0 < stack[-1]:
            if abs(a) > stack[-1]:
                stack.pop()
                continue
            elif abs(a) == stack[-1]:
                stack.pop()
            break
        else:
            stack.append(a)
    return stack

print(f"[5,10,-5]: {asteroid([5,10,-5])}")


# ============================================================
# Example 10: Largest Rectangle
# ============================================================
print("\n=== Largest Rectangle ===")

def largest_rect(heights):
    stack = []
    max_area = 0
    for i, h in enumerate(heights):
        start = i
        while stack and stack[-1][1] > h:
            idx, height = stack.pop()
            max_area = max(max_area, height * (i - idx))
            start = idx
        stack.append((start, h))
    for idx, h in stack:
        max_area = max(max_area, h * (len(heights) - idx))
    return max_area

print(f"[2,1,5,6,2,3]: {largest_rect([2,1,5,6,2,3])}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
STACK APPLICATIONS:
- Expression evaluation
- Parentheses matching
- Next greater element
- Daily temperatures
- Largest rectangle
""")
