# Example263: Binary Indexed Tree (Fenwick Tree)
class FenwickTree:
    def __init__(self, n):
        self.n = n
        self.tree = [0] * (n + 1)
    
    def update(self, i, delta):
        while i <= self.n:
            self.tree[i] += delta
            i += i & (-i)
    
    def query(self, i):
        res = 0
        while i > 0:
            res += self.tree[i]
            i -= i & (-i)
        return res
    
    def range_sum(self, l, r):
        return self.query(r) - self.query(l - 1)

print("Fenwick Tree:")
ft = FenwickTree(6)
arr = [0, 1, 3, 5, 7, 9, 11]
for i in range(1, len(arr)):
    ft.update(i, arr[i])

print(f"Array (1-indexed): {arr[1:]}")
print(f"Prefix sum [1,3]: {ft.query(3)}")
print(f"Range sum [2,5]: {ft.range_sum(2, 5)}")

# Count inversions with Fenwick
def count_inversions_ft(arr):
    """Count inversions using Fenwick tree."""
    ft = FenwickTree(max(arr) + 1)
    inv = 0
    for x in reversed(arr):
        inv += ft.query(x - 1)
        ft.update(x, 1)
    return inv

print("\nCount inversions:")
arr = [8, 4, 2, 1]
print(f"Array: {arr}")
print(f"Inversions: {count_inversions_ft(arr)}")

# Find k-th smallest
def kth_smallest(arr, k):
    """Find k-th smallest using Fenwick."""
    ft = FenwickTree(max(arr) + 1)
    for x in arr:
        ft.update(x, 1)
    for i in range(1, ft.n + 1):
        if ft.query(i) >= k:
            return i
    return -1

print("\nK-th smallest:")
arr = [3, 1, 4, 1, 5, 9, 2, 6]
print(f"Array: {arr}")
print(f"3rd smallest: {kth_smallest(arr, 3)}")

# Frequency counter with Fenwick
class FrequencyCounter:
    def __init__(self, max_val):
        self.ft = FenwickTree(max_val)
    
    def add(self, val):
        self.ft.update(val, 1)
    
    def remove(self, val):
        self.ft.update(val, -1)
    
    def count_less_equal(self, val):
        return self.ft.query(val)

print("\nFrequency counter:")
fc = FrequencyCounter(10)
for x in [3, 3, 2, 5, 2, 1]:
    fc.add(x)
print(f"Count <= 3: {fc.count_less_equal(3)}")
fc.remove(2)
print(f"After remove, Count <= 3: {fc.count_less_equal(3)}")
