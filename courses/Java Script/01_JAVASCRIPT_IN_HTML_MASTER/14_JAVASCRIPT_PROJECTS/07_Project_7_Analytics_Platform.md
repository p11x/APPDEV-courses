# 📊 Project 7: Analytics Platform

## 📋 Project Overview

Build a data analytics platform with interactive charts, data filtering, export capabilities, and real-time metrics display. This project demonstrates:
- Multiple chart types with Chart.js
- Data filtering and aggregation
- Export to CSV functionality
- Real-time data updates

---

## 🎯 Core Features

### Analytics Engine

```javascript
class AnalyticsEngine {
    constructor() {
        this.data = [];
        this.filters = {};
    }
    
    loadSampleData() {
        this.data = [
            { date: '2024-01-01', visitors: 1200, revenue: 4500, conversions: 45 },
            { date: '2024-01-02', visitors: 1350, revenue: 5200, conversions: 52 },
            { date: '2024-01-03', visitors: 1100, revenue: 3800, conversions: 38 },
            { date: '2024-01-04', visitors: 1450, revenue: 5800, conversions: 58 },
            { date: '2024-01-05', visitors: 1600, revenue: 6500, conversions: 65 },
            { date: '2024-01-06', visitors: 1700, revenue: 7200, conversions: 72 },
            { date: '2024-01-07', visitors: 1550, revenue: 6100, conversions: 61 },
        ];
    }
    
    getMetrics() {
        const totals = this.data.reduce((acc, day) => ({
            visitors: acc.visitors + day.visitors,
            revenue: acc.revenue + day.revenue,
            conversions: acc.conversions + day.conversions
        }), { visitors: 0, revenue: 0, conversions: 0 });
        
        return {
            totalVisitors: totals.visitors,
            totalRevenue: totals.revenue,
            totalConversions: totals.conversions,
            avgDailyVisitors: Math.round(totals.visitors / this.data.length),
            conversionRate: ((totals.conversions / totals.visitors) * 100).toFixed(2)
        };
    }
    
    filterData(startDate, endDate) {
        return this.data.filter(d => 
            d.date >= startDate && d.date <= endDate
        );
    }
    
    exportToCSV() {
        const headers = Object.keys(this.data[0]).join(',');
        const rows = this.data.map(row => Object.values(row).join(','));
        return [headers, ...rows].join('\n');
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
    <title>Analytics Platform</title>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="analytics-app">
        <header>
            <h1>📈 Analytics Platform</h1>
            <button id="exportBtn">Export CSV</button>
        </header>
        
        <div class="metrics-grid">
            <div class="metric-card">
                <h3>Total Visitors</h3>
                <div id="visitorsMetric">0</div>
            </div>
            <div class="metric-card">
                <h3>Total Revenue</h3>
                <div id="revenueMetric">$0</div>
            </div>
            <div class="metric-card">
                <h3>Conversions</h3>
                <div id="conversionsMetric">0</div>
            </div>
            <div class="metric-card">
                <h3>Conversion Rate</h3>
                <div id="rateMetric">0%</div>
            </div>
        </div>
        
        <div class="charts-grid">
            <div class="chart-box">
                <h3>Visitors Trend</h3>
                <canvas id="visitorsChart"></canvas>
            </div>
            <div class="chart-box">
                <h3>Revenue Trend</h3>
                <canvas id="revenueChart"></canvas>
            </div>
        </div>
    </div>
</body>
</html>
```

---

## 🔗 Related Topics

- [06_Project_6_Financial_Dashboard.md](./06_Project_6_Financial_Dashboard.md)
- [03_Async_Await_Master_Class.md](../08_ASYNC_JAVASCRIPT/03_Async_Await_Master_Class.md)

---

**Next: [Task Management System](./08_Project_8_Task_Management_System.md)**