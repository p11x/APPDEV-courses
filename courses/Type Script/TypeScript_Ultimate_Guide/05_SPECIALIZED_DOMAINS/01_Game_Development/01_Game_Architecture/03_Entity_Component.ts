/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 01_Game_Development
 * Concept: 01_Game_Architecture
 * Topic: 03_Entity_Component
 * Purpose: Define entity-component system types for game architecture
 * Difficulty: advanced
 * UseCase: game development
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(n) entity queries, O(1) component access
 * Security: Type-safe component access patterns
 */

namespace EntityComponentTypes {
  export type EntityId = string;
  export type ComponentId = string;

  export interface Entity {
    id: EntityId;
    components: Map<ComponentId, Component>;
    active: boolean;
    tags: Set<string>;
  }

  export interface Component {
    id: ComponentId;
    type: string;
    data: unknown;
    entityId: EntityId;
  }

  export interface TransformComponent extends Component {
    type: 'transform';
    data: {
      position: { x: number; y: number; z: number };
      rotation: { x: number; y: number; z: number; w: number };
      scale: { x: number; y: number; z: number };
    };
  }

  export interface RenderComponent extends Component {
    type: 'render';
    data: {
      meshId: string;
      materialId: string;
      visible: boolean;
      layer: number;
    };
  }

  export interface PhysicsComponent extends Component {
    type: 'physics';
    data: {
      bodyType: 'static' | 'dynamic' | 'kinematic';
      mass: number;
      friction: number;
      restitution: number;
      collisionGroup: string;
    };
  }

  export interface HealthComponent extends Component {
    type: 'health';
    data: {
      current: number;
      maximum: number;
      invulnerable: boolean;
    };
  }

  export interface ComponentFactory<T extends Component> {
    create(entityId: EntityId, data: T['data']): T;
  }

  export interface EntityManager {
    createEntity(tags?: string[]): Entity;
    destroyEntity(entityId: EntityId): void;
    getEntity(entityId: EntityId): Entity | undefined;
    addComponent<T extends Component>(entityId: EntityId, component: T): void;
    removeComponent(entityId: EntityId, componentId: ComponentId): void;
    getComponent<T extends Component>(entityId: EntityId, componentId: string): T | undefined;
    query(components: string[]): Entity[];
  }

  class EntityManagerImpl implements EntityManager {
    private entities: Map<EntityId, Entity> = new Map();
    private entityCounter = 0;

    createEntity(tags: string[] = []): Entity {
      const entity: Entity = {
        id: `entity_${++this.entityCounter}`,
        components: new Map(),
        active: true,
        tags: new Set(tags),
      };
      this.entities.set(entity.id, entity);
      return entity;
    }

    destroyEntity(entityId: EntityId): void {
      this.entities.delete(entityId);
    }

    getEntity(entityId: EntityId): Entity | undefined {
      return this.entities.get(entityId);
    }

    addComponent<T extends Component>(entityId: EntityId, component: T): void {
      const entity = this.entities.get(entityId);
      if (entity) {
        entity.components.set(component.id, component);
      }
    }

    removeComponent(entityId: EntityId, componentId: ComponentId): void {
      const entity = this.entities.get(entityId);
      if (entity) {
        entity.components.delete(componentId);
      }
    }

    getComponent<T extends Component>(entityId: EntityId, componentId: string): T | undefined {
      const entity = this.entities.get(entityId);
      return entity?.components.get(componentId) as T | undefined;
    }

    query(components: string[]): Entity[] {
      return Array.from(this.entities.values()).filter(entity => 
        components.every(comp => 
          Array.from(entity.components.values()).some(c => c.type === comp)
        )
      );
    }
  }
}

// Cross-reference: 02_Game_State_Types.ts (state management), 01_Game_Types.ts (game types)
console.log("\n=== Entity Component Types ===");
console.log("Related: 02_Game_State_Types.ts, 01_Game_Types.ts");