---
title: "Figma to Bootstrap"
difficulty: 2
category: "Advanced Development"
subcategory: "Design Systems"
prerequisites:
  - Figma API
  - Figma Tokens Plugin
  - Bootstrap 5 SCSS Variables
---

## Overview

The design-to-code workflow between Figma and Bootstrap automates the transfer of design tokens, component specifications, and style definitions from design files to production code. Figma's Variables and Tokens plugins export design decisions in structured formats that transform into Bootstrap SCSS variables, eliminating manual transcription errors and keeping design and code in sync.

The workflow has three stages: designers define tokens in Figma using the Variables feature or Tokens Studio plugin, the tokens export as JSON following the DTCG format, and a build script transforms the JSON into SCSS variables that override Bootstrap's defaults. This pipeline ensures that when a designer updates a color in Figma, the next build automatically reflects the change in code.

## Basic Implementation

```js
// scripts/figma-sync.js
const fs = require('fs');

// Figma Tokens Studio exports this format
function transformFigmaTokens(figmaExport) {
  const scss = [];

  // Colors
  if (figmaExport.global?.colors) {
    Object.entries(figmaExport.global.colors).forEach(([name, token]) => {
      scss.push(`$${name}: ${token.value};`);
    });
  }

  // Map to Bootstrap variables
  const bootstrapMapping = {
    'primary': figmaExport.global?.colors?.brandPrimary?.value,
    'secondary': figmaExport.global?.colors?.brandSecondary?.value,
    'body-bg': figmaExport.global?.colors?.backgroundPrimary?.value,
    'body-color': figmaExport.global?.colors?.textPrimary?.value
  };

  Object.entries(bootstrapMapping).forEach(([bsVar, value]) => {
    if (value) scss.push(`$${bsVar}: ${value};`);
  });

  return `// Auto-generated from Figma - ${new Date().toISOString()}\n${scss.join('\n')}`;
}

// Read Figma export
const figmaTokens = JSON.parse(fs.readFileSync('figma/tokens.json', 'utf8'));
const scss = transformFigmaTokens(figmaTokens);
fs.writeFileSync('src/scss/_figma-tokens.scss', scss);
```

```json
// figma/tokens.json (Figma Tokens Studio export)
{
  "global": {
    "colors": {
      "brandPrimary": { "value": "#6366f1", "type": "color" },
      "brandSecondary": { "value": "#8b5cf6", "type": "color" },
      "backgroundPrimary": { "value": "#ffffff", "type": "color" },
      "textPrimary": { "value": "#1e293b", "type": "color" },
      "textMuted": { "value": "#64748b", "type": "color" }
    },
    "spacing": {
      "xs": { "value": "4", "type": "spacing" },
      "sm": { "value": "8", "type": "spacing" },
      "md": { "value": "16", "type": "spacing" },
      "lg": { "value": "24", "type": "spacing" },
      "xl": { "value": "32", "type": "spacing" }
    },
    "typography": {
      "fontFamily": { "value": "Inter", "type": "fontFamilies" },
      "headingSize": { "value": "24", "type": "fontSizes" },
      "bodySize": { "value": "16", "type": "fontSizes" }
    }
  }
}
```

## Advanced Variations

```js
// GitHub Action for automated Figma sync
// .github/workflows/figma-sync.yml
// name: Sync Figma Tokens
// on:
//   workflow_dispatch:
//   schedule:
//     - cron: '0 9 * * 1'
// jobs:
//   sync:
//     runs-on: ubuntu-latest
//     steps:
//       - uses: actions/checkout@v4
//       - uses: actions/setup-node@v4
//       - run: npm ci
//       - run: node scripts/figma-sync.js
//         env:
//           FIGMA_TOKEN: ${{ secrets.FIGMA_TOKEN }}
//       - name: Create PR
//         uses: peter-evans/create-pull-request@v5
//         with:
//           title: 'chore: sync Figma tokens'
//           body: 'Automated token sync from Figma'
//           branch: chore/figma-token-sync
```

## Best Practices

1. **Use Figma Variables** - Native Figma Variables are more reliable than plugin-based token management.
2. **Export as DTCG format** - Follow the Design Token Community Group spec for cross-tool compatibility.
3. **Automate the sync** - Use CI/CD to pull tokens regularly; don't rely on manual exports.
4. **Validate token names** - Ensure Figma token names match your SCSS variable naming conventions.
5. **Map to Bootstrap variables** - Create a translation layer from Figma tokens to Bootstrap's variable names.
6. **Diff before committing** - Review token changes before merging to catch unintended modifications.
7. **Keep Figma as source of truth** - Code should never define tokens that don't exist in Figma.
8. **Version the export format** - Track which Figma export format version your pipeline supports.
9. **Include component tokens** - Sync not just global tokens but component-level overrides.
10. **Document the workflow** - Designers and developers should both understand the sync process.

## Common Pitfalls

1. **Manual sync** - Forgetting to export from Figma leads to design/code divergence.
2. **Name mismatches** - Figma's camelCase vs SCSS's kebab-case creates broken references.
3. **Missing units** - Figma exports spacing as numbers; SCSS needs `px` or `rem` units.
4. **One-way sync** - Not accounting for code-side changes that need to flow back to Figma.
5. **Stale exports** - Caching old Figma exports instead of fetching fresh data.

## Accessibility Considerations

Color tokens exported from Figma should include contrast ratio annotations. The sync pipeline should validate that text/background color pairs meet WCAG AA contrast requirements.

## Responsive Behavior

Figma should define breakpoint-specific tokens for spacing and typography. The sync process should generate responsive SCSS variables or CSS custom properties.

```scss
// Generated responsive tokens from Figma
$font-size-body: 14px; // mobile default from Figma

@include media-breakpoint-up(md) {
  $font-size-body: 16px; // tablet from Figma
}

@include media-breakpoint-up(lg) {
  $font-size-body: 16px; // desktop from Figma
}
```
