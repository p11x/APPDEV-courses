# Example211.py
# Topic: Generator Functions & yield

# This file demonstrates generator functions with yield for memory-efficient iteration.


# ============================================================
# Example 1: Basic Generator Function
# ============================================================
print("=== Basic Generator ===")

def count_up_to(n):
    i = 1
    while i <= n:
        yield i
        i += 1

gen = count_up_to(5)
print(f"Gen: {list(gen)}")


# ============================================================
# Example 2: Generator with loop
# ============================================================
print("\n=== For Loop ===")

def fibonacci(n):
    a, b = 0, 1
    for _ in range(n):
        yield a
        a, b = b, a + b

for num in fibonacci(10):
    print(num, end=" ")
print()


# ============================================================
# Example 3: Generator Expression
# ============================================================
print("\n=== Generator Expression ===")

gen = (x**2 for x in range(5))
print(f"Gen: {list(gen)}")


# ============================================================
# Example 4: Next and Send
# ============================================================
print("\n=== Next and Send ===")

def echo():
    while True:
        received = yield
        print(f"Got: {received}")

gen = echo()
next(gen)
gen.send("Hello")
gen.send("World")


# ============================================================
# Example 5: Generator with Return
# ============================================================
print("\n=== Generator with Return ===")

def gen_with_return():
    yield 1
    yield 2
    return "Done"

for val in gen_with_return():
    print(val, end=" ")
print()


# ============================================================
# Example 6: Infinite Generator
# ============================================================
print("\n=== Infinite Generator ===")

def infinite_count(start=0):
    while True:
        yield start
        start += 1

gen = infinite_count(5)
for _ in range(5):
    print(next(gen), end=" ")
print()


# ============================================================
# Example 7: Generator Pipeline
# ============================================================
print("\n=== Pipeline ===")

def integers():
    for i in range(1, 10):
        yield i

def squares(gen):
    for x in gen:
        yield x**2

def filter_even(gen):
    for x in gen:
        if x % 2 == 0:
            yield x

result = filter_even(squares(integers()))
print(f"Pipeline: {list(result)}")


# ============================================================
# Example 8: Generator with try-except
# ============================================================
print("\n=== Try Except ===")

def safe_gen():
    for i in range(5):
        try:
            yield i / (i - 2)
        except:
            yield 0

print(f"Safe: {list(safe_gen())}")


# ============================================================
# Example 9: Class as Generator
# ============================================================
print("\n=== Iterator Class ===")

class CountDown:
    def __init__(self, start):
        self.current = start
    
    def __iter__(self):
        return self
    
    def __next__(self):
        if self.current <= 0:
            raise StopIteration
        self.current -= 1
        return self.current + 1

for num in CountDown(5):
    print(num, end=" ")
print()


# ============================================================
# Example 10: Send Values
# ============================================================
print("\n=== Counter ===")

def counter():
    count = 0
    while True:
        received = yield count
        if received:
            count = received

gen = counter()
print(next(gen))
gen.send(10)
print(next(gen))


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
GENERATORS:
- yield: pause function
- next(): get next value
- send(): send values back
- Memory efficient
""")
