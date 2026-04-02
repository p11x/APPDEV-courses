# OpenAPI Specification

## What You'll Learn

- Writing OpenAPI 3.0 specs
- Schema definitions
- Request/response documentation

## Example Spec

```yaml
# openapi.yaml
openapi: 3.0.0
info:
  title: Bookmarks API
  version: 1.0.0
paths:
  /api/bookmarks:
    get:
      summary: List bookmarks
      parameters:
        - name: page
          in: query
          schema: { type: integer, default: 1 }
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
      summary: Create bookmark
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/CreateBookmark'
      responses:
        '201':
          description: Created bookmark
components:
  schemas:
    Bookmark:
      type: object
      properties:
        id: { type: integer }
        url: { type: string, format: uri }
        title: { type: string }
```

## Next Steps

For API docs UI, continue to [API Docs UI](./03-api-docs-ui.md).
