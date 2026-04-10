/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 08_Monitoring_Observability Topic: 10_Health_Checks Purpose: Health check endpoints and monitoring Difficulty: intermediate UseCase: web, backend, devops Version: TS 5.0+ Compatibility: Node.js 16+ Performance: fast Security: authentication */

declare namespace HealthChecks {
  interface HealthStatus {
    status: 'healthy' | 'degraded' | 'unhealthy';
    version: string;
    uptime: number;
    timestamp: number;
    services: ServiceHealth[];
    checks?: Check[];
  }

  interface ServiceHealth {
    name: string;
    status: 'healthy' | 'unhealthy';
    latency?: number;
    message?: string;
    timestamp: number;
  }

  interface Check {
    name: string;
    status: 'pass' | 'fail';
    output?: string;
    duration?: number;
    timestamp: number;
  }

  interface HealthCheckOptions {
    database?: DatabaseHealthCheckOptions;
    cache?: CacheHealthCheckOptions;
    external?: ExternalServiceCheck;
    custom?: CustomCheck[];
  }

  interface DatabaseHealthCheckOptions {
    type: 'postgres' | 'mysql' | 'mongodb';
    connectionString: string;
    timeout?: number;
  }

  interface CacheHealthCheckOptions {
    type: 'redis' | 'memcached';
    connectionString: string;
    timeout?: number;
  }

  interface ExternalServiceCheck {
    url: string;
    timeout?: number;
    headers?: Record<string, string>;
  }

  interface CustomCheck {
    name: string;
    check: () => Promise<CheckResult> | CheckResult;
  }

  interface CheckResult {
    status: 'pass' | 'fail';
    output?: string;
    duration?: number;
  }

  interface HealthMiddleware {
    (req: Request, res: Response, next: Function): void;
  }

  interface Request {
    method: string;
    path: string;
    headers: Record<string, string>;
    query: Record<string, string>;
  }

  interface Response {
    status(statusCode: number): this;
    json(body: unknown): this;
    send(body: string): this;
  }

  interface LivenessResponse {
    status: string;
    timestamp: number;
  }

  interface ReadinessResponse extends HealthStatus {}
}

import { Request, Response } from 'express';
import { Pool } from 'pg';
import Redis from 'ioredis';

const pool = new Pool({ connectionString: process.env.DATABASE_URL });
const redis = new Redis(process.env.REDIS_URL);

async function performDatabaseCheck(): Promise<HealthChecks.CheckResult> {
  const start = Date.now();
  try {
    await pool.query('SELECT 1');
    return { status: 'pass', duration: Date.now() - start };
  } catch (error) {
    return { status: 'fail', output: String(error), duration: Date.now() - start };
  }
}

async function performRedisCheck(): Promise<HealthChecks.CheckResult> {
  const start = Date.now();
  try {
    await redis.ping();
    return { status: 'pass', duration: Date.now() - start };
  } catch (error) {
    return { status: 'fail', output: String(error), duration: Date.now() - start };
  }
}

app.get('/health/live', (req: Request, res: Response) => {
  res.json({ status: 'ok', timestamp: Date.now() });
});

app.get('/health/ready', async (req: Request, res: Response) => {
  const [db, redis] = await Promise.all([performDatabaseCheck(), performRedisCheck()]);
  
  const services = [
    { name: 'database', status: db.status === 'pass' ? 'healthy' : 'unhealthy', latency: db.duration, timestamp: Date.now() },
    { name: 'redis', status: redis.status === 'pass' ? 'healthy' : 'unhealthy', latency: redis.duration, timestamp: Date.now() },
  ];

  const allHealthy = services.every(s => s.status === 'healthy');
  
  res.status(allHealthy ? 200 : 503).json({
    status: allHealthy ? 'healthy' : 'unhealthy',
    version: process.env.APP_VERSION || '1.0.0',
    uptime: process.uptime(),
    timestamp: Date.now(),
    services,
  });
});

describe('Health Checks', () => {
  describe('Database Check', () => {
    it('should check database', async () => {
      const result = await performDatabaseCheck();
      expect(result.status).toBeDefined();
    });
  });

  describe('Redis Check', () => {
    it('should check Redis', async () => {
      const result = await performRedisCheck();
      expect(result.status).toBeDefined();
    });
  });
});

console.log('\n=== Health Checks Complete ===');
console.log('Next: 11_Alerting_Types.ts');