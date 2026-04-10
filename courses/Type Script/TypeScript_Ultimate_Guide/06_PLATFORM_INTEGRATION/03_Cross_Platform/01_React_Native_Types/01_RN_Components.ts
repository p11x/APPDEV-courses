/**
 * Category: 06_PLATFORM_INTEGRATION Subcategory: 03_Cross_Platform Concept: 01 Topic: RN_Components Purpose: React Native component type definitions Difficulty: intermediate UseCase: mobile development, iOS/Android Version: TS5.0+ Compatibility: React Native 0.72+ Performance: Render efficiency Security: Native code safety */

/**
 * React Native Components - Comprehensive Guide
 * ===========================================
 * 
 * 📚 WHAT: React Native built-in component types
 * 💡 WHY: Build cross-platform mobile UIs
 * 🔧 HOW: View, Text, TextInput, ScrollView, etc.
 */

// ============================================================================
// SECTION 1: WHAT IS RN COMPONENT
// ============================================================================

// Example 1.1: Basic Concept
// -------------------------------

// React Native provides a set of core components that render to native
// iOS and Android views

type RNComponentType =
  | "View"
  | "Text"
  | "Image"
  | "TextInput"
  | "ScrollView"
  | "FlatList"
  | "SectionList"
  | "TouchableHighlight"
  | "TouchableOpacity"
  | "Button"
  | "Switch"
  | "ActivityIndicator"
  | "Modal";

// ============================================================================
// SECTION 2: VIEW COMPONENT
// ============================================================================

// Example 2.1: View Props
// -----------------------------

interface ViewProps {
  style?: ViewStyle;
  ref?: React.Ref<View>;
  children?: React.ReactNode;
  onLayout?: (event: LayoutEvent) => void;
  pointerEvents?: "box-none" | "none" | "box-only" | "auto";
}

interface ViewStyle {
  width?: number | string;
  height?: number | string;
  backgroundColor?: string;
  opacity?: number;
  transform?: Transform[];
  flex?: number;
  flexDirection?: "row" | "column";
  justifyContent?: FlexJustify;
  alignItems?: FlexAlign;
  padding?: number | string;
  paddingTop?: number;
  paddingBottom?: number;
  paddingLeft?: number;
  paddingRight?: number;
  margin?: number | string;
  borderWidth?: number;
  borderColor?: string;
  borderRadius?: number;
  position?: "absolute" | "relative";
  top?: number;
  bottom?: number;
  left?: number;
  right?: number;
  zIndex?: number;
}

// ============================================================================
// SECTION 3: TEXT COMPONENT
// ============================================================================

// Example 3.1: Text Props
// -----------------------------

interface TextProps {
  children?: React.ReactNode;
  style?: TextStyle;
  numberOfLines?: number;
  selectable?: boolean;
  onPress?: () => void;
}

interface TextStyle extends ViewStyle {
  color?: string;
  fontSize?: number;
  fontWeight?: FontWeight;
  fontStyle?: "normal" | "italic";
  textAlign?: "left" | "center" | "right";
  textDecorationLine?: "none" | "underline" | "line-through";
  lineHeight?: number;
}

type FontWeight = "normal" | "bold" | "100" | "200" | "300" | "400" | "500" | "600" | "700" | "800" | "900";

// ============================================================================
// SECTION 4: TEXTINPUT COMPONENT
// ============================================================================

// Example 4.1: TextInput Props
// -----------------------------

interface TextInputProps {
  value?: string;
  placeholder?: string;
  onChangeText?: (text: string) => void;
  onChange?: (event: ChangeEvent) => void;
  onSubmitEditing?: () => void;
  onFocus?: (event: FocusEvent) => void;
  onBlur?: (event: FocusEvent) => void;
  secureTextEntry?: boolean;
  autoCapitalize?: "none" | "sentences" | "words" | "characters";
  autoCorrect?: boolean;
  autoFocus?: boolean;
  keyboardType?: KeyboardType;
  returnKeyType?: ReturnKeyType;
  multiline?: boolean;
  numberOfLines?: number;
  style?: TextInputStyle;
}

type KeyboardType = 
  | "default"
  | "email-address"
  | "numeric"
  | "phone-pad"
  | "number-pad"
  | "decimal-pad"
  | "url"
  | "web-search";

interface TextInputStyle extends TextStyle {
  padding?: number;
  textAlignVertical?: "top" | "center" | "bottom";
}

// ============================================================================
// SECTION 5: IMAGE COMPONENT
// ============================================================================

// Example 5.1: Image Props
// -----------------------------

interface ImageProps {
  source: ImageSource;
  style?: ImageStyle;
  resizeMode?: ResizeMode;
  onLoad?: () => void;
  onError?: (error: ImageError) => void;
}

type ImageSource = 
  | { uri: string }
  | number
  | { uri: string; width: number; height: number };

interface ImageStyle extends ViewStyle {
  width?: number;
  height?: number;
}

type ResizeMode = "contain" | "cover" | "stretch" | "center" | "repeat";

// ============================================================================
// SECTION 6: SCROLLVIEW COMPONENT
// ============================================================================

// Example 6.1: ScrollView Props
// -----------------------------

interface ScrollViewProps {
  children?: React.ReactNode;
  contentContainerStyle?: ViewStyle;
  horizontal?: boolean;
  showsHorizontalScrollIndicator?: boolean;
  showsVerticalScrollIndicator?: boolean;
  onScroll?: (event: ScrollEvent) => void;
  onMomentumScrollEnd?: (event: ScrollEvent) => void;
  onScrollEndDrag?: (event: ScrollEvent) => void;
  pagingEnabled?: boolean;
  scrollEnabled?: boolean;
  bounces?: boolean;
  contentOffset?: Point;
}

interface ScrollEvent {
  nativeEvent: {
    contentOffset: Point;
    contentSize: Size;
    layoutMeasurement: Size;
  };
}

interface Point {
  x: number;
  y: number;
}

interface Size {
  width: number;
  height: number;
}

// ============================================================================
// SECTION 7: FLATLIST COMPONENT
// ============================================================================

// Example 7.1: FlatList Props
// -----------------------------

interface FlatListProps<T> {
  data: T[];
  renderItem: (info: ListRenderItemInfo<T>) => React.ReactElement;
  keyExtractor: (item: T, index: number) => string;
  onEndReached?: () => void;
  onEndReachedThreshold?: number;
  ListHeaderComponent?: React.ReactElement;
  ListFooterComponent?: React.ReactElement;
  ItemSeparatorComponent?: React.ReactElement;
  numColumns?: number;
}

interface ListRenderItemInfo<T> {
  item: T;
  index: number;
  separators: {
    highlight: () => void;
    unhighlight: () => void;
  };
}

// ============================================================================
// SECTION 8: TOUCHABLE COMPONENTS
// ============================================================================

// Example 8.1: Touchable Props
// -----------------------------

interface TouchableProps {
  onPress: () => void;
  onPressIn?: () => void;
  onPressOut?: () => void;
  onLongPress?: () => void;
  disabled?: boolean;
  activeOpacity?: number;
  underlayColor?: string;
}

// ============================================================================
// SECTION 9: MODAL COMPONENT
// ============================================================================

// Example 9.1: Modal Props
// -----------------------------

interface ModalProps {
  visible: boolean;
  onRequestClose?: () => void;
  onShow?: () => void;
  animationType?: "none" | "slide" | "fade";
  transparent?: boolean;
  hardwareAccelerated?: boolean;
  children?: React.ReactNode;
}

// ============================================================================
// SECTION 10: PERFORMANCE
// ============================================================================

// Example 10.1: Performance
// -----------------------

interface RNComponentPerformance {
  renderTime: string;
  listPerformance: string;
}

const componentPerformance: RNComponentPerformance = {
  renderTime: "~16ms for 60fps",
  listPerformance: "Use FlatList for virtualization"
};

// ============================================================================
// SECTION 11: COMPATIBILITY
// ============================================================================

// Example 11.1: Compatibility
// -----------------------

interface RNComponentCompatibility {
  iosVersion: string;
  androidVersion: string;
}

const componentCompatibility: RNComponentCompatibility = {
  iosVersion: "iOS 13+",
  androidVersion: "API 23+"
};

// ============================================================================
// SECTION 12: CROSS-REFERENCE
// ============================================================================

// Related topics:
// - 03_Cross_Platform/01_React_Native_Types/02_RN_Native_Modules.ts
// - 03_Cross_Platform/01_React_Native_Types/03_RN_Bridges.ts

console.log("\n=== RN Components Complete ===");
console.log("Next: 03_Cross_Platform/01_React_Native_Types/02_RN_Native_Modules");