# Code Review Checklist

## What You'll Learn

- What to look for in code reviews
- Node.js-specific review items
- Security review checklist

## General Checklist

- [ ] Code follows project style guide (ESLint/Prettier)
- [ ] No `console.log` left in production code (use proper logging)
- [ ] Error handling is present and correct
- [ ] No hardcoded secrets or credentials
- [ ] Tests are included for new functionality
- [ ] Documentation is updated

## Node.js Specific

- [ ] Uses `const`/`let` — never `var`
- [ ] Uses ES Modules (`import`/`export`)
- [ ] No synchronous blocking operations in request handlers
- [ ] Database connections use pooling
- [ ] All async operations have error handling
- [ ] No `eval()` or `Function()` with dynamic input
- [ ] Environment variables are used for configuration
- [ ] Graceful shutdown is handled (SIGTERM/SIGINT)

## Security Checklist

- [ ] Input validation on all user data
- [ ] No SQL injection (parameterized queries)
- [ ] No command injection (no shell interpolation)
- [ ] Security headers are set (Helmet)
- [ ] Rate limiting on auth endpoints
- [ ] JWT secrets are from environment variables
- [ ] Dependencies are audited (`npm audit`)

## Performance Checklist

- [ ] No N+1 queries (use DataLoader or includes)
- [ ] Large responses are compressed
- [ ] Static assets have cache headers
- [ ] No unbounded arrays or caches

## Next Steps

For frontend integration, continue to [Chapter 29: Frontend Integration](../../29-frontend-integration/01-react-setup.md).
