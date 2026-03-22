# Example321: Practice with Random
import random

# Random functions
print("Random Functions:")
print(f"Random: {random.random()}")
print(f"Randint: {random.randint(1, 10)}")
print(f"Choice: {random.choice([1, 2, 3])}")
print(f"Sample: {random.sample([1,2,3,4,5], 2)}")

# Shuffle
print("\nShuffle:")
lst = [1, 2, 3, 4, 5]
random.shuffle(lst)
print(f"Shuffled: {lst}")

# Seed
print("\nSeed:")
random.seed(42)
print(f"Seed 42: {random.random()}")
random.seed(42)
print(f"Seed 42 again: {random.random()}")
