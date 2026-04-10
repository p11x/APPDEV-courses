/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 05_IoT
 * Concept: 01_Device_Types
 * Topic: 01_Sensor_Types
 * Purpose: Define sensor types for IoT devices
 * Difficulty: intermediate
 * UseCase: IoT
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Embedded Systems
 * Performance: O(1) data collection, real-time processing
 * Security: Device authentication, data validation
 */

namespace SensorTypes {
  export type SensorCategory = 'temperature' | 'humidity' | 'pressure' | 'motion' | 'light' | 'sound' | 'chemical' | 'image';

  export interface Sensor {
    id: string;
    name: string;
    category: SensorCategory;
    type: string;
    unit: string;
    range: SensorRange;
    accuracy: number;
    resolution: number;
    samplingRate: number;
    calibration: CalibrationData;
  }

  export interface SensorRange {
    min: number;
    max: number;
  }

  export interface CalibrationData {
    offset: number;
    scale: number;
    lastCalibrated?: number;
    nextCalibration?: number;
  }

  export interface SensorReading {
    sensorId: string;
    value: number;
    quality: DataQuality;
    timestamp: number;
    location?: GeoLocation;
    metadata?: Record<string, unknown>;
  }

  export type DataQuality = 'good' | 'uncertain' | 'bad' | 'no_data';

  export interface GeoLocation {
    latitude: number;
    longitude: number;
    altitude?: number;
    accuracy?: number;
  }

  export interface TemperatureSensor extends Sensor {
    category: 'temperature';
    unit: 'celsius' | 'fahrenheit' | 'kelvin';
    range: { min: -40; max: 125 };
    thermalTimeConstant: number;
  }

  export interface HumiditySensor extends Sensor {
    category: 'humidity';
    unit: 'percent';
    range: { min: 0; max: 100 };
    responseTime: number;
    hysteresis: number;
  }

  export interface PressureSensor extends Sensor {
    category: 'pressure';
    unit: 'hPa' | 'psi' | 'bar';
    range: { min: 300; max: 1100 };
    pressureType: 'absolute' | 'gauge' | 'differential';
  }

  export interface MotionSensor extends Sensor {
    category: 'motion';
    detectionRange: number;
    detectionAngle: number;
    sensitivity: 'low' | 'medium' | 'high';
    types: ('pir' | 'microwave' | 'ultrasonic' | 'accelerometer')[];
  }

  export interface LightSensor extends Sensor {
    category: 'light';
    unit: 'lux' | 'foot-candle';
    spectralResponse: 'visible' | 'full' | 'infrared';
    range: { min: 0; max: 100000 };
  }

  export interface SoundSensor extends Sensor {
    category: 'sound';
    unit: 'dB';
    frequencyRange: { min: 20; max: 20000 };
    weight: 'a' | 'c' | 'z';
  }

  export interface ChemicalSensor extends Sensor {
    category: 'chemical';
    targetGas?: string;
    detectionLimit: number;
    specificity: string[];
    operatingTemp: number;
  }

  export interface CameraSensor extends Sensor {
    category: 'image';
    resolution: { width: number; height: number };
    frameRate: number;
    bitDepth: number;
    compression: 'none' | 'jpeg' | 'h264' | 'h265';
    nightVision: boolean;
  }

  export interface SensorArray {
    id: string;
    sensors: Sensor[];
    busType: 'i2c' | 'spi' | 'uart' | 'can' | 'custom';
    address?: string;
    powerMode: PowerMode;
    dataFormat: DataFormat;
  }

  export type PowerMode = 'always_on' | 'low_power' | 'periodic' | 'on_demand';
  export type DataFormat = 'raw' | 'calibrated' | 'processed' | 'compressed';

  export interface SensorManager {
    initialize(sensors: Sensor[]): Promise<void>;
    read(sensorId: string): Promise<SensorReading>;
    readAll(): Promise<SensorReading[]>;
    calibrate(sensorId: string, data: CalibrationData): Promise<void>;
    configure(sensorId: string, config: Partial<Sensor>): Promise<void>;
  }
}

// Cross-reference: 02_Actuator_Types.ts (actuators), 01_MQTT_Types.ts (communication)
console.log("\n=== Sensor Types ===");
console.log("Related: 02_Actuator_Types.ts, 01_MQTT_Types.ts");