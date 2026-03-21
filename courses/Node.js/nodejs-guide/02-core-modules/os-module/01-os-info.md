# Getting System Information with OS Module

## What You'll Learn

- How to use the os module to get system information
- Getting CPU, memory, and platform details
- Working with user home directory
- Building system diagnostics tools

## The os Module

Node.js provides the `os` module for accessing operating system information. It's useful for:
- Building system monitoring tools
- Logging system information
- Configuring application behavior based on the environment

## Importing the os Module

```javascript
import os from 'os';
```

## Basic System Information

### Getting Platform and Architecture

```javascript
// os-basics.js - Basic OS information

import os from 'os';

console.log('=== System Information ===\n');

// Operating system name
console.log('Platform:', os.platform());
// 'win32', 'darwin', 'linux', etc.

// Operating system architecture
console.log('Architecture:', os.arch());
// 'x64', 'arm64', 'arm', etc.

// Node.js version running on this system
console.log('Node version:', process.version);
console.log('OS Release:', os.release());
```

### Getting CPU Information

```javascript
// os-cpus.js - CPU information

import os from 'os';

console.log('=== CPU Information ===\n');

// Get CPU details
const cpus = os.cpus();

console.log('Number of CPUs:', cpus.length);
console.log('\nCPU Details:');

cpus.forEach((cpu, index) => {
  console.log(`\nCPU ${index + 1}:`);
  console.log('  Model:', cpu.model);
  console.log('  Speed:', cpu.speed, 'MHz');
  
  // Times show how long CPU spent in each state
  console.log('  Times:', {
    user: cpu.times.user,
    nice: cpu.times.nice,
    sys: cpu.times.sys,
    idle: cpu.times.idle,
    irq: cpu.times.irq
  });
});
```

### Getting Memory Information

```javascript
// os-memory.js - Memory information

import os from 'os';

console.log('=== Memory Information ===\n');

// Total memory in bytes
const totalMemory = os.totalmem();
console.log('Total Memory:', formatBytes(totalMemory));

// Free memory in bytes  
const freeMemory = os.freemem();
console.log('Free Memory:', formatBytes(freeMemory));

// Used memory
const usedMemory = totalMemory - freeMemory;
console.log('Used Memory:', formatBytes(usedMemory));

// Memory usage percentage
const usagePercent = ((usedMemory / totalMemory) * 100).toFixed(2);
console.log('Usage:', usagePercent, '%');

// Helper function to format bytes
function formatBytes(bytes) {
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
  if (bytes === 0) return '0 Bytes';
  const i = Math.floor(Math.log(bytes) / Math.log(1024));
  return Math.round(bytes / Math.pow(1024, i) * 100) / 100 + ' ' + sizes[i];
}
```

### Getting User and Home Directory

```javascript
// os-user.js - User information

import os from 'os';

console.log('=== User Information ===\n');

// User's home directory
console.log('Home Directory:', os.homedir());

// User's temporary directory
console.log('Temp Directory:', os.tmpdir());

// User's default username
console.log('Username:', os.userInfo().username);

// Full user info object
const userInfo = os.userInfo();
console.log('\nFull User Info:');
console.log('  Username:', userInfo.username);
console.log('  UID:', userInfo.uid);
console.log('  GID:', userInfo.gid);
console.log('  Home:', userInfo.homedir);
console.log('  Shell:', userInfo.shell);
```

### Getting Network Interfaces

```javascript
// os-network.js - Network information

import os from 'os';

console.log('=== Network Interfaces ===\n');

// Get network interfaces
const interfaces = os.networkInterfaces();

Object.keys(interfaces).forEach((name) => {
  console.log(`\nInterface: ${name}`);
  
  interfaces[name].forEach((info) => {
    console.log('  Address:', info.address);
    console.log  ('  Netmask:', info.netmask);
    console.log  ('  MAC:', info.mac);
    console.log('  Family:', info.family);  // IPv4 or IPv6
    console.log('  Internal:', info.internal);  // true if internal
  });
});
```

## Code Example: System Dashboard

Here's a comprehensive system monitoring script:

```javascript
// system-dashboard.js - Complete system monitoring

import os from 'os';

function formatBytes(bytes) {
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
  if (bytes === 0) return '0 Bytes';
  const i = Math.floor(Math.log(bytes) / Math.log(1024));
  return (bytes / Math.pow(1024, i)).toFixed(2) + ' ' + sizes[i];
}

function getUptime() {
  const seconds = os.uptime();
  const days = Math.floor(seconds / 86400);
  const hours = Math.floor((seconds % 86400) / 3600);
  const minutes = Math.floor((seconds % 3600) / 60);
  return `${days}d ${hours}h ${minutes}m`;
}

function getCpuUsage() {
  // Get initial CPU times
  const start = os.cpus().map(cpu => cpu.times);
  
  // Wait a bit (in a real app, you'd measure over time)
  // For now, just show total
  const end = os.cpus().map(cpu => cpu.times);
  
  let totalIdle = 0;
  let totalTick = 0;
  
  end.forEach((times, i) => {
    const startTimes = start[i];
    const idle = times.idle - startTimes.idle;
    const total = (times.user - startTimes.user) + 
                  (times.nice - startTimes.nice) + 
                  (times.sys - startTimes.sys) + 
                  (times.idle - startTimes.idle) + 
                  (times.irq - startTimes.irq);
    totalIdle += idle;
    totalTick += total;
  });
  
  return {
    idle: totalIdle / end.length,
    total: totalTick / end.length
  };
}

console.log('╔══════════════════════════════════════════╗');
console.log('║         SYSTEM DASHBOARD                  ║');
console.log('╚══════════════════════════════════════════╝\n');

// System Info
console.log('📦 SYSTEM INFO');
console.log('─'.repeat(40));
console.log(`Platform:      ${os.platform()} (${os.arch()})`);
console.log(`OS Release:    ${os.release()}`);
console.log(`Node Version:  ${process.version}`);
console.log(`Uptime:        ${getUptime()}`);

// Memory Info
console.log('\n💾 MEMORY');
console.log('─'.repeat(40));
const total = os.totalmem();
const free = os.freemem();
const used = total - free;
const percent = ((used / total) * 100).toFixed(1);

console.log(`Total:    ${formatBytes(total)}`);
console.log(`Used:     ${formatBytes(used)} (${percent}%)`);
console.log(`Free:     ${formatBytes(free)}`);

// CPU Info
console.log('\n⚙️  CPU');
console.log('─'.repeat(40));
const cpus = os.cpus();
console.log(`Model:    ${cpus[0].model}`);
console.log(`Cores:    ${cpus.length}`);
console.log(`Speed:    ${cpus[0].speed} MHz`);

// User Info
console.log('\n👤 USER');
console.log('─'.repeat(40));
console.log(`Home:     ${os.homedir()}`);
console.log(`Temp:     ${os.tmpdir()}`);

// End
console.log('\n' + '═'.repeat(40));
```

## Common Mistakes

### Mistake 1: Confusing Platform Names

```javascript
// Check platform correctly
if (os.platform() === 'win32') {
  // Windows-specific code
}

// NOT 'windows' - that's not a valid platform value
```

### Mistake 2: Not Converting Bytes

Memory and time values come in specific units. Always convert for display:

```javascript
// WRONG - shows raw bytes
console.log(os.totalmem());  // 17179869184

// CORRECT - convert to human-readable format
function formatBytes(bytes) {
  return (bytes / 1024 / 1024 / 1024).toFixed(2) + ' GB';
}
console.log(formatBytes(os.totalmem()));  // 16.00 GB
```

## Try It Yourself

### Exercise 1: System Reporter
Create a script that prints a formatted report of system information.

### Exercise 2: Memory Monitor
Create a script that checks if free memory is below a threshold and prints a warning.

### Exercise 3: CPU Info
Create a script that displays detailed information about each CPU core.

## Next Steps

Now you know how to get system information. Let's explore the events module for event-driven programming. Continue to [EventEmitter Basics](../events-module/01-eventemitter.md).
