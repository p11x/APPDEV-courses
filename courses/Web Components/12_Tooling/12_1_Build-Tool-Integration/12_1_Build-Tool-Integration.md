# Build Tool Integration

## OVERVIEW

Build tool integration enables efficient development workflows. This guide covers Webpack, Rollup, Vite, and custom build configurations for Web Components.

## IMPLEMENTATION DETAILS

### Webpack Configuration

```javascript
// webpack.config.js
module.exports = {
  entry: './src/index.js',
  output: {
    filename: 'bundle.js',
    path: path.resolve(__dirname, 'dist'),
    clean: true
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        use: 'babel-loader'
      },
      {
        test: /\.css$/,
        use: ['style-loader', 'css-loader']
      }
    ]
  },
  resolve: {
    extensions: ['.js']
  },
  experiments: {
    topLevelAwait: true
  }
};
```

### Rollup Configuration

```javascript
// rollup.config.js
export default {
  input: 'src/index.js',
  output: {
    format: 'es',
    dir: 'dist',
    entryFileNames: '[name].js'
  },
  plugins: [
    nodeResolve(),
    terser()
  ]
};
```

### Vite Configuration

```javascript
// vite.config.js
import { defineConfig } from 'vite';

export defineConfig({
  build: {
    lib: {
      entry: 'src/index.js',
      formats: ['es']
    },
    rollupOptions: {
      external: /^lit/
    }
  }
});
```

## NEXT STEPS

Proceed to **12_Tooling/12_2_Testing-Framework-Setup**.