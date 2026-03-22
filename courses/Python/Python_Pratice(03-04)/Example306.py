# Example306: Heapq Deep Dive
import heapq

# Basic heap
print("Heapq Basics:")
heap = [3, 1, 4, 1, 5, 9]
heapq.heapify(heap)
print(f"Heapified: {heap}")

# Push and pop
heapq.heappush(heap, 2)
print(f"After push: {heap}")
print(f"Popped: {heapq.heappop(heap)}")

# nlargest and nsmallest
print("\nN-largest and smallest:")
data = [1, 4, 2, 8, 5, 9, 3]
print(f"3 largest: {heapq.nlargest(3, data)}")
print(f"3 smallest: {heapq.nsmallest(3, data)}")

# Merge sorted
print("\nMerge:")
list1 = [1, 3, 5]
list2 = [2, 4, 6]
merged = list(heapq.merge(list1, list2))
print(f"Merged: {merged}")
