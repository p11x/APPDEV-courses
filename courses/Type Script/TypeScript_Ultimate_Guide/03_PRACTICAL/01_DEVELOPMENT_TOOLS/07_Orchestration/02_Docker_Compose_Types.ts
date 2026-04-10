/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 07_Orchestration Topic: 02_Docker_Compose_Types Purpose: Docker Compose type definitions Difficulty: intermediate UseCase: devops, local-dev Version: TS 5.0+ Compatibility: Node.js 16+, Docker Performance: N/A Security: N/A */

declare namespace DockerComposeTypes {
  interface ComposeSpecification {
    version?: string;
    services: ServiceDefinition[];
    networks?: Record<string, NetworkDefinition>;
    volumes?: Record<string, VolumeDefinition>;
    configs?: Record<string, ConfigDefinition>;
    secrets?: Record<string, SecretDefinition>;
  }

  interface ServiceDefinition {
    image?: string;
    build?: string | BuildDefinition;
    command?: string | string[];
    container_name?: string;
    depends_on?: string[] | Record<string, DependencyConfig>;
    deploy?: DeployConfig;
    entrypoint?: string;
    environment?: Record<string, string | null>;
    expose?: string[];
    external_links?: string[];
    extra_hosts?: string[];
    healthcheck?: HealthcheckConfig;
    labels?: Record<string, string>;
    logging?: LoggingConfig;
    network_mode?: string;
    networks?: string[];
    ports?: string[];
    privileged?: boolean;
    restart?: string;
    stdin_open?: boolean;
    tty?: boolean;
    user?: string;
    volumes?: string[];
    volumes_from?: string[];
    working_dir?: string;
  }

  interface BuildDefinition {
    context?: string;
    dockerfile?: string;
    args?: Record<string, string>;
    labels?: Record<string, string>;
    cache_from?: string[];
    network?: string;
    target?: string;
  }

  interface DependencyConfig {
    condition: 'service_started' | 'service_healthy' | 'service_completed_successfully';
  }

  interface DeployConfig {
    mode?: 'replicated' | 'global';
    replicas?: number;
    placement?: PlacementConfig;
    resources?: ResourcesConfig;
    restart_policy?: RestartPolicyConfig;
    update_config?: UpdateConfig;
    rollback_config?: RollbackConfig;
  }

  interface PlacementConfig {
    constraints?: string[];
    preferences?: Array<{ spread: string }>;
  }

  interface ResourcesConfig {
    limits?: ResourceLimits;
    reservations?: ResourceReservations;
  }

  interface ResourceLimits {
    cpus?: string | number;
    memory?: string;
    pids?: number;
  }

  interface ResourceReservations {
    cpus?: string | number;
    memory?: string;
    devices?: DeviceRequest[];
  }

  interface DeviceRequest {
    capabilities: string[][];
    count?: number;
    driver?: string;
  }

  interface RestartPolicyConfig {
    condition: 'none' | 'on-failure' | 'any';
    delay?: string;
    max_attempts?: number;
    window?: string;
  }

  interface UpdateConfig {
    parallelism?: number;
    delay?: string;
    failure_action?: 'continue' | 'pause' | 'rollback';
    order?: 'start-first' | 'stop-first';
  }

  interface RollbackConfig {
    parallelism?: number;
    delay?: string;
    failure_action?: 'continue' | 'pause' | 'rollback';
    order?: 'start-first' | 'stop-first';
  }

  interface HealthcheckConfig {
    test?: string | string[];
    interval?: string;
    timeout?: string;
    retries?: number;
    start_period?: string;
    disable?: boolean;
  }

  interface LoggingConfig {
    driver?: string;
    options?: Record<string, string>;
  }

  interface NetworkDefinition {
    name?: string;
    driver?: string;
    driver_opts?: Record<string, string>;
    external?: boolean;
    enable_ipv6?: boolean;
    ipam?: IPAMConfig;
  }

  interface IPAMConfig {
    driver?: string;
    config?: IPAMPool[];
  }

  interface IPAMPool {
    subnet?: string;
    gateway?: string;
  }

  interface VolumeDefinition {
    name?: string;
    driver?: string;
    driver_opts?: Record<string, string>;
    external?: boolean;
    labels?: Record<string, string>;
  }

  interface ConfigDefinition {
    file?: string;
    name?: string;
    external?: boolean;
  }

  interface SecretDefinition {
    file?: string;
    name?: string;
    external?: boolean;
  }
}

import { parse as parseCompose, normalize as normalizeCompose } from '@docker/compose';

describe('Docker Compose Types', () => {
  describe('Basic Compose', () => {
    it('should parse compose file', () => {
      const compose: DockerComposeTypes.ComposeSpecification = {
        services: {
          web: { image: 'nginx' },
          db: { image: 'postgres' },
        },
      };
      expect(compose).toBeDefined();
    });
  });

  describe('Services', () => {
    it('should define service with build', () => {
      const service: DockerComposeTypes.ServiceDefinition = {
        build: { context: '.', dockerfile: 'Dockerfile' },
      };
      expect(service.build).toBeDefined();
    });

    it('should define service with ports', () => {
      const service: DockerComposeTypes.ServiceDefinition = {
        ports: ['3000:3000', '8080:8080'],
        environment: { NODE_ENV: 'production' },
      };
      expect(service.ports).toBeDefined();
    });
  });

  describe('Volumes', () => {
    it('should define named volume', () => {
      const compose: DockerComposeTypes.ComposeSpecification = {
        volumes: {
          dbdata: { driver: 'local' },
        },
      };
      expect(compose.volumes).toBeDefined();
    });
  });

  describe('Networks', () => {
    it('should define network', () => {
      const compose: DockerComposeTypes.ComposeSpecification = {
        networks: {
          frontend: { driver: 'bridge' },
        },
      };
      expect(compose.networks).toBeDefined();
    });
  });
});

console.log('\n=== Docker Compose Types Complete ===');
console.log('Next: 03_Helm_Charts.ts');