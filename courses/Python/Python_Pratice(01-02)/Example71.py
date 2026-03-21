# Example71.py
# Topic: Exception Handling — Common Mistakes

# Common mistakes to avoid

# === MISTAKE 1: Using Bare Except ===

# WRONG - catches everything including KeyboardInterrupt
# try:
#     risky_code()
# except:
#     print("Error")

# CORRECT - catch specific exceptions
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Can't divide by zero")

# CORRECT - catch multiple specific
try:
    value = int("abc")
except (ValueError, TypeError):
    print("Invalid value")

# === MISTAKE 2: Swallowing Exceptions ===

# WRONG - silently ignoring errors
try:
    do_something()
except Exception:
    pass  # Bad! We don't know what went wrong

# CORRECT - at least log the error
try:
    do_something()
except Exception as e:
    print("Error occurred: " + str(e))

# === MISTAKE 3: Not Using Finally for Cleanup ===

# WRONG - file might not close if error occurs
# file = open("data.txt")
# try:
#     process(file)
# finally:
#     file.close()

# CORRECT - use finally
# try:
#     file = open("data.txt")
#     content = file.read()
# finally:
#     file.close()  # Always runs!

# === MISTAKE 4: Wrong Exception Order ===

# WRONG - more general first
# except Exception catches everything!
# except ZeroDivisionError:  # Never reached!
#     ...

# CORRECT - specific first
try:
    result = 10 / 0
except ZeroDivisionError:
    print("Specific error")
except Exception:
    print("General error")

# === MISTAKE 5: Catching without handling ===

# WRONG
# try:
#     result = risky_operation()
# except:
#     pass  # Do nothing

# CORRECT - handle properly
try:
    result = risky_operation()
except SpecificError:
    handle_error()
    # or re-raise
    raise

# === MISTAKE 6: Using exceptions for flow control ===

# WRONG - exceptions shouldn't control normal flow
# try:
#     value = data["key"]
# except KeyError:
#     value = default  # This is slow!

# CORRECT - use .get()
# value = data.get("key", default)

# === MISTAKE 7: Not understanding what else does ===

# WRONG - thinking else runs after except
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Error")
else:
    print("No error")  # This DOES run!

# === Best Practices ===

# 1. Catch specific exceptions
# 2. Use finally for cleanup
# 3. Don't swallow exceptions silently
# 4. Order exceptions from specific to general
# 5. Use .get() for dict, in for list when possible
# 6. Log errors for debugging

# === Good pattern ===
def safe_divide(a, b):
    try:
        return a / b
    except ZeroDivisionError:
        return None
    except TypeError:
        return None
    finally:
        print("Division attempted")

print(safe_divide(10, 2))
print(safe_divide(10, 0))
