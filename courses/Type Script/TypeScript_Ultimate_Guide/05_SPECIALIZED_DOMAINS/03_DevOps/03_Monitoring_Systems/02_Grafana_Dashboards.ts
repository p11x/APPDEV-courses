/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 03_Monitoring_Systems
 * Topic: 02_Grafana_Dashboards
 * Purpose: Define Grafana dashboard types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Grafana
 * Performance: Efficient panel rendering, caching
 * Security: Role-based access, secure datasource
 */

namespace GrafanaDashboardTypes {
  export interface GrafanaDashboard {
    id?: number;
    uid?: string;
    title: string;
    tags?: string[];
    timezone?: string;
    schemaVersion: number;
    version: number;
    refresh?: string;
    panels?: Panel[];
    templating?: Templating;
    annotations?: Annotations;
    links?: DashboardLink[];
    login?: string;
    folderId?: number;
    folderUid?: string;
    created?: string;
    updated?: string;
  }

  export interface Panel {
    id?: number;
    title?: string;
    type: PanelType;
    gridPos?: GridPos;
    x?: number;
    y?: number;
    w?: number;
    h?: number;
    datasource?: DataSourceRef;
    targets?: Query[];
    options?: PanelOptions;
    fieldConfig?: FieldConfig;
    pluginVersion?: string;
    transformations?: Transformation[];
    interval?: string;
    description?: string;
    links?: PanelLink[];
  }

  export type PanelType = 
    | 'timeseries' | 'graph' | 'stat' | 'gauge' | 'bargauge' | 'table' | 'piechart' 
    | 'logs' | 'heatmap' | 'status-history' | 'candlestick' | 'traces' | 'node-graph'
    | 'flame-graph' | 'canvas';

  export interface GridPos {
    x: number;
    y: number;
    w: number;
    h: number;
    static?: boolean;
  }

  export interface DataSourceRef {
    type: string;
    uid: string;
  }

  export interface Query {
    refId: string;
    expr?: string;
    legendFormat?: string;
    interval?: string;
    datasource?: DataSourceRef;
    queryType?: string;
    hide?: boolean;
    metric?: string;
  }

  export interface PanelOptions {
    legend?: LegendOptions;
    tooltip?: TooltipOptions;
    thresholds?: ThresholdsConfig;
    mappings?: ValueMapping[];
    noValue?: string;
  }

  export interface LegendOptions {
    displayMode: 'table' | 'list' | 'hidden';
    placement: 'bottom' | 'right';
    showLegend?: boolean;
    values?: ('min' | 'max' | 'avg' | 'current' | 'total')[];
    calcs?: string[];
  }

  export interface TooltipOptions {
    mode: 'single' | 'multi' | 'none';
    sort: 'none' | 'asc' | 'desc';
  }

  export interface ThresholdsConfig {
    mode: 'absolute' | 'percentage';
    steps: ThresholdStep[];
  }

  export interface ThresholdStep {
    color: string;
    value: number;
    label?: string;
  }

  export interface ValueMapping {
    type: 'value' | 'range' | 'special';
    options?: Record<string, unknown>;
  }

  export interface FieldConfig {
    defaults: FieldConfigDefaults;
    overrides?: FieldOverride[];
  }

  export interface FieldConfigDefaults {
    unit?: string;
    decimals?: number;
    min?: number;
    max?: number;
    threshholds?: ThresholdsConfig;
    color?: FieldColor;
    mappings?: ValueMapping[];
    custom?: Record<string, unknown>;
  }

  export interface FieldColor {
    mode: 'thresholds' | 'palette' | 'continuous-GrYlRd' | 'fixed' | 'shades';
    fixedColor?: string;
  }

  export interface FieldOverride {
    matcher: OverrideMatcher;
    properties: OverrideProperty[];
  }

  export interface OverrideMatcher {
    id: string;
    options?: unknown;
  }

  export interface OverrideProperty {
    id: string;
    value?: unknown;
  }

  export interface Transformation {
    id: string;
    options?: Record<string, unknown>;
  }

  export interface PanelLink {
    title: string;
    url: string;
    asDropdown?: boolean;
    targetBlank?: boolean;
  }

  export interface Templating {
    list: TemplateVariable[];
  }

  export interface TemplateVariable {
    name: string;
    type: VariableType;
    label?: string;
    query: string;
    datasource?: DataSourceRef;
    options?: VariableOption[];
    current?: VariableOption;
    definition?: string;
    hide?: number;
    refresh?: number;
    sort?: number;
    multi?: boolean;
    includeAll?: boolean;
    allValue?: string;
  }

  export type VariableType = 'query' | 'interval' | 'custom' | 'textbox' | 'constant' | 'datasource';

  export interface VariableOption {
    text: string;
    value: string;
    selected?: boolean;
  }

  export interface Annotations {
    list: AnnotationQuery[];
  }

  export interface AnnotationQuery {
    name: string;
    datasource: DataSourceRef;
    iconColor?: string;
    enable?: boolean;
    hide?: boolean;
    type?: string;
    query?: string;
    tags?: string[];
    matchAny?: boolean;
  }

  export interface DashboardLink {
    title: string;
    url: string;
    asDropdown?: boolean;
    icon?: string;
    includeVars?: boolean;
    keepTime?: boolean;
    tags?: string[];
    type: 'link' | 'dashboard';
  }

  export interface Folder {
    id: number;
    uid: string;
    title: string;
    url: string;
    hasAcl: boolean;
  }

  export interface Datasource {
    id: number;
    uid: string;
    name: string;
    type: string;
    url: string;
    access: 'proxy' | 'direct';
    isDefault: boolean;
    jsonData: Record<string, unknown>;
    secureJsonData?: Record<string, string>;
  }
}

// Cross-reference: 01_Prometheus_Types.ts (metrics source)
console.log("\n=== Grafana Dashboard Types ===");
console.log("Related: 01_Prometheus_Types.ts");