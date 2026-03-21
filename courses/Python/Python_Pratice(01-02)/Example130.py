# Example130.py
# Topic: Built-in Context Managers — threading.Lock

# threading.Lock ensures only one thread can access code at a time
# This prevents race conditions when multiple threads share data
# The lock is acquired with 'with' and automatically released

import threading
import time

# Create a shared counter and a lock
counter = 0
lock = threading.Lock()

def increment_many_times():
    global counter
    # Only one thread can be inside this block at a time
    # Other threads wait until the lock is released
    with lock:
        for i in range(1000):
            counter = counter + 1

# Create multiple threads that all try to increment
threads = []
for i in range(5):
    t = threading.Thread(target=increment_many_times)
    threads.append(t)

for t in threads:
    t.start()

for t in threads:
    t.join()

# Without the lock, this would be less than 5000
# With the lock, it should be exactly 5000
print(counter)

# Another example: protecting a shared list
shared_data = []
data_lock = threading.Lock()

def add_item(item):
    with data_lock:
        shared_data.append(item)
        time.sleep(0.01)

def read_items():
    with data_lock:
        return list(shared_data)

# Producer: add items
for i in range(10):
    add_item("Item " + str(i))

# Consumer: read items
items = read_items()
print(len(items))

# Using lock for a critical section
balance = 1000

def withdraw(amount):
    global balance
    with lock:
        if balance >= amount:
            time.sleep(0.01)
            balance = balance - amount
            return True
        return False

# Multiple threads trying to withdraw
withdrawals = []
def try_withdraw():
    result = withdraw(100)
    withdrawals.append(result)

threads = []
for i in range(15):
    t = threading.Thread(target=try_withdraw)
    threads.append(t)

for t in threads:
    t.start()

for t in threads:
    t.join()

print(balance)
print(sum(withdrawals))

# RLock - reentrant lock (same thread can acquire multiple times)
rlock = threading.RLock()

def outer():
    with rlock:
        print("Outer")

def inner():
    with rlock:
        print("Inner")

def nested():
    # RLock allows the same thread to acquire again
    with rlock:
        outer()
        inner()

nested()

# Using lock to implement a simple semaphore (counting lock)
class SimpleSemaphore:
    def __init__(self, count):
        self.count = count
        self.lock = threading.Lock()
    
    def acquire(self):
        with self.lock:
            if self.count > 0:
                self.count = self.count - 1
                return True
            return False
    
    def release(self):
        with self.lock:
            self.count = self.count + 1

# Only 2 threads can run at a time
sem = SimpleSemaphore(2)

def limited_task(task_id):
    if sem.acquire():
        print("Task " + str(task_id) + " started")
        time.sleep(0.1)
        print("Task " + str(task_id) + " finished")
        sem.release()

threads = []
for i in range(5):
    t = threading.Thread(target=limited_task, args=(i,))
    threads.append(t)

for t in threads:
    t.start()

for t in threads:
    t.join()
