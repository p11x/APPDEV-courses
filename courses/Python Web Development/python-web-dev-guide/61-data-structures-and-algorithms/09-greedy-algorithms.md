# Greedy Algorithms

## What You'll Learn

- Greedy approach
- When greedy works
- Common greedy problems

## Prerequisites

- Completed `08-dynamic-programming.md`

## What Is Greedy

Greedy algorithms make locally optimal choices at each step, hoping for a global optimum. They're simpler than DP but don't always work.

Think of it like filling a backpack with the most valuable items first without knowing if it's the optimal solution.

## When Greedy Works

Greedy works when:
- Problem has greedy-choice property
- Optimal substructure
- Examples: Huffman coding, Dijkstra's algorithm

## Activity Selection

```python
def activity_selection(activities: list[tuple[int, int]]) -> list[int]:
    """Select maximum number of non-overlapping activities."""
    # Sort by end time
    sorted_activities = sorted(activities, key=lambda x: x[1])
    
    result = [0]  # Select first activity
    last_end = sorted_activities[0][1]
    
    for i in range(1, len(sorted_activities)):
        start, end = sorted_activities[i]
        if start >= last_end:
            result.append(i)
            last_end = end
    
    return result
```

## Fractional Knapsack

```python
def fractional_knapsack(
    weights: list[float],
    values: list[float],
    capacity: float
) -> float:
    """Maximum value with fractional items."""
    # Calculate value/weight ratio
    items = sorted(
        zip(weights, values),
        key=lambda x: x[1] / x[0],
        reverse=True
    )
    
    total_value = 0.0
    remaining_capacity = capacity
    
    for weight, value in items:
        if remaining_capacity == 0:
            break
        
        if weight <= remaining_capacity:
            total_value += value
            remaining_capacity -= weight
        else:
            fraction = remaining_capacity / weight
            total_value += value * fraction
            remaining_capacity = 0
    
    return total_value
```

## Huffman Coding

```python
from dataclasses import dataclass
from typing import Literal
from collections import Counter

@dataclass
class HuffmanNode:
    char: str | None
    freq: int
    left: "HuffmanNode | None" = None
    right: "HuffmanNode | None" = None
    
    def __lt__(self, other: "HuffmanNode") -> bool:
        return self.freq < other.freq

def huffman_encoding(text: str) -> dict[str, str]:
    """Generate Huffman codes."""
    # Count frequencies
    freq = Counter(text)
    
    # Create priority queue (min heap)
    import heapq
    heap = [HuffmanNode(char, f) for char, f in freq.items()]
    heapq.heapify(heap)
    
    # Build tree
    while len(heap) > 1:
        left = heapq.heappop(heap)
        right = heapq.heappop(heap)
        merged = HuffmanNode(None, left.freq + right.freq, left, right)
        heapq.heappush(heap, merged)
    
    # Generate codes
    codes = {}
    
    def generate_codes(node: HuffmanNode, code: str = "") -> None:
        if node.char:
            codes[node.char] = code or "0"
        else:
            if node.left:
                generate_codes(node.left, code + "0")
            if node.right:
                generate_codes(node.right, code + "1")
    
    if heap:
        generate_codes(heap[0])
    
    return codes
```

## Dijkstra's Algorithm

```python
import heapq
from collections import defaultdict

def dijkstra(graph: dict, start: str) -> dict[str, float]:
    """Shortest path from start to all nodes."""
    distances = {node: float('inf') for node in graph}
    distances[start] = 0
    pq = [(0, start)]
    visited = set()
    
    while pq:
        dist, node = heapq.heappop(pq)
        
        if node in visited:
            continue
        
        visited.add(node)
        
        for neighbor, weight in graph[node]:
            new_dist = dist + weight
            if new_dist < distances[neighbor]:
                distances[neighbor] = new_dist
                heapq.heappush(pq, (new_dist, neighbor))
    
    return distances
```

## Summary

- Greedy makes local optimal choices
- Works for some problems, not all
- Simpler than dynamic programming

## Next Steps

Continue to `10-algorithms-in-web-development.md`.
