# Data Compression and Storage Optimization

## What You'll Learn

- Data compression techniques in Node.js
- Database-level compression
- Storage optimization strategies
- Compression benchmarks
- Streaming compression pipelines

## Node.js Compression

```javascript
import { createGzip, createGunzip, createBrotliCompress, createBrotliDecompress } from 'node:zlib';
import { pipeline } from 'node:stream/promises';
import { createReadStream, createWriteStream } from 'node:fs';

// Gzip compression
async function compressFile(inputPath, outputPath) {
    await pipeline(
        createReadStream(inputPath),
        createGzip({ level: 6 }),
        createWriteStream(outputPath)
    );
}

async function decompressFile(inputPath, outputPath) {
    await pipeline(
        createReadStream(inputPath),
        createGunzip(),
        createWriteStream(outputPath)
    );
}

// Brotli compression (better ratio)
async function brotliCompress(inputPath, outputPath) {
    await pipeline(
        createReadStream(inputPath),
        createBrotliCompress({
            params: {
                [zlib.constants.BROTLI_PARAM_QUALITY]: 4,
                [zlib.constants.BROTLI_PARAM_SIZE_HINT]: 0,
            },
        }),
        createWriteStream(outputPath)
    );
}

// In-memory compression
import { gzipSync, gunzipSync, brotliCompressSync, brotliDecompressSync } from 'node:zlib';

function compressData(data) {
    const json = JSON.stringify(data);
    return gzipSync(Buffer.from(json));
}

function decompressData(compressed) {
    const decompressed = gunzipSync(compressed);
    return JSON.parse(decompressed.toString());
}
```

## Database Compression Strategies

```sql
-- PostgreSQL TOAST (automatic for large values)
-- Large text/bytea fields are automatically compressed

-- PostgreSQL column compression (pg 14+)
ALTER TABLE logs SET (toast_compression = lz4);

-- Enable table compression (PostgreSQL with extensions)
CREATE EXTENSION IF NOT EXISTS pg_stat_statements;

-- Compress JSONB storage
ALTER TABLE events ALTER COLUMN payload SET STORAGE EXTERNAL;

-- MySQL compressed tables
ALTER TABLE logs ROW_FORMAT=COMPRESSED KEY_BLOCK_SIZE=8;

-- MongoDB compressed collection (WiredTiger)
db.createCollection("logs", {
    storageEngine: {
        wiredTiger: {
            configString: "block_compressor=snappy"
        }
    }
});
```

## Compressed JSON Storage

```javascript
import { gzipSync, gunzipSync } from 'node:zlib';

class CompressedStorage {
    constructor(pool) {
        this.pool = pool;
    }

    async store(key, data) {
        const compressed = gzipSync(JSON.stringify(data));
        await this.pool.query(
            'INSERT INTO compressed_data (key, data, size) VALUES ($1, $2, $3) ON CONFLICT (key) DO UPDATE SET data = $2, size = $3',
            [key, compressed, compressed.length]
        );
        return { key, originalSize: JSON.stringify(data).length, compressedSize: compressed.length };
    }

    async retrieve(key) {
        const { rows } = await this.pool.query(
            'SELECT data FROM compressed_data WHERE key = $1',
            [key]
        );
        if (rows.length === 0) return null;
        return JSON.parse(gunzipSync(rows[0].data).toString());
    }

    async getCompressionStats() {
        const { rows } = await this.pool.query(`
            SELECT 
                COUNT(*) as total_records,
                SUM(size) as total_compressed_bytes,
                AVG(size) as avg_compressed_bytes,
                pg_size_pretty(SUM(size)) as total_compressed_size
            FROM compressed_data
        `);
        return rows[0];
    }
}
```

## Compression Benchmarks

```
Compression Benchmarks (1MB JSON data):
─────────────────────────────────────────────
Algorithm    Compress(ms)  Decompress(ms)  Ratio
─────────────────────────────────────────────
Gzip (1)        12           8             3.2:1
Gzip (6)        25           8             3.8:1
Gzip (9)        45           8             3.9:1
Brotli (1)      18           6             4.1:1
Brotli (4)      35           6             4.5:1
Brotli (11)     180          6             4.8:1
Zstd (1)        8            5             3.5:1
Zstd (3)        15           5             4.0:1

Recommendations:
├── Real-time: Gzip level 1 or Zstd level 1
├── Storage: Brotli level 4
├── Archival: Brotli level 11
└── Database: Use database-native compression
```

## Best Practices Checklist

- [ ] Use database-native compression for large columns
- [ ] Compress data before storing in JSON/JSONB columns
- [ ] Choose compression level based on use case
- [ ] Use streaming compression for large transfers
- [ ] Monitor compression ratios and storage savings
- [ ] Consider Brotli for static content, Gzip for dynamic

## Cross-References

- See [Streaming Data](./01-streaming-data.md) for stream processing
- See [Caching](../04-caching-strategies-implementation/01-in-memory-caching.md) for cache storage
- See [Database Design](../10-database-design-architecture/01-schema-design.md) for schema optimization

## Next Steps

Continue to [Database Security](../07-database-security-implementation/01-connection-security.md) for security.
