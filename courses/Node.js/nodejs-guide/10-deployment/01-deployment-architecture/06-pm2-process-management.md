# Using PM2

## What You'll Learn

- Installing PM2
- Starting and managing apps
- Monitoring

## Installing PM2

```bash
npm install -g pm2
```

## Basic Commands

```bash
# Start app
pm2 start app.js

# List processes
pm2 list

# Stop app
pm2 stop app

# Restart
pm2 restart app

# View logs
pm2 logs

# Monitor
pm2 monit
```

## Cluster Mode

```bash
# Run in cluster mode (auto load balance)
pm2 start app.js -i 4

# Auto restart on file change
pm2 start app.js --watch
```

## Code Example

```javascript
// ecosystem.config.js - PM2 configuration

module.exports = {
  apps: [{
    name: 'my-app',
    script: './index.js',
    instances: 'max',
    env: {
      NODE_ENV: 'development'
    },
    env_production: {
      NODE_ENV: 'production'
    }
  }]
};
```

Run with:
```bash
pm2 start ecosystem.config.js
```

## Try It Yourself

### Exercise 1: Start with PM2
Start your app using PM2.

### Exercise 2: Monitor
Use PM2 monitoring commands.
