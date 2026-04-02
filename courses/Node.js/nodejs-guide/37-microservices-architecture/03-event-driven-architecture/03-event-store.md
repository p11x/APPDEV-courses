# Event Store

## What You'll Learn

- How to implement a persistent event store
- How to use PostgreSQL as an event store
- How to handle event versioning
- How to snapshot aggregates

## PostgreSQL Event Store

```ts
// event-store-pg.ts

import { Pool } from 'pg';

class PostgresEventStore {
  constructor(private pool: Pool) {}

  async init() {
    await this.pool.query(`
      CREATE TABLE IF NOT EXISTS events (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        aggregate_id TEXT NOT NULL,
        aggregate_type TEXT NOT NULL,
        event_type TEXT NOT NULL,
        data JSONB NOT NULL,
        version INTEGER NOT NULL,
        created_at TIMESTAMPTZ DEFAULT NOW(),
        UNIQUE(aggregate_id, version)
      );

      CREATE INDEX IF NOT EXISTS idx_events_aggregate
        ON events(aggregate_id, version);
    `);
  }

  async append(aggregateId: string, type: string, data: Record<string, unknown>, expectedVersion: number) {
    const result = await this.pool.query(
      `INSERT INTO events (aggregate_id, aggregate_type, event_type, data, version)
       VALUES ($1, $2, $3, $4, $5)
       ON CONFLICT (aggregate_id, version) DO NOTHING
       RETURNING *`,
      [aggregateId, type.split('.')[0], type, JSON.stringify(data), expectedVersion + 1]
    );

    if (result.rowCount === 0) {
      throw new Error('Concurrency conflict — version mismatch');
    }

    return result.rows[0];
  }

  async getEvents(aggregateId: string, fromVersion = 0) {
    const result = await this.pool.query(
      `SELECT * FROM events WHERE aggregate_id = $1 AND version > $2 ORDER BY version`,
      [aggregateId, fromVersion]
    );

    return result.rows.map((row) => ({
      ...row,
      data: JSON.parse(row.data),
    }));
  }
}
```

## Snapshots

```ts
// Store periodic snapshots to avoid replaying all events

async function saveSnapshot(aggregateId: string, state: unknown, version: number) {
  await pool.query(
    `INSERT INTO snapshots (aggregate_id, state, version) VALUES ($1, $2, $3)
     ON CONFLICT (aggregate_id) DO UPDATE SET state = $2, version = $3`,
    [aggregateId, JSON.stringify(state), version]
  );
}

async function loadAggregate(aggregateId: string) {
  // Load latest snapshot
  const snapshot = await pool.query(
    'SELECT * FROM snapshots WHERE aggregate_id = $1', [aggregateId]
  );

  const fromVersion = snapshot.rows[0]?.version || 0;
  const state = snapshot.rows[0]?.state ? JSON.parse(snapshot.rows[0].state) : {};

  // Load events after snapshot
  const events = await eventStore.getEvents(aggregateId, fromVersion);

  // Apply events to snapshot state
  return events.reduce(applyEvent, state);
}
```

## Next Steps

For message patterns, continue to [Message Patterns](./04-message-patterns.md).
