# Example272: Regular Expressions Basics
import re

# Basic patterns
print("Regular Expressions Basics:")

# Match
pattern = r"\d+"  # Match digits
text = "There are 123 apples and 456 oranges"
match = re.search(pattern, text)
print(f"Search for digits: {match.group()}")

# Find all
print(f"Find all digits: {re.findall(pattern, text)}")

# Match at start
pattern = r"\w+"  # Match word
print(f"Match at start: {re.match(r'There', text).group()}")

# Replace
print(f"Replace digits with #: {re.sub(r'\d+', '#', text)}")

# Split
text = "apple,banana;cherry|orange"
parts = re.split(r'[,;|]', text)
print(f"Split: {parts}")

# Character classes
print("\nCharacter classes:")
print(f"\d (digits): {re.findall(r'\d', 'abc123def')}")
print(f"\w (word): {re.findall(r'\w', 'hello world!')}")
print(f"\s (space): {re.findall(r'\s', 'hello world')}")

# Quantifiers
print("\nQuantifiers:")
text = "aaa bb c"
print(f"* (0 or more): {re.findall(r'a*', text)}")
print(f"+ (1 or more): {re.findall(r'a+', text)}")
print(f"? (0 or 1): {re.findall(r'colou?r', 'color colour')}")

# Groups
print("\nGroups:")
pattern = r'(\w+)@(\w+)\.(\w+)'
email = "user@example.com"
match = re.match(pattern, email)
print(f"Full: {match.group()}")
print(f"Username: {match.group(1)}")
print(f"Domain: {match.group(2)}")

# Named groups
pattern = r'(?P<user>\w+)@(?P<domain>\w+)\.(?P<tld>\w+)'
match = re.match(pattern, 'user@example.com')
print(f"Named - user: {match.group('user')}")
print(f"Named - domain: {match.group('domain')}")

# Flags
print("\nFlags:")
text = "Hello\nWorld"
print(f"IGNORECASE: {re.findall(r'hello', text, re.IGNORECASE)}")
print(f"MULTILINE: {re.findall(r'^W', text, re.MULTILINE)}")
