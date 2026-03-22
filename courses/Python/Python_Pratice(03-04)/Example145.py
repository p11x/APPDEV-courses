# Example145.py
# Topic: Combining Data Structures - Practical Examples


# ============================================================
# Example 1: Data Pipeline with Multiple Structures
# ============================================================
print("=== Data Pipeline ===")

from collections import deque, defaultdict
import heapq

class DataPipeline:
    def __init__(self):
        self.buffer = deque(maxlen=100)
        self.processed = []
        self.priority_queue = []
    
    def receive(self, item):
        self.buffer.append(item)
    
    def process(self):
        while self.buffer:
            item = self.buffer.popleft()
            if item["priority"] > 5:
                heapq.heappush(self.priority_queue, (-item["priority"], item))
            else:
                self.processed.append(item)
    
    def get_high_priority(self):
        if self.priority_queue:
            return heapq.heappop(self.priority_queue)[1]
        return None

pipeline = DataPipeline()
for i in [{"id": i, "priority": i * 2} for i in range(10)]:
    pipeline.receive(i)

pipeline.process()
print(f"Processed: {len(pipeline.processed)}")
print(f"High priority: {pipeline.get_high_priority()}")


# ============================================================
# Example 2: Inverted Index
# ============================================================
print("\n=== Inverted Index ===")

from collections import defaultdict

class InvertedIndex:
    def __init__(self):
        self.index = defaultdict(set)
    
    def add(self, doc_id, text):
        words = text.lower().split()
        for word in words:
            self.index[word].add(doc_id)
    
    def search(self, query):
        words = query.lower().split()
        if not words:
            return set()
        result = self.index[words[0]].copy()
        for word in words[1:]:
            result &= self.index[word]
        return result

idx = InvertedIndex()
idx.add(1, "python programming tutorial")
idx.add(2, "java programming basics")
idx.add(3, "python tips and tricks")

print(f"Search 'python': {idx.search('python')}")
print(f"Search 'programming': {idx.search('programming')}")


# ============================================================
# Example 3: LRU Cache Implementation
# ============================================================
print("\n=== LRU Cache ===")

from collections import OrderedDict

class LRUCache:
    def __init__(self, capacity):
        self.cache = OrderedDict()
        self.capacity = capacity
    
    def get(self, key):
        if key not in self.cache:
            return None
        self.cache.move_to_end(key)
        return self.cache[key]
    
    def put(self, key, value):
        if key in self.cache:
            self.cache.move_to_end(key)
        self.cache[key] = value
        if len(self.cache) > self.capacity:
            self.cache.popitem(last=False)

cache = LRUCache(3)
cache.put("a", 1)
cache.put("b", 2)
cache.put("c", 3)
cache.get("a")
cache.put("d", 4)
print(f"Cache keys: {list(cache.cache.keys())}")


# ============================================================
# Example 4: Trie (Prefix Tree)
# ============================================================
print("\n=== Trie ===")

class TrieNode:
    def __init__(self):
        self.children = {}
        self.is_end = False

class Trie:
    def __init__(self):
        self.root = TrieNode()
    
    def insert(self, word):
        node = self.root
        for char in word:
            if char not in node.children:
                node.children[char] = TrieNode()
            node = node.children[char]
        node.is_end = True
    
    def search(self, word):
        node = self.root
        for char in word:
            if char not in node.children:
                return False
            node = node.children[char]
        return node.is_end
    
    def starts_with(self, prefix):
        node = self.root
        for char in prefix:
            if char not in node.children:
                return False
            node = node.children[char]
        return True

trie = Trie()
for word in ["apple", "app", "application", "apply"]:
    trie.insert(word)

print(f"Search 'app': {trie.search('app')}")
print(f"Starts with 'app': {trie.starts_with('app')}")
print(f"Starts with 'xyz': {trie.starts_with('xyz')}")


# ============================================================
# Example 5: Graph with Adjacency List
# ============================================================
print("\n=== Graph ===")

from collections import defaultdict, deque

class Graph:
    def __init__(self):
        self.adj = defaultdict(list)
    
    def add_edge(self, u, v):
        self.adj[u].append(v)
        self.adj[v].append(u)
    
    def bfs(self, start):
        visited = set()
        queue = deque([start])
        result = []
        
        while queue:
            node = queue.popleft()
            if node not in visited:
                visited.add(node)
                result.append(node)
                queue.extend(self.adj[node])
        
        return result
    
    def dfs(self, start):
        visited = set()
        stack = [start]
        result = []
        
        while stack:
            node = stack.pop()
            if node not in visited:
                visited.add(node)
                result.append(node)
                stack.extend(self.adj[node])
        
        return result

g = Graph()
g.add_edge("A", "B")
g.add_edge("A", "C")
g.add_edge("B", "D")
g.add_edge("C", "D")

print(f"BFS from A: {g.bfs('A')}")
print(f"DFS from A: {g.dfs('A')}")


# ============================================================
# Example 6: Event Logger with Multiple Data Structures
# ============================================================
print("\n=== Event Logger ===")

from collections import deque, defaultdict
from datetime import datetime

class EventLogger:
    def __init__(self, max_events=1000):
        self.events = deque(maxlen=max_events)
        self.by_type = defaultdict(list)
        self.by_user = defaultdict(list)
    
    def log(self, event_type, user_id, message):
        event = {
            "type": event_type,
            "user": user_id,
            "message": message,
            "timestamp": datetime.now().isoformat()
        }
        self.events.append(event)
        self.by_type[event_type].append(event)
        self.by_user[user_id].append(event)
    
    def get_by_type(self, event_type):
        return self.by_type[event_type]
    
    def get_by_user(self, user_id):
        return self.by_user[user_id]

logger = EventLogger()
logger.log("login", "user1", "User logged in")
logger.log("click", "user1", "Button clicked")
logger.log("login", "user2", "User logged in")

print(f"User1 events: {len(logger.get_by_user('user1'))}")
print(f"Login events: {len(logger.get_by_type('login'))}")
