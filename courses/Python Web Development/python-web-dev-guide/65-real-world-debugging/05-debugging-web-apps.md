# Debugging Web Apps

## What You'll Learn

- Debugging Flask apps
- Debugging FastAPI apps
- Common web issues

## Prerequisites

- Completed `04-common-errors.md`

## Flask Debug Mode

```python
# Enable debug mode
app.run(debug=True)

# Shows detailed error pages
```

## FastAPI Debug Mode

```python
import uvicorn

uvicorn.run(app, debug=True)
```

## Summary

- Use debug mode for development
- Check server logs

## Next Steps

Continue to `06-debugging-database.md`.
