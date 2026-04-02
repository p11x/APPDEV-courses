# Search Engines (Elasticsearch, Typesense) with Node.js

## What You'll Learn

- Elasticsearch integration and querying
- Typesense for fast search
- Index design and mapping
- Full-text search patterns
- Search relevance tuning

## Elasticsearch Integration

```bash
npm install @elastic/elasticsearch
```

```javascript
import { Client } from '@elastic/elasticsearch';

const es = new Client({
    node: process.env.ELASTICSEARCH_URL || 'http://localhost:9200',
    auth: {
        username: process.env.ES_USER || 'elastic',
        password: process.env.ES_PASSWORD,
    },
});

// Create index with mapping
async function createProductIndex() {
    const exists = await es.indices.exists({ index: 'products' });
    if (exists) return;

    await es.indices.create({
        index: 'products',
        body: {
            settings: {
                number_of_shards: 2,
                number_of_replicas: 1,
                analysis: {
                    analyzer: {
                        product_analyzer: {
                            type: 'custom',
                            tokenizer: 'standard',
                            filter: ['lowercase', 'asciifolding', 'synonym_filter'],
                        },
                    },
                    filter: {
                        synonym_filter: {
                            type: 'synonym',
                            synonyms: ['phone,smartphone,mobile', 'laptop,notebook'],
                        },
                    },
                },
            },
            mappings: {
                properties: {
                    name: { type: 'text', analyzer: 'product_analyzer', boost: 2 },
                    description: { type: 'text', analyzer: 'product_analyzer' },
                    category: { type: 'keyword' },
                    price: { type: 'float' },
                    brand: { type: 'keyword' },
                    tags: { type: 'keyword' },
                    createdAt: { type: 'date' },
                    inStock: { type: 'boolean' },
                },
            },
        },
    });
}

// Index a document
async function indexProduct(id, product) {
    await es.index({
        index: 'products',
        id,
        document: product,
        refresh: true,
    });
}

// Bulk index
async function bulkIndexProducts(products) {
    const operations = products.flatMap(doc => [
        { index: { _index: 'products', _id: doc.id } },
        doc,
    ]);

    const result = await es.bulk({ operations, refresh: true });
    return {
        items: result.items.length,
        errors: result.errors,
    };
}

// Search with filters
async function searchProducts(query, filters = {}) {
    const must = query ? [{
        multi_match: {
            query,
            fields: ['name^3', 'description', 'brand^2', 'tags'],
            type: 'best_fields',
            fuzziness: 'AUTO',
        },
    }] : [{ match_all: {} }];

    const filter = [];
    if (filters.category) filter.push({ term: { category: filters.category } });
    if (filters.brand) filter.push({ term: { brand: filters.brand } });
    if (filters.minPrice || filters.maxPrice) {
        filter.push({
            range: {
                price: {
                    ...(filters.minPrice && { gte: filters.minPrice }),
                    ...(filters.maxPrice && { lte: filters.maxPrice }),
                },
            },
        });
    }
    if (filters.inStock !== undefined) {
        filter.push({ term: { inStock: filters.inStock } });
    }

    const result = await es.search({
        index: 'products',
        body: {
            query: {
                bool: { must, filter },
            },
            sort: filters.sort || [{ _score: 'desc' }, { createdAt: 'desc' }],
            from: filters.offset || 0,
            size: filters.limit || 20,
            aggs: {
                categories: { terms: { field: 'category', size: 20 } },
                brands: { terms: { field: 'brand', size: 20 } },
                price_ranges: {
                    range: {
                        field: 'price',
                        ranges: [
                            { to: 25 },
                            { from: 25, to: 50 },
                            { from: 50, to: 100 },
                            { from: 100 },
                        ],
                    },
                },
            },
        },
    });

    return {
        total: result.hits.total.value,
        hits: result.hits.hits.map(h => ({ id: h._id, score: h._score, ...h._source })),
        aggregations: result.aggregations,
    };
}
```

## Typesense Integration

```bash
npm install typesense
```

```javascript
import Typesense from 'typesense';

const client = new Typesense.Client({
    nodes: [{
        host: process.env.TYPESENSE_HOST || 'localhost',
        port: parseInt(process.env.TYPESENSE_PORT || '8108'),
        protocol: 'http',
    }],
    apiKey: process.env.TYPESENSE_API_KEY,
});

// Create collection
async function createCollection() {
    await client.collections().create({
        name: 'products',
        fields: [
            { name: 'name', type: 'string', sort: true },
            { name: 'description', type: 'string' },
            { name: 'category', type: 'string', facet: true },
            { name: 'brand', type: 'string', facet: true },
            { name: 'price', type: 'float', facet: true, sort: true },
            { name: 'tags', type: 'string[]', facet: true },
            { name: 'inStock', type: 'bool', facet: true },
            { name: 'createdAt', type: 'int64', sort: true },
        ],
        default_sorting_field: 'createdAt',
    });
}

// Search
async function search(query, options = {}) {
    const result = await client.collections('products').documents().search({
        q: query,
        query_by: 'name,description,brand,tags',
        filter_by: buildFilter(options.filters),
        sort_by: options.sort || '_text_match:desc',
        page: options.page || 1,
        per_page: options.limit || 20,
        facet_by: 'category,brand,price',
        highlight_start: '<mark>',
        highlight_end: '</mark>',
    });

    return {
        total: result.found,
        hits: result.hits.map(h => ({
            id: h.document.id,
            highlights: h.highlights,
            ...h.document,
        })),
        facets: result.facet_counts,
    };
}

function buildFilter(filters = {}) {
    const parts = [];
    if (filters.category) parts.push(`category:=${filters.category}`);
    if (filters.brand) parts.push(`brand:=${filters.brand}`);
    if (filters.minPrice) parts.push(`price:>=${filters.minPrice}`);
    if (filters.maxPrice) parts.push(`price:<=${filters.maxPrice}`);
    if (filters.inStock !== undefined) parts.push(`inStock:=${filters.inStock}`);
    return parts.join(' && ') || undefined;
}
```

## Database + Search Sync Pattern

```javascript
class SearchSync {
    constructor(pool, searchClient) {
        this.pool = pool;
        this.search = searchClient;
    }

    async syncTable(tableName, searchIndex) {
        let lastId = 0;
        let totalSynced = 0;

        while (true) {
            const { rows } = await this.pool.query(
                `SELECT * FROM ${tableName} WHERE id > $1 ORDER BY id LIMIT 1000`,
                [lastId]
            );

            if (rows.length === 0) break;

            await this.search.bulkIndex(rows.map(row => ({
                ...row,
                id: String(row.id),
            })));

            lastId = rows[rows.length - 1].id;
            totalSynced += rows.length;
        }

        return totalSynced;
    }

    // CDC-based sync
    async setupTriggerSync(tableName) {
        await this.pool.query(`
            CREATE OR REPLACE FUNCTION sync_to_search()
            RETURNS TRIGGER AS $$
            BEGIN
                PERFORM pg_notify('search_sync', json_build_object(
                    'table', TG_TABLE_NAME,
                    'operation', TG_OP,
                    'id', COALESCE(NEW.id, OLD.id)
                )::text);
                RETURN COALESCE(NEW, OLD);
            END;
            $$ LANGUAGE plpgsql;

            DROP TRIGGER IF EXISTS search_sync_trigger ON ${tableName};
            CREATE TRIGGER search_sync_trigger
            AFTER INSERT OR UPDATE OR DELETE ON ${tableName}
            FOR EACH ROW EXECUTE FUNCTION sync_to_search();
        `);
    }
}
```

## Best Practices Checklist

- [ ] Design index mappings based on query patterns
- [ ] Use analyzers appropriate for your language
- [ ] Keep search index in sync with database
- [ ] Use bulk operations for indexing
- [ ] Tune relevance with boost and scoring
- [ ] Implement faceted search for filtering
- [ ] Monitor search performance and index size

## Cross-References

- See [NoSQL Patterns](./01-nosql-patterns.md) for document databases
- See [Caching](../04-caching-strategies-implementation/02-redis-caching.md) for search caching
- See [Database Design](../10-database-design-architecture/01-schema-design.md) for modeling

## Next Steps

Continue to [Database Design](../10-database-design-architecture/01-schema-design.md) for schema design.
