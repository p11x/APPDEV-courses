/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 02_Physics_Integration
 * Topic: 04_Rigid_Body_Types
 * Purpose: Define rigid body physics types for simulation
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(1) transform updates per body
 * Security: Force clamping prevents explosive behavior
 */

namespace RigidBodyTypes {
  export type MotionType = 'static' | 'dynamic' | 'kinematic';

  export interface RigidBody {
    id: string;
    motionType: MotionType;
    mass: number;
    position: Vector3D;
    rotation: Quaternion;
    linearVelocity: Vector3D;
    angularVelocity: Vector3D;
    forces: Vector3D;
    torques: Vector3D;
    linearDamping: number;
    angularDamping: number;
    gravityScale: number;
    isEnabled: boolean;
  }

  export interface Vector3D {
    x: number;
    y: number;
    z: number;
  }

  export interface Quaternion {
    x: number;
    y: number;
    z: number;
    w: number;
  }

  export interface RigidBodyBuilder {
    withMass(mass: number): RigidBodyBuilder;
    withPosition(pos: Vector3D): RigidBodyBuilder;
    withRotation(rot: Quaternion): RigidBodyBuilder;
    withLinearDamping(damping: number): RigidBodyBuilder;
    withAngularDamping(damping: number): RigidBodyBuilder;
    withMotionType(type: MotionType): RigidBodyBuilder;
    build(): RigidBody;
  }

  export interface RigidBodySystem {
    addBody(body: RigidBody): void;
    removeBody(bodyId: string): void;
    getBody(bodyId: string): RigidBody | undefined;
    applyForce(bodyId: string, force: Vector3D): void;
    applyImpulse(bodyId: string, impulse: Vector3D): void;
    applyTorque(bodyId: string, torque: Vector3D): void;
    setVelocity(bodyId: string, velocity: Vector3D): void;
    setAngularVelocity(bodyId: string, velocity: Vector3D): void;
    getVelocityAtPoint(bodyId: string, point: Vector3D): Vector3D;
    computeKineticEnergy(body: RigidBody): number;
  }

  export interface IntegrationMethod {
    integrate(body: RigidBody, dt: number): void;
  }

  export interface EulerIntegration implements IntegrationMethod {
    integrate(body: RigidBody, dt: number): void {
      body.linearVelocity.x += (body.forces.x / body.mass) * dt;
      body.linearVelocity.y += (body.forces.y / body.mass) * dt;
      body.linearVelocity.z += (body.forces.z / body.mass) * dt;
      body.position.x += body.linearVelocity.x * dt;
      body.position.y += body.linearVelocity.y * dt;
      body.position.z += body.linearVelocity.z * dt;
    }
  }

  export function clampForce(force: Vector3D, maxForce: number): Vector3D {
    const magnitude = Math.sqrt(force.x ** 2 + force.y ** 2 + force.z ** 2);
    if (magnitude > maxForce) {
      const scale = maxForce / magnitude;
      return { x: force.x * scale, y: force.y * scale, z: force.z * scale };
    }
    return force;
  }
}

// Cross-reference: 01_Physics_Types.ts (world), 03_Shape_Types.ts (shapes)
console.log("\n=== Rigid Body Types ===");
console.log("Related: 01_Physics_Types.ts, 03_Shape_Types.ts");