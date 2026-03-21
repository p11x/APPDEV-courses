# Injection Attacks

## 📌 What You'll Learn

- SQL, NoSQL, and command injection
- Prevention techniques

## 💻 Code Example

```js
// SQL Injection - BAD
app.get('/users', (req, res) => {
  const query = `SELECT * FROM users WHERE id = ${req.query.id}`;
  // DANGEROUS!
});

// SQL Injection - GOOD (Parameterized)
app.get('/users', async (req, res) => {
  const result = await db.query(
    'SELECT * FROM users WHERE id = $1',
    [req.query.id]
  );
  res.json(result.rows);
});

// NoSQL Injection - Use validation
import Joi from 'joi';

const schema = Joi.object({
  email: Joi.string().email()
});

app.post('/login', (req, res) => {
  const { error } = schema.validate(req.body);
  if (error) return res.status(400).send(error);
  // Continue...
});
```
