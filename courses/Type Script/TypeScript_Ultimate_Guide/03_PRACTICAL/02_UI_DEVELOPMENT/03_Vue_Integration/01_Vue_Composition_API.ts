/**
 * Category: PRACTICAL
 * Subcategory: UI_DEVELOPMENT
 * Concept: Vue_Integration
 * Purpose: Vue Composition API with TypeScript
 * Difficulty: intermediate
 * UseCase: web
 */

/**
 * Vue TypeScript - Comprehensive Guide
 * ==================================
 * 
 * 📚 WHAT: Using TypeScript with Vue Composition API
 * 💡 WHERE: Reactive UI components
 * 🔧 HOW: Ref, reactive, computed types
 */

// ============================================================================
// SECTION 1: REACTIVE TYPES
// ============================================================================

// Example 1.1: Ref Types
// ---------------------------------

import { ref, reactive, computed, watch } from "vue";

// Basic ref
const count = ref(0);
const message = ref<string>("");

// Generic ref with type
const items = ref<string[]>([]);

// Ref with initial value
const user = ref<{ name: string; email: string }>({
  name: "John",
  email: "john@example.com"
});

// ============================================================================
// SECTION 2: REACTIVE TYPES
// ============================================================================

// Example 2.1: Reactive Object
// ---------------------------------

interface UserState {
  name: string;
  email: string;
  isAdmin: boolean;
}

const state = reactive<UserState>({
  name: "John",
  email: "john@example.com",
  isAdmin: false
});

// ============================================================================
// SECTION 3: COMPUTED TYPES
// ============================================================================

// Example 3.1: Computed Values
// ---------------------------------

const firstName = ref("John");
const lastName = ref("Doe");

const fullName = computed(() => {
  return `${firstName.value} ${lastName.value}`;
});

// ============================================================================
// SECTION 4: PROPS AND EMITS
// ============================================================================

// Example 4.1: Component Props
// ---------------------------------

/*
interface Props {
  title: string;
  count?: number;
}

const props = defineProps<Props>();
const emit = defineEmits<{
  (e: "update", value: number): void;
  (e: "delete", id: string): void;
}>();
*/

console.log("\n=== Vue TypeScript Complete ===");
console.log("Next: PRACTICAL/UI_DEVELOPMENT/02_Angular_Integration/01_Angular_Forms_Types.ts");