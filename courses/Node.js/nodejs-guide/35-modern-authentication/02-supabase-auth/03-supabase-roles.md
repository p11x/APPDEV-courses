# Supabase Roles

## What You'll Learn

- How to implement role-based access with Supabase
- How to use Row Level Security (RLS)
- How to create custom roles
- How to protect routes by role

## Row Level Security

```sql
-- Enable RLS on tables
ALTER TABLE posts ENABLE ROW LEVEL SECURITY;

-- Policy: users can only see their own posts
CREATE POLICY "Users can view own posts" ON posts
  FOR SELECT USING (auth.uid() = user_id);

-- Policy: users can insert their own posts
CREATE POLICY "Users can insert own posts" ON posts
  FOR INSERT WITH CHECK (auth.uid() = user_id);

-- Policy: admins can see all posts
CREATE POLICY "Admins can see all posts" ON posts
  FOR SELECT USING (
    EXISTS (
      SELECT 1 FROM user_roles
      WHERE user_id = auth.uid() AND role = 'admin'
    )
  );
```

## Custom Roles

```ts
// Add role to user metadata
await supabase.auth.updateUser({
  data: { role: 'admin' },
});

// Check role in RLS
// In SQL: auth.jwt() -> 'user_metadata' ->> 'role' = 'admin'
```

## Next Steps

For security, continue to [Supabase Security](./04-supabase-security.md).
