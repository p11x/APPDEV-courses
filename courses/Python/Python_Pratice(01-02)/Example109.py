# Example109.py
# Topic: Iteration Tools — Zip Longest

# Use zip_longest() to iterate over unequal length iterables

# === Import zip_longest ===
from itertools import zip_longest

# === Basic zip_longest ===
# zip_longest fills missing values with a fillvalue
short = [1, 2]
long = [10, 20, 30, 40]

print("zip (short):")
for a, b in zip(short, long):
    print(str(a) + " - " + str(b))

print("\nzip_longest (fillvalue=None):")
for a, b in zip_longest(short, long):
    print(str(a) + " - " + str(b))

# === With custom fillvalue ===
print("\nzip_longest (fillvalue=0):")
for a, b in zip_longest(short, long, fillvalue=0):
    print(str(a) + " - " + str(b))

print("\nzip_longest (fillvalue='X'):")
for a, b in zip_longest(short, long, fillvalue="X"):
    print(str(a) + " - " + str(b))

# === Practical: Aligning data of different lengths ===
names = ["Alice", "Bob", "Carol", "David", "Eve"]
scores = [95, 87, 92]  # One person has no score

print("\nScores with zip_longest:")
for name, score in zip_longest(names, scores, fillvalue="N/A"):
    print(name + ": " + str(score))

# === Reverse: short list as second ===
print("\nReversed fillvalue:")
for a, b in zip_longest([1, 2], [10, 20, 30, 40], fillvalue=0):
    print(str(a) + " - " + str(b))

# === With three iterables ===
from itertools import zip_longest

a = [1, 2]
b = ["a", "b", "c"]
c = [True, True, True, True]

print("\nThree iterables:")
for x, y, z in zip_longest(a, b, c, fillvalue="?"):
    print(str(x) + " - " + y + " - " + str(z))

# === Practical: Database merge ===
master = ["user1", "user2", "user3", "user4", "user5"]
logins = ["user1", "user3"]  # Only some users logged in

print("\nLogin merge:")
for user, logged in zip_longest(master, logins, fillvalue="Never"):
    status = "Logged in" if logged != "Never" else "Never logged in"
    print(user + ": " + status)

# === zip_longest with None fillvalue (default) ===
result = list(zip_longest([1, 2], [10, 20, 30]))
print("\nDefault fillvalue is None:")
print(str(result))

# === When to use zip vs zip_longest ===
# Use zip when lengths are equal
# Use zip_longest when you need all values from longest

# === Practical: Processing CSV with optional fields ===
header = ["name", "age", "city", "phone"]
row1 = ["Alice", "30", "NYC", "555-1234"]
row2 = ["Bob", "25"]  # No phone
row3 = ["Carol"]  # Only name

print("\nProcessing rows with optional fields:")
for r in [row1, row2, row3]:
    data = dict(zip_longest(header, r, fillvalue=""))
    print(str(data))

# === Key difference from zip ===
print("\n=== Key difference ===")
x = [1, 2, 3]
y = ["a", "b"]

print("zip: " + str(list(zip(x, y))))
print("zip_longest: " + str(list(zip_longest(x, y, fillvalue=None))))
