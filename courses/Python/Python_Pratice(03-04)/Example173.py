# Example173.py
# Topic: Advanced Data Structure Patterns


# ============================================================
# Example 1: LRU Cache Implementation
# ============================================================
print("=== LRU Cache ===")

from collections import OrderedDict
from typing import Any, Optional

class LRUCache:
    def __init__(self, capacity: int):
        self.cache = OrderedDict()
        self.capacity = capacity
    
    def get(self, key: str) -> Optional[Any]:
        if key not in self.cache:
            return None
        self.cache.move_to_end(key)
        return self.cache[key]
    
    def put(self, key: str, value: Any) -> None:
        if key in self.cache:
            self.cache.move_to_end(key)
        self.cache[key] = value
        if len(self.cache) > self.capacity:
            self.cache.popitem(last=False)
    
    def __repr__(self):
        return f"LRUCache({dict(self.cache)})"

cache = LRUCache(3)
cache.put("a", 1)
cache.put("b", 2)
cache.put("c", 3)
print(f"Initial: {cache}")

cache.get("a")
print(f"After get a: {cache}")

cache.put("d", 4)
print(f"After put d: {cache}")

cache.put("b", 20)
print(f"After put b: {cache}")


# ============================================================
# Example 2: Thread-Safe Counter
# ============================================================
print("\n=== Thread-Safe Counter ===")

from collections import Counter
from threading import Lock

class ThreadSafeCounter:
    def __init__(self):
        self._counter = Counter()
        self._lock = Lock()
    
    def increment(self, key: str, value: int = 1) -> None:
        with self._lock:
            self._counter[key] += value
    
    def decrement(self, key: str, value: int = 1) -> None:
        with self._lock:
            self._counter[key] -= value
    
    def get(self, key: str) -> int:
        with self._lock:
            return self._counter[key]
    
    def most_common(self, n: int = None):
        with self._lock:
            return self._counter.most_common(n)

counter = ThreadSafeCounter()
counter.increment("requests")
counter.increment("requests")
counter.increment("errors")
counter.increment("requests")

print(f"Most common: {counter.most_common()}")


# ============================================================
# Example 3: Bijective Mapping
# ============================================================
print("\n=== Bijective Mapping ===")

from collections import defaultdict

class BijectiveDict:
    def __init__(self):
        self._forward = {}
        self._backward = {}
    
    def __setitem__(self, key, value):
        if key in self._backward:
            del self._backward[self._forward[key]]
        if value in self._forward:
            del self._forward[self._backward[value]]
        
        self._forward[key] = value
        self._backward[value] = key
    
    def __getitem__(self, key):
        return self._forward[key]
    
    def get_forward(self, key, default=None):
        return self._forward.get(key, default)
    
    def get_backward(self, value, default=None):
        return self._backward.get(value, default)

bio = BijectiveDict()
bio["a"] = 1
bio["b"] = 2
bio["c"] = 3

print(f"Forward: {bio._forward}")
print(f"Backward: {bio._backward}")
print(f"Get a: {bio.get_forward('a')}")
print(f"Get 2: {bio.get_backward(2)}")


# ============================================================
# Example 4: MultiDict
# ============================================================
print("\n=== MultiDict ===")

from collections import defaultdict
from typing import List, Any

class MultiDict:
    def __init__(self):
        self._data = defaultdict(list)
    
    def add(self, key: str, value: Any) -> None:
        self._data[key].append(value)
    
    def get(self, key: str, default: Any = None) -> Any:
        values = self._data.get(key, [])
        return values[0] if values else default
    
    def get_all(self, key: str) -> List[Any]:
        return self._data.get(key, [])
    
    def __repr__(self):
        return dict(self._data)

md = MultiDict()
md.add("server", "server1.example.com")
md.add("server", "server2.example.com")
md.add("port", "8080")
md.add("port", "3000")

print(f"MultiDict: {md}")
print(f"Get server: {md.get('server')}")
print(f"Get all server: {md.get_all('server')}")


# ============================================================
# Example 5: Sparse Matrix with Dict
# ============================================================
print("\n=== Sparse Matrix ===")

class SparseMatrix:
    def __init__(self, rows: int, cols: int):
        self.rows = rows
        self.cols = cols
        self._data = {}
    
    def __setitem__(self, key, value):
        row, col = key
        if value != 0:
            self._data[(row, col)] = value
        else:
            self._data.pop((row, col), None)
    
    def __getitem__(self, key):
        row, col = key
        return self._data.get((row, col), 0)
    
    def __repr__(self):
        result = []
        for i in range(self.rows):
            row = [str(self[(i, j)]) for j in range(self.cols)]
            result.append(" ".join(row))
        return "\n".join(result)

matrix = SparseMatrix(3, 3)
matrix[(0, 0)] = 1
matrix[(1, 1)] = 5
matrix[(2, 2)] = 9
matrix[(0, 2)] = 3

print("Sparse Matrix:")
print(matrix)


# ============================================================
# Example 6: Ring Buffer
# ============================================================
print("\n=== Ring Buffer ===")

class RingBuffer:
    def __init__(self, capacity: int):
        self.capacity = capacity
        self.buffer = [None] * capacity
        self.head = 0
        self.tail = 0
        self.size = 0
    
    def append(self, item):
        self.buffer[self.tail] = item
        self.tail = (self.tail + 1) % self.capacity
        if self.size < self.capacity:
            self.size += 1
        else:
            self.head = (self.head + 1) % self.capacity
    
    def get_all(self):
        if self.size == 0:
            return []
        if self.tail > self.head:
            return self.buffer[self.head:self.tail]
        return self.buffer[self.head:] + self.buffer[:self.tail]

rb = RingBuffer(5)
for i in range(7):
    rb.append(i)
    print(f"After append {i}: {rb.get_all()}")


# ============================================================
# Example 7: Bloom Filter
# ============================================================
print("\n=== Bloom Filter ===")

import hashlib

class BloomFilter:
    def __init__(self, size: int = 100):
        self.size = size
        self.bit_array = [False] * size
        self.count = 0
    
    def _hashes(self, item: str):
        for i in range(3):
            hash_val = int(hashlib.md5(f"{item}{i}".encode()).hexdigest(), 16)
            yield hash_val % self.size
    
    def add(self, item: str):
        for idx in self._hashes(item):
            self.bit_array[idx] = True
        self.count += 1
    
    def contains(self, item: str) -> bool:
        return all(self.bit_array[idx] for idx in self._hashes(item))

bf = BloomFilter()
words = ["hello", "world", "python", "filter"]

for word in words:
    bf.add(word)

test_words = ["hello", "python", "missing", "world"]
print("Bloom filter check:")
for word in test_words:
    result = "Probably present" if bf.contains(word) else "Not present"
    print(f"  {word}: {result}")


# ============================================================
# Example 8: Skip List Basics
# ============================================================
print("\n=== Simple Skip List ===")

import random

class Node:
    def __init__(self, value, level=0):
        self.value = value
        self.forward = [None] * (level + 1)

class SkipList:
    def __init__(self):
        self.header = Node(None, 10)
        self.level = 0
    
    def search(self, target):
        current = self.header
        for i in range(self.level, -1, -1):
            while current.forward[i] and current.forward[i].value < target:
                current = current.forward[i]
        current = current.forward[0]
        if current and current.value == target:
            return True
        return False
    
    def insert(self, value):
        update = [None] * (self.level + 1)
        current = self.header
        
        for i in range(self.level, -1, -1):
            while current.forward[i] and current.forward[i].value < value:
                current = current.forward[i]
            update[i] = current
        
        current = current.forward[0]
        if current is None or current.value != value:
            new_level = random.randint(0, self.level + 1)
            if new_level > self.level:
                for i in range(self.level + 1, new_level + 1):
                    update.append(self.header)
                    self.level = i
            node = Node(value, new_level)
            for i in range(new_level + 1):
                node.forward[i] = update[i].forward[i]
                update[i].forward[i] = node

sl = SkipList()
for i in [3, 6, 7, 9, 12, 19]:
    sl.insert(i)

print("Search tests:")
print(f"  7: {sl.search(7)}")
print(f"  10: {sl.search(10)}")
print(f"  19: {sl.search(19)}")
