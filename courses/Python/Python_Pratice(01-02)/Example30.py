# Example30.py
# Topic: Operators — The Walrus Operator (:=)

# The walrus operator (:=) was introduced in Python 3.8
# It assigns a value to a variable AS PART of an expression
# The name comes from the shape: := looks like walrus eyes and tusks

# Without walrus: calculate first, then use
# Calculate the length, store in variable, then check
result = len("hello")    # int  — length of the string

if result > 3:
    print("Length is " + str(result))  # 5

# With walrus: assign and use in the same expression
# The := assigns and returns the value at the same time
if (result := len("hello")) > 3:
    print("Length is " + str(result))  # 5

# Key difference: walrus lets you calculate AND check in one line
# This is useful when:
# 1. You need a value for both the condition AND the body
# 2. You want to avoid calculating the same thing twice
# 3. You want cleaner, more compact code

# In conditional — assign and test at once
text = "Hello World"

# Without walrus: calculate length twice
if len(text) > 5:
    print("Text: " + text + ", Length: " + str(len(text)))

# With walrus: calculate once
if (length := len(text)) > 5:
    print("Text: " + text + ", Length: " + str(length))

# Using walrus with different operators
# The walrus returns the assigned value, so it works anywhere a value is expected

# In a function call
message = "Hello"                   # str  — a message

# Print uses the assigned value
print("Message:", (message := "Updated"))  # Updated

# In a list (we'll cover lists later, but the concept applies)
# numbers = [1, 2, (x := 3), 4]
# print(x)  # 3

# In a comparison chain
# Walrus assigns and the comparison continues
a = 5                               # int  — first value
b = 10                              # int  — second value

# Both assign and return their values
result = (a := 3) < (b := 7)
print(result)                       # True
print("a is now", a)                # 3  (changed from 5!)
print("b is now", b)                # 7  (changed from 10!)

# Important: walrus CHANGES the variable when it assigns!
# So be careful — the original values are overwritten

# Common use case: caching expensive calculations
# Without walrus: calculate expensively, store result
import time
# expensive_result = some_expensive_function()  # Would take time
# if expensive_result > threshold:
#     process(expensive_result)

# With walrus: you could do it inline (though this is a simplified demo)
value = 10                          # int  — some computed value

# Check and use the same value
if (doubled := value * 2) > 15:
    print("Doubled value exceeds 15:", doubled)

# Real-world example: checking and logging
user_input = "admin"                # str  — user typed this

# Check length and use in message
if (input_length := len(user_input)) > 5:
    print("Long username detected! Length:", input_length)

# Real-world example: extracting and using a substring
full_text = "Price: $99.99"         # str  — text with price

# Extract numeric part and convert
if (colon_pos := full_text.find(":")) != -1:
    price_str = full_text[colon_pos + 1:]
    print("Price substring:", price_str.strip())

# Note: walrus REQUIRES parentheses in many contexts
# This is because of Python's parsing rules
# if x := 5 > 3:   # WRONG — confusing!
# if (x := 5) > 3: # CORRECT — clear intent
