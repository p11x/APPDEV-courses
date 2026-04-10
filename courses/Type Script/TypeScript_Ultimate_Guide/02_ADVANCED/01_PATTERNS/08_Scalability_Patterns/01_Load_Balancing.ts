/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 08_Scalability_Patterns
 * Topic: 01_Load_Balancing
 * Purpose: Deep dive into Load Balancing Patterns with TypeScript examples
 * Difficulty: advanced
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Critical for high traffic
 * Security: N/A
 */

/**
 * Load Balancing Patterns - Comprehensive Guide
 * =============================================
 * 
 * WHAT: Patterns for distributing incoming network traffic across multiple
 * servers to ensure no single server is overwhelmed.
 * 
 * WHY:
 * - Handle high traffic
 * - Improve availability
 * - Scale horizontally
 * - Failover support
 * 
 * HOW:
 * - Use round-robin for simple distribution
 * - Use least connections for varying loads
 * - Use IP hash for session persistence
 * - Implement health checks
 */

// ============================================================================
// SECTION 1: LOAD BALANCER INTERFACE
// ============================================================================

interface Server {
  id: string;
  host: string;
  port: number;
  healthy: boolean;
  activeConnections: number;
}

interface LoadBalancer {
  selectServer(): Server | null;
  markHealthy(serverId: string): void;
  markUnhealthy(serverId: string): void;
}

// ============================================================================
// SECTION 2: ROUND-ROBIN LOAD BALANCER
// ============================================================================

class RoundRobinLoadBalancer implements LoadBalancer {
  private servers: Server[] = [];
  private currentIndex = 0;
  
  addServer(server: Server): void {
    this.servers.push(server);
  }
  
  selectServer(): Server | null {
    const healthyServers = this.servers.filter(s => s.healthy);
    
    if (healthyServers.length === 0) {
      return null;
    }
    
    const server = healthyServers[this.currentIndex % healthyServers.length];
    this.currentIndex++;
    
    return server;
  }
  
  markHealthy(serverId: string): void {
    const server = this.servers.find(s => s.id === serverId);
    if (server) server.healthy = true;
  }
  
  markUnhealthy(serverId: string): void {
    const server = this.servers.find(s => s.id === serverId);
    if (server) server.healthy = false;
  }
}

// ============================================================================
// SECTION 3: LEAST CONNECTIONS
// ============================================================================

class LeastConnectionsLoadBalancer implements LoadBalancer {
  private servers: Server[] = [];
  
  addServer(server: Server): void {
    this.servers.push(server);
  }
  
  selectServer(): Server | null {
    const healthyServers = this.servers.filter(s => s.healthy);
    
    if (healthyServers.length === 0) {
      return null;
    }
    
    return healthyServers.reduce((min, server) =>
      server.activeConnections < min.activeConnections ? server : min
    );
  }
  
  markHealthy(serverId: string): void {
    const server = this.servers.find(s => s.id === serverId);
    if (server) server.healthy = true;
  }
  
  markUnhealthy(serverId: string): void {
    const server = this.servers.find(s => s.id === serverId);
    if (server) server.healthy = false;
  }
}

// ============================================================================
// SECTION 4: HEALTH CHECKS
// ============================================================================

interface HealthCheck {
  check(server: Server): Promise<boolean>;
}

class HTTPHealthCheck implements HealthCheck {
  constructor(private endpoint: string) {}
  
  async check(server: Server): Promise<boolean> {
    try {
      const response = await fetch(`http://${server.host}:${server.port}${this.endpoint}`, {
        method: "HEAD"
      });
      return response.ok;
    } catch {
      return false;
    }
  }
}

class HealthCheckMonitor {
  private intervalId: NodeJS.Timeout | null = null;
  
  constructor(
    private loadBalancer: LoadBalancer,
    private healthCheck: HealthCheck,
    private servers: Server[],
    private interval: number = 10000
  ) {}
  
  start(): void {
    this.intervalId = setInterval(async () => {
      for (const server of this.servers) {
        const healthy = await this.healthCheck.check(server);
        
        if (healthy !== server.healthy) {
          if (healthy) {
            this.loadBalancer.markHealthy(server.id);
          } else {
            this.loadBalancer.markUnhealthy(server.id);
          }
        }
      }
    }, this.interval);
  }
  
  stop(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Load Balancing Performance:
// - Minimal overhead
// - Health checks add load
// - Consider sticky sessions

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
// - Secure health check endpoints
// - Consider DDoS protection

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing load balancing:
// 1. Test server selection
// 2. Test health checks
// 3. Test failover

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Circuit Breaker (02_Circuit_Breaker.ts)
// - Sharding (03_Sharding_Patterns.ts)
// - CDN Integration (04_CDN_Integration.ts)

// Next: 02_Circuit_Breaker.ts
