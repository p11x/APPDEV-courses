# Framework Deployment and Building

Comprehensive guide to building and deploying JavaScript framework applications. Covers build tools, optimization, deployment strategies, and CI/CD.

## Table of Contents

1. [Build Tools Overview](#build-tools-overview)
2. [Webpack Configuration](#webpack-configuration)
3. [Vite Configuration](#vite-configuration)
4. [Build Optimization](#build-optimization)
5. [Deployment Strategies](#deployment-strategies)
6. [CI/CD Pipeline](#cicd-pipeline)
7. [Environment Configuration](#environment-configuration)
8. [Performance Monitoring](#performance-monitoring)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)

---

## Build Tools Overview

### Build Tool Comparison

| Tool | Type | Speed | Learning Curve |
|------|------|-------|---------------|
| Webpack | Bundler | Slow | High |
| Vite | Bundler/Dev Server | Fast | Low |
| esbuild | Bundler | Very Fast | Low |
| Parcel | Bundler | Fast | Low |
| Rollup | Bundler | Medium | Medium |

---

## Webpack Configuration

### Basic Configuration

```javascript
// file: webpack.config.js
const path = require('path');

module.exports = {
  mode: 'production',
  entry: './src/index.js',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: '[name].[contenthash].js',
    clean: true,
    publicPath: '/',
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: [
              '@babel/preset-env',
              ['@babel/preset-react', { runtime: 'automatic' }],
            ],
          },
        },
      },
      {
        test: /\.css$/,
        use: ['style-loader', 'css-loader', 'postcss-loader'],
      },
      {
        test: /\.(png|jpg|gif|svg)$/,
        type: 'asset/resource',
      },
      {
        test: /\.(woff|woff2|eot|ttf|otf)$/,
        type: 'asset/resource',
      },
    ],
  },
  resolve: {
    extensions: ['.js', '.jsx'],
    alias: {
      '@': path.resolve(__dirname, 'src'),
      '@components': path.resolve(__dirname, 'src/components'),
      '@utils': path.resolve(__dirname, 'src/utils'),
      '@hooks': path.resolve(__dirname, 'src/hooks'),
    },
  },
  optimization: {
    minimize: true,
    usedExports: true,
    splitChunks: {
      chunks: 'all',
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          chunks: 'all',
        },
      },
    },
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: './public/index.html',
      inject: true,
    }),
    new MiniCssExtractPlugin({
      filename: '[name].[contenthash].css',
    }),
  ],
  devServer: {
    static: {
      directory: path.join(__dirname, 'public'),
    },
    port: 3000,
    hot: true,
    historyApiFallback: true,
  },
};
```

### Advanced Webpack Config

```javascript
// file: webpack.advanced.config.js
const path = require('path');
const { BundleAnalyzerPlugin } = require('webpack-bundle-analyzer');
const CompressionPlugin = require('compression-webpack-plugin');
const { DefinePlugin } = require('webpack');

const createConfig = (env, argv) => {
  const isProduction = argv.mode === 'production';
  const isAnalyze = process.argv.includes('--analyze');

  return {
    mode: isProduction ? 'production' : 'development',
    devtool: isProduction ? 'source-map' : 'eval-source-map',

    entry: {
      main: './src/index.js',
      admin: './src/admin.js',
    },

    output: {
      path: path.resolve(__dirname, 'dist'),
      filename: isProduction
        ? '[name].[contenthash].js'
        : '[name].js',
      chunkFilename: isProduction
        ? '[name].[contenthash].chunk.js'
        : '[name].chunk.js',
      publicPath: process.env.PUBLIC_PATH || '/',
      clean: true,
    },

    optimization: {
      minimize: isProduction,
      moduleIds: 'deterministic',
      runtimeChunk: 'single',
      splitChunks: {
        chunks: 'all',
        maxInitialRequests: 25,
        minSize: 20000,
        cacheGroups: {
          defaultVendors: {
            test: /[\\/]node_modules[\\/]/,
            priority: -10,
            name: 'vendors',
          },
          react: {
            test: /[\\/]node_modules[\\/](react|react-dom|react-router)[\\/]/,
            name: 'react',
            priority: 20,
          },
          common: {
            minChunks: 2,
            priority: -10,
            reuseExistingChunk: true,
          },
        },
      },
    },

    plugins: [
      new DefinePlugin({
        'process.env.NODE_ENV': JSON.stringify(argv.mode),
        'process.env.API_URL': JSON.stringify(process.env.API_URL),
        'process.env.VERSION': JSON.stringify(process.env.npm_package_version),
      }),

      isProduction &&
        new CompressionPlugin({
          algorithm: 'gzip',
          test: /\.(js|css|html|svg)$/,
          threshold: 10240,
          minRatio: 0.8,
        }),

      isAnalyze && new BundleAnalyzerPlugin(),
    ].filter(Boolean),

    performance: {
      hints: isProduction ? 'warning' : false,
      maxEntrypointSize: 512000,
      maxAssetSize: 512000,
    },
  };
};

module.exports = createConfig;
```

---

## Vite Configuration

### Basic Vite Setup

```javascript
// file: vite.config.js
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 3000,
    open: true,
  },
  build: {
    outDir: 'dist',
    sourcemap: true,
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom', 'react-router'],
          utils: ['lodash', 'axios'],
        },
      },
    },
  },
  css: {
    modules: {
      localsConvention: 'camelCase',
    },
  },
});
```

### Vite with PWA

```javascript
// file: vite.pwa.config.js
import { defineConfig } from 'vite';
import { VitePWA } from 'vite-plugin-pwa';

export default defineConfig({
  plugins: [
    VitePWA({
      registerType: 'autoUpdate',
      includeAssets: ['favicon.ico', 'robots.txt'],
      manifest: {
        name: 'My App',
        short_name: 'MyApp',
        description: 'My Awesome App',
        theme_color: '#ffffff',
        icons: [
          {
            src: 'pwa-192x192.png',
            sizes: '192x192',
            type: 'image/png',
          },
          {
            src: 'pwa-512x512.png',
            sizes: '512x512',
            type: 'image/png',
          },
        ],
      },
      workbox: {
        globPatterns: ['**/*.{js,css,html,ico,png,svg}'],
        runtimeCaching: [
          {
            urlPattern: /^https:\/\/api\./i,
            handler: 'NetworkFirst',
            options: {
              cacheName: 'api-cache',
              expiration: {
                maxEntries: 10,
                maxAgeSeconds: 60 * 60 * 24,
              },
            },
          },
        ],
      },
    }),
  ],
});
```

---

## Build Optimization

### Bundle Analysis

```javascript
// file: optimization/bundle-analyzer.js
const { StatsWriterPlugin } = require('webpack-stats-plugin');

const webpackConfig = {
  plugins: [
    new StatsWriterPlugin({
      fields: ['assets', 'chunks', 'modules', 'error'],
    }),
  ],
};

const analyzeBundle = async () => {
  const stats = {
    initial: [],
    async: [],
    total: 0,
  };

  const getBundleSize = (asset) => {
    return (asset.size / 1024).toFixed(2) + ' KB';
  };

  console.log('Bundle Analysis:');
  console.log('Initial chunks:', stats.initial.map(getBundleSize).join(', '));
  console.log('Async chunks:', stats.async.map(getBundleSize).join(', '));
};
```

### Code Splitting

```javascript
// file: optimization/code-splitting.js
const codeSplittingConfig = {
  optimization: {
    splitChunks: {
      chunks: 'all',
      maxInitialRequests: 30,
      maxAsyncRequests: 30,
      minSize: 20000,
      cacheGroups: {
        defaultVendors: {
          test: /[\\/]node_modules[\\/]/,
          priority: -10,
          reuseExistingChunk: true,
          name(module) {
            const packageName = module.context.match(
              /[\\/]node_modules[\\/](.*?)[\\/]/
            )[1];
            return `vendor.${packageName.replace('@', '')}`;
          },
        },
        default: {
          minChunks: 2,
          priority: -20,
          reuseExistingChunk: true,
        },
      },
    },
  },
};
```

---

## Deployment Strategies

### Static Deployment

```javascript
// file: deployment/static.js
const staticDeploymentConfig = {
  buildCommand: 'npm run build',
  outputDirectory: 'dist',
  routing: {
    type: 'static',
    rewrites: [
      { from: '/api/*', to: '/public/api/$1' },
    ],
  },
  headers: {
    'Cache-Control': 'public, max-age=31536000, immutable',
  },
  redirects: {
    '/old-page': '/new-page',
    '/docs/v1': '/docs/v2',
  },
};

const deployStatic = async (config) => {
  console.log('Building application...');
  await exec(config.buildCommand);

  console.log('Uploading to CDN...');
  await uploadDirectory(config.outputDirectory);

  console.log('Invalidating CDN cache...');
  await invalidateCache();

  console.log('Deployment complete!');
};
```

### Docker Deployment

```dockerfile
# file: Dockerfile
FROM node:20-alpine AS builder

WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:alpine

COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

```nginx.conf
# file: nginx.conf
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api/ {
        proxy_pass http://backend:3000/api/;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
    }

    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }
}
```

---

## CI/CD Pipeline

### GitHub Actions

```yaml
# file: .github/workflows/deploy.yml
name: CI/CD Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '20'
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
        
      - name: Lint
        run: npm run lint
        
      - name: Type check
        run: npm run typecheck
        
      - name: Test
        run: npm run test:coverage
        
      - name: Build
        run: npm run build

  deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '20'
          cache: 'npm'
      
      - name: Install dependencies
        run: npm ci
      
      - name: Build
        run: npm run build
      
      - name: Deploy to production
        run: |
          echo "Deploying to production..."
          # Add deployment commands here
```

### GitLab CI

```yaml
# file: .gitlab-ci.yml
stages:
  - test
  - build
  - deploy

variables:
  NODE_VERSION: "20"

test:
  stage: test
  image: node:${NODE_VERSION}
  script:
    - npm ci
    - npm run lint
    - npm run typecheck
    - npm run test:coverage
  artifacts:
    paths:
      - coverage/

build:
  stage: build
  image: node:${NODE_VERSION}
  script:
    - npm ci
    - npm run build
  artifacts:
    paths:
      - dist/

deploy:
  stage: deploy
  script:
    - echo "Deploying..."
  environment:
    name: production
  only:
    - main
```

---

## Environment Configuration

### Environment Variables

```javascript
// file: env/index.js
const getEnvironmentConfig = () => {
  const env = process.env.NODE_ENV || 'development';

  const configs = {
    development: {
      apiUrl: 'http://localhost:3000/api',
      debug: true,
      logLevel: 'debug',
      cacheDuration: 0,
    },
    staging: {
      apiUrl: 'https://staging-api.example.com/api',
      debug: false,
      logLevel: 'info',
      cacheDuration: 300000,
    },
    production: {
      apiUrl: 'https://api.example.com/api',
      debug: false,
      logLevel: 'warn',
      cacheDuration: 600000,
    },
  };

  return configs[env];
};

const environmentConfig = getEnvironmentConfig();
export default environmentConfig;
```

### Environment File Handling

```javascript
// file: env/client.js
import { createClient } from '@supabase/supabase-js';

const supabaseUrl = import.meta.env.VITE_SUPABASE_URL;
const supabaseKey = import.meta.env.VITE_SUPABASE_KEY;

export const supabase = createClient(supabaseUrl, supabaseKey);

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 30000,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
```

---

## Performance Monitoring

### Performance Tracking

```javascript
// file: monitoring/performance.js
const reportWebVitals = (onPerfEntry) => {
  if (onPerfEntry && onPerfEntry instanceof Function) {
    import('web-vitals').then(({ getCLS, getFID, getFCP, getLCP, getTTFB }) => {
      getCLS(onPerfEntry);
      getFID(onPerfEntry);
      getFCP(onPerfEntry);
      getLCP(onPerfEntry);
      getTTFB(onPerfEntry);
    });
  }
};

const analyticsReporter = (metric) => {
  console.log(metric);
  
  window.gtag?.('event', metric.name, {
    event_category: 'Web Vitals',
    event_label: metric.id,
    value: Math.round(metric.value),
    non_interaction: true,
  });
};

reportWebVitals(analyticsReporter);
```

### Error Tracking

```javascript
// file: monitoring/error-tracking.js
const setupErrorTracking = () => {
  window.addEventListener('error', (event) => {
    const error = {
      message: event.message,
      filename: event.filename,
      lineno: event.lineno,
      colno: event.colno,
      stack: event.error?.stack,
      userAgent: navigator.userAgent,
      timestamp: new Date().toISOString(),
    };

    console.error('Error captured:', error);
    
    window.gtag?.('event', 'exception', {
      description: error.message,
      fatal: false,
    });
  });

  window.addEventListener('unhandledrejection', (event) => {
    const error = {
      message: event.reason?.message || 'Unhandled Promise Rejection',
      stack: event.reason?.stack,
      timestamp: new Date().toISOString(),
    };

    console.error('Unhandled rejection:', error);
  });
};

setupErrorTracking();
```

---

## Key Takeaways

1. **Vite** offers the best developer experience
2. **Webpack** provides maximum customization
3. **Code splitting** reduces bundle sizes
4. **CDN deployment** improves performance
5. **CI/CD** automates deployment
6. **Monitoring** helps maintain performance

---

## Common Pitfalls

1. **Not optimizing production builds**
2. **Hardcoding environment values**
3. **Ignoring build errors**
4. **Not caching static assets**
5. **Missing error boundaries**
6. **Not setting up monitoring**

---

## Related Files

- [05_FRAMEWORK_ROUTING_MASTER](./05_FRAMEWORK_ROUTING_MASTER.md)
- [06_FRAMEWORK_PERFORMANCE_OPTIMIZATION](./06_FRAMEWORK_PERFORMANCE_OPTIMIZATION.md)
- [07_FRAMEWORK_TESTING_STRATEGIES](./07_FRAMEWORK_TESTING_STRATEGIES.md)
- [01_FRAMEWORK_COMPARISON_MASTER](./01_FRAMEWORK_COMPARISON_MASTER.md)