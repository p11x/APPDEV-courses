/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 08_Scalability_Patterns
 * Topic: 04_CDN_Integration
 * Purpose: Deep dive into CDN Integration Patterns with TypeScript examples
 * Difficulty: advanced
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Reduces latency significantly
 * Security: Consider cache poisoning
 */

/**
 * CDN Integration - Comprehensive Guide
 * ====================================
 * 
 * WHAT: Patterns for integrating Content Delivery Networks to serve static
 * assets from edge locations closer to users.
 * 
 * WHY:
 * - Reduce latency
 * - Offload origin server
 * - Improve global availability
 * - Handle traffic spikes
 * 
 * HOW:
 * - Configure CDN for static assets
 * - Set appropriate cache headers
 * - Handle cache invalidation
 * - Use signed URLs for private content
 */

// ============================================================================
// SECTION 1: CDN URL GENERATOR
// ============================================================================

interface CDNConfig {
  baseUrl: string;
  domain: string;
}

class CDNUrlGenerator {
  constructor(private config: CDNConfig) {}
  
  getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.slice(1);
    }
    
    return `${this.config.baseUrl}/${path}`;
  }
  
  getSignedUrl(path: string, expires: Date, secret: string): string {
    const expiry = Math.floor(expires.getTime() / 1000);
    const signature = this.generateSignature(path, expiry, secret);
    
    return `${this.getUrl(path)}?expires=${expiry}&signature=${signature}`;
  }
  
  private generateSignature(path: string, expiry: number, secret: string): string {
    const data = `${path}${expiry}`;
    return btoa(data + secret);
  }
}

// ============================================================================
// SECTION 2: CACHE HEADER MANAGEMENT
// ============================================================================

interface CacheConfig {
  maxAge: number;
  sMaxAge?: number;
  immutable?: boolean;
  private?: boolean;
}

const cacheHeaders = {
  noCache: {
    "Cache-Control": "no-cache, no-store, must-revalidate",
    "Pragma": "no-cache",
    "Expires": "0"
  },
  shortCache: {
    "Cache-Control": "public, max-age=60, s-maxage=60"
  },
  longCache: {
    "Cache-Control": "public, max-age=31536000, immutable"
  }
};

function applyCacheHeaders(response: Response, config: CacheConfig): void {
  const directives = ["public"];
  
  if (config.private) {
    directives[0] = "private";
  }
  
  if (config.maxAge !== undefined) {
    directives.push(`max-age=${config.maxAge}`);
  }
  
  if (config.sMaxAge !== undefined) {
    directives.push(`s-maxage=${config.sMaxAge}`);
  }
  
  if (config.immutable) {
    directives.push("immutable");
  }
  
  response.headers.set("Cache-Control", directives.join(", "));
}

// ============================================================================
// SECTION 3: STATIC ASSET HANDLER
// ============================================================================

interface StaticAssetHandler {
  handle(path: string): Promise<Response>;
}

class CDNStaticAssetHandler implements StaticAssetHandler {
  constructor(
    private cdn: CDNUrlGenerator,
    private originPath: string
  ) {}
  
  async handle(path: string): Promise<Response> {
    if (this.isStaticAsset(path)) {
      const cdnUrl = this.cdn.getUrl(path);
      return Response.redirect(cdnUrl, 302);
    }
    
    return new Response("Not found", { status: 404 });
  }
  
  private isStaticAsset(path: string): boolean {
    const staticExtensions = [".js", ".css", ".png", ".jpg", ".gif", ".svg", ".woff", ".woff2"];
    return staticExtensions.some(ext => path.endsWith(ext));
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// CDN Performance:
// - Reduces latency significantly
// - Offloads origin
// - Handles traffic spikes

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Browsers

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security considerations:
// - Use signed URLs for private content
// - Validate CDN responses
// - Protect origin

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing CDN integration:
// 1. Test URL generation
// 2. Test signed URLs
// 3. Test cache headers

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Load Balancing (01_Load_Balancing.ts)
// - Cache Patterns (06_Performance_Patterns/04_Cache_Patterns.ts)
// - Lazy Loading (06_Performance_Patterns/01_Lazy_Loading_Patterns.ts)

// End of Scalability Patterns
