# Example23.py
# Topic: python-guide/01_Foundations/03_Operators/01_arithmetic_operators.md

print("=== Operator Precedence Examples ===\n")   # === Operator Precedence Examples ===\n
    
result1 = 2 + 3 * 4 # int  — whole number, no quotes
print("2 + 3 * 4 = " + str(result1))
print("  Explanation: 3 * 4 = 12, then 2 + 12 = 14")#   Explanation: 3 * 4 = 12, then 2 + 12 = 14
print("  (NOT 5 * 4 = 20!)\n")                    #   (NOT 5 * 4 = 20!)\n
    
result2 = (2 + 3) * 4 # int  — whole number, no quotes
print("(2 + 3) * 4 = " + str(result2))
print("  Explanation: 2 + 3 = 5, then 5 * 4 = 20\n")#   Explanation: 2 + 3 = 5, then 5 * 4 = 20\n
    
result3 = 10 + 20 / 2 # float — decimal number
print("10 + 20 / 2 = " + str(result3))
print("  Explanation: 20 / 2 = 10, then 10 + 10 = 20.0")#   Explanation: 20 / 2 = 10, then 10 + 10 = 20.0
print("  (NOT 30 / 2 = 15!)\n")                   #   (NOT 30 / 2 = 15!)\n
    
result4 = (10 + 20) / 2 # float — decimal number
print("(10 + 20) / 2 = " + str(result4))
print("  Explanation: 10 + 20 = 30, then 30 / 2 = 15.0 (correct average)\n")#   Explanation: 10 + 20 = 30, then 30 / 2 = 15.0 (correct average)\n
    
result5 = 2 ** 3 * 2 # int  — whole number, no quotes
print("2 ** 3 * 2 = " + str(result5))
print("  Explanation: 2 ** 3 = 8, then 8 * 2 = 16\n")#   Explanation: 2 ** 3 = 8, then 8 * 2 = 16\n
    
result6 = (10 - 2) ** 2 / 4 # float — decimal number
print("(10 - 2) ** 2 / 4 = " + str(result6))
print("  Explanation:")                           #   Explanation:
print("    Step 1: 10 - 2 = 8")                   #     Step 1: 10 - 2 = 8
print("    Step 2: 8 ** 2 = 64")                  #     Step 2: 8 ** 2 = 64
print("    Step 3: 64 / 4 = 16.0\n")              #     Step 3: 64 / 4 = 16.0\n
# = Precedence Table =
    
print("=== Precedence Table ===")                 # === Precedence Table ===
print("1. Parentheses: ()")                       # 1. Parentheses: ()
print("2. Exponentiation: **")                    # 2. Exponentiation: **
print("3. Unary: - (negation)")                   # 3. Unary: - (negation)
print("4. Multiplication/Division: *, /, //, %")  # 4. Multiplication/Division: *, /, //, %
print("5. Addition/Subtraction: +, -\n")          # 5. Addition/Subtraction: +, -\n
# = Common Mistakes =
    
print("=== Common Mistakes ===\n")                # === Common Mistakes ===\n
    
# WRONG: Forgetting precedence
wrong_avg = 10 + 20 / 2  # This equals 20.0, not 15! # float — decimal number
correct_avg = (10 + 20) / 2  # This equals 15.0 # float — decimal number
print("WRONG average: 10 + 20 / 2 = " + str(wrong_avg) + " (should be 15.0!)")# WRONG average: 10 + 20 / 2 = " + str(wrong_avg) + " (should be 15.0!)
print("CORRECT average: (10 + 20) / 2 = " + str(correct_avg) + "\n")# CORRECT average: (10 + 20) / 2 = " + str(correct_avg) + "\n
    
# Another common mistake
wrong_order = 2 + 3 * 4 ** 2 # int  — whole number, no quotes
correct_order = 2 + (3 * (4 ** 2)) # int  — whole number, no quotes
print("2 + 3 * 4 ** 2 = " + str(wrong_order))
print("  Explanation: 4 ** 2 = 16, then 3 * 16 = 48, then 2 + 48 = 50")#   Explanation: 4 ** 2 = 16, then 3 * 16 = 48, then 2 + 48 = 50
print("  (Using parentheses): 2 + 3 * (4 ** 2) = " + str(correct_order) + "\n")#   (Using parentheses): 2 + 3 * (4 ** 2) = " + str(correct_order) + "\n
    
# Negative numbers and precedence
result_neg = -5 ** 2  # What happens here? # int  — whole number, no quotes
result_neg_correct = (-5) ** 2 # int  — whole number, no quotes
print("-5 ** 2 = " + str(result_neg))  # Exponent has higher precedence than negation!
print("(-5) ** 2 = " + str(result_neg_correct))
print("  Note: ** binds tighter than unary -, so -5**2 = -(5**2) = -25\n")#   Note: ** binds tighter than unary -, so -5**2 = -(5**2) = -25\n
    
# Using parentheses makes it clear
result_clear = -(5 ** 2) # int  — whole number, no quotes
print("-(5 ** 2) = " + str(result_clear))
print("  This explicitly shows negation after exponentiation")#   This explicitly shows negation after exponentiation

# Real-world example: