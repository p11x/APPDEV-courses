# Example223.py
# Topic: OrderedDict and ChainMap

# This file demonstrates OrderedDict and ChainMap features.


# ============================================================
# Example 1: OrderedDict Basics
# ============================================================
print("=== OrderedDict ===")

from collections import OrderedDict

d = OrderedDict()
d["a"] = 1
d["b"] = 2
d["c"] = 3
print(f"Keys: {list(d.keys())}")


# ============================================================
# Example 2: move_to_end
# ============================================================
print("\n=== move_to_end ===")

d = OrderedDict({"a": 1, "b": 2, "c": 3})
d.move_to_end("b")
print(f"Keys: {list(d.keys())}")


# ============================================================
# Example 3: popitem
# ============================================================
print("\n=== popitem ===")

d = OrderedDict({"a": 1, "b": 2})
print(f"Pop last: {d.popitem()}")
print(f"Pop first: {d.popitem(last=False)}")


# ============================================================
# Example 4: ChainMap Basics
# ============================================================
print("\n=== ChainMap ===")

from collections import ChainMap

dict1 = {"a": 1, "b": 2}
dict2 = {"b": 3, "c": 4}

cm = ChainMap(dict1, dict2)
print(f"a: {cm['a']}")
print(f"b: {cm['b']}")
print(f"c: {cm['c']}")


# ============================================================
# Example 5: ChainMap Maps
# ============================================================
print("\n=== maps property ===")

cm = ChainMap({"a": 1}, {"b": 2})
print(f"Maps: {cm.maps}")


# ============================================================
# Example 6: new_child
# ============================================================
print("\n=== new_child ===")

cm1 = ChainMap({"a": 1})
cm2 = cm1.new_child({"b": 2})
print(f"New child maps: {cm2.maps}")


# ============================================================
# Example 7: ChainMap Update
# ============================================================
print("\n=== Update ===")

cm = ChainMap({"a": 1})
cm["a"] = 100
print(f"Dict: {cm.maps[0]}")


# ============================================================
# Example 8: Context Variables
# ============================================================
print("\n=== Context ===")

from collections import ChainMap

local = {"user": "Alice"}
global_env = {"user": "Bob", "path": "/home"}
cm = ChainMap(local, global_env)
print(f"User: {cm['user']}")


# ============================================================
# Example 9: Default Values
# ============================================================
print("\n=== Defaults ===")

from collections import ChainMap

defaults = {"theme": "dark", "lang": "en"}
user = {"lang": "fr"}
cm = ChainMap(user, defaults)
print(f"Theme: {cm['theme']}")
print(f"Lang: {cm['lang']}")


# ============================================================
# Example 10: Reverse Lookup
# ============================================================
print("\n=== Reverse Lookup ===")

from collections import ChainMap

child = {"key": "child_value"}
parent = {"key": "parent_value"}
cm = ChainMap(child, parent)
for m in cm.maps:
    if "key" in m:
        print(f"Found in: {m}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("SUMMARY")
print("=" * 50)
print("""
ORDEREDDICT:
- move_to_end(): reorder
- popitem(): remove items
- preserves insertion order

CHAINMAP:
- Multiple dicts as one view
- First dict wins for lookup
- new_child(): add scope
""")
