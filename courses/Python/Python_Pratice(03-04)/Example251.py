# Example251: Sliding Window Technique
def max_subarray_sum(arr, k):
    """Find maximum sum of k consecutive elements."""
    if len(arr) < k:
        return None
    window_sum = sum(arr[:k])
    max_sum = window_sum
    for i in range(k, len(arr)):
        window_sum += arr[i] - arr[i - k]
        max_sum = max(max_sum, window_sum)
    return max_sum

print("Sliding Window - Max subarray sum:")
arr = [2, 1, 5, 1, 3, 2]
k = 3
print(f"Array: {arr}, k: {k}")
print(f"Max sum: {max_subarray_sum(arr, k)}")

# Find average of all subarrays of length k
def find_averages(k, arr):
    """Find averages of all k-sized subarrays."""
    result = []
    window_sum = sum(arr[:k])
    result.append(window_sum / k)
    for i in range(k, len(arr)):
        window_sum = window_sum - arr[i - k] + arr[i]
        result.append(window_sum / k)
    return result

print("\nSubarray averages:")
arr = [1, 3, 2, 6, -1, 4, 1, 8, 2]
k = 5
print(f"Array: {arr}, k: {k}")
print(f"Averages: {find_averages(k, arr)}")

# Longest substring with k distinct characters
def longest_substring_k_distinct(s, k):
    """Find longest substring with at most k distinct chars."""
    if k == 0:
        return 0
    char_count = {}
    left = 0
    max_length = 0
    for right in range(len(s)):
        char_count[s[right]] = char_count.get(s[right], 0) + 1
        while len(char_count) > k:
            char_count[s[left]] -= 1
            if char_count[s[left]] == 0:
                del char_count[s[left]]
            left += 1
        max_length = max(max_length, right - left + 1)
    return max_length

print("\nLongest substring with k distinct:")
s = "araaci"
k = 2
print(f"String: {s}, k: {k}")
print(f"Length: {longest_substring_k_distinct(s, k)}")

# Minimum sliding window
def min_window(s, t):
    """Find minimum window containing all chars of t."""
    from collections import Counter
    need = Counter(t)
    window = {}
    have, need_len = 0, len(need)
    res, res_len = [-1, -1], float('inf')
    left = 0
    for right, char in enumerate(s):
        window[char] = window.get(char, 0) + 1
        if char in need and window[char] == need[char]:
            have += 1
        while have == need_len:
            if right - left + 1 < res_len:
                res = [left, right]
                res_len = right - left + 1
            window[s[left]] -= 1
            if s[left] in need and window[s[left]] < need[s[left]]:
                have -= 1
            left += 1
    return s[res[0]:res[1]+1] if res_len != float('inf') else ""

print("\nMinimum window:")
s = "ADOBECODEBANC"
t = "ABC"
print(f"S: {s}, T: {t}")
print(f"Min window: '{min_window(s, t)}'")
