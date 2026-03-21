# Example77.py
# Topic: Exception Handling — ExceptionGroups Basics

# ExceptionGroups (Python 3.11+) handle multiple exceptions at once

# === What is ExceptionGroup? ===
# A way to group multiple exceptions together

# Create an ExceptionGroup manually
eg = ExceptionGroup("Multiple errors", [
    ValueError("Invalid value"),
    TypeError("Wrong type"),
])

print(eg)
print(type(eg))

# === The problem before ExceptionGroups ===
# Before: If one task fails, you lose info about others

# Old way - stops at first error
def process_old(items):
    results = []
    for item in items:
        results.append(item * 2)  # Would fail on error
    return results

# New way - can collect all errors

# === When to use ExceptionGroup ===
# - Running multiple tasks
# - Collecting all errors
# - Parallel/concurrent code

# === Basic structure ===
eg = ExceptionGroup("Task errors", [
    ValueError("Error 1"),
    ValueError("Error 2"),
    TypeError("Error 3"),
])

print(eg.exceptions)  # Get all individual exceptions

# === Check Python version ===
import sys

print("Python version: " + sys.version)

# Note: ExceptionGroup requires Python 3.11+
