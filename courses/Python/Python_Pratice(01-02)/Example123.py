# Example123.py
# Topic: Iteration Tools — Cycle and Repeat

# Using itertools.cycle() and itertools.repeat()

# === Import ===
from itertools import cycle, repeat, islice

# === Basic repeat() ===
# repeat(value, times) - repeat value times times
# If times is None, repeats forever

# Repeat 3 times
result = list(repeat(5, 3))
print("Repeat 3 times: " + str(result))

# Repeat forever (needs limit)
# repeat("hello") - infinite!

# Use with islice to limit
result = list(repeat("x", 5))
print("Repeat with limit: " + str(result))

# === Practical: Repeat for zip ===
names = ["Alice", "Bob", "Carol"]
# Make each name appear twice
repeated = list(zip(names, repeat(2)))
print("Zipped with repeat: " + str(repeated))

# === repeat() with for loop ===
print("Using repeat in loop:")
for i, name in enumerate(zip(names, repeat("!"))):
    if i >= 5:
        break
    print(name)

# === cycle() - repeat infinitely ===
# cycle(iterable) - cycles through forever

# Basic cycle
colors = cycle(["red", "green", "blue"])
print("\nCycle colors:")
for i in range(6):
    print("Color: " + next(colors))

# === cycle() needs to be consumed carefully ===
# cycle creates infinite iterator!
# Always use with break or islice

colors = cycle(["a", "b", "c"])
limited = list(islice(colors, 7))
print("\nCycle limited: " + str(limited))

# === Practical: Alternating pattern ===
from itertools import islice

# Alternate between on and off
states = cycle([True, False])
for i in range(6):
    state = next(states)
    print("State " + str(i) + ": " + ("ON" if state else "OFF"))

# === Practical: Round-robin scheduling ===
from itertools import cycle, islice

players = ["Alice", "Bob", "Carol"]
game = cycle(players)

print("\nTurn order (first 5):")
for i in range(5):
    print("Turn " + str(i+1) + ": " + next(game))

# === repeat() vs cycle() ===
# repeat(x, n) - repeats x exactly n times
# cycle(x) - cycles through x forever

print("\nrepeat vs cycle:")
print("repeat('a', 3): " + str(list(repeat("a", 3))))
print("cycle(['a', 'b']): " + str(list(islice(cycle(["a", "b"]), 4))))

# === Practical: Default values with repeat ===
from itertools import repeat, zip_longest

keys = ["name", "age", "city"]
values = ["Alice", 30]

# Use repeat for missing values
combined = list(zip(keys, repeat("N/A")))
print("\nWith repeat: " + str(combined))

combined = list(zip_longest(keys, values, fillvalue="N/A"))
print("With zip_longest: " + str(combined))

# === repeat() creates lazy iterator ===
r = repeat(42)
print("\nRepeat type: " + str(type(r)))

# Consume
print("First: " + str(next(r)))
print("Second: " + str(next(r)))

# === cycle() creates lazy iterator ===
c = cycle([1, 2])
print("\nCycle type: " + str(type(c)))

print("First: " + str(next(c)))
print("Second: " + str(next(c)))
print("Third: " + str(next(c)))

# === Practical: Tile a pattern ===
from itertools import islice

pattern = [1, 0, 1, 0, 1]
tiled = list(islice(cycle(pattern), 10))
print("\nTiled pattern: " + str(tiled))

# === Note: Memory efficiency ===
# repeat() and cycle() are lazy
# They don't create full lists in memory

# This is memory efficient:
million_repeat = repeat(1, 1000000)  # Only stores the value!

# vs:
million_list = [1] * 1000000  # Creates full list!
