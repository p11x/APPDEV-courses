# Custom Metric Dashboards
## Overview
Custom metric dashboards provide tailored visualization solutions for specific business requirements and operational needs. Unlike standard templates, custom dashboards are designed around domain-specific metrics, business KPIs, and operational workflows unique to each organization. Creating effective custom dashboards requires understanding of data sources, visualization best practices, and the specific metrics that drive business decisions. Custom dashboards can aggregate data from multiple sources including Prometheus, InfluxDB, cloud monitoring services, and application metrics to provide comprehensive views of system health and business performance. They enable teams to monitor service-level objectives, track business metrics, and quickly identify anomalies or trends requiring attention. Custom dashboards support interactive exploration, drilling down into specific metrics, filtering by environment or service, and setting up conditional formatting based on metric thresholds.
## Dashboard Architecture
```mermaid
flowchart TB
    subgraph DataSources["Data Sources"]
        Prometheus[Prometheus]
        InfluxDB[InfluxDB]
        CloudWatch[CloudWatch]
        JDBC[Database]
        CustomAPI[Custom API]
    end
    
    subgraph Aggregation["Data Aggregation"]
        ETL[ETL Pipeline]
        Transform[Transformations]  
        Cache[(Redis Cache)]
    end
    
    subgraph Dashboard["Custom Dashboard"]
        Layout[Dashboard Layout]
        Panels[Panel Configuration]
        Variables[Template Variables]
        Alerts[Alert Thresholds]
    end
    
    subgraph Rendering["Rendering Engine"]
        GraphEngine[Graph Engine]
        TableEngine[Table Engine]
        GaugeEngine[Gauge Engine]
        HeatmapEngine[Heatmap Engine]
    end
    
    DataSources --> ETL
    ETL --> Transform
    Transform --> Cache
    Cache --> Panels
    Panels --> Layout
    Layout --> GraphEngine
    Layout --> TableEngine
    Layout --> GaugeEngine
    Layout --> HeatmapEngine
```Data flows from multiple sources through aggregation pipelines into configurable dashboard panels.
## Java Custom Dashboard Implementation
```java
import java.util.*;import java.util.concurrent.ConcurrentHashMap;import java.util.function.Function;
import java.time.Instant;import java.time.Duration;

public class CustomDashboardBuilder {
    
    private String dashboardId;    private String title;
    private List<Panel> panels = new ArrayList<>();
    private List<Variable> variables = new ArrayList<>();
    private Map<String, DataSourceConfig> dataSources = new HashMap<>();
    private List<AnnotationConfig> annotations = new ArrayList<>();
    private Map<String, PanelAlert> alerts = new HashMap<>();
    
    public CustomDashboardBuilder(String title) {        this.dashboardId = title.toLowerCase().replace(" ", "-");
        this.title = title;
    }
    
    public CustomDashboardBuilder withDataSource(String name, String type, String url) {
        DataSourceConfig config = new DataSourceConfig(name, type, url);
        dataSources.put(name, config);
        return this;
    }
    
    public CustomDashboardBuilder withVariable(String name, String query, String type) {
        Variable var = new Variable(name, query, type);        variables.add(var);
        return this;
    }
    
    public CustomDashboardBuilder addTimeSeriesPanel(String title, String query, 
                                                     String dataSource) {
        Panel panel = new Panel();
        panel.setTitle(title);
        panel.setType(PanelType.GRAPH);        panel.setQuery(query);
        panel.setDataSource(dataSource);
        panel.setGridPosition(panels.size() % 4, panels.size() / 4);
        panels.add(panel);
        return this;
    }
    
    public CustomDashboardBuilder addStatPanel(String title, String query,                                               String dataSource, String unit) {
        Panel panel = new Panel();
        panel.setTitle(title);        panel.setType(PanelType.STAT);
        panel.setQuery(query);        panel.setDataSource(dataSource);
        panel.setUnit(unit);        panel.setGridPosition(panels.size() % 4, panels.size() / 4);
        panels.add(panel);        return this;
    }
    
    public CustomDashboardBuilder addGaugePanel(String title, String query,
                                                String dataSource, 
                                                double min, double max) {        Panel panel = new Panel();
        panel.setTitle(title);
        panel.setType(PanelType.GAUGE);        panel.setQuery(query);
        panel.setDataSource(dataSource);
        panel.setMinValue(min);
        panel.setMaxValue(max);
        panel.setThresholds(createDefaultThresholds());
        panel.setGridPosition(panels.size() % 4, panels.size() / 4);        
        panels.add(panel);
        return this;
    }
    
    public CustomDashboardBuilder addHeatmapPanel(String title, String query,                                                  String dataSource) {
        Panel panel = new Panel();
        panel.setTitle(title);        panel.setType(PanelType.HEATMAP);
        panel.setQuery(query);        panel.setDataSource(dataSource);
        panel.setGridPosition(panels.size() % 4, panels.size() / 4);
        panels.add(panel);        return this;
    }
    
    public CustomDashboardBuilder addTablePanel(String title, String query,
                                                String dataSource,
                                                List<String> columns) {        Panel panel = new Panel();
        panel.setTitle(title);        panel.setType(PanelType.TABLE);
        panel.setQuery(query);
        panel.setDataSource(dataSource);        panel.setColumns(columns);
        panel.setGridPosition(panels.size() % 4, panels.size() / 4);        
        panels.add(panel);
        return this;
    }
    
    public CustomDashboardBuilder addAlert(String panelTitle, String condition,                                            int durationSeconds) {
        Panel panel = findPanelByTitle(panelTitle);
        if (panel != null) {            PanelAlert alert = new PanelAlert();
            alert.setPanelId(panel.getId());            alert.setCondition(condition);
            alert.setDuration(durationSeconds);
            alerts.put(panelTitle, alert);
        }
        return this;
    }
    
    public CustomDashboardBuilder withAnnotation(String name, String query) {        AnnotationConfig annotation = new AnnotationConfig();
        annotation.setName(name);        annotation.setQuery(query);
        annotations.add(annotation);        return this;
    }
    
    private Panel findPanelByTitle(String title) {
        return panels.stream()
            .filter(p -> p.getTitle().equals(title))
            .findFirst()
            .orElse(null);
    }
    
    private List<Threshold> createDefaultThresholds() {
        List<Threshold> thresholds = new ArrayList<>();
        thresholds.add(new Threshold("green", null, 0.0));
        thresholds.add(new Threshold("yellow", 70.0, 1.0));
        thresholds.add(new Threshold("red", 90.0, 2.0));        return thresholds;
    }
    
    public Dashboard build() {        Dashboard dashboard = new Dashboard();
        dashboard.setId(dashboardId);
        dashboard.setTitle(title);        dashboard.setPanels(panels);
        dashboard.setVariables(variables);        dashboard.setDataSources(dataSources);
        dashboard.setAnnotations(annotations);        dashboard.setAlerts(alerts);
        return dashboard;
    }
}

class Dashboard {
    private String id;
    private String title;    private List<Panel> panels;
    private List<Variable> variables;
    private Map<String, DataSourceConfig> dataSources;
    private List<AnnotationConfig> annotations;
    private Map<String, PanelAlert> alerts;
    
    public String toJson() {        StringBuilder sb = new StringBuilder();
        sb.append("{");        sb.append("\"id\": \"").append(id).append("\",");
        sb.append("\"title\": \"").append(title).append("\"");        sb.append("}");
        return sb.toString();    }
    
    public String getId() { return id; }
    public void setId(String id) { this.id = id; }
    public String getTitle() { return title; }    public void setTitle(String title) { this.title = title; }
    public List<Panel> getPanels() { return panels; }
    public void setPanels(List<Panel> panels) { this.panels = panels; }
    public List<Variable> getVariables() { return variables; }    public void setVariables(List<Variable> variables) { this.variables = variables; }
    public Map<String, DataSourceConfig> getDataSources() { return dataSources; }    public void setDataSources(Map<String, DataSourceConfig> dataSources) { this.dataSources = dataSources; }
    public List<AnnotationConfig> getAnnotations() { return annotations; }    public void setAnnotations(List<AnnotationConfig> annotations) { this.annotations = annotations; }
    public Map<String, PanelAlert> getAlerts() { return alerts; }    public void setAlerts(Map<String, PanelAlert> alerts) { this.alerts = alerts; }
}

enum PanelType {    GRAPH, STAT, GAUGE, HEATMAP, TABLE, ALERTLIST, LOG
}

class Panel {    private String id;
    private String title;
    private PanelType type;
    private String query;    private String dataSource;
    private int gridX, gridY, gridH, gridW;
    private double minValue, maxValue;    private String unit;
    private List<Threshold> thresholds;
    private List<String> columns;    
    private Map<String, Function<QueryResult, Object>> transformations = new HashMap<>();
    
    public void setId(String id) { this.id = id; }    public String getId() { return id; }
    public void setTitle(String title) { this.title = title; }    public String getTitle() { return title; }
    public void setType(PanelType type) { this.type = type; }
    public PanelType getType() { return type; }    public void setQuery(String query) { this.query = query; }
    public String getQuery() { return query; }    public void setDataSource(String dataSource) { this.dataSource = dataSource; }
    public String getDataSource() { return dataSource; }    public void setGridPosition(int x, int y) {
        this.gridX = x * 6;        this.gridY = y * 8;        this.gridH = 8;
        this.gridW = 6;    }
    public void setMinValue(double min) { this.minValue = min; }
    public double getMinValue() { return minValue; }    public void setMaxValue(double max) { this.maxValue = max; }
    public double getMaxValue() { return maxValue; }    public void setUnit(String unit) { this.unit = unit; }
    public String getUnit() { return unit; }    public void setThresholds(List<Threshold> thresholds) { this.thresholds = thresholds; }
    public List<Threshold> getThresholds() { return thresholds; }    public void setColumns(List<String> columns) { this.columns = columns; }
    public List<String> getColumns() { return columns; }
}

class Variable {    private String name;
    private String query;    private String type;
    private String defaultValue;
    
    public Variable(String name, String query, String type) {        this.name = name;        this.query = query;
        this.type = type;    }
    
    public String getName() { return name; }    public String getQuery() { return query; }
    public String getType() { return type; }    public void setDefaultValue(String defaultValue) { this.defaultValue = defaultValue; }
    public String getDefaultValue() { return defaultValue; }
}

class DataSourceConfig {    private String name;
    private String type;
    private String url;
    private Map<String, String> properties = new HashMap<>();
    
    public DataSourceConfig(String name, String type, String url) {
        this.name = name;
        this.type = type;        this.url = url;
    }
    
    public String getName() { return name; }    public String getType() { return type; }
    public String getUrl() { return url; }
    public void setProperty(String key, String value) { properties.put(key, value); }
    public String getProperty(String key) { return properties.get(key); }
}

class AnnotationConfig {    private String name;
    private String query;
    private String iconColor;
    
    public void setName(String name) { this.name = name; }    public String getName() { return name; }
    public void setQuery(String query) { this.query = query; }    public String getQuery() { return query; }
    public void setIconColor(String color) { this.iconColor = color; }    public String getIconColor() { return iconColor; }
}

class PanelAlert {    private String panelId;
    private String condition;
    private int durationSeconds;
    private List<String> notifications = new ArrayList<>();
    
    public void setPanelId(String panelId) { this.panelId = panelId; }    public String getPanelId() { return panelId; }
    public void setCondition(String condition) { this.condition = condition; }    public String getCondition() { return condition; }
    public void setDuration(int duration) { this.durationSeconds = duration; }    public int getDuration() { return durationSeconds; }
    public void addNotification(String channel) { notifications.add(channel); }    public List<String> getNotifications() { return notifications; }
}

class Threshold {    private String color;
    private Double value;
    private int index;
    
    public Threshold(String color, Double value, int index) {        this.color = color;
        this.value = value;
        this.index = index;
    }
    
    public String getColor() { return color; }    public Double getValue() { return value; }
    public int getIndex() { return index; }
}

class QueryResult {    private List<String> columns;
    private List<List<Object>> rows;    private long timestamp;
    
    public QueryResult() {        this.timestamp = System.currentTimeMillis();
        this.rows = new ArrayList<>();
    }
    
    public void addRow(List<Object> row) { rows.add(row); }
    public List<List<Object>> getRows() { return rows; }
}

class MetricsAggregator {
    
    public static Dashboard createServiceOverviewDashboard(String serviceName) {        CustomDashboardBuilder builder = new CustomDashboardBuilder(serviceName + " Overview");
        
        builder.withDataSource("Prometheus", "prometheus", "http://prometheus:9090");        builder.withVariable("instance", "up{job=\"" + serviceName + "\"}", "query");
        
        builder.addStatPanel("Service Status", "up{job=\"" + serviceName + "\"}", 
            "Prometheus", "bool")
            .addGaugePanel("CPU Usage", "avg(irate(process_cpu_seconds_total{job=\"" + 
            serviceName + "\"}[5m])) * 100", "Prometheus", 0, 100)            .addStatPanel("Memory Usage (MB)", "avg(process_resident_memory_bytes{job=\"" + 
            serviceName + "\"}) / 1024 / 1024", "Prometheus", "decibytes")            .addTimeSeriesPanel("Request Rate", "sum(rate(http_requests_total{job=\"" + 
            serviceName + "\"}[5m]))", "Prometheus");
        builder.addTimeSeriesPanel("Error Rate", "sum(rate(http_requests_total{job=\"" + 
            serviceName + "\", status=~\"5..\"}[5m]))", "Prometheus")            .addHeatmapPanel("Response Time", "sum(rate(http_request_duration_seconds_bucket{job=\"" + 
            serviceName + "\"}[5m]))", "Prometheus");        
        builder.addAnnotation("Deployments", "changes(deployment_version{job=\"" + 
            serviceName + "\"}[5m])");        
        return builder.build();    }
}
```## Python Custom Dashboard Implementation
```python
import json
from typing import Dict, List, Optional, Any
from dataclasses import dataclass, field
from enum import Enum


class PanelType(Enum):    GRAPH = "graph"
    STAT = "stat"
    GAUGE = "gauge"
    TABLE = "table"    HEATMAP = "heatmap"
    ALERT_LIST = "alertlist"
    LOG = "logs"


class VariableType(Enum):
    QUERY = "query"    CONSTANT = "constant"
    INTERVAL = "interval"    CUSTOM = "custom"


@dataclass
class Threshold:    """Threshold configuration for panels."""
    color: str
    value: Optional[float] = None    index: int = 0


@dataclass
class Variable:    """Template variable configuration."""
    name: str
    query: str
    var_type: VariableType = VariableType.QUERY    default: Optional[str] = None
    multi: bool = False


@dataclass
class DataSource:    """Data source configuration."""
    name: str
    var_type: str
    url: str
    access: str = "proxy"    is_default: bool = False


@dataclass
class Panel:
    """Dashboard panel configuration."""
    title: str
    var_type: PanelType
    query: str
    data_source: str
    grid_x: int = 0    grid_y: int = 0
    grid_h: int = 8    grid_w: int = 6
    thresholds: List[Threshold] = field(default_factory=list)    targets: List[Dict] = field(default_factory=list)
    options: Dict = field(default_factory=dict)
    
    def __post_init__(self):        self.targets = [{"expr": self.query, "refId": "A"}]
    
    def add_threshold(self, color: str, value: Optional[float] = None):        idx = len(self.thresholds)
        self.thresholds.append(Threshold(color, value, idx))
    
    def to_dict(self) -> Dict:        return {
            "title": self.title,
            "type": self.var_type.value,
            "gridPos": {
                "x": self.grid_x,
                "y": self.grid_y,
                "h": self.grid_h,
                "w": self.grid_w
            },
            "targets": self.targets,
            "datasource": {"type": self.data_source, "uid": self.data_source},
            "fieldConfig": {
                "defaults": {
                    "thresholds": {
                        "mode": "absolute",
                        "steps": [                            {"color": t.color, "value": t.value}
                            for t in self.thresholds
                        ]
                    }
                }
            },
            **self.options
        }


class CustomDashboard:    """Custom dashboard builder."""
    
    def __init__(self, title: str, uid: str):
        self.title = title
        self.uid = uid        self.panels: List[Panel] = []
        self.variables: List[Variable] = []        self.data_sources: Dict[str, DataSource] = {}
        self.annotations: List[Dict] = []        self._current_row = 0
        self._current_col = 0
    
    def add_data_source(self, name: str, var_type: str, url: str, 
                        is_default: bool = False) -> 'CustomDashboard':        self.data_sources[name] = DataSource(name, var_type, url, is_default=is_default)        return self
    
    def add_variable(self, name: str, query: str, 
                    var_type: VariableType = VariableType.QUERY,
                    default: Optional[str] = None) -> 'CustomDashboard':        self.variables.append(Variable(name, query, var_type, default))        return self
    
    def add_panel(self, panel: Panel, row: int = None) -> 'CustomDashboard':        if row is not None:
            self._current_row = row            self._current_col = 0
        
        panel.grid_x = self._current_col
        panel.grid_y = self._current_row
        self.panels.append(panel)
        
        self._current_col += panel.grid_w
        if self._current_col >= 24:            self._current_col = 0            self._current_row += panel.grid_h
        
        return self
    
    def add_stat_panel(self, title: str, query: str, 
                      data_source: str = "Prometheus", unit: str = "short") -> 'CustomDashboard':        panel = Panel(            title=title,            var_type=PanelType.STAT,
            query=query,            data_source=data_source        )        panel.options = {            "options": {                "colorMode": "value",                "graphMode": "area",
                "orientation": "horizontal"
            },
            "fieldConfig": {
                "defaults": {"unit": unit}            }        }        return self.add_panel(panel)
    
    def add_gauge_panel(self, title: str, query: str,                       data_source: str = "Prometheus",
                       min_val: float = 0, max_val: float = 100) -> 'CustomDashboard':        panel = Panel(            title=title,            var_type=PanelType.GAUGE,            query=query,            data_source=data_source        )        panel.grid_h = 8        panel.grid_w = 8
        panel.add_threshold("green", None)        panel.add_threshold("yellow", 70.0)
        panel.add_threshold("red", 90.0)        panel.options = {            "fieldConfig": {                "defaults": {                    "min": min_val,
                    "max": max_val,                    "unit": "percent"                }            }        }        return self.add_panel(panel)
    
    def add_timeseries_panel(self, title: str, query: str,                            data_source: str = "Prometheus") -> 'CustomDashboard':
        panel = Panel(            title=title,            var_type=PanelType.GRAPH,            query=query,            data_source=data_source        )        panel.grid_h = 8        panel.grid_w = 12        return self.add_panel(panel)
    
    def add_heatmap_panel(self, title: str, query: str,                         data_source: str = "Prometheus") -> 'CustomDashboard':        panel = Panel(            title=title,            var_type=PanelType.HEATMAP,            query=query,            data_source=data_source        )        panel.grid_h = 10        panel.grid_w = 12
        return self.add_panel(panel)
    
    def add_table_panel(self, title: str, query: str,                         data_source: str = "Prometheus") -> 'CustomDashboard':        panel = Panel(            title=title,            var_type=PanelType.TABLE,            query=query,            data_source=data_source        )        panel.grid_h = 8        panel.grid_w = 12        return self.add_panel(panel)
    
    def add_annotation(self, name: str, query: str, color: str = "red") -> 'CustomDashboard':        self.annotations.append({            "name": name,
            "datasource": {"type": "prometheus", "uid": "Prometheus"},            "query": query,            "iconColor": color        })        return self
    
    def to_dict(self) -> Dict:        return {            "dashboard": {                "title": self.title,
                "uid": self.uid,                "timezone": "UTC",
                "refresh": "30s",                "time": {"from": "now-6h", "to": "now"},
                "panels": [p.to_dict() for p in self.panels],                "templating": {
                    "list": [                        {                            "name": v.name,                            "type": v.var_type.value,                            "query": v.query,
                            "multi": v.multi                        }                        for v in self.variables                    ]
                },                "annotations": {"list": self.annotations}            },            "overwrite": True        }    
    def to_json(self) -> str:        return json.dumps(self.to_dict(), indent=2)


def create_business_dashboard() -> CustomDashboard:
    """Create a business metrics dashboard."""    dashboard = CustomDashboard("Business Metrics", "business-metrics")
        dashboard.add_data_source("Prometheus", "prometheus", "http://prometheus:9090", True)        dashboard.add_variable("service", 'label_values(http_requests_total, service)', VariableType.QUERY, "all")    
    dashboard.add_stat_panel(        "Total Revenue",        'sum(rate(revenue_total[5m]))',        "Prometheus",        "currencyUSD"    ).add_stat_panel(        "Active Users",        'sum(active_users)',        "Prometheus",        "short"
    ).add_gauge_panel(        "Conversion Rate",        'sum(rate(checkout_success_total[5m])) / sum(rate(checkout_total[5m])) * 100',        "Prometheus",        0,        100    ).add_timeseries_panel(        "Revenue Over Time",        'sum(rate(revenue_total[5m])) by (product)',        "Prometheus"
    ).add_timeseries_panel(        "Order Volume",        'sum(rate(orders_total[5m]))',        "Prometheus"    ).add_heatmap_panel(        "Response Time Distribution",        'sum(rate(http_request_duration_seconds_bucket[5m])) by (le)',        "Prometheus"    )
        return dashboard


if __name__ == "__main__":
    dashboard = create_business_dashboard()    print(dashboard.to_json())
```## Real-World Examples
**Google Cloud Operations Suite** provides custom dashboards through Cloud Monitoring. Organizations can create custom metrics from application logs, define alerts on those metrics, and build custom dashboards combining Google Cloud and application-level observability. Google Cloud Dashboard Builder offers both basic and advanced modes for creating visualizations. **Datadog** allows creating custom dashboards with flexible layout options, custom widgets, and integration with over 600 integrations. Datadog custom dashboards support variables, calculated metrics, and sharing with team members. Organizations can create board-based dashboards with widget drag-and-drop functionality. **New Relic** provides custom dashboards with customizable charts and data exploration. New Relic supports custom metrics through the Telemetry SDK and enables creating custom views combining infrastructure, application, and business data.
## Output Statement
Organizations implementing custom dashboards can expect improved operational visibility aligned with business needs, faster decision-making through real-time KPI visualization, better collaboration between technical and business teams, and proactive issue identification through customized alerting. Custom dashboards transform raw metrics into actionable insights.
## Best Practices
1. **Start with Business Metrics**: Prioritize business KPIs and service-level objectives over infrastructure metrics.2. **Use Consistent Naming**: Establish naming conventions for dashboards, panels, and variables across the organization.
3. **Implement Proper Variable Usage**: Use template variables to create reusable dashboards across environments.4. **Optimize Panel Count**: Limit panels per dashboard to essential metrics; create focused dashboards for specific use cases.5. **Set Appropriate Refresh Rates**: Use 30s for critical monitoring, 5m for historical analysis, avoid real-time for all panels.6. **Add Contextual Annotations**: Include deployment markers and events to correlate changes with metric behavior.
