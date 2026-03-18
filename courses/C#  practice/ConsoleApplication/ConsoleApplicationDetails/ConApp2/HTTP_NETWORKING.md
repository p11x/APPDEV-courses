# ConApp2 - HTTP and Networking

This document covers HTTP client operations and REST API consumption in ConApp2.

## Table of Contents
1. [HTTP Client Basics](#http-client-basics)
2. [REST API Consumption](#rest-api-consumption)
3. [Async/Await Operations](#asyncawait-operations)

---

## HTTP Client Basics

### Making HTTP Requests

```csharp
using System.Net.Http;

// Create HTTP client
HttpClient client = new HttpClient();

// GET Request
HttpResponseMessage response = await client.GetAsync("https://api.example.com/data");

// Check response status
if (response.IsSuccessStatusCode)
{
    string content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);
}
```

**Source:** [Class56.cs](../../ConsoleApplication/ConApp2/Class56.cs)

### HTTP Methods

| Method | Description |
|--------|-------------|
| GET | Retrieve data |
| POST | Create new resource |
| PUT | Update existing resource |
| DELETE | Remove resource |

### Common Headers

```csharp
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("Authorization", "Bearer token");
```

---

## REST API Consumption

### Processing JSON Response

```csharp
public async Task GetApiData()
{
    HttpClient client = new HttpClient();
    
    try
    {
        HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
        
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            
            // Parse JSON (using Newtonsoft.Json or System.Text.Json)
            var posts = JsonConvert.DeserializeObject<List<Post>>(json);
            
            foreach (var post in posts)
            {
                Console.WriteLine($"Title: {post.title}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

### Post Model

```csharp
public class Post
{
    public int userId { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string body { get; set; }
}
```

---

## Async/Await Operations

### Async Methods

```csharp
public async Task<string> FetchDataAsync(string url)
{
    using (HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}

// Call async method
string result = await FetchDataAsync("https://api.example.com");
```

### Benefits of Async

1. **Non-blocking** - Doesn't freeze UI
2. **Scalability** - Better resource utilization
3. **Performance** - Handles more concurrent requests

---

## Best Practices

### 1. Use IDisposable

```csharp
// Good practice - dispose after use
using (HttpClient client = new HttpClient())
{
    // Make requests
}
```

### 2. Reuse HttpClient

```csharp
// Preferred: Single instance
static HttpClient client = new HttpClient();

// Avoid: Creating new instance per request
// This can cause socket exhaustion
```

### 3. Handle Errors

```csharp
try
{
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Request failed: {ex.Message}");
}
```

---

## Related Classes

| Class | Topic |
|-------|-------|
| Class56 | HTTP Client |
| Class55 | JSON Processing |

---

## Common REST APIs for Practice

| API | Endpoint | Description |
|-----|----------|-------------|
| JSONPlaceholder | https://jsonplaceholder.typicode.com | Fake REST API |
| OpenWeather | https://api.openweathermap.org | Weather data |
| GitHub | https://api.github.com | GitHub data |

---

## Summary

- Use `HttpClient` for HTTP operations
- Always dispose of HttpClient or reuse instance
- Use async/await for non-blocking operations
- Handle exceptions appropriately
- Parse JSON responses with Newtonsoft.Json or System.Text.Json

---

*Last Updated: 2026-03-11*