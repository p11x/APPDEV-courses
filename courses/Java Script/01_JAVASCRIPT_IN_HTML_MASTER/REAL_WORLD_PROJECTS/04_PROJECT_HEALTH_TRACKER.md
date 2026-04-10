# 💪 Project 19: Health Tracker

## Fitness and Health Data Management

---

## Table of Contents

1. [Health Tracking](#health-tracking)
2. [Activity Monitoring](#activity-monitoring)
3. [Nutrition Logging](#nutrition-logging)
4. [Analytics Dashboard](#analytics-dashboard)

---

## Health Tracking

### Health Data Types

```javascript
const healthDataTypes = {
  weight: { unit: 'kg', min: 20, max: 300 },
  steps: { unit: 'count', min: 0, max: 100000 },
  sleep: { unit: 'hours', min: 0, max: 24 },
  heartRate: { unit: 'bpm', min: 30, max: 220 },
  calories: { unit: 'kcal', min: 0, max: 10000 }
};
```

### Weight Tracking

```javascript
class WeightTracker {
  constructor() {
    this.entries = [];
  }
  
  addEntry(weight, date = new Date()) {
    this.entries.push({ weight, date, id: Date.now() });
  }
  
  getTrend() {
    if (this.entries.length < 2) return 'insufficient data';
    
    const recent = this.entries.slice(-7);
    const first = recent[0].weight;
    const last = recent[recent.length - 1].weight;
    
    if (last < first) return 'decreasing';
    if (last > first) return 'increasing';
    return 'stable';
  }
  
  getAverage(days = 7) {
    const recent = this.entries.slice(-days);
    return recent.reduce((a, b) => a + b.weight, 0) / recent.length;
  }
}
```

---

## Activity Monitoring

### Steps Counter

```javascript
class StepsTracker {
  constructor() {
    this.dailySteps = {};
  }
  
  addSteps(date, steps) {
    const dateKey = this.getDateKey(date);
    this.dailySteps[dateKey] = steps;
  }
  
  getDailySteps(date) {
    return this.dailySteps[this.getDateKey(date)] || 0;
  }
  
  getWeeklyTotal() {
    const week = [];
    for (let i = 0; i < 7; i++) {
      const date = new Date();
      date.setDate(date.getDate() - i);
      week.push(this.getDailySteps(date));
    }
    return week.reduce((a, b) => a + b, 0);
  }
  
  getDateKey(date) {
    return date.toISOString().split('T')[0];
  }
}
```

---

## Nutrition Logging

### Food Database

```javascript
const foodDatabase = {
  apple: { calories: 95, protein: 0.5, carbs: 25, fat: 0.3 },
  banana: { calories: 105, protein: 1.3, carbs: 27, fat: 0.4 },
  chicken_breast: { calories: 165, protein: 31, carbs: 0, fat: 3.6 }
};

class NutritionLogger {
  constructor() {
    this.meals = [];
  }
  
  logMeal(food, quantity) {
    const nutrition = foodDatabase[food.toLowerCase()];
    if (!nutrition) return null;
    
    const entry = {
      ...nutrition,
      food,
      quantity,
      calories: nutrition.calories * quantity,
      id: Date.now()
    };
    
    this.meals.push(entry);
    return entry;
  }
  
  getDailyCalories(date) {
    return this.meals
      .filter(m => this.isSameDay(m.date, date))
      .reduce((sum, m) => sum + m.calories, 0);
  }
}
```

---

## Analytics Dashboard

### Health Charts

```javascript
function renderWeightChart(entries) {
  const ctx = document.getElementById('weight-chart').getContext('2d');
  
  return new Chart(ctx, {
    type: 'line',
    data: {
      labels: entries.map(e => e.date.toLocaleDateString()),
      datasets: [{
        label: 'Weight (kg)',
        data: entries.map(e => e.weight),
        borderColor: '#4CAF50',
        fill: false
      }]
    }
  });
}
```

---

## Summary

### Key Takeaways

1. **Health Data**: Multiple data types
2. **Trends**: Data visualization
3. **Goals**: Target setting

### Next Steps

- Continue with: [05_PROJECT_SOCIAL_NETWORK.md](05_PROJECT_SOCIAL_NETWORK.md)
- Add wearable integration
- Implement reminders

---

## Cross-References

- **Previous**: [03_PROJECT_EDUCATIONAL_PLATFORM.md](03_PROJECT_EDUCATIONAL_PLATFORM.md)
- **Next**: [05_PROJECT_SOCIAL_NETWORK.md](05_PROJECT_SOCIAL_NETWORK.md)

---

*Last updated: 2024*