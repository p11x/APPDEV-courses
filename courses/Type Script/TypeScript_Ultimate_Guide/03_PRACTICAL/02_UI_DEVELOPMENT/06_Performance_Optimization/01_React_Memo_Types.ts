/**
 * Category: PRACTICAL
 * Subcategory: UI_DEVELOPMENT
 * Concept: Performance_Optimization
 * Purpose: React performance optimization types
 * Difficulty: intermediate
 * UseCase: web
 */

/**
 * Performance Optimization - Comprehensive Guide
 * ===============================================
 * 
 * 📚 WHAT: Optimizing React performance with TypeScript
 * 💡 WHERE: Large React applications
 * 🔧 HOW: Memo, useMemo, useCallback
 */

// ============================================================================
// SECTION 1: REACT.MEMO
// ============================================================================

// Example 1.1: Memoized Components
// ---------------------------------

import { memo, PureComponent } from "react";

interface ButtonProps {
  label: string;
  onClick: () => void;
  variant?: "primary" | "secondary";
}

// Regular component
function Button({ label, onClick, variant = "primary" }: ButtonProps) {
  return (
    <button onClick={onClick} data-variant={variant}>
      {label}
    </button>
  );
}

// Memoized component - only re-renders when props change
const MemoizedButton = memo(function MemoizedButton({
  label,
  onClick
}: ButtonProps) {
  return <button onClick={onClick}>{label}</button>;
});

// ============================================================================
// SECTION 2: USE MEMO
// ============================================================================

// Example 2.1: Memoized Values
// ---------------------------------

import { useMemo, useCallback } from "react";

interface User {
  id: number;
  name: string;
}

function UserList({ users }: { users: User[] }) {
  // Memoized expensive computation
  const sortedUsers = useMemo(() => {
    return users
      .filter(u => u.name.length > 0)
      .sort((a, b) => a.name.localeCompare(b.name));
  }, [users]);
  
  // Memoized callback
  const handleSelect = useCallback((user: User) => {
    console.log("Selected:", user);
  }, []);
  
  return (
    <ul>
      {sortedUsers.map(user => (
        <li key={user.id} onClick={() => handleSelect(user)}>
          {user.name}
        </li>
      ))}
    </ul>
  );
}

console.log("\n=== Performance Optimization Complete ===");
console.log("Next: PRACTICAL/DEVELOPMENT_TOOLS/02_Linting_and_Formatting/03_TSLint_Migration.ts");