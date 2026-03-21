# Example13.py
# Topic: Keyword-Only Parameters (*)

# This file demonstrates keyword-only parameters in Python.
# Parameters after the * (or *args) must be passed as keywords, not positionally.
# This ensures clarity in function calls and prevents argument order mistakes.


# Simple function with keyword-only parameters
# Both name and greeting must be passed as keywords
def greet(*, name, greeting="Hello") -> None:
    print(greeting + ", " + name + "!")

greet(name="Alice")              # Hello, Alice! (uses default greeting)
greet(name="Bob", greeting="Hi")    # Hi, Bob! (custom greeting)


# Parameters after * must be passed by keyword, not positionally
# age and city are keyword-only, name is regular parameter
def create_user(*, name, age, city="Unknown") -> None:
    print("Name: " + name + ", Age: " + str(age) + ", City: " + city)

create_user(name="Alice", age=25)              # Name: Alice, Age: 25, City: Unknown
create_user(name="Bob", age=30, city="NYC")    # Name: Bob, Age: 30, City: NYC


# Mix regular parameters with keyword-only using *
# a and b can be positional, c and d must be keywords
def func(a, b, *, c, d) -> None:
    print("a=" + str(a) + ", b=" + str(b) + ", c=" + str(c) + ", d=" + str(d))

func(1, 2, c=3, d=4)    # a=1, b=2, c=3, d=4


# All parameters after * are keyword-only - requires all to be specified
def required_kwargs(*, name, email, role) -> None:
    print("Name: " + name)
    print("Email: " + email)
    print("Role: " + role)

required_kwargs(name="John", email="john@example.com", role="admin")    # Name: John, Email: john@example.com, Role: admin


# Keyword-only parameters with default values
def configure(*, host="localhost", port=8080, ssl=False) -> None:
    protocol = "https" if ssl else "http"    # str — protocol based on ssl flag
    print(protocol + "://" + host + ":" + str(port))

configure()                              # http://localhost:8080
configure(port=3000)                     # http://localhost:3000
configure(host="api.example.com", ssl=True)    # https://api.example.com:8080


# Using * with **kwargs for flexible keyword-only options
def process(*, debug=False, **options) -> None:
    print("Debug: " + str(debug))
    print("Options: " + str(options))

process()                              # Debug: False, Options: {}
process(debug=True, timeout=30, retries=3)    # Debug: True, Options: {'timeout': 30, 'retries': 3}


# Forces readable function calls - no guessing what arguments mean
def send_email(*, to, subject, body, from_addr="noreply@company.com") -> None:
    print("To: " + to)
    print("Subject: " + subject)
    print("Body: " + body)
    print("From: " + from_addr)

send_email(to="user@example.com", subject="Hello", body="Message")    # all required keywords


# Keyword-only for configuration - common pattern for database connections
def database_connect(*, host, port, database, user, password) -> str:
    return "Connecting to " + database + " on " + host + ":" + str(port)

conn = database_connect(host="localhost", port=5432, database="mydb", user="admin", password="secret")    # Connecting to mydb on localhost:5432


# Prevents accidental parameter swapping by requiring keywords
def calculate(*, x, y, operation) -> int:
    if operation == "add":
        return x + y
    elif operation == "subtract":
        return x - y
    return 0

result = calculate(x=10, y=5, operation="add")    # int — 10+5 = 15
print("Result: " + str(result))                  # Result: 15


# Combining *args and keyword-only parameters
def flexible_func(a, *args, keyword_only, **kwargs) -> None:
    print("a=" + str(a))    # regular positional argument
    print("args=" + str(args))    # tuple of extra positional args
    print("keyword_only=" + str(keyword_only))    # required keyword-only
    print("kwargs=" + str(kwargs))    # extra keyword arguments

flexible_func(1, 2, 3, 4, keyword_only="value", extra="data")    # a=1, args=(2,3,4), keyword_only=value, kwargs={'extra': 'data'}


# Function with all parameters keyword-only after *
def full_kwonly(*, req1, req2, opt1="default", opt2=10) -> None:
    print("req1=" + req1 + ", req2=" + req2 + ", opt1=" + opt1 + ", opt2=" + str(opt2))

full_kwonly(req1="a", req2="b")                          # req1=a, req2=b, opt1=default, opt2=10
full_kwonly(req1="x", req2="y", opt1="custom", opt2=20)    # req1=x, req2=y, opt1=custom, opt2=20


# API endpoint definition style - common in web frameworks
def define_route(path, *, method="GET", auth_required=True, rate_limit=100) -> None:
    print("Path: " + path)
    print("Method: " + method)
    print("Auth: " + str(auth_required))
    print("Rate limit: " + str(rate_limit))

define_route("/api/users")                              # Path: /api/users, Method: GET
define_route("/api/login", method="POST", auth_required=False)    # Path: /api/login, Method: POST, Auth: False


# Ensures clarity in function calls - especially useful for configuration
def draw_shape(*, shape, x=0, y=0, color="black", size=10) -> None:
    print("Drawing " + shape + " at (" + str(x) + "," + str(y) + ")")
    print("Color: " + color + ", Size: " + str(size))

draw_shape(shape="circle", x=10, y=20)         # Drawing circle at (10,20), Color: black, Size: 10
draw_shape(shape="square", color="red", size=5)    # Drawing square at (0,0), Color: red, Size: 5


# Database query with keyword-only parameters - prevents SQL errors from arg order
def query_db(*, table, where=None, order_by=None, limit=None) -> str:
    sql = "SELECT * FROM " + table
    
    if where:    # if where condition provided
        sql = sql + " WHERE " + where    # add WHERE clause
    if order_by:    # if ordering specified
        sql = sql + " ORDER BY " + order_by    # add ORDER BY
    if limit:    # if limit specified
        sql = sql + " LIMIT " + str(limit)    # add LIMIT
    
    return sql

query = query_db(table="users", where="age > 18", order_by="name", limit=10)    # SELECT * FROM users WHERE age > 18 ORDER BY name LIMIT 10
print("Query: " + query)                                                         # Query: SELECT * FROM users WHERE age > 18 ORDER BY name LIMIT 10
