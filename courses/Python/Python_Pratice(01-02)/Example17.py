# Example17.py
# Topic: Type Hints - reveal_type() Function

print("=" * 50)
print("TYPE HINTS - reveal_type() FUNCTION")      # TYPE HINTS - reveal_type() FUNCTION
print("=" * 50)
# What Is Reveal Type()?
print("\n--- What is reveal_type()? ---\n")       # \n--- What is reveal_type()? ---\n
    
print("reveal_type() is a special function used with type checkers.")# reveal_type() is a special function used with type checkers.
print("It shows what type Python infers for a variable.")# It shows what type Python infers for a variable.
print("Install mypy: pip install mypy")           # Install mypy: pip install mypy
print("Run: mypy example17.py")                   # Run: mypy example17.py
print("\n--- Examples (run with mypy to see output) ---\n")# \n--- Examples (run with mypy to see output) ---\n
    
x = 42
# reveal_type(x)  # mypy shows: Revealed type is "int"
print("x = " + str(x))
print("(Uncomment reveal_type(x) and run mypy to see the type)")# (Uncomment reveal_type(x) and run mypy to see the type)
    
y = "hello"
# reveal_type(y)  # mypy shows: Revealed type is "str"
print("y = '" + str(y) + "'")                     # y = '" + str(y) + "'
    
z = [1, 2, 3]
# reveal_type(z)  # mypy shows: Revealed type is "list[int]"
print("z = " + str(z))
# With Type Hints
print("\n--- With Type Hints ---\n")              # \n--- With Type Hints ---\n
    
# With hints, reveal_type confirms the type
name = "Alice" # str  — text, always wrapped in quotes
# reveal_type(name)  # mypy shows: Revealed type is "str"
print("name: str = '" + str(name) + "'")          # name: str = '" + str(name) + "'
    
age = 25 # int  — whole number, no quotes
# reveal_type(age)  # mypy shows: Revealed type is "int"
print("age: int = " + str(age))
# Practical Debugging
print("\n--- Practical Debugging ---\n")          # \n--- Practical Debugging ---\n
    
def process_data(data: list[str]) -> list[int]:
    # Without hints, Python infers types
    result = []
    for item in data:
        # What type is item?
        # reveal_type(item)  # Shows: Revealed type is "str"
        result.append(len(item))
    return result
    
output: list[int] = process_data(["hello", "world"])
print("process_data(['hello', 'world']) = " + str(output))
# Type Inference In Expressions
print("\n--- Type Inference in Expressions ---\n")# \n--- Type Inference in Expressions ---\n
    
# What type does this expression have?
a = 10 + 5
# reveal_type(a)  # Shows: Revealed type is "int"
print("a = 10 + 5 = " + str(a))
    
b = 10 + 5.0
# reveal_type(b)  # Shows: Revealed type is "float"
print("b = 10 + 5.0 = " + str(b))
    
c = "hello" + " world"
# reveal_type(c)  # Shows: Revealed type is "str"
print("c = 'hello' + ' world' = '" + str(c) + "'")# c = 'hello' + ' world' = '" + str(c) + "'
# How To Run With Mypy
print("\n--- How to Run with mypy ---\n")         # \n--- How to Run with mypy ---\n
    
print("1. Install mypy:")                         # 1. Install mypy:
print("   pip install mypy")                      #    pip install mypy
print("")                                         # 
print("2. Create a test file with reveal_type():")# 2. Create a test file with reveal_type():
print("   # test_types.py")                       #    # test_types.py
print("   x = 42")                                #    x = 42
print("   reveal_type(x)")                        #    reveal_type(x)
print("")                                         # 
print("3. Run mypy:")                             # 3. Run mypy:
print("   mypy test_types.py")                    #    mypy test_types.py
print("")                                         # 
print("4. Output:")                               # 4. Output:
print("   test_types.py:2: note: Revealed type is 'int'")#    test_types.py:2: note: Revealed type is 'int'
# Summary
print("\n" + "=" * 50)
print("reveal_type() SUMMARY")                    # reveal_type() SUMMARY
print("=" * 50)
print("Key Points:")                              # Key Points:
print("- reveal_type() shows inferred type")      # - reveal_type() shows inferred type
print("- Only works with type checkers (mypy, pyright)")# - Only works with type checkers (mypy, pyright)
print("- Install: pip install mypy")              # - Install: pip install mypy
print("- Run: mypy your_file.py")                 # - Run: mypy your_file.py
print("- Helps debug complex type inference")     # - Helps debug complex type inference

# Real-world example: