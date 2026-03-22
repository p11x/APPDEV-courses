# Example258: String Algorithms
# Reverse string
def reverse_string(s):
    return s[::-1]

print("String Algorithms - Reverse:")
s = "hello world"
print(f"Original: '{s}'")
print(f"Reversed: '{reverse_string(s)}'")

# Palindrome check
def is_palindrome(s):
    cleaned = ''.join(c.lower() for c in s if c.isalnum())
    return cleaned == cleaned[::-1]

print("\nPalindrome check:")
tests = ["racecar", "A man a plan a canal Panama", "hello"]
for test in tests:
    print(f"  '{test}': {is_palindrome(test)}")

# Anagram check
def is_anagram(s1, s2):
    return sorted(s1.lower()) == sorted(s2.lower())

print("\nAnagram check:")
print(f"'listen' vs 'silent': {is_anagram('listen', 'silent')}")
print(f"'hello' vs 'world': {is_anagram('hello', 'world')}")

# Substring search (naive)
def naive_search(text, pattern):
    """Naive substring search."""
    positions = []
    for i in range(len(text) - len(pattern) + 1):
        if text[i:i+len(pattern)] == pattern:
            positions.append(i)
    return positions

print("\nNaive search:")
text = "AABAACAADAABAABA"
pattern = "AABA"
print(f"Text: {text}")
print(f"Pattern: {pattern}")
print(f"Positions: {naive_search(text, pattern)}")

# Longest common prefix
def longest_common_prefix(strs):
    if not strs:
        return ""
    min_len = min(len(s) for s in strs)
    result = ""
    for i in range(min_len):
        char = strs[0][i]
        if all(s[i] == char for s in strs):
            result += char
        else:
            break
    return result

print("\nLongest common prefix:")
strs = ["flower", "flow", "flight"]
print(f"Words: {strs}")
print(f"LCP: '{longest_common_prefix(strs)}'")

# Count character frequency
def char_frequency(s):
    from collections import Counter
    return dict(Counter(s))

print("\nCharacter frequency:")
print(f"'hello': {char_frequency('hello')}")

# Remove duplicates
def remove_duplicates(s):
    seen = set()
    result = []
    for c in s:
        if c not in seen:
            seen.add(c)
            result.append(c)
    return ''.join(result)

print("\nRemove duplicates:")
print(f"'hello world': '{remove_duplicates('hello world')}'")
