/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 08_Scalability_Patterns
 * Topic: 03_Sharding_Patterns
 * Purpose: Deep dive into Sharding Patterns with TypeScript examples
 * Difficulty: advanced
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Enables horizontal scaling
 * Security: Consider data isolation
 */

/**
 * Sharding Patterns - Comprehensive Guide
 * =======================================
 * 
 * WHAT: A technique to split large datasets into smaller, more manageable
 * pieces called shards, which can be stored on different servers.
 * 
 * WHY:
 * - Scale horizontally
 * - Reduce load on single server
 * - Improve query performance
 * - Enable geographic distribution
 * 
 * HOW:
 * - Choose shard key
 * - Implement shard routing
 * - Handle cross-shard queries
 * - Rebalance when needed
 */

// ============================================================================
// SECTION 1: SHARD ROUTING
// ============================================================================

interface Shard {
  id: string;
  contains(key: string): boolean;
}

interface ShardingStrategy {
  getShard(key: string): Shard;
}

class HashShardingStrategy implements ShardingStrategy {
  constructor(private shards: Shard[]) {}
  
  getShard(key: string): Shard {
    const hash = this.hashKey(key);
    const index = hash % this.shards.length;
    return this.shards[index];
  }
  
  private hashKey(key: string): number {
    let hash = 0;
    for (let i = 0; i < key.length; i++) {
      hash = ((hash << 5) - hash) + key.charCodeAt(i);
      hash = hash & hash;
    }
    return Math.abs(hash);
  }
}

class RangeShardingStrategy implements ShardingStrategy {
  constructor(private shards: Shard[]) {}
  
  getShard(key: string): Shard {
    for (const shard of this.shards) {
      if (shard.contains(key)) {
        return shard;
      }
    }
    return this.shards[0];
  }
}

// ============================================================================
// SECTION 2: SHARDED REPOSITORY
// ============================================================================

interface ShardedRepository<T> {
  save(key: string, entity: T): Promise<void>;
  findById(key: string, id: string): Promise<T | null>;
  findByShardKey(shardKey: string): Promise<T[]>;
}

class UserShardedRepository implements ShardedRepository<User> {
  private shards: Map<string, Map<string, User>> = new Map();
  private strategy: ShardingStrategy;
  
  constructor(strategy: ShardingStrategy) {
    this.strategy = strategy;
  }
  
  async save(key: string, entity: User): Promise<void> {
    const shard = this.strategy.getShard(key);
    
    if (!this.shards.has(shard.id)) {
      this.shards.set(shard.id, new Map());
    }
    
    this.shards.get(shard.id)!.set(entity.id, entity);
  }
  
  async findById(key: string, id: string): Promise<User | null> {
    const shard = this.strategy.getShard(key);
    return this.shards.get(shard.id)?.get(id) || null;
  }
  
  async findByShardKey(shardKey: string): Promise<User[]> {
    const shard = this.strategy.getShard(shardKey);
    return Array.from(this.shards.get(shard.id)?.values() || []);
  }
}

interface User {
  id: string;
  name: string;
  email: string;
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Sharding Performance:
// - Reduces query load per shard
// - Cross-shard queries expensive
// - Rebalancing costly

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security considerations:
// - Isolate shards properly
// - Consider data residency

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing sharding:
// 1. Test shard routing
// 2. Test cross-shard queries
// 3. Test rebalancing

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Load Balancing (01_Load_Balancing.ts)
// - Partitioning
// - Replication

// Next: 04_CDN_Integration.ts
