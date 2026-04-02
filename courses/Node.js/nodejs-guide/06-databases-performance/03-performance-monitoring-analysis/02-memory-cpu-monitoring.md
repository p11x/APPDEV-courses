# Memory and CPU Monitoring with Node.js

## What You'll Learn

- Memory usage monitoring and heap analysis
- CPU utilization tracking and profiling
- Event loop monitoring
- Resource leak detection
- Automated alerting for resource thresholds

## Memory Monitor

```javascript
import { performance, monitorEventLoopDelay } from 'node:perf_hooks';

class MemoryMonitor {
    constructor(options = {}) {
        this.interval = options.interval || 10000;
        this.heapWarningThreshold = options.heapWarning || 0.8; // 80% of heap limit
        this.rssWarningMB = options.rssWarning || 512;
        this.samples = [];
        this.maxSamples = options.maxSamples || 1000;
    }

    collect() {
        const usage = process.memoryUsage();
        const sample = {
            timestamp: Date.now(),
            rssMB: +(usage.rss / 1024 / 1024).toFixed(1),
            heapUsedMB: +(usage.heapUsed / 1024 / 1024).toFixed(1),
            heapTotalMB: +(usage.heapTotal / 1024 / 1024).toFixed(1),
            externalMB: +(usage.external / 1024 / 1024).toFixed(1),
            arrayBuffersMB: +(usage.arrayBuffers / 1024 / 1024).toFixed(1),
            heapUtilization: +((usage.heapUsed / usage.heapTotal) * 100).toFixed(1),
        };

        this.samples.push(sample);
        if (this.samples.length > this.maxSamples) this.samples.shift();

        if (sample.heapUtilization > this.heapWarningThreshold * 100) {
            console.warn(`High heap utilization: ${sample.heapUtilization}%`);
        }
        if (sample.rssMB > this.rssWarningMB) {
            console.warn(`High RSS: ${sample.rssMB}MB`);
        }

        return sample;
    }

    getStats() {
        if (this.samples.length === 0) return null;
        const recent = this.samples.slice(-100);

        return {
            current: this.samples[this.samples.length - 1],
            averages: {
                rssMB: +(recent.reduce((s, r) => s + r.rssMB, 0) / recent.length).toFixed(1),
                heapUsedMB: +(recent.reduce((s, r) => s + r.heapUsedMB, 0) / recent.length).toFixed(1),
                heapUtilization: +(recent.reduce((s, r) => s + r.heapUtilization, 0) / recent.length).toFixed(1),
            },
            peaks: {
                rssMB: Math.max(...recent.map(r => r.rssMB)),
                heapUsedMB: Math.max(...recent.map(r => r.heapUsedMB)),
            },
        };
    }

    detectLeak() {
        if (this.samples.length < 50) return null;

        const recent = this.samples.slice(-50);
        const first10 = recent.slice(0, 10);
        const last10 = recent.slice(-10);

        const avgFirst = first10.reduce((s, r) => s + r.heapUsedMB, 0) / first10.length;
        const avgLast = last10.reduce((s, r) => s + r.heapUsedMB, 0) / last10.length;
        const growthRate = ((avgLast - avgFirst) / avgFirst) * 100;

        return {
            growthRate: +growthRate.toFixed(1),
            suspectedLeak: growthRate > 10,
            avgFirstMB: +avgFirst.toFixed(1),
            avgLastMB: +avgLast.toFixed(1),
        };
    }

    start() {
        this.timer = setInterval(() => this.collect(), this.interval);
    }

    stop() {
        clearInterval(this.timer);
    }
}
```

## CPU Monitor

```javascript
import os from 'node:os';

class CPUMonitor {
    constructor(options = {}) {
        this.interval = options.interval || 10000;
        this.samples = [];
        this.maxSamples = options.maxSamples || 1000;
        this.lastCPUTime = process.cpuUsage();
        this.lastTimestamp = Date.now();
    }

    collect() {
        const currentCPUTime = process.cpuUsage(this.lastCPUTime);
        const currentTimestamp = Date.now();
        const elapsed = currentTimestamp - this.lastTimestamp;

        const userPercent = (currentCPUTime.user / 1000 / elapsed) * 100;
        const systemPercent = (currentCPUTime.system / 1000 / elapsed) * 100;

        const loadAvg = os.loadavg();
        const cpuCount = os.cpus().length;

        const sample = {
            timestamp: currentTimestamp,
            processUser: +userPercent.toFixed(1),
            processSystem: +systemPercent.toFixed(1),
            processTotal: +(userPercent + systemPercent).toFixed(1),
            systemLoad1: +loadAvg[0].toFixed(2),
            systemLoad5: +loadAvg[1].toFixed(2),
            systemLoad15: +loadAvg[2].toFixed(2),
            cpuCount,
            systemLoadPercent: +((loadAvg[0] / cpuCount) * 100).toFixed(1),
        };

        this.samples.push(sample);
        if (this.samples.length > this.maxSamples) this.samples.shift();

        this.lastCPUTime = process.cpuUsage();
        this.lastTimestamp = currentTimestamp;

        return sample;
    }

    getStats() {
        if (this.samples.length === 0) return null;
        const recent = this.samples.slice(-100);

        return {
            current: this.samples[this.samples.length - 1],
            averages: {
                processTotal: +(recent.reduce((s, r) => s + r.processTotal, 0) / recent.length).toFixed(1),
                systemLoad: +(recent.reduce((s, r) => s + r.systemLoad1, 0) / recent.length).toFixed(2),
            },
        };
    }

    start() {
        this.timer = setInterval(() => this.collect(), this.interval);
    }

    stop() {
        clearInterval(this.timer);
    }
}
```

## Event Loop Monitor

```javascript
import { monitorEventLoopDelay, eventLoopUtilization } from 'node:perf_hooks';

class EventLoopMonitor {
    constructor(options = {}) {
        this.histogram = monitorEventLoopDelay({ resolution: options.resolution || 20 });
        this.histogram.enable();
        this.lastELU = eventLoopUtilization();
        this.samples = [];
        this.maxSamples = options.maxSamples || 1000;
        this.interval = options.interval || 10000;
    }

    collect() {
        const elu = eventLoopUtilization(this.lastELU);
        this.lastELU = eventLoopUtilization();

        const sample = {
            timestamp: Date.now(),
            utilization: +(elu.utilization * 100).toFixed(1),
            active: +elu.active.toFixed(2),
            idle: +elu.idle.toFixed(2),
            lagMeanMs: +(this.histogram.mean / 1e6).toFixed(2),
            lagP50Ms: +(this.histogram.percentile(50) / 1e6).toFixed(2),
            lagP95Ms: +(this.histogram.percentile(95) / 1e6).toFixed(2),
            lagP99Ms: +(this.histogram.percentile(99) / 1e6).toFixed(2),
            lagMaxMs: +(this.histogram.max / 1e6).toFixed(2),
        };

        this.samples.push(sample);
        if (this.samples.length > this.maxSamples) this.samples.shift();

        if (sample.lagP99Ms > 100) {
            console.warn(`High event loop lag (p99): ${sample.lagP99Ms}ms`);
        }

        this.histogram.reset();
        return sample;
    }

    start() {
        this.timer = setInterval(() => this.collect(), this.interval);
    }

    stop() {
        clearInterval(this.timer);
    }
}
```

## Combined Resource Monitor

```javascript
class ResourceMonitor {
    constructor(options = {}) {
        this.memory = new MemoryMonitor(options.memory);
        this.cpu = new CPUMonitor(options.cpu);
        this.eventLoop = new EventLoopMonitor(options.eventLoop);
        this.interval = options.interval || 10000;
        this.alerts = [];
    }

    start() {
        this.memory.start();
        this.cpu.start();
        this.eventLoop.start();

        this.timer = setInterval(() => {
            this.checkThresholds();
        }, this.interval);
    }

    checkThresholds() {
        const mem = this.memory.getStats();
        const cpu = this.cpu.getStats();
        const el = this.eventLoop.collect();

        if (mem?.current.heapUtilization > 85) {
            this.alert({ type: 'memory', message: `Heap utilization: ${mem.current.heapUtilization}%` });
        }
        if (cpu?.current.processTotal > 80) {
            this.alert({ type: 'cpu', message: `Process CPU: ${cpu.current.processTotal}%` });
        }
        if (el.lagP99Ms > 100) {
            this.alert({ type: 'eventloop', message: `Event loop p99 lag: ${el.lagP99Ms}ms` });
        }
    }

    alert(alert) {
        this.alerts.push({ ...alert, timestamp: Date.now() });
        console.warn(`[ALERT] ${alert.type}: ${alert.message}`);
    }

    getDashboard() {
        return {
            memory: this.memory.getStats(),
            cpu: this.cpu.getStats(),
            eventLoop: this.eventLoop.getStats(),
            memoryLeak: this.memory.detectLeak(),
            alerts: this.alerts.slice(-20),
        };
    }

    stop() {
        this.memory.stop();
        this.cpu.stop();
        this.eventLoop.stop();
        clearInterval(this.timer);
    }
}

// Express endpoint
const monitor = new ResourceMonitor();
monitor.start();

app.get('/api/resources', (req, res) => {
    res.json(monitor.getDashboard());
});
```

## Best Practices Checklist

- [ ] Monitor memory usage at regular intervals
- [ ] Set alerts for heap utilization > 80%
- [ ] Track CPU usage to detect hot paths
- [ ] Monitor event loop lag for I/O bottlenecks
- [ ] Detect memory leaks with trend analysis
- [ ] Expose resource metrics via API endpoints
- [ ] Use `--max-old-space-size` to set heap limits
- [ ] Profile with `--inspect` for detailed analysis

## Cross-References

- See [APM Setup](./01-apm-setup.md) for application monitoring
- See [Database Profiling](../02-database-performance-optimization/06-database-monitoring-profiling.md) for DB profiling
- See [Performance Testing](../08-performance-testing-benchmarking/01-load-testing.md) for load testing

## Next Steps

Continue to [I/O and Network Monitoring](./03-io-network-monitoring.md) for I/O performance analysis.
