# Example1.py
# Topic: Topic

name = "World" # str  — text, always wrapped in quotes
    
# Use f-string (formatted string literal) to create a greeting
message = "Hello, " + str(name) + "! Welcome to Python 3.12+" # str  — text, always wrapped in quotes
    
# Print the message to the console
print(message)
    
# Demonstrate f-string with multiple variables
version = 3      # Major version number # int  — whole number, no quotes
minor = 12       # Minor version number # int  — whole number, no quotes
    
# f-strings can include expressions inside {}
print("You're learning Python " + str(version) + "." + str(minor) + "+")# You're learning Python " + str(version) + "." + str(minor) + "+

# Real-world example: