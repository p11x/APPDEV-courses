# Example265: BST Operations
class BSTNode:
    def __init__(self, val):
        self.val = val
        self.left = None
        self.right = None

class BST:
    def __init__(self):
        self.root = None
    
    def insert(self, val):
        if not self.root:
            self.root = BSTNode(val)
            return
        node = self.root
        while True:
            if val < node.val:
                if not node.left:
                    node.left = BSTNode(val)
                    return
                node = node.left
            else:
                if not node.right:
                    node.right = BSTNode(val)
                    return
                node = node.right
    
    def search(self, val):
        node = self.root
        while node:
            if val == node.val:
                return True
            elif val < node.val:
                node = node.left
            else:
                node = node.right
        return False
    
    def delete(self, val):
        self.root = self._delete(self.root, val)
    
    def _delete(self, node, val):
        if not node:
            return None
        if val < node.val:
            node.left = self._delete(node.left, val)
        elif val > node.val:
            node.right = self._delete(node.right, val)
        else:
            if not node.left:
                return node.right
            if not node.right:
                return node.left
            min_larger = node.right
            while min_larger.left:
                min_larger = min_larger.left
            node.val = min_larger.val
            node.right = self._delete(node.right, node.val)
        return node
    
    def inorder(self):
        result = []
        self._inorder(self.root, result)
        return result
    
    def _inorder(self, node, result):
        if node:
            self._inorder(node.left, result)
            result.append(node.val)
            self._inorder(node.right, result)

print("BST Operations:")
bst = BST()
for val in [5, 3, 7, 1, 4, 6, 8]:
    bst.insert(val)

print(f"Inorder: {bst.inorder()}")
print(f"Search 5: {bst.search(5)}")
print(f"Search 10: {bst.search(10)}")

bst.delete(5)
print(f"After delete 5: {bst.inorder()}")

# BST validation
def is_valid_bst(root, min_val=float('-inf'), max_val=float('inf')):
    if not root:
        return True
    if root.val <= min_val or root.val >= max_val:
        return False
    return (is_valid_bst(root.left, min_val, root.val) and
            is_valid_bst(root.right, root.val, max_val))

print("\nBST Validation:")
root = BSTNode(2)
root.left = BSTNode(1)
root.right = BSTNode(3)
print(f"Valid: {is_valid_bst(root)}")

root2 = BSTNode(5)
root2.left = BSTNode(1)
root2.right = BSTNode(4)
root2.right.left = BSTNode(3)
root2.right.right = BSTNode(6)
print(f"Valid: {is_valid_bst(root2)}")

# Lowest common ancestor
def lca(root, p, q):
    if not root or root.val in (p.val, q.val):
        return root
    if p.val < root.val and q.val < root.val:
        return lca(root.left, p, q)
    if p.val > root.val and q.val > root.val:
        return lca(root.right, p, q)
    return root

print("\nLCA:")
root = BSTNode(6)
root.left = BSTNode(2)
root.right = BSTNode(8)
root.left.left = BSTNode(0)
root.left.right = BSTNode(4)
root.right.left = BSTNode(7)
root.right.right = BSTNode(9)
p = root.left
q = root.left.right
lca_node = lca(root, p, q)
print(f"LCA of 2 and 4: {lca_node.val}")
