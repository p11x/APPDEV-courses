# Example14.py
# Topic: Modern Python 3.12+ Parameter Syntax

# This file demonstrates the modern parameter syntax that combines all
# parameter types: positional-only, positional-or-keyword, *args,
# keyword-only, and **kwargs. This is the most flexible function signature.


# Complete modern function signature demonstrating all parameter types
# pos_or_kw: positional-or-keyword (before /)
# *args: collects extra positional arguments
# keyword_only: must be passed as keyword (after *)
# **kwargs: collects extra keyword arguments
def modern_func(
    pos_or_kw,    # positional-or-keyword — can be passed positionally or as keyword
    /,            # slash marks end of positional-only parameters
    *args,        # *args — captures extra positional arguments as tuple
    keyword_only,    # keyword-only — must be passed as keyword argument
    **kwargs        # **kwargs — captures extra keyword arguments as dict
) -> None:
    print("pos_or_kw:", pos_or_kw)
    print("args:", args)
    print("keyword_only:", keyword_only)
    print("kwargs:", kwargs)

modern_func(1, 2, 3, keyword_only="value", extra="data")    # pos_or_kw=1, args=(2,3), keyword_only=value, kwargs={'extra': 'data'}


# Real-world API endpoint handler using modern syntax
# path: positional-only
# method: positional-or-keyword with default
# auth_required, timeout, headers: keyword-only
def handle_request(
    path,    # positional-only — must be passed positionally
    /,       # end of positional-only section
    method="GET",    # positional-or-keyword — can use default or pass as keyword
    *,      # asterisk marks start of keyword-only section
    auth_required=True,    # keyword-only — must be passed as keyword
    timeout=30,            # keyword-only with default value
    headers=None          # keyword-only with default value
) -> None:
    print("Path:", path)
    print("Method:", method)
    print("Auth required:", auth_required)
    print("Timeout:", timeout)
    print("Headers:", headers)

handle_request("/api/users")                          # default GET request
handle_request("/api/login", "POST", auth_required=False)    # POST to login endpoint


# Database query builder with all parameter types
# table: positional-only
# columns: *args for variable columns
# where_clause, order_by, limit: keyword-only with defaults
def build_query(
    table,    # positional-only — table name is required positionally
    /,        # end of positional-only
    *columns,    # *args — extra column names as positional args
    where_clause=None,    # keyword-only with default — WHERE condition
    order_by=None,        # keyword-only with default — ORDER BY clause
    limit=None            # keyword-only with default — LIMIT value
) -> str:
    col_str = ", ".join(columns) if columns else "*"    # str — columns or all (*)
    query = "SELECT " + col_str + " FROM " + table
    
    if where_clause:    # if WHERE clause provided
        query = query + " WHERE " + where_clause
    
    if order_by:    # if ORDER BY provided
        query = query + " ORDER BY " + order_by
    
    if limit:    # if LIMIT provided
        query = query + " LIMIT " + str(limit)
    
    return query

sql = build_query("users", "name", "email", where_clause="active=1", order_by="name", limit=10)    # SELECT name, email FROM users WHERE active=1 ORDER BY name LIMIT 10
print("SQL:", sql)                                                                              # SQL: SELECT name, email FROM users WHERE active=1 ORDER BY name LIMIT 10


# Flexible HTTP request function
# url: positional-only
# paths: *args for URL path parts
# method, headers, timeout: keyword-only with defaults
# params: **kwargs for query parameters
def make_request(
    url,    # positional-only — base URL is required
    /,      # end of positional-only
    *paths,    # *args — additional path components
    method="GET",    # keyword-only with default — HTTP method
    headers=None,    # keyword-only with default — HTTP headers
    timeout=30,      # keyword-only with default — timeout in seconds
    **params         # **kwargs — query string parameters
) -> str:
    full_url = url    # str — start with base URL
    for part in paths:    # append each path part
        full_url = full_url + "/" + part
    
    if params:    # if query parameters provided
        query = "&".join(k + "=" + str(v) for k, v in params.items())    # build query string
        full_url = full_url + "?" + query    # append to URL
    
    print("URL:", full_url)
    print("Method:", method)
    print("Headers:", headers)
    print("Timeout:", timeout)
    return full_url

make_request("https://api.example.com", "users", "123", method="GET", token="abc")    # URL: https://api.example.com/users/123?token=abc


# Advanced calculator with positional-only and keyword-only parameters
# a, b: positional-only
# operations: *args for multiple operations
# show_steps, precision: keyword-only
def calculator(
    a,    # positional-only — first number
    b,    # positional-only — second number
    /,    # end of positional-only
    *operations,    # *args — operations to perform
    show_steps=False,    # keyword-only with default — whether to print steps
    precision=2          # keyword-only with default — decimal precision
):
    results = {}    # dict — stores results of each operation
    
    for op in operations:    # process each operation
        if op == "add":
            results["add"] = a + b
        elif op == "subtract":
            results["subtract"] = a - b
        elif op == "multiply":
            results["multiply"] = a * b
        elif op == "divide":
            if b != 0:    # avoid division by zero
                results["divide"] = a / b
    
    if show_steps:    # if user wants to see steps
        print("Operations performed:", operations)
        print("Results:", results)
    
    return results

calc_results = calculator(10, 5, "add", "multiply", show_steps=True, precision=3)    # Results: {'add': 15, 'multiply': 50}
print("Results:", calc_results)                                                      # Results: {'add': 15, 'multiply': 50}


# Logger with complete parameter flexibility
# message: positional-only
# tags: *args for additional tags
# level, timestamp, output: keyword-only with defaults
# metadata: **kwargs for extra data
def log_message(
    message,    # positional-only — the log message itself
    /,           # end of positional-only
    *tags,       # *args — additional tags as positional args
    level="INFO",    # keyword-only with default — log level
    timestamp=True,    # keyword-only with default — whether to add timestamp
    output="console",    # keyword-only with default — output destination
    **metadata        # **kwargs — additional metadata key-value pairs
) -> None:
    parts = []    # list — builds the final log message
    
    if timestamp:    # if timestamp is enabled
        import datetime    # import datetime module
        now = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")    # current timestamp
        parts.append("[" + now + "]")    # add timestamp to parts
    
    parts.append("[" + level + "]")    # add log level
    parts.append(message)    # add the main message
    
    if tags:    # if tags were provided
        tag_str = " ".join("#" + t for t in tags)    # format tags with hashtags
        parts.append(tag_str)    # add to parts
    
    if metadata:    # if metadata was provided
        parts.append(str(metadata))    # add as string
    
    final = " ".join(parts)    # join all parts with spaces
    print(final)

log_message("Server started", "system", "boot")    # [2024-...] [INFO] Server started #system #boot
log_message("User logged in", level="WARNING", user_id=123, ip="192.168.1.1")    # [2024-...] [WARNING] User logged in {'user_id': 123, 'ip': '192.168.1.1'}


# Complete function signature showing all parameter types together
# pos_only: positional-only
# pos_or_kw: positional-or-keyword with default
# args: *args for extra positional
# kw_only: keyword-only with default
# kwargs: **kwargs for extra keyword
def complete_example(
    pos_only,    # positional-only — must be passed positionally
    /,           # end of positional-only
    pos_or_kw="default",    # positional-or-keyword — can use default or pass keyword
    *args,       # *args — collects extra positional arguments
    kw_only="keyword",    # keyword-only with default value
    **kwargs     # **kwargs — collects extra keyword arguments
) -> dict:
    return {
        "pos_only": pos_only,
        "pos_or_kw": pos_or_kw,
        "args": args,
        "kw_only": kw_only,
        "kwargs": kwargs
    }

result = complete_example(1, "two", 3, 4, kw_only="custom", extra="value")    # {'pos_only': 1, 'pos_or_kw': 'two', 'args': (3, 4), 'kw_only': 'custom', 'kwargs': {'extra': 'value'}}
print("Result:", result)                                                      # Result: {'pos_only': 1, 'pos_or_kw': 'two', 'args': (3, 4), 'kw_only': 'custom', 'kwargs': {'extra': 'value'}}


# Data processor with all parameter types
# source: positional-only
# fields: *args for field names
# filter_by, transform, validate: keyword-only
# options: **kwargs for extra options
def process_data(
    source,    # positional-only — data source identifier
    /,         # end of positional-only
    *fields,   # *args — field names to process
    filter_by=None,    # keyword-only with default — filter condition
    transform=None,    # keyword-only with default — transformation type
    validate=True,     # keyword-only with default — validation flag
    **options          # **kwargs — additional processing options
) -> dict:
    return {
        "source": source,
        "fields": fields,
        "filter_by": filter_by,
        "transform": transform,
        "validate": validate,
        "options": options
    }

data = process_data("database", "name", "email", "age", filter_by="active", transform="uppercase")    # {'source': 'database', 'fields': ('name', 'email', 'age'), ...}
print("Data:", data)                                                                                  # Data: {'source': 'database', 'fields': ('name', 'email', 'age'), 'filter_by': 'active', ...}
