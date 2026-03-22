# Example102.py
# Topic: Real-World Shopping Cart with Lists

# This file demonstrates a practical shopping cart implementation.


# ============================================================
# Example 1: Shopping Cart Class
# ============================================================
print("=== Shopping Cart Class ===")

class ShoppingCart:
    def __init__(self):
        self.items = []
    
    def add_item(self, name, price, qty=1):
        # Check if item exists
        for item in self.items:
            if item["name"] == name:
                item["qty"] += qty
                return
        self.items.append({"name": name, "price": price, "qty": qty})
    
    def remove_item(self, name):
        self.items = [i for i in self.items if i["name"] != name]
    
    def update_qty(self, name, qty):
        for item in self.items:
            if item["name"] == name:
                item["qty"] = qty
                return
    
    def get_total(self):
        return sum(item["price"] * item["qty"] for item in self.items)
    
    def __str__(self):
        lines = ["Shopping Cart:"]
        for item in self.items:
            lines.append(f"  {item['name']}: ${item['price']} x {item['qty']}")
        lines.append(f"Total: ${self.get_total():.2f}")
        return "\n".join(lines)

cart = ShoppingCart()
cart.add_item("Apple", 0.50, 4)
cart.add_item("Banana", 0.25, 6)
cart.add_item("Orange", 0.75, 3)
cart.add_item("Apple", 0.50, 2)  # Adds to existing

print(cart)

cart.update_qty("Banana", 10)
cart.remove_item("Orange")
print(cart)


# ============================================================
# Example 2: Inventory Management
# ============================================================
print("\n=== Inventory Management ===")

class Inventory:
    def __init__(self):
        self.stock = []
    
    def add_product(self, name, qty, price):
        self.stock.append({
            "name": name,
            "qty": qty,
            "price": price
        })
    
    def find_product(self, name):
        for p in self.stock:
            if p["name"] == name:
                return p
        return None
    
    def update_stock(self, name, qty):
        product = self.find_product(name)
        if product:
            product["qty"] = qty
    
    def get_low_stock(self, threshold=5):
        return [p for p in self.stock if p["qty"] <= threshold]
    
    def get_total_value(self):
        return sum(p["qty"] * p["price"] for p in self.stock)

inventory = Inventory()
inventory.add_product("Apple", 50, 0.50)
inventory.add_product("Banana", 100, 0.25)
inventory.add_product("Orange", 3, 0.75)

print(f"Low stock: {[p['name'] for p in inventory.get_low_stock()]}")
print(f"Total value: ${inventory.get_total_value():.2f}")


# ============================================================
# Example 3: To-Do List
# ============================================================
print("\n=== To-Do List ===")

tasks = []

def add_task(title, priority=1):
    tasks.append({
        "title": title,
        "priority": priority,
        "done": False,
        "id": len(tasks) + 1
    })

def complete_task(task_id):
    for task in tasks:
        if task["id"] == task_id:
            task["done"] = True

def get_pending():
    return [t for t in tasks if not t["done"]]

def get_by_priority():
    return sorted(tasks, key=lambda t: t["priority"], reverse=True)

add_task("Buy groceries", 3)
add_task("Clean house", 1)
add_task("Pay bills", 2)

print(f"Pending: {[t['title'] for t in get_pending()]}")
print(f"By priority: {[t['title'] for t in get_by_priority()]}")

complete_task(1)
print(f"After completing task 1: {[t['title'] for t in get_pending()]}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("REAL-WORLD LIST PATTERNS")
print("=" * 50)
print("""
Shopping Cart:
- Add/update/remove items
- Calculate totals
- Track quantities

Inventory:
- Track stock levels
- Find products
- Low stock alerts

To-Do List:
- Add tasks with priority
- Mark complete
- Filter by status
""")
