# Time-Series Databases (InfluxDB, TimescaleDB)

## What You'll Learn

- InfluxDB integration with Node.js
- TimescaleDB with PostgreSQL
- Time-series data modeling
- Aggregation and downsampling
- Retention policies

## InfluxDB Integration

```bash
npm install @influxdata/influxdb-client
```

```javascript
import { InfluxDB, Point } from '@influxdata/influxdb-client';

const client = new InfluxDB({
    url: process.env.INFLUX_URL || 'http://localhost:8086',
    token: process.env.INFLUX_TOKEN,
});

const writeApi = client.getWriteApi(process.env.INFLUX_ORG, process.env.INFLUX_BUCKET, 'ns');
const queryApi = client.getQueryApi(process.env.INFLUX_ORG);

// Write metrics
async function writeMetric(measurement, tags, fields, timestamp = new Date()) {
    const point = new Point(measurement);

    for (const [key, value] of Object.entries(tags)) {
        point.tag(key, value);
    }

    for (const [key, value] of Object.entries(fields)) {
        if (typeof value === 'number') {
            point.floatField(key, value);
        } else if (typeof value === 'string') {
            point.stringField(key, value);
        } else if (typeof value === 'boolean') {
            point.booleanField(key, value);
        }
    }

    point.timestamp(timestamp);
    writeApi.writePoint(point);
}

// Batch write
async function writeMetrics(metrics) {
    for (const m of metrics) {
        await writeMetric(m.measurement, m.tags, m.fields, m.timestamp);
    }
    await writeApi.flush();
}

// Query metrics
async function queryMetrics(bucket, measurement, timeRange = '-1h') {
    const fluxQuery = `
        from(bucket: "${bucket}")
            |> range(start: ${timeRange})
            |> filter(fn: (r) => r._measurement == "${measurement}")
            |> filter(fn: (r) => r._field == "value")
            |> aggregateWindow(every: 1m, fn: mean, createEmpty: false)
            |> yield(name: "mean")
    `;

    const results = [];
    for await (const { values, tableMeta } of queryApi.iterateRows(fluxQuery)) {
        results.push(tableMeta.toObject(values));
    }
    return results;
}

// Application metrics
async function recordRequestMetrics(method, path, duration, statusCode) {
    await writeMetric('http_requests', {
        method,
        path,
        status: String(statusCode),
    }, {
        duration_ms: duration,
        count: 1,
    });
}
```

## TimescaleDB (PostgreSQL Extension)

```sql
-- Enable TimescaleDB
CREATE EXTENSION IF NOT EXISTS timescaledb;

-- Create hypertable
CREATE TABLE metrics (
    time TIMESTAMPTZ NOT NULL,
    device_id TEXT NOT NULL,
    metric_name TEXT NOT NULL,
    value DOUBLE PRECISION
);

SELECT create_hypertable('metrics', 'time');

-- Create indexes
CREATE INDEX idx_metrics_device ON metrics (device_id, time DESC);
CREATE INDEX idx_metrics_name ON metrics (metric_name, time DESC);

-- Continuous aggregate (materialized view)
CREATE MATERIALIZED VIEW metrics_hourly
WITH (timescaledb.continuous) AS
SELECT 
    time_bucket('1 hour', time) as bucket,
    device_id,
    metric_name,
    AVG(value) as avg_value,
    MIN(value) as min_value,
    MAX(value) as max_value,
    COUNT(*) as sample_count
FROM metrics
GROUP BY bucket, device_id, metric_name;

-- Retention policy
SELECT add_retention_policy('metrics', INTERVAL '90 days');

-- Compression policy
ALTER TABLE metrics SET (
    timescaledb.compress,
    timescaledb.compress_segmentby = 'device_id',
    timescaledb.compress_orderby = 'time DESC'
);
SELECT add_compression_policy('metrics', INTERVAL '7 days');
```

```javascript
import { Pool } from 'pg';

const pool = new Pool({ /* config */ });

// Write time-series data
async function writeTimeSeries(deviceId, metricName, value) {
    await pool.query(
        'INSERT INTO metrics (time, device_id, metric_name, value) VALUES (NOW(), $1, $2, $3)',
        [deviceId, metricName, value]
    );
}

// Batch write
async function batchWriteTimeSeries(records) {
    const values = records.map((r, i) =>
        `($${i * 4 + 1}, $${i * 4 + 2}, $${i * 4 + 3}, $${i * 4 + 4})`
    ).join(', ');

    const params = records.flatMap(r => [r.time, r.deviceId, r.metricName, r.value]);

    await pool.query(
        `INSERT INTO metrics (time, device_id, metric_name, value) VALUES ${values}`,
        params
    );
}

// Query with time bucketing
async function queryAggregated(deviceId, metricName, interval = '1 hour', duration = '24 hours') {
    const { rows } = await pool.query(`
        SELECT 
            time_bucket($1, time) as bucket,
            AVG(value) as avg_value,
            MIN(value) as min_value,
            MAX(value) as max_value
        FROM metrics
        WHERE device_id = $2 
            AND metric_name = $3
            AND time > NOW() - $4::INTERVAL
        GROUP BY bucket
        ORDER BY bucket DESC
    `, [interval, deviceId, metricName, duration]);

    return rows;
}
```

## Best Practices Checklist

- [ ] Use appropriate time granularity for your use case
- [ ] Implement retention policies to manage storage
- [ ] Use continuous aggregates for common queries
- [ ] Batch writes for high-throughput ingestion
- [ ] Compress old data automatically
- [ ] Index on device/entity + time for fast queries

## Cross-References

- See [Database Performance](../02-database-performance-optimization/01-query-optimization.md) for optimization
- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for caching
- See [Monitoring](../03-performance-monitoring-analysis/01-apm-setup.md) for metrics

## Next Steps

Continue to [Search Engines](./04-search-engines.md) for full-text search.
