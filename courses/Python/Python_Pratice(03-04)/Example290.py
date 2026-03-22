# Example290: Generators and Yield
# Basic generator
def count_up_to(n):
    count = 1
    while count <= n:
        yield count
        count += 1

print("Basic Generator:")
gen = count_up_to(5)
print(f"First: {next(gen)}")
print(f"Second: {next(gen)}")
print(f"All: {list(count_up_to(5))}")

# Generator with send
def counter():
    count = 0
    while True:
        value = yield count
        if value is not None:
            count = value
        else:
            count += 1

print("\nGenerator with send:")
gen = counter()
print(next(gen))
print(next(gen))
print(gen.send(10))
print(next(gen))

# Generator expression
print("\nGenerator Expression:")
gen = (x**2 for x in range(5))
print(list(gen))

# Generator pipeline
def integers():
    n = 1
    while True:
        yield n
        n += 1

def squares(gen):
    for n in gen:
        yield n ** 2

def limit(gen, n):
    for i, val in enumerate(gen):
        if i >= n:
            break
        yield val

print("\nGenerator Pipeline:")
result = limit(squares(integers()), 5)
print(list(result))
