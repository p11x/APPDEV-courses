# Example198.py
# Topic: OrderedDict, namedtuple, ChainMap

# This file demonstrates additional collections: OrderedDict, namedtuple, and ChainMap.


# ============================================================
# Example 1: OrderedDict Insertion Order
# ============================================================
print("=== OrderedDict ===")

from collections import OrderedDict

d = OrderedDict()
d["first"] = 1
d["second"] = 2
d["third"] = 3
print(f"Order: {list(d.keys())}")    # ['first', 'second', 'third']


# ============================================================
# Example 2: move_to_end
# ============================================================
print("\n=== move_to_end ===")

d = OrderedDict({"a": 1, "b": 2, "c": 3})
d.move_to_end("b")
print(f"Keys: {list(d.keys())}")    # ['a', 'c', 'b']

d.move_to_end("b", last=False)
print(f"Keys: {list(d.keys())}")    # ['b', 'a', 'c']


# ============================================================
# Example 3: OrderedDict Equality
# ============================================================
print("\n=== OrderedDict Equality ===")

d1 = OrderedDict({"a": 1, "b": 2})
d2 = OrderedDict({"b": 2, "a": 1})
d3 = {"a": 1, "b": 2}

print(f"OrderedDict: {d1 == d2}")    # False - different order
print(f"Dict: {dict(d1) == d3}")    # True - order ignored


# ============================================================
# Example 4: Basic namedtuple
# ============================================================
print("\n=== namedtuple ===")

from collections import namedtuple

Point = namedtuple("Point", ["x", "y"])
p = Point(10, 20)
print(f"Point: {p}")    # Point(x=10, y=20)
print(f"x: {p.x}, y: {p.y}")    # x: 10, y: 20


# ============================================================
# Example 5: namedtuple Methods
# ============================================================
print("\n=== namedtuple Methods ===")

Point = namedtuple("Point", "x y z")
p = Point(1, 2, 3)

print(f"As dict: {p._asdict()}")    # OrderedDict([('x',1),...])
print(f"Fields: {p._fields}")    # ('x', 'y', 'z')
print(f"Replace: {p._replace(x=100)}")    # Point(x=100, y=2, z=3)


# ============================================================
# Example 6: namedtuple with Defaults
# ============================================================
print("\n=== Defaults ===")

from collections import namedtuple

Point = namedtuple("Point", "x y z", defaults=[0, 0])
p = Point(10)
print(f"Partial: {p}")    # Point(x=10, y=0, z=0)


# ============================================================
# Example 7: Basic ChainMap
# ============================================================
print("\n=== ChainMap ===")

from collections import ChainMap

dict1 = {"a": 1, "b": 2}
dict2 = {"b": 3, "c": 4}

cm = ChainMap(dict1, dict2)
print(f"Get a: {cm['a']}")    # 1
print(f"Get b: {cm['b']}")    # 2 (first dict)
print(f"Get c: {cm['c']}")    # 4 (second dict)


# ============================================================
# Example 8: ChainMap Maps
# ============================================================
print("\n=== Maps Property ===")

dict1 = {"a": 1}
dict2 = {"b": 2}
dict3 = {"c": 3}

cm = ChainMap(dict1, dict2, dict3)
print(f"Maps: {cm.maps}")    # [dict1, dict2, dict3]


# ============================================================
# Example 9: ChainMap New Child
# ============================================================
print("\n=== New Child ===")

dict1 = {"a": 1}
dict2 = {"b": 2}

cm = ChainMap(dict1, dict2)
cm_new = cm.new_child({"c": 3})
print(f"New child: {cm_new.maps}")    # [c:3, a:1], [b:2]


# ============================================================
# Example 10: ChainMap Update
# ============================================================
print("\n=== Update ===")

dict1 = {"a": 1}
dict2 = {"b": 2}

cm = ChainMap(dict1, dict2)
cm["a"] = 100  # Modifies first dict
print(f"Dict1: {dict1}")    # {'a': 100}


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ORDEREDDICT:
- Preserves insertion order (Python 3.7+ dict does too)
- move_to_end(): Move key to end/start

NAMEDTUPLE:
- Lightweight object
- p.x, p.y access
- _asdict(), _replace(), _fields

CHAINMAP:
- Multiple dicts as one view
- Lookup: first dict wins
- new_child(): Add new dict at front
""")
