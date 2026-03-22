# Example53.py
# Topic: Closures and Scope - Comprehensive Examples

# This file provides comprehensive real-world examples of closures
# and scope in Python applications.


# ============================================================
# Example 1: Currying
# ============================================================
print("=== Advanced: Currying ===")

def curried_add(a):
    """Curried add function."""
    def inner(b):
        return a + b
    return inner

# Usage
add_5 = curried_add(5)
print(f"5 + 3 = {add_5(3)}")  # 8

# Full currying
def curry(func, *args):
    """Generic currying function."""
    def inner(*more_args):
        return func(*args, *more_args)
    return inner

def add_three(a, b, c):
    return a + b + c

curried_add_three = curry(add_three, 1)
print(f"1 + 2 + 3 = {curried_add_three(2, 3)}")  # 6


# ============================================================
# Example 2: Partial Application
# ============================================================
print("\n=== Advanced: Partial Application ===")

def partial(func, *args, **kwargs):
    """Create a partial application of a function."""
    def wrapper(*more_args, **more_kwargs):
        combined = {**kwargs, **more_kwargs}
        return func(*args, *more_args, **combined)
    return wrapper

def power(base, exponent):
    return base ** exponent

square = partial(power, exponent=2)
cube = partial(power, exponent=3)

print(f"5 squared = {square(5)}")   # 25
print(f"5 cubed = {cube(5)}")       # 125


# ============================================================
# Example 3: Function Composition
# ============================================================
print("\n=== Advanced: Function Composition ===")

def compose(f, g):
    """Compose two functions: compose(f, g)(x) = f(g(x))"""
    return lambda x: f(g(x))

def add_one(x):
    return x + 1

def double(x):
    return x * 2

def subtract_three(x):
    return x - 3

# Compose functions
composed = compose(add_one, compose(double, subtract_three))
print(f"add_one(double(subtract_three(5))) = {composed(5)}")  # add_one(double(2)) = add_one(4) = 5

# Pipeline version
def pipeline(*funcs):
    def wrapper(x):
        result = x
        for f in funcs:
            result = f(result)
        return result
    return wrapper

pipe = pipeline(subtract_three, double, add_one)
print(f"Pipeline: {pipe(5)}")  # ((5-3)*2)+1 = 5


# ============================================================
# Example 4: Real-world - API Client Factory
# ============================================================
print("\n=== Real-world: API Client Factory ===")

def make_api_client(base_url, api_key):
    """Create an API client with base configuration."""
    request_count = 0
    
    def get(endpoint, params=None):
        nonlocal request_count
        request_count += 1
        return {
            "method": "GET",
            "url": f"{base_url}/{endpoint}",
            "params": params or {},
            "headers": {"Authorization": f"Bearer {api_key}"},
            "request_number": request_count
        }
    
    def post(endpoint, data=None):
        nonlocal request_count
        request_count += 1
        return {
            "method": "POST",
            "url": f"{base_url}/{endpoint}",
            "data": data or {},
            "headers": {"Authorization": f"Bearer {api_key}"},
            "request_number": request_count
        }
    
    def get_stats():
        return {"base_url": base_url, "requests_made": request_count}
    
    return get, post, get_stats

get, post, stats = make_api_client("https://api.example.com", "secret-key-123")

print(f"GET /users: {get('users')['url']}")
print(f"POST /orders: {post('orders', {'item': 'book'})['method']}")
print(f"Stats: {stats()}")


# ============================================================
# Example 5: Real-world - State Machine
# ============================================================
print("\n=== Real-world: State Machine ===")

def make_state_machine(initial_state, transitions):
    """Create a state machine."""
    current_state = [initial_state]
    history = [initial_state]
    
    def transition(event):
        key = (current_state[0], event)
        if key in transitions:
            current_state[0] = transitions[key]
            history.append(current_state[0])
            return current_state[0]
        return None
    
    def get_state():
        return current_state[0]
    
    def get_history():
        return history.copy()
    
    def reset():
        current_state[0] = initial_state
        history.clear()
        history.append(initial_state)
    
    return transition, get_state, get_history, reset

# Define transitions
transitions = {
    ("idle", "start"): "loading",
    ("loading", "success"): "ready",
    ("loading", "error"): "error",
    ("ready", "process"): "processing",
    ("processing", "complete"): "done",
    ("done", "reset"): "idle",
    ("error", "retry"): "loading"
}

transition, get_state, history, reset = make_state_machine("idle", transitions)

print(f"Initial: {get_state()}")
transition("start")
print(f"After start: {get_state()}")
transition("success")
print(f"After success: {get_state()}")
transition("process")
print(f"After process: {get_state()}")
transition("complete")
print(f"After complete: {get_state()}")
print(f"History: {history()}")


# ============================================================
# Example 6: Real-world - Builder Pattern
# ============================================================
print("\n=== Real-world: Builder Pattern ===")

def make_query_builder():
    """Create a SQL query builder."""
    query_parts = []
    params = []
    
    def select(*columns):
        query_parts.append(f"SELECT {', '.join(columns)}")
        return builder
    
    def from_table(table):
        query_parts.append(f"FROM {table}")
        return builder
    
    def where(condition, *values):
        query_parts.append(f"WHERE {condition}")
        params.extend(values)
        return builder
    
    def order_by(column, direction="ASC"):
        query_parts.append(f"ORDER BY {column} {direction}")
        return builder
    
    def limit(count):
        query_parts.append(f"LIMIT {count}")
        return builder
    
    def build():
        query = " ".join(query_parts)
        return query, params.copy()
    
    def reset():
        query_parts.clear()
        params.clear()
    
    builder = type('Builder', (), {
        'select': select,
        'from': from_table,
        'where': where,
        'order_by': order_by,
        'limit': limit,
        'build': build,
        'reset': reset
    })()

query, params = (builder
    .select("id", "name", "email")
    .from("users")
    .where("age > ? AND status = ?", 18, "active")
    .order_by("name")
    .limit(10)
    .build())

print(f"Query: {query}")
print(f"Params: {params}")


# ============================================================
# Example 7: Real-world - Observer Pattern
# ============================================================
print("\n=== Real-world: Observer Pattern ===")

def make_observable():
    """Create an observable subject."""
    observers = []
    state = {}
    
    def attach(observer):
        observers.append(observer)
        return observer
    
    def detach(observer):
        observers.remove(observer)
    
    def notify(event, *args, **kwargs):
        results = []
        for obs in observers:
            results.append(obs(event, *args, **kwargs))
        return results
    
    def set_state(key, value):
        state[key] = value
        notify("state_changed", key, value)
    
    def get_state(key=None):
        if key:
            return state.get(key)
        return state.copy()
    
    return attach, detach, notify, set_state, get_state

attach, detach, notify, set_state, get_state = make_observable()

# Create observers
def logger(event, *args, **kwargs):
    print(f"[LOG] Event: {event}, Args: {args}")

def recorder(event, *args, **kwargs):
    return {"event": event, "args": args}

attach(logger)
attach(recorder)

set_state("user", {"name": "Alice", "age": 30})
set_state("count", 42)


# ============================================================
# Example 8: Real-world - Pipeline Processor
# ============================================================
print("\n=== Real-world: Pipeline Processor ===")

def make_pipeline():
    """Create a data processing pipeline."""
    stages = []
    
    def add_stage(name, processor):
        stages.append({"name": name, "processor": processor})
        return builder
    
    def process(data):
        results = []
        for stage in stages:
            data = stage["processor"](data)
            results.append({"stage": stage["name"], "data": data})
        return results
    
    def clear():
        stages.clear()
    
    builder = type('Pipeline', (), {
        'add_stage': add_stage,
        'process': process,
        'clear': clear
    })()

    return builder

# Define processors
def filter_stage(data):
    return [x for x in data if x > 0]

def transform_stage(data):
    return [x * 2 for x in data]

def sort_stage(data):
    return sorted(data)

# Build pipeline
pipeline = make_pipeline()
pipeline.add_stage("filter", filter_stage)
pipeline.add_stage("transform", transform_stage)
pipeline.add_stage("sort", sort_stage)

input_data = [5, -3, 8, -1, 2, 10, -5]
results = pipeline.process(input_data)

for r in results:
    print(f"{r['stage']}: {r['data']}")


# ============================================================
# Example 9: Real-world - Throttle/Debounce
# ============================================================
print("\n=== Real-world: Throttle ===")

def make_throttler(limit_calls, time_window):
    """Create a throttler."""
    import time
    calls = []
    
    def throttle(func):
        def wrapper(*args, **kwargs):
            now = time.time()
            calls[:] = [c for c in calls if now - c < time_window]
            
            if len(calls) < limit_calls:
                calls.append(now)
                return func(*args, **kwargs)
            return None
        return wrapper
    
    return throttle

throttle = make_throttler(limit_calls=2, time_window=1)

@throttle
def make_api_call(request_id):
    return f"Request {request_id} processed"

for i in range(5):
    result = make_api_call(i)
    status = "Processed" if result else "Throttled"
    print(f"Request {i}: {status}")


# ============================================================
# Example 10: Real-world - Once Executor
# ============================================================
print("\n=== Real-world: Once Executor ===")

def make_once():
    """Create a function that executes only once."""
    executed = [False]
    result = [None]
    
    def once(*args, **kwargs):
        if not executed[0]:
            executed[0] = True
            if args and callable(args[0]):
                result[0] = args[0]()
            else:
                result[0] = args
        return result[0]
    
    def reset():
        executed[0] = False
        result[0] = None
    
    return once, reset

once, reset = make_once()

def expensive_operation():
    print("Expensive operation executed!")
    return "Result"

print(f"First call: {once(expensive_operation)}")
print(f"Second call: {once(lambda: 'cached')}")
print(f"Third call: {once('another')}")

reset()
print(f"After reset: {once('new result')}")


# ============================================================
# Summary
# ============================================================
print("\n" + "=" * 50)
print("COMPREHENSIVE CLOSURE PATTERNS:")
print("=" * 50)
print("""
1. CURRYING: Transform multi-arg function to sequence
   of single-arg functions

2. PARTIAL APPLICATION: Fix some arguments of a function

3. COMPOSITION: Chain functions together

4. PRACTICAL USES:
   - API clients
   - State machines
   - Builder patterns
   - Observer pattern
   - Pipeline processing
   - Throttling
   - Memoization
   - Rate limiting

5. KEY CONCEPTS:
   - Closures capture enclosing scope
   - nonlocal modifies enclosing variables
   - Mutable containers share state
   - Function factories create specialized functions
""")
