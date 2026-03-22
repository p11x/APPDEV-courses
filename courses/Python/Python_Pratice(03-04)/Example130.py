# Example130.py
# Topic: heapq and Priority Queues


# ============================================================
# Example 1: Basic heapq Usage
# ============================================================
print("=== heapq Basics ===")

import heapq

heap = [3, 1, 4, 1, 5, 9, 2, 6]
heapq.heapify(heap)
print(f"Heap: {heap}")

heapq.heappush(heap, 0)
print(f"After push 0: {heap}")

smallest = heapq.heappop(heap)
print(f"Popped: {smallest}")
print(f"Remaining: {heap}")


# ============================================================
# Example 2: Get N Smallest/Largest
# ============================================================
print("\n=== N Smallest/Largest ===")

import heapq

data = [5, 2, 8, 1, 9, 3, 7, 4]
print(f"Data: {data}")

print(f"3 smallest: {heapq.nsmallest(3, data)}")
print(f"3 largest: {heapq.nlargest(3, data)}")


# ============================================================
# Example 3: Priority Queue with Tuples
# ============================================================
print("\n=== Priority Queue ===")

import heapq

pq = []
heapq.heappush(pq, (2, "task B"))
heapq.heappush(pq, (1, "task A"))
heapq.heappush(pq, (3, "task C"))

while pq:
    priority, task = heapq.heappop(pq)
    print(f"Priority {priority}: {task}")


# ============================================================
# Example 4: Max Heap (negate values)
# ============================================================
print("\n=== Max Heap ===")

import heapq

data = [5, 2, 8, 1, 9]
max_heap = [-x for x in data]
heapq.heapify(max_heap)

print("Largest 3:")
for _ in range(min(3, len(max_heap))):
    print(f"  {-heapq.heappop(max_heap)}")


# ============================================================
# Example 5: Merge Sorted Sequences
# ============================================================
print("\n=== Merge Sorted ===")

import heapq

list1 = [1, 4, 7, 10]
list2 = [2, 5, 8, 11]
list3 = [3, 6, 9, 12]

merged = list(heapq.merge(list1, list2, list3))
print(f"Merged: {merged}")


# ============================================================
# Example 6: Heap Sort
# ============================================================
print("\n=== Heap Sort ===")

import heapq

def heap_sort(arr):
    heapq.heapify(arr)
    return [heapq.heappop(arr) for _ in range(len(arr))]

data = [5, 2, 8, 1, 9, 3, 7, 4]
print(f"Original: {data}")
print(f"Heap sort: {heap_sort(data[:])}")


# ============================================================
# Example 7: Real-World: Top K Products
# ============================================================
print("\n=== Real-World: Top K Products ===")

import heapq

products = [
    {"name": "Laptop", "sales": 150},
    {"name": "Phone", "sales": 300},
    {"name": "Tablet", "sales": 100},
    {"name": "Watch", "sales": 200},
    {"name": "Headphones", "sales": 80},
]

top_k = heapq.nlargest(3, products, key=lambda p: p["sales"])
print("Top 3 products:")
for p in top_k:
    print(f"  {p['name']}: {p['sales']} sales")
