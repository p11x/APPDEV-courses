/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 03_DevOps
 * Concept: 01_CI_CD_Pipelines
 * Topic: 03_Travis_CI
 * Purpose: Define Travis CI configuration types
 * Difficulty: intermediate
 * UseCase: DevOps
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Travis CI
 * Performance: Matrix builds, caching
 * Security: Environment variables, secure keys
 */

namespace TravisCITypes {
  export interface TravisConfig {
    language?: string;
    os?: OS[];
    dist?: Distribution;
    arch?: Architecture[];
    version?: string;
    sudo?: boolean;
    before_install?: Script[];
    install?: Script[];
    before_script?: Script[];
    script?: Script[];
    after_script?: Script[];
    after_failure?: Script[];
    before_deploy?: Script[];
    deploy?: Deploy[];
    after_deploy?: Script[];
    env?: Environment;
    matrix?: Matrix;
    addons?: Addons;
    cache?: Cache;
    notifications?: Notifications;
  }

  export type OS = 'linux' | 'osx' | 'windows';
  export type Distribution = 'precise' | 'trusty' | 'bionic' | 'focal' | 'jammy';
  export type Architecture = 'amd64' | 'arm64';

  export type Script = string | string[];

  export interface Deploy {
    provider: Provider;
    on?: DeployCondition;
    edge?: EdgeOptions;
    script?: string;
    [key: string]: unknown;
  }

  export type Provider = 
    | 'heroku' | 'AWS_S3' | 'AWS_EB' | 'cloudfoundry' | 'npm' 
    | 'pypi' | 'pages' | 'release' | 'script' | 'modulus' | 'openshift';

  export interface DeployCondition {
    branch?: string | string[];
    tags?: boolean;
    condition?: string;
    all_branches?: boolean;
    python?: string;
    node?: string;
    r?: string;
    'run_id'?: number;
  }

  export interface EdgeOptions {
    branch: string;
    enabled?: boolean;
  }

  export interface Environment {
    global?: Record<string, string>;
    matrix?: EnvironmentMatrix[];
  }

  export interface EnvironmentMatrix {
    include?: Record<string, string>[];
    exclude?: Record<string, string>[];
    env?: string[];
  }

  export interface Matrix {
    include?: MatrixRow[];
    exclude?: MatrixRow[];
    allow_failures?: MatrixRow[];
    fast_finish?: boolean;
  }

  export interface MatrixRow {
    language?: string;
    os?: OS;
    dist?: Distribution;
    version?: string;
    env?: string;
    [key: string]: unknown;
  }

  export interface Addons {
    apt?: AptAddon;
    snaps?: string[];
    homebrew?: HomebrewAddon;
    sauce_connect?: SauceConnect;
    jwt?: JWTConfig;
    code_climate?: CodeClimate;
    coverity?: Coverity;
  }

  export interface AptAddon {
    packages?: string[];
    sources?: AptSource[];
    update?: boolean;
  }

  export interface AptSource {
    sourceline: string;
    key_url?: string;
  }

  export interface HomebrewAddon {
    packages?: string[];
    casks?: string[];
  }

  export interface SauceConnect {
    enabled: boolean;
    username: string;
    access_key: string;
  }

  export interface JWTConfig {
    secure: string;
  }

  export interface CodeClimate {
    repo_token?: string;
    repo_token_secure?: string;
  }

  export interface Coverity {
    project: string;
    description?: string;
    token: string;
    token_secure: string;
  }

  export interface Cache {
    directories?: string[];
    apt?: boolean;
    pip?: boolean;
    npm?: boolean;
    bundler?: boolean;
    composer?: boolean;
    ccache?: boolean;
  }

  export interface Notifications {
    email?: EmailNotification;
    slack?: SlackNotification;
    irc?: IRCNotification;
    webhooks?: WebhookNotification;
    flowdock?: FlowdockNotification;
    campfire?: CampfireNotification;
    hipchat?: HipchatNotification;
  }

  export interface EmailNotification {
    recipients?: string[];
    on_success?: 'always' | 'never' | 'change';
    on_failure?: 'always' | 'never' | 'change';
    on_start?: 'always' | 'never';
    on_cancel?: 'always' | 'never';
  }

  export interface SlackNotification {
    enabled: boolean;
    room?: string;
    on_success?: string;
    on_failure?: string;
    on_start?: string;
    on_cancel?: string;
    template?: string[];
  }

  export interface IRCNotification {
    enabled: boolean;
    channels?: string[];
    on_success?: string;
    on_failure?: string;
    on_start?: string;
    on_cancel?: string;
  }

  export interface WebhookNotification {
    enabled: boolean;
    urls?: string[];
    on_success?: string;
    on_failure?: string;
    on_start?: string;
    on_cancel?: string;
  }

  export interface FlowdockNotification {
    enabled: boolean;
    api_token?: string;
  }

  export interface CampfireNotification {
    enabled: boolean;
    room?: string;
    template?: string;
  }

  export interface HipchatNotification {
    enabled: boolean;
    room?: string;
    on_success?: string;
    on_failure?: string;
    on_start?: string;
  }
}

// Cross-reference: 01_GitHub_Actions.ts, 02_Jenkins_Types.ts
console.log("\n=== Travis CI Types ===");
console.log("Related: 01_GitHub_Actions.ts, 02_Jenkins_Types.ts");