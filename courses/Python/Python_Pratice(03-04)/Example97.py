# Example97.py
# Topic: Lists - Methods (append, extend, insert, pop, remove, sort, reverse)

# This file demonstrates list methods.


# ============================================================
# Example 1: Adding Elements
# ============================================================
print("=== Adding Elements ===")

my_list = [1, 2, 3]

# append() - adds to end
my_list.append(4)
print(f"After append(4): {my_list}")

# extend() - adds multiple items
my_list.extend([5, 6])
print(f"After extend([5, 6]): {my_list}")

# insert() - at specific index
my_list.insert(0, 0)
print(f"After insert(0, 0): {my_list}")

# Also works with + (creates new list)
new_list = my_list + [7, 8]
print(f"my_list + [7, 8]: {new_list}")


# ============================================================
# Example 2: Removing Elements
# ============================================================
print("\n=== Removing Elements ===")

my_list = [1, 2, 3, 4, 5]

# pop() - removes and returns last element
popped = my_list.pop()
print(f"Popped: {popped}, List: {my_list}")

# pop() at specific index
popped = my_list.pop(0)
print(f"Popped index 0: {popped}, List: {my_list}")

# remove() - removes first occurrence
my_list = [1, 2, 3, 2, 4]
my_list.remove(2)
print(f"After remove(2): {my_list}")

# clear() - removes all
my_list.clear()
print(f"After clear(): {my_list}")


# ============================================================
# Example 3: Finding Elements
# ============================================================
print("\n=== Finding Elements ===")

my_list = [1, 2, 3, 4, 5, 3]

# index() - first occurrence
index = my_list.index(3)
print(f"Index of 3: {index}")

# count() - occurrences
count = my_list.count(3)
print(f"Count of 3: {count}")

# in operator
print(f"3 in list: {3 in my_list}")
print(f"10 in list: {10 in my_list}")


# ============================================================
# Example 4: Sorting
# ============================================================
print("\n=== Sorting ===")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

# sort() - in-place
numbers.sort()
print(f"After sort(): {numbers}")

numbers = [3, 1, 4, 1, 5, 9, 2, 6]

# sorted() - returns new list
sorted_numbers = sorted(numbers)
print(f"sorted(): {sorted_numbers}")
print(f"Original unchanged: {numbers}")

# Reverse sort
numbers.sort(reverse=True)
print(f"Reverse sort: {numbers}")

# Sort with key
words = ["banana", "apple", "cherry"]
words.sort(key=len)
print(f"Sort by length: {words}")


# ============================================================
# Example 5: Reversing
# ============================================================
print("\n=== Reversing ===")

my_list = [1, 2, 3, 4, 5]

# reverse() - in-place
my_list.reverse()
print(f"After reverse(): {my_list}")

# Reversed iterator
my_list = [1, 2, 3]
for item in reversed(my_list):
    print(f"Reversed item: {item}")

# Slice reverse (creates new list)
reversed_list = my_list[::-1]
print(f"Slice reverse: {reversed_list}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY: List Methods")
print("=" * 50)
print("""
ADDING:
- list.append(item) - add to end
- list.extend(items) - add multiple
- list.insert(index, item) - at index

REMOVING:
- list.pop() - remove and return last
- list.pop(index) - remove at index
- list.remove(item) - remove first occurrence
- list.clear() - remove all

FINDING:
- list.index(item) - first occurrence
- list.count(item) - count occurrences
- item in list - membership test

SORTING:
- list.sort() - in-place
- sorted(list) - new list
- list.sort(reverse=True) - reverse

REVERSING:
- list.reverse() - in-place
- reversed(list) - iterator
- list[::-1] - new reversed list
""")
