# 🌤️ Project 22: Weather Station

## Weather Data and Forecasting

---

## Table of Contents

1. [Current Weather](#current-weather)
2. [Forecasting](#forecasting)
3. [Weather Maps](#weather-maps)
4. [Alerts System](#alerts-system)

---

## Current Weather

### Weather Data

```javascript
const weatherSchema = {
  location: 'New York, NY',
  temperature: 72,
  feelsLike: 74,
  humidity: 65,
  windSpeed: 10,
  windDirection: 'NW',
  conditions: 'partly cloudy',
  icon: 'partly-cloudy',
  uv: 5,
  pressure: 1015,
  visibility: 10,
  updatedAt: new Date()
};
```

### Weather Display

```javascript
class WeatherDisplay {
  constructor(data) {
    this.data = data;
  }
  
  render() {
    return `
      <div class="weather-card">
        <h2>${this.data.location}</h2>
        <div class="temperature">${this.data.temperature}°F</div>
        <div class="conditions">${this.data.conditions}</div>
        <div class="details">
          <span>Feels like: ${this.data.feelsLike}°F</span>
          <span>Humidity: ${this.data.humidity}%</span>
          <span>Wind: ${this.data.windSpeed} mph ${this.data.windDirection}</span>
        </div>
      </div>
    `;
  }
}
```

---

## Forecasting

### Forecast Data

```javascript
const forecastSchema = [
  { day: 'Monday', high: 75, low: 60, conditions: 'sunny' },
  { day: 'Tuesday', high: 72, low: 58, conditions: 'cloudy' },
  { day: 'Wednesday', high: 68, low: 55, conditions: 'rain' }
];

class ForecastDisplay {
  constructor(forecast) {
    this.forecast = forecast;
  }
  
  render() {
    const days = this.forecast.map(day => `
      <div class="forecast-day">
        <span class="day">${day.day}</span>
        <span class="conditions">${day.conditions}</span>
        <span class="high">${day.high}°</span>
        <span class="low">${day.low}°</span>
      </div>
    `).join('');
    
    return `<div class="forecast">${days}</div>`;
  }
}
```

---

## Weather Maps

### Map Integration

```javascript
function initWeatherMap(location) {
  const map = L.map('weather-map').setView(location, 10);
  
  L.tileLayer('https://{s}.tile.openweathermap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenWeatherMap'
  }).addTo(map);
  
  L.tileLayer('https://tile.openweathermap.org/map/temp/{z}/{x}/{y}.png', {
    layers: 'temp'
  }).addTo(map);
  
  return map;
}
```

---

## Alerts System

### Weather Alerts

```javascript
class WeatherAlerts {
  constructor() {
    this.alerts = [];
  }
  
  async fetchAlerts(location) {
    const alerts = await API.getAlerts(location);
    this.alerts = alerts.filter(a => a.severity !== 'minor');
    this.render();
  }
  
  getActiveAlerts() {
    return this.alerts.filter(a => new Date(a.expires) > new Date());
  }
  
  render() {
    const alerts = this.getActiveAlerts();
    alerts.forEach(alert => {
      this.showNotification(alert);
    });
  }
}
```

---

## Summary

### Key Takeaways

1. **Current Weather**: Real-time data
2. **Forecast**: Multiple days
3. **Alerts**: Notifications

### Next Steps

- Continue with: [08_PROJECT_MARKETPLACE.md](08_PROJECT_MARKETPLACE.md)
- Add weather widgets
- Implement history

---

## Cross-References

- **Previous**: [06_PROJECT_JOB_BOARD.md](06_PROJECT_JOB_BOARD.md)
- **Next**: [08_PROJECT_MARKETPLACE.md](08_PROJECT_MARKETPLACE.md)

---

*Last updated: 2024*