# Documentation and Code Quality for NodeMark

## What You'll Build In This File

Comprehensive API documentation with OpenAPI, code quality enforcement, and developer documentation standards.

## OpenAPI Specification

```yaml
# docs/openapi.yaml — Complete API specification
openapi: 3.0.3
info:
  title: NodeMark API
  description: Bookmark management API
  version: 1.0.0
  contact:
    name: API Support
    email: support@nodemark.example

servers:
  - url: http://localhost:3000
    description: Development
  - url: https://api.nodemark.example
    description: Production

security:
  - bearerAuth: []

paths:
  /auth/register:
    post:
      tags: [Auth]
      summary: Register new user
      security: []
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              required: [name, email, password]
              properties:
                name: { type: string, minLength: 1, maxLength: 255 }
                email: { type: string, format: email }
                password: { type: string, minLength: 8 }
      responses:
        '201':
          description: User created
        '400':
          description: Validation error
        '409':
          description: Email already exists

  /auth/login:
    post:
      tags: [Auth]
      summary: Login
      security: []
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              required: [email, password]
              properties:
                email: { type: string, format: email }
                password: { type: string }
      responses:
        '200':
          description: Login successful
          content:
            application/json:
              schema:
                type: object
                properties:
                  accessToken: { type: string }
                  refreshToken: { type: string }
        '401':
          description: Invalid credentials

  /bookmarks:
    get:
      tags: [Bookmarks]
      summary: List bookmarks
      parameters:
        - name: limit
          in: query
          schema: { type: integer, default: 20, minimum: 1, maximum: 100 }
        - name: offset
          in: query
          schema: { type: integer, default: 0 }
        - name: tag
          in: query
          schema: { type: string }
      responses:
        '200':
          description: List of bookmarks
          content:
            application/json:
              schema:
                type: array
                items:
                  $ref: '#/components/schemas/Bookmark'

    post:
      tags: [Bookmarks]
      summary: Create bookmark
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateBookmark'
      responses:
        '201':
          description: Bookmark created
        '400':
          description: Validation error
        '409':
          description: URL already bookmarked

components:
  securitySchemes:
    bearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT

  schemas:
    Bookmark:
      type: object
      properties:
        id: { type: integer }
        title: { type: string }
        url: { type: string, format: uri }
        description: { type: string }
        tags: { type: array, items: { type: string } }
        createdAt: { type: string, format: date-time }

    CreateBookmark:
      type: object
      required: [title, url]
      properties:
        title: { type: string, minLength: 1, maxLength: 500 }
        url: { type: string, format: uri }
        description: { type: string, maxLength: 2000 }
        tags: { type: array, items: { type: string }, maxItems: 10 }
```

## Swagger UI Setup

```javascript
// src/docs/swagger.js — Swagger UI integration
import swaggerUi from 'swagger-ui-express';
import YAML from 'yaml';
import fs from 'node:fs';

const openApiDoc = YAML.parse(
    fs.readFileSync('./docs/openapi.yaml', 'utf-8')
);

export function setupSwagger(app) {
    app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(openApiDoc, {
        customCss: '.swagger-ui .topbar { display: none }',
        customSiteTitle: 'NodeMark API Documentation',
    }));

    // Serve raw spec
    app.get('/api-docs/openapi.yaml', (req, res) => {
        res.type('text/yaml');
        res.send(fs.readFileSync('./docs/openapi.yaml', 'utf-8'));
    });
}
```

## Code Quality with ESLint

```javascript
// eslint.config.js — ESLint flat config
import js from '@eslint/js';
import globals from 'globals';

export default [
    js.configs.recommended,
    {
        languageOptions: {
            ecmaVersion: 2024,
            sourceType: 'module',
            globals: {
                ...globals.node,
                ...globals.es2024,
            },
        },
        rules: {
            'no-unused-vars': ['error', { argsIgnorePattern: '^_' }],
            'no-console': ['warn', { allow: ['warn', 'error'] }],
            'eqeqeq': 'error',
            'no-var': 'error',
            'prefer-const': 'error',
            'no-duplicate-imports': 'error',
            'no-throw-literal': 'error',
            'curly': ['error', 'multi-line'],
        },
    },
    {
        ignores: ['dist/', 'node_modules/', 'coverage/'],
    },
];
```

```json
// package.json scripts
{
    "scripts": {
        "lint": "eslint src/ tests/",
        "lint:fix": "eslint src/ tests/ --fix",
        "format": "prettier --write 'src/**/*.js' 'tests/**/*.js'",
        "format:check": "prettier --check 'src/**/*.js' 'tests/**/*.js'",
        "typecheck": "tsc --noEmit",
        "check": "npm run format:check && npm run lint && npm test",
        "validate": "npm run check && npm run test:e2e"
    }
}
```

## JSDoc Documentation

```javascript
// src/services/bookmarks.js — Documented service
import { query } from '../db/index.js';

/**
 * Bookmark service — handles all bookmark business logic
 * @module services/bookmarks
 */

/**
 * Create a new bookmark for a user
 * @param {number} userId - The user's ID
 * @param {Object} data - Bookmark data
 * @param {string} data.title - Bookmark title
 * @param {string} data.url - Bookmark URL
 * @param {string} [data.description] - Optional description
 * @param {string[]} [data.tags] - Optional tag names
 * @returns {Promise<Object>} Created bookmark with tags
 * @throws {Error} If URL already bookmarked by user
 */
export async function createBookmark(userId, data) {
    const { title, url, description, tags } = data;

    const result = await query(
        `INSERT INTO bookmarks (user_id, title, url, description)
         VALUES ($1, $2, $3, $4)
         RETURNING id, title, url, description, created_at`,
        [userId, title, url, description || null]
    );

    const bookmark = result.rows[0];

    if (tags?.length) {
        bookmark.tags = await attachTags(userId, bookmark.id, tags);
    }

    return bookmark;
}

/**
 * List bookmarks for a user with optional tag filter
 * @param {number} userId - The user's ID
 * @param {Object} [options] - Query options
 * @param {number} [options.limit=20] - Max results
 * @param {number} [options.offset=0] - Offset for pagination
 * @param {string} [options.tag] - Filter by tag name
 * @returns {Promise<Object[]>} Array of bookmarks
 */
export async function listBookmarks(userId, options = {}) {
    // ... implementation
}
```

## How It Connects

- API docs follow REST best practices
- Code quality follows [28-code-quality](../../../28-code-quality/) patterns
- Documentation follows Node.js documentation standards

## Common Mistakes

- Outdated API documentation (always update OpenAPI with code changes)
- Not enforcing linting in CI/CD
- Missing JSDoc on public functions
- No type checking configured

## Try It Yourself

### Exercise 1: Add OpenAPI Spec
Document the DELETE /bookmarks/:id endpoint.

### Exercise 2: Fix Lint Errors
Run `npm run lint` and fix all issues.

### Exercise 3: Add JSDoc
Add JSDoc to all functions in `src/routes/`.

## Next Steps

Continue to [22-project-management/01-planning-milestones.md](../22-project-management/01-planning-milestones.md).
