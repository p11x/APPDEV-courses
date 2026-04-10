/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 07_Security_Patterns
 * Topic: 04_Authorization_Patterns
 * Purpose: Deep dive into Authorization Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Depends on policy complexity
 * Security: Critical - enforces access control
 */

/**
 * Authorization Patterns - Comprehensive Guide
 * ============================================
 * 
 * WHAT: Patterns for controlling access to resources based on user identity,
 * roles, and permissions.
 * 
 * WHY:
 * - Enforce access control
 * - Role-based permissions
 * - Audit access attempts
 * - Defense in depth
 * 
 * HOW:
 * - Define roles and permissions
 * - Check authorization before actions
 * - Use middleware for enforcement
 * - Implement RBAC or ABAC
 */

// ============================================================================
// SECTION 1: ROLE-BASED ACCESS CONTROL
// ============================================================================

interface Permission {
  resource: string;
  action: string;
}

interface Role {
  name: string;
  permissions: Permission[];
}

interface User {
  id: string;
  roles: string[];
}

class RBACService {
  private roles: Map<string, Role> = new Map();
  private userRoles: Map<string, string[]> = new Map();
  
  registerRole(role: Role): void {
    this.roles.set(role.name, role);
  }
  
  assignRole(userId: string, roleName: string): void {
    const userRoles = this.userRoles.get(userId) || [];
    userRoles.push(roleName);
    this.userRoles.set(userId, userRoles);
  }
  
  hasPermission(userId: string, resource: string, action: string): boolean {
    const userRoleNames = this.userRoles.get(userId) || [];
    
    for (const roleName of userRoleNames) {
      const role = this.roles.get(roleName);
      
      if (role) {
        const hasPermission = role.permissions.some(
          p => p.resource === resource && p.action === action
        );
        
        if (hasPermission) return true;
      }
    }
    
    return false;
  }
}

// ============================================================================
// SECTION 2: POLICY-BASED ACCESS CONTROL
// ============================================================================

interface Policy {
  id: string;
  effect: "allow" | "deny";
  principal: string;
  resource: string;
  action: string;
  condition?: (context: any) => boolean;
}

class PolicyEngine {
  private policies: Policy[] = [];
  
  addPolicy(policy: Policy): void {
    this.policies.push(policy);
  }
  
  evaluate(principal: string, resource: string, action: string, context: any = {}): boolean {
    const matchingPolicies = this.policies.filter(p =>
      p.principal === principal &&
      p.resource === resource &&
      p.action === action
    );
    
    if (matchingPolicies.length === 0) {
      return false;
    }
    
    const denyPolicies = matchingPolicies.filter(p => p.effect === "deny");
    const allowPolicies = matchingPolicies.filter(p => p.effect === "allow");
    
    for (const policy of denyPolicies) {
      if (!policy.condition || policy.condition(context)) {
        return false;
      }
    }
    
    for (const policy of allowPolicies) {
      if (!policy.condition || policy.condition(context)) {
        return true;
      }
    }
    
    return false;
  }
}

// ============================================================================
// SECTION 3: ACCESS CONTROL MIDDLEWARE
// ============================================================================

interface AuthContext {
  userId: string;
  roles: string[];
}

type Middleware = (
  context: AuthContext,
  resource: string,
  action: string
) => Promise<boolean>;

class AccessControlMiddleware {
  constructor(
    private rbac: RBACService,
    private logger: any
  ) {}
  
  createMiddleware(resource: string, action: string): Middleware {
    return async (context: AuthContext): Promise<boolean> => {
      const hasPermission = this.rbac.hasPermission(
        context.userId,
        resource,
        action
      );
      
      if (!hasPermission) {
        this.logger.warn(`Access denied: ${context.userId} on ${resource}.${action}`);
      }
      
      return hasPermission;
    };
  }
}

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Authorization Performance:
// - Policy evaluation fast
// - Caching permissions helpful
// - Avoid complex conditions

// ============================================================================
// COMPATIBILITY
// ============================================================================

// Compatible with:
// - TypeScript 1.6+
// - All ES targets
// - Node.js and browsers

// ============================================================================
// SECURITY CONSIDERATIONS
// ============================================================================

// Security critical:
// - Default deny
// - Audit access
// - Don't expose sensitive data

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing authorization:
// 1. Test role permissions
// 2. Test policy evaluation
// 3. Test middleware

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Authentication (03_Authentication_Patterns.ts)
// - Input Validation (01_Input_Validation.ts)
// - Encryption (05_Encryption_Types.ts)

// Next: 05_Encryption_Types.ts
