/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 02_Native_Scripting Concept: 03 Topic: Electron_Types Purpose: Electron framework type definitions Difficulty: intermediate UseCase: desktop applications, cross-platform Version: TS5.0+ Compatibility: Electron 22+ Performance: Resource usage Security: Context isolation, preload */

/**
 * Electron Types - Comprehensive Guide
 * ==============================
 * 
 * 📚 WHAT: Electron desktop application framework types
 * 💡 WHY: Build cross-platform desktop apps with web technologies
 * 🔧 HOW: Main process, renderer, preload, IPC
 */

// ============================================================================
// SECTION 1: WHAT IS ELECTRON
// ============================================================================

// Example 1.1: Basic Concept
// -------------------------------

// Electron combines Chromium and Node.js to create desktop applications
// using web technologies (HTML, CSS, JavaScript)

interface ElectronApp {
  name: string;
  version: string;
  electronVersion: string;
  chromeVersion: string;
  nodeVersion: string;
}

// ============================================================================
// SECTION 2: APP LIFECYCLE
// ============================================================================

// Example 2.1: Application Events
// -----------------------------

type ElectronAppEvent =
  | "ready"
  | "will-quit"
  | "before-quit"
  | "quit"
  | "window-all-closed"
  | "activate"
  | "session-end";

interface ElectronAppHandler {
  on: (event: ElectronAppEvent, handler: () => void) => void;
  once: (event: ElectronAppEvent, handler: () => void) => void;
}

function createElectronApp(): ElectronAppHandler {
  return {
    on: () => {},
    once: () => {}
  };
}

// ============================================================================
// SECTION 3: WINDOW MANAGEMENT
// ============================================================================

// Example 3.1: BrowserWindow Types
// -----------------------------

interface ElectronWindow {
  id: number;
  title: string;
  webContents: WebContents;
}

interface BrowserWindowOptions {
  width: number;
  height: number;
  x?: number;
  y?: number;
  minWidth?: number;
  minHeight?: number;
  frame?: boolean;
  titleBarStyle?: "default" | "hidden" | "hiddenInset";
  backgroundColor?: string;
  show?: boolean;
  resizable?: boolean;
  closable?: boolean;
  minimizable?: boolean;
  maximizable?: boolean;
}

const defaultWindowOptions: BrowserWindowOptions = {
  width: 1200,
  height: 800,
  show: true,
  resizable: true
};

// ============================================================================
// SECTION 4: WEB CONTENTS
// ============================================================================

// Example 4.1: WebContents Types
// -----------------------------

interface WebContents {
  id: number;
  url: string;
  title: string;
  isLoading: boolean;
  canGoBack: boolean;
  canGoForward: boolean;
}

interface WebContentsActions {
  loadURL: (url: string) => Promise<void>;
  loadFile: (filePath: string) => Promise<void>;
  goBack: () => void;
  goForward: () => void;
  reload: () => void;
  send: (channel: string, ...args: unknown[]) => void;
}

// ============================================================================
// SECTION 5: MENU
// ============================================================================

// Example 5.1: Menu Types
// -----------------------------

interface MenuItem {
  id?: string;
  label: string;
  type: "normal" | "separator" | "submenu" | "checkbox" | "radio";
  accelerator?: string;
  click?: () => void;
  enabled?: boolean;
  visible?: boolean;
  checked?: boolean;
}

class MenuBuilder {
  private items: MenuItem[] = [];

  addItem(item: MenuItem): this {
    this.items.push(item);
    return this;
  }

  addSeparator(): this {
    this.items.push({ type: "separator", label: "" });
    return this;
  }

  build(): MenuItem[] {
    return [...this.items];
  }
}

// ============================================================================
// SECTION 6: DIALOG
// ============================================================================

// Example 6.1: Dialog Types
// -----------------------------

interface DialogOptions {
  title?: string;
  defaultPath?: string;
  filters?: { name: string; extensions: string[] }[];
  properties?: ("openFile" | "openDirectory" | "multiSelections")[];
}

interface DialogReturn {
  canceled: boolean;
  filePaths: string[];
}

async function showOpenDialog(options: DialogOptions): Promise<DialogReturn> {
  return { canceled: false, filePaths: [] };
}

// ============================================================================
// SECTION 7: TRAY
// ============================================================================

// Example 7.1: Tray Types
// -----------------------------

interface TrayIcon {
  image: string;
  tooltip?: string;
  menu?: MenuItem[];
}

class TrayManager {
  createTray(icon: string): TrayIcon {
    return { image: icon };
  }

  setTooltip(tray: TrayIcon, tooltip: string): void {
    tray.tooltip = tooltip;
  }
}

// ============================================================================
// SECTION 8: GLOBAL SHORTCUT
// ============================================================================

// Example 8.1: Shortcut Registration
// -----------------------------

interface GlobalShortcut {
  accelerator: string;
  action: () => void;
}

class GlobalShortcutManager {
  private shortcuts: Map<string, GlobalShortcut> = new Map();

  register(accelerator: string, action: () => void): boolean {
    this.shortcuts.set(accelerator, { accelerator, action });
    return true;
  }

  unregister(accelerator: string): void {
    this.shortcuts.delete(accelerator);
  }

  isRegistered(accelerator: string): boolean {
    return this.shortcuts.has(accelerator);
  }
}

// ============================================================================
// SECTION 9: PROTOCOL HANDLER
// ============================================================================

// Example 9.1: Custom Protocol
// -----------------------------

interface ProtocolHandler {
  scheme: string;
  handler: (request: { url: string }) => void;
}

function registerProtocol(scheme: string, handler: (request: { url: string }) => void): void {
  return;
}

// ============================================================================
// SECTION 10: SHELL
// ============================================================================

// Example 10.1: Shell Operations
// -----------------------

interface ShellOperations {
  openExternal: (url: string) => Promise<void>;
  openPath: (path: string) => Promise<string>;
  showItemInFolder: (path: string) => void;
}

const shell: ShellOperations = {
  openExternal: async () => {},
  openPath: async () => "",
  showItemInFolder: () => {}
};

// ============================================================================
// SECTION 11: PERFORMANCE
// ============================================================================

// Example 11.1: Performance
// -----------------------

interface ElectronPerformance {
  memoryUsage: string;
  startupTime: string;
}

const electronPerformance: ElectronPerformance = {
  memoryUsage: "~100-300MB baseline",
  startupTime: "~1-3 seconds"
};

// ============================================================================
// SECTION 12: COMPATIBILITY
// ============================================================================

// Example 12.1: Compatibility
// -----------------------

interface ElectronCompatibility {
  platforms: string[];
  electronVersion: string;
}

const electronCompatibility: ElectronCompatibility = {
  platforms: ["Windows", "macOS", "Linux"],
  electronVersion: "22+"
};

// ============================================================================
// SECTION 13: SECURITY
// ============================================================================

// Example 13.1: Security
// -----------------------

interface ElectronSecurity {
  contextIsolation: boolean;
  nodeIntegration: boolean;
  sandbox: boolean;
}

const electronSecurity: ElectronSecurity = {
  contextIsolation: true,
  nodeIntegration: false,
  sandbox: true
};

// ============================================================================
// SECTION 14: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 02_Native_Scripting/02_Electron_Integration/02_IPC_Types.ts
// - 03_Cross_Platform/02_Electron_Apps/01_Electron_Main.ts

console.log("\n=== Electron Types Complete ===");
console.log("Next: 02_Native_Scripting/02_Electron_Integration/02_IPC_Types");