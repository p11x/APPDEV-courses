/**
 * Category: 02_ADVANCED
 * Subcategory: 01_PATTERNS
 * Concept: 07_Security_Patterns
 * Topic: 02_Sanitization_Patterns
 * Purpose: Deep dive into Sanitization Patterns with TypeScript examples
 * Difficulty: intermediate
 * UseCase: web, backend, enterprise
 * Version: TS 5.0+
 * Compatibility: Browsers, Node.js
 * Performance: Depends on content size
 * Security: Critical - prevents XSS, injection
 */

/**
 * Sanitization Patterns - Comprehensive Guide
 * ==========================================
 * 
 * WHAT: Patterns for cleaning and neutralizing potentially dangerous content
 * before it's stored or displayed.
 * 
 * WHY:
 * - Prevent XSS attacks
 * - Prevent injection attacks
 * - Safe data handling
 * - Output encoding
 * 
 * HOW:
 * - Encode output context
 * - Strip dangerous elements
 * - Validate URLs
 * - Handle special characters
 */

// ============================================================================
// SECTION 1: OUTPUT ENCODING
// ============================================================================

const encodeHtml = (input: string): string => {
  const entities: Record<string, string> = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': "&quot;",
    "'": "&#x27;",
    "/": "&#x2F;"
  };
  
  return input.replace(/[&<>"'/]/g, c => entities[c] || c);
};

const encodeAttribute = (input: string): string => {
  return input.replace(/["&<>`]/g, c => {
    const entities: Record<string, string> = {
      '"': "&quot;",
      "&": "&amp;",
      "<": "&lt;",
      ">": "&gt;",
      "`": "&#96;"
    };
    return entities[c] || c;
  });
};

const encodeJavaScript = (input: string): string => {
  return JSON.stringify(input).slice(1, -1);
};

// ============================================================================
// SECTION 2: URL SANITIZATION
// ============================================================================

const sanitizeUrl = (url: string): string | null => {
  try {
    const parsed = new URL(url);
    
    if (!["http:", "https:"].includes(parsed.protocol)) {
      return null;
    }
    
    return parsed.href;
  } catch {
    return null;
  }
};

const sanitizeRedirect = (url: string, allowedHosts: string[]): string | null => {
  try {
    const parsed = new URL(url);
    
    if (!allowedHosts.includes(parsed.host)) {
      return null;
    }
    
    return parsed.pathname;
  } catch {
    return null;
  }
};

// ============================================================================
// SECTION 3: CONTENT SANITIZATION
// ============================================================================

interface SanitizeOptions {
  allowedTags?: string[];
  allowedAttributes?: Record<string, string[]>;
}

const sanitizeHtmlContent = (html: string, options: SanitizeOptions = {}): string => {
  const allowedTags = options.allowedTags || ["p", "br", "b", "i", "em", "strong"];
  const allowedAttributes = options.allowedAttributes || {};
  
  const tagRegex = /<\/?([a-z]+)[^>]*>/gi;
  
  return html.replace(tagRegex, (match, tag) => {
    const lowerTag = tag.toLowerCase();
    
    if (!allowedTags.includes(lowerTag)) {
      return "";
    }
    
    const attrRegex = /([a-z]+)="([^"]*)"/gi;
    const sanitizedAttrs = match.replace(attrRegex, (attrMatch, attr, value) => {
      const allowedAttrs = allowedAttributes[lowerTag] || [];
      
      if (allowedAttrs.includes(attr)) {
        const safeValue = encodeAttribute(value);
        return `${attr}="${safeValue}"`;
      }
      
      return "";
    });
    
    return sanitizedAttrs;
  });
};

// ============================================================================
// PERFORMANCE CONSIDERATIONS
// ============================================================================

// Sanitization Performance:
// - Encoding has minimal overhead
// - Complex HTML parsing expensive
// - Parse on input, encode on output

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
// - Always encode on output
// - Use context-specific encoding
// - Don't rely on client-side only

// ============================================================================
// TESTING STRATEGY
// ============================================================================

// Testing sanitization:
// 1. Test encoding
// 2. Test dangerous inputs
// 3. Test edge cases

// ============================================================================
// CROSS-REFERENCE
// ============================================================================

// Related patterns:
// - Input Validation (01_Input_Validation.ts)
// - Authentication (03_Authentication_Patterns.ts)
// - Authorization (04_Authorization_Patterns.ts)

// Next: 03_Authentication_Patterns.ts
