# Example262: Segment Tree Basics
class SegmentTree:
    def __init__(self, arr):
        self.n = len(arr)
        self.tree = [0] * (4 * self.n)
        if self.n > 0:
            self.build(arr, 0, 0, self.n - 1)
    
    def build(self, arr, node, start, end):
        if start == end:
            self.tree[node] = arr[start]
        else:
            mid = (start + end) // 2
            self.build(arr, 2*node+1, start, mid)
            self.build(arr, 2*node+2, mid+1, end)
            self.tree[node] = self.tree[2*node+1] + self.tree[2*node+2]
    
    def query(self, left, right):
        return self._query(0, 0, self.n-1, left, right)
    
    def _query(self, node, start, end, left, right):
        if right < start or left > end:
            return 0
        if left <= start and end <= right:
            return self.tree[node]
        mid = (start + end) // 2
        return (self._query(2*node+1, start, mid, left, right) +
                self._query(2*node+2, mid+1, end, left, right))
    
    def update(self, idx, value):
        self._update(0, 0, self.n-1, idx, value)
    
    def _update(self, node, start, end, idx, value):
        if start == end:
            self.tree[node] = value
        else:
            mid = (start + end) // 2
            if idx <= mid:
                self._update(2*node+1, start, mid, idx, value)
            else:
                self._update(2*node+2, mid+1, end, idx, value)
            self.tree[node] = self.tree[2*node+1] + self.tree[2*node+2]

print("Segment Tree:")
arr = [1, 3, 5, 7, 9, 11]
st = SegmentTree(arr)
print(f"Array: {arr}")
print(f"Sum [1,3]: {st.query(1, 3)}")
print(f"Sum [0,5]: {st.query(0, 5)}")
st.update(2, 10)
print(f"After update idx 2 to 10:")
print(f"Sum [0,5]: {st.query(0, 5)}")

# Range maximum query
class SegmentTreeMax:
    def __init__(self, arr):
        self.n = len(arr)
        self.tree = [0] * (4 * self.n)
        if self.n > 0:
            self.build(arr, 0, 0, self.n - 1)
    
    def build(self, arr, node, start, end):
        if start == end:
            self.tree[node] = arr[start]
        else:
            mid = (start + end) // 2
            self.build(arr, 2*node+1, start, mid)
            self.build(arr, 2*node+2, mid+1, end)
            self.tree[node] = max(self.tree[2*node+1], self.tree[2*node+2])
    
    def query(self, left, right):
        return self._query(0, 0, self.n-1, left, right)
    
    def _query(self, node, start, end, left, right):
        if right < start or left > end:
            return float('-inf')
        if left <= start and end <= right:
            return self.tree[node]
        mid = (start + end) // 2
        return max(self._query(2*node+1, start, mid, left, right),
                   self._query(2*node+2, mid+1, end, left, right))

print("\nSegment Tree (Max):")
arr = [1, 3, 5, 7, 9, 11]
st = SegmentTreeMax(arr)
print(f"Array: {arr}")
print(f"Max [1,3]: {st.query(1, 3)}")
print(f"Max [0,5]: {st.query(0, 5)}")
