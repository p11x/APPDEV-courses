---
title: "PostCSS Setup with Bootstrap 5"
module: "Development Workflow"
difficulty: 2
estimated_time: 25 min
prerequisites:
  - NPM setup (05_02_01)
  - Sass compilation (05_02_02)
  - Basic CSS toolchain knowledge
tags:
  - postcss
  - autoprefixer
  - cssnano
  - optimization
---

# PostCSS Setup with Bootstrap 5

## Overview

PostCSS is a CSS transformation tool that processes your stylesheets through a pipeline of plugins. When paired with Bootstrap 5, PostCSS adds vendor prefixes via Autoprefixer (ensuring cross-browser compatibility), minifies CSS with cssnano, and can resolve `@import` statements with postcss-import. PostCSS operates on compiled CSS, so it typically sits after Sass in the build chain. This guide covers installing PostCSS, configuring essential plugins, integrating with your existing Sass workflow, and optimizing Bootstrap's CSS output for production.

## Basic Implementation

Install PostCSS and its core plugins:

```bash
npm install postcss postcss-cli autoprefixer cssnano --save-dev
```

Create a `postcss.config.js` configuration file:

```json
{
  "plugins": {
    "autoprefixer": {
      "overrideBrowserslist": [
        "> 1%",
        "last 2 versions",
        "not dead"
      ]
    },
    "cssnano": {
      "preset": "default"
    }
  }
}
```

Add PostCSS scripts to `package.json` to run after Sass compilation:

```json
{
  "scripts": {
    "sass:build": "sass src/scss/main.scss dist/css/main.unprocessed.css",
    "postcss:build": "postcss dist/css/main.unprocessed.css -o dist/css/main.css",
    "build:css": "npm run sass:build && npm run postcss:build"
  }
}
```

## Advanced Variations

Integrate `postcss-import` to resolve `@import` statements before other plugins process the CSS:

```bash
npm install postcss-import --save-dev
```

Configure a full PostCSS pipeline with import resolution, prefixing, and minification:

```json
{
  "plugins": {
    "postcss-import": {},
    "autoprefixer": {
      "grid": "autoplace"
    },
    "cssnano": {
      "preset": ["default", {
        "discardComments": { "removeAll": true },
        "normalizeWhitespace": true
      }]
    }
  }
}
```

For integration with Webpack or Vite, PostCSS plugins are configured through the bundler's plugin system rather than `postcss-cli`. In Webpack, place `postcss-loader` between `css-loader` and `sass-loader`. In Vite, PostCSS is auto-detected from `postcss.config.js`.

## Best Practices

1. **Always use Autoprefixer** — Bootstrap's source CSS does not include vendor prefixes; Autoprefixer adds them based on your browser targets.
2. **Configure `browserslist` in `package.json`** — share browser targets with Autoprefixer, Babel, and other tools from a single source.
3. **Place `postcss-import` first** in the plugin list — it must resolve `@import` statements before other plugins can process the full CSS.
4. **Use cssnano's `default` preset** — it provides safe optimizations; the `advanced` preset may break Bootstrap styles.
5. **Run PostCSS after Sass compilation** — PostCSS operates on CSS, not SCSS; Sass must compile first.
6. **Enable `grid: "autoplace"`** in Autoprefixer for IE11 grid support if your project requires legacy browser compatibility.
7. **Use `postcss.config.js`** (not inline) for shared configuration across CLI, Webpack, and Vite.
8. **Test vendor prefix output** — verify `-webkit-`, `-moz-`, and `-ms-` prefixes appear for properties like `flexbox` and `transform`.
9. **Minify only in production** — cssnano strips comments and whitespace; keep full CSS in development for debugging.
10. **Combine with `stylelint`** (see module 05_02_08) to lint CSS before PostCSS processes it.

## Common Pitfalls

1. **Plugin order errors** — `postcss-import` must be first, Autoprefixer before cssnano. Wrong order causes unresolved imports or stripped prefixes.
2. **Missing `browserslist` configuration** — without targets, Autoprefixer uses defaults that may not match your project requirements.
3. **Running PostCSS on SCSS files** — PostCSS expects CSS input; passing `.scss` files causes parse errors unless `postcss-scss` is installed.
4. **cssnano breaking Bootstrap** — the `advanced` preset merges shorthand properties in ways that override Bootstrap's specificity; stick to `default`.
5. **Forgetting `postcss-cli`** — without it, you cannot run PostCSS from npm scripts; the `postcss` package alone is the core library.
6. **Duplicate vendor prefixes** — if Sass already adds prefixes (via a mixin), Autoprefixer may add them again; disable Sass prefix mixins when using Autoprefixer.
7. **Conflicting with `autoprefixer` in other tools** — if your bundler already runs Autoprefixer, adding it to `postcss.config.js` causes double-processing.

## Accessibility Considerations

PostCSS plugins do not remove accessibility-related CSS. However, cssnano's whitespace removal can affect `content` properties used for screen-reader-only text in pseudo-elements. Test that `::before` and `::after` content used for accessibility (e.g., external link indicators) is preserved after minification. Autoprefixer's vendor prefix additions do not impact accessibility — they ensure styles render consistently across browsers, which benefits all users.

## Responsive Behavior

PostCSS does not modify Bootstrap's responsive breakpoints or grid system. Autoprefixer ensures that CSS Grid and Flexbox properties used in Bootstrap's responsive grid work across all targeted browsers. For projects using CSS custom properties for responsive theming, PostCSS plugins like `postcss-custom-properties` can provide fallback values for older browsers, ensuring responsive layouts degrade gracefully in legacy environments.
