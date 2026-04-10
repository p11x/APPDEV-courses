/**
 * Category: ADVANCED
 * Subcategory: INTEGRATION
 * Concept: Backend_Development
 * Purpose: Database ORM types in TypeScript
 * Difficulty: intermediate
 * UseCase: backend
 */

type ORMType<T> = {
  findAll(): Promise<T[]>;
  findById(id: number): Promise<T | null>;
  create(entity: Partial<T>): Promise<T>;
  update(id: number, entity: Partial<T>): Promise<T>;
  delete(id: number): Promise<boolean>;
};

console.log("\n=== Database ORM Types Complete ===");
console.log("Next: ADVANCED/INTEGRATION/02_Backend_Development/02_GraphQL_Types.ts");