# Example264: Tree Traversals and Operations
class TreeNode:
    def __init__(self, val):
        self.val = val
        self.left = None
        self.right = None

# Build tree from array
def build_tree(arr):
    """Build tree from level-order array."""
    if not arr:
        return None
    root = TreeNode(arr[0])
    queue = [root]
    i = 1
    while queue and i < len(arr):
        node = queue.pop(0)
        if i < len(arr) and arr[i] is not None:
            node.left = TreeNode(arr[i])
            queue.append(node.left)
        i += 1
        if i < len(arr) and arr[i] is not None:
            node.right = TreeNode(arr[i])
            queue.append(node.right)
        i += 1
    return root

# Inorder traversal
def inorder(root):
    if not root:
        return []
    return inorder(root.left) + [root.val] + inorder(root.right)

# Preorder traversal
def preorder(root):
    if not root:
        return []
    return [root.val] + preorder(root.left) + preorder(root.right)

# Postorder traversal
def postorder(root):
    if not root:
        return []
    return postorder(root.left) + postorder(root.right) + [root.val]

# Level order
def level_order(root):
    if not root:
        return []
    result = []
    queue = [root]
    while queue:
        level = []
        for _ in range(len(queue)):
            node = queue.pop(0)
            level.append(node.val)
            if node.left:
                queue.append(node.left)
            if node.right:
                queue.append(node.right)
        result.append(level)
    return result

print("Tree Traversals:")
arr = [1, 2, 3, 4, 5, None, 6]
root = build_tree(arr)
print(f"Tree from array: {arr}")
print(f"Inorder: {inorder(root)}")
print(f"Preorder: {preorder(root)}")
print(f"Postorder: {postorder(root)}")
print(f"Level order: {level_order(root)}")

# Tree height
def height(root):
    if not root:
        return 0
    return 1 + max(height(root.left), height(root.right))

print(f"\nTree height: {height(root)}")

# Count nodes
def count_nodes(root):
    if not root:
        return 0
    return 1 + count_nodes(root.left) + count_nodes(root.right)

print(f"Node count: {count_nodes(root)}")

# Find max value
def find_max(root):
    if not root:
        return float('-inf')
    return max(root.val, find_max(root.left), find_max(root.right))

print(f"Max value: {find_max(root)}")
