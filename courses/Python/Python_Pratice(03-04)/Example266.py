# Example266: Tries (Prefix Tree)
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
        node = self._find_node(word)
        return node is not None and node.is_end
    
    def starts_with(self, prefix):
        return self._find_node(prefix) is not None
    
    def _find_node(self, prefix):
        node = self.root
        for char in prefix:
            if char not in node.children:
                return None
            node = node.children[char]
        return node

print("Trie (Prefix Tree):")
trie = Trie()
words = ["apple", "app", "application", "apply", "banana"]
for word in words:
    trie.insert(word)

print(f"Search 'app': {trie.search('app')}")
print(f"Search 'appl': {trie.search('appl')}")
print(f"Starts with 'app': {trie.starts_with('app')}")
print(f"Starts with 'ban': {trie.starts_with('ban')}")

# Auto-complete
def autocomplete(trie, prefix):
    node = trie._find_node(prefix)
    if not node:
        return []
    results = []
    def dfs(node, path):
        if node.is_end:
            results.append(path)
        for char, child in node.children.items():
            dfs(child, path + char)
    dfs(node, prefix)
    return results

print("\nAuto-complete:")
print(f"autocomplete('app'): {autocomplete(trie, 'app')}")
print(f"autocomplete('ap'): {autocomplete(trie, 'ap')}")

# Word count in trie
class TrieWithCount(TrieNode):
    def __init__(self):
        super().__init__()
        self.count = 0

def count_prefix(trie, prefix):
    node = trie._find_node(prefix)
    if not node:
        return 0
    def dfs(node):
        total = node.count
        for child in node.children.values():
            total += dfs(child)
        return total
    return dfs(node)

# Longest common prefix
def longest_common_prefix(words):
    if not words:
        return ""
    trie = Trie()
    for word in words:
        trie.insert(word)
    prefix = ""
    node = trie.root
    while len(node.children) == 1 and not node.is_end:
        char = list(node.children.keys())[0]
        prefix += char
        node = node.children[char]
    return prefix

print("\nLongest common prefix:")
words = ["flower", "flow", "flight"]
print(f"Words: {words}")
print(f"LCP: '{longest_common_prefix(words)}'")
