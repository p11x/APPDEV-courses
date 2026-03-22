# Example252: Divide and Conquer - Merge Sort
def merge_sort(arr):
    """Classic merge sort implementation."""
    if len(arr) <= 1:
        return arr
    
    mid = len(arr) // 2
    left = merge_sort(arr[:mid])
    right = merge_sort(arr[mid:])
    return merge(left, right)

def merge(left, right):
    """Merge two sorted arrays."""
    result = []
    i = j = 0
    while i < len(left) and j < len(right):
        if left[i] <= right[j]:
            result.append(left[i])
            i += 1
        else:
            result.append(right[j])
            j += 1
    result.extend(left[i:])
    result.extend(right[j:])
    return result

print("Divide and Conquer - Merge Sort:")
arr = [64, 34, 25, 12, 22, 11, 90]
print(f"Original: {arr}")
print(f"Sorted: {merge_sort(arr)}")

# Count inversions
def count_inversions(arr):
    """Count inversions using merge sort."""
    def merge_and_count(left, right):
        result = []
        count = 0
        i = j = 0
        while i < len(left) and j < len(right):
            if left[i] <= right[j]:
                result.append(left[i])
                i += 1
            else:
                result.append(right[j])
                count += len(left) - i
                j += 1
        result.extend(left[i:])
        result.extend(right[j:])
        return result, count
    
    if len(arr) <= 1:
        return arr, 0
    
    mid = len(arr) // 2
    left, left_inv = count_inversions(arr[:mid])
    right, right_inv = count_inversions(arr[mid:])
    merged, split_inv = merge_and_count(left, right)
    
    return merged, left_inv + right_inv + split_inv

print("\nCount inversions:")
arr = [2, 4, 1, 3, 5]
result, inversions = count_inversions(arr)
print(f"Array: {arr}")
print(f"Inversions: {inversions}")

# Find closest pair
def closest_pair(points):
    """Find closest pair of points."""
    def distance(p1, p2):
        return ((p1[0] - p2[0])**2 + (p1[1] - p2[1])**2)**0.5
    
    if len(points) <= 3:
        min_dist = float('inf')
        pair = None
        for i in range(len(points)):
            for j in range(i + 1, len(points)):
                d = distance(points[i], points[j])
                if d < min_dist:
                    min_dist = d
                    pair = (points[i], points[j])
        return pair, min_dist
    
    mid = len(points) // 2
    left_pair, left_dist = closest_pair(points[:mid])
    right_pair, right_dist = closest_pair(points[mid:])
    
    if left_dist < right_dist:
        min_dist, best_pair = left_dist, left_pair
    else:
        min_dist, best_pair = right_dist, right_pair
    
    strip = [p for p in points if abs(p[0] - points[mid][0]) < min_dist]
    strip.sort(key=lambda p: p[1])
    
    for i in range(len(strip)):
        for j in range(i + 1, min(i + 7, len(strip))):
            d = distance(strip[i], strip[j])
            if d < min_dist:
                min_dist = d
                best_pair = (strip[i], strip[j])
    
    return best_pair, min_dist

print("\nClosest pair:")
points = [(2, 3), (12, 30), (40, 50), (5, 1), (12, 10), (3, 4)]
pair, dist = closest_pair(points)
print(f"Points: {points}")
print(f"Closest: {pair}, distance: {dist:.2f}")
