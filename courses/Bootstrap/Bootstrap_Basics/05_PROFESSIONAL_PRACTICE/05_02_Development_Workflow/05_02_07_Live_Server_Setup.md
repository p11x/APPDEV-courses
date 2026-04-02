---
title: "Live Server Setup for Bootstrap 5 Development"
module: "Development Workflow"
difficulty: 1
estimated_time: 15 min
prerequisites:
  - VS Code or Node.js installed
  - Basic project structure
tags:
  - live-server
  - browser-sync
  - hmr
  - development
---

# Live Server Setup for Bootstrap 5

## Overview

A live development server automatically reloads your browser when files change, dramatically speeding up the Bootstrap development cycle. Without one, you must manually refresh after every CSS, HTML, or JavaScript edit. This module covers two primary approaches: VS Code's Live Server extension for simple projects and BrowserSync for advanced scenarios requiring synchronized multi-device testing, CSS injection without page reload, and configurable server options. Both tools serve your project over a local HTTP server, which is required for Bootstrap's JavaScript components to function correctly (file:// protocol blocks ES module imports and some fetch operations).

## Basic Implementation

**VS Code Live Server** — install the extension and launch with one click:

```bash
# Install via VS Code Extensions panel, or from terminal:
code --install-extension ritwickdey.LiveServer
```

Open your Bootstrap project folder, right-click `index.html`, and select "Open with Live Server". The server starts at `http://127.0.0.1:5500` and auto-reloads on file changes.

**BrowserSync** — install globally or as a project devDependency:

```bash
npm install browser-sync --save-dev
```

Add BrowserSync to your `package.json` scripts:

```json
{
  "scripts": {
    "serve": "browser-sync start --server --files \"*.html, css/**/*.css, js/**/*.js\" --no-notify"
  },
  "devDependencies": {
    "browser-sync": "^3.1.0"
  }
}
```

Run `npm run serve` and BrowserSync starts a local server at `http://localhost:3000` with live reload.

## Advanced Variations

BrowserSync supports proxy mode for existing backend servers (e.g., PHP, Django) and CSS injection that updates styles without a full page reload:

```bash
browser-sync start --proxy "localhost:8080" --files "dist/**/*.css" --no-notify --inject-changes
```

Configure BrowserSync programmatically in `bs-config.js` for fine-grained control:

```json
{
  "server": {
    "baseDir": "./",
    "index": "index.html"
  },
  "files": [
    "*.html",
    "dist/css/**/*.css",
    "dist/js/**/*.js"
  ],
  "port": 3000,
  "open": false,
  "notify": false,
  "ghostMode": {
    "clicks": true,
    "scroll": true,
    "forms": true
  }
}
```

For VS Code Live Server, configure workspace settings in `.vscode/settings.json` to customize the default port, root directory, and browser:

```html
<!-- index.html served at http://127.0.0.1:5500 -->
<script src="node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
```

## Best Practices

1. **Use BrowserSync for multi-device testing** — its ghost mode syncs clicks, scrolls, and form inputs across connected browsers.
2. **Configure CSS injection** — BrowserSync can inject CSS changes without full reload, preserving scroll position and JS state.
3. **Set `open: false`** in BrowserSync config to prevent auto-opening the browser on every server start.
4. **Use `--files` to limit watched paths** — watching `node_modules` or `dist/` source maps causes excessive reloads.
5. **Choose port 3000+** to avoid conflicts with common services (Apache: 8080, React: 3000, Vite: 5173).
6. **Disable ghost mode for solo projects** — set `ghostMode: false` to avoid unexpected scroll/click mirroring.
7. **Use VS Code Live Server for simple static sites** — it requires zero configuration and works out of the box.
8. **Switch to BrowserSync when you need proxy mode** — Live Server cannot proxy to backend servers.
9. **Watch only your source files** — exclude `.git`, `node_modules`, and build output from watched paths.
10. **Use `--no-notify`** to suppress BrowserSync's browser notification overlay during development.
11. **Restart the server after changing `bs-config.js`** — configuration changes require a restart.

## Common Pitfalls

1. **Opening HTML via `file://` protocol** — Bootstrap's JavaScript components (modals, dropdowns) fail without an HTTP server; always use Live Server or BrowserSync.
2. **Watching too many files** — monitoring `node_modules` or large `dist/` folders causes high CPU usage and frequent unnecessary reloads.
3. **Port conflicts** — if port 5500 or 3000 is in use, the server fails silently or uses a random port; configure explicit ports.
4. **Live Server serving wrong root** — if your HTML is in a subdirectory, configure `liveServer.settings.root` to the correct path.
5. **BrowserSync ghost mode interfering with testing** — mirrored clicks can trigger unintended actions during form testing.
6. **CSS injection not working** — BrowserSync requires CSS files to be in watched paths; missing CSS from `--files` causes full page reload instead.
7. **VS Code Live Server not restarting after crash** — if the extension stops, reload VS Code's window (`Ctrl+Shift+P` > "Reload Window").

## Accessibility Considerations

Live reload does not affect Bootstrap's accessibility features, but frequent automatic reloads can disrupt screen reader users during testing. BrowserSync's ghost mode may cause unexpected focus changes across synced devices, confusing assistive technology. When testing accessibility, disable ghost mode and use manual refresh to maintain consistent screen reader state. Both Live Server and BrowserSync serve files over HTTP, so test accessibility features in the same protocol environment as production.

## Responsive Behavior

BrowserSync's device sync feature is valuable for testing Bootstrap's responsive behavior across multiple viewports simultaneously. Connect a phone, tablet, and desktop browser to the same BrowserSync instance — scrolling and navigation actions sync across all devices, making it easy to verify that responsive grid layouts, breakpoints, and mobile navigation (e.g., offcanvas, collapsed navbar) work correctly at every screen size.
