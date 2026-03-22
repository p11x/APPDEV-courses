# Example255: Greedy Algorithms
# Activity selection
def activity_selection(activities):
    """Select max activities that don't overlap."""
    activities.sort(key=lambda x: x[1])
    selected = [activities[0]]
    last_end = activities[0][1]
    for start, end in activities[1:]:
        if start >= last_end:
            selected.append((start, end))
            last_end = end
    return selected

print("Greedy - Activity Selection:")
activities = [(1, 4), (3, 5), (0, 6), (5, 7), (3, 9), (5, 9), (6, 10), (8, 11)]
selected = activity_selection(activities)
print(f"Activities: {activities}")
print(f"Selected: {selected}")

# Fractional knapsack
def fractional_knapsack(items, capacity):
    """Max value with fractional items."""
    items.sort(key=lambda x: x[1]/x[0], reverse=True)
    total_value = 0
    for weight, value in items:
        if capacity == 0:
            break
        if weight <= capacity:
            total_value += value
            capacity -= weight
        else:
            total_value += (capacity / weight) * value
            capacity = 0
    return total_value

print("\nFractional Knapsack:")
items = [(10, 60), (20, 100), (30, 120)]  # (weight, value)
capacity = 50
print(f"Items: {items}, Capacity: {capacity}")
print(f"Max value: {fractional_knapsack(items, capacity)}")

# Coin change (minimum coins)
def coin_change(coins, amount):
    """Minimum coins needed."""
    dp = [float('inf')] * (amount + 1)
    dp[0] = 0
    for coin in coins:
        for i in range(coin, amount + 1):
            dp[i] = min(dp[i], dp[i - coin] + 1)
    return dp[amount] if dp[amount] != float('inf') else -1

print("\nCoin Change:")
coins = [1, 2, 5]
amount = 11
print(f"Coins: {coins}, Amount: {amount}")
print(f"Min coins: {coin_change(coins, amount)}")

# Job sequencing
def job_sequencing(jobs):
    """Max profit job sequencing with deadlines."""
    jobs.sort(key=lambda x: x[1], reverse=True)
    max_deadline = max(j[0] for j in jobs)
    slots = [False] * (max_deadline + 1)
    profit = 0
    for deadline, profit_val in jobs:
        for i in range(min(max_deadline, deadline), 0, -1):
            if not slots[i]:
                slots[i] = True
                profit += profit_val
                break
    return profit

print("\nJob Sequencing:")
jobs = [(2, 100), (2, 20), (3, 30), (3, 40), (1, 50)]  # (deadline, profit)
print(f"Jobs: {jobs}")
print(f"Max profit: {job_sequencing(jobs)}")

# Huffman coding (basic)
print("\nHuffman coding:")
from collections import heapq

def huffman(freq):
    """Build Huffman codes."""
    heap = [[weight, [char, ""]] for char, weight in freq.items()]
    heapq.heapify(heap)
    while len(heap) > 1:
        lo = heapq.heappop(heap)
        hi = heapq.heappop(heap)
        lo[1][1] = '0' + lo[1][1]
        hi[1][1] = '1' + hi[1][1]
        heapq.heappush(heap, [lo[0] + hi[0]] + lo[1] + hi[1])
    return dict(heap[0][1])

freq = {'a': 45, 'b': 13, 'c': 12, 'd': 16, 'e': 9, 'f': 5}
codes = huffman(freq)
for char in sorted(codes):
    print(f"  {char}: {codes[char]}")
