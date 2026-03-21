# Example69.py
# Topic: Exception Handling — The Finally Block

# The finally block ALWAYS runs, whether there's an exception or not

# === Basic finally ===
try:
    result = 10 / 2
    print("Result: " + str(result))
except ZeroDivisionError:
    print("Can't divide by zero")
finally:
    print("This always runs")
# Output: Result: 5.0
#         This always runs

# === With error ===
try:
    result = 10 / 0
    print("Result: " + str(result))
except ZeroDivisionError:
    print("Can't divide by zero")
finally:
    print("This always runs")
# Output: Can't divide by zero
#         This always runs

# === Real-world: File handling ===
# finally ensures file is closed
# try:
#     f = open("file.txt", "r")
#     content = f.read()
# except FileNotFoundError:
#     print("File not found")
# finally:
#     f.close()  # Always closes file!

# === Real-world: Database connection ===
# try:
#     conn = connect_to_db()
#     result = conn.query("SELECT * FROM users")
# except DatabaseError:
#     print("Database error")
# finally:
#     conn.close()  # Always closes connection!

# === Practical: Timer ===
import time

def measure_time():
    start = time.time()
    
    try:
        # Simulate some work
        result = 10 / 2
    except ZeroDivisionError:
        print("Error!")
    finally:
        end = time.time()
        print("Time elapsed: " + str(end - start))

measure_time()

# === Cleanup pattern ===
def process_data(data):
    resource = None
    try:
        resource = acquire_resource()
        result = process(resource, data)
        return result
    except ProcessingError:
        return "Processing failed"
    finally:
        if resource:
            release_resource(resource)

# === Multiple return statements ===
def example():
    try:
        return "from try"
    finally:
        print("Finally runs even before return")

result = example()
print(result)
# Output: Finally runs even before return
#         from try

# === Try/except/else/finally all together ===
try:
    result = 10 / 2
except ZeroDivisionError:
    print("Error")
else:
    print("Success: " + str(result))
finally:
    print("Cleanup")

# === Use cases for finally ===
# - Closing files
# - Releasing locks
# - Closing network connections
# - Cleaning up resources
# - Logging
