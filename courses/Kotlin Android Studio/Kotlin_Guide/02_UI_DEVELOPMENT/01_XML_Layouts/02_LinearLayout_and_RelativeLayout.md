# LEARNING OBJECTIVES

1. Understanding LinearLayout fundamentals
2. Understanding RelativeLayout positioning
3. Choosing between layout types for different scenarios
4. Implementing common UI patterns
5. Optimizing layout performance

```kotlin
package com.android.ui.layouts
```

---

## SECTION 1: LINEARLAYOUT FUNDAMENTALS

```kotlin
/**
 * LinearLayout Fundamentals
 * 
 * LinearLayout arranges children in a single direction (horizontal or vertical).
 * It is simple and efficient for many UI scenarios.
 */
object LinearLayoutFundamentals {
    
    // Vertical orientation (default)
    const val VERTICAL_LAYOUT = """
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="vertical"
    android:padding="16dp">
    
    <TextView
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Item 1" />
        
    <TextView
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Item 2" />
        
    <TextView
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Item 3" />
        
</LinearLayout>
    """.trimIndent()
    
    // Horizontal orientation
    const val HORIZONTAL_LAYOUT = """
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal"
    android:padding="16dp">
    
    <ImageView
        android:layout_width="48dp"
        android:layout_height="48dp"
        android:src="@drawable/icon" />
        
    <TextView
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="1"
        android:layout_marginStart="8dp"
        android:text="Content" />
        
    <Button
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="Action" />
        
</LinearLayout>
    """.trimIndent()
    
    // Weight for proportional sizing
    const val WEIGHT_LAYOUT = """
<!-- Equal distribution -->
<LinearLayout
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal">
    
    <TextView
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="1"
        android:text="Left" />
        
    <TextView
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="2"
        android:text="Center (larger)" />
        
    <TextView
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="1"
        android:text="Right" />
        
</LinearLayout>
    """.trimIndent()
    
    // Gravity settings
    object GravitySettings {
        const val START = "android:gravity=\"start\""
        const val CENTER = "android:gravity=\"center\""
        const val END = "android:gravity=\"end\""
        const val CENTER_HORIZONTAL = "android:gravity=\"center_horizontal\""
        const val CENTER_VERTICAL = "android:gravity=\"center_vertical\""
        const val FILL = "android:gravity=\"fill\""
        const val MULTIPLE = "android:gravity=\"center_horizontal|bottom\""
    }
    
    // Layout gravity (for children)
    object LayoutGravity {
        const val START = "android:layout_gravity=\"start\""
        const val CENTER = "android:layout_gravity=\"center\""
        const val END = "android:layout_gravity=\"end\""
        const val FILL = "android:layout_gravity=\"fill\""
    }
}
```

---

## SECTION 2: RELATIVELAYOUT FUNDAMENTALS

```kotlin
/**
 * RelativeLayout Fundamentals
 * 
 * RelativeLayout positions children relative to each other or the parent.
 * It provides flexible positioning without nesting.
 */
object RelativeLayoutFundamentals {
    
    // Basic relative positioning
    const val BASIC_RELATIVE = """
<RelativeLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    
    <!-- Align with parent -->
    <TextView
        android:id="@+id/text1"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_alignParentTop="true"
        android:layout_centerHorizontal="true"
        android:text="Top Center" />
    
    <!-- Position relative to another view -->
    <TextView
        android:id="@+id/text2"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_below="@id/text1"
        android:layout_toEndOf="@id/text1"
        android:text="Below and right" />
        
    <!-- Align with another view -->
    <Button
        android:id="@+id/button"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_below="@id/text2"
        android:layout_alignStart="@id/text2"
        android:text="Button" />
        
</RelativeLayout>
    """.trimIndent()
    
    // Relative to parent
    const val PARENT_ALIGNMENT = """
<!-- Various parent alignments -->
<RelativeLayout
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    
    <TextView
        android:layout_alignParentTop="true"
        android:layout_alignParentStart="true"
        android:text="Top Left" />
        
    <TextView
        android:layout_alignParentTop="true"
        android:layout_centerHorizontal="true"
        android:text="Top Center" />
        
    <TextView
        android:layout_alignParentTop="true"
        android:layout_alignParentEnd="true"
        android:text="Top Right" />
        
    <TextView
        android:layout_centerInParent="true"
        android:text="Center" />
        
    <TextView
        android:layout_alignParentBottom="true"
        android:layout_centerHorizontal="true"
        android:text="Bottom Center" />
        
</RelativeLayout>
    """.trimIndent()
    
    // Relative to other views
    const val VIEW_RELATIVE = """
<!-- Position relative to other views -->
<ImageView
    android:id="@+id/icon"
    android:layout_width="48dp"
    android:layout_height="48dp"
    android:src="@drawable/icon" />

<TextView
    android:id="@+id/title"
    android:layout_toEndOf="@id/icon"
    android:layout_toStartOf="@id/action"
    android:layout_alignTop="@id/icon"
    android:text="Title" />

<TextView
    android:id="@+id/subtitle"
    android:layout_toEndOf="@id/icon"
    android:layout_below="@id/title"
    android:layout_toStartOf="@id/action"
    android:text="Subtitle" />

<ImageView
    android:id="@+id/action"
    android:layout_alignParentEnd="true"
    android:layout_centerVertical="true"
    android:layout_width="24dp"
    android:layout_height="24dp" />
    """.trimIndent()
    
    // Alignment options
    object AlignmentOptions {
        // Align with parent
        val parentAlignments = listOf(
            "alignParentTop", "alignParentBottom",
            "alignParentStart", "alignParentEnd",
            "alignParentLeft", "alignParentRight",
            "centerInParent", "centerHorizontal", "centerVertical"
        )
        
        // Align with other views
        val viewAlignments = listOf(
            "alignTop", "alignBottom", "alignStart", "alignEnd",
            "alignLeft", "alignRight", "alignWithParentIfMissing"
        )
        
        // Position relative to
        val relativePositions = listOf(
            "below", "above", "toStartOf", "toEndOf",
            "toLeftOf", "toRightOf"
        )
    }
}
```

---

## SECTION 3: COMMON PATTERNS

```kotlin
/**
 * Common Layout Patterns
 * 
 * Pre-built patterns using LinearLayout and RelativeLayout.
 */
class CommonPatterns {
    
    // List item pattern with LinearLayout
    fun listItemPattern(): String {
        return """
<!-- List item with icon, text, and action -->
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal"
    android:padding="16dp"
    android:gravity="center_vertical">
    
    <ImageView
        android:id="@+id/icon"
        android:layout_width="40dp"
        android:layout_height="40dp" />
    
    <LinearLayout
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="1"
        android:orientation="vertical"
        android:layout_marginStart="12dp">
        
        <TextView
            android:id="@+id/title"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:textStyle="bold" />
        
        <TextView
            android:id="@+id/subtitle"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:textColor="@color/text_secondary" />
            
    </LinearLayout>
    
    <ImageView
        android:id="@+id/arrow"
        android:layout_width="24dp"
        android:layout_height="24dp" />
        
</LinearLayout>
        """.trimIndent()
    }
    
    // Form pattern with LinearLayout
    fun formPattern(): String {
        return """
<!-- Form layout -->
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="vertical"
    android:padding="16dp">
    
    <TextView
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Name"
        android:textStyle="bold" />
        
    <EditText
        android:id="@+id/nameInput"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:layout_marginBottom="16dp" />
        
    <TextView
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Email"
        android:textStyle="bold" />
        
    <EditText
        android:id="@+id/emailInput"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:inputType="textEmailAddress"
        android:layout_marginBottom="16dp" />
        
    <Button
        android:id="@+id/submitButton"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:text="Submit" />
        
</LinearLayout>
        """.trimIndent()
    }
    
    // Toolbar pattern with RelativeLayout
    fun toolbarPattern(): String {
        return """
<!-- Toolbar with back, title, and action -->
<RelativeLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="56dp"
    android:background="@color/primary"
    android:paddingHorizontal="8dp">
    
    <ImageButton
        android:id="@+id/backButton"
        android:layout_width="48dp"
        android:layout_height="48dp"
        android:layout_alignParentStart="true"
        android:layout_centerVertical="true"
        android:background="?attr/selectableItemBackgroundBorderless"
        android:src="@drawable/ic_back" />
    
    <TextView
        android:id="@+id/title"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_centerInParent="true"
        android:textColor="@color/white"
        android:textSize="18sp" />
    
    <ImageButton
        android:id="@+id/actionButton"
        android:layout_width="48dp"
        android:layout_height="48dp"
        android:layout_alignParentEnd="true"
        android:layout_centerVertical="true"
        android:background="?attr/selectableItemBackgroundBorderless"
        android:src="@drawable/ic_action" />
        
</RelativeLayout>
        """.trimIndent()
    }
    
    // Card pattern
    fun cardPattern(): String {
        return """
<!-- Card with image, title, description, and button -->
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="vertical"
    android:background="@color/card_background"
    android:elevation="4dp"
    android:padding="16dp">
    
    <ImageView
        android:id="@+id/image"
        android:layout_width="match_parent"
        android:layout_height="200dp"
        android:scaleType="centerCrop" />
    
    <TextView
        android:id="@+id/title"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:textSize="18sp"
        android:textStyle="bold"
        android:layout_marginTop="12dp" />
    
    <TextView
        android:id="@+id/description"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:layout_marginTop="8dp"
        android:textColor="@color/text_secondary" />
    
    <Button
        android:id="@+id/actionButton"
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:layout_marginTop="12dp" />
        
</LinearLayout>
        """.trimIndent()
    }
}
```

---

## SECTION 4: LAYOUT CHOICE GUIDE

```kotlin
/**
 * Layout Choice Guide
 * 
 * When to use LinearLayout vs RelativeLayout vs ConstraintLayout.
 */
object LayoutChoiceGuide {
    
    data class Recommendation(
        val useCase: String,
        val recommended: String,
        val reason: String
    )
    
    val recommendations = listOf(
        // LinearLayout
        Recommendation(
            "Simple vertical list of items",
            "LinearLayout",
            "Simple and efficient for uniform items"
        ),
        Recommendation(
            "Equal distribution with weights",
            "LinearLayout",
            "Built-in weight system"
        ),
        Recommendation(
            "Simple horizontal row of items",
            "LinearLayout (horizontal)",
            "Natural fit for one-dimensional layouts"
        ),
        
        // RelativeLayout
        Recommendation(
            "Positioning relative to siblings",
            "RelativeLayout",
            "No nesting needed"
        ),
        Recommendation(
            "Aligning items to edges",
            "RelativeLayout",
            "Easy edge alignment"
        ),
        Recommendation(
            "Center items in parent",
            "RelativeLayout",
            "Simple center options"
        ),
        
        // ConstraintLayout
        Recommendation(
            "Complex layouts with many constraints",
            "ConstraintLayout",
            "More flexible than RelativeLayout"
        ),
        Recommendation(
            "Responsive layouts for different screens",
            "ConstraintLayout",
            "Percentage and ratio constraints"
        ),
        Recommendation(
            "Flat hierarchy for performance",
            "ConstraintLayout",
            "Better than nested layouts"
        ),
        Recommendation(
            "Animated layout changes",
            "ConstraintLayout with MotionLayout",
            "Built-in animation support"
        )
    )
    
    fun chooseLayout(useCase: String): String {
        return recommendations.find { it.useCase == useCase }?.recommended ?: "LinearLayout"
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Nested layouts causing performance issues**
- Solution: Use ConstraintLayout instead, reduce nesting depth, use include layouts efficiently

**Pitfall 2: Weight not working as expected**
- Solution: Set layout_width="0dp" for horizontal, set layout_height="0dp" for vertical, ensure weight sum is correct

**Pitfall 3: RelativeLayout not showing items**
- Solution: Ensure at least one horizontal and vertical constraint, use alignWithParentIfMissing as fallback, check visibility of children

**Pitfall 4: Layout gravity not working**
- Solution: Use layout_gravity on child, not gravity on parent, check parent orientation, for horizontal parent, vertical gravity needs specific setup

---

## Best Practices

1. Use LinearLayout for simple one-dimensional layouts
2. Use RelativeLayout for relative positioning
3. Use ConstraintLayout for complex layouts
4. Avoid deep nesting (max 3-4 levels)
5. Use include for reusability
6. Set weights on 0dp views
7. Use dp for dimensions
8. Test layouts on multiple screen sizes
9. Use wrap_content vs match_parent appropriately
10. Consider ConstraintLayout for new projects

---

## Troubleshooting Guide

**Issue: Views not visible**
- Steps: 1. Check layout_width/layout_height 2. Verify parent has size 3. Check visibility attribute

**Issue: Views overlapping**
- Steps: 1. Check layout positioning 2. Verify gravity settings 3. Use proper layout type

**Issue: Layout not filling screen**
- Steps: 1. Check parent layout_width/height 2. Use match_parent 3. Verify weights

---

## Advanced Tips and Tricks

- **Tip 1: Use ViewStub for lazy loading** - Load on demand, Reduce initial load time

- **Tip 2: Use include for reusability** - Create common layouts, Override ids with android:id

- **Tip 3: Use merge for root** - Reduce hierarchy, Use with include

- **Tip 4: Optimize for ListView/RecyclerView** - Use view types efficiently, Avoid complex layouts in items

- **Tip 5: Use weight for equal distribution** - Efficient alternative to ConstraintLayout, Works well for simple cases

---

## EXAMPLE 1: NAVIGATION DRAWER HEADER

```kotlin
/**
 * Navigation Drawer Header Example
 * 
 * Complete layout for navigation drawer header.
 */
class NavigationDrawerHeader {
    
    fun getLayout(): String {
        return """
<RelativeLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="180dp"
    android:background="@drawable/nav_header_bg"
    android:padding="16dp">
    
    <!-- User avatar -->
    <ImageView
        android:id="@+id/userAvatar"
        android:layout_width="64dp"
        android:layout_height="64dp"
        android:layout_alignParentBottom="true"
        android:layout_marginBottom="8dp"
        android:src="@drawable/default_avatar" />
    
    <!-- User name -->
    <TextView
        android:id="@+id/userName"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_above="@id/userAvatar"
        android:textColor="@color/white"
        android:textSize="16sp"
        android:textStyle="bold"
        android:text="John Doe" />
    
    <!-- User email -->
    <TextView
        android:id="@+id/userEmail"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:layout_above="@id/userName"
        android:textColor="@color/white_alpha_70"
        android:textSize="14sp"
        android:text="john@example.com" />
    
    <!-- Settings icon -->
    <ImageView
        android:id="@+id/settingsIcon"
        android:layout_width="24dp"
        android:layout_height="24dp"
        android:layout_alignParentEnd="true"
        android:layout_alignParentTop="true"
        android:src="@drawable/ic_settings"
        android:contentDescription="@string/settings" />
        
</RelativeLayout>
        """.trimIndent()
    }
}
```

---

## EXAMPLE 2: COMPLEX FORM WITH SECTIONS

```kotlin
/**
 * Complex Form with Sections
 * 
 * Multi-section form using nested LinearLayouts.
 */
class ComplexFormLayout {
    
    fun getLayout(): String {
        return """
<ScrollView
    xmlns:android="http://schemas.android.com/apk/res/android"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    
    <LinearLayout
        android:layout_width="match_parent"
        android:layout_height="wrap_content"
        android:orientation="vertical"
        android:padding="16dp">
        
        <!-- Section 1: Personal Info -->
        <TextView
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:text="@string/personal_info"
            android:textSize="18sp"
            android:textStyle="bold"
            android:layout_marginBottom="8dp" />
        
        <LinearLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:orientation="vertical"
            android:layout_marginBottom="16dp">
            
            <com.google.android.material.textfield.TextInputLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:hint="@string/first_name"
                style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                
                <com.google.android.material.textfield.TextInputEditText
                    android:id="@+id/firstName"
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content" />
                    
            </com.google.android.material.textfield.TextInputLayout>
            
            <com.google.android.material.textfield.TextInputLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:hint="@string/last_name"
                android:layout_marginTop="8dp"
                style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                
                <com.google.android.material.textfield.TextInputEditText
                    android:id="@+id/lastName"
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content" />
                    
            </com.google.android.material.textfield.TextInputLayout>
            
        </LinearLayout>
        
        <!-- Section 2: Contact Info -->
        <TextView
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:text="@string/contact_info"
            android:textSize="18sp"
            android:textStyle="bold"
            android:layout_marginBottom="8dp" />
        
        <LinearLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:orientation="vertical"
            android:layout_marginBottom="16dp">
            
            <com.google.android.material.textfield.TextInputLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:hint="@string/email"
                style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                
                <com.google.android.material.textfield.TextInputEditText
                    android:id="@+id/email"
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content"
                    android:inputType="textEmailAddress" />
                    
            </com.google.android.material.textfield.TextInputLayout>
            
            <com.google.android.material.textfield.TextInputLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:hint="@string/phone"
                android:layout_marginTop="8dp"
                style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                
                <com.google.android.material.textfield.TextInputEditText
                    android:id="@+id/phone"
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content"
                    android:inputType="phone" />
                    
            </com.google.android.material.textfield.TextInputLayout>
            
        </LinearLayout>
        
        <!-- Section 3: Address -->
        <TextView
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:text="@string/address"
            android:textSize="18sp"
            android:textStyle="bold"
            android:layout_marginBottom="8dp" />
        
        <LinearLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:orientation="vertical">
            
            <com.google.android.material.textfield.TextInputLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:hint="@string/street_address"
                style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                
                <com.google.android.material.textfield.TextInputEditText
                    android:id="@+id/street"
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content" />
                    
            </com.google.android.material.textfield.TextInputLayout>
            
            <!-- City and State row -->
            <LinearLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:orientation="horizontal"
                android:layout_marginTop="8dp">
                
                <com.google.android.material.textfield.TextInputLayout
                    android:layout_width="0dp"
                    android:layout_height="wrap_content"
                    android:layout_weight="1"
                    android:hint="@string/city"
                    style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                    
                    <com.google.android.material.textfield.TextInputEditText
                        android:id="@+id/city"
                        android:layout_width="match_parent"
                        android:layout_height="wrap_content" />
                        
                </com.google.android.material.textfield.TextInputLayout>
                
                <com.google.android.material.textfield.TextInputLayout
                    android:layout_width="0dp"
                    android:layout_height="wrap_content"
                    android:layout_weight="1"
                    android:layout_marginStart="8dp"
                    android:hint="@string/state"
                    style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                    
                    <com.google.android.material.textfield.TextInputEditText
                        android:id="@+id/state"
                        android:layout_width="match_parent"
                        android:layout_height="wrap_content" />
                        
                </com.google.android.material.textfield.TextInputLayout>
                    
            </LinearLayout>
            
            <!-- ZIP and Country row -->
            <LinearLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:orientation="horizontal"
                android:layout_marginTop="8dp">
                
                <com.google.android.material.textfield.TextInputLayout
                    android:layout_width="0dp"
                    android:layout_height="wrap_content"
                    android:layout_weight="1"
                    android:hint="@string/zip"
                    style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                    
                    <com.google.android.material.textfield.TextInputEditText
                        android:id="@+id/zip"
                        android:layout_width="match_parent"
                        android:layout_height="wrap_content"
                        android:inputType="number" />
                        
                </com.google.android.material.textfield.TextInputLayout>
                
                <com.google.android.material.textfield.TextInputLayout
                    android:layout_width="0dp"
                    android:layout_height="wrap_content"
                    android:layout_weight="1"
                    android:layout_marginStart="8dp"
                    android:hint="@string/country"
                    style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox">
                    
                    <com.google.android.material.textfield.TextInputEditText
                        android:id="@+id/country"
                        android:layout_width="match_parent"
                        android:layout_height="wrap_content" />
                        
                </com.google.android.material.textfield.TextInputLayout>
                    
            </LinearLayout>
            
        </LinearLayout>
        
        <!-- Submit Button -->
        <Button
            android:id="@+id/submitButton"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:layout_marginTop="24dp"
            android:text="@string/submit" />
        
    </LinearLayout>
    
</ScrollView>
        """.trimIndent()
    }
}
```

---

## EXAMPLE 3: BOTTOM NAVIGATION BAR

```kotlin
/**
 * Bottom Navigation Bar Layout
 * 
 * Using LinearLayout for bottom navigation.
 */
class BottomNavigationLayout {
    
    fun getLayout(): String {
        return """
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="56dp"
    android:orientation="horizontal"
    android:background="@color/white"
    android:elevation="8dp"
    android:gravity="center">
    
    <!-- Home -->
    <LinearLayout
        android:id="@+id/navHome"
        android:layout_width="0dp"
        android:layout_height="match_parent"
        android:layout_weight="1"
        android:orientation="vertical"
        android:gravity="center"
        android:background="?attr/selectableItemBackground">
        
        <ImageView
            android:id="@+id/iconHome"
            android:layout_width="24dp"
            android:layout_height="24dp"
            android:src="@drawable/ic_home" />
        
        <TextView
            android:layout_width="wrap_content"
            android:layout_height="wrap_content"
            android:text="@string/home"
            android:textSize="12sp" />
            
    </LinearLayout>
    
    <!-- Search -->
    <LinearLayout
        android:id="@+id/navSearch"
        android:layout_width="0dp"
        android:layout_height="match_parent"
        android:layout_weight="1"
        android:orientation="vertical"
        android:gravity="center"
        android:background="?attr/selectableItemBackground">
        
        <ImageView
            android:id="@+id/iconSearch"
            android:layout_width="24dp"
            android:layout_height="24dp"
            android:src="@drawable/ic_search" />
        
        <TextView
            android:layout_width="wrap_content"
            android:layout_height="wrap_content"
            android:text="@string/search"
            android:textSize="12sp" />
            
    </LinearLayout>
    
    <!-- Add (center, larger) -->
    <LinearLayout
        android:id="@+id/navAdd"
        android:layout_width="0dp"
        android:layout_height="match_parent"
        android:layout_weight="1"
        android:orientation="vertical"
        android:gravity="center"
        android:background="?attr/selectableItemBackground">
        
        <ImageView
            android:layout_width="32dp"
            android:layout_height="32dp"
            android:src="@drawable/ic_add" />
            
    </LinearLayout>
    
    <!-- Notifications -->
    <LinearLayout
        android:id="@+id/navNotifications"
        android:layout_width="0dp"
        android:layout_height="match_parent"
        android:layout_weight="1"
        android:orientation="vertical"
        android:gravity="center"
        android:background="?attr/selectableItemBackground">
        
        <ImageView
            android:layout_width="24dp"
            android:layout_height="24dp"
            android:src="@drawable/ic_notifications" />
        
        <TextView
            android:layout_width="wrap_content"
            android:layout_height="wrap_content"
            android:text="@string/notifications"
            android:textSize="12sp" />
            
    </LinearLayout>
    
    <!-- Profile -->
    <LinearLayout
        android:id="@+id/navProfile"
        android:layout_width="0dp"
        android:layout_height="match_parent"
        android:layout_weight="1"
        android:orientation="vertical"
        android:gravity="center"
        android:background="?attr/selectableItemBackground">
        
        <ImageView
            android:layout_width="24dp"
            android:layout_height="24dp"
            android:src="@drawable/ic_profile" />
        
        <TextView
            android:layout_width="wrap_content"
            android:layout_height="wrap_content"
            android:text="@string/profile"
            android:textSize="12sp" />
            
    </LinearLayout>
    
</LinearLayout>
        """.trimIndent()
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**LinearLayout:**
- orientation: vertical/horizontal
- weight: proportional sizing
- gravity: content alignment
- layout_gravity: child alignment
- Divider, baseline aligned

**RelativeLayout:**
- alignParent*: align with parent edges
- center*: center in parent
- below, above: vertical relative
- toStartOf, toEndOf: horizontal relative
- alignTop, alignBottom: align with sibling

**Common Patterns:**
- List item: icon + text + action
- Form: vertical sections
- Toolbar: back + title + actions
- Card: image + text + button

**Performance:**
- LinearLayout: efficient for simple cases
- RelativeLayout: flexible but can nest
- ConstraintLayout: best for complex

**Recommendations:**
- Simple vertical list: LinearLayout
- Equal spacing: LinearLayout with weights
- Relative positioning: RelativeLayout
- Complex responsive: ConstraintLayout

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/03_RecyclerView_Implementation.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/04_Custom_Views_and_Components.md

---

## END OF LINEARLAYOUT AND RELATIVELAYOUT GUIDE
