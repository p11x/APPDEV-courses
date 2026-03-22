# Example235: itertools - tee, islice, pairwise (Python 3.10+)
import itertools

# tee(iterable, n=2) - creates n independent iterators
print("tee - create multiple independent iterators:")  # tee - create multiple independent iterators:
data = [1, 2, 3, 4, 5]
it1, it2 = itertools.tee(data)

print("Iterator 1:", list(it1))  # Iterator 1: [1, 2, 3, 4, 5]
print("Iterator 2:", list(it2))  # Iterator 2: [1, 2, 3, 4, 5]

# Independent iteration
print("\nIndependent iteration:")  # (blank line)
it1, it2 = itertools.tee(data)
print("First 3 from it1:", [next(it1) for _ in range(3)])  # First 3 from it1: [1, 2, 3]
print("First 3 from it2:", [next(it2) for _ in range(3)])  # First 3 from it2: [1, 2, 3]

# tee with transformation
print("\ntee with map:")  # (blank line)
it1, it2 = itertools.tee(data)
mapped = map(lambda x: x * 2, it1)
print(f"Doubled: {list(mapped)}")  # Doubled: [2, 4, 6, 8, 10]
print(f"Original it2: {list(it2)}")  # Original it2: [1, 2, 3, 4, 5]

# pairwise (Python 3.10+) - consecutive pairs
try:
    print("\npairwise - consecutive pairs:")  # (blank line)
    data = [1, 2, 3, 4, 5]
    pairs = list(itertools.pairwise(data))
    print(f"Input: {data}")  # Input: [1, 2, 3, 4, 5]
    print(f"Pairs: {pairs}")  # Pairs: [(1, 2), (2, 3), (3, 4), (4, 5)]
except AttributeError:
    print("\npairwise not available (Python 3.10+), using alternative:")  # (blank line)
    data = [1, 2, 3, 4, 5]
    pairs = list(zip(data, data[1:]))
    print(f"Pairs (alternative): {pairs}")  # Pairs (alternative): [(1, 2), (2, 3), (3, 4), (4, 5)]

# windowed view using tee
print("\nWindow of 3 (manual):")  # (blank line)
data = [1, 2, 3, 4, 5]
windows = list(zip(*[itertools.islice(data, i, None) for i in range(3)]))
print(f"Windows of 3: {windows}")  # Windows of 3: [(1, 2, 3), (2, 3, 4), (3, 4, 5)]

# Practical: sliding window average
print("\nSliding window average:")  # (blank line)
data = [1, 3, 5, 7, 9, 11]
window_size = 3
windows = [data[i:i+window_size] for i in range(len(data) - window_size + 1)]
averages = [sum(w) / len(w) for w in windows]
print(f"Data: {data}")  # Data: [1, 3, 5, 7, 9, 11]
print(f"Window size: {window_size}")  # Window size: 3
print(f"Averages: {averages}")  # Averages: [3.0, 5.0, 7.0, 9.0]
