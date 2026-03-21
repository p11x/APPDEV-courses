# Example15.py
# Topic: Practical Examples with *args and **kwargs

# This file provides practical real-world examples that combine
# *args, **kwargs, positional-only, and keyword-only parameters.
# These patterns are commonly used in web frameworks and APIs.


# Flexible logger that can handle any type of logging
# level: positional-only (log level like INFO, WARNING, ERROR)
# message: positional-only (the log message)
# tags: *args (additional tags like "network", "database")
# details: **kwargs (additional key-value details)
def log(level, message, *tags, **details) -> None:
    print("[" + level + "]", message)    # print level and message
    if tags:    # if any tags provided
        print("  Tags:", ", ".join("#" + t for t in tags))    # format tags with commas
    if details:    # if any details provided
        print("  Details:")    # label for details section
        for key, value in details.items():    # iterate through each detail
            print("    " + key + ": " + str(value))    # indent and print key-value

log("INFO", "Application started")    # [INFO] Application started
log("WARNING", "Low memory", "system", usage=75)    # [WARNING] Low memory, Tags: #system, Details: usage: 75
log("ERROR", "Connection failed", "network", code=500, retry=3)    # [ERROR] Connection failed, Tags: #network, Details: code: 500, retry: 3


# Decorator-like function wrapper that forwards arguments
# name: positional-only (function or command name)
# *args: extra positional arguments
# verbose: keyword-only flag
# **kwargs: extra keyword arguments
def execute(name, /, *args, verbose=False, **kwargs) -> None:
    print("Executing:", name)    # print what we're executing
    if args:    # if positional arguments provided
        print("  Args:", args)    # print extra arguments
    if kwargs:    # if keyword arguments provided
        print("  Kwargs:", kwargs)    # print extra keyword arguments
    print("  Verbose:", verbose)    # print verbose flag status

execute("process_data", 1, 2, 3, verbose=True)    # Executing: process_data, Args: (1, 2, 3), Verbose: True
execute("send_email", to="user@example.com", subject="Test")    # Executing: send_email, Kwargs: {'to': ..., 'subject': ...}


# HTTP request builder for building URLs with paths and query parameters
# url: positional-only (base URL)
# *paths: additional path components
# method: keyword-only with default (HTTP method)
# **params: query string parameters
def build_request(url, /, *paths, method="GET", **params) -> str:
    full_url = url    # str — start with base URL
    for path in paths:    # append each path component
        full_url = full_url + "/" + str(path)
    
    query_parts = []    # list — builds query string parts
    for key, value in params.items():    # iterate through parameters
        query_parts.append(str(key) + "=" + str(value))    # "key=value"
    
    if query_parts:    # if there are query parameters
        full_url = full_url + "?" + "&".join(query_parts)    # append query string
    
    print("Method:", method)    # print HTTP method
    print("URL:", full_url)    # print the full URL
    return full_url

build_request("https://api.example.com", "users", "123", method="GET", format="json")    # Method: GET, URL: https://api.example.com/users/123?format=json


# Configuration builder for services
# service: positional-only (service name)
# *features: list of features to enable
# debug: keyword-only flag
# **settings: additional configuration settings
def configure(service, /, *features, debug=False, **settings) -> dict:
    config = {    # dict — build configuration dictionary
        "service": service,
        "features": features,
        "debug": debug,
        "settings": settings
    }
    return config

cfg = configure("database", "caching", "logging", debug=True, host="localhost", port=5432)    # {'service': 'database', 'features': ('caching', 'logging'), ...}
print("Config:", cfg)                                                                          # Config: {'service': 'database', 'features': ('caching', 'logging'), 'debug': True, 'settings': {'host': 'localhost', 'port': 5432}}


# Event handler with flexible parameters for event-driven systems
# event_type: positional-only (type of event)
# *data: event payload data
# priority: keyword-only with default
# **metadata: additional event metadata
def handle_event(event_type, /, *data, priority="normal", **metadata) -> None:
    print("Event:", event_type)    # print event type
    print("Priority:", priority)    # print priority level
    if data:    # if event data provided
        print("Data:", data)    # print the data tuple
    if metadata:    # if metadata provided
        print("Metadata:", metadata)    # print the metadata dict

handle_event("user_login", "192.168.1.1", "Chrome", priority="high", user_id=123)    # Event: user_login, Priority: high, Data: ('192.168.1.1', 'Chrome'), Metadata: {'user_id': 123}
handle_event("page_view", "/home", "/about")    # Event: page_view, Priority: normal, Data: ('/home', '/about')


# Command builder for shell-like commands
# command: positional-only (command name)
# *args: command arguments
# env, cwd: keyword-only with defaults
# **options: command options (flags and values)
def build_command(command, /, *args, env=None, cwd=None, **options) -> str:
    parts = [command]    # list — starts with command name
    
    for arg in args:    # process positional arguments
        if " " in str(arg):    # if argument contains spaces
            parts.append('"' + str(arg) + '"')    # wrap in quotes
        else:
            parts.append(str(arg))    # otherwise use as-is
    
    for key, value in options.items():    # process keyword options
        if len(key) == 1:    # short flag (like -v)
            parts.append("-" + key)    # add flag
            if value is not True:    # if value is not just a flag
                parts.append(str(value))    # add the value
        else:    # long option (like --verbose)
            parts.append("--" + key + "=" + str(value))    # add as --key=value
    
    result = " ".join(parts)    # str — join all parts with spaces
    print("Command:", result)    # print the built command
    return result

build_command("python", "script.py", "--input", "data.csv", "-v", env={"PYTHONPATH": "/lib"})    # Command: python script.py --input data.csv -v


# API router for web frameworks
# path: positional-only (URL path)
# *path_params: URL path parameters (like :id)
# method: keyword-only with default (HTTP method)
# auth, rate_limit: keyword-only parameters
def route(path, /, *path_params, method="GET", auth=True, rate_limit=100) -> None:
    print("Route:", path)    # print the route path
    print("Method:", method)    # print HTTP method
    print("Path params:", path_params)    # print captured path parameters
    print("Auth required:", auth)    # print auth requirement
    print("Rate limit:", rate_limit)    # print rate limit

route("/api/users", "123", "profile", method="GET")    # Route: /api/users, Method: GET, Path params: ('123', 'profile')
route("/api/login", method="POST", auth=False)         # Route: /api/login, Method: POST, Auth required: False


# Flexible test runner
# test_name: positional-only (name of the test)
# *assertions: test assertions (True/False values)
# verbose: keyword-only flag
# **test_data: test data key-value pairs
def run_test(test_name, /, *assertions, verbose=True, **test_data) -> None:
    print("Running:", test_name)    # print test name
    print("Assertions:", len(assertions))    # print number of assertions
    
    passed = 0    # int — counter for passed assertions
    for assertion in assertions:    # count passed assertions
        if assertion:    # if assertion is True
            passed += 1    # increment counter
    
    print("Passed:", passed, "/", len(assertions))    # print pass count
    
    if test_data:    # if test data provided
        print("Test data:", test_data)    # print test data

run_test("test_user_creation", True, True, True, user_name="alice", user_id=1)    # Running: test_user_creation, Assertions: 3, Passed: 3/3
run_test("test_login", False, True, verbose=False)                                # Running: test_login, Assertions: 2, Passed: 1/2


# Flexible data processor for ETL pipelines
# data_type: positional-only (type of data being processed)
# *values: the actual data values
# transform, validate: keyword-only options
# **config: additional processing configuration
def process(data_type, /, *values, transform=None, validate=True, **config) -> dict:
    result = {    # dict — build result dictionary
        "type": data_type,
        "values": values,
        "transform": transform,
        "validate": validate,
        "config": config
    }
    return result

processed = process("numbers", 1, 2, 3, 4, 5, transform="square", validate=True, precision=2)    # {'type': 'numbers', 'values': (1,2,3,4,5), ...}
print("Processed:", processed)                                                                     # Processed: {'type': 'numbers', 'values': (1, 2, 3, 4, 5), 'transform': 'square', 'validate': True, 'config': {'precision': 2}}

processed2 = process("strings", "a", "b", "c", transform="uppercase")    # {'type': 'strings', 'values': ('a','b','c'), ...}
print("Processed:", processed2)                                            # Processed: {'type': 'strings', 'values': ('a', 'b', 'c'), 'transform': 'uppercase', 'validate': True, 'config': {}}


# Message sender with flexible options for email or messaging systems
# recipient: positional-only (who receives the message)
# *cc: carbon copy recipients
# subject, body: keyword-only with defaults
# **options: additional sending options
def send_message(recipient, /, *cc, subject="", body="", **options) -> None:
    print("To:", recipient)    # print primary recipient
    if cc:    # if carbon copies provided
        print("CC:", ", ".join(cc))    # print CC recipients
    print("Subject:", subject)    # print subject
    print("Body:", body)    # print body
    print("Options:", options)    # print additional options

send_message("user@example.com", "other@example.com", subject="Hello", body="Message text", priority="high")    # To: user@example.com, CC: other@example.com, Subject: Hello, Body: Message text, Options: {'priority': 'high'}


# Flexible query builder for building SQL queries
# table: positional-only (database table name)
# *fields: fields to select
# **conditions: WHERE conditions
def build_query(table, /, *fields, where=None, **conditions) -> str:
    cols = ", ".join(fields) if fields else "*"    # str — columns or all (*)
    query = "SELECT " + cols + " FROM " + table
    
    where_clauses = []    # list — collect WHERE conditions
    if where:    # if base where condition provided
        where_clauses.append(where)    # add to clauses
    
    for key, value in conditions.items():    # iterate through keyword conditions
        where_clauses.append(key + "=" + str(value))    # add as "key=value"
    
    if where_clauses:    # if there are any conditions
        query += " WHERE " + " AND ".join(where_clauses)    # combine with AND
    
    return query

sql = build_query("users", "name", "email", where="active=1", age=">18", city="NYC")    # SELECT name, email FROM users WHERE active=1 AND age>18 AND city=NYC
print("SQL:", sql)                                                                     # SQL: SELECT name, email FROM users WHERE active=1 AND age>18 AND city=NYC


# Webhook handler for processing incoming webhooks
# event: positional-only (event type)
# *payload: event payload data
# signature: keyword-only for verification
# **headers: webhook headers
def handle_webhook(event, /, *payload, signature=None, **headers) -> None:
    print("Event:", event)    # print event type
    print("Payload:", payload)    # print payload tuple
    print("Signature:", signature)    # print signature if provided
    print("Headers:", headers)    # print headers dict

handle_webhook("order.created", 123, 456, signature="abc123", content_type="application/json")    # Event: order.created, Payload: (123, 456), Signature: abc123, Headers: {'content_type': 'application/json'}
