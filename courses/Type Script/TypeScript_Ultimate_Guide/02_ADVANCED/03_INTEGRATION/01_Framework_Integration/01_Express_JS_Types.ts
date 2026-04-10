/**
 * Category: ADVANCED
 * Subcategory: INTEGRATION
 * Concept: Framework_Integration
 * Purpose: Express.js type integration
 * Difficulty: intermediate
 * UseCase: backend
 */

/**
 * Express Integration - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Using TypeScript with Express.js
 * 💡 WHERE: Building type-safe REST APIs
 * 🔧 HOW: Request/Response types, middleware types
 */

// ============================================================================
// SECTION 1: REQUEST/RESPONSE TYPES
// ============================================================================

// Example 1.1: Express Request Type Extension
// ---------------------------------

import express, { Request, Response, NextFunction } from "express";

interface AuthRequest extends Request {
  user?: {
    id: string;
    name: string;
  };
}

interface TypedRequest<T = unknown> extends Request {
  body: T;
}

interface TypedResponse<T> extends Response {
  json(body: T): this;
}

// Example 1.2: Response Type
// -----------------------

function sendSuccess<T>(res: Response, data: T): void {
  res.json({
    success: true,
    data
  });
}

function sendError(res: Response, message: string, status = 400): void {
  res.status(status).json({
    success: false,
    error: message
  });
}

// ============================================================================
// SECTION 2: MIDDLEWARE TYPES
// ============================================================================

// Example 2.1: Authentication Middleware
// ---------------------------------

type AsyncRequestHandler = (
  req: AuthRequest,
  res: Response,
  next: NextFunction
) => Promise<void>;

function authMiddleware: AsyncRequestHandler = async (req, res, next) => {
  try {
    // Verify token
    const token = req.headers.authorization;
    if (!token) {
      return res.status(401).json({ error: "Unauthorized" });
    }
    
    // Attach user to request
    req.user = { id: "1", name: "John" };
    next();
  } catch (error) {
    next(error);
  }
};

// Example 2.2: Error Handler Type
// ---------------------------------

interface ErrorWithStatus extends Error {
  status?: number;
}

function errorHandler(
  err: ErrorWithStatus,
  req: Request,
  res: Response,
  next: NextFunction
): void {
  console.error(err.stack);
  res.status(err.status || 500).json({
    error: err.message
  });
}

// ============================================================================
// SECTION 3: ROUTING TYPES
// ============================================================================

// Example 3.1: Generic Route Handler
// ---------------------------------

type RouteHandler<T, U> = (req: TypedRequest<T>, res: TypedResponse<U>) => void;

interface CreateUserRequest {
  name: string;
  email: string;
}

interface UserResponse {
  id: string;
  name: string;
}

const createUserHandler: RouteHandler<CreateUserRequest, UserResponse> = (req, res) => {
  const user = {
    id: "1",
    name: req.body.name
  };
  res.json(user);
};

// ============================================================================
// SECTION 4: ROUTER TYPES
// ============================================================================

// Example 4.1: Typed Router
// ---------------------------------

const router = express.Router();

interface TypedRouter {
  get<T>(path: string, handler: RouteHandler<unknown, T>): this;
  post<T>(path: string, handler: RouteHandler<unknown, T>): this;
  put<T>(path: string, handler: RouteHandler<unknown, T>): this;
  delete(path: string, handler: RouteHandler<unknown, unknown>): this;
}

console.log("\n=== Express Integration Complete ===");
console.log("Next: ADVANCED/INTEGRATION/02_Backend_Development/01_Database_ORM_Types.ts");