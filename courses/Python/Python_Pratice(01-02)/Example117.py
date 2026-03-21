# Example117.py
# Topic: Iteration Tools — Practical Uses and Differences

# Practical uses and key differences between any() and all()

# === any() vs all() - basic comparison ===
numbers = [1, 2, 3, 4, 5]

print("any (any is truthy?): " + str(any(numbers)))  # True (all are truthy)
print("all (all are truthy?): " + str(all(numbers)))  # True

numbers = [0, 2, 3, 4, 5]
print("\nWith zero:")
print("any: " + str(any(numbers)))  # True (some are truthy)
print("all: " + str(all(numbers)))  # False (zero is falsy)

# === Practical: Form validation ===
form_data = {"name": "Alice", "email": "alice@example.com", "age": 25}

# Check if all required fields are present
required = ["name", "email", "phone"]
print("\nForm validation:")
print("All required present: " + str(all(field in form_data for field in required)))

# Check if any optional field is missing
optional = ["phone", "twitter", "linkedin"]
print("Any optional present: " + str(any(field in form_data for field in optional)))

# === Practical: Permission checking ===
user = {"role": "admin", "active": True, "verified": False}

# Admin needs specific permissions
permissions = ["read", "write", "delete"]
user_permissions = ["read", "write"]

print("\nPermission check:")
print("Has all permissions: " + str(all(p in user_permissions for p in permissions)))
print("Has any permission: " + str(any(p in user_permissions for p in permissions)))

# === Practical: List comprehension equivalent ===
numbers = [1, 2, 3, 4, 5]

# any equivalent: any(x > 10 for x in numbers)
any_result = any(x > 10 for x in numbers)
any_comp = any([x > 10 for x in numbers])

# all equivalent: all(x > 0 for x in numbers)
all_result = all(x > 0 for x in numbers)
all_comp = all([x > 0 for x in numbers])

print("\nEquivalent forms:")
print("any(x > 10): " + str(any_result))
print("all(x > 0): " + str(all_result))

# === Combining any and all ===
scores = [85, 90, 78, 92, 88]

# Check if anyone got perfect score
print("\nCombining:")
print("Any perfect (100): " + str(any(s == 100 for s in scores)))

# Check if everyone passed (>= 60)
print("All passed: " + str(all(s >= 60 for s in scores)))

# Check if any failed and not all passed
has_failures = any(s < 60 for s in scores)
all_passed = all(s >= 60 for s in scores)
print("Mixed: has failures=" + str(has_failures) + ", all passed=" + str(all_passed))

# === Practical: Data validation ===
records = [
    {"name": "Alice", "email": "alice@example.com"},
    {"name": "Bob", "email": "bob@example.com"},
    {"name": "Carol", "email": "carol@example.com"}
]

# Check if all have valid emails
print("\nData validation:")
print("All have @: " + str(all("@" in r["email"] for r in records)))

# Check if any have .com
print("Any .com: " + str(any(".com" in r["email"] for r in records)))

# Check if any have .org
print("Any .org: " + str(any(".org" in r["email"] for r in records)))

# === Practical: File type checking ===
files = ["document.pdf", "image.jpg", "video.mp4", "audio.wav"]

# Check if all are images
all_images = all(f.endswith((".jpg", ".png", ".gif")) for f in files)
print("\nFile checks:")
print("All images: " + str(all_images))

# Check if any are videos
any_video = any(f.endswith((".mp4", ".avi", ".mov")) for f in files)
print("Any video: " + str(any_video))

# Check if any is PDF
any_pdf = any(f.endswith(".pdf") for f in files)
print("Any PDF: " + str(any_pdf))

# === Short-circuit behavior demonstration ===
def slow_check(x):
    print("Checking: " + str(x))
    return x > 10

values = [1, 5, 15, 20]

print("\nShort-circuit (any):")
result = any(slow_check(v) for v in values)
print("Result: " + str(result))

print("\nShort-circuit (all):")
result = all(slow_check(v) for v in values)
print("Result: " + str(result))

# === any() and all() with empty lists ===
print("\nEmpty list behavior:")
print("any([]): " + str(any([])))  # False
print("all([]): " + str(all([])))  # True (vacuous truth!)

# === Best use cases ===
print("\n=== When to use each ===")
print("Use any(): When you need at least ONE item to match")
print("Use all(): When you need EVERY item to match")
