/**
 * Category: PRACTICAL
 * Subcategory: DATA_PROCESSING
 * Concept: Algorithms_Implementation
 * Purpose: Common algorithms implemented in TypeScript
 * Difficulty: intermediate
 * UseCase: web, backend
 */

/**
 * Algorithms Implementation - Comprehensive Guide
 * ============================================
 * 
 * 📚 WHAT: Common algorithms and their implementations in TypeScript
 * 💡 WHY: Essential for solving complex problems efficiently
 * 🔧 HOW: Sorting, searching, graph algorithms
 */

// ============================================================================
// SECTION 1: SORTING ALGORITHMS
// ============================================================================

// Example 1.1: Bubble Sort
// -----------------------

function bubbleSort<T extends number>(arr: T[]): T[] {
  const result = [...arr];
  for (let i = 0; i < result.length; i++) {
    for (let j = 0; j < result.length - i - 1; j++) {
      if (result[j] > result[j + 1]) {
        [result[j], result[j + 1]] = [result[j + 1], result[j]];
      }
    }
  }
  return result;
}

// Example 1.2: Quick Sort
// -----------------------

function quickSort<T>(arr: T[], compare: (a: T, b: T) => number): T[] {
  if (arr.length <= 1) return arr;
  const pivot = arr[arr.length - 1];
  const left: T[] = [];
  const right: T[] = [];
  
  for (let i = 0; i < arr.length - 1; i++) {
    if (compare(arr[i], pivot) < 0) {
      left.push(arr[i]);
    } else {
      right.push(arr[i]);
    }
  }
  
  return [...quickSort(left, compare), pivot, ...quickSort(right, compare)];
}

// Example 1.3: Merge Sort
// -----------------------

function mergeSort<T>(arr: T[], compare: (a: T, b: T) => number): T[] {
  if (arr.length <= 1) return arr;
  
  const mid = Math.floor(arr.length / 2);
  const left = mergeSort(arr.slice(0, mid), compare);
  const right = mergeSort(arr.slice(mid), compare);
  
  return merge(left, right, compare);
}

function merge<T>(left: T[], right: T[], compare: (a: T, b: T) => number): T[] {
  const result: T[] = [];
  let i = 0, j = 0;
  
  while (i < left.length && j < right.length) {
    if (compare(left[i], right[j]) < 0) {
      result.push(left[i++]);
    } else {
      result.push(right[j++]);
    }
  }
  
  return [...result, ...left.slice(i), ...right.slice(j)];
}

// ============================================================================
// SECTION 2: SEARCHING ALGORITHMS
// ============================================================================

// Example 2.1: Binary Search
// -----------------------

function binarySearch<T>(arr: T[], target: T, compare: (a: T, b: T) => number): number {
  let left = 0;
  let right = arr.length - 1;
  
  while (left <= right) {
    const mid = Math.floor((left + right) / 2);
    const cmp = compare(arr[mid], target);
    
    if (cmp === 0) return mid;
    if (cmp < 0) left = mid + 1;
    else right = mid - 1;
  }
  
  return -1;
}

// Example 2.2: Linear Search
// -----------------------

function linearSearch<T>(arr: T[], target: T): number {
  for (let i = 0; i < arr.length; i++) {
    if (arr[i] === target) return i;
  }
  return -1;
}

// ============================================================================
// SECTION 3: GRAPH ALGORITHMS
// ============================================================================

// Example 3.1: Breadth-First Search
// -----------------------

interface Graph<T> {
  nodes: Map<T, T[]>;
}

function bfs<T>(graph: Graph<T>, start: T, target: T): boolean {
  const visited = new Set<T>();
  const queue: T[] = [start];
  
  while (queue.length > 0) {
    const node = queue.shift()!;
    if (node === target) return true;
    if (visited.has(node)) continue;
    
    visited.add(node);
    graph.nodes.get(node)?.forEach(neighbor => {
      if (!visited.has(neighbor)) queue.push(neighbor);
    });
  }
  
  return false;
}

// Example 3.2: Depth-First Search
// -----------------------

function dfs<T>(graph: Graph<T>, start: T, target: T, visited = new Set<T>()): boolean {
  if (start === target) return true;
  if (visited.has(start)) return false;
  
  visited.add(start);
  for (const neighbor of graph.nodes.get(start) || []) {
    if (dfs(graph, neighbor, target, visited)) return true;
  }
  
  return false;
}

console.log("\n=== Algorithms Implementation Complete ===");