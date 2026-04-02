# API Docs UI

## What You'll Learn

- Serving interactive API documentation
- Swagger UI setup
- Redoc as an alternative

## Swagger UI

```js
import swaggerUi from 'swagger-ui-express';
import { readFileSync } from 'node:fs';
import { parse } from 'yaml';

const spec = parse(readFileSync('./openapi.yaml', 'utf-8'));

app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(spec, {
  customCss: '.swagger-ui .topbar { display: none }',
  customSiteTitle: 'Bookmarks API Docs',
}));
```

## Next Steps

For performance optimization in the capstone, continue to [Caching Implementation](../08-performance/01-caching-implementation.md).
