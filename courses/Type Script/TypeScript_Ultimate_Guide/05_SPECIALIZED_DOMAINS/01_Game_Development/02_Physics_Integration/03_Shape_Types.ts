/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 02_Physics_Integration
 * Topic: 03_Shape_Types
 * Purpose: Define geometric shapes for physics simulation
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: Shape complexity affects collision detection speed
 * Security: Validated geometry prevents physics instability
 */

namespace ShapeTypes {
  export type ShapeType = 'sphere' | 'box' | 'capsule' | 'cylinder' | 'convexHull' | 'mesh';

  export interface PhysicsShape {
    id: string;
    type: ShapeType;
    density: number;
    friction: number;
    restitution: number;
  }

  export interface SphereShape extends PhysicsShape {
    type: 'sphere';
    radius: number;
  }

  export interface BoxShape extends PhysicsShape {
    type: 'box';
    halfExtents: Vector3D;
  }

  export interface CapsuleShape extends PhysicsShape {
    type: 'capsule';
    radius: number;
    height: number;
  }

  export interface CylinderShape extends PhysicsShape {
    type: 'cylinder';
    radius: number;
    height: number;
  }

  export interface ConvexHullShape extends PhysicsShape {
    type: 'convexHull';
    vertices: Vector3D[];
    faces: number[][];
  }

  export interface MeshShape extends PhysicsShape {
    type: 'mesh';
    vertices: Vector3D[];
    indices: number[];
    isStatic: boolean;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface ShapeFactory {
    createSphere(radius: number): SphereShape;
    createBox(halfExtents: Vector3D): BoxShape;
    createCapsule(radius: number, height: number): CapsuleShape;
    createCylinder(radius: number, height: number): CylinderShape;
    createConvexHull(vertices: Vector3D[]): ConvexHullShape;
    createMesh(vertices: Vector3D[], indices: number[]): MeshShape;
  }

  export function computeBoundingSphere(vertices: Vector3D[]): { center: Vector3D; radius: number } {
    let center = { x: 0, y: 0, z: 0 };
    for (const v of vertices) {
      center.x += v.x;
      center.y += v.y;
      center.z += v.z;
    }
    const n = vertices.length;
    center = { x: center.x / n, y: center.y / n, z: center.z / n };
    let radius = 0;
    for (const v of vertices) {
      const d = distance(center, v);
      if (d > radius) radius = d;
    }
    return { center, radius };
  }

  function distance(a: Vector3D, b: Vector3D): number {
    const dx = b.x - a.x, dy = b.y - a.y, dz = b.z - a.z;
    return Math.sqrt(dx * dx + dy * dy + dz * dz);
  }
}

// Cross-reference: 02_Collision_Types.ts (collision), 04_Rigid_Body_Types.ts (bodies)
console.log("\n=== Shape Types ===");
console.log("Related: 02_Collision_Types.ts, 04_Rigid_Body_Types.ts");