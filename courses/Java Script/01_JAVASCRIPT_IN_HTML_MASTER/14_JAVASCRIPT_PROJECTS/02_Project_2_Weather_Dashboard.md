# 🌤️ Project 2: Weather Dashboard

## 📋 Project Overview

Build a real-time weather dashboard that fetches data from a weather API, displays current conditions, forecasts, and provides location-based weather information. This project showcases:
- Fetch API mastery
- Async/await patterns
- Real-time data visualization
- Error handling and loading states
- Responsive weather interface

---

## 🏗️ Architecture Overview

```
weather-dashboard/
├── index.html
├── css/
│   └── styles.css
└── js/
    ├── app.js
    ├── api.js
    └── utils.js
```

---

## 🎯 Core Features

### Weather API Integration

```javascript
class WeatherAPI {
    constructor() {
        this.baseURL = 'https://api.openweathermap.org/data/2.5';
        this.apiKey = 'YOUR_API_KEY'; // Get free key at openweathermap.org
    }
    
    async getCurrentWeather(city) {
        const url = `${this.baseURL}/weather?q=${city}&appid=${this.apiKey}&units=metric`;
        
        try {
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`Weather data not found: ${response.status}`);
            }
            
            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }
    
    async getForecast(city, days = 5) {
        const url = `${this.baseURL}/forecast?q=${city}&appid=${this.apiKey}&units=metric`;
        
        try {
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`Forecast not found: ${response.status}`);
            }
            
            const data = await response.json();
            return this.processForecastData(data.list);
        } catch (error) {
            console.error('Forecast Error:', error);
            throw error;
        }
    }
    
    processForecastData(list) {
        // Group by day and get daily summary
        const days = {};
        
        list.forEach(item => {
            const date = new Date(item.dt * 1000).toLocaleDateString();
            
            if (!days[date]) {
                days[date] = {
                    date,
                    temps: [],
                    conditions: [],
                    humidity: []
                };
            }
            
            days[date].temps.push(item.main.temp);
            days[date].conditions.push(item.weather[0].main);
            days[date].humidity.push(item.main.humidity);
        });
        
        return Object.values(days).map(day => ({
            date: day.date,
            temp: Math.round(day.temps.reduce((a, b) => a + b, 0) / day.temps.length),
            condition: this.getMostFrequent(day.conditions),
            humidity: Math.round(day.humidity.reduce((a, b) => a + b, 0) / day.humidity.length)
        }));
    }
    
    getMostFrequent(arr) {
        return arr.sort((a, b) =>
            arr.filter(v => v === a).length - arr.filter(v => v === b).length
        ).pop();
    }
}
```

### Weather Display Component

```javascript
class WeatherDisplay {
    constructor(container) {
        this.container = container;
    }
    
    renderCurrentWeather(data) {
        const icon = this.getWeatherIcon(data.weather[0].main);
        
        return `
            <div class="current-weather">
                <div class="weather-main">
                    <div class="weather-icon">${icon}</div>
                    <div class="temperature">${Math.round(data.main.temp)}°C</div>
                </div>
                <div class="weather-details">
                    <h2>${data.name}, ${data.sys.country}</h2>
                    <p class="condition">${data.weather[0].description}</p>
                    <div class="details-grid">
                        <div class="detail">
                            <span class="label">Humidity</span>
                            <span class="value">${data.main.humidity}%</span>
                        </div>
                        <div class="detail">
                            <span class="label">Wind</span>
                            <span class="value">${data.wind.speed} m/s</span>
                        </div>
                        <div class="detail">
                            <span class="label">Feels Like</span>
                            <span class="value">${Math.round(data.main.feels_like)}°C</span>
                        </div>
                        <div class="detail">
                            <span class="label">Pressure</span>
                            <span class="value">${data.main.pressure} hPa</span>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }
    
    renderForecast(forecast) {
        return `
            <div class="forecast">
                <h3>5-Day Forecast</h3>
                <div class="forecast-grid">
                    ${forecast.slice(0, 5).map(day => this.renderForecastDay(day)).join('')}
                </div>
            </div>
        `;
    }
    
    renderForecastDay(day) {
        const icon = this.getWeatherIcon(day.condition);
        const dayName = new Date(day.date).toLocaleDateString('en-US', { weekday: 'short' });
        
        return `
            <div class="forecast-day">
                <div class="day-name">${dayName}</div>
                <div class="day-icon">${icon}</div>
                <div class="day-temp">${day.temp}°</div>
                <div class="day-humidity">💧${day.humidity}%</div>
            </div>
        `;
    }
    
    getWeatherIcon(condition) {
        const icons = {
            'Clear': '☀️',
            'Clouds': '☁️',
            'Rain': '🌧️',
            'Drizzle': '🌦️',
            'Thunderstorm': '⛈️',
            'Snow': '❄️',
            'Mist': '🌫️',
            'Fog': '🌫️'
        };
        return icons[condition] || '🌤️';
    }
}
```

---

## 🎨 HTML Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Weather Dashboard</title>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="weather-app">
        <header class="weather-header">
            <h1>🌤️ Weather Dashboard</h1>
            <form class="search-form" id="searchForm">
                <input type="text" id="cityInput" placeholder="Enter city name" required>
                <button type="submit">🔍</button>
            </form>
        </header>
        
        <div class="loading" id="loading">
            <div class="spinner"></div>
            <p>Loading weather data...</p>
        </div>
        
        <div class="weather-content" id="weatherContent">
            <div class="current-weather" id="currentWeather"></div>
            <div class="forecast" id="forecast"></div>
        </div>
        
        <div class="error" id="errorMessage"></div>
        
        <div class="recent-searches" id="recentSearches">
            <h3>Recent Searches</h3>
            <div class="search-tags" id="searchTags"></div>
        </div>
    </div>
    
    <script src="js/utils.js"></script>
    <script src="js/api.js"></script>
    <script src="js/app.js"></script>
</body>
</html>
```

---

## 🎨 CSS Styling

```css
:root {
    --primary: #3498db;
    --secondary: #2c3e50;
    --accent: #e74c3c;
    --background: #ecf0f1;
    --card-bg: white;
    --text-primary: #2c3e50;
    --text-secondary: #7f8c8d;
}

* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    min-height: 100vh;
    padding: 2rem;
}

.weather-app {
    max-width: 800px;
    margin: 0 auto;
}

.weather-header {
    text-align: center;
    color: white;
    margin-bottom: 2rem;
}

.weather-header h1 {
    margin-bottom: 1rem;
    text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
}

.search-form {
    display: flex;
    justify-content: center;
    gap: 0.5rem;
}

.search-form input {
    padding: 0.75rem 1.5rem;
    border: none;
    border-radius: 50px;
    width: 300px;
    font-size: 1rem;
    box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}

.search-form button {
    padding: 0.75rem 1.5rem;
    border: none;
    border-radius: 50%;
    background: white;
    cursor: pointer;
    font-size: 1.2rem;
    transition: transform 0.2s;
}

.search-form button:hover {
    transform: scale(1.1);
}

.loading {
    text-align: center;
    padding: 3rem;
    display: none;
}

.loading.show {
    display: block;
}

.spinner {
    width: 50px;
    height: 50px;
    border: 5px solid rgba(255,255,255,0.3);
    border-top-color: white;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin: 0 auto 1rem;
}

@keyframes spin {
    to { transform: rotate(360deg); }
}

.weather-content {
    display: none;
}

.weather-content.show {
    display: block;
}

.current-weather {
    background: var(--card-bg);
    border-radius: 20px;
    padding: 2rem;
    box-shadow: 0 10px 30px rgba(0,0,0,0.2);
    margin-bottom: 1.5rem;
}

.weather-main {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 2rem;
    margin-bottom: 2rem;
}

.weather-icon {
    font-size: 5rem;
}

.temperature {
    font-size: 4rem;
    font-weight: bold;
    color: var(--primary);
}

.weather-details h2 {
    text-align: center;
    margin-bottom: 0.5rem;
}

.condition {
    text-align: center;
    text-transform: capitalize;
    color: var(--text-secondary);
    margin-bottom: 1.5rem;
}

.details-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 1rem;
}

.detail {
    background: var(--background);
    padding: 1rem;
    border-radius: 10px;
    text-align: center;
}

.detail .label {
    display: block;
    color: var(--text-secondary);
    font-size: 0.85rem;
    margin-bottom: 0.25rem;
}

.detail .value {
    font-weight: bold;
    color: var(--text-primary);
}

.forecast {
    background: var(--card-bg);
    border-radius: 20px;
    padding: 2rem;
    box-shadow: 0 10px 30px rgba(0,0,0,0.2);
}

.forecast h3 {
    margin-bottom: 1rem;
    color: var(--secondary);
}

.forecast-grid {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 1rem;
}

.forecast-day {
    background: var(--background);
    padding: 1rem;
    border-radius: 10px;
    text-align: center;
}

.day-name {
    font-weight: bold;
    margin-bottom: 0.5rem;
}

.day-icon {
    font-size: 2rem;
    margin-bottom: 0.5rem;
}

.day-temp {
    font-weight: bold;
    color: var(--primary);
}

.day-humidity {
    font-size: 0.85rem;
    color: var(--text-secondary);
    margin-top: 0.5rem;
}

.error {
    background: var(--accent);
    color: white;
    padding: 1rem;
    border-radius: 10px;
    text-align: center;
    display: none;
}

.error.show {
    display: block;
}

@media (max-width: 600px) {
    .search-form input {
        width: 200px;
    }
    
    .forecast-grid {
        grid-template-columns: repeat(2, 1fr);
    }
}
```

---

## 📊 Features Summary

| Feature | Implementation |
|---------|----------------|
| City Search | Form with input validation |
| Current Weather | Temperature, humidity, wind, pressure |
| 5-Day Forecast | Processed from API data |
| Error Handling | User-friendly error messages |
| Loading States | Spinner animation |
| Responsive | Mobile-friendly design |
| Recent Searches | Local storage for history |

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](../08_ASYNC_JAVASCRIPT/02_Promises_Complete_Guide.md)
- [03_Async_Await_Master_Class.md](../08_ASYNC_JAVASCRIPT/03_Async_Await_Master_Class.md)
- [06_Event_Handling_Deep_Dive.md](../09_DOM_MANIPULATION/06_Event_Handling_Deep_Dive.md)

---

**Next: Learn about [E-Commerce Cart Project](./03_Project_3_E_Commerce_Cart.md)**