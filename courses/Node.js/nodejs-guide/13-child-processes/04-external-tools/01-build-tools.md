# External Tool Integration with Child Processes

## What You'll Learn

- Integrating build tools (Webpack, Babel)
- Linting and formatting tool integration
- Database CLI tool integration
- Development workflow automation
- Tool discovery and validation

## Build Tool Integration

```js
// lib/build-runner.js — Run build tools via child processes
import { spawn } from 'node:child_process';
import { promisify } from 'node:util';

class BuildRunner {
    constructor(cwd = process.cwd()) {
        this.cwd = cwd;
    }

    async webpack(configPath) {
        return this.run('npx', ['webpack', '--config', configPath, '--mode', 'production']);
    }

    async babel(inputDir, outputDir) {
        return this.run('npx', ['babel', inputDir, '--out-dir', outputDir]);
    }

    async typescript(tsconfigPath) {
        return this.run('npx', ['tsc', '--project', tsconfigPath]);
    }

    async eslint(paths, fix = false) {
        const args = ['eslint', ...paths];
        if (fix) args.push('--fix');
        return this.run('npx', args);
    }

    async prettier(paths, write = false) {
        const args = ['prettier', ...paths];
        if (write) args.push('--write');
        return this.run('npx', args);
    }

    async run(command, args) {
        return new Promise((resolve, reject) => {
            let stdout = '';
            let stderr = '';

            const proc = spawn(command, args, {
                cwd: this.cwd,
                stdio: ['pipe', 'pipe', 'pipe'],
            });

            proc.stdout.on('data', (data) => { stdout += data; });
            proc.stderr.on('data', (data) => { stderr += data; });

            proc.on('close', (code) => {
                resolve({ code, stdout: stdout.trim(), stderr: stderr.trim(), success: code === 0 });
            });

            proc.on('error', reject);
        });
    }
}

// Usage in a build script
const builder = new BuildRunner('./my-app');

// Lint
const lintResult = await builder.eslint(['src/**/*.js']);
if (!lintResult.success) {
    console.error('Lint errors:', lintResult.stderr);
    process.exit(1);
}

// Build
const buildResult = await builder.webpack('webpack.config.js');
if (!buildResult.success) {
    console.error('Build failed:', buildResult.stderr);
    process.exit(1);
}

console.log('Build successful!');
```

## Database CLI Integration

```js
// lib/db-tools.js — Database CLI tool integration
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

class DatabaseTools {
    constructor(config) {
        this.config = config;
    }

    // PostgreSQL pg_dump
    async pgDump(outputPath) {
        await execFileAsync('pg_dump', [
            '-h', this.config.host,
            '-p', String(this.config.port),
            '-U', this.config.user,
            '-d', this.config.database,
            '-Fc', // Custom format (compressed)
            '-f', outputPath,
        ], {
            env: { ...process.env, PGPASSWORD: this.config.password },
            timeout: 300000, // 5 minutes
        });
        return outputPath;
    }

    // PostgreSQL pg_restore
    async pgRestore(inputPath, options = {}) {
        const args = [
            '-h', this.config.host,
            '-p', String(this.config.port),
            '-U', this.config.user,
            '-d', this.config.database,
        ];
        if (options.clean) args.push('--clean');
        if (options.noOwner) args.push('--no-owner');
        args.push(inputPath);

        await execFileAsync('pg_restore', args, {
            env: { ...process.env, PGPASSWORD: this.config.password },
            timeout: 600000, // 10 minutes
        });
    }

    // MongoDB mongodump
    async mongoDump(outputPath) {
        await execFileAsync('mongodump', [
            `--uri=${this.config.uri}`,
            `--out=${outputPath}`,
        ], { timeout: 300000 });
    }

    // Redis RDB dump
    async redisDump(outputPath) {
        await execFileAsync('redis-cli', [
            '-h', this.config.host,
            '-p', String(this.config.port),
            '--rdb', outputPath,
        ], { timeout: 60000 });
    }
}

// Usage
const db = new DatabaseTools({
    host: 'localhost', port: 5432,
    user: 'postgres', password: 'secret',
    database: 'myapp',
});

await db.pgDump('./backups/myapp-2024.sql');
console.log('Backup created');
```

## Tool Discovery and Validation

```js
// lib/tool-discovery.js — Discover and validate external tools
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';

const execFileAsync = promisify(execFile);

async function checkTool(name, versionFlag = '--version') {
    try {
        const { stdout } = await execFileAsync(name, [versionFlag], { timeout: 5000 });
        return { name, available: true, version: stdout.trim().split('\n')[0] };
    } catch {
        return { name, available: false, version: null };
    }
}

async function validateEnvironment(tools) {
    const results = await Promise.all(
        tools.map(t => checkTool(t.name, t.versionFlag))
    );

    const missing = results.filter(r => !r.available);
    if (missing.length > 0) {
        console.error('Missing required tools:');
        missing.forEach(t => console.error(`  - ${t.name}`));
        throw new Error(`Missing ${missing.length} required tools`);
    }

    console.log('Environment validated:');
    results.forEach(r => console.log(`  ✓ ${r.name}: ${r.version}`));

    return results;
}

// Validate development environment
await validateEnvironment([
    { name: 'node', versionFlag: '--version' },
    { name: 'npm', versionFlag: '--version' },
    { name: 'git', versionFlag: '--version' },
    { name: 'docker', versionFlag: '--version' },
    { name: 'psql', versionFlag: '--version' },
    { name: 'redis-cli', versionFlag: '--version' },
]);
```

## Common Mistakes

- Not checking if tools exist before running
- Not handling tool-specific error formats
- Hardcoding tool paths instead of using PATH
- Not setting timeouts for long-running tools

## Try It Yourself

### Exercise 1: Build Pipeline
Create a build pipeline that runs lint → test → build sequentially.

### Exercise 2: DB Backup Script
Write a script that backs up PostgreSQL and uploads to S3.

### Exercise 3: Environment Check
Build a pre-commit hook that validates the dev environment.

## Next Steps

Continue to [Process Management](../05-process-management/01-supervision.md).
