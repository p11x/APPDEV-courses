# Example144.py
# Topic: ChainMap and Nested Data


# ============================================================
# Example 1: Basic ChainMap
# ============================================================
print("=== Basic ChainMap ===")

from collections import ChainMap

defaults = {"theme": "dark", "lang": "en"}
user = {"lang": "fr", "notifications": True}
session = {"theme": "light"}

combined = ChainMap(session, user, defaults)
print(f"theme: {combined['theme']}")
print(f"lang: {combined['lang']}")
print(f"notifications: {combined['notifications']}")


# ============================================================
# Example 2: Modifying ChainMap
# ============================================================
print("\n=== Modifying ===")

from collections import ChainMap

a = {"x": 1}
b = {"x": 2, "y": 3}

cm = ChainMap(a, b)
cm["x"] = 10
print(f"After cm['x'] = 10: {dict(cm)}")

cm["z"] = 4
print(f"After cm['z'] = 4: {dict(cm)}")


# ============================================================
# Example 3: New Child Maps
# ============================================================
print("\n=== New Child ===")

from collections import ChainMap

base = {"a": 1, "b": 2}
child = {"b": 3}

cm = ChainMap(child, base)
print(f"Combined: {dict(cm)}")

new_cm = cm.new_child({"c": 4})
print(f"New child: {dict(new_cm)}")


# ============================================================
# Example 4: Accessing Maps
# ============================================================
print("\n=== Access Maps ===")

from collections import ChainMap

a = {"x": 1}
b = {"y": 2}
c = {"z": 3}

cm = ChainMap(a, b, c)
print(f"Maps: {cm.maps}")
print(f"First map: {cm.maps[0]}")


# ============================================================
# Example 5: Real-World: Configuration Priority
# ============================================================
print("\n=== Real-World: Config Priority ===")

from collections import ChainMap

default_config = {
    "debug": False,
    "max_connections": 100,
    "timeout": 30,
}

env_config = {"debug": True}

app_config = {"max_connections": 200}

config = ChainMap(app_config, env_config, default_config)

print(f"Config: {dict(config)}")
print(f"debug: {config['debug']}")
print(f"timeout: {config['timeout']}")


# ============================================================
# Example 6: Scope/Context Pattern
# ============================================================
print("\n=== Real-World: Scopes ===")

from collections import ChainMap

global_scope = {"__name__": "__main__", "__builtins__": {}}
function_scope = {"x": 10, "y": 20}
local_scope = {"x": 15}

context = ChainMap(local_scope, function_scope, global_scope)
print(f"x: {context['x']}")
print(f"y: {context['y']}")
print(f"__name__: {context['__name__']}")
