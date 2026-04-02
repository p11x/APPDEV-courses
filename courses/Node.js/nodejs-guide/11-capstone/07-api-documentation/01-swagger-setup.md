# API Documentation with Swagger

## What You'll Learn

- Setting up Swagger/OpenAPI for the capstone
- Documenting endpoints
- Interactive API docs UI

## Setup

```bash
npm install swagger-jsdoc swagger-ui-express
```

```js
// swagger.js
import swaggerJsdoc from 'swagger-jsdoc';

const options = {
  definition: {
    openapi: '3.0.0',
    info: {
      title: 'Bookmarks API',
      version: '1.0.0',
      description: 'A bookmark management API',
    },
    servers: [{ url: 'http://localhost:3000' }],
  },
  apis: ['./routes/*.js'],
};

export const swaggerSpec = swaggerJsdoc(options);
```

```js
// app.js
import swaggerUi from 'swagger-ui-express';
import { swaggerSpec } from './swagger.js';

app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerSpec));
```

## Next Steps

For caching implementation, continue to [Caching Implementation](../08-performance/01-caching-implementation.md).
