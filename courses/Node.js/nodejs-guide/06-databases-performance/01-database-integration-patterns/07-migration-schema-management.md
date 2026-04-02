# Database Migration and Schema Management

## What You'll Learn

- Migration frameworks and tools
- Schema versioning strategies
- Zero-downtime migration patterns
- Rollback and recovery procedures
- Schema drift detection and management

## Migration with Knex.js

```bash
npm install knex pg
```

```bash
# Create migration
npx knex migrate:make create_users_table --knexfile knexfile.js

# Run migrations
npx knex migrate:latest --knexfile knexfile.js

# Rollback
npx knex migrate:rollback --knexfile knexfile.js
```

```javascript
// knexfile.js
export default {
    development: {
        client: 'pg',
        connection: {
            host: process.env.PG_HOST || 'localhost',
            database: process.env.PG_DATABASE,
            user: process.env.PG_USER,
            password: process.env.PG_PASSWORD,
        },
        migrations: {
            directory: './migrations',
            tableName: 'knex_migrations',
        },
    },
    production: {
        client: 'pg',
        connection: {
            host: process.env.PG_HOST,
            database: process.env.PG_DATABASE,
            user: process.env.PG_USER,
            password: process.env.PG_PASSWORD,
            ssl: { rejectUnauthorized: false },
        },
        pool: { min: 2, max: 20 },
        migrations: {
            directory: './migrations',
            tableName: 'knex_migrations',
        },
    },
};
```

```javascript
// migrations/20240101_create_users_table.js
export async function up(knex) {
    await knex.schema.createTable('users', (table) => {
        table.increments('id').primary();
        table.string('name', 255).notNullable();
        table.string('email', 255).notNullable().unique();
        table.string('role', 50).defaultTo('user');
        table.timestamps(true, true); // created_at, updated_at
    });

    await knex.schema.createTable('posts', (table) => {
        table.increments('id').primary();
        table.integer('user_id').unsigned().references('id').inTable('users').onDelete('CASCADE');
        table.string('title', 500).notNullable();
        table.text('content');
        table.boolean('published').defaultTo(false);
        table.timestamps(true, true);
    });

    await knex.raw('CREATE INDEX idx_posts_user_id ON posts(user_id)');
    await knex.raw('CREATE INDEX idx_posts_published ON posts(published) WHERE published = true');
}

export async function down(knex) {
    await knex.schema.dropTableIfExists('posts');
    await knex.schema.dropTableIfExists('users');
}
```

## Migration with Prisma

```bash
npm install prisma @prisma/client
npx prisma init
```

```prisma
// prisma/schema.prisma
datasource db {
    provider = "postgresql"
    url      = env("DATABASE_URL")
}

generator client {
    provider = "prisma-client-js"
}

model User {
    id        Int      @id @default(autoincrement())
    email     String   @unique
    name      String
    role      String   @default("user")
    posts     Post[]
    createdAt DateTime @default(now()) @map("created_at")
    updatedAt DateTime @updatedAt @map("updated_at")

    @@map("users")
}

model Post {
    id         Int      @id @default(autoincrement())
    title      String
    content    String?
    published  Boolean  @default(false)
    userId     Int      @map("user_id")
    user       User     @relation(fields: [userId], references: [id], onDelete: Cascade)
    createdAt  DateTime @default(now()) @map("created_at")

    @@index([userId])
    @@map("posts")
}
```

```bash
# Create and apply migration
npx prisma migrate dev --name add_posts_table

# Deploy to production
npx prisma migrate deploy

# Check migration status
npx prisma migrate status

# Reset database (development only)
npx prisma migrate reset
```

## Migration with node-pg-migrate

```bash
npm install node-pg-migrate
```

```javascript
// migrations/1704067200000_create-users.js
export const shorthands = {};

export async function up(pgm) {
    pgm.createTable('users', {
        id: 'id',
        name: { type: 'varchar(255)', notNull: true },
        email: { type: 'varchar(255)', notNull: true, unique: true },
        role: { type: 'varchar(50)', default: 'user' },
        created_at: {
            type: 'timestamp',
            notNull: true,
            default: pgm.func('current_timestamp'),
        },
    });

    pgm.createIndex('users', 'email');
    pgm.createIndex('users', 'created_at');
}

export async function down(pgm) {
    pgm.dropTable('users');
}
```

```json
// package.json scripts
{
    "scripts": {
        "migrate": "node-pg-migrate",
        "migrate:up": "node-pg-migrate up",
        "migrate:down": "node-pg-migrate down",
        "migrate:redo": "node-pg-migrate redo",
        "migrate:status": "node-pg-migrate status"
    }
}
```

## Zero-Downtime Migration Pattern

```javascript
// Phase 1: Add new column (nullable, backward-compatible)
export async function up_phase1(knex) {
    await knex.schema.alterTable('users', (table) => {
        table.string('username', 100).nullable();
    });
}

// Phase 2: Backfill data (run in batches)
export async function up_phase2_backfill(knex) {
    const BATCH_SIZE = 1000;
    let offset = 0;
    
    while (true) {
        const users = await knex('users')
            .whereNull('username')
            .limit(BATCH_SIZE)
            .offset(offset);

        if (users.length === 0) break;

        const updates = users.map(u => ({
            id: u.id,
            username: u.email.split('@')[0],
        }));

        await knex.transaction(async (trx) => {
            for (const u of updates) {
                await trx('users')
                    .where('id', u.id)
                    .update({ username: u.username });
            }
        });

        offset += users.length;
        console.log(`Backfilled ${offset} users`);
    }
}

// Phase 3: Add NOT NULL constraint (after backfill complete)
export async function up_phase3(knex) {
    await knex.schema.alterTable('users', (table) => {
        table.string('username', 100).notNullable().alter();
    });
}

// Phase 4: Add unique constraint
export async function up_phase4(knex) {
    await knex.raw('ALTER TABLE users ADD CONSTRAINT users_username_unique UNIQUE (username)');
}
```

## Schema Drift Detection

```javascript
import { Pool } from 'pg';

class SchemaInspector {
    constructor(pool) {
        this.pool = pool;
    }

    async getTables() {
        const { rows } = await this.pool.query(`
            SELECT table_name, table_type
            FROM information_schema.tables
            WHERE table_schema = 'public'
            ORDER BY table_name
        `);
        return rows;
    }

    async getColumns(tableName) {
        const { rows } = await this.pool.query(`
            SELECT column_name, data_type, is_nullable, column_default,
                   character_maximum_length
            FROM information_schema.columns
            WHERE table_schema = 'public' AND table_name = $1
            ORDER BY ordinal_position
        `, [tableName]);
        return rows;
    }

    async getIndexes(tableName) {
        const { rows } = await this.pool.query(`
            SELECT indexname, indexdef
            FROM pg_indexes
            WHERE schemaname = 'public' AND tablename = $1
        `, [tableName]);
        return rows;
    }

    async getConstraints(tableName) {
        const { rows } = await this.pool.query(`
            SELECT constraint_name, constraint_type
            FROM information_schema.table_constraints
            WHERE table_schema = 'public' AND table_name = $1
        `, [tableName]);
        return rows;
    }

    async generateSchemaSnapshot() {
        const tables = await this.getTables();
        const snapshot = {};

        for (const table of tables) {
            snapshot[table.table_name] = {
                columns: await this.getColumns(table.table_name),
                indexes: await this.getIndexes(table.table_name),
                constraints: await this.getConstraints(table.table_name),
            };
        }

        return snapshot;
    }

    async compareSnapshots(expected, actual) {
        const differences = [];

        for (const table in expected) {
            if (!actual[table]) {
                differences.push({ type: 'missing_table', table });
                continue;
            }

            const expectedCols = new Set(expected[table].columns.map(c => c.column_name));
            const actualCols = new Set(actual[table].columns.map(c => c.column_name));

            for (const col of expectedCols) {
                if (!actualCols.has(col)) {
                    differences.push({ type: 'missing_column', table, column: col });
                }
            }
        }

        return differences;
    }
}
```

## Migration Runner with Locking

```javascript
class MigrationRunner {
    constructor(pool, migrationsDir) {
        this.pool = pool;
        this.migrationsDir = migrationsDir;
    }

    async ensureMigrationsTable() {
        await this.pool.query(`
            CREATE TABLE IF NOT EXISTS schema_migrations (
                version VARCHAR(255) PRIMARY KEY,
                name VARCHAR(255) NOT NULL,
                applied_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )
        `);
    }

    async acquireLock() {
        const client = await this.pool.connect();
        try {
            // Advisory lock (PostgreSQL)
            await client.query('SELECT pg_advisory_lock(123456789)');
            return client;
        } catch (err) {
            client.release();
            throw err;
        }
    }

    async releaseLock(client) {
        try {
            await client.query('SELECT pg_advisory_unlock(123456789)');
        } finally {
            client.release();
        }
    }

    async getAppliedVersions() {
        const { rows } = await this.pool.query(
            'SELECT version FROM schema_migrations ORDER BY version'
        );
        return new Set(rows.map(r => r.version));
    }

    async runPending() {
        await this.ensureMigrationsTable();
        const lockClient = await this.acquireLock();

        try {
            const applied = await this.getAppliedVersions();
            const migrationFiles = await this.getMigrationFiles();
            const pending = migrationFiles.filter(f => !applied.has(f.version));

            if (pending.length === 0) {
                console.log('No pending migrations');
                return;
            }

            for (const migration of pending) {
                console.log(`Applying migration: ${migration.name}`);
                
                await this.pool.query('BEGIN');
                try {
                    const { up } = await import(migration.path);
                    await up(this.pool);
                    await this.pool.query(
                        'INSERT INTO schema_migrations (version, name) VALUES ($1, $2)',
                        [migration.version, migration.name]
                    );
                    await this.pool.query('COMMIT');
                    console.log(`Applied: ${migration.name}`);
                } catch (err) {
                    await this.pool.query('ROLLBACK');
                    throw new Error(`Migration ${migration.name} failed: ${err.message}`);
                }
            }
        } finally {
            await this.releaseLock(lockClient);
        }
    }

    async getMigrationFiles() {
        const fs = await import('node:fs/promises');
        const files = await fs.readdir(this.migrationsDir);
        return files
            .filter(f => f.endsWith('.js'))
            .map(f => ({
                version: f.split('_')[0],
                name: f.replace('.js', ''),
                path: `file://${this.migrationsDir}/${f}`,
            }))
            .sort((a, b) => a.version.localeCompare(b.version));
    }
}
```

## Best Practices Checklist

- [ ] Use a migration framework (Knex, Prisma, node-pg-migrate)
- [ ] Each migration should be atomic (single transaction)
- [ ] Always provide `down` migrations for rollback
- [ ] Use advisory locks to prevent concurrent migrations
- [ ] Test migrations against a copy of production data
- [ ] Use phased migrations for zero-downtime deployments
- [ ] Backfill large tables in batches
- [ ] Keep migrations small and focused
- [ ] Version control all migration files
- [ ] Run schema drift detection in CI/CD

## Cross-References

- See [Transaction Management](./05-transaction-management.md) for transaction patterns
- See [Error Handling](./06-error-handling-recovery.md) for migration error recovery
- See [CI/CD](../../../26-cicd-github-actions/) for migration in pipelines

## Next Steps

Continue to [Database Performance Optimization](../02-database-performance-optimization/01-query-optimization.md) for query optimization.
