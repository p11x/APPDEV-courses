# Example246: Heapq - More Advanced Patterns
import heapq

# Find k smallest/largest
print("Heapq for k smallest/largest:")
data = [5, 2, 8, 1, 9, 3, 7, 4, 6]
print(f"Data: {data}")
print(f"3 smallest: {heapq.nsmallest(3, data)}")
print(f"3 largest: {heapq.nlargest(3, data)}")

# Merge sorted iterables
print("\nMerge sorted iterables:")
list1 = [1, 4, 7, 10]
list2 = [2, 5, 8, 11]
list3 = [3, 6, 9, 12]
merged = list(heapq.merge(list1, list2, list3))
print(f"Merged: {merged}")

# Heap replace and push
print("\nHeap replace vs push:")
heap = [1, 2, 3]
heapq.heapify(heap)
print(f"Initial: {heap}")
# heappushpop - push new item then pop smallest
result = heapq.heappushpop(heap, 0)
print(f"heappushpop(0): popped={result}, heap={heap}")
# heapreplace - pop smallest then push new item
heap = [1, 2, 3]
heapq.heapify(heap)
result = heapq.heapreplace(heap, 0)
print(f"heapreplace(0): popped={result}, heap={heap}")

# Two-heap approach for median
print("\nTwo-heap for running median:")
class MedianFinder:
    def __init__(self):
        self.small = []  # max heap (negated)
        self.large = []  # min heap
    
    def add_num(self, num):
        heapq.heappush(self.small, -num)
        heapq.heappush(self.large, -heapq.heappop(self.small))
        if len(self.small) < len(self.large):
            heapq.heappush(self.small, -heapq.heappop(self.large))
    
    def find_median(self):
        if len(self.small) > len(self.large):
            return -self.small[0]
        return (-self.small[0] + self.large[0]) / 2

mf = MedianFinder()
for num in [5, 15, 1, 3]:
    mf.add_num(num)
    print(f"Added {num}, median: {mf.find_median()}")

# Find median of streams
print("\nRunning median:")
mf = MedianFinder()
numbers = [12, 4, 5, 3, 8, 7]
for n in numbers:
    mf.add_num(n)
    print(f"Median after {n}: {mf.find_median()}")
