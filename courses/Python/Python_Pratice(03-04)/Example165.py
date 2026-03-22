# Example165.py
# Topic: Advanced Iteration Patterns


# ============================================================
# Example 1: Iterator Protocol
# ============================================================
print("=== Iterator Protocol ===")

class Counter:
    def __init__(self, limit):
        self.limit = limit
        self.current = 0
    
    def __iter__(self):
        return self
    
    def __next__(self):
        if self.current < self.limit:
            result = self.current
            self.current += 1
            return result
        raise StopIteration

for i in Counter(5):
    print(i, end=" ")
print()


# ============================================================
# Example 2: Generator Functions
# ============================================================
print("\n=== Generator Functions ===")

def count_up_to(n):
    i = 1
    while i <= n:
        yield i
        i += 1

gen = count_up_to(5)
print(f"Generator: {gen}")
print(f"Next: {next(gen)}")
print(f"Next: {next(gen)}")

for i in count_up_to(3):
    print(i, end=" ")
print()


# ============================================================
# Example 3: Generator with Send
# ============================================================
print("\n=== Generator with Send ===")

def generator_with_send():
    received = yield "Ready"
    yield f"Received: {received}"

gen = generator_with_send()
print(next(gen))
print(gen.send("Hello"))


# ============================================================
# Example 4: Infinite Generator
# ============================================================
print("\n=== Infinite Generator ===")

def infinite_counter(start=0):
    while True:
        yield start
        start += 1

counter = infinite_counter(10)
for i in range(5):
    print(next(counter), end=" ")
print()


# ============================================================
# Example 5: Generator Pipeline
# ============================================================
print("\n=== Generator Pipeline ===")

def numbers():
    for i in range(10):
        yield i

def double(n):
    return n * 2

def filter_even(n):
    return n % 2 == 0

pipeline = map(double, filter(filter_even, numbers()))
print(list(pipeline))


# ============================================================
# Example 6: Coroutine Basics
# ============================================================
print("\n=== Coroutine ===")

def coroutine():
    while True:
        value = yield
        print(f"Received: {value}")

co = coroutine()
next(co)
co.send("Hello")
co.send("World")
co.close()


# ============================================================
# Example 7: Yield From
# ============================================================
print("\n=== Yield From ===")

def chain(*iterables):
    for it in iterables:
        yield from it

result = list(chain([1, 2], [3, 4], [5, 6]))
print(f"Chained: {result}")

def flatten(nested):
    for item in nested:
        if isinstance(item, (list, tuple)):
            yield from flatten(item)
        else:
            yield item

nested = [1, [2, [3, 4]], 5]
print(f"Flattened: {list(flatten(nested))}")


# ============================================================
# Example 8: Real-World: Stream Processing
# ============================================================
print("\n=== Stream Processing ===")

def process_stream(data):
    def filter_func(x):
        return x > 0
    
    def transform_func(x):
        return x * 2
    
    filtered = (x for x in data if filter_func(x))
    transformed = (transform_func(x) for x in filtered)
    return list(transformed)

data = [-2, -1, 0, 1, 2, 3, 4, 5]
result = process_stream(data)
print(f"Input: {data}")
print(f"Output: {result}")
