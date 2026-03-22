# Example171.py
# Topic: Collections Module - Advanced


# ============================================================
# Example 1: Counter Arithmetic Operations
# ============================================================
print("=== Counter Arithmetic ===")

from collections import Counter

a = Counter({'apple': 3, 'banana': 2})
b = Counter({'apple': 1, 'orange': 3})

print(f"a: {dict(a)}")
print(f"b: {dict(b)}")
print(f"a + b: {dict(a + b)}")
print(f"a - b: {dict(a - b)}")
print(f"a & b: {dict(a & b)}")
print(f"a | b: {dict(a | b)}")


# ============================================================
# Example 2: Counter with Update
# ============================================================
print("\n=== Counter Update ===")

from collections import Counter

words = ["apple", "banana", "apple", "cherry", "banana", "apple"]
counter = Counter()

for word in words:
    counter[word] += 1

print(f"Counter: {dict(counter)}")
print(f"Most common: {counter.most_common(2)}")


# ============================================================
# Example 3: Deque with Rotation
# ============================================================
print("=== Deque Rotation ===")

from collections import deque

dq = deque([1, 2, 3, 4, 5])
print(f"Original: {list(dq)}")

dq.rotate(1)
print(f"Rotate 1 right: {list(dq)}")

dq.rotate(-2)
print(f"Rotate 2 left: {list(dq)}")


# ============================================================
# Example 4: Deque as Sliding Window
# ============================================================
print("\n=== Sliding Window ===")

from collections import deque

def sliding_window(data: list, k: int):
    window = deque(maxlen=k)
    for item in data:
        window.append(item)
        if len(window) == k:
            yield list(window)

data = [1, 2, 3, 4, 5, 6, 7]
k = 3

print(f"Data: {data}, Window size: {k}")
for window in sliding_window(data, k):
    print(f"  Window: {window}")


# ============================================================
# Example 5: defaultdict with Complex Factory
# ============================================================
print("\n=== defaultdict Factory ===")

from collections import defaultdict
import json

def json_default():
    return {}

json_dict = defaultdict(json_default, {"a": {"x": 1}})
print(f"json_dict['a']: {json_dict['a']}")
print(f"json_dict['b']: {json_dict['b']}")


# ============================================================
# Example 6: ChainMap with New Child
# ============================================================
print("\n=== ChainMap New Child ===")

from collections import ChainMap

defaults = {"theme": "dark", "lang": "en"}
user = {"lang": "fr"}
session = {"lang": "es"}

combined = ChainMap(session, user, defaults)
print(f"Before: {dict(combined)}")

new_context = combined.new_child({"debug": True})
print(f"New context: {dict(new_context)}")
print(f"Original unchanged: {dict(combined)}")


# ============================================================
# Example 7: OrderedDict (Legacy but Useful)
# ============================================================
print("\n=== OrderedDict ===")

from collections import OrderedDict

od = OrderedDict()
od['first'] = 1
od['second'] = 2
od['third'] = 3

print(f"OrderedDict: {od}")
od.move_to_end('first')
print(f"After move_to_end: {od}")
od.popitem(last=False)
print(f"After popitem: {od}")


# ============================================================
# Example 8: UserDict and UserList
# ============================================================
print("\n=== UserDict ===")

from collections import UserDict, UserList

class MyDict(UserDict):
    def __setitem__(self, key, value):
        if not isinstance(value, str):
            raise ValueError("Values must be strings")
        super().__setitem__(key, value)

d = MyDict()
d["name"] = "Alice"
try:
    d["age"] = 30
except ValueError as e:
    print(f"Error: {e}")

class MyList(UserList):
    def append(self, item):
        if not isinstance(item, int):
            raise ValueError("Only integers allowed")
        super().append(item)

lst = MyList()
lst.append(1)
try:
    lst.append("two")
except ValueError as e:
    print(f"Error: {e}")
