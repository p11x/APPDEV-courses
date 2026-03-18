# Trees and Graphs

## What You'll Learn

- Binary trees
- Binary search trees
- Graph representations

## Prerequisites

- Completed `05-searching-algorithms.md`

## Binary Tree

```python
from dataclasses import dataclass
from typing import Optional, Generator

@dataclass
class TreeNode[T]:
    value: T
    left: Optional["TreeNode[T]"] = None
    right: Optional["TreeNode[T]"] = None

class BinaryTree[T]:
    def __init__(self):
        self.root: Optional[TreeNode[T]] = None
    
    def insert(self, value: T) -> None:
        """Insert value into tree."""
        if not self.root:
            self.root = TreeNode(value)
            return
        
        current = self.root
        while True:
            if value < current.value:
                if not current.left:
                    current.left = TreeNode(value)
                    return
                current = current.left
            else:
                if not current.right:
                    current.right = TreeNode(value)
                    return
                current = current.right
    
    def search(self, value: T) -> bool:
        """Search for value in tree."""
        current = self.root
        while current:
            if value == current.value:
                return True
            elif value < current.value:
                current = current.left
            else:
                current = current.right
        return False
    
    def inorder(self) -> Generator[T, None, None]:
        """Inorder traversal (left, root, right)."""
        def _traverse(node: Optional[TreeNode[T]]) -> Generator[T, None, None]:
            if node:
                yield from _traverse(node.left)
                yield node.value
                yield from _traverse(node.right)
        
        yield from _traverse(self.root)
    
    def preorder(self) -> Generator[T, None, None]:
        """Preorder traversal (root, left, right)."""
        def _traverse(node: Optional[TreeNode[T]]) -> Generator[T, None, None]:
            if node:
                yield node.value
                yield from _traverse(node.left)
                yield from _traverse(node.right)
        
        yield from _traverse(self.root)
    
    def postorder(self) -> Generator[T, None, None]:
        """Postorder traversal (left, right, root)."""
        def _traverse(node: Optional[TreeNode[T]]) -> Generator[T, None, None]:
            if node:
                yield from _traverse(node.left)
                yield from _traverse(node.right)
                yield node.value
        
        yield from _traverse(self.root)
```

## Binary Search Tree (BST)

A BST has the property that left subtree values are less than root, right subtree values are greater:

```python
# The BinaryTree class above is a BST
# Operations are O(log n) average case

tree = BinaryTree[int]()
tree.insert(5)
tree.insert(3)
tree.insert(7)
tree.insert(1)
tree.insert(4)

print(list(tree.inorder()))  # [1, 3, 4, 5, 7]
print(tree.search(4))  # True
print(tree.search(10))  # False
```

## Graph Representations

```python
from collections import defaultdict
from dataclasses import dataclass, field
from typing import Literal

@dataclass
class Graph:
    """Undirected graph using adjacency list."""
    _adjacency: dict[str, list[str]] = field(default_factory=lambda: defaultdict(list))
    
    def add_node(self, node: str) -> None:
        """Add a node."""
        if node not in self._adjacency:
            self._adjacency[node] = []
    
    def add_edge(self, u: str, v: str) -> None:
        """Add an undirected edge."""
        self.add_node(u)
        self.add_node(v)
        self._adjacency[u].append(v)
        self._adjacency[v].append(u)
    
    def neighbors(self, node: str) -> list[str]:
        """Get neighbors of a node."""
        return self._adjacency.get(node, [])
    
    def nodes(self) -> list[str]:
        """Get all nodes."""
        return list(self._adjacency.keys())
    
    def edges(self) -> list[tuple[str, str]]:
        """Get all edges."""
        seen = set()
        result = []
        for node in self._adjacency:
            for neighbor in self._adjacency[node]:
                edge = tuple(sorted([node, neighbor]))
                if edge not in seen:
                    seen.add(edge)
                    result.append(edge)
        return result
```

## Use Cases

- **Trees**: File systems, XML/HTML DOM, organization charts
- **Graphs**: Social networks, road maps, dependencies

## Summary

- Trees have hierarchical structure
- BST enables efficient search
- Graphs model relationships

## Next Steps

Continue to `07-recursion.md`.
