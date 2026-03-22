# Example288: Dunder Methods (Magic Methods)
class Book:
    def __init__(self, title, pages):
        self.title = title
        self.pages = pages
    
    def __str__(self):
        return f"Book: {self.title}"
    
    def __repr__(self):
        return f"Book('{self.title}', {self.pages})"
    
    def __len__(self):
        return self.pages
    
    def __add__(self, other):
        return self.pages + other.pages
    
    def __eq__(self, other):
        return self.title == other.title and self.pages == other.pages
    
    def __lt__(self, other):
        return self.pages < other.pages

print("Dunder Methods:")
book1 = Book("Python", 300)
book2 = Book("Java", 250)

print(f"str: {str(book1)}")
print(f"repr: {repr(book1)}")
print(f"len: {len(book1)}")
print(f"book1 + book2 (pages): {book1 + book2}")
print(f"book1 == book2: {book1 == book2}")
print(f"book1 < book2: {book1 < book2}")

# Container dunders
class Stack:
    def __init__(self):
        self.items = []
    
    def push(self, item):
        self.items.append(item)
    
    def pop(self):
        return self.items.pop()
    
    def __contains__(self, item):
        return item in self.items
    
    def __iter__(self):
        return iter(self.items)
    
    def __len__(self):
        return len(self.items)

print("\nContainer Dunders:")
stack = Stack()
stack.push(1)
stack.push(2)
stack.push(3)
print(f"Length: {len(stack)}")
print(f"2 in stack: {2 in stack}")
print(f"Iteration: {[x for x in stack]}")
