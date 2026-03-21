# Example11.py
# Topic: Combining *args and **kwargs

# This file demonstrates how to use both *args and **kwargs together
# in the same function to accept any combination of positional and keyword arguments.
# This provides maximum flexibility in function design.


# Prints both positional arguments (as a tuple) and keyword arguments (as a dictionary)
def print_all(*args, **kwargs) -> None:
    print("Positional arguments:")    # label for positional args
    for arg in args:    # iterate through the tuple
        print("  " + str(arg))    # print each positional arg
    print("Keyword arguments:")    # label for keyword args
    for key, value in kwargs.items():    # iterate through the dictionary
        print("  " + key + ": " + str(value))    # print each key-value pair

print_all(1, 2, 3, name="Alice", age=25)    # 1, 2, 3 + name=Alice, age=25


# Processes both positional and keyword arguments into a result dictionary
def process(*args, **kwargs) -> dict:
    result = {"args": args, "kwargs": kwargs}    # dict — contains both tuples and dict
    return result

data = process(1, 2, "hello", key1="value1", key2=42)    # dict — mixed data
print("Result: " + str(data))                             # Result: {'args': (1, 2, 'hello'), 'kwargs': {'key1': 'value1', 'key2': 42}}


# Flexible calculator that accepts numbers, operation type, and additional options
def calculate(*numbers, operation="add", **options) -> int:
    if not numbers:    # check if no numbers were provided
        return 0
    
    # Perform the specified operation on all numbers
    if operation == "add":    # addition
        result = 0
        for n in numbers:
            result = result + n
    elif operation == "multiply":    # multiplication
        result = 1
        for n in numbers:
            result = result * n
    else:
        result = numbers[0]    # default to first number
    
    # Check if result should be doubled based on options
    if options.get("double"):    # get "double" from kwargs, default False
        result = result * 2
    
    return result

result = calculate(1, 2, 3, operation="add")    # int — 1+2+3 = 6
print("Sum: " + str(result))                    # Sum: 6

result2 = calculate(2, 3, 4, operation="multiply")    # int — 2*3*4 = 24
print("Product: " + str(result2))                      # Product: 24

result3 = calculate(5, 6, operation="add", double=True)    # int — (5+6)*2 = 22
print("Doubled sum: " + str(result3))                       # Doubled sum: 22


# Greets multiple names with customizable greeting and punctuation
def greet(greeting, *names, **options) -> None:
    punct = options.get("punctuation", "!")    # str — punctuation, default "!"
    loud = options.get("loud", False)          # bool — uppercase if True
    
    for name in names:    # loop through each name
        msg = greeting + ", " + name + punct    # build the message
        if loud:    # if loud is True, uppercase the message
            msg = msg.upper()
        print(msg)

greet("Hello", "Alice", "Bob")                     # Hello, Alice!, Hello, Bob!
greet("Hi", "Charlie", punctuation="?", loud=True)  # HI, CHARLIE?


# Builds a SQL query string from table name, columns, and conditions
def query(table, *columns, **conditions) -> str:
    col_str = ", ".join(columns) if columns else "*"    # str — columns or all
    query = "SELECT " + col_str + " FROM " + table
    
    if conditions:    # if there are any keyword arguments
        where_parts = []    # list — collect WHERE conditions
        for key, value in conditions.items():    # iterate through conditions
            where_parts.append(key + " = " + str(value))    # "key = value"
        query = query + " WHERE " + " AND ".join(where_parts)    # combine with AND
    
    return query

sql = query("users", "name", "email", age=25, city="NYC")    # str — SQL query
print("SQL: " + sql)                                         # SQL: SELECT name, email FROM users WHERE age = 25 AND city = NYC

sql2 = query("products")    # str — query with all columns
print("SQL: " + sql2)       # SQL: SELECT * FROM products


# Flexible logging function that accepts level, tags, and detailed information
def log(level, *tags, **details) -> None:
    print("[" + level + "]", end=" ")    # print log level without newline
    
    if tags:    # if there are any positional tags
        print(" ".join("#" + tag for tag in tags), end=" ")    # add hashtags
    
    print("Message:", end=" ")    # label for message
    
    for key, value in details.items():    # print each detail as key=value
        print(key + "=" + str(value), end=" ")    # inline key-value pairs
    print()    # newline at the end

log("INFO", "user", "login", username="alice", ip="192.168.1.1")    # [INFO] #user #login Message: username=alice ip=192.168.1.1
log("ERROR", code=500, message="Server down")                         # [ERROR] Message: code=500 message=Server down


# Text formatting function with style and attribute options
def format_text(text, *styles, **attrs) -> None:
    result = text    # str — start with original text
    
    # Apply styles from positional arguments
    for style in styles:    # loop through each style
        if style == "upper":
            result = result.upper()    # convert to uppercase
        elif style == "lower":
            result = result.lower()    # convert to lowercase
        elif style == "title":
            result = result.title()    # title case each word
    
    prefix = ""    # str — opening markers
    suffix = ""    # str — closing markers
    
    # Apply attributes from keyword arguments
    if attrs.get("bold"):    # check for bold attribute
        prefix = prefix + "**"    # add markdown bold markers
        suffix = suffix + "**"
    if attrs.get("italic"):    # check for italic attribute
        prefix = prefix + "*"    # add markdown italic marker
        suffix = suffix + "*"
    if attrs.get("code"):    # check for code attribute
        prefix = prefix + "`"    # add backtick for code
        suffix = suffix + "`"
    
    print(prefix + result + suffix)    # print formatted result

format_text("hello", "upper", bold=True)    # **HELLO**
format_text("world", "title", italic=True)  # *World*


# Builds a complete HTTP URL from base, path parts, and query parameters
def make_request(url, *path_parts, **params) -> str:
    full_url = url    # str — start with base URL
    for part in path_parts:    # append each path part
        full_url = full_url + "/" + part
    
    if params:    # if there are query parameters
        query = "&".join(k + "=" + str(v) for k, v in params.items())    # build query string
        full_url = full_url + "?" + query    # append to URL
    
    return full_url

request = make_request("https://api.example.com", "users", "123", format="json")    # str — full URL
print("Request: " + request)                                                            # Request: https://api.example.com/users/123?format=json

request2 = make_request("https://site.com", page=1, limit=10)    # str — URL with query only
print("Request: " + request2)                                    # Request: https://site.com?page=1&limit=10


# Calculates totals from both positional numbers and keyword number values
def totalize(*args, **kwargs) -> dict:
    arg_sum = 0    # int — sum of positional arguments
    for n in args:
        arg_sum = arg_sum + n
    
    kwarg_sum = 0    # int — sum of keyword argument values
    for v in kwargs.values():    # iterate through dictionary values only
        if isinstance(v, (int, float)):    # only include numbers
            kwarg_sum = kwarg_sum + v
    
    return {
        "args_total": arg_sum,
        "kwargs_total": kwarg_sum,
        "combined": arg_sum + kwarg_sum
    }

totals = totalize(1, 2, 3, a=4, b=5)    # dict — all totals
print("Totals: " + str(totals))           # Totals: {'args_total': 6, 'kwargs_total': 9, 'combined': 15}
