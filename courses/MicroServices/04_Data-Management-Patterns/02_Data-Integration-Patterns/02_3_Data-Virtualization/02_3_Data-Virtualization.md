# Data Virtualization

## Overview

Data virtualization is a pattern that enables transparent access to data distributed across multiple sources without physically moving or replicating the data. Instead of copying data into a central data warehouse, data virtualization creates a logical abstraction layer that federates queries across heterogeneous data sources. Applications query the virtual layer as if working with a single database, while the virtualization engine decomposes queries, pushes down predicate filters to source systems, and aggregates results. This pattern is particularly valuable in microservices architectures where each service owns its data but other services need integrated views.

The federation layer accepts queries and determines the optimal execution plan based on source capabilities. Not all data sources support pushdown operations, so the engine may need to fetch partial results and perform filtering or joining in the federation layer. Query rewriting transforms user queries to leverage source-specific capabilities, converting standard SQL or API calls into native formats understood by each data source. The pattern supports both real-time queries against operational systems and aggregated analytical workloads.

Data virtualization addresses several challenges in distributed architectures. It eliminates the need for extensive data duplication, reducing storage costs and synchronization complexity. It provides a unified view over data that cannot be consolidated due to sovereignty requirements or technical constraints. It enables applications to evolve independently without requiring changes to a centralized data layer. However, performance can be variable depending on source system capabilities and network latency, making this pattern most suitable for queries with predictable performance requirements or where fresh data is essential.

The pattern supports various deployment models: as a library embedded in applications, as a middleware layer that intercepts queries, or as a gateway that handles federation across services. Modern implementations often use GraphQL or similar query languages that provide flexibility in request structure while enabling efficient backend query generation. Some organizations use data virtualization as an intermediary step toward eventual data consolidation, while others maintain it as a permanent architecture for regulatory or operational reasons.

## Flow Chart

```mermaid
flowchart TD
    A[Client Application] -->|GraphQL/SQL Query| B[Data Virtualization Layer]
    
    B --> C{Query Analysis}
    C -->|Determine Sources| D[Query Analyzer]
    D --> E[Source Capability Detection]
    
    E --> F[Query Rewrite Engine]
    F --> G{Determine Pushdown Strategy}
    
    G -->|Full Pushdown| H[PostgreSQL Service]
    G -->|Full Pushdown| I[MongoDB Service]
    G -->|Partial Processing| J[REST API Service]
    G -->|Partial Processing| K[Legacy System]
    
    H -.->|Results| F
    I -.->|Results| F
    J -.->|Results| L[Aggregation Engine]
    K -.->|Results| L
    
    L -->|Aggregated Results| F
    F -->|Final Result| B
    B -->|Unified Response| A
    
    M[Metadata Catalog] --> E
    N[Query Cache] -->|Cached Results| B
    
    style B fill:#fff3e0
    style L fill:#e8f5e9
    style H fill:#e1f5fe
    style I fill:#e1f5fe
```

## Standard Example

### Java Implementation with GraphQL Federation

```java
import java.util.*;
import java.util.concurrent.*;
import java.util.stream.Collectors;

public class DataVirtualizationExample {
    
    public interface DataSource {
        String getName();
        Set<String> getSupportedOperations();
        CompletableFuture<List<Map<String, Object>>> query(
            String table, 
            Map<String, Object> filters,
            List<String> columns
        );
    }
    
    public interface QueryExecutor {
        CompletableFuture<List<Map<String, Object>>> execute(
            String query,
            Map<String, Object> variables
        );
    }
    
    public static class PostgreSQLDataSource implements DataSource {
        private final String connectionUrl;
        private final String username;
        private final String password;
        
        public PostgreSQLDataSource(String url, String user, String pass) {
            this.connectionUrl = url;
            this.username = user;
            this.password = pass;
        }
        
        @Override
        public String getName() {
            return "postgresql";
        }
        
        @Override
        public Set<String> getSupportedOperations() {
            return Set.of("eq", "gt", "lt", "like", "in", "between", "orderBy", "limit");
        }
        
        @Override
        public CompletableFuture<List<Map<String, Object>>> query(
            String table,
            Map<String, Object> filters,
            List<String> columns
        ) {
            StringBuilder sql = new StringBuilder("SELECT ");
            sql.append(String.join(", ", columns));
            sql.append(" FROM ").append(table);
            
            if (!filters.isEmpty()) {
                sql.append(" WHERE ");
                List<String> conditions = new ArrayList<>();
                for (Map.Entry<String, Object> entry : filters.entrySet()) {
                    conditions.add(entry.getKey() + " = $" + entry.getValue());
                }
                sql.append(String.join(" AND ", conditions));
            }
            
            System.out.println("Executing on PostgreSQL: " + sql);
            return CompletableFuture.completedFuture(List.of());
        }
    }
    
    public static class MongoDBDataSource implements DataSource {
        private final String connectionUrl;
        private final String database;
        
        public MongoDBDataSource(String url, String db) {
            this.connectionUrl = url;
            this.database = db;
        }
        
        @Override
        public String getName() {
            return "mongodb";
        }
        
        @Override
        public Set<String> getSupportedOperations() {
            return Set.of("eq", "in", "regex", "limit");
        }
        
        @Override
        public CompletableFuture<List<Map<String, Object>>> query(
            String table,
            Map<String, Object> filters,
            List<String> columns
        ) {
            Map<String, Object> query = new HashMap<>();
            query.put("collection", table);
            query.put("filter", filters);
            query.put("projection", columns.stream()
                .collect(Collectors.toMap(c -> c, c -> 1)));
            
            System.out.println("Executing on MongoDB: " + query);
            return CompletableFuture.completedFuture(List.of());
        }
    }
    
    public static class RESTDataSource implements DataSource {
        private final String baseUrl;
        
        public RESTDataSource(String url) {
            this.baseUrl = url;
        }
        
        @Override
        public String getName() {
            return "rest";
        }
        
        @Override
        public Set<String> getSupportedOperations() {
            return Set.of("eq", "limit");
        }
        
        @Override
        public CompletableFuture<List<Map<String, Object>>> query(
            String table,
            Map<String, Object> filters,
            List<String> columns
        ) {
            StringBuilder url = new StringBuilder(baseUrl);
            url.append("/").append(table);
            
            if (!filters.isEmpty()) {
                url.append("?");
                url.append(filters.entrySet().stream()
                    .map(e -> e.getKey() + "=" + e.getValue())
                    .collect(Collectors.joining("&")));
            }
            
            System.out.println("Executing REST call: " + url);
            return CompletableFuture.completedFuture(List.of());
        }
    }
    
    public static class QueryAnalyzer {
        
        public Map<String, List<String>> analyzeSources(String query) {
            Map<String, List<String>> sourceColumns = new HashMap<>();
            
            if (query.contains("customers")) {
                sourceColumns.put("postgresql", List.of("id", "name", "email"));
            }
            if (query.contains("orders")) {
                sourceColumns.put("mongodb", List.of("orderId", "customerId", "total"));
            }
            if (query.contains("products")) {
                sourceColumns.put("rest", List.of("sku", "name", "price"));
            }
            
            return sourceColumns;
        }
        
        public Map<String, Object> rewriteFilters(
            Map<String, Object> filters,
            DataSource source
        ) {
            Set<String> supportedOps = source.getSupportedOperations();
            Map<String, Object> rewritten = new HashMap<>();
            
            for (Map.Entry<String, Object> entry : filters.entrySet()) {
                String operation = detectOperation(entry.getKey());
                if (supportedOps.contains(operation)) {
                    rewritten.put(entry.getKey(), entry.getValue());
                }
            }
            
            return rewritten;
        }
        
        private String detectOperation(String filter) {
            if (filter.contains("_gt")) return "gt";
            if (filter.contains("_lt")) return "lt";
            if (filter.contains("_like")) return "like";
            if (filter.contains("_in")) return "in";
            return "eq";
        }
    }
    
    public static class FederationEngine {
        
        private final Map<String, DataSource> dataSources;
        private final QueryAnalyzer analyzer;
        private final List<String> cache;
        
        public FederationEngine() {
            this.dataSources = new ConcurrentHashMap<>();
            this.analyzer = new QueryAnalyzer();
            this.cache = new CopyOnWriteArrayList<>();
        }
        
        public void registerDataSource(String name, DataSource source) {
            dataSources.put(name, source);
            System.out.println("Registered data source: " + name);
        }
        
        public CompletableFuture<List<Map<String, Object>>> federateQuery(
            String query,
            Map<String, Object> variables
        ) {
            Map<String, List<String>> sourceColumns = analyzer.analyzeSources(query);
            
            List<CompletableFuture<List<Map<String, Object>>>> futures = new ArrayList<>();
            
            for (Map.Entry<String, List<String>> entry : sourceColumns.entrySet()) {
                String sourceName = entry.getKey();
                DataSource source = dataSources.get(sourceName);
                
                if (source != null) {
                    Map<String, Object> filters = analyzer.rewriteFilters(
                        variables, 
                        source
                    );
                    
                    CompletableFuture<List<Map<String, Object>>> future = source.query(
                        extractTableName(query),
                        filters,
                        entry.getValue()
                    );
                    futures.add(future);
                }
            }
            
            return CompletableFuture.allOf(
                futures.toArray(new CompletableFuture[0])
            ).thenApply(v -> {
                return futures.stream()
                    .map(CompletableFuture::join)
                    .flatMap(List::stream)
                    .collect(Collectors.toList());
            });
        }
        
        private String extractTableName(String query) {
            return query.toLowerCase().contains("customer") ? 
                "customers" : "orders";
        }
    }
    
    public static void main(String[] args) {
        FederationEngine engine = new FederationEngine();
        
        engine.registerDataSource("postgresql", 
            new PostgreSQLDataSource("jdbc:postgresql://localhost:5432/db", 
                                     "user", "pass"));
        engine.registerDataSource("mongodb",
            new MongoDBDataSource("mongodb://localhost:27017", "catalog"));
        engine.registerDataSource("products-api",
            new RESTDataSource("https://api.example.com/v1"));
        
        Map<String, Object> variables = Map.of(
            "region", "US",
            "status", "active"
        );
        
        engine.federateQuery("SELECT * FROM customers c JOIN orders o ON c.id = o.customerId", variables)
            .thenAccept(results -> System.out.println("Federated results: " + results.size()));
    }
}
```

### Python Implementation with GraphQL

```python
from dataclasses import dataclass
from typing import List, Dict, Any, Optional
from abc import ABC, abstractmethod
import asyncio
import aiohttp
import asyncpg
from pymongo import AsyncMongoClient

@dataclass
class QueryPlan:
    source_name: str
    table: str
    filters: Dict[str, Any]
    columns: List[str]
    pushdown_capable: bool

class DataSource(ABC):
    @abstractmethod
    async def query(self, table: str, filters: Dict[str, Any], 
                    columns: List[str]) -> List[Dict[str, Any]]:
        pass
    
    @abstractmethod
    def supports_operation(self, operation: str) -> bool:
        pass

class PostgreSQLSource(DataSource):
    def __init__(self, dsn: str):
        self.dsn = dsn
        self.pool: Optional[asyncpg.Pool] = None
    
    async def connect(self):
        self.pool = await asyncpg.create_pool(self.dsn)
    
    async def disconnect(self):
        if self.pool:
            await self.pool.close()
    
    def supports_operation(self, operation: str) -> bool:
        operations = {"eq", "gt", "lt", "like", "in", "between", "order_by", "limit"}
        return operation in operations
    
    async def query(self, table: str, filters: Dict[str, Any],
                    columns: List[str]) -> List[Dict[str, Any]]:
        if not self.pool:
            await self.connect()
        
        column_list = ", ".join(columns) if columns else "*"
        query = f"SELECT {column_list} FROM {table}"
        
        where_clauses = []
        params = []
        param_num = 1
        
        for key, value in filters.items():
            op = self._extract_operator(key)
            if op in ["eq", "gt", "lt"]:
                where_clauses.append(f"{key} {op} ${param_num}")
                params.append(value)
                param_num += 1
            elif op == "in":
                placeholders = ", ".join([f"${param_num + i}" 
                                         for i in range(len(value))])
                where_clauses.append(f"{key} IN ({placeholders})")
                params.extend(value)
                param_num += len(value)
        
        if where_clauses:
            query += " WHERE " + " AND ".join(where_clauses)
        
        async with self.pool.acquire() as connection:
            results = await connection.fetch(query, *params)
            return [dict(row) for row in results]
    
    def _extract_operator(self, filter_key: str) -> str:
        if "_gt" in filter_key:
            return "gt"
        if "_lt" in filter_key:
            return "lt"
        if "_like" in filter_key:
            return "like"
        if "_in" in filter_key:
            return "in"
        return "eq"

class MongoDBSource(DataSource):
    def __init__(self, connection_string: str, database: str):
        self.connection_string = connection_string
        self.database = database
        self.client: Optional[AsyncMongoClient] = None
    
    async def connect(self):
        self.client = AsyncMongoClient(self.connection_string)
    
    async def disconnect(self):
        if self.client:
            self.client.close()
    
    def supports_operation(self, operation: str) -> bool:
        operations = {"eq", "in", "regex", "limit"}
        return operation in operations
    
    async def query(self, table: str, filters: Dict[str, Any],
                    columns: List[str]) -> List[Dict[str, Any]]:
        if not self.client:
            await self.connect()
        
        db = self.client[self.database]
        collection = db[table]
        
        query_filter = {}
        for key, value in filters.items():
            op = self._extract_operator(key)
            if op == "eq":
                query_filter[key] = value
            elif op == "in":
                query_filter[key] = {"$in": value}
            elif op == "regex":
                query_filter[key] = {"$regex": value}
        
        projection = {col: 1 for col in columns} if columns else None
        
        cursor = collection.find(query_filter, projection)
        results = await cursor.to_list(length=1000)
        return results
    
    def _extract_operator(self, filter_key: str) -> str:
        if "_in" in filter_key:
            return "in"
        if "_regex" in filter_key:
            return "regex"
        return "eq"

class RESTDataSource(DataSource):
    def __init__(self, base_url: str):
        self.base_url = base_url
        self.session: Optional[aiohttp.ClientSession] = None
    
    async def connect(self):
        self.session = aiohttp.ClientSession()
    
    async def disconnect(self):
        if self.session:
            await self.session.close()
    
    def supports_operation(self, operation: str) -> bool:
        return operation in {"eq", "limit"}
    
    async def query(self, table: str, filters: Dict[str, Any],
                    columns: List[str]) -> List[Dict[str, Any]]:
        if not self.session:
            await self.connect()
        
        url = f"{self.base_url}/{table}"
        params = {k: v for k, v in filters.items() if v is not None}
        
        async with self.session.get(url, params=params) as response:
            response.raise_for_status()
            data = await response.json()
            return data if isinstance(data, list) else [data]

class QueryAnalyzer:
    def analyze(self, query: str, variables: Dict[str, Any]) -> List[QueryPlan]:
        plans = []
        
        sources = self._identify_sources(query)
        
        for source_name, table, columns in sources:
            filters = self._extract_filters(source_name, variables)
            pushdown = self._can_pushdown(source_name, filters)
            
            plans.append(QueryPlan(
                source_name=source_name,
                table=table,
                filters=filters,
                columns=columns,
                pushdown_capable=pushdown
            ))
        
        return plans
    
    def _identify_sources(self, query: str) -> List[tuple]:
        sources = []
        query_lower = query.lower()
        
        if "customer" in query_lower:
            sources.append(("postgresql", "customers", ["id", "name", "email"]))
        if "order" in query_lower:
            sources.append(("mongodb", "orders", ["order_id", "customer_id", "total"]))
        if "product" in query_lower:
            sources.append(("rest", "products", ["sku", "name", "price"]))
        
        return sources
    
    def _extract_filters(self, source: str, 
                       variables: Dict[str, Any]) -> Dict[str, Any]:
        filters = {}
        for key, value in variables.items():
            filters[key] = value
        return filters
    
    def _can_pushdown(self, source: str, filters: Dict[str, Any]) -> bool:
        return True

class FederationEngine:
    def __init__(self):
        self.sources: Dict[str, DataSource] = {}
        self.analyzer = QueryAnalyzer()
    
    def register(self, name: str, source: DataSource):
        self.sources[name] = source
    
    async def execute(self, query: str, 
                    variables: Dict[str, Any]) -> List[Dict[str, Any]]:
        plans = self.analyzer.analyze(query, variables)
        
        tasks = []
        for plan in plans:
            source = self.sources.get(plan.source_name)
            if source:
                task = source.query(plan.table, plan.filters, plan.columns)
                tasks.append(task)
        
        results = await asyncio.gather(*tasks)
        
        merged = []
        for result in results:
            merged.extend(result)
        
        return merged

async def main():
    engine = FederationEngine()
    
    engine.register("postgresql", PostgreSQLSource("postgresql://user:pass@localhost/db"))
    engine.register("mongodb", MongoDBSource("mongodb://localhost", "catalog"))
    engine.register("rest", RESTDataSource("https://api.example.com/v1"))
    
    results = await engine.execute(
        "SELECT * FROM customers c JOIN orders o ON c.id = o.customer_id",
        {"region": "US", "status": "active"}
    )
    
    print(f"Federated query returned {len(results)} results")

if __name__ == "__main__":
    asyncio.run(main())
```

## Real-World Examples

### Netflix

Netflix employs data virtualization principles to provide unified access to their content catalog data distributed across multiple systems. Their content metadata resides in different systems: Cassandra stores viewing history, Elasticsearch powers search, and PostgreSQL maintains member entitlements. Rather than duplicating data across systems, Netflix uses a virtualized layer that federates queries to the appropriate source based on the query requirements. This enables their recommendation system to access viewing patterns from Cassandra, content metadata from Elasticsearch, and subscription status from PostgreSQL through a unified query interface. The approach allows Netflix to evolve their underlying storage systems without impacting the numerous applications that depend on this data.

### Spotify

Spotify uses data virtualization to enable their numerous applications to access user data, playlists, and streaming history across their distributed infrastructure. User data spans multiple systems: PostgreSQL stores account information, Cassandra handles playlist configurations, and Redis caches the currently playing track. Instead of maintaining materialized views across all these systems, Spotify's virtualization layer federates queries in real-time, ensuring that applications always access the most current data. This pattern supports their use case where a user's playlists might include millions of tracks while maintaining sub-second query response times. The virtualization approach also enables Spotify to comply with data sovereignty requirements by keeping certain data in specific regions while providing a unified query interface.

## Output Statement

Data virtualization enables transparent access to distributed data sources through a unified query layer without physical data movement. The federation engine analyzes queries, rewrites them based on source capabilities, and pushes down operations to maximize efficiency at the source. This pattern reduces data duplication and synchronization complexity while providing fresh data to consumers. It is particularly valuable when data cannot be consolidated due to sovereignty, regulatory, or technical constraints. Organizations should plan for variable query performance depending on source system latency and implement caching strategies for frequently accessed data. The pattern works best for query-focused workloads where fresh data is essential and query patterns are predictable enough to optimize.

## Best Practices

1. **Implement Query Pushdown**: Push as much processing as possible to source systems. Analyze source capabilities and rewrite queries to leverage source-specific optimizations. Only pull data that's absolutely necessary for processing in the federation layer.

2. **Use Metadata Catalogs**: Maintain comprehensive metadata about available sources, their schemas, and capabilities. Catalog information enables the federation engine to make optimal routing decisions and helps developers understand available data.

3. **Implement Caching Strategically**: Cache frequently accessed data at the federation layer to reduce source load and improve response times. Use appropriate caching strategies based on data freshness requirements, implementing time-based or invalidation-based cache expiry.

4. **Monitor Query Performance**: Track query latency and source load to identify optimization opportunities. Set performance baselines and alert on degradation. Analyze slow queries to identify opportunities for caching or source-side optimization.

5. **Handle Source Failures Gracefully**: Implement circuit breakers to prevent cascade failures when sources become unavailable. Provide appropriate error messages and fallback strategies for applications when sources are unavailable.

6. **Document Data Lineage**: Track which data comes from which sources to support data governance and compliance. Document data ownership, quality metrics, and update frequencies for consumers.

7. **Optimize Join Operations**: Co-locate frequently joined data sources or optimize join execution across sources. Consider denormalization for frequently joined entities to reduce cross-source query overhead.

8. **Plan for Scale**: Design federation architecture to handle query volume growth. Partition queries across multiple federation instances when necessary. Consider read replicas for frequently accessed sources.