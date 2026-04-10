# 📊 Project 6: Financial Dashboard

## 📋 Project Overview

Build a comprehensive financial dashboard with expense tracking, budget management, visual charts, and spending analytics. This project demonstrates:
- Data visualization with Chart.js
- Financial calculations and summaries
- Local storage for data persistence
- Interactive charts and graphs

---

## 🏗️ Architecture Overview

```
financial-dashboard/
├── index.html
├── css/
│   └── styles.css
└── js/
    ├── app.js
    ├── finance.js
    └── charts.js
```

---

## 🎯 Core Features

### Finance Manager

```javascript
class FinanceManager {
    constructor() {
        this.transactions = [];
        this.categories = [
            'Food', 'Transport', 'Shopping', 'Entertainment', 
            'Bills', 'Health', 'Education', 'Other'
        ];
        this.loadFromStorage();
    }
    
    addTransaction(amount, category, description, type = 'expense') {
        const transaction = {
            id: this.generateId(),
            amount: parseFloat(amount),
            category,
            description,
            type,
            date: new Date().toISOString(),
            month: new Date().toLocaleString('en-US', { month: 'short', year: 'numeric' })
        };
        
        this.transactions.unshift(transaction);
        this.saveToStorage();
        return transaction;
    }
    
    getBalance() {
        const income = this.transactions
            .filter(t => t.type === 'income')
            .reduce((sum, t) => sum + t.amount, 0);
        
        const expenses = this.transactions
            .filter(t => t.type === 'expense')
            .reduce((sum, t) => sum + t.amount, 0);
        
        return { income, expenses, balance: income - expenses };
    }
    
    getCategorySpending() {
        const spending = {};
        
        this.transactions
            .filter(t => t.type === 'expense')
            .forEach(t => {
                spending[t.category] = (spending[t.category] || 0) + t.amount;
            });
        
        return spending;
    }
    
    getMonthlyData() {
        const monthly = {};
        
        this.transactions.forEach(t => {
            if (!monthly[t.month]) {
                monthly[t.month] = { income: 0, expense: 0 };
            }
            if (t.type === 'income') {
                monthly[t.month].income += t.amount;
            } else {
                monthly[t.month].expense += t.amount;
            }
        });
        
        return monthly;
    }
    
    getTopExpenses(limit = 5) {
        return this.transactions
            .filter(t => t.type === 'expense')
            .sort((a, b) => b.amount - a.amount)
            .slice(0, limit);
    }
    
    deleteTransaction(id) {
        this.transactions = this.transactions.filter(t => t.id !== id);
        this.saveToStorage();
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('financeData', JSON.stringify(this.transactions));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('financeData');
        if (stored) {
            try {
                this.transactions = JSON.parse(stored);
            } catch (e) {
                this.transactions = [];
            }
        }
    }
}
```

### Chart Manager

```javascript
class ChartManager {
    constructor(canvasId) {
        this.canvas = document.getElementById(canvasId);
        this.chart = null;
    }
    
    createPieChart(data, labels) {
        if (this.chart) this.chart.destroy();
        
        this.chart = new Chart(this.canvas, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: [
                        '#FF6384', '#36A2EB', '#FFCE56', 
                        '#4BC0C0', '#9966FF', '#FF9F40',
                        '#C9CBCF', '#4BC0C0'
                    ]
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { position: 'bottom' }
                }
            }
        });
    }
    
    createBarChart(months, incomeData, expenseData) {
        if (this.chart) this.chart.destroy();
        
        this.chart = new Chart(this.canvas, {
            type: 'bar',
            data: {
                labels: months,
                datasets: [
                    { label: 'Income', data: incomeData, backgroundColor: '#4BC0C0' },
                    { label: 'Expenses', data: expenseData, backgroundColor: '#FF6384' }
                ]
            },
            options: {
                responsive: true,
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
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
    <title>Financial Dashboard</title>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="dashboard">
        <header class="dashboard-header">
            <h1>💰 Financial Dashboard</h1>
            <div class="date-range">
                <span id="currentMonth"></span>
            </div>
        </header>
        
        <div class="balance-cards">
            <div class="card income">
                <h3>Income</h3>
                <div class="amount" id="incomeAmount">$0.00</div>
            </div>
            <div class="card expense">
                <h3>Expenses</h3>
                <div class="amount" id="expenseAmount">$0.00</div>
            </div>
            <div class="card balance">
                <h3>Balance</h3>
                <div class="amount" id="balanceAmount">$0.00</div>
            </div>
        </div>
        
        <div class="charts-section">
            <div class="chart-container">
                <h3>Spending by Category</h3>
                <canvas id="categoryChart"></canvas>
            </div>
            <div class="chart-container">
                <h3>Monthly Overview</h3>
                <canvas id="monthlyChart"></canvas>
            </div>
        </div>
        
        <div class="transaction-section">
            <div class="add-transaction">
                <h3>Add Transaction</h3>
                <form id="transactionForm">
                    <select id="type" required>
                        <option value="expense">Expense</option>
                        <option value="income">Income</option>
                    </select>
                    <input type="number" id="amount" placeholder="Amount" required>
                    <select id="category" required></select>
                    <input type="text" id="description" placeholder="Description">
                    <button type="submit">Add</button>
                </form>
            </div>
            
            <div class="recent-transactions">
                <h3>Recent Transactions</h3>
                <div id="transactionsList"></div>
            </div>
        </div>
    </div>
    
    <script src="js/finance.js"></script>
    <script src="js/charts.js"></script>
    <script src="js/app.js"></script>
</body>
</html>
```

---

## 🎨 CSS Styling

```css
:root {
    --primary: #3498db;
    --success: #27ae60;
    --danger: #e74c3c;
    --warning: #f39c12;
    --dark: #2c3e50;
    --light: #ecf0f1;
    --white: white;
}

* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', sans-serif;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    min-height: 100vh;
    padding: 2rem;
}

.dashboard {
    max-width: 1200px;
    margin: 0 auto;
}

.dashboard-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
    color: white;
}

.balance-cards {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 1.5rem;
    margin-bottom: 2rem;
}

.card {
    background: white;
    padding: 1.5rem;
    border-radius: 15px;
    box-shadow: 0 10px 30px rgba(0,0,0,0.2);
    text-align: center;
}

.card h3 {
    color: var(--dark);
    margin-bottom: 0.5rem;
    font-size: 0.9rem;
    text-transform: uppercase;
}

.card .amount {
    font-size: 2rem;
    font-weight: bold;
}

.card.income .amount { color: var(--success); }
.card.expense .amount { color: var(--danger); }
.card.balance .amount { color: var(--primary); }

.charts-section {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 1.5rem;
    margin-bottom: 2rem;
}

.chart-container {
    background: white;
    padding: 1.5rem;
    border-radius: 15px;
    box-shadow: 0 10px 30px rgba(0,0,0,0.2);
}

.chart-container h3 {
    margin-bottom: 1rem;
    color: var(--dark);
}

.transaction-section {
    display: grid;
    grid-template-columns: 1fr 2fr;
    gap: 1.5rem;
}

.add-transaction, .recent-transactions {
    background: white;
    padding: 1.5rem;
    border-radius: 15px;
    box-shadow: 0 10px 30px rgba(0,0,0,0.2);
}

.add-transaction h3, .recent-transactions h3 {
    margin-bottom: 1rem;
    color: var(--dark);
}

#transactionForm {
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
}

#transactionForm input, 
#transactionForm select {
    padding: 0.75rem;
    border: 1px solid #ddd;
    border-radius: 8px;
    font-size: 1rem;
}

#transactionForm button {
    padding: 0.75rem;
    background: var(--primary);
    color: white;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    font-weight: bold;
}

.transaction-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.75rem;
    border-bottom: 1px solid #eee;
}

.transaction-info h4 {
    font-size: 0.95rem;
    color: var(--dark);
}

.transaction-info p {
    font-size: 0.85rem;
    color: #7f8c8d;
}

.transaction-amount {
    font-weight: bold;
    font-size: 1.1rem;
}

.transaction-amount.income { color: var(--success); }
.transaction-amount.expense { color: var(--danger); }

.delete-btn {
    background: var(--danger);
    color: white;
    border: none;
    padding: 0.25rem 0.5rem;
    border-radius: 4px;
    cursor: pointer;
    margin-left: 0.5rem;
}

@media (max-width: 900px) {
    .balance-cards { grid-template-columns: 1fr; }
    .charts-section { grid-template-columns: 1fr; }
    .transaction-section { grid-template-columns: 1fr; }
}
```

---

## 📊 Features Summary

| Feature | Implementation |
|---------|----------------|
| Balance Display | Income, expenses, balance cards |
| Category Chart | Doughnut chart with Chart.js |
| Monthly Chart | Bar chart comparing income vs expenses |
| Add Transaction | Form with type, amount, category, description |
| Transaction List | Recent transactions with delete option |
| Data Persistence | LocalStorage for all transactions |
| Dynamic Updates | Real-time chart updates on new transactions |

---

## 🔗 Related Topics

- [09_Numbers_in_Depth.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/09_Numbers_in_Depth.md)
- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)

---

**Next: [Analytics Platform Project](./07_Project_7_Analytics_Platform.md)**