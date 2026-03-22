# Example127.py
# Topic: Collections Module Deep Dive


# ============================================================
# Example 1: deque - Double-Ended Queue
# ============================================================
print("=== deque ===")

from collections import deque

dq = deque([1, 2, 3])
print(f"Initial: {dq}")

dq.appendleft(0)
print(f"After appendleft: {dq}")

dq.append(4)
print(f"After append: {dq}")

dq.popleft()
print(f"After popleft: {dq}")

dq.pop()
print(f"After pop: {dq}")


# ============================================================
# Example 2: deque - Max Length (Sliding Window)
# ============================================================
print("\n=== deque with maxlen ===")

from collections import deque

window = deque(maxlen=3)
for i in [1, 2, 3, 4, 5]:
    window.append(i)
    print(f"After {i}: {list(window)}")


# ============================================================
# Example 3: Counter - Basic Usage
# ============================================================
print("\n=== Counter ===")

from collections import Counter

text = "hello world"
c = Counter(text)
print(f"Counter: {c}")
print(f"Letters: {dict(c)}")


# ============================================================
# Example 4: Counter - Most Common
# ============================================================
print("\n=== Counter most_common ===")

from collections import Counter

words = ["apple", "banana", "apple", "cherry", "banana", "apple"]
c = Counter(words)
print(f"Counts: {c}")
print(f"Most common 2: {c.most_common(2)}")


# ============================================================
# Example 5: Counter - Arithmetic
# ============================================================
print("\n=== Counter arithmetic ===")

from collections import Counter

c1 = Counter(a=3, b=1, c=0)
c2 = Counter(a=1, b=2, d=3)

print(f"c1 + c2: {c1 + c2}")
print(f"c1 - c2: {c1 - c2}")
print(f"c1 & c2: {c1 & c2}")
print(f"c1 | c2: {c1 | c2}")


# ============================================================
# Example 6: ChainMap - Multiple Dictionaries
# ============================================================
print("\n=== ChainMap ===")

from collections import ChainMap

defaults = {"theme": "dark", "lang": "en"}
user_prefs = {"lang": "fr"}
session = {"lang": "es"}

combined = ChainMap(session, user_prefs, defaults)
print(f"theme: {combined['theme']}")
print(f"lang: {combined['lang']}")


# ============================================================
# Example 7: OrderedDict (Legacy)
# ============================================================
print("\n=== OrderedDict ===")

from collections import OrderedDict

od = OrderedDict()
od["first"] = 1
od["second"] = 2
od["third"] = 3

print(f"OrderedDict: {od}")
print(f"Keys: {list(od.keys())}")


# ============================================================
# Example 8: Real-World: Task Queue with deque
# ============================================================
print("\n=== Real-World: Task Queue ===")

from collections import deque
from time import sleep

task_queue = deque()

task_queue.append({"task": "email", "priority": 1})
task_queue.append({"task": "report", "priority": 2})
task_queue.appendleft({"task": "urgent", "priority": 0})

print("Processing tasks:")
while task_queue:
    task = task_queue.popleft()
    print(f"  - {task['task']} (priority: {task['priority']})")
