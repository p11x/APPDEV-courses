/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 05_IoT
 * Concept: 01_Device_Types
 * Topic: 02_Actuator_Types
 * Purpose: Define actuator types for IoT control
 * Difficulty: intermediate
 * UseCase: IoT
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Embedded Systems
 * Performance: O(1) command execution, real-time control
 * Security: Authentication prevents unauthorized control
 */

namespace ActuatorTypes {
  export type ActuatorCategory = 'motor' | 'solenoid' | 'relay' | 'servo' | 'valve' | 'heater' | 'cooler' | 'lighting' | 'display';
  export type ActuatorState = 'idle' | 'active' | 'error' | 'disabled';

  export interface Actuator {
    id: string;
    name: string;
    category: ActuatorCategory;
    type: string;
    state: ActuatorState;
    capabilities: string[];
    limits: ActuatorLimits;
    position: number;
    feedback: FeedbackConfig;
  }

  export interface ActuatorLimits {
    minValue: number;
    maxValue: number;
    maxSpeed?: number;
    maxForce?: number;
    overTravelProtection: boolean;
  }

  export interface FeedbackConfig {
    enabled: boolean;
    type: 'position' | 'velocity' | 'force' | 'current';
    resolution: number;
    feedbackRate: number;
  }

  export interface DCMotor extends Actuator {
    category: 'motor';
    type: 'dc';
    voltage: number;
    currentRating: number;
    rpm: number;
    torque: number;
    direction: 'forward' | 'reverse' | 'brake';
    pwmSpeed: number;
  }

  export interface StepperMotor extends Actuator {
    category: 'motor';
    type: 'stepper';
    stepsPerRevolution: number;
    microstepping: number;
    currentPhase: number;
    holdTorque: number;
    stepMode: 'full' | 'half' | 'quarter' | 'eighth' | 'sixteenth';
  }

  export interface ServoMotor extends Actuator {
    category: 'servo';
    type: 'rc' | 'digital';
    angleRange: { min: number; max: number };
    rotationSpeed: number;
    torque: number;
    feedback: 'none' | 'internal' | 'external';
  }

  export interface Solenoid extends Actuator {
    category: 'solenoid';
    type: 'push' | 'pull' | 'latching';
    stroke: number;
    force: number;
    responseTime: number;
    holdingCurrent: number;
    position: 'extended' | 'retracted';
  }

  export interface Relay extends Actuator {
    category: 'relay';
    type: 'spst' | 'dpst' | 'spdt' | 'dpdt';
    voltageRating: number;
    currentRating: number;
    contactType: 'normally_open' | 'normally_closed' | 'changeover';
    switchingTime: number;
  }

  export interface Valve extends Actuator {
    category: 'valve';
    type: 'ball' | 'butterfly' | 'gate' | 'globe' | 'solenoid';
    size: number;
    flowCoefficient: number;
    pressureRating: number;
    temperatureRange: { min: number; max: number };
    position: number;
  }

  export interface Heater extends Actuator {
    category: 'heater';
    type: 'resistive' | 'inductive' | 'peltier' | 'infrared';
    power: number;
    voltage: number;
    maxTemperature: number;
    sensorIntegration: string;
    pwmControl: boolean;
  }

  export interface Cooler extends Actuator {
    category: 'cooler';
    type: 'fan' | 'peltier' | 'compressor' | 'chiller';
    capacity: number;
    fanSpeed: number;
    temperatureSetpoint: number;
    mode: 'cooling' | 'heating' | 'auto';
  }

  export interface LightingActuator extends Actuator {
    category: 'lighting';
    type: 'led' | 'halogen' | 'fluorescent' | 'neon';
    brightness: number;
    color?: RGBColor;
    colorTemperature?: number;
    dimmable: boolean;
  }

  export interface RGBColor {
    r: number;
    g: number;
    b: number;
  }

  export interface DisplayActuator extends Actuator {
    category: 'display';
    type: 'lcd' | 'oled' | 'led_matrix' | 'eink';
    resolution: { width: number; height: number };
    backlight: boolean;
    colorDepth: number;
  }

  export interface ActuatorCommand {
    actuatorId: string;
    action: 'set' | 'move' | 'stop' | 'reset' | 'calibrate';
    value: number;
    speed?: number;
    force?: boolean;
  }

  export interface ActuatorStatus {
    actuatorId: string;
    state: ActuatorState;
    position: number;
    velocity: number;
    current: number;
    temperature?: number;
    error?: string;
    timestamp: number;
  }

  export interface ActuatorManager {
    sendCommand(command: ActuatorCommand): Promise<void>;
    getStatus(actuatorId: string): Promise<ActuatorStatus>;
    calibrate(actuatorId: string): Promise<void>;
    emergencyStop(actuatorId?: string): Promise<void>;
  }
}

// Cross-reference: 01_Sensor_Types.ts (sensors), 01_MQTT_Types.ts (communication)
console.log("\n=== Actuator Types ===");
console.log("Related: 01_Sensor_Types.ts, 01_MQTT_Types.ts");