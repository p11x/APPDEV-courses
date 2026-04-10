/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 02_Physics_Integration
 * Topic: 02_Collision_Types
 * Purpose: Define collision detection and response types
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n²) naive, O(n log n) with broadphase
 * Security: Collision layers prevent unintended interactions
 */

namespace CollisionTypes {
  export type CollisionResponse = 'block' | 'overlap' | 'trigger';

  export interface CollisionLayer {
    name: string;
    mask: number;
    intersects: string[];
  }

  export interface CollisionMask {
    categories: number;
    mask: number;
  }

  export interface CollisionResult {
    collided: boolean;
    contacts: CollisionContact[];
    normal: Vector3D;
    penetration: number;
  }

  export interface CollisionContact {
    point: Vector3D;
    normal: Vector3D;
    penetration: number;
    velocity: Vector3D;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface AABB {
    min: Vector3D;
    max: Vector3D;
  }

  export interface OBB {
    center: Vector3D;
    axes: Vector3D[];
    halfExtents: Vector3D;
  }

  export interface Ray {
    origin: Vector3D;
    direction: Vector3D;
    maxDistance: number;
    collisionMask: CollisionMask;
  }

  export interface RayHit {
    hit: boolean;
    point: Vector3D;
    normal: Vector3D;
    distance: number;
    entity: string;
  }

  export interface CollisionEvent {
    type: 'enter' | 'stay' | 'exit';
    bodyA: string;
    bodyB: string;
    contacts: CollisionContact[];
    timestamp: number;
  }

  export interface CollisionDetector {
    detectSphereSphere(a: Sphere, b: Sphere): CollisionResult;
    detectSphereBox(sphere: Sphere, box: AABB): CollisionResult;
    detectBoxBox(a: AABB, b: AABB): CollisionResult;
    castRay(ray: Ray): RayHit;
  }

  export interface Sphere {
    center: Vector3D;
    radius: number;
  }
}

// Cross-reference: 01_Physics_Types.ts (physics), 03_Shape_Types.ts (geometric shapes)
console.log("\n=== Collision Types ===");
console.log("Related: 01_Physics_Types.ts, 03_Shape_Types.ts");