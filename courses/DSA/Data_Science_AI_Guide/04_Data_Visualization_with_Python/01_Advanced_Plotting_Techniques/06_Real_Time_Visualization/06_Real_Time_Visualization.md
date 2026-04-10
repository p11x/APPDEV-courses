# Real-Time Visualization

## I. INTRODUCTION

### What is Real-Time Visualization?

Real-time visualization displays data that updates automatically as new data arrives, without requiring manual refresh. This is essential for monitoring dashboards, live trading systems, IoT sensor monitoring, and operational dashboards.

Key features:
- Auto-updating displays
- Live data feeds
- Streaming data handling
- Animated transitions
- WebSocket integrations

## II. FUNDAMENTALS

### Key Concepts

**Streaming Data**: Data arriving continuously over time.

**Update Interval**: How frequently the display refreshes.

**Callback**: Function triggered on new data.

**Buffer**: Temporary storage for incoming data.

## III. IMPLEMENTATION

```python
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.animation import FuncAnimation
import pandas as pd
import plotly.graph_objects as go
import time
import threading
from collections import deque
import warnings
warnings.filterwarnings('ignore')


class DataStream:
    """Simulated data stream for real-time visualization."""
    
    def __init__(self, maxlen=100):
        self.buffer = deque(maxlen=maxlen)
        self.running = False
        self.thread = None
    
    def start(self):
        """Start generating data."""
        self.running = True
        self.thread = threading.Thread(target=self._generate)
        self.thread.daemon = True
        self.thread.start()
    
    def stop(self):
        """Stop generating data."""
        self.running = False
        if self.thread:
            self.thread.join()
    
    def _generate(self):
        """Generate simulated data."""
        t = 0
        base_value = 100
        while self.running:
            value = base_value + np.random.normal(0, 5)
            self.buffer.append((t, value))
            t += 1
            time.sleep(0.1)
    
    def get_data(self):
        """Get current buffer data."""
        return list(self.buffer)


def create_matplotlib_realtime():
    """Create real-time plot with matplotlib."""
    fig, ax = plt.subplots(figsize=(10, 6))
    
    x_data = []
    y_data = []
    line, = ax.plot([], [], 'b-', lw=2)
    
    ax.set_xlim(0, 50)
    ax.set_ylim(50, 150)
    ax.set_xlabel('Time')
    ax.set_ylabel('Value')
    ax.set_title('Real-Time Data')
    
    def init():
        return line,
    
    def update(frame):
        x_data.append(frame)
        y_data.append(100 + np.random.normal(0, 10))
        
        if len(x_data) > 50:
            x_data.pop(0)
            y_data.pop(0)
        
        line.set_data(x_data, y_data)
        ax.set_xlim(max(0, len(x_data)-50), max(50, len(x_data)))
        
        return line,
    
    anim = FuncAnimation(fig, update, init_func=init, 
                       frames=range(200), interval=50, blit=True)
    
    plt.tight_layout()
    return fig, anim


def create_plotly_realtime():
    """Create real-time plot with Plotly."""
    fig = go.Figure()
    
    fig.add_trace(go.Scatter(mode='lines', name='Data'))
    
    fig.update_layout(
        xaxis=dict(range=[0, 50], autorange='reversed'),
        yaxis=dict(range=[50, 150]),
        title='Real-Time Streaming',
        xaxis_title='Time',
        yaxis_title='Value'
    )
    
    return fig


def update_plotly_realtime(fig, new_data):
    """Update Plotly figure with new data."""
    fig.update_traces(
        x=[d[0] for d in new_data],
        y=[d[1] for d in new_data]
    )
    
    return fig


def create_gauge_dashboard():
    """Create gauge dashboard for real-time metrics."""
    fig = make_subplots(
        rows=2, cols=2,
        specs=[[{'type': 'indicator'}, {'type': 'indicator'}],
               [{'type': 'indicator'}, {'type': 'indicator'}]],
        subplot_titles=['Revenue', 'Active Users', 'System Load', 'Error Rate']
    )
    
    # Add gauge traces
    fig.add_trace(go.Indicator(
        mode='gauge+number',
        value=450,
        gauge={'axis': {'range': [0, 500]}},
        title={'text': 'Revenue (K)'}
    ), row=1, col=1)
    
    fig.add_trace(go.Indicator(
        mode='gauge+number',
        value=820,
        gauge={'axis': {'range': [0, 1000]}},
        title={'text': 'Active Users'}
    ), row=1, col=2)
    
    fig.add_trace(go.Indicator(
        mode='gauge+number',
        value=72,
        gauge={'axis': {'range': [0, 100]}},
        title={'text': 'System Load %'}
    ), row=2, col=1)
    
    fig.add_trace(go.Indicator(
        mode='gauge+number',
        value=0.5,
        gauge={'axis': {'range': [0, 5]}},
        title={'text': 'Error Rate %'}
    ), row=2, col=2)
    
    fig.update_layout(height=600)
    
    return fig


def simulate_trading_updates():
    """Simulate real-time stock price updates."""
    prices = {'AAPL': 150, 'GOOG': 2800, 'MSFT': 300}
    
    fig = go.Figure()
    
    for symbol, price in prices.items():
        fig.add_trace(go.Scatter(
            y=[price],
            mode='lines',
            name=symbol
        ))
    
    fig.update_layout(
        title='Real-Time Stock Prices',
        yaxis_title='Price ($)',
        xaxis_title='Time',
        xaxis=dict(autorange='reversed')
    )
    
    return fig


def create_heatmap_realtime():
    """Create real-time heatmap."""
    np.random.seed(42)
    
    fig = go.Figure(data=[go.Heatmap(
        z=[[1, 2, 3], [4, 5, 6], [7, 8, 9]]
    )])
    
    fig.update_layout(title='Real-Time Heatmap')
    
    return fig


def create_multi_metric_dashboard():
    """Create comprehensive real-time dashboard."""
    np.random.seed(42)
    
    # Create subplot figure
    fig = make_subplots(
        rows=2, cols=2,
        subplot_titles=['Revenue Trend', 'User Activity', 'System Metrics', 'Alerts']
    )
    
    # Revenue line chart
    fig.add_trace(
        go.Scatter(
            y=np.cumsum(np.random.randn(50)),
            mode='lines',
            name='Revenue',
            line=dict(color='green')
        ),
        row=1, col=1
    )
    
    # User activity bar
    fig.add_trace(
        go.Bar(
            y=np.random.randint(100, 500, 10),
            name='Users'
        ),
        row=1, col=2
    )
    
    # System metrics gauge
    fig.add_trace(
        go.Indicator(
            mode='gauge+number',
            value=72,
            gauge={'axis': {'range': [0, 100]}}
        ),
        row=2, col=1
    )
    
    # Alert log
    alerts = ['System OK', 'Backup Complete', 'User Login', 'API Request']
    fig.add_trace(
        go.Table(
            header=dict(values=['Time', 'Alert']),
            cells=dict(values=[
                ['10:00', '10:05', '10:10', '10:15'],
                alerts
            ])
        ),
        row=2, col=2
    )
    
    fig.update_layout(height=700, title='Operational Dashboard')
    
    return fig
```

## IV. APPLICATIONS

### Banking Real-Time Dashboard

```python
def banking_realtime_dashboard():
    """Real-time banking metrics dashboard."""
    np.random.seed(42)
    
    fig = make_subplots(
        rows=2, cols=2,
        specs=[[{'type': 'scatter'}, {'type': 'indicator'}],
               [{'type': 'bar'}, {'type': 'indicator'}]],
        subplot_titles=['Transaction Volume', 'Active Sessions',
                       'Transaction Types', 'System Health']
    )
    
    # Transaction Volume
    fig.add_trace(
        go.Scatter(
            y=np.cumsum(np.random.randn(50)),
            mode='lines',
            name='Volume',
            fill='tozeroy'
        ),
        row=1, col=1
    )
    
    # Active Sessions
    fig.add_trace(
        go.Indicator(
            mode='number+gauge',
            value=1250,
            number={'suffix': ' users'}
        ),
        row=1, col=2
    )
    
    # Transaction Types
    fig.add_trace(
        go.Bar(
            x=['Deposits', 'Withdrawals', 'Transfers', 'Payments'],
            y=[450, 320, 280, 200]
        ),
        row=2, col=1
    )
    
    # System Health
    fig.add_trace(
        go.Indicator(
            mode='gauge',
            value=98,
            gauge={
                'axis': {'range': [0, 100]},
                'bar': {'color': 'green'}
            }
        ),
        row=2, col=2
    )
    
    fig.update_layout(
        height=700,
        title='Banking Operations - Real-Time'
    )
    
    return fig


def healthcare_realtime_dashboard():
    """Real-time healthcare monitoring dashboard."""
    np.random.seed(42)
    
    fig = make_subplots(
        rows=2, cols=2,
        specs=[[{'type': 'scatter'}, {'type': 'indicator'}],
               [{'type': 'scatter'}, {'type': 'indicator'}]],
        subplot_titles=['Emergency Wait Times', 'Bed Occupancy',
                       'Patient Admissions', 'Resource Usage']
    )
    
    # Emergency Wait Times
    fig.add_trace(
        go.Scatter(
            y=np.random.normal(30, 10, 50),
            mode='lines',
            fill='tozeroy'
        ),
        row=1, col=1
    )
    
    # Bed Occupancy
    fig.add_trace(
        go.Indicator(
            mode='gauge+number',
            value=87,
            number={'suffix': '%'}
        ),
        row=1, col=2
    )
    
    # Patient Admissions
    fig.add_trace(
        go.Scatter(
            y=np.cumsum(np.random.poisson(5, 50)),
            mode='lines+markers'
        ),
        row=2, col=1
    )
    
    # Resource Usage  
    fig.add_trace(
        go.Indicator(
            mode='gauge+number',
            value=65,
            number={'suffix': '%'}
        ),
        row=2, col=2
    )
    
    fig.update_layout(
        height=700,
        title='Healthcare Real-Time Monitor'
    )
    
    return fig
```

## V. BEST PRACTICES

1. Use appropriate update intervals
2. Buffer data to prevent memory issues
3. Handle disconnections gracefully
4. Use efficient plotting methods
5. Consider data throttling

## VI. CONCLUSION

### Key Takeaways

1. Real-time visualization enables live monitoring.

2. Multiple tools: matplotlib animation, Plotly streaming.

3. Gauges for KPI tracking.

4. Domain applications in finance and healthcare.

### Further Reading

- Plotly Streaming: plotly.com/python/streaming/
- Matplotlib Animation: matplotlib.org/stable/api/_as_gen/matplotlib.animation.FuncAnimation.html