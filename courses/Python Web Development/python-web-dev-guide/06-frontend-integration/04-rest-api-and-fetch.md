# REST API and Fetch

## What You'll Learn
- Using JavaScript fetch with APIs
- Making requests to Flask/FastAPI
- Handling responses

## Prerequisites
- Completed JavaScript basics

## Using Fetch

```javascript
// GET request
async function fetchData() {
    const response = await fetch('/api/users');
    const data = await response.json();
    console.log(data);
}

// POST request
async function createUser(userData) {
    const response = await fetch('/api/users', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    });
    return await response.json();
}
```

## Example: Loading Data

```html
<button id="loadBtn">Load Users</button>
<div id="usersContainer"></div>

<script>
document.getElementById('loadBtn').addEventListener('click', async () => {
    const response = await fetch('/api/users');
    const users = await response.json();
    
    const container = document.getElementById('usersContainer');
    container.innerHTML = users.map(user => 
        `<li>${user.name} - ${user.email}</li>`
    ).join('');
});
</script>
```

## Summary
- Use `fetch()` for API calls
- Set headers and body for POST requests
- Handle JSON responses with `.json()`
