# Example227.py
# Topic: operator Module Functions

# This file demonstrates operator module functions.


# ============================================================
# Example 1: itemgetter
# ============================================================
print("=== itemgetter ===")

from operator import itemgetter

data = [(1, "a"), (2, "b"), (3, "c")]
get_first = itemgetter(0)
print(f"First of (1,a): {get_first(data[0])}")


# ============================================================
# Example 2: attrgetter
# ============================================================
print("\n=== attrgetter ===")

from operator import attrgetter

class Person:
    def __init__(self, name, age):
        self.name = name
        self.age = age

p = Person("Alice", 30)
get_name = attrgetter("name")
print(f"Name: {get_name(p)}")


# ============================================================
# Example 3: methodcaller
# ============================================================
print("\n=== methodcaller ===")

from operator import methodcaller

s = "hello"
upper = methodcaller("upper")
print(f"Upper: {upper(s)}")


# ============================================================
# Example 4: add, sub, mul
# ============================================================
print("\n=== Arithmetic ===")

from operator import add, sub, mul, truediv

print(f"add: {add(3, 4)}")
print(f"sub: {sub(5, 2)}")
print(f"mul: {mul(3, 4)}")
print(f"div: {truediv(10, 2)}")


# ============================================================
# Example 5: eq, lt, le
# ============================================================
print("\n=== Comparison ===")

from operator import eq, lt, le, gt, ge

print(f"eq: {eq(3, 3)}")
print(f"lt: {lt(2, 3)}")
print(f"le: {le(3, 3)}")
print(f"gt: {gt(4, 3)}")


# ============================================================
# Example 6: and_, or_, xor
# ============================================================
print("\n=== Bitwise ===")

from operator import and_, or_, xor

print(f"and: {and_(5, 3)}")
print(f"or: {or_(5, 3)}")
print(f"xor: {xor(5, 3)}")


# ============================================================
# Example 7: is_, is_not
# ============================================================
print("\n=== Identity ===")

from operator import is_, is_not

a = [1, 2]
b = [1, 2]
c = a

print(f"is_: {a is c}")
print(f"is_not: {a is_not b}")


# ============================================================
# Example 8: getitem, setitem
# ============================================================
print("\n=== Item Access ===")

from operator import getitem, setitem

data = [1, 2, 3]
print(f"getitem: {getitem(data, 0)}")
setitem(data, 0, 10)
print(f"After setitem: {data}")


# ============================================================
# Example 9: contains
# ============================================================
print("\n=== Contains ===")

from operator import contains

data = [1, 2, 3, 4, 5]
print(f"Contains 3: {contains(data, 3)}")
print(f"Contains 10: {contains(data, 10)}")


# ============================================================
# Example 10: inv, neg
# ============================================================
print("\n=== Unary ===")

from operator import inv, neg, pos

print(f"neg: {neg(5)}")
print(f"pos: {pos(-5)}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
OPERATOR:
- itemgetter: get item
- attrgetter: get attr
- methodcaller: call method
- Arithmetic: add, sub, mul
- Comparison: eq, lt, gt
""")
