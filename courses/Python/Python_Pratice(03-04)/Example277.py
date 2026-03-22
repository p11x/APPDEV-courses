# Example277: Random and Statistics
import random
import statistics

# Random numbers
print("Random Numbers:")
print(f"Random float [0,1): {random.random()}")
print(f"Random int [1,10): {random.randint(1, 10)}")
print(f"Random choice from list: {random.choice([1,2,3,4,5])}")
print(f"Random sample (3): {random.sample([1,2,3,4,5], 3)}")

# Shuffle
print("\nShuffle:")
arr = [1, 2, 3, 4, 5]
random.shuffle(arr)
print(f"Shuffled: {arr}")

# Weighted choice
print("\nWeighted Choice:")
choices = ['a', 'b', 'c']
weights = [0.5, 0.3, 0.2]
print(f"Choice: {random.choices(choices, weights, k=5)}")

# Statistics
print("\nStatistics:")
data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
print(f"Data: {data}")
print(f"Mean: {statistics.mean(data)}")
print(f"Median: {statistics.median(data)}")
print(f"Mode: {statistics.mode([1,1,2,2,3])}")
print(f"Stdev: {statistics.stdev(data)}")
print(f"Variance: {statistics.variance(data)}")

# Statistics with collections
from collections import Counter
data = [1, 2, 2, 3, 3, 3, 4]
freq = Counter(data)
print(f"\nMost common: {freq.most_common(1)}")

# Random with seed
print("\nRandom with seed:")
random.seed(42)
print(f"First: {random.random()}")
random.seed(42)
print(f"Second (same): {random.random()}")

# Normal distribution
print("\nNormal Distribution:")
samples = [random.gauss(0, 1) for _ in range(5)]
print(f"Gaussian samples: {[round(s, 2) for s in samples]}")
