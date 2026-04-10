/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 01_WebAssembly Concept: 05 Topic: Memory_Management Purpose: Managing WASM memory buffers Difficulty: intermediate UseCase: performance-critical applications Version: TS5.0+ Compatibility: Modern browsers, Node.js 16+ Performance: Memory allocation efficiency Security: Buffer overflow prevention */

/**
 * Memory Management - Comprehensive Guide
 * =====================================
 * 
 * 📚 WHAT: Managing linear memory in WebAssembly
 * 💡 WHY: Ensure efficient memory usage and data sharing
 * 🔧 HOW: WebAssembly.Memory, allocation, deallocation
 */

// ============================================================================
// SECTION 1: WHAT IS MEMORY MANAGEMENT
// ============================================================================

// Example 1.1: Basic Concept
// -------------------------------

// WebAssembly uses linear memory - a contiguous array of bytes that can be
// read and written by both JavaScript and WASM code

interface MemoryManager {
  memory: WebAssembly.Memory;
  allocate: (size: number) => number;
  deallocate: (pointer: number) => void;
  read: (pointer: number, size: number) => ArrayBuffer;
  write: (pointer: number, data: ArrayBuffer) => void;
}

// Example 1.2: Memory Model
// ----------------------

interface MemoryModel {
  pages: number;
  pageSize: number;
  totalBytes: number;
}

const memoryModel: MemoryModel = {
  pages: 0,
  pageSize: 65536,
  totalBytes: 0
};

// ============================================================================
// SECTION 2: ALLOCATION
// ============================================================================

// Example 2.1: Basic Allocation
// -----------------------------

interface AllocationResult {
  pointer: number;
  size: number;
  aligned: boolean;
}

class WASMAllocator {
  private memory: WebAssembly.Memory;
  private freeList: number[] = [];

  constructor(memory: WebAssembly.Memory) {
    this.memory = memory;
  }

  allocate(size: number, alignment: number = 8): AllocationResult {
    const alignedSize = this.alignTo(size, alignment);
    const pointer = this.findFreeBlock(alignedSize) ?? this.sbrk(alignedSize);
    return { pointer, size: alignedSize, aligned: true };
  }

  private alignTo(size: number, alignment: number): number {
    return Math.ceil(size / alignment) * alignment;
  }

  private findFreeBlock(size: number): number | null {
    return this.freeList.find(() => true) ?? null;
  }

  private sbrk(size: number): number {
    return 0;
  }
}

// Example 2.2: Stack Allocation
// -----------------------------

function allocateStack(
  size: number,
  align: number = 8
): number {
  return Math.ceil(size / align) * align;
}

// ============================================================================
// SECTION 3: DEALLOCATION
// ============================================================================

// Example 3.1: Basic Deallocation
// -----------------------------

class WASMDeallocator {
  private freeList: Map<number, number> = new Map();

  deallocate(pointer: number, size: number): void {
    this.freeList.set(pointer, size);
  }

  isAllocated(pointer: number): boolean {
    return this.freeList.has(pointer);
  }

  getFragmentation(): number {
    return this.freeList.size;
  }
}

// Example 3.2: Memory Compaction
// -----------------------------

function compactMemory(freeList: Map<number, number>): number {
  let fragmentation = 0;
  const sorted = Array.from(freeList.entries()).sort(([a], [b]) => a - b);
  
  for (let i = 1; i < sorted.length; i++) {
    const [prevPointer, prevSize] = sorted[i - 1];
    const [currPointer] = sorted[i];
    fragmentation += currPointer - (prevPointer + prevSize);
  }
  
  return fragmentation;
}

// ============================================================================
// SECTION 4: MEMORY GROWTH
// ============================================================================

// Example 4.1: Growing Memory
// -----------------------------

class WASMMemoryGrow {
  private memory: WebAssembly.Memory;
  private currentPages: number;

  constructor(memory: WebAssembly.Memory) {
    this.memory = memory;
    this.currentPages = memory.buffer.byteLength / 65536;
  }

  grow(pages: number): boolean {
    try {
      this.memory.grow(pages);
      this.currentPages += pages;
      return true;
    } catch {
      return false;
    }
  }

  getCurrentPages(): number {
    return this.currentPages;
  }
}

// Example 4.2: Dynamic Growth Strategy
// ---------------------------------

interface GrowthStrategy {
  initialPages: number;
  growthFactor: number;
  maxPages: number;
}

const growthStrategy: GrowthStrategy = {
  initialPages: 1,
  growthFactor: 2,
  maxPages: 4096
};

// ============================================================================
// SECTION 5: DATA TRANSFER
// ============================================================================

// Example 5.1: JavaScript to WASM
// -----------------------------

function jsToWASM(
  memory: WebAssembly.Memory,
  data: string
): number {
  const encoder = new TextEncoder();
  const bytes = encoder.encode(data + '\0');
  const pointer = allocateStack(bytes.length);
  const view = new Uint8Array(memory.buffer);
  view.set(bytes, pointer);
  return pointer;
}

// Example 5.2: WASM to JavaScript
// -----------------------------

function wasmToJS(
  memory: WebAssembly.Memory,
  pointer: number
): string {
  const view = new Uint8Array(memory.buffer);
  let end = pointer;
  while (view[end] !== 0) end++;
  const bytes = view.slice(pointer, end);
  const decoder = new TextDecoder();
  return decoder.decode(bytes);
}

// ============================================================================
// SECTION 6: TYPE-SAFE OPERATIONS
// ============================================================================

// Example 6.1: Type-safe Read/Write
// -----------------------------

class TypeSafeMemory {
  private memory: WebAssembly.Memory;
  private view: DataView;

  constructor(memory: WebAssembly.Memory) {
    this.memory = memory;
    this.view = new DataView(memory.buffer);
  }

  readInt32(pointer: number): number {
    return this.view.getInt32(pointer, true);
  }

  writeInt32(pointer: number, value: number): void {
    this.view.setInt32(pointer, value, true);
  }

  readFloat64(pointer: number): number {
    return this.view.getFloat64(pointer, true);
  }

  writeFloat64(pointer: number, value: number): void {
    this.view.setFloat64(pointer, value, true);
  }
}

// ============================================================================
// SECTION 7: PERFORMANCE
// ============================================================================

// Example 7.1: Performance Considerations
// ------------------------------------

interface MemoryPerformance {
  allocationSpeed: string;
  accessSpeed: string;
  growth: string;
}

const memoryPerformance: MemoryPerformance = {
  allocationSpeed: "O(1) for simple allocator",
  accessSpeed: "Sequential access fastest",
  growth: "Page growth is expensive"
};

// ============================================================================
// SECTION 8: COMPATIBILITY
// ============================================================================

// Example 8.1: Browser Support
// --------------------------

interface MemoryCompatibility {
  browsers: string[];
  nodeVersion: string;
}

const memoryCompatibility: MemoryCompatibility = {
  browsers: ["Chrome 66+", "Firefox 78+", "Safari 15.4+", "Edge 79+"],
  nodeVersion: "16+"
};

// ============================================================================
// SECTION 9: SECURITY
// ============================================================================

// Example 9.1: Security Considerations
// -----------------------------

interface MemorySecurity {
  boundsCheck: boolean;
  overflow: string;
  sanitizers: string;
}

const memorySecurity: MemorySecurity = {
  boundsCheck: true,
  overflow: "Use bounds checking to prevent overflow",
  sanitizers: "Use ASAN/MemorySanitizer in native code"
};

// ============================================================================
// SECTION 10: TESTING
// ============================================================================

// Example 10.1: Memory Testing
// -----------------------

interface MemoryTest {
  allocation: boolean;
  deallocation: boolean;
  growth: boolean;
  dataTransfer: boolean;
}

const memoryTests: MemoryTest = {
  allocation: true,
  deallocation: true,
  growth: true,
  dataTransfer: true
};

// ============================================================================
// SECTION 11: DEBUGGING
// ============================================================================

// Example 11.1: Debugging Memory
// -----------------------

interface MemoryDebug {
  dump: (pointer: number, size: number) => string;
  verify: (pointer: number, size: number) => boolean;
  track: (operation: string, pointer: number, size: number) => void;
}

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 01_WASM_Types/02_WASM_Memory_Types.ts
// - 02_WASM_Integration/01_Loading_WASM.ts
// - 02_WASM_Integration/03_Calling_JavaScript.ts

console.log("\n=== Memory Management Complete ===");
console.log("Next: 02_WASM_Integration/03_Calling_JavaScript");