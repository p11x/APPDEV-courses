# Config Files

## What You'll Learn

- How to load configuration from multiple sources with cosmiconfig
- How rc files work (`.myapprc`, `.myapprc.json`)
- How XDG Base Directory paths organize config files
- How to merge configurations from multiple sources
- How to implement a config search and merge strategy

## What Is Cosmiconfig?

Applications need configuration from multiple places:
- Config files (`.myapprc.json`, `myapp.config.js`)
- `package.json` field (`"myapp": { ... }`)
- Environment variables
- Command-line arguments

**Cosmiconfig** searches for config files in standard locations and merges them. It supports JSON, YAML, JS, and TypeScript formats.

## Project Setup

```bash
npm install cosmiconfig
```

## Basic Config Loading

```js
// config.js — Load configuration with cosmiconfig

import { cosmiconfig } from 'cosmiconfig';

// Create an explorer for your application
// The first argument is your app name — cosmiconfig searches for files based on it
const explorer = cosmiconfig('myapp', {
  // File formats to search for (in order of precedence)
  searchPlaces: [
    'package.json',           // "myapp" field in package.json
    '.myapprc',               // JSON or YAML
    '.myapprc.json',          // JSON
    '.myapprc.yaml',          // YAML
    '.myapprc.yml',           // YAML
    '.myapprc.js',            // JS module (export default)
    '.myapprc.cjs',           // CommonJS module
    'myapp.config.js',        // JS module
    'myapp.config.cjs',       // CommonJS module
  ],

  // Only search up to the project root (where package.json is)
  stopDir: process.cwd(),
});

async function loadConfig() {
  // search() looks for config files starting from the current directory
  // and walking up to the filesystem root (or stopDir)
  const result = await explorer.search();

  if (!result) {
    console.log('No config file found — using defaults');
    return getDefaultConfig();
  }

  console.log(`Config loaded from: ${result.filepath}`);
  return result.config;
}

function getDefaultConfig() {
  return {
    port: 3000,
    host: 'localhost',
    database: {
      url: 'sqlite://./data.db',
    },
    logLevel: 'info',
  };
}

const config = await loadConfig();
console.log('Configuration:', config);
```

## Config File Formats

### JSON (`.myapprc.json`)

```json
{
  "port": 4000,
  "host": "0.0.0.0",
  "database": {
    "url": "postgres://localhost/mydb"
  },
  "logLevel": "debug"
}
```

### JavaScript (`myapp.config.js`)

```js
// myapp.config.js — Dynamic configuration

export default {
  port: parseInt(process.env.PORT) || 3000,
  host: process.env.HOST || 'localhost',
  database: {
    url: process.env.DATABASE_URL || 'sqlite://./data.db',
  },
  // Can include logic — e.g., different config per environment
  logLevel: process.env.NODE_ENV === 'production' ? 'warn' : 'debug',
};
```

### package.json Field

```json
{
  "name": "my-project",
  "myapp": {
    "port": 3000,
    "host": "localhost"
  }
}
```

## Config Merge Strategy

```js
// merge-config.js — Merge config from multiple sources

import { cosmiconfig } from 'cosmiconfig';

async function loadMergedConfig() {
  const explorer = cosmiconfig('myapp');

  // 1. Start with defaults
  const defaults = {
    port: 3000,
    host: 'localhost',
    logLevel: 'info',
    database: { url: 'sqlite://./data.db', poolSize: 5 },
  };

  // 2. Load from config file
  const fileConfig = await explorer.search();
  const fromFile = fileConfig?.config || {};

  // 3. Load from environment variables (highest priority)
  const envConfig = {};
  if (process.env.MYAPP_PORT) envConfig.port = parseInt(process.env.MYAPP_PORT);
  if (process.env.MYAPP_HOST) envConfig.host = process.env.MYAPP_HOST;
  if (process.env.MYAPP_LOG_LEVEL) envConfig.logLevel = process.env.MYAPP_LOG_LEVEL;
  if (process.env.MYAPP_DATABASE_URL) {
    envConfig.database = { url: process.env.MYAPP_DATABASE_URL };
  }

  // 4. Deep merge: defaults → file → env (env wins)
  const config = deepMerge(defaults, fromFile, envConfig);

  return config;
}

// Deep merge multiple objects — later objects override earlier ones
function deepMerge(...objects) {
  const result = {};

  for (const obj of objects) {
    for (const [key, value] of Object.entries(obj)) {
      if (value && typeof value === 'object' && !Array.isArray(value)) {
        // Recursively merge nested objects
        result[key] = deepMerge(result[key] || {}, value);
      } else {
        // Overwrite primitives and arrays
        result[key] = value;
      }
    }
  }

  return result;
}

const config = await loadMergedConfig();
console.log('Merged config:', JSON.stringify(config, null, 2));
```

## XDG Base Directory

The [XDG Base Directory](https://specifications.freedesktop.org/basedir-spec/basedir-spec-latest.html) specification defines standard locations for config files:

```
~/.config/myapp/config.json     ← User-level config (XDG_CONFIG_HOME)
/etc/myapp/config.json          ← System-wide config
./.myapprc.json                 ← Project-level config (highest priority)
```

```js
// xdg-config.js — Load config from XDG-compliant paths

import { join } from 'node:path';
import { homedir } from 'node:os';
import { existsSync } from 'node:fs';
import { readFile } from 'node:fs/promises';

async function loadXDGConfig(appName) {
  // XDG_CONFIG_HOME defaults to ~/.config
  const xdgConfig = process.env.XDG_CONFIG_HOME || join(homedir(), '.config');

  const configPaths = [
    join(xdgConfig, appName, 'config.json'),   // User config
    '/etc/' + appName + '/config.json',        // System config
    join(process.cwd(), '.' + appName + 'rc.json'),  // Project config
  ];

  // Merge configs from lowest to highest priority
  let config = {};

  for (const path of configPaths) {
    if (existsSync(path)) {
      const content = JSON.parse(await readFile(path, 'utf-8'));
      config = { ...config, ...content };  // Later overrides earlier
      console.log(`Loaded config from: ${path}`);
    }
  }

  return config;
}

const config = await loadXDGConfig('myapp');
console.log('Config:', config);
```

## How It Works

### Config Search Order

```
Current directory
  ↓ search up
./myapp.config.js       ← Found? Use it
./.myapprc.json         ← Found? Use it
./package.json          ← Has "myapp" field? Use it
  ↓ search up
../myapp.config.js
../.myapprc.json
  ↓ continue to root (or stopDir)
```

The **first** config file found is used. To merge multiple files, call `explorer.search()` at each directory level manually.

### Config Precedence

```
Default values          (lowest priority)
  ↓
Config file             (overrides defaults)
  ↓
Environment variables   (overrides config file)
  ↓
CLI arguments           (highest priority)
```

## Common Mistakes

### Mistake 1: No Defaults

```js
// WRONG — if no config file exists, config.port is undefined
const result = await explorer.search();
const config = result.config;
server.listen(config.port);  // NaN!

// CORRECT — always have defaults
const config = { port: 3000, ...result?.config };
```

### Mistake 2: Committing Secrets in Config Files

```json
{
  "database": {
    "url": "postgres://admin:secret123@db.example.com/mydb"
  }
}
```

```bash
# WRONG — passwords in config files committed to git
git add .myapprc.json

# CORRECT — use environment variables for secrets
# .myapprc.json should reference env vars
{
  "database": {
    "url": "${DATABASE_URL}"
  }
}
# Or load secrets from env vars directly, not config files
```

### Mistake 3: Not Validating Config

```js
// WRONG — use config values without checking
const config = result.config;
server.listen(config.port);  // What if port is "abc" or -1?

// CORRECT — validate after loading
if (typeof config.port !== 'number' || config.port < 1 || config.port > 65535) {
  throw new Error(`Invalid port: ${config.port}`);
}
```

## Try It Yourself

### Exercise 1: Multi-Source Config

Create an app that loads config from defaults, `.myapprc.json`, environment variables, and CLI args. Print the final merged config.

### Exercise 2: Config Schema

Use Zod (Chapter 04) to validate the config after loading. Reject invalid ports, missing required fields, or wrong types.

### Exercise 3: Config Init Command

Create a CLI command `myapp init` that generates a `.myapprc.json` file with default values using interactive prompts (Chapter 20).

## Next Steps

You understand CLI configuration. For structured logging, continue to [Chapter 21: Logging & Monitoring](../../21-logging-monitoring/structured-logging/01-pino-setup.md).
