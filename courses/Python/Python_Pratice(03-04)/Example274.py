# Example274: More Practice with Common Patterns
from collections import Counter, defaultdict, deque

# Frequency counter pattern
def most_frequent(nums, k):
    """Find k most frequent elements."""
    freq = Counter(nums)
    return [item for item, count in freq.most_common(k)]

print("Most Frequent Elements:")
nums = [1, 1, 1, 2, 2, 3, 3, 3, 3, 4]
print(f"Array: {nums}")
print(f"Top 2: {most_frequent(nums, 2)}")

# Group by frequency
def group_by_frequency(arr):
    """Group elements by their frequency."""
    freq = Counter(arr)
    groups = defaultdict(list)
    for num, count in freq.items():
        groups[count].append(num)
    return dict(groups)

print("\nGroup by Frequency:")
arr = [1, 1, 1, 2, 2, 3, 4, 4, 4]
print(f"Array: {arr}")
print(f"Groups: {group_by_frequency(arr)}")

# Running median with heaps
import heapq

class RunningMedian:
    def __init__(self):
        self.small = []
        self.large = []
    
    def add(self, num):
        heapq.heappush(self.small, -num)
        heapq.heappush(self.large, -heapq.heappop(self.small))
        if len(self.small) < len(self.large):
            heapq.heappush(self.small, -heapq.heappop(self.large))
    
    def get_median(self):
        if len(self.small) > len(self.large):
            return -self.small[0]
        return (-self.small[0] + self.large[0]) / 2

print("\nRunning Median:")
rm = RunningMedian()
for num in [5, 15, 1, 3]:
    rm.add(num)
    print(f"After adding {num}, median: {rm.get_median()}")

# Subarray sum to target
def subarray_sum(nums, target):
    """Find subarray with sum equal to target."""
    current_sum = 0
    prefix_sum = {0: -1}
    for i, num in enumerate(nums):
        current_sum += num
        if current_sum - target in prefix_sum:
            start = prefix_sum[current_sum - target] + 1
            return nums[start:i+1]
        prefix_sum[current_sum] = i
    return []

print("\nSubarray Sum:")
nums = [1, 2, 3, 4, 5]
target = 9
print(f"Array: {nums}, Target: {target}")
print(f"Subarray: {subarray_sum(nums, target)}")

# Sliding window min/max
def sliding_window_min(nums, k):
    """Find min in each sliding window."""
    dq = deque()
    result = []
    for i, num in enumerate(nums):
        while dq and nums[dq[-1]] >= num:
            dq.pop()
        dq.append(i)
        if dq[0] <= i - k:
            dq.popleft()
        if i >= k - 1:
            result.append(nums[dq[0]])
    return result

print("\nSliding Window Min:")
nums = [4, 2, 12, 3, 5, 2, 9]
k = 3
print(f"Array: {nums}, k: {k}")
print(f"Min values: {sliding_window_min(nums, k)}")
