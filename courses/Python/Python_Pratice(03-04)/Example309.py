# Example309: Operator Module
import operator

# Arithmetic
print("Operator Module:")
print(f"add(1, 2): {operator.add(1, 2)}")
print(f"mul(3, 4): {operator.mul(3, 4)}")
print(f"pow(2, 3): {operator.pow(2, 3)}")

# Comparison
print(f"\neq(1, 1): {operator.eq(1, 1)}")
print(f"lt(1, 2): {operator.lt(1, 2)}")

# Item and attr
print("\nItem/Attr:")
print(f"getitem([1,2,3], 1): {operator.getitem([1,2,3], 1)}")
print(f"setitem: ", end="")
l = [1, 2, 3]
operator.setitem(l, 0, 10)
print(l)

# Methodcaller
print("\nMethodcaller:")
print(operator.methodcaller('upper')('hello'))

# Attrgetter and itemgetter
from operator import itemgetter, attrgetter
print(f"\nitemgetter(1)([1,2,3]): {itemgetter(1)([1,2,3])}")
