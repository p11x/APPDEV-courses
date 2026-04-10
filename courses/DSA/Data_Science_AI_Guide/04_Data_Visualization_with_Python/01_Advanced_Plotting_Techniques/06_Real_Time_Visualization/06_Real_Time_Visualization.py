# Topic: Real Time Visualization
# Author: AI Assistant
# Date: 06-04-2026

"""
Comprehensive implementation for Real Time Visualization

I. INTRODUCTION
This module covers real-time visualization techniques including
live updating charts, streaming data visualization, and
dynamic dashboard elements.

II. CORE CONCEPTS
- Live data updates
- Streaming visualization
- Dynamic animations

III. IMPLEMENTATION
- matplotlib animation
- plotly live updates
- threading for real-time data

IV. EXAMPLES
- Banking: Live stock prices
- Healthcare: Real-time monitoring

V. OUTPUT RESULTS
- Real-time visualizations

VI. TESTING
- Animation functions

VII. ADVANCED TOPICS
- WebSocket streaming

VIII. CONCLUSION
Best practices for real-time visualization
"""

import matplotlib.pyplot as plt
import matplotlib.animation as animation
import seaborn as sns
import numpy as np
import pandas as pd
import plotly.graph_objects as go
from plotly.subplots import make_subplots
import warnings
import threading
import time
from collections import deque
warnings.filterwarnings('ignore')

plt.style.use('seaborn-v0_8-whitegrid')


class RealTimeDataGenerator:
    """Generate real-time data."""
    
    def __init__(self, maxlen=100):
        self.data = deque(maxlen=maxlen)
        self.running = False
        self.thread = None
    
    def start(self, interval=0.1):
        self.running = True
        self.thread = threading.Thread(target=self._generate, args=(interval,))
        self.thread.daemon = True
        self.thread.start()
    
    def stop(self):
        self.running = False
        if self.thread:
            self.thread.join()
    
    def _generate(self, interval):
        counter = 0
        while self.running:
            value = np.sin(counter * 0.1) + np.random.randn() * 0.1
            self.data.append(value)
            counter += 1
            time.sleep(interval)
    
    def get_data(self):
        return list(self.data)


def create_live_plot(max_points=100, interval=50):
    """Create live updating plot."""
    fig, ax = plt.subplots(figsize=(12, 6))
    
    data = deque(maxlen=max_points)
    x_data = deque(maxlen=max_points)
    
    line, = ax.plot([], [], 'b-', linewidth=2)
    
    x = 0
    
    def init():
        ax.set_xlim(0, max_points)
        ax.set_ylim(-3, 3)
        ax.set_xlabel('Time')
        ax.set_ylabel('Value')
        ax.set_title('Real-Time Data Stream')
        ax.grid(True, alpha=0.3)
        return line,
    
    def update(frame):
        nonlocal x
        value = np.sin(x * 0.1) + np.random.randn() * 0.1
        x_data.append(x)
        data.append(value)
        
        line.set_data(x_data, data)
        ax.set_xlim(max(0, x - max_points), x + 10)
        
        x += 1
        return line,
    
    ani = animation.FuncAnimation(fig, update, init_func=init,
                                frames=200, interval=interval, blit=True)
    
    plt.tight_layout()
    
    return fig, ani


def create_scrolling_plot(data_gen, max_points=100, interval=100):
    """Create scrolling chart."""
    fig, ax = plt.subplots(figsize=(12, 6))
    
    line, = ax.plot([], [], 'b-', marker='o', markersize=4, linewidth=2)
    fill = ax.fill_between([], [], alpha=0.3, color='blue')
    
    def init():
        ax.set_xlim(0, max_points)
        ax.set_ylim(-3, 3)
        ax.set_xlabel('Time')
        ax.set_ylabel('Value')
        ax.set_title('Scrolling Chart')
        ax.grid(True, alpha=0.3)
        return line,
    
    def update(frame):
        data = data_gen.get_data()
        
        if len(data) > 0:
            x = list(range(max(0, len(data) - max_points), len(data)))
            line.set_data(x, data[-max_points:])
            
            ax.collections.clear()
            ax.fill_between(x, [v - 0.5 for v in data[-max_points:]],
                          [v + 0.5 for v in data[-max_points:]], 
                          alpha=0.2, color='blue')
        
        return line,
    
    ani = animation.FuncAnimation(fig, update, init_func=init,
                                frames=200, interval=interval, blit=True)
    
    plt.tight_layout()
    
    return fig, ani


def create_multi_series_live(series_count=3, max_points=100):
    """Create multi-series live chart."""
    fig, ax = plt.subplots(figsize=(12, 6))
    
    lines = []
    for i in range(series_count):
        line, = ax.plot([], [], linewidth=2, label=f'Series {i+1}')
        lines.append(line)
    
    def init():
        ax.set_xlim(0, max_points)
        ax.set_ylim(-5, 5)
        ax.set_xlabel('Time')
        ax.set_ylabel('Value')
        ax.set_title('Multi-Series Live Chart')
        ax.legend()
        ax.grid(True, alpha=0.3)
        return lines,
    
    frame_count = [0]
    
    def update(frame):
        for line in lines:
            offset = lines.index(line) * 2
            value = np.sin(frame * 0.1 + offset) + np.random.randn() * 0.1
            
            data = line.get_data()
            x_data = list(data[0]) if len(data) > 0 else []
            y_data = list(data[1]) if len(data) > 0 else []
            
            if len(x_data) >= max_points:
                x_data.pop(0)
                y_data.pop(0)
            
            x_data.append(frame_count[0])
            y_data.append(value)
            
            line.set_data(x_data, y_data)
        
        ax.set_xlim(max(0, frame_count[0] - max_points), frame_count[0] + 10)
        frame_count[0] += 1
        
        return lines,
    
    ani = animation.FuncAnimation(fig, update, init_func=init,
                                frames=200, interval=100, blit=True)
    
    plt.tight_layout()
    
    return fig, ani


def create_dashboard_panel():
    """Create dashboard with multiple panels."""
    fig = plt.figure(figsize=(16, 10))
    
    gs = fig.add_gridspec(3, 3, hspace=0.3, wspace=0.3)
    
    ax1 = fig.add_subplot(gs[0, 0])
    ax2 = fig.add_subplot(gs[0, 1])
    ax3 = fig.add_subplot(gs[0, 2])
    ax4 = fig.add_subplot(gs[1, :2])
    ax5 = fig.add_subplot(gs[1, 2])
    ax6 = fig.add_subplot(gs[2, :2])
    ax7 = fig.add_subplot(gs[2, 2])
    
    return fig, [ax1, ax2, ax3, ax4, ax5, ax6, ax7]


def create_stock_ticker():
    """Create stock ticker visualization."""
    fig, ax = plt.subplots(figsize=(14, 6))
    
    stocks = ['AAPL', 'GOOGL', 'MSFT', 'AMZN', 'TSLA']
    prices = [150, 2800, 300, 3300, 700]
    changes = [2.5, -1.2, 0.8, 3.2, -0.5]
    
    colors = ['green' if c >= 0 else 'red' for c in changes]
    
    bars = ax.bar(stocks, prices, color=colors, alpha=0.7)
    
    for bar, price, change in zip(bars, prices, changes):
        height = bar.get_height()
        ax.text(bar.get_x() + bar.get_width()/2., height,
               f'${price}\n({change:+.1f}%)',
               ha='center', va='bottom', fontsize=10)
    
    ax.set_ylabel('Stock Price ($)')
    ax.set_title('Live Stock Ticker')
    ax.grid(True, alpha=0.3, axis='y')
    
    plt.tight_layout()
    
    return fig


def create_gauge_chart(value, title='Gauge', min_val=0, max_val=100):
    """Create gauge chart."""
    fig, ax = plt.subplots(figsize=(8, 6), subplot_kw={'projection': 'polar'})
    
    theta = np.linspace(0, np.pi, 100)
    r = np.ones(100)
    
    ax.fill_between(theta, 0, r, alpha=0.3, color='gray')
    
    value_norm = (value - min_val) / (max_val - min_val) * np.pi
    ax.fill_between([0, value_norm], 0, 1, alpha=0.7, color='green')
    
    ax.set_ylim(0, 1)
    ax.set_xticks([])
    ax.set_yticks([])
    ax.set_title(title, fontsize=14, fontweight='bold', pad=20)
    
    ax.text(np.pi/2, 0.5, f'{value:.1f}', ha='center', va='center',
           fontsize=24, fontweight='bold')
    
    plt.tight_layout()
    
    return fig


def create_metrics_dashboard():
    """Create real-time metrics dashboard."""
    fig = plt.figure(figsize=(18, 12))
    
    metrics = [
        ('CPU Usage', 45, 100),
        ('Memory', 72, 100),
        ('Network', 2300, 5000),
        ('Disk I/O', 150, 1000),
        ('Active Users', 1250, 5000),
        ('Requests/sec', 340, 1000)
    ]
    
    positions = [(0, 0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2)]
    
    for i, (name, value, max_v) in enumerate(metrics):
        ax = fig.add_subplot(2, 3, i+1, projection='polar')
        
        value_norm = value / max_v
        
        theta = np.linspace(0, 2*np.pi*value_norm, 50)
        r = np.ones(50)
        
        ax.fill(theta, r, alpha=0.5, color='steelblue')
        ax.plot(theta, r, linewidth=2, color='steelblue')
        
        ax.set_ylim(0, 1.5)
        ax.set_xticks([])
        ax.set_yticks([])
        ax.set_title(f'{name}\n{value}/{max_v}', fontsize=12, fontweight='bold')
        
        ax.text(np.pi, 0.7, f'{value_norm*100:.0f}%', ha='center', 
               fontsize=16, fontweight='bold')
    
    plt.tight_layout()
    
    return fig


def create_streaming_line():
    """Create streaming line chart."""
    fig = go.Figure()
    
    fig.add_trace(go.Scatter(x=[], y=[], mode='lines', name='Data'))
    
    fig.update_layout(
        xaxis_title='Time',
        yaxis_title='Value',
        title='Streaming Data',
        xaxis=dict(range=[0, 100], autorange=False),
        yaxis=dict(range=[-3, 3], autorange=False),
        template='plotly_white'
    )
    
    return fig


def update_streaming_line(fig, x, y):
    """Update streaming line."""
    fig.data[0].x = list(x)
    fig.data[0].y = list(y)
    fig.update_xaxes(range=[min(x), max(x)])
    fig.update_yaxes(range=[min(y)-0.5, max(y)+0.5])
    
    return fig


def core_implementation():
    """Core real-time visualization."""
    results = {}
    
    results['stock_ticker'] = create_stock_ticker()
    results['gauge'] = create_gauge_chart(75, 'Performance')
    results['metrics'] = create_metrics_dashboard()
    results['streaming'] = create_streaming_line()
    
    return results


def banking_example():
    """Banking - Live Trading Dashboard."""
    fig = plt.figure(figsize=(18, 14))
    
    gs = fig.add_gridspec(3, 3, hspace=0.3, wspace=0.3)
    
    ax1 = fig.add_subplot(gs[0, :])
    ax2 = fig.add_subplot(gs[1, 0])
    ax3 = fig.add_subplot(gs[1, 1])
    ax4 = fig.add_subplot(gs[1, 2])
    ax5 = fig.add_subplot(gs[2, :])
    
    stocks = ['AAPL', 'GOOG', 'MSFT', 'AMZN', 'TSLA']
    prices = [150, 2800, 300, 3300, 700]
    changes = [2.5, -1.2, 0.8, 3.2, -0.5]
    colors = ['green' if c >= 0 else 'red' for c in changes]
    
    bars = ax1.bar(stocks, prices, color=colors, alpha=0.7)
    ax1.set_ylabel('Price ($)')
    ax1.set_title('Live Stock Prices')
    ax1.grid(True, alpha=0.3, axis='y')
    
    x = np.arange(50)
    y1 = np.cumsum(np.random.randn(50))
    ax2.plot(x, y1, linewidth=2, color='blue')
    ax2.set_title('Portfolio Value')
    ax2.grid(True, alpha=0.3)
    
    ax3.pie([25, 30, 20, 15, 10], labels=stocks, autopct='%1.1f%%',
           colors=plt.cm.Set3.colors)
    ax3.set_title('Portfolio Allocation')
    
    metrics = ['Returns', 'Risk', 'Sharpe', 'Volatility']
    values = [12.5, 15.2, 0.85, 18.3]
    colors_m = ['green', 'blue', 'purple', 'orange']
    ax4.barh(metrics, values, color=colors_m)
    ax4.set_xlabel('Value')
    ax4.set_title('Metrics')
    ax4.grid(True, alpha=0.3)
    
    x_full = np.arange(100)
    y_full = np.cumsum(np.random.randn(100))
    ax5.plot(x_full, y_full, linewidth=2)
    ax5.fill_between(x_full, y_full, alpha=0.3)
    ax5.axhline(y_full.mean(), color='red', linestyle='--', 
               label=f'Mean: {y_full.mean():.1f}')
    ax5.set_xlabel('Time')
    ax5.set_ylabel('Value')
    ax5.set_title('Account Value History')
    ax5.legend()
    ax5.grid(True, alpha=0.3)
    
    plt.tight_layout()
    
    print("=" * 60)
    print("LIVE TRADING DASHBOARD")
    print("=" * 60)
    print(f"\nStocks Tracked: {len(stocks)}")
    print(f"Portfolio Value: ${np.mean(prices)*100:,.0f}")
    print(f"Average Change: {np.mean(changes):.1f}%")
    
    return fig


def healthcare_example():
    """Healthcare - Real-time Patient Monitoring."""
    fig = plt.figure(figsize=(18, 14))
    
    gs = fig.add_gridspec(3, 3, hspace=0.3, wspace=0.3)
    
    ax1 = fig.add_subplot(gs[0, :])
    ax2 = fig.add_subplot(gs[1, 0])
    ax3 = fig.add_subplot(gs[1, 1])
    ax4 = fig.add_subplot(gs[1, 2])
    ax5 = fig.add_subplot(gs[2, :])
    
    vitals = ['HR', 'BP', 'Temp', 'SpO2', 'RR']
    values = [72, 120, 98.6, 98, 16]
    ranges = [(60, 100), (90, 140), (97, 99), (95, 100), (12, 20)]
    colors = []
    for i, (val, (low, high)) in enumerate(zip(values, ranges)):
        if low <= val <= high:
            colors.append('green')
        else:
            colors.append('red')
    
    bars = ax1.bar(vitals, values, color=colors, alpha=0.7)
    ax1.axhline(y=[r[0] for r in ranges], color='green', linestyle='--', alpha=0.5)
    ax1.axhline(y=[r[1] for r in ranges], color='red', linestyle='--', alpha=0.5)
    ax1.set_ylabel('Value')
    ax1.set_title('Patient Vitals')
    ax1.grid(True, alpha=0.3, axis='y')
    
    t = np.arange(50)
    hr = 70 + np.cumsum(np.random.randn(50))
    ax2.plot(t, hr, linewidth=2, color='red')
    ax2.set_xlabel('Time')
    ax2.set_ylabel('Heart Rate')
    ax2.set_title('Heart Rate Trend')
    ax2.grid(True, alpha=0.3)
    
    bp = 120 + np.cumsum(np.random.randn(50))
    ax3.plot(t, bp, linewidth=2, color='blue')
    ax3.set_xlabel('Time')
    ax3.set_ylabel('Blood Pressure')
    ax3.set_title('BP Trend')
    ax3.grid(True, alpha=0.3)
    
    ax4.pie([40, 30, 20, 10], labels=['Stable', 'Warning', 'Critical', 'Discharged'],
           autopct='%1.1f%%', colors=['green', 'yellow', 'red', 'blue'])
    ax4.set_title('Ward Status')
    
    bed_occupancy = np.random.randint(50, 100)
    ax5.text(0.5, 0.6, f'{bed_occupancy}%', ha='center', va='center',
            fontsize=48, fontweight='bold')
    ax5.text(0.5, 0.4, 'Bed Occupancy', ha='center', va='center',
            fontsize=16)
    ax5.set_xlim(0, 1)
    ax5.set_ylim(0, 1)
    ax5.axis('off')
    ax5.set_title('Hospital Status')
    
    plt.tight_layout()
    
    print("=" * 60)
    print("REAL-TIME PATIENT MONITORING")
    print("=" * 60)
    print(f"\nHeart Rate: {values[0]} bpm")
    print(f"Blood Pressure: {values[1]} mmHg")
    print(f"Temperature: {values[2]}°F")
    print(f"SpO2: {values[3]}%")
    print(f"Respiratory Rate: {values[4]} breaths/min")
    
    return fig


def main():
    """Main execution."""
    print("Executing Real Time Visualization")
    print("=" * 60)
    
    create_stock_ticker()
    create_gauge_chart(75)
    create_metrics_dashboard()
    
    plt.show()
    
    return True


if __name__ == "__main__":
    main()