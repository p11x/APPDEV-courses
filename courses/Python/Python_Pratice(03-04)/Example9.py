# Example9.py
# Topic: Using *args for Variable Positional Arguments

# This file demonstrates how to use *args to create functions that can
# accept any number of positional arguments, collected into a tuple.


# Prints each argument passed to the function, one per line
def print_all(*args) -> None:
    for arg in args:
        print(arg)

# Call with different numbers of arguments to see flexibility
print_all(1, 2, 3)
print_all("a", "b")
print_all(1, "hello", True)


# Greets multiple names with a given greeting
def greet(*names) -> None:
    for name in names:
        print("Hello, " + name + "!")

greet("Alice", "Bob", "Charlie")


# Sums all numbers passed to the function
def sum_all(*numbers) -> int:
    total = 0
    for num in numbers:
        total = total + num
    return total

result = sum_all(1, 2, 3, 4, 5)
print("Sum: " + str(result))

result2 = sum_all(10, 20)
print("Sum: " + str(result2))

result3 = sum_all()
print("Sum: " + str(result3))


# Returns the largest of all numbers passed
def find_max(*numbers) -> int:
    if not numbers:
        return 0
    max_val = numbers[0]
    for num in numbers:
        if num > max_val:
            max_val = num
    return max_val

max_num = find_max(3, 1, 9, 5, 2)
print("Max: " + str(max_num))

max_num2 = find_max(-5, -1, -10)
print("Max: " + str(max_num2))


# Returns the average of all numbers passed
def calculate_average(*numbers) -> float:
    if not numbers:
        return 0.0
    total = 0
    for num in numbers:
        total = total + num
    return total / len(numbers)

avg = calculate_average(10, 20, 30, 40)
print("Average: " + str(avg))


# Counts how many arguments were passed
def count_args(*args) -> int:
    return len(args)

count = count_args(1, 2, 3, 4, 5)
print("Count: " + str(count))

count2 = count_args("a", "b")
print("Count: " + str(count2))


# Combines all strings into one
def concatenate(*strings) -> str:
    result = ""
    for s in strings:
        result = result + s
    return result

combined = concatenate("Hello", " ", "World", "!")
print("Combined: " + combined)


# Returns all arguments as a list
def to_list(*args) -> list:
    return list(args)

items = to_list(1, 2, 3)
print("List: " + str(items))

items2 = to_list("first", "second")
print("List: " + str(items2))


# Multiplies all numbers together
def multiply_all(*numbers) -> int:
    if not numbers:
        return 0
    result = 1
    for num in numbers:
        result = result * num
    return result

product = multiply_all(2, 3, 4)
print("Product: " + str(product))

product2 = multiply_all(5, 2)
print("Product: " + str(product2))


# Checks if all numbers are even
def all_even(*numbers) -> bool:
    for num in numbers:
        if num % 2 != 0:
            return False
    return True

print("All even: " + str(all_even(2, 4, 6)))
print("All even: " + str(all_even(2, 3, 4)))


# Joins strings with a separator
def join_with(separator, *strings) -> str:
    return separator.join(strings)

joined = join_with("-", "apple", "banana", "cherry")
print("Joined: " + joined)
