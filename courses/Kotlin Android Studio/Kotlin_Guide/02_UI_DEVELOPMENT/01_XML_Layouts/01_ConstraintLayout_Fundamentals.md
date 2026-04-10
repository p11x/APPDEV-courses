# LEARNING OBJECTIVES

1. Understanding ConstraintLayout fundamentals
2. Creating responsive layouts with constraints
3. Using chains and guidelines
4. Implementing complex UI designs
5. Optimizing layout performance

```kotlin
package com.android.ui.constraint
```

---

## SECTION 1: CONSTRAINTLAYOUT OVERVIEW

```kotlin
/**
 * ConstraintLayout Overview
 * 
 * ConstraintLayout is the most flexible layout manager for Android.
 * It allows you to create complex layouts without nested view groups.
 */
object ConstraintLayoutOverview {
    
    const val VERSION = "2.1.4"
    
    // Basic XML structure
    const val BASIC_XML = """
<androidx.constraintlayout.widget.ConstraintLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    
    <TextView
        android:id="@+id/textView"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="Hello"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintBottom_toBottomOf="parent" />
        
</androidx.constraintlayout.widget.ConstraintLayout>
    """.trimIndent()
    
    // Key advantages
    val advantages = listOf(
        "Flat hierarchy - better performance",
        "Responsive design - adapts to screen size",
        "Visual editor support",
        "Percentage-based positioning",
        "Barrier and group support"
    )
}
```

---

## SECTION 2: BASIC CONSTRAINTS

```kotlin
/**
 * Basic Constraints
 * 
 * Constraints connect a view's edges to other views or parent.
 */
class BasicConstraints {
    
    // Position relative to parent
    const val PARENT_CONSTRAINTS = """
<!-- Center in parent -->
app:layout_constraintTop_toTopOf="parent"
app:layout_constraintBottom_toBottomOf="parent"
app:layout_constraintStart_toStartOf="parent"
app:layout_constraintEnd_toEndOf="parent"

<!-- Align to specific edge -->
app:layout_constraintTop_toTopOf="parent"
app:layout_constraintStart_toStartOf="parent"
    """.trimIndent()
    
    // Position relative to other views
    const val RELATIVE_CONSTRAINTS = """
<!-- Below another view -->
app:layout_constraintTop_toBottomOf="@id/otherView"

<!-- To the right of another view -->
app:layout_constraintStart_toEndOf="@id/otherView"

<!-- With margin -->
android:layout_marginTop="16dp"
android:layout_marginStart="8dp"
    """.trimIndent()
    
    // Percentage positioning
    const val PERCENTAGE_CONSTRAINTS = """
<!-- Center horizontally at 50% -->
app:layout_constraintHorizontal_bias="0.5"
app:layout_constraintStart_toStartOf="parent"
app:layout_constraintEnd_toEndOf="parent"

<!-- Offset at specific percentage -->
app:layout_constraintHorizontal_bias="0.3"
    """.trimIndent()
    
    // Circular positioning
    const val CIRCULAR_CONSTRAINTS = """
<!-- Position relative to another view in a circle -->
app:layout_constraintCircle="@id/centerView"
app:layout_constraintCircleRadius="100dp"
app:layout_constraintCircleAngle="45"
    """.trimIndent()
}
```

---

## SECTION 3: CHAINS AND GUIDELINES

```kotlin
/**
 * Chains and Guidelines
 * 
 * Chains link multiple views together.
 * Guidelines provide invisible anchor lines.
 */
class ChainsAndGuidelines {
    
    // Chain styles
    enum class ChainStyle {
        SPREAD,          // Default - distribute evenly
        SPREAD_INSIDE,   // First and last at edges
        PACKED,          // Pack together
        PACKED_BIASED    // Pack with bias
    }
    
    // Horizontal chain
    const val HORIZONTAL_CHAIN = """
<!-- Chain of buttons -->
<Button
    android:id="@+id/button1"
    app:layout_constraintHorizontal_chainStyle="spread" />

<Button
    android:id="@+id/button2"
    app:layout_constraintHorizontal_chainStyle="spread_inside" />

<Button
    android:id="@+id/button3"
    app:layout_constraintHorizontal_chainStyle="packed" />
    """.trimIndent()
    
    // Guidelines
    const val GUIDELINES = """
<!-- Vertical guideline at 30% -->
<androidx.constraintlayout.widget.Guideline
    android:id="@+id/guidelineV"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    android:orientation="vertical"
    app:layout_constraintGuide_percent="0.3" />

<!-- Horizontal guideline -->
<androidx.constraintlayout.widget.Guideline
    android:id="@+id/guidelineH"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    android:orientation="horizontal"
    app:layout_constraintGuide_begin="100dp" />

<!-- Usage -->
app:layout_constraintStart_toStartOf="@id/guidelineV"
app:layout_constraintTop_toTopOf="@id/guidelineH"
    """.trimIndent()
    
    // Chain with weights
    const val CHAIN_WEIGHTS = """
<!-- Chain with weights -->
<Button
    android:id="@+id/button1"
    app:layout_constraintHorizontal_weight="1" />

<Button
    android:id="@+id/button2"
    app:layout_constraintHorizontal_weight="2" />

<Button
    android:id="@+id/button3"
    app:layout_constraintHorizontal_weight="1" />
    """.trimIndent()
}
```

---

## SECTION 4: BARRIERS AND GROUPS

```kotlin
/**
 * Barriers and Groups
 * 
 * Barriers are invisible views that respect the bounds of other views.
 * Groups manage visibility of multiple views.
 */
class BarriersAndGroups {
    
    // Barrier
    const val BARRIER = """
<!-- End barrier - respects the rightmost view in the group -->
<androidx.constraintlayout.widget.Barrier
    android:id="@+id/barrierEnd"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    app:barrierDirection="end"
    app:constraint_referenced_ids="text1,text2,text3" />

<!-- Usage - Text won't overlap barrier -->
<TextView
    android:id="@+id/description"
    app:layout_constraintEnd_toStartOf="@id/barrierEnd"
    app:layout_constraintStart_toStartOf="parent" />
    """.trimIndent()
    
    // Group
    const val GROUP = """
<!-- Control visibility of multiple views -->
<androidx.constraintlayout.widget.Group
    android:id="@+id/group"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    android:visibility="visible"
    app:constraint_referenced_ids="button1,button2,button3" />

<!-- Java/Kotlin access -->
group.setVisibility(View.VISIBLE);
group.setVisibility(View.GONE);
    """.trimIndent()
    
    // Multiple barriers
    const val MULTIPLE_BARRIERS = """
<!-- Start and end barrier for dynamic content -->
<androidx.constraintlayout.widget.Barrier
    android:id="@+id/barrierStart"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    app:barrierDirection="start"
    app:constraint_referenced_ids="icon1,icon2" />

<androidx.constraintlayout.widget.Barrier
    android:id="@+id/barrierEnd"
    android:layout_width="wrap_content"
    android:layout_height="wrap_content"
    app:barrierDirection="end"
    app:constraint_referenced_ids="text1,text2" />
    """.trimIndent()
}
```

---

## SECTION 5: COMPLEX LAYOUT PATTERNS

```kotlin
/**
 * Complex Layout Patterns
 * 
 * Advanced patterns using ConstraintLayout.
 */
class ComplexLayoutPatterns {
    
    // Complex responsive card
    const val RESPONSIVE_CARD = """
<androidx.constraintlayout.widget.ConstraintLayout
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:padding="16dp">
    
    <!-- Image -->
    <ImageView
        android:id="@+id/image"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintDimensionRatio="16:9"
        app:layout_constraintWidth_max="300dp" />
    
    <!-- Title -->
    <TextView
        android:id="@+id/title"
        app:layout_constraintTop_toBottomOf="@id/image"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintWidth_max="300dp" />
    
    <!-- Description -->
    <TextView
        android:id="@+id/description"
        app:layout_constraintTop_toBottomOf="@id/title"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintWidth_max="300dp" />
        
</androidx.constraintlayout.widget.ConstraintLayout>
    """.trimIndent()
    
    // Layer for effects
    const val LAYER_EFFECTS = """
<!-- Layer for tint, rotation, scale -->
<androidx.constraintlayout.utils.widget.ImageView
    android:id="@+id/image"
    app:layout_constraintTop_toTopOf="parent"
    app:layout_constraintStart_toStartOf="parent" />

<androidx.constraintlayout.utils.widget.ImageFilterView
    android:id="@+id/filteredImage"
    app:layout_constraintTop_toBottomOf="@id/image"
    app:layout_constraintStart_toStartOf="parent"
    app:brightness="1.2"
    app:contrast="1.1"
    app:saturation="0.8" />
    """.trimIndent()
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Views not visible or incorrectly positioned**
- Solution: Check all constraints are defined, verify layout_width and layout_height, ensure no conflicting constraints

**Pitfall 2: Chain not working as expected**
- Solution: Define constraints on all views in chain, set chain style on first view, check weights are correct

**Pitfall 3: Circular dependency**
- Solution: Avoid circular references, use guidelines or barriers, simplify constraint structure

**Pitfall 4: Layout performance issues**
- Solution: Use match_constraint (0dp) sparingly, avoid complex chains, use ConstraintLayout instead of nested layouts

---

## Best Practices

1. Use match_constraint (0dp) for flexible sizing
2. Set constraints on all edges you want to position
3. Use chains for related views
4. Use guidelines for percentage positioning
5. Use barriers for dynamic content
6. Use groups for visibility control
7. Prefer dimension ratio over fixed sizes
8. Test on multiple screen sizes
9. Use chains for centering
10. Use barriers for text overflow

---

## Troubleshooting Guide

**Issue: Red lines in constraint editor**
- Steps: 1. Check constraint syntax 2. Verify referenced IDs exist 3. Remove invalid constraints

**Issue: View at wrong position**
- Steps: 1. Check all constraints are complete 2. Verify margins and biases 3. Check parent constraints

**Issue: Layout not responsive**
- Steps: 1. Use percentage constraints 2. Use max/min dimensions 3. Test different screen sizes

---

## Advanced Tips and Tricks

- **Tip 1: Use flow for virtual collections** - Dynamic number of items, Automatic wrapping

- **Tip 2: Use motion editor** - Animated transitions, Constraint set animations

- **Tip 3: Use ImageFilterView** - Crossfade images, Apply filters

- **Tip 4: Use circular reveal** - Animated visibility, Custom transitions

- **Tip 5: Optimize for performance** - Use constraint optimization, Analyze layout hierarchy

---

## EXAMPLE 1: COMPLETE LOGIN SCREEN LAYOUT

```kotlin
/**
 * Login Screen Layout Example
 * 
 * Complete ConstraintLayout example for a login screen.
 */
class LoginScreenLayout {
    
    fun getXMLLayout(): String {
        return """
<androidx.constraintlayout.widget.ConstraintLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent"
    android:padding="24dp"
    android:background="@color/background">
    
    <!-- Logo -->
    <ImageView
        android:id="@+id/logo"
        android:layout_width="120dp"
        android:layout_height="120dp"
        android:src="@drawable/logo"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintDimensionRatio="1:1" />
    
    <!-- Title -->
    <TextView
        android:id="@+id/title"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="@string/app_name"
        android:textSize="24sp"
        android:textStyle="bold"
        android:textColor="@color/text_primary"
        app:layout_constraintTop_toBottomOf="@id/logo"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginTop="16dp" />
    
    <!-- Email Input -->
    <com.google.android.material.textfield.TextInputLayout
        android:id="@+id/emailLayout"
        style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:hint="@string/email"
        app:layout_constraintTop_toBottomOf="@id/title"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginTop="32dp">
        
        <com.google.android.material.textfield.TextInputEditText
            android:id="@+id/emailInput"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:inputType="textEmailAddress"
            android:maxLines="1" />
            
    </com.google.android.material.textfield.TextInputLayout>
    
    <!-- Password Input -->
    <com.google.android.material.textfield.TextInputLayout
        android:id="@+id/passwordLayout"
        style="@style/Widget.MaterialComponents.TextInputLayout.OutlinedBox"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:hint="@string/password"
        app:passwordToggleEnabled="true"
        app:layout_constraintTop_toBottomOf="@id/emailLayout"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginTop="16dp">
        
        <com.google.android.material.textfield.TextInputEditText
            android:id="@+id/passwordInput"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:inputType="textPassword"
            android:maxLines="1" />
            
    </com.google.android.material.textfield.TextInputLayout>
    
    <!-- Login Button -->
    <Button
        android:id="@+id/loginButton"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:text="@string/login"
        android:textAllCaps="false"
        app:layout_constraintTop_toBottomOf="@id/passwordLayout"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginTop="24dp" />
    
    <!-- Forgot Password -->
    <TextView
        android:id="@+id/forgotPassword"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="@string/forgot_password"
        android:textColor="@color/link"
        app:layout_constraintTop_toBottomOf="@id/loginButton"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginTop="16dp" />
    
    <!-- Register Link -->
    <TextView
        android:id="@+id/registerLink"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:text="@string/register"
        android:textColor="@color/link"
        app:layout_constraintBottom_toBottomOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginBottom="24dp" />
        
</androidx.constraintlayout.widget.ConstraintLayout>
        """.trimIndent()
    }
    
    fun getConstraintsSummary(): Map<String, String> {
        return mapOf(
            "logo" to "Centered horizontally, below parent top",
            "title" to "Below logo, centered",
            "emailLayout" to "Below title, full width with padding",
            "passwordLayout" to "Below email, full width with padding",
            "loginButton" to "Below password, full width",
            "forgotPassword" to "Below login, centered",
            "registerLink" to "Above parent bottom, centered"
        )
    }
}
```

---

## EXAMPLE 2: COMPLEX LIST ITEM WITH CONSTRAINTLAYOUT

```kotlin
/**
 * Complex List Item Layout
 * 
 * Using ConstraintLayout for efficient list items.
 */
class ListItemLayout {
    
    fun getListItemXML(): String {
        return """
<!-- List item layout using ConstraintLayout -->
<androidx.constraintlayout.widget.ConstraintLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    xmlns:tools="http://schemas.android.com/tools"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:padding="16dp"
    android:background="?attr/selectableItemBackground">
    
    <!-- Avatar -->
    <ImageView
        android:id="@+id/avatar"
        android:layout_width="48dp"
        android:layout_height="48dp"
        android:contentDescription="@string/avatar"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintBottom_toBottomOf="parent"
        tools:src="@drawable/avatar_placeholder" />
    
    <!-- Name -->
    <TextView
        android:id="@+id/name"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:ellipsize="end"
        android:maxLines="1"
        android:textStyle="bold"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintStart_toEndOf="@id/avatar"
        app:layout_constraintEnd_toStartOf="@id/timestamp"
        app:layout_constraintVertical_chainStyle="packed"
        android:layout_marginStart="12dp"
        android:layout_marginEnd="8dp"
        tools:text="John Doe" />
    
    <!-- Timestamp -->
    <TextView
        android:id="@+id/timestamp"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content"
        android:textColor="@color/text_secondary"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        tools:text="2:30 PM" />
    
    <!-- Message preview -->
    <TextView
        android:id="@+id/message"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:ellipsize="end"
        android:maxLines="2"
        android:textColor="@color/text_secondary"
        app:layout_constraintTop_toBottomOf="@id/name"
        app:layout_constraintStart_toEndOf="@id/avatar"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintBottom_toBottomOf="parent"
        android:layout_marginStart="12dp"
        android:layout_marginTop="4dp"
        tools:text="This is a preview of the message content..." />
    
    <!-- Unread indicator -->
    <View
        android:id="@+id/unreadIndicator"
        android:layout_width="8dp"
        android:layout_height="8dp"
        android:background="@drawable/circle_indicator"
        android:visibility="gone"
        app:layout_constraintTop_toTopOf="@id/name"
        app:layout_constraintBottom_toBottomOf="@id/name"
        app:layout_constraintStart_toEndOf="@id/name"
        android:layout_marginStart="8dp" />
        
</androidx.constraintlayout.widget.ConstraintLayout>
        """.trimIndent()
    }
    
    fun getOptimizationBenefits(): List<String> {
        return listOf(
            "Single layout instead of nested LinearLayouts",
            "Better performance for large lists",
            "Easier to maintain single file",
            "More flexible positioning"
        )
    }
}
```

---

## EXAMPLE 3: RESPONSIVE PROFILE SCREEN

```kotlin
/**
 * Responsive Profile Screen
 * 
 * Using constraints for adaptive layouts.
 */
class ProfileScreenLayout {
    
    fun getResponsiveLayout(): String {
        return """
<androidx.constraintlayout.widget.ConstraintLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="match_parent">
    
    <!-- Background image -->
    <ImageView
        android:id="@+id/coverImage"
        android:layout_width="0dp"
        android:layout_height="0dp"
        android:scaleType="centerCrop"
        app:layout_constraintDimensionRatio="H,2:1"
        app:layout_constraintTop_toTopOf="parent"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent" />
    
    <!-- Profile image with circular mask -->
    <ImageView
        android:id="@+id/profileImage"
        android:layout_width="100dp"
        android:layout_height="100dp"
        app:layout_constraintTop_toBottomOf="@id/coverImage"
        app:layout_constraintBottom_toBottomOf="@id/coverImage"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        app:layout_constraintVertical_bias="1.0"
        android:translationY="-50dp" />
    
    <!-- Name -->
    <TextView
        android:id="@+id/name"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:gravity="center"
        android:textSize="22sp"
        android:textStyle="bold"
        app:layout_constraintTop_toBottomOf="@id/profileImage"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:layout_marginTop="8dp"
        android:translationY="-30dp" />
    
    <!-- Bio -->
    <TextView
        android:id="@+id/bio"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:gravity="center"
        android:paddingHorizontal="32dp"
        app:layout_constraintTop_toBottomOf="@id/name"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent"
        android:translationY="-20dp" />
    
    <!-- Stats row -->
    <LinearLayout
        android:id="@+id/statsRow"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:orientation="horizontal"
        android:padding="16dp"
        app:layout_constraintTop_toBottomOf="@id/bio"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent">
        
        <!-- Posts -->
        <LinearLayout
            android:layout_width="0dp"
            android:layout_height="wrap_content"
            android:layout_weight="1"
            android:gravity="center"
            android:orientation="vertical">
            
            <TextView
                android:id="@+id/postCount"
                android:layout_width="wrap_content"
                android:layout_height="wrap_content"
                android:textStyle="bold"
                android:textSize="18sp" />
                
            <TextView
                android:layout_width="wrap_content"
                android:layout_height="wrap_content"
                android:text="@string/posts"
                android:textColor="@color/text_secondary" />
                
        </LinearLayout>
        
        <!-- Followers -->
        <LinearLayout
            android:layout_width="0dp"
            android:layout_height="wrap_content"
            android:layout_weight="1"
            android:gravity="center"
            android:orientation="vertical">
            
            <TextView
                android:id="@+id/followerCount"
                android:layout_width="wrap_content"
                android:layout_height="wrap_content"
                android:textStyle="bold"
                android:textSize="18sp" />
                
            <TextView
                android:layout_width="wrap_content"
                android:layout_height="wrap_content"
                android:text="@string/followers"
                android:textColor="@color/text_secondary" />
                
        </LinearLayout>
        
        <!-- Following -->
        <LinearLayout
            android:layout_width="0dp"
            android:layout_height="wrap_content"
            android:layout_weight="1"
            android:gravity="center"
            android:orientation="vertical">
            
            <TextView
                android:id="@+id/followingCount"
                android:layout_width="wrap_content"
                android:layout_height="wrap_content"
                android:textStyle="bold"
                android:textSize="18sp" />
                
            <TextView
                android:layout_width="wrap_content"
                android:layout_height="wrap_content"
                android:text="@string/following"
                android:textColor="@color/text_secondary" />
                
        </LinearLayout>
        
    </LinearLayout>
    
    <!-- Action buttons -->
    <Button
        android:id="@+id/editProfile"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_margin="16dp"
        android:text="@string/edit_profile"
        app:layout_constraintTop_toBottomOf="@id/statsRow"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toStartOf="@id/shareProfile" />
    
    <Button
        android:id="@+id/shareProfile"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_margin="16dp"
        android:layout_marginStart="8dp"
        android:text="@string/share"
        app:layout_constraintTop_toBottomOf="@id/statsRow"
        app:layout_constraintStart_toEndOf="@id/editProfile"
        app:layout_constraintEnd_toEndOf="parent" />
    
    <!-- Content tabs -->
    <com.google.android.material.tabs.TabLayout
        android:id="@+id/tabs"
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        app:layout_constraintTop_toBottomOf="@id/editProfile"
        app:layout_constraintStart_toStartOf="parent"
        app:layout_constraintEnd_toEndOf="parent" />
        
</androidx.constraintlayout.widget.ConstraintLayout>
        """.trimIndent()
    }
    
    fun getAdaptiveFeatures(): List<String> {
        return listOf(
            "Dimension ratio for cover image",
            "Translation for overlapping elements",
            "Percentage-based vertical positioning",
            "Weighted stats row for equal distribution",
            "Constraint dimensions for responsive width"
        )
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**ConstraintLayout Features:**
- Basic constraints: top, bottom, start, end, relative
- Chains: spread, spread_inside, packed with bias
- Guidelines: percentage and fixed position
- Barriers: dynamic content adaptation
- Groups: collective visibility control
- Dimension ratios: aspect ratio constraints
- Circular positioning: angle and radius
- Bias control: percentage positioning

**Performance Benefits:**
- Flat hierarchy
- Single layout pass
- Less memory than nested layouts
- Optimized for complex UIs

**Responsive Design:**
- Percentage-based positioning
- Max/min dimension constraints
- Dimension ratios
- Guidelines for adaptation
- Barrier for dynamic content

**Layout Patterns:**
- Login screen with full constraints
- List item with avatar, name, message
- Profile with overlapping elements
- Responsive card designs

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/02_LinearLayout_and_RelativeLayout.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/03_RecyclerView_Implementation.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/05_Material_Design_Implementation.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md

---

## END OF CONSTRAINTLAYOUT FUNDAMENTALS GUIDE
