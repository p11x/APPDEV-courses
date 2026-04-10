/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 02_Physics_Integration
 * Topic: 01_Physics_Types
 * Purpose: Define core physics engine types for game development
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) collision detection, O(1) body updates
 * Security: Validated physics parameters prevent instability
 */

namespace PhysicsTypes {
  export type PhysicsWorldType = 'discrete' | 'continuous';

  export interface PhysicsWorld {
    gravity: Vector3D;
    worldType: PhysicsWorldType;
    solverIterations: number;
    allowSleep: boolean;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface PhysicsBody {
    id: string;
    bodyType: BodyType;
    mass: number;
    inverseMass: number;
    inertia: Vector3D;
    inverseInertia: Vector3D;
    position: Vector3D;
    rotation: Quaternion;
    linearVelocity: Vector3D;
    angularVelocity: Vector3D;
    force: Vector3D;
    torque: Vector3D;
    sleepState: SleepState;
  }

  export type BodyType = 'static' | 'dynamic' | 'kinematic';
  export type SleepState = 'active' | 'sleeping' | 'wakeOnDemand';

  export interface Quaternion {
    x: number;
    y: number;
    z: number;
    w: number;
  }

  export interface PhysicsMaterial {
    friction: number;
    restitution: number;
    frictionCombine: CombineMode;
    restitutionCombine: CombineMode;
  }

  export type CombineMode = 'average' | 'minimum' | 'multiply' | 'maximum';

  export interface PhysicsConstraint {
    id: string;
    type: ConstraintType;
    bodyA: string;
    bodyB: string;
    breakingForce: number;
    enabled: boolean;
  }

  export type ConstraintType = 'point' | 'hinge' | 'slider' | 'generic6DOF';

  export interface PhysicsResult {
    contacts: ContactPoint[];
    manifolds: ContactManifold[];
  }

  export interface ContactPoint {
    position: Vector3D;
    normal: Vector3D;
    penetration: number;
    impulse: number;
  }

  export interface ContactManifold {
    bodyA: string;
    bodyB: string;
    contacts: ContactPoint[];
  }

  export interface PhysicsTick {
    deltaTime: number;
    totalTime: number;
    substeps: number;
  }

  export interface PhysicsEvent {
    type: 'collision_start' | 'collision_end' | 'trigger_enter' | 'trigger_exit';
    bodyA: string;
    bodyB: string;
    contacts: ContactPoint[];
  }
}

// Cross-reference: 02_Collision_Types.ts (collision detection), 04_Rigid_Body_Types.ts (rigid bodies)
console.log("\n=== Physics Types ===");
console.log("Related: 02_Collision_Types.ts, 04_Rigid_Body_Types.ts");