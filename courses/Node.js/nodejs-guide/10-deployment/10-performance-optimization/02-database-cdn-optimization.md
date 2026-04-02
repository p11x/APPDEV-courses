# Database and CDN Performance Optimization

## What You'll Learn

- Database connection pooling with pg-pool and mysql2
- Query optimization and indexing strategies
- Read replica configuration and intelligent query routing
- CDN setup with CloudFront, Cloudflare, and Fastly
- Cache invalidation strategies and edge caching patterns
- Static asset optimization: fingerprinting, compression, tree-shaking
- Image optimization pipeline with responsive formats
- Bundle optimization with webpack/esbuild

## Database Connection Pooling

Connection pooling eliminates the overhead of creating new database connections for every request.

### PostgreSQL with pg-pool

```javascript
const { Pool } = require('pg');

const pool = new Pool({
  host: process.env.DB_HOST,
  port: process.env.DB_PORT || 5432,
  database: process.env.DB_NAME,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  max: 20,                     // Maximum pool size
  min: 4,                      // Minimum idle connections
  idleTimeoutMillis: 30000,    // Close idle connections after 30s
  connectionTimeoutMillis: 5000,
  statement_timeout: 10000,    // Query timeout
  allowExitOnIdle: true,
});

// Graceful shutdown
async function shutdown() {
  await pool.end();
  console.log('Connection pool closed');
}
process.on('SIGTERM', shutdown);
process.on('SIGINT', shutdown);

// Usage with error handling
async function query(text, params) {
  const start = Date.now();
  const result = await pool.query(text, params);
  const duration = Date.now() - start;
  console.log('Query executed', { text: text.substring(0, 50), duration, rows: result.rowCount });
  return result;
}

module.exports = { pool, query };
```

### MySQL with mysql2

```javascript
const mysql = require('mysql2/promise');

const pool = mysql.createPool({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME,
  waitForConnections: true,
  connectionLimit: 20,
  queueLimit: 0,
  enableKeepAlive: true,
  keepAliveInitialDelay: 10000,
  maxIdle: 10,
  idleTimeout: 60000,
});

// Named prepared statement wrapper
async function execute(sql, params) {
  const [rows] = await pool.execute(sql, params);
  return rows;
}

module.exports = { pool, execute };
```

## Query Optimization and Indexing

### Query Performance Patterns

```javascript
const { pool } = require('./db');

// BAD: N+1 query problem
async function getUsersWithPosts_NAIVE() {
  const users = await pool.query('SELECT * FROM users LIMIT 100');
  for (const user of users.rows) {
    user.posts = await pool.query(
      'SELECT * FROM posts WHERE user_id = $1', [user.id]
    );
  }
  return users.rows;
}

// GOOD: Single JOIN query
async function getUsersWithPosts_OPTIMIZED() {
  const result = await pool.query(`
    SELECT u.*, p.id AS post_id, p.title, p.created_at AS post_created
    FROM users u
    LEFT JOIN posts p ON p.user_id = u.id
    WHERE u.id IN (
      SELECT id FROM users ORDER BY created_at DESC LIMIT 100
    )
    ORDER BY u.created_at DESC, p.created_at DESC
  `);

  // Group results in application code
  const userMap = new Map();
  for (const row of result.rows) {
    if (!userMap.has(row.id)) {
      userMap.set(row.id, { ...row, posts: [] });
    }
    if (row.post_id) {
      userMap.get(row.id).posts.push({
        id: row.post_id,
        title: row.title,
        created_at: row.post_created,
      });
    }
  }
  return [...userMap.values()];
}

// Pagination with cursor-based approach
async function getProducts(cursor, limit = 20) {
  const query = cursor
    ? `SELECT * FROM products WHERE id > $1 ORDER BY id ASC LIMIT $2`
    : `SELECT * FROM products ORDER BY id ASC LIMIT $1`;
  const params = cursor ? [cursor, limit] : [limit];
  const result = await pool.query(query, params);
  return {
    data: result.rows,
    nextCursor: result.rows.length ? result.rows[result.rows.length - 1].id : null,
  };
}
```

### Indexing Strategies

```sql
-- Composite index for common query patterns
CREATE INDEX idx_orders_user_status_date
ON orders (user_id, status, created_at DESC);

-- Partial index for active records only
CREATE INDEX idx_users_active_email
ON users (email) WHERE active = true;

-- GIN index for JSONB queries
CREATE INDEX idx_products_attributes
ON products USING GIN (attributes jsonb_path_ops);

-- Covering index (index-only scan)
CREATE INDEX idx_orders_covering
ON orders (user_id, status) INCLUDE (total, created_at);

-- Analyze query plans
EXPLAIN (ANALYZE, BUFFERS, FORMAT TEXT)
SELECT * FROM orders WHERE user_id = 42 AND status = 'pending';
```

## Read Replica Configuration

```javascript
const { Pool } = require('pg');

// Separate pools for primary and replicas
const primaryPool = new Pool({ host: 'db-primary', max: 20 });
const replicaPools = [
  new Pool({ host: 'db-replica-1', max: 30 }),
  new Pool({ host: 'db-replica-2', max: 30 }),
];

let replicaIndex = 0;

function getReplica() {
  const pool = replicaPools[replicaIndex % replicaPools.length];
  replicaIndex++;
  return pool;
}

// Query router
async function routedQuery(text, params, { write = false } = {}) {
  if (write) {
    return primaryPool.query(text, params);
  }
  try {
    return await getReplica().query(text, params);
  } catch (err) {
    if (err.code === 'ECONNREFUSED') {
      console.warn('Replica unavailable, falling back to primary');
      return primaryPool.query(text, params);
    }
    throw err;
  }
}

// Middleware: auto-detect read vs write
function dbMiddleware(req, res, next) {
  req.db = {
    query: (text, params) => {
      const isWrite = /^\s*(INSERT|UPDATE|DELETE|CREATE|ALTER|DROP)/i.test(text);
      return routedQuery(text, params, { write: isWrite });
    },
  };
  next();
}
```

## Database Caching Layer

```javascript
const Redis = require('ioredis');
const crypto = require('crypto');

const redis = new Redis({
  host: process.env.REDIS_HOST,
  keyPrefix: 'dbcache:',
});

function cacheKey(query, params) {
  return crypto
    .createHash('md5')
    .update(`${query}:${JSON.stringify(params)}`)
    .digest('hex');
}

async function cachedQuery(pool, query, params = [], ttl = 300) {
  const key = cacheKey(query, params);
  const cached = await redis.get(key);
  if (cached) return JSON.parse(cached);

  const result = await pool.query(query, params);
  await redis.setex(key, ttl, JSON.stringify(result.rows));
  return result.rows;
}

// Cache invalidation on write
async function invalidatePattern(pattern) {
  const keys = await redis.keys(`dbcache:${pattern}`);
  if (keys.length) await redis.del(keys);
}
```

## CDN Configuration

### CloudFront Setup

```javascript
const {
  CloudFrontClient,
  CreateInvalidationCommand,
} = require('@aws-sdk/client-cloudfront');

const cfClient = new CloudFrontClient({ region: 'us-east-1' });

async function invalidateCDN(paths) {
  const command = new CreateInvalidationCommand({
    DistributionId: process.env.CLOUDFRONT_DIST_ID,
    InvalidationBatch: {
      CallerReference: `inv-${Date.now()}`,
      Paths: {
        Quantity: paths.length,
        Items: paths,
      },
    },
  });
  return cfClient.send(command);
}

// Batch invalidation on deployment
async function invalidateOnDeploy() {
  await invalidateCDN([
    '/index.html',
    '/css/*',
    '/js/*',
    '/assets/*',
  ]);
}
```

### Cloudflare Workers Edge Caching

```javascript
// cloudflare-worker.js
addEventListener('fetch', (event) => {
  event.respondWith(handleRequest(event.request));
});

async function handleRequest(request) {
  const cache = caches.default;
  const url = new URL(request.url);

  // Check edge cache first
  let response = await cache.match(request);
  if (response) return response;

  // Fetch from origin
  response = await fetch(request);

  // Cache static assets at edge
  if (url.pathname.match(/\.(js|css|png|jpg|webp|woff2)$/)) {
    response = new Response(response.body, response);
    response.headers.set('Cache-Control', 'public, max-age=31536000, immutable');
    event.waitUntil(cache.put(request, response.clone()));
  }

  return response;
}
```

## Static Asset Optimization

### Asset Fingerprinting and Compression

```javascript
const express = require('express');
const compression = require('compression');
const path = require('path');
const fs = require('fs');

const app = express();

// Brotli + gzip compression
app.use(
  compression({
    level: 6,
    threshold: 1024,
    filter: (req, res) => {
      if (req.headers['x-no-compression']) return false;
      return compression.filter(req, res);
    },
  })
);

// Serve pre-compressed assets
app.use(express.static('public', {
  maxAge: '1y',
  immutable: true,
  setHeaders: (res, filePath) => {
    if (filePath.endsWith('.br')) {
      res.setHeader('Content-Encoding', 'br');
      res.setHeader('Content-Type', getMimeType(filePath.replace('.br', '')));
    } else if (filePath.endsWith('.gz')) {
      res.setHeader('Content-Encoding', 'gzip');
      res.setHeader('Content-Type', getMimeType(filePath.replace('.gz', '')));
    }
    // Content hash in filename enables long-term caching
    if (filePath.match(/\.[a-f0-9]{8}\./)) {
      res.setHeader('Cache-Control', 'public, max-age=31536000, immutable');
    }
  },
}));

function getMimeType(filePath) {
  const types = {
    '.js': 'application/javascript',
    '.css': 'text/css',
    '.html': 'text/html',
    '.json': 'application/json',
    '.svg': 'image/svg+xml',
  };
  return types[path.extname(filePath)] || 'application/octet-stream';
}
```

## Image Optimization Pipeline

```javascript
const sharp = require('sharp');
const path = require('path');
const fs = require('fs/promises');

const SIZES = [320, 640, 1024, 1920];
const FORMATS = ['webp', 'avif', 'original'];

async function optimizeImage(inputPath, outputDir) {
  const image = sharp(inputPath);
  const metadata = await image.metadata();
  const basename = path.basename(inputPath, path.extname(inputPath));
  const results = [];

  for (const size of SIZES) {
    if (size > metadata.width) continue;
    const resized = sharp(inputPath).resize(size, null, {
      withoutEnlargement: true,
      fit: 'inside',
    });

    for (const format of FORMATS) {
      let pipeline;
      let ext;
      if (format === 'webp') {
        pipeline = resized.clone().webp({ quality: 80, effort: 4 });
        ext = 'webp';
      } else if (format === 'avif') {
        pipeline = resized.clone().avif({ quality: 65, effort: 4 });
        ext = 'avif';
      } else {
        pipeline = resized.clone().jpeg({ quality: 85, mozjpeg: true });
        ext = 'jpg';
      }

      const filename = `${basename}-${size}w.${ext}`;
      const outputPath = path.join(outputDir, filename);
      await pipeline.toFile(outputPath);
      results.push({ path: outputPath, width: size, format: ext });
    }
  }
  return results;
}

// Generate responsive <picture> markup
function pictureTag(filename, alt) {
  const basename = path.basename(filename, path.extname(filename));
  const sources = SIZES.map((size) =>
    `  <source srcset="/img/${basename}-${size}w.avif" type="image/avif" media="(min-width: ${size}px)">`
  ).join('\n');
  return `<picture>
${sources}
  <source srcset="/img/${basename}-640w.webp" type="image/webp">
  <img src="/img/${basename}-640w.jpg" alt="${alt}" loading="lazy" decoding="async">
</picture>`;
}
```

## Bundle Optimization

### esbuild Configuration

```javascript
const esbuild = require('esbuild');

const sharedConfig = {
  entryPoints: ['src/index.ts'],
  bundle: true,
  splitting: true,
  format: 'esm',
  target: ['es2020'],
  minify: true,
  treeShaking: true,
  sourcemap: true,
  metafile: true,
  outdir: 'dist',
  define: {
    'process.env.NODE_ENV': '"production"',
  },
};

async function build() {
  const result = await esbuild.build(sharedConfig);
  const analysis = await esbuild.analyzeMetafile(result.metafile);
  console.log(analysis);
}

// Dynamic import for code splitting
// In source code:
// const heavyModule = await import('./heavy-feature.js');
```

### Webpack Code Splitting

```javascript
// webpack.config.js
module.exports = {
  mode: 'production',
  entry: './src/index.js',
  output: {
    filename: '[name].[contenthash:8].js',
    chunkFilename: '[name].[contenthash:8].chunk.js',
    clean: true,
  },
  optimization: {
    splitChunks: {
      chunks: 'all',
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          priority: -10,
        },
        common: {
          minChunks: 2,
          priority: -20,
          reuseExistingChunk: true,
        },
      },
    },
  },
};
```

## Content Negotiation Middleware

```javascript
const zlib = require('zlib');

function contentNegotiation(req, res, next) {
  const acceptEncoding = req.headers['accept-encoding'] || '';

  res.compressBody = (body) => {
    const contentType = res.getHeader('Content-Type') || 'application/json';

    if (acceptEncoding.includes('br')) {
      res.setHeader('Content-Encoding', 'br');
      res.setHeader('Content-Type', contentType);
      return zlib.brotliCompress(Buffer.from(body), (err, result) => {
        res.end(result);
      });
    }
    if (acceptEncoding.includes('gzip')) {
      res.setHeader('Content-Encoding', 'gzip');
      res.setHeader('Content-Type', contentType);
      return zlib.gzip(Buffer.from(body), (err, result) => {
        res.end(result);
      });
    }
    res.end(body);
  };

  next();
}
```

## Performance Benchmark Data

| Optimization | Before | After | Improvement |
|---|---|---|---|
| Connection pooling (20 conns) | 450ms p99 | 120ms p99 | 73% |
| Query with proper index | 850ms | 12ms | 98% |
| Redis query cache (hit) | 850ms | 3ms | 99.6% |
| Read replica routing | 100% primary load | 70% offloaded | 70% |
| Brotli compression | 2.4MB transfer | 620KB transfer | 74% |
| WebP images | 1.8MB total | 480KB total | 73% |
| AVIF images | 1.8MB total | 310KB total | 83% |
| Code splitting (initial) | 1.2MB bundle | 280KB bundle | 77% |
| Asset fingerprinting + CDN | 1.8s FCP | 0.4s FCP | 78% |

## Best Practices Checklist

- [ ] Configure connection pool sizing based on DB max_connections / app instances
- [ ] Set idle timeout and connection timeout to prevent resource leaks
- [ ] Use prepared statements or parameterized queries to prevent SQL injection
- [ ] Add composite indexes matching your most common WHERE/JOIN patterns
- [ ] Use EXPLAIN ANALYZE to verify query plans use indexes
- [ ] Route read traffic to replicas automatically with fallback to primary
- [ ] Cache frequent read queries with TTL-based invalidation
- [ ] Serve assets with content-hash filenames and `immutable` cache headers
- [ ] Pre-compress assets with both brotli and gzip at build time
- [ ] Generate responsive images in WebP/AVIF with fallbacks
- [ ] Split bundles for route-based lazy loading
- [ ] Invalidate CDN cache on deployment, not on every change
- [ ] Monitor cache hit ratios and connection pool utilization

## Cross-References

- [Performance Optimization Fundamentals](./01-performance-optimization.md)
- [API and Network Optimization](./03-api-network-optimization.md)
- [Docker Deployment](../docker/)
- [Monitoring](../monitoring/)
- [Architecture Patterns](../01-deployment-architecture/01-architecture-patterns.md)

## Next Steps

Continue to [API and Network Optimization](./03-api-network-optimization.md) to learn about API response optimization, connection tuning, worker threads, and performance profiling.
