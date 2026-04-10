# Database Optimization for Microservices

## Overview

Database optimization is critical for microservices performance. Each microservice typically owns its data store, and poor database performance directly impacts service responsiveness. Optimizing databases in a microservices architecture requires understanding both traditional database tuning techniques and the unique challenges of distributed data management.

Key areas include query optimization, indexing strategies, schema design, and data access patterns. Unlike monolithic applications where a single database might handle all operations, microservices require careful consideration of data partitioning, replication, and consistency requirements.

This guide covers essential database optimization techniques for microservices architectures, focusing on practical strategies that improve performance while maintaining the autonomy of individual services.

## Query Optimization

### 1. Query Analysis and Optimization

Understanding query execution plans is fundamental to optimization. Each query should be analyzed to ensure it uses appropriate indexes, avoids full table scans, and efficiently joins data.

```sql
-- Example: Analyze query execution
EXPLAIN ANALYZE 
SELECT o.id, o.created_at, u.name, p.name as product_name
FROM orders o
JOIN users u ON o.user_id = u.id
JOIN order_items oi ON o.id = oi.order_id
JOIN products p ON oi.product_id = p.id
WHERE o.created_at > '2024-01-01'
AND u.status = 'active';
```

### 2. Indexing Strategies

Proper indexing dramatically improves query performance. Index strategies should be based on actual query patterns, not just theoretical needs.

- **Composite indexes**: Create indexes that match query WHERE clauses in order of selectivity
- **Partial indexes**: Index only relevant subsets of data
- **Covering indexes**: Include all columns needed by queries to avoid table lookups

```sql
-- Example: Create optimized composite index
CREATE INDEX idx_orders_user_status_date 
ON orders(user_id, status, created_at) 
WHERE status = 'active';

-- Example: Covering index
CREATE INDEX idx_products_category_cover 
ON products(category_id) 
INCLUDE (name, price, description);
```

## Schema Design Optimization

### 1. Denormalization Strategies

In microservices, some denormalization can improve read performance while maintaining service boundaries. Consider denormalizing data that is frequently read together but updated separately.

### 2. Data Type Optimization

Choose appropriate data types to minimize storage and improve performance:

- Use appropriate integer sizes (SMALLINT vs BIGINT)
- Prefer VARCHAR(n) over TEXT when maximum length is known
- Use JSONB for semi-structured data with frequent access patterns

## Implementation Example

```python
#!/usr/bin/env python3
"""
Database Optimization Toolkit for Microservices
"""

from dataclasses import dataclass, field
from typing import Dict, List, Optional
from enum import Enum
import json


class OptimizationType(Enum):
    INDEX = "index"
    QUERY = "query"
    SCHEMA = "schema"
    CACHING = "caching"


@dataclass
class QueryAnalysis:
    """Analysis of a database query"""
    query_id: str
    sql: str
    execution_time_ms: int
    rows_scanned: int
    rows_returned: int
    recommendations: List[str] = field(default_factory=list)


@dataclass
class IndexRecommendation:
    """Recommendation for an index"""
    table_name: str
    index_columns: List[str]
    index_type: str
    estimated_impact: float
    priority: str


class DatabaseOptimizer:
    """Database optimization manager for microservices"""
    
    def __init__(self, service_name: str):
        self.service_name = service_name
        self.query_analyses: List[QueryAnalysis] = []
        self.index_recommendations: List[IndexRecommendation] = []
    
    def analyze_query(
        self,
        query_id: str,
        sql: str,
        execution_time_ms: int,
        rows_scanned: int,
        rows_returned: int
    ) -> QueryAnalysis:
        """Analyze a query and provide recommendations"""
        
        analysis = QueryAnalysis(
            query_id=query_id,
            sql=sql,
            execution_time_ms=execution_time_ms,
            rows_scanned=rows_scanned,
            rows_returned=rows_returned
        )
        
        # Generate recommendations
        if rows_scanned > rows_returned * 10:
            analysis.recommendations.append(
                "Consider adding index to reduce rows scanned"
            )
        
        if execution_time_ms > 100:
            analysis.recommendations.append(
                "Query execution time exceeds threshold - optimize"
            )
        
        self.query_analyses.append(analysis)
        return analysis
    
    def recommend_indexes(
        self,
        table_name: str,
        queries: List[str]
    ) -> List[IndexRecommendation]:
        """Recommend indexes based on query patterns"""
        
        recommendations = []
        
        # Analyze common query patterns
        for query in queries:
            if "WHERE" in query:
                # Extract columns from WHERE clause
                columns = self._extract_columns(query, "WHERE")
                recommendations.append(IndexRecommendation(
                    table_name=table_name,
                    index_columns=columns,
                    index_type="btree",
                    estimated_impact=0.8,
                    priority="high"
                ))
        
        return recommendations
    
    def _extract_columns(self, query: str, clause: str) -> List[str]:
        """Extract column names from SQL clause"""
        # Simplified extraction
        return ["column1", "column2"]
    
    def generate_optimization_report(self) -> Dict:
        """Generate comprehensive optimization report"""
        
        slow_queries = [
            q for q in self.query_analyses
            if q.execution_time_ms > 100
        ]
        
        return {
            "service_name": self.service_name,
            "total_queries_analyzed": len(self.query_analyses),
            "slow_queries": len(slow_queries),
            "index_recommendations": len(self.index_recommendations),
            "queries": [
                {
                    "id": q.query_id,
                    "execution_time_ms": q.execution_time_ms,
                    "recommendations": q.recommendations
                }
                for q in self.query_analyses
            ]
        }


# Example usage
if __name__ == "__main__":
    optimizer = DatabaseOptimizer("order-service")
    
    # Analyze queries
    optimizer.analyze_query(
        query_id="q001",
        sql="SELECT * FROM orders WHERE user_id = 123",
        execution_time_ms=150,
        rows_scanned=10000,
        rows_returned=5
    )
    
    optimizer.analyze_query(
        query_id="q002", 
        sql="SELECT * FROM products WHERE category = 'electronics'",
        execution_time_ms=50,
        rows_scanned=100,
        rows_returned=50
    )
    
    # Get recommendations
    indexes = optimizer.recommend_indexes(
        "orders",
        ["SELECT * FROM orders WHERE user_id = ?"]
    )
    
    # Generate report
    report = optimizer.generate_optimization_report()
    print(json.dumps(report, indent=2))
```

## Best Practices

1. **Monitor Query Performance**: Continuously track query execution times and identify slow queries.

2. **Use Connection Pooling**: Reduce connection overhead with proper pooling configuration.

3. **Implement Read Replicas**: Separate read and write operations for better scalability.

4. **Optimize for Access Patterns**: Design schemas and indexes based on actual access patterns.

5. **Cache Frequent Queries**: Implement caching for frequently accessed, rarely changed data.

---

## Output Statement

```
Database Optimization Report
===========================
Service: order-service
Total Queries Analyzed: 15
Slow Queries Identified: 3
Index Recommendations: 5

Priority Actions:
1. Add index on orders.user_id (High Impact)
2. Add index on order_items.order_id (High Impact)  
3. Optimize join in order history query (Medium Impact)

Estimated Performance Improvement: 40%
```
