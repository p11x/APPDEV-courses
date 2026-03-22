# Example120.py
# Topic: Real-World Data Structure Examples

# Practical examples using multiple data structures.


# ============================================================
# Example 1: Student Gradebook
# ============================================================
print("=== Student Gradebook ===")

from collections import defaultdict, Counter

class Gradebook:
    def __init__(self):
        self.students = {}  # name -> grades
    
    def add_grade(self, name, subject, grade):
        if name not in self.students:
            self.students[name] = defaultdict(list)
        self.students[name][subject].append(grade)
    
    def get_average(self, name):
        if name not in self.students:
            return 0
        all_grades = []
        for subject, grades in self.students[name].items():
            all_grades.extend(grades)
        return sum(all_grades) / len(all_grades) if all_grades else 0

gb = Gradebook()
gb.add_grade("Alice", "Math", 95)
gb.add_grade("Alice", "Math", 88)
gb.add_grade("Alice", "Science", 92)
gb.add_grade("Bob", "Math", 78)

print(f"Alice average: {gb.get_average('Alice'):.1f}")


# ============================================================
# Example 2: Inventory System
# ============================================================
print("\n=== Inventory ===")

from collections import defaultdict

class Inventory:
    def __init__(self):
        self.stock = defaultdict(int)
    
    def add(self, item, qty):
        self.stock[item] += qty
    
    def remove(self, item, qty):
        self.stock[item] -= qty
    
    def get(self, item):
        return self.stock[item]
    
    def low_stock(self, threshold=10):
        return {k: v for k, v in self.stock.items() if v <= threshold}

inv = Inventory()
inv.add("Apple", 50)
inv.add("Banana", 30)
inv.add("Orange", 5)

print(f"Apple: {inv.get('Apple')}")
print(f"Low stock: {inv.low_stock(20)}")


# ============================================================
# Example 3: Text Analysis
# ============================================================
print("\n=== Text Analysis ===")

from collections import Counter

def analyze_text(text):
    words = text.lower().split()
    
    return {
        "total_words": len(words),
        "unique_words": len(set(words)),
        "word_count": Counter(words),
        "most_common": Counter(words).most_common(3)
    }

text = "the quick brown fox jumps over the lazy dog the fox"
result = analyze_text(text)

print(f"Total: {result['total_words']}")
print(f"Unique: {result['unique_words']}")
print(f"Most common: {result['most_common']}")


# ============================================================
# Example 4: Graph Adjacency List
# ============================================================
print("\n=== Graph ===")

from collections import defaultdict

class Graph:
    def __init__(self):
        self.edges = defaultdict(list)
    
    def add_edge(self, u, v):
        self.edges[u].append(v)
    
    def neighbors(self, u):
        return self.edges[u]

g = Graph()
g.add_edge("A", "B")
g.add_edge("A", "C")
g.add_edge("B", "C")
g.add_edge("C", "A")

print(f"A neighbors: {g.neighbors('A')}")
print(f"C neighbors: {g.neighbors('C')}")


# ============================================================
# Example 5: Cache with LRU
# ============================================================
print("\n=== LRU Cache ===")

from collections import OrderedDict

class LRUCache:
    def __init__(self, capacity):
        self.capacity = capacity
        self.cache = OrderedDict()
    
    def get(self, key):
        if key in self.cache:
            self.cache.move_to_end(key)
            return self.cache[key]
        return None
    
    def put(self, key, value):
        if key in self.cache:
            self.cache.move_to_end(key)
        self.cache[key] = value
        if len(self.cache) > self.capacity:
            self.cache.popitem(last=False)

cache = LRUCache(3)
cache.put("A", 1)
cache.put("B", 2)
cache.put("C", 3)
cache.put("D", 4)

print(f"Cache: {dict(cache.cache)}")
