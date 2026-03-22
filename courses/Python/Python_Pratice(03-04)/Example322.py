# Example322: Practice with Statistics
import statistics

# Statistics functions
print("Statistics:")
data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
print(f"Mean: {statistics.mean(data)}")
print(f"Median: {statistics.median(data)}")
print(f"Mode: {statistics.mode([1, 2, 2, 3])}")
print(f"Stdev: {statistics.stdev(data)}")
print(f"Variance: {statistics.variance(data)}")

# Median low/high
print("\nMedian low/high:")
data = [1, 2, 3, 4]
print(f"Median low: {statistics.median_low(data)}")
print(f"Median high: {statistics.median_high(data)}")
