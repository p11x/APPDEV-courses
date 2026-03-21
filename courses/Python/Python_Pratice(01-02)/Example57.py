# Example57.py
# Topic: Loops — While Loop Practice Examples

# More practical while loop examples

# === Example 1: Fibonacci sequence ===
print("Fibonacci sequence:")
a = 0
b = 1
count = 0

while count < 10:
    print(a)
    temp = a + b
    a = b
    b = temp
    count += 1

# 0, 1, 1, 2, 3, 5, 8, 13, 21, 34

# === Example 2: Reverse a number ===
print("\nReverse a number:")
number = 12345
reversed_num = 0

while number > 0:
    digit = number % 10
    reversed_num = reversed_num * 10 + digit
    number = number // 10

print("Reversed: " + str(reversed_num))

# === Example 3: Sum of digits ===
print("\nSum of digits:")
number = 12345
sum_digits = 0

while number > 0:
    digit = number % 10
    sum_digits += digit
    number = number // 10

print("Sum: " + str(sum_digits))

# === Example 4: Find factors ===
print("\nFactors of 12:")
n = 12
divisor = 1

while divisor <= n:
    if n % divisor == 0:
        print(divisor)
    divisor += 1
# 1, 2, 3, 4, 6, 12

# === Example 5: Power calculation ===
print("\n2^10:")
base = 2
exponent = 10
result = 1

while exponent > 0:
    result *= base
    exponent -= 1

print("2^10 = " + str(result))

# === Example 6: Factorial ===
print("\n5! (factorial):")
n = 5
factorial = 1

while n > 0:
    factorial *= n
    n -= 1

print("5! = " + str(factorial))

# === Example 7: Guessing game ===
print("\nGuessing game:")
secret = 7
guess = 0
attempts = 0
max_attempts = 3
found = False

while attempts < max_attempts and not found:
    attempts += 1
    guess = secret  # Simulated correct guess
    
    if guess == secret:
        found = True
        print("You got it in " + str(attempts) + " attempts!")
    else:
        print("Wrong guess, try again")

if not found:
    print("Out of attempts!")

# === Example 8: Binary search simulation ===
print("\nBinary search:")
sorted_list = [1, 3, 5, 7, 9, 11, 13, 15]
target = 7
left = 0
right = len(sorted_list) - 1
found = False

while left <= right:
    mid = (left + right) // 2
    
    if sorted_list[mid] == target:
        found = True
        print("Found at index " + str(mid))
        break
    elif sorted_list[mid] < target:
        left = mid + 1
    else:
        right = mid - 1

if not found:
    print("Not found")

# === Example 9: LCM calculation ===
print("\nLCM of 12 and 18:")
a = 12
b = 18
lcm = max(a, b)

while lcm % a != 0 or lcm % b != 0:
    lcm += 1

print("LCM = " + str(lcm))

# === Example 10: GCD calculation ===
print("\nGCD of 48 and 18:")
x = 48
y = 18

while y != 0:
    temp = y
    y = x % y
    x = temp

print("GCD = " + str(x))

# === Example 11: Countdown with condition ===
print("\nCountdown until zero:")
current = 10
target = 5

while current > target:
    print(current)
    current -= 1

print("Reached target: " + str(current))

# === Example 12: Running sum ===
print("\nRunning sum:")
numbers = [1, 2, 3, 4, 5]
index = 0
running_sum = 0

while index < len(numbers):
    running_sum += numbers[index]
    print("Added " + str(numbers[index]) + ", sum = " + str(running_sum))
    index += 1

print("Total: " + str(running_sum))

# === Example 13: Remove duplicates (simple) ===
print("\nRemove duplicates:")
items = [1, 2, 2, 3, 3, 3, 4, 5, 5]
unique = []
index = 0

while index < len(items):
    item = items[index]
    if item not in unique:
        unique.append(item)
    index += 1

print("Unique: " + str(unique))
