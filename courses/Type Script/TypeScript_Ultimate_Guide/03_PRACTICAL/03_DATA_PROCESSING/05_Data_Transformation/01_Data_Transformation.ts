/**
 * Category: PRACTICAL
 * Subcategory: DATA_PROCESSING
 * Concept: Data_Transformation
 * Purpose: Data transformation techniques
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Data Transformation - Comprehensive Guide
 * ====================================
 * 
 * 📚 WHAT: Transforming data between formats
 * 💡 WHERE: API responses, storage
 * 🔧 HOW: Maps, filters, reducers
 */

// ============================================================================
// SECTION 1: MAP TRANSFORMATIONS
// ============================================================================

// Example 1.1: Basic Map
// ---------------------------------

interface User {
  id: number;
  firstName: string;
  lastName: string;
}

interface UserDTO {
  id: string;
  name: string;
}

function toUserDTO(user: User): UserDTO {
  return {
    id: String(user.id),
    name: `${user.firstName} ${user.lastName}`
  };
}

function mapUsers<T, U>(users: T[], mapper: (item: T) => U): U[] {
  return users.map(mapper);
}

// ============================================================================
// SECTION 2: FILTER TRANSFORMATIONS
// ============================================================================

// Example 2.1: Conditional Filter
// ---------------------------------

function filterActive<T extends { isActive?: boolean }>(items: T[]): T[] {
  return items.filter(item => item.isActive !== false);
}

function filterByProperty<T, K extends keyof T>(
  items: T[],
  key: K,
  value: T[K]
): T[] {
  return items.filter(item => item[key] === value);
}

// ============================================================================
// SECTION 3: GROUP BY
// ============================================================================

// Example 3.1: Group By
// ---------------------------------

function groupBy<T>(
  items: T[],
  key: keyof T
): Record<string, T[]> {
  return items.reduce((acc, item) => {
    const groupKey = String(item[key]);
    (acc[groupKey] = acc[groupKey] || []).push(item);
    return acc;
  }, {} as Record<string, T[]>);
}

console.log("\n=== Data Transformation Complete ===");
console.log("Next: PRACTICAL/DATA_PROCESSING/05_Data_Transformation/01_Data_Transformation.ts");