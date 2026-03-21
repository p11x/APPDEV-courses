# Example56.py
# Topic: Loops — Input Validation

# While loops are commonly used for input validation
# Keep asking until user provides valid input

# === Basic input validation ===
print("Enter your age:")

# Simulated inputs (in real code, would use input())
test_inputs = ["25", "-5", "abc", "30"]
input_index = 0

def get_next_input():
    global input_index
    if input_index < len(test_inputs):
        val = test_inputs[input_index]
        input_index += 1
        return val
    return "quit"

age_valid = False

while not age_valid:
    user_input = get_next_input()
    
    # Try to convert to integer
    try:
        age = int(user_input)
        
        # Check if age is valid
        if age < 0:
            print("Age cannot be negative. Try again.")
        elif age > 150:
            print("That's unrealistic. Try again.")
        else:
            print("Valid age: " + str(age))
            age_valid = True
            
    except ValueError:
        print("'" + user_input + "' is not a number. Try again.")

# === Menu choice validation ===
print("\nMenu selection:")

valid_choices = ["A", "B", "C", "D"]
choice = ""

while choice not in valid_choices:
    choice = "B"  # Simulated input
    
    if choice not in valid_choices:
        print("Invalid choice. Must be A, B, C, or D.")

print("You chose: " + choice)

# === Number range validation ===
print("\nEnter a number between 1 and 100:")

number = 0
user_inputs = ["150", "50", "-10", "75"]
input_idx = 0

def get_test_input():
    global input_idx
    if input_idx < len(user_inputs):
        val = user_inputs[input_idx]
        input_idx += 1
        return val
    return "quit"

while number < 1 or number > 100:
    user_input = get_test_input()
    
    try:
        number = int(user_input)
        
        if number < 1:
            print("Number must be at least 1.")
        elif number > 100:
            print("Number must be at most 100.")
        else:
            print("Valid number: " + str(number))
            
    except ValueError:
        print("Please enter a valid number.")

# === Email validation (simple) ===
print("\nEnter email:")

email = ""
test_emails = ["test", "user@email.com", "invalid@", "admin@domain.com"]
email_idx = 0

def get_email_input():
    global email_idx
    if email_idx < len(test_emails):
        val = test_emails[email_idx]
        email_idx += 1
        return val
    return "quit"

while "@" not in email or "." not in email:
    email = get_email_input()
    
    if email == "quit":
        print("Cancelled.")
        break
    
    if "@" not in email or "." not in email:
        print("Invalid email format. Try again.")

if email != "quit":
    print("Valid email: " + email)

# === Password validation ===
print("\nCreate password:")

password = ""
test_passwords = ["short", "123456", "ValidPass1", "MySecure!Pass"]
pw_idx = 0

def get_pw_input():
    global pw_idx
    if pw_idx < len(test_passwords):
        val = test_passwords[pw_idx]
        pw_idx += 1
        return val
    return "quit"

password_valid = False

while not password_valid:
    password = get_pw_input()
    
    if password == "quit":
        print("Cancelled.")
        break
    
    # Check requirements
    errors = []
    
    if len(password) < 8:
        errors.append("at least 8 characters")
    if not any(c.isupper() for c in password):
        errors.append("at least one uppercase letter")
    if not any(c.isdigit() for c in password):
        errors.append("at least one number")
    
    if errors:
        print("Password invalid: needs " + ", ".join(errors))
    else:
        print("Password valid!")
        password_valid = True

# === Yes/No confirmation ===
print("\nConfirm action (yes/no):")

yes_no = ""
valid_yes_no = ["yes", "no", "y", "n"]
test_confirm = ["maybe", "yes"]
confirm_idx = 0

def get_confirm_input():
    global confirm_idx
    if confirm_idx < len(test_confirm):
        val = test_confirm[confirm_idx]
        confirm_idx += 1
        return val
    return "no"

while yes_no not in valid_yes_no:
    yes_no = get_confirm_input()
    
    if yes_no not in valid_yes_no:
        print("Please answer yes or no.")

if yes_no in ["yes", "y"]:
    print("Confirmed!")
else:
    print("Cancelled.")
