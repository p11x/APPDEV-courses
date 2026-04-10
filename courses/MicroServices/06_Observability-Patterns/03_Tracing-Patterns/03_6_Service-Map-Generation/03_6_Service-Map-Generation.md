# Service Map Generation Patterns

## Overview

Service map generation visualizes the relationships between services in a microservices architecture. By analyzing trace data, service maps show which services communicate with each other, the frequency of calls, and the latency between services.

Service maps are essential for understanding system architecture, identifying dependencies, and detecting issues like tight coupling or circular dependencies. They provide the "big picture" view of distributed systems that individual traces cannot.

The service map is built by analyzing spans across all traces, extracting parent-child relationships between services, and aggregating this data to show service dependencies, call volumes, and latency distributions.

## Service Map Data Model

The service map data model consists of nodes and edges.

**Nodes** represent services. Each node contains service metadata, metrics (request rate, error rate, latency), and health status.

**Edges** represent communication between services. Each edge contains call volume, error rate, latency, and dependency direction.

## Map Generation Architecture

```mermaid
flowchart TB
    subgraph Input["Trace Input"]
        Spans[Spans from Traces]
    end
    
    subgraph Processing["Map Generation"]
        Parser[Parse Spans]
        Relation[Extract Relations]
        Aggregate[Aggregate Metrics]
        Detect[Detect Changes]
    end
    
    subgraph DataModel["Service Map Data"]
        Nodes[_nodes.json
        Edges[Edges.json
    end
    
    subgraph Output["Visualization"]
        MapUI[Service Map UI]
        Alert[Alerting]
        API[API Access]
    end
    
    Spans --> Parser
    Parser --> Relation
    Relation --> Aggregate
    Aggregate --> Nodes
    Aggregate --> Edges
    
    Nodes --> MapUI
    Edges --> MapUI
    Nodes --> Alert
    Edges --> Alert
```

Service maps are generated from trace data through relationship extraction and metric aggregation.

## Java Implementation

```java
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.stream.Collectors;

public class ServiceMapGenerationExample {
    
    public static class ServiceNode {
        public final String serviceName;
        public long requestCount;
        public long errorCount;
        public double avgLatencyMs;
        public double p50LatencyMs;
        public double p99LatencyMs;
        public Map<String, Long> versions;
        
        public ServiceNode(String serviceName) {
            this.serviceName = serviceName;
            this.requestCount = 0;
            this.errorCount = 0;
            this.versions = new HashMap<>();
        }
    }
    
    public static class ServiceEdge {
        public final String sourceService;
        public final String targetService;
        public long callCount;
        public long errorCount;
        public double avgLatencyMs;
        public double p50LatencyMs;
        public double p99LatencyMs;
        
        public ServiceEdge(String source, String target) {
            this.sourceService = source;
            this.targetService = target;
        }
    }
    
    public static class ServiceMap {
        public Map<String, ServiceNode> nodes = new ConcurrentHashMap<>();
        public Map<String, ServiceEdge> edges = new ConcurrentHashMap<>();
        
        public void addSpan(String serviceName, String parentService,
                         long latencyMs, boolean isError) {
            nodes.computeIfAbsent(serviceName, ServiceNode::new);
            
            ServiceNode node = nodes.get(serviceName);
            node.requestCount++;
            if (isError) {
                node.errorCount++;
            }
            node.avgLatencyMs = (node.avgLatencyMs * (node.requestCount - 1) + 
                              latencyMs) / node.requestCount;
            
            if (parentService != null && !parentService.equals(serviceName)) {
                String edgeKey = parentService + "->" + serviceName;
                
                edges.computeIfAbsent(edgeKey, 
                    () -> new ServiceEdge(parentService, serviceName));
                
                ServiceEdge edge = edges.get(edgeKey);
                edge.callCount++;
                if (isError) edge.errorCount++;
                edge.avgLatencyMs = (edge.avgLatencyMs * (edge.callCount - 1) + 
                                  latencyMs) / edge.callCount;
            }
        }
        
        public List<ServiceNode> getNodes() {
            return new ArrayList<>(nodes.values());
        }
        
        public List<ServiceEdge> getEdges() {
            return new ArrayList<>(edges.values());
        }
        
        public List<ServiceNode> getNodesSortedByRequestCount() {
            return nodes.values().stream()
                .sorted((a, b) -> Long.compare(b.requestCount, a.requestCount))
                .collect(Collectors.toList());
        }
        
        public List<ServiceEdge> getEdgesSortedByCallCount() {
            return edges.values().stream()
                .sorted((a, b) -> Long.compare(b.callCount, a.callCount))
                .collect(Collectors.toList());
        }
        
        public void clear() {
            nodes.clear();
            edges.clear();
        }
    }
    
    public static class SpanData {
        public String traceId;
        public String spanId;
        public String parentSpanId;
        public String serviceName;
        public String operationName;
        public long startTime;
        public long endTime;
        public String status;
        public List<String> tags;
        
        public long getDurationMs() {
            return (endTime - startTime) / 1_000_000;
        }
    }
    
    public static class ServiceMapGenerator {
        private final ServiceMap serviceMap;
        private final Map<String, List<SpanData>> spansByTrace;
        
        public ServiceMapGenerator() {
            this.serviceMap = new ServiceMap();
            this.spansByTrace = new ConcurrentHashMap<>();
        }
        
        public void addTrace(List<SpanData> spans) {
            for (SpanData span : spansByTrace.getOrDefault(
                    span.traceId, Collections.emptyList())) {
                
                String parentService = findParentService(span);
                
                serviceMap.addSpan(
                    span.serviceName,
                    parentService,
                    span.getDurationMs(),
                    "ERROR".equals(span.status)
                );
            }
        }
        
        private String findParentService(SpanData span) {
            if (span.parentSpanId == null) return null;
            
            for (List<SpanData> traceSpans : spansByTrace.values()) {
                for (SpanData parent : traceSpans) {
                    if (span.parentSpanId.equals(parent.spanId)) {
                        return parent.serviceName;
                    }
                }
            }
            return null;
        }
        
        public ServiceMap getServiceMap() {
            return serviceMap;
        }
        
        public Map<String, Object> getServiceMapJSON() {
            Map<String, Object> result = new HashMap<>();
            
            List<Map<String, Object>> nodes = new ArrayList<>();
            for (ServiceNode node : serviceMap.getNodes()) {
                Map<String, Object> nodeData = new HashMap<>();
                nodeData.put("name", node.serviceName);
                nodeData.put("requests", node.requestCount);
                nodeData.put("errors", node.errorCount);
                nodeData.put("latency_p50", node.p50LatencyMs);
                nodeData.put("latency_p99", node.p99LatencyMs);
                nodes.add(nodeData);
            }
            result.put("nodes", nodes);
            
            List<Map<String, Object>> edges = new ArrayList<>();
            for (ServiceEdge edge : serviceMap.getEdges()) {
                Map<String, Object> edgeData = new HashMap<>();
                edgeData.put("source", edge.sourceService);
                edgeData.put("target", edge.targetService);
                edgeData.put("calls", edge.callCount);
                edgeData.put("errors", edge.errorCount);
                edges.add(edgeData);
            }
            result.put("edges", edges);
            
            return result;
        }
        
        public List<String> detectCircularDependencies() {
            List<String> cycles = new ArrayList<>();
            
            for (ServiceEdge edge : serviceMap.edges.values()) {
                String source = edge.sourceService;
                String target = edge.targetService;
                
                for (ServiceEdge reverseEdge : serviceMap.edges.values()) {
                    if (reverseEdge.sourceService.equals(target) && 
                        reverseEdge.targetService.equals(source)) {
                        cycles.add(source + " <-> " + target);
                    }
                }
            }
            
            return cycles;
        }
        
        public List<String> detectUnhealthyServices(double errorThreshold) {
            List<String> unhealthy = new ArrayList<>();
            
            for (ServiceNode node : serviceMap.nodes.values()) {
                double errorRate = (double) node.errorCount / node.requestCount;
                if (errorRate > errorThreshold) {
                    unhealthy.add(node.serviceName);
                }
            }
            
            return unhealthy;
        }
    }
    
    public static class LiveServiceMap {
        private final ServiceMapGenerator generator;
        private final Timer updateTimer;
        
        public LiveServiceMap(long updateIntervalMs) {
            this.generator = new ServiceMapGenerator();
            this.updateTimer = new Timer();
            
            updateTimer.scheduleAtFixedRate(
                () -> generateMap(),
                0,
                updateIntervalMs
            );
        }
        
        private void generateMap() {
            ServiceMap map = generator.getServiceMap();
            System.out.println("Service Map Update:");
            for (ServiceNode node : map.getNodesSortedByRequestCount()) {
                System.out.printf("  %s: %d requests, %.2f%% errors%n",
                    node.serviceName,
                    node.requestCount,
                    (double) node.errorCount / node.requestCount * 100
                );
            }
        }
        
        public ServiceMap getCurrentMap() {
            return generator.getServiceMap();
        }
    }
    
    public static void main(String[] args) {
        ServiceMapGenerator generator = new ServiceMapGenerator();
        
        List<SpanData> trace1 = createMockTrace("trace-1", 
            "gateway", "order-service");
        List<SpanData> trace2 = createMockTrace("trace-2",
            "gateway", "payment-service");
        
        generator.addTrace(trace1);
        generator.addTrace(trace2);
        
        Map<String, Object> map = generator.getServiceMapJSON();
        System.out.println("Service Map: " + map);
        
        List<String> cycles = generator.detectCircularDependencies();
        System.out.println("Circular dependencies: " + cycles);
    }
    
    private static List<SpanData> createMockTrace(String traceId, 
                                          String... services) {
        List<SpanData> spans = new ArrayList<>();
        long baseTime = System.currentTimeMillis() * 1_000_000;
        
        for (int i = 0; i < services.length; i++) {
            SpanData span = new SpanData();
            span.traceId = traceId;
            span.spanId = "span-" + i;
            span.parentSpanId = i > 0 ? "span-" + (i - 1) : null;
            span.serviceName = services[i];
            span.operationName = "operation";
            span.startTime = baseTime + i * 1000000;
            span.endTime = baseTime + (i + 1) * 1000000;
            span.status = "OK";
            
            spans.add(span);
        }
        
        return spans;
    }
}
```

## Python Implementation

```python
from typing import Dict, List, Optional, Tuple
from dataclasses import dataclass, field
from collections import defaultdict
import time
import threading


@dataclass
class ServiceNode:
    """Service in the service map."""
    name: str
    request_count: int = 0
    error_count: int = 0
    avg_latency_ms: float = 0.0
    p50_latency_ms: float = 0.0
    p99_latency_ms: float = 0.0
    versions: Dict[str, int] = field(default_factory=dict)


@dataclass
class ServiceEdge:
    """Edge between services."""
    source: str
    target: str
    call_count: int = 0
    error_count: int = 0
    avg_latency_ms: float = 0.0
    p50_latency_ms: float = 0.0
    p99_latency_ms: float = 0.0


class ServiceMap:
    """Service map data structure."""
    
    def __init__(self):
        self.nodes: Dict[str, ServiceNode] = {}
        self.edges: Dict[str, ServiceEdge] = {}
    
    def add_span(self, service_name: str, parent_service: Optional[str],
              latency_ms: float, is_error: bool):
        """Add span data to the map."""
        if service_name not in self.nodes:
            self.nodes[service_name] = ServiceNode(service_name)
        
        node = self.nodes[service_name]
        node.request_count += 1
        if is_error:
            node.error_count += 1
        
        if node.request_count == 1:
            node.avg_latency_ms = latency_ms
        else:
            node.avg_latency_ms = (
                node.avg_latency_ms * (node.request_count - 1) + 
                latency_ms
            ) / node.request_count
        
        if parent_service and parent_service != service_name:
            edge_key = f"{parent_service}->{service_name}"
            
            if edge_key not in self.edges:
                self.edges[edge_key] = ServiceEdge(
                    parent_service, service_name
                )
            
            edge = self.edges[edge_key]
            edge.call_count += 1
            if is_error:
                edge.error_count += 1
    
    def get_nodes(self) -> List[ServiceNode]:
        return list(self.nodes.values())
    
    def get_edges(self) -> List[ServiceEdge]:
        return list(self.edges.values())
    
    def get_node(self, name: str) -> Optional[ServiceNode]:
        return self.nodes.get(name)
    
    def clear(self):
        """Clear the service map."""
        self.nodes.clear()
        self.edges.clear()


class ServiceMapGenerator:
    """Generate service maps from spans."""
    
    def __init__(self):
        self.service_map = ServiceMap()
        self._lock = threading.Lock()
    
    def add_trace(self, spans: List[Dict]):
        """Add spans from a trace."""
        with self._lock:
            for span in spans:
                self._process_span(span)
    
    def _process_span(self, span: Dict):
        """Process a single span."""
        service = span.get('service_name')
        parent = span.get('parent_service')
        latency = span.get('latency_ms', 0)
        is_error = span.get('status') == 'ERROR'
        
        self.service_map.add_span(service, parent, latency, is_error)
    
    def get_service_map(self) -> ServiceMap:
        return self.service_map
    
    def get_service_map_json(self) -> Dict:
        """Get service map as JSON."""
        nodes = [
            {
                'name': node.name,
                'requests': node.request_count,
                'errors': node.error_count,
                'latency_p50': node.p50_latency_ms,
                'latency_p99': node.p99_latency_ms
            }
            for node in self.service_map.get_nodes()
        ]
        
        edges = [
            {
                'source': edge.source,
                'target': edge.target,
                'calls': edge.call_count,
                'errors': edge.error_count
            }
            for edge in self.service_map.get_edges()
        ]
        
        return {'nodes': nodes, 'edges': edges}
    
    def detect_circular_dependencies(self) -> List[str]:
        """Detect circular dependencies."""
        cycles = []
        
        for edge_key, edge in self.service_map.edges.items():
            reverse_key = f"{edge.target}->{edge.source}"
            
            if reverse_key in self.service_map.edges:
                cycles.append(f"{edge.source} <-> {edge.target}")
        
        return cycles
    
    def detect_unhealthy_services(self, error_threshold: float = 0.05) -> List[str]:
        """Detect unhealthy services."""
        unhealthy = []
        
        for node in self.service_map.get_nodes():
            error_rate = node.error_count / node.request_count if node.request_count > 0 else 0
            
            if error_rate > error_threshold:
                unhealthy.append(node.name)
        
        return unhealthy
    
    def get_service_dependencies(self, service_name: str) -> List[str]:
        """Get service dependencies."""
        deps = []
        
        for edge in self.service_map.get_edges():
            if edge.source == service_name:
                deps.append(edge.target)
        
        return deps


class LiveServiceMap:
    """Live updating service map."""
    
    def __init__(self, generator: ServiceMapGenerator,
                 update_interval_seconds: int = 60):
        self.generator = generator
        self.update_interval = update_interval_seconds
        self._running = False
        self._thread = None
    
    def start(self):
        """Start live updates."""
        self._running = True
        self._thread = threading.Thread(
            target=self._update_loop,
            daemon=True
        )
        self._thread.start()
    
    def stop(self):
        """Stop live updates."""
        self._running = False
        if self._thread:
            self._thread.join()
    
    def _update_loop(self):
        """Update loop."""
        while self._running:
            self._print_map()
            time.sleep(self.update_interval)
    
    def _print_map(self):
        """Print current service map."""
        service_map = self.generator.get_service_map()
        
        print("Service Map Update:")
        
        nodes = sorted(
            service_map.get_nodes(),
            key=lambda n: n.request_count,
            reverse=True
        )
        
        for node in nodes:
            error_rate = node.error_count / node.request_count * 100 if node.request_count else 0
            print(f"  {node.name}: {node.request_count} requests, "
                  f"{error_rate:.2f}% errors")
        
        if not nodes:
            print("  (no data)")


def create_mock_span(trace_id: str, service_name: str, 
                  parent_service: Optional[str] = None) -> Dict:
    """Create mock span data."""
    return {
        'trace_id': trace_id,
        'span_id': f"span-{service_name}",
        'service_name': service_name,
        'parent_service': parent_service,
        'latency_ms': 100.0,
        'status': 'OK'
    }


if __name__ == "__main__":
    generator = ServiceMapGenerator()
    
    trace1 = [
        create_mock_span("trace-1", "gateway"),
        create_mock_span("trace-1", "order-service", "gateway"),
        create_mock_span("trace-1", "inventory-service", "order-service"),
    ]
    
    trace2 = [
        create_mock_span("trace-2", "gateway"),
        create_mock_span("trace-2", "payment-service", "gateway"),
    ]
    
    generator.add_trace(trace1)
    generator.add_trace(trace2)
    
    service_map = generator.get_service_map_json()
    print(f"Service Map: {service_map}")
    
    cycles = generator.detect_circular_dependencies()
    print(f"Circular dependencies: {cycles}")
    
    unhealthy = generator.detect_unhealthy_services(0.01)
    print(f"Unhealthy services: {unhealthy}")
```

## Real-World Examples

**Jaeger** provides built-in service map visualization showing service dependencies.

**Zipkin** generates service dependency graphs from trace data.

**Istio** provides service mesh visualization showing service communication.

## Output Statement

Organizations implementing service map generation can expect: visual understanding of service architecture; identification of circular dependencies; detection of unhealthy services; and monitoring of service dependencies over time.

Service maps provide the essential architectural overview that enables understanding and managing complex microservices systems.

## Best Practices

1. **Generate from Traces**: Build service maps from trace data for accuracy.

2. **Update Regularly**: Update service maps periodically to reflect changes.

3. **Show Metrics**: Include latency and error metrics on nodes and edges.

4. **Detect Dependencies**: Alert on new dependencies forming between services.

5. **Visualize Architecture**: Use service maps to document system architecture.

6. **Track Changes**: Monitor how service dependencies change over time.