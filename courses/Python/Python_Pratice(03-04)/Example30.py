# Example30.py
# Topic: *args - Variable Positional Arguments

# This file demonstrates the use of *args to accept any number of
# positional arguments, which are packed into a tuple.


# Returns the sum of all provided numbers
def sum_all(*args: int) -> int:
    total = 0
    for num in args:
        total += num
    return total

result = sum_all(1, 2, 3, 4, 5)    # int — sum of all arguments
print("Sum of 1-5: " + str(result))    # Sum of 1-5: 15


# Returns the product of all provided numbers
def multiply_all(*args: float) -> float:
    if not args:
        return 1.0
    product = 1.0
    for num in args:
        product *= num
    return product

result = multiply_all(2, 3, 4)    # float — product of all arguments
print("Product of 2, 3, 4: " + str(result))    # Product of 2, 3, 4: 24.0


# Returns the average of all provided numbers
def calculate_average(*args: float) -> float:
    if not args:
        return 0.0
    return sum(args) / len(args)

avg = calculate_average(10, 20, 30, 40)    # float — average of arguments
print("Average: " + str(avg))    # Average: 25.0


# Returns the maximum value from all provided arguments
def find_maximum(*args: int) -> int:
    if not args:
        return 0
    return max(args)

max_val = find_maximum(5, 15, 10, 25, 20)    # int — maximum value
print("Maximum: " + str(max_val))    # Maximum: 25


# Returns all arguments as a concatenated string
def concatenate_strings(*args: str) -> str:
    return " ".join(args)

result = concatenate_strings("Hello", "World", "from", "Python")    # str — concatenated
print("Concatenated: " + result)    # Concatenated: Hello World from Python


# Returns the count of each unique element
def count_items(*args: object) -> dict:
    counts = {}
    for item in args:
        counts[item] = counts.get(item, 0) + 1
    return counts

counts = count_items("apple", "banana", "apple", "cherry", "banana", "apple")
print("Item counts: " + str(counts))    # {'apple': 3, 'banana': 2, 'cherry': 1}


# Returns True if all arguments are even numbers
def all_even(*args: int) -> bool:
    return all(num % 2 == 0 for num in args)

print("All even (2,4,6): " + str(all_even(2, 4, 6)))    # True
print("All even (2,3,4): " + str(all_even(2, 3, 4)))    # False


# Returns True if any argument is positive
def any_positive(*args: int) -> bool:
    return any(num > 0 for num in args)

print("Any positive (1,-2,3): " + str(any_positive(1, -2, 3)))    # True
print("Any positive (-1,-2,-3): " + str(any_positive(-1, -2, -3)))    # False


# Returns a formatted string with all arguments
def format_list(*args: str) -> str:
    if len(args) == 0:
        return "No items"
    if len(args) == 1:
        return str(args[0])
    result = ", ".join(str(arg) for arg in args[:-1])
    result += " and " + str(args[-1])
    return result

print("Format list: " + format_list("apple", "banana", "cherry"))    # apple, banana and cherry
print("Format list: " + format_list("only one"))    # only one


# Returns the factorial of all numbers provided
def factorials(*args: int) -> list[int]:
    results = []
    for n in args:
        if n < 0:
            results.append(0)
        elif n <= 1:
            results.append(1)
        else:
            fact = 1
            for i in range(2, n + 1):
                fact *= i
            results.append(fact)
    return results

facts = factorials(5, 3, 7, 0)
print("Factorials: " + str(facts))    # [120, 6, 5040, 1]


# Real-life Example 1: Calculate total order price with multiple items
def calculate_order_total(*prices: float) -> float:
    subtotal = sum(prices)
    tax = subtotal * 0.08
    shipping = 5.99 if subtotal < 50 else 0
    return subtotal + tax + shipping

total = calculate_order_total(29.99, 49.99, 15.00)
print(f"Order Total: ${total:.2f}")    # Order Total: $102.39


# Real-life Example 2: Calculate average rating from multiple reviews
def calculate_average_rating(*ratings: float) -> dict:
    if not ratings:
        return {"average": 0.0, "count": 0, "total": 0}
    return {
        "average": sum(ratings) / len(ratings),
        "count": len(ratings),
        "total": sum(ratings)
    }

result = calculate_average_rating(4.5, 4.0, 5.0, 3.5, 4.5)
print(f"Rating: {result['average']:.1f}/5.0 from {result['count']} reviews")
# Rating: 4.3/5.0 from 5 reviews


# Real-life Example 3: Find the range of temperatures
def temperature_range(*temps: float) -> dict:
    if not temps:
        return {"min": None, "max": None, "range": None}
    return {
        "min": min(temps),
        "max": max(temps),
        "range": max(temps) - min(temps)
    }

result = temperature_range(72, 75, 68, 71, 74, 69)
print(f"Temperature: Low {result['min']}°F, High {result['max']}°F, Range {result['range']}°F")
# Temperature: Low 68.0°F, High 75.0°F, Range 7.0°F


# Real-life Example 4: Sum multiple donations
def total_donations(*amounts: float) -> dict:
    total = sum(amounts)
    count = len(amounts)
    average = total / count if count > 0 else 0
    return {
        "total": total,
        "count": count,
        "average": average,
        "largest": max(amounts) if amounts else 0
    }

donations = total_donations(100.0, 250.0, 50.0, 500.0, 75.0)
print(f"Total: ${donations['total']}, Count: {donations['count']}, Avg: ${donations['average']:.2f}")
# Total: $975.0, Count: 5, Avg: $195.00


# Real-life Example 5: Calculate cumulative GPA
def calculate_gpa(*grades: float) -> dict:
    if not grades:
        return {"gpa": 0.0, "credits": 0}
    
    grade_points = []
    for g in grades:
        if g >= 93:
            grade_points.append(4.0)
        elif g >= 90:
            grade_points.append(3.7)
        elif g >= 87:
            grade_points.append(3.3)
        elif g >= 83:
            grade_points.append(3.0)
        elif g >= 80:
            grade_points.append(2.7)
        elif g >= 77:
            grade_points.append(2.3)
        elif g >= 73:
            grade_points.append(2.0)
        elif g >= 70:
            grade_points.append(1.7)
        elif g >= 67:
            grade_points.append(1.3)
        elif g >= 63:
            grade_points.append(1.0)
        elif g >= 60:
            grade_points.append(0.7)
        else:
            grade_points.append(0.0)
    
    gpa = sum(grade_points) / len(grade_points)
    return {"gpa": gpa, "courses": len(grades)}

result = calculate_gpa(95, 88, 92, 78, 85)
print(f"GPA: {result['gpa']:.2f} from {result['courses']} courses")    # GPA: 3.56 from 5 courses


# Real-life Example 6: Combine multiple shipping boxes weights
def calculate_shipping_weight(*weights: float) -> dict:
    total_weight = sum(weights)
    is_oversized = total_weight > 100
    return {
        "total_weight": total_weight,
        "box_count": len(weights),
        "per_box_avg": total_weight / len(weights) if weights else 0,
        "oversized": is_oversized,
        "shipping_cost": 0 if total_weight <= 50 else (total_weight - 50) * 0.5
    }

shipping = calculate_shipping_weight(10.5, 15.2, 8.3, 22.0)
print(f"Total Weight: {shipping['total_weight']} lbs, Boxes: {shipping['box_count']}, Cost: ${shipping['shipping_cost']:.2f}")
# Total Weight: 56.0 lbs, Boxes: 4, Cost: $3.00
