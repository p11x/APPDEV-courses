# Repository Pattern Deep Dive

## 📌 What You'll Learn

- What the repository pattern is and why it abstracts data access
- Creating interfaces that define data operations
- Implementing repositories for different databases
- Using repositories for unit testing with mocks

## 🧠 Concept Explained (Plain English)

The repository pattern creates an abstraction layer between your Express routes and the database. Instead of having SQL queries or MongoDB calls directly in your route handlers, you call methods on a repository object.

Think of it like a library:
- You don't go directly to the archives to find a book
- You ask a librarian (the repository) who knows where everything is
- If the archives change (new storage system), the librarian still helps

**Why use repositories?**

1. **Swappable databases**: Change from PostgreSQL to MongoDB without touching route code
2. **Testability**: Replace real database with mock objects in tests
3. **Organization**: Business logic stays separate from data access
4. **Reuse**: Shared query logic across endpoints

**Key components:**
- **Interface/Contract**: Defines what methods exist (find, create, update, delete)
- **Implementation**: Actual database code
- **Usage**: Routes only know about the interface

## 💻 Code Example

```js
// ES Module syntax
import express from 'express';

const app = express();


// ============================================
// Repository Interface (Contract)
// ============================================

/*
// In TypeScript, this would be an interface:
// interface UserRepository {
//   findById(id: string): Promise<User | null>;
//   findAll(): Promise<User[]>;
//   findByEmail(email: string): Promise<User | null>;
//   create(user: CreateUserDTO): Promise<User>;
//   update(id: string, updates: UpdateUserDTO): Promise<User>;
//   delete(id: string): Promise<void>;
// }
*/


// ============================================
// In-Memory Implementation (for demo)
// ============================================

class InMemoryUserRepository {
  constructor() {
    this.users = new Map();
  }
  
  async findById(id) {
    return this.users.get(id) || null;
  }
  
  async findAll() {
    return Array.from(this.users.values());
  }
  
  async findByEmail(email) {
    for (const user of this.users.values()) {
      if (user.email === email) return user;
    }
    return null;
  }
  
  async create(userData) {
    const id = 'user-' + Date.now();
    const user = {
      id,
      ...userData,
      createdAt: new Date().toISOString()
    };
    this.users.set(id, user);
    return user;
  }
  
  async update(id, updates) {
    const user = this.users.get(id);
    if (!user) throw new Error('User not found');
    
    const updated = { ...user, ...updates, updatedAt: new Date().toISOString() };
    this.users.set(id, updated);
    return updated;
  }
  
  async delete(id) {
    if (!this.users.has(id)) throw new Error('User not found');
    this.users.delete(id);
  }
}


// ============================================
// PostgreSQL Implementation (example)
// ============================================

/*
class PostgreSQLUserRepository {
  constructor(pool) {
    this.pool = pool;
  }
  
  async findById(id) {
    const result = await this.pool.query(
      'SELECT * FROM users WHERE id = $1',
      [id]
    );
    return result.rows[0] || null;
  }
  
  async findAll() {
    const result = await this.pool.query('SELECT * FROM users');
    return result.rows;
  }
  
  async findByEmail(email) {
    const result = await this.pool.query(
      'SELECT * FROM users WHERE email = $1',
      [email]
    );
    return result.rows[0] || null;
  }
  
  async create(userData) {
    const result = await this.pool.query(
      `INSERT INTO users (name, email) VALUES ($1, $2) 
       RETURNING *`,
      [userData.name, userData.email]
    );
    return result.rows[0];
  }
  
  async update(id, updates) {
    const fields = Object.keys(updates);
    const values = Object.values(updates);
    const setClause = fields.map((f, i) => `${f} = $${i + 2}`).join(', ');
    
    const result = await this.pool.query(
      `UPDATE users SET ${setClause}, updated_at = NOW() 
       WHERE id = $1 RETURNING *`,
      [id, ...values]
    );
    return result.rows[0];
  }
  
  async delete(id) {
    await this.pool.query('DELETE FROM users WHERE id = $1', [id]);
  }
}
*/


// ============================================
// Repository Factory (switch implementations)
// ============================================

function createUserRepository(type = 'memory') {
  switch (type) {
    case 'memory':
      return new InMemoryUserRepository();
    // case 'postgres':
    //   return new PostgreSQLUserRepository(dbPool);
    default:
      throw new Error(`Unknown repository type: ${type}`);
  }
}

// Initialize based on environment
const userRepository = createUserRepository(process.env.DB_TYPE || 'memory');


// ============================================
// Dependency Injection via Middleware
// ============================================

// Attach repository to requests
app.use((req, res, next) => {
  req.userRepository = userRepository;
  next();
});


// ============================================
// Routes (clean and simple)
// ============================================

app.use(express.json());

// GET all users
app.get('/api/users', async (req, res) => {
  const users = await req.userRepository.findAll();
  res.json({ users, count: users.length });
});

// GET single user
app.get('/api/users/:id', async (req, res) => {
  const user = await req.userRepository.findById(req.params.id);
  
  if (!user) {
    return res.status(404).json({ error: 'User not found' });
  }
  
  res.json(user);
});

// GET user by email
app.get('/api/users/email/:email', async (req, res) => {
  const user = await req.userRepository.findByEmail(req.params.email);
  
  if (!user) {
    return res.status(404).json({ error: 'User not found' });
  }
  
  res.json(user);
});

// POST create user
app.post('/api/users', async (req, res) => {
  const { name, email } = req.body || {};
  
  // Validation (could be in a separate layer)
  if (!name || !email) {
    return res.status(400).json({ error: 'Name and email required' });
  }
  
  // Check if email exists (using repository method)
  const existing = await req.userRepository.findByEmail(email);
  if (existing) {
    return res.status(409).json({ error: 'Email already exists' });
  }
  
  // Create via repository
  const user = await req.userRepository.create({ name, email });
  
  res.status(201).json(user);
});

// PUT update user
app.put('/api/users/:id', async (req, res) => {
  const { name, email } = req.body || {};
  
  try {
    const user = await req.userRepository.update(req.params.id, { name, email });
    res.json(user);
  } catch (error) {
    res.status(404).json({ error: error.message });
  }
});

// DELETE user
app.delete('/api/users/:id', async (req, res) => {
  try {
    await req.userRepository.delete(req.params.id);
    res.status(204).send();
  } catch (error) {
    res.status(404).json({ error: error.message });
  }
});


// ============================================
// Product Repository (another example)
// ============================================

class ProductRepository {
  constructor() {
    this.products = new Map([
      ['prod-1', { id: 'prod-1', name: 'Widget', price: 9.99, stock: 100 }],
      ['prod-2', { id: 'prod-2', name: 'Gadget', price: 19.99, stock: 50 }],
      ['prod-3', { id: 'prod-3', name: 'Gizmo', price: 29.99, stock: 25 }]
    ]);
  }
  
  async findById(id) {
    return this.products.get(id) || null;
  }
  
  async findAll() {
    return Array.from(this.products.values());
  }
  
  async findByName(name) {
    const search = name.toLowerCase();
    return Array.from(this.products.values()).filter(
      p => p.name.toLowerCase().includes(search)
    );
  }
  
  async updateStock(id, quantity) {
    const product = this.products.get(id);
    if (!product) throw new Error('Product not found');
    
    product.stock += quantity;
    return product;
  }
}

const productRepository = new ProductRepository();

app.get('/api/products', async (req, res) => {
  const { search } = req.query;
  
  if (search) {
    const products = await productRepository.findByName(search);
    return res.json({ products });
  }
  
  const products = await productRepository.findAll();
  res.json({ products });
});

app.get('/api/products/:id', async (req, res) => {
  const product = await productRepository.findById(req.params.id);
  
  if (!product) {
    return res.status(404).json({ error: 'Product not found' });
  }
  
  res.json(product);
});

app.patch('/api/products/:id/stock', async (req, res) => {
  const { quantity } = req.body || {};
  
  try {
    const product = await productRepository.updateStock(req.params.id, quantity);
    res.json(product);
  } catch (error) {
    res.status(404).json({ error: error.message });
  }
});


app.get('/health', (req, res) => {
  res.json({ status: 'ok' });
});


const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14-28 | Interface definition | Documents what methods repository must have |
| 35-65 | `InMemoryUserRepository` | Demo implementation with Map |
| 87-94 | `findById` method | Retrieves by primary key |
| 97-100 | `findAll` method | Gets all records |
| 103-109 | `findByEmail` method | Query by unique field |
| 112-119 | `create` method | Insert new record |
| 122-130 | `update` method | Update existing record |
| 133-137 | `delete` method | Remove record |
| 145-157 | Repository factory | Switches between implementations |
| 163-168 | Middleware injection | Attaches repo to requests |
| 174-190 | GET routes | Clean route handlers using repo |
| 192-206 | POST route | Creation with validation |
| 209-221 | PUT/DELETE routes | Update and delete operations |
| 227-263 | Product repository | Another example repository |

## ⚠️ Common Mistakes

### 1. Leaking database logic into routes

**What it is**: Routes doing SQL queries or using ORM directly.

**Why it happens**: Skipping repository layer for simplicity.

**How to fix it**: Always go through repository methods.

### 2. Not having consistent interfaces

**What it is**: Different methods for similar operations across repositories.

**Why it happens**: Ad-hoc development without planning.

**How to fix it**: Define interfaces upfront and follow them.

### 3. Creating repositories per request

**What it is**: New repository instance created for each request.

**Why it happens**: Creating in middleware without caching.

**How to fix it**: Create once and reuse (singleton or dependency injection).

## ✅ Quick Recap

- Repository pattern abstracts database operations
- Routes only know interface, not implementation
- Easy to swap databases (PostgreSQL, MongoDB, etc.)
- Simplifies unit testing with mock repositories
- Create repositories once, inject via middleware

## 🔗 What's Next

Now learn about [Domain-Driven Design Intro](./03_domain-driven-design-intro.md).
