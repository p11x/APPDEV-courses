/**
 * Category: 07_ADVANCED_TOPICS
 * Subcategory: 04_Type_Libraries
 * Concept: 01_Third_Party_Types
 * Topic: 01_Community_Types
 * Purpose: Learn about community-maintained type definitions
 * Difficulty: beginner
 * UseCase: library-integration
 * Version: TS 4.0+
 * Compatibility: All TypeScript targets
 * Performance: N/A
 * Security: Verify type package sources
 */

/**
 * WHAT: The TypeScript community maintains type definitions for popular
 * JavaScript libraries through @types packages on npm.
 */

declare module "lodash" {
  function chunk<T>(array: T[], size?: number): T[][];
  function cloneDeep<T>(value: T): T;
  function debounce<T extends (...args: any[]) => any>(func: T, wait?: number, options?: { leading?: boolean; trailing?: boolean }): T;
  function throttle<T extends (...args: any[]) => any>(func: T, wait?: number): T;
  function pick<T, K extends keyof T>(obj: T, keys: K[]): Pick<T, K>;
  function omit<T, K extends keyof T>(obj: T, keys: K[]): Omit<T, K>;
  function merge<T>(object: T, sources: Partial<T>[]): T;
  function unique<T>(array: T[]): T[];
  function groupBy<T>(collection: T[], iteratee: (item: T) => string): Record<string, T[]>;
}

declare module "express" {
  interface Request {
    body: any;
    params: Record<string, string>;
    query: Record<string, any>;
    headers: Record<string, string>;
  }
  
  interface Response {
    json(body: any): Response;
    status(code: number): Response;
    send(body: string | object): Response;
  }
  
  interface Router {
    get(path: string, handler: (req: Request, res: Response) => void): Router;
    post(path: string, handler: (req: Request, res: Response) => void): Router;
  }
  
  function express(): Router;
  export = express;
}

declare module "react" {
  function useState<T>(initial: T): [T, (value: T) => void];
  function useEffect(effect: () => void | (() => void), deps?: any[]): void;
  function useCallback<T extends (...args: any[]) => any>(callback: T, deps: any[]): T;
  function useMemo<T>(factory: () => T, deps: any[]): T;
}

declare module "axios" {
  interface AxiosRequestConfig {
    url?: string;
    method?: string;
    headers?: Record<string, string>;
    params?: Record<string, any>;
  }
  
  interface AxiosResponse<T = any> {
    data: T;
    status: number;
    statusText: string;
  }
  
  function request<T>(config: AxiosRequestConfig): Promise<AxiosResponse<T>>;
  export = { request };
}

type AsyncReturnType<T extends (...args: any[]) => Promise<any>> = T extends (...args: any[]) => Promise<infer R> ? R : never;

console.log("\n=== Community Types Demo ===");
console.log("Type definitions loaded from @types packages");

/**
 * COMPATIBILITY:
 * - Install via npm install -D @types/* 
 * - Use skipLibCheck to skip declaration errors
 * 
 * SECURITY:
 * - Only use trusted type packages
 * - Verify package maintainers
 * 
 * CROSS-REFERENCE:
 * - 02_Typed_Arrays.ts - Typed array implementations
 * - 02_Type_Creation/01_Factory_Functions.ts - Type factories
 */