# Example243: Deque - Advanced Patterns
from collections import deque

# Max deque size - maintain fixed size
print("Fixed-size deque (keep last N):")
recent = deque(maxlen=3)
recent.append(1)
recent.append(2)
recent.append(3)
print(f"After 1,2,3: {list(recent)}")
recent.append(4)
print(f"After 4: {list(recent)}")
recent.append(5)
print(f"After 5: {list(recent)}")

# Rotate
print("\nRotate:")
d = deque([1, 2, 3, 4, 5])
print(f"Original: {list(d)}")
d.rotate(1)
print(f"Rotate 1: {list(d)}")
d.rotate(-2)
print(f"Rotate -2: {list(d)}")

# Practical: sliding window
print("\nSliding window maximum:")
def sliding_max(data, k):
    result = []
    window = deque()
    for i, num in enumerate(data):
        # Remove indices out of window
        while window and window[0] <= i - k:
            window.popleft()
        # Remove smaller indices from back
        while window and data[window[-1]] <= num:
            window.pop()
        window.append(i)
        if i >= k - 1:
            result.append(data[window[0]])
    return result

data = [1, 3, 2, 6, 4, 3]
print(f"Data: {data}, k=3")
print(f"Max of each window: {sliding_max(data, 3)}")

# Palindrome check with deque
print("\nPalindrome check:")
def is_palindrome(s):
    d = deque(s)
    while len(d) > 1:
        if d.popleft() != d.pop():
            return False
    return True

words = ["radar", "hello", "level", "world"]
for word in words:
    print(f"  {word}: {is_palindrome(word)}")

# Moving average
print("\nMoving average:")
def moving_average(data, k):
    result = []
    window = deque(maxlen=k)
    for num in data:
        window.append(num)
        if len(window) == k:
            result.append(sum(window) / k)
    return result

data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
print(f"Data: {data}, window=3")
print(f"Moving avg: {moving_average(data, 3)}")
