# LEARNING OBJECTIVES

1. Understanding Material Design principles
2. Implementing Material Components
3. Creating Material themes and styles
4. Using Material patterns and guidelines
5. Building accessible UI with Material

```kotlin
package com.android.ui.material
```

---

## SECTION 1: MATERIAL DESIGN OVERVIEW

```kotlin
/**
 * Material Design Overview
 * 
 * Material Design is Google's design language, providing components
 * and guidelines for modern Android apps.
 */
object MaterialDesignOverview {
    
    const val VERSION = "1.11.0"
    
    // Dependencies
    object Dependencies {
        const val MATERIAL = "com.google.android.material:material:1.11.0"
        
        const val CORE = "androidx.core:core-ktx:1.12.0"
        const val APPCOMPAT = "androidx.appcompat:appcompat:1.6.1"
    }
    
    // Design principles
    val principles = listOf(
        "Material is the metaphor",
        "Bold, graphic, intentional",
        "Motion provides meaning",
        "Surfaces have light and depth",
        "Adaptive design is universal"
    )
}
```

---

## SECTION 2: MATERIAL COMPONENTS

```kotlin
/**
 * Material Components
 * 
 * Available Material Design UI components.
 */
class MaterialComponents {
    
    // Material Button
    object Buttons {
        const val FILLED = "style=\"Widget.Material3.Button\""
        const val OUTLINED = "style=\"Widget.Material3.Button.OutlinedButton\""
        const val TEXT = "style=\"Widget.Material3.Button.TextButton\""
        const val TONAL = "style=\"Widget.Material3.Button.TonalButton\""
        const val ICON = "style=\"Widget.Material3.Button.IconButton\""
    }
    
    // Text Fields
    object TextFields {
        const val FILLED = "style=\"Widget.Material3.TextInputLayout.FilledBox\""
        const val OUTLINED = "style=\"Widget.Material3.TextInputLayout.OutlinedBox\""
        const val TEXT = "style=\"Widget.Material3.TextInputLayout.TextInputLayout\""
        
        // Error state
        const val ERROR_STATE = "app:errorEnabled=\"true\""
        
        // Password toggle
        const val PASSWORD_TOGGLE = "app:passwordToggleEnabled=\"true\""
    }
    
    // Cards
    object Cards {
        const val ELEVATED = "style=\"Widget.Material3.CardView.Elevated\""
        const val FILLED = "style=\"Widget.Material3.CardView.Filled\""
        const val OUTLINED = "style=\"Widget.Material3.CardView.Outlined\""
    }
    
    // AppBar
    object AppBar {
        const val STANDARD = "style=\"Widget.Material3.AppBarLayout\""
        const val SMALL = "app:layout_scrollFlags=\"scroll|exitUntilCollapsed\""
        const val LARGE = "style=\"Widget.Material3.LargeTopAppBar\""
    }
    
    // Bottom Navigation
    object BottomNavigation {
        const val STANDARD = "style=\"Widget.Material3.BottomNavigationView\""
        const val LABEL_MODE = "app:labelVisibilityMode=\"labeled\""
    }
    
    // FAB
    object FAB {
        const val REGULAR = "style=\"Widget.Material3.FloatingActionButton.Primary\""
        const val SMALL = "style=\"Widget.Material3.FloatingActionButton.Primary.Small\""
        const val EXTENDED = "style=\"Widget.Material3.ExtendedFloatingActionButton\""
    }
    
    // Chips
    object Chips {
        const val INPUT = "style=\"Widget.Material3.Chip.Input\""
        const val FILTER = "style=\"Widget.Material3.Chip.Filter\""
        const val SUGGESTION = "style=\"Widget.Material3.Chip.Suggestion\""
        const val ACTION = "style=\"Widget.Material3.Chip.Assist\""
    }
    
    // Dialogs
    object Dialogs {
        const val ALERT = "AlertDialog"
        const val BOTTOM_SHEET = "BottomSheetDialogFragment"
        const val FULLSCREEN = "DialogFragment with fullscreen"
    }
    
    // Snackbars
    object Snackbars {
        const val BASIC = "Snackbar.make(view, message, duration)"
        const val WITH_ACTION = "Snackbar.make(view, message, duration).setAction(action, listener)"
    }
}
```

---

## SECTION 3: MATERIAL THEMES

```kotlin
/**
 * Material Themes
 * 
 * Configuring Material Design themes.
 */
class MaterialThemes {
    
    // Theme configuration
    const val THEME_STYLES = """
<!-- themes.xml -->
<resources>
    <style name="Theme.MyApp" parent="Theme.Material3.Light.NoActionBar">
        <!-- Primary colors -->
        <item name="colorPrimary">@color/primary</item>
        <item name="colorPrimaryVariant">@color/primary_variant</item>
        <item name="colorOnPrimary">@color/on_primary</item>
        
        <!-- Secondary colors -->
        <item name="colorSecondary">@color/secondary</item>
        <item name="colorSecondaryVariant">@color/secondary_variant</item>
        <item name="colorOnSecondary">@color/on_secondary</item>
        
        <!-- Background and surface -->
        <item name="android:colorBackground">@color/background</item>
        <item name="colorSurface">@color/surface</item>
        <item name="colorOnSurface">@color/on_surface</item>
        
        <!-- Error -->
        <item name="colorError">@color/error</item>
        <item name="colorOnError">@color/on_error</item>
        
        <!-- Status bar -->
        <item name="android:statusBarColor">@color/primary</item>
        <item name="android:navigationBarColor">@color/surface</item>
        
        <!-- Shape -->
        <item name="shapeAppearanceSmallComponent">@style/ShapeAppearance.MyApp.SmallComponent</item>
        <item name="shapeAppearanceMediumComponent">@style/ShapeAppearance.MyApp.MediumComponent</item>
        <item name="shapeAppearanceLargeComponent">@style/ShapeAppearance.MyApp.LargeComponent</item>
    </style>
</resources>
    """.trimIndent()
    
    // Shape customization
    const val SHAPE_STYLES = """
<resources>
    <style name="ShapeAppearance.MyApp.SmallComponent" parent="ShapeAppearance.Material3.SmallComponent">
        <item name="cornerFamily">rounded</item>
        <item name="cornerSize">8dp</item>
    </style>
    
    <style name="ShapeAppearance.MyApp.MediumComponent" parent="ShapeAppearance.Material3.MediumComponent">
        <item name="cornerFamily">rounded</item>
        <item name="cornerSize">12dp</item>
    </style>
    
    <style name="ShapeAppearance.MyApp.LargeComponent" parent="ShapeAppearance.Material3.LargeComponent">
        <item name="cornerFamily">rounded</item>
        <item name="cornerSize">16dp</item>
    </style>
</resources>
    """.trimIndent()
    
    // Dark theme
    const val DARK_THEME = """
<resources>
    <style name="Theme.MyApp" parent="Theme.Material3.Dark.NoActionBar">
        <item name="colorPrimary">@color/primary_dark</item>
        <item name="colorPrimaryVariant">@color/primary_variant_dark</item>
        <item name="colorOnPrimary">@color/on_primary_dark</item>
        <item name="colorSecondary">@color/secondary_dark</item>
        <item name="colorOnSecondary">@color/on_secondary_dark</item>
        <item name="android:colorBackground">@color/background_dark</item>
        <item name="colorSurface">@color/surface_dark</item>
    </style>
</resources>
    """.trimIndent()
    
    // Custom colors XML
    const val COLORS_XML = """
<resources>
    <!-- Primary -->
    <color name="primary">#6200EE</color>
    <color name="primary_variant">#3700B3</color>
    <color name="on_primary">#FFFFFF</color>
    
    <!-- Secondary -->
    <color name="secondary">#03DAC6</color>
    <color name="secondary_variant">#018786</color>
    <color name="on_secondary">#000000</color>
    
    <!-- Background/Surface -->
    <color name="background">#FFFFFF</color>
    <color name="surface">#FFFFFF</color>
    <color name="on_surface">#000000</color>
    
    <!-- Error -->
    <color name="error">#B00020</color>
    <color name="on_error">#FFFFFF</color>
    
    <!-- Dark theme colors -->
    <color name="primary_dark">#BB86FC</color>
    <color name="primary_variant_dark">#3700B3</color>
    <color name="on_primary_dark">#000000</color>
    <color name="secondary_dark">#03DAC6</color>
    <color name="on_secondary_dark">#000000</color>
    <color name="background_dark">#121212</color>
    <color name="surface_dark">#121212</color>
</resources>
    """.trimIndent()
}
```

---

## SECTION 4: COMPONENT IMPLEMENTATION

```kotlin
/**
 * Component Implementation
 * 
 * Kotlin code for Material components.
 */
class ComponentImplementation {
    
    // MaterialButton
    fun createButton(context: android.content.Context): com.google.android.material.button.MaterialButton {
        return com.google.android.material.button.MaterialButton(context).apply {
            text = "Click Me"
            setOnClickListener { /* handle click */ }
            
            // Styling
            cornerRadius = 8
            iconGravity = com.google.android.material.button.MaterialButton.ICON_GRAVITY_START
            iconPadding = 8
            
            // Ripple color
            rippleColor = android.content.res.ColorStateList.valueOf(
                android.graphics.Color.parseColor("#80FFFFFF")
            )
        }
    }
    
    // TextInputLayout
    fun createTextField(context: android.content.Context): com.google.android.material.textfield.TextInputLayout {
        return com.google.android.material.textfield.TextInputLayout(context).apply {
            hint = "Email address"
            boxStrokeColor = android.content.res.ColorStateList.valueOf(
                android.graphics.Color.parseColor("#6200EE")
            )
            setErrorEnabled(true)
            
            // Add EditText
            addView(com.google.android.material.textfield.TextInputEditText(context).apply {
                inputType = android.text.InputType.TYPE_CLASS_TEXT or
                           android.text.InputType.TYPE_TEXT_VARIATION_EMAIL_ADDRESS
            })
        }
    }
    
    // CardView
    fun createCard(context: android.content.Context): com.google.android.material.card.MaterialCardView {
        return com.google.android.material.card.MaterialCardView(context).apply {
            radius = 12f
            cardElevation = 4f
            setContentPadding(16, 16, 16, 16)
            isClickable = true
            isCheckable = false
            
            setOnClickListener { /* handle click */ }
            
            // Add content
            val textView = android.widget.TextView(context).apply {
                text = "Card Content"
            }
            addView(textView)
        }
    }
    
    // FloatingActionButton
    fun createFAB(context: android.content.Context): com.google.android.material.floatingactionbutton.FloatingActionButton {
        return com.google.android.material.floatingactionbutton.FloatingActionButton(context).apply {
            setImageResource(android.R.drawable.ic_add)
            contentDescription = "Add item"
            
            setOnClickListener { /* handle click */ }
        }
    }
    
    // BottomNavigation
    fun createBottomNav(context: android.content.Context): com.google.android.material.bottomnavigation.BottomNavigationView {
        return com.google.android.material.bottomnavigation.BottomNavigationView(context).apply {
            menuInflater.inflate(R.menu.bottom_nav_menu, menu)
            
            setOnItemSelectedListener { item ->
                when (item.itemId) {
                    R.id.nav_home -> { /* navigate */ true }
                    R.id.nav_search -> { /* navigate */ true }
                    R.id.nav_profile -> { /* navigate */ true }
                    else -> false
                }
            }
            
            // Label behavior
            labelVisibilityMode = com.google.android.material.navigation.NavigationBar.LABEL_VISIBILITY_LABELED
        }
    }
    
    // Snackbar
    fun showSnackbar(view: android.view.View, message: String) {
        com.google.android.material.snackbar.Snackbar.make(view, message, com.google.android.material.snackbar.Snackbar.LENGTH_SHORT)
            .setAction("Action") {
                // Handle action
            }
            .show()
    }
    
    // Chip
    fun createChip(context: android.content.Context): com.google.android.material.chip.Chip {
        return com.google.android.material.chip.Chip(context).apply {
            text = "Chip"
            isCheckable = true
            isChecked = false
            
            setOnCheckedChangeListener { _, isChecked ->
                // Handle check change
            }
        }
    }
    
    // MaterialAlertDialog
    fun showAlertDialog(context: android.content.Context) {
        com.google.android.material.dialog.MaterialAlertDialogBuilder(context)
            .setTitle("Title")
            .setMessage("Message")
            .setPositiveButton("OK") { dialog, _ ->
                dialog.dismiss()
            }
            .setNegativeButton("Cancel") { dialog, _ ->
                dialog.dismiss()
            }
            .setNeutralButton("More") { dialog, _ ->
                // Handle
            }
            .show()
    }
}
```

---

## SECTION 5: MATERIAL PATTERNS

```kotlin
/**
 * Material Patterns
 * 
 * Common UI patterns using Material components.
 */
class MaterialPatterns {
    
    // Bottom Sheet Pattern
    class BottomSheetDialog : com.google.android.material.bottomsheet.BottomSheetDialogFragment() {
        
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View? {
            return inflater.inflate(R.layout.bottom_sheet, container, false)
        }
        
        override fun onViewCreated(view: android.view.View, savedInstanceState: android.os.Bundle?) {
            super.onViewCreated(view, savedInstanceState)
            
            // Configure behavior
            behavior.state = com.google.android.material.bottomsheet.BottomSheetBehavior.STATE_EXPANDED
            behavior.skipCollapsed = true
        }
    }
    
    // AppBar with collapsing toolbar
    fun getCollapsingAppBarLayout(): String {
        return """
<com.google.android.material.appbar.AppBarLayout
    android:id="@+id/appBarLayout"
    android:layout_width="match_parent"
    android:layout_height="200dp"
    android:fitsSystemWindows="true">
    
    <com.google.android.material.appbar.CollapsingToolbarLayout
        android:id="@+id/collapsingToolbar"
        android:layout_width="match_parent"
        android:layout_height="match_parent"
        app:layout_scrollFlags="scroll|exitUntilCollapsed"
        app:contentScrim="@color/primary"
        app:expandedTitleMarginStart="48dp"
        app:expandedTitleMarginBottom="16dp"
        app:title="Collapsing Toolbar">
        
        <ImageView
            android:id="@+id/headerImage"
            android:layout_width="match_parent"
            android:layout_height="match_parent"
            android:scaleType="centerCrop"
            android:fitsSystemWindows="true"
            app:layout_collapseMode="parallax" />
        
        <androidx.appcompat.widget.Toolbar
            android:id="@+id/toolbar"
            app:layout_collapseMode="pin" />
            
    </com.google.android.material.appbar.CollapsingToolbarLayout>
    
</com.google.android.material.appbar.AppBarLayout>
        """.trimIndent()
    }
    
    // Navigation Drawer
    fun getNavigationDrawer(): String {
        return """
<com.google.android.material.navigation.NavigationView
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/navigationView"
    android:layout_width="wrap_content"
    android:layout_height="match_parent"
    android:layout_gravity="start"
    app:headerLayout="@layout/nav_header"
    app:menu="@menu/nav_menu"
    app:itemIconTint="@color/primary"
    app:itemTextColor="@color/primary" />
        """.trimIndent()
    }
    
    // Tabs
    fun getTabLayout(): String {
        return """
<com.google.android.material.tabs.TabLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/tabLayout"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    app:tabMode="fixed"
    app:tabGravity="fill"
    app:tabIndicatorColor="@color/primary"
    app:tabSelectedTextColor="@color/primary"
    app:tabTextColor="@color/primary_variant" />
        """.trimIndent()
    }
    
    // Search Bar
    fun getSearchBar(): String {
        return """
<com.google.android.material.search.SearchBar
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:id="@+id/searchBar"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:hint="Search"
    app:backgroundTint="@color/surface_variant"
    app:strokeWidth="0dp" />
        """.trimIndent()
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Material Components not appearing**
- Solution: Add Material dependency, use Material3 theme parent, check for conflicts with AppCompat

**Pitfall 2: Theme not applying**
- Solution: Apply theme to Application class, check style inheritance, use Theme.Material3.DayNight for theming

**Pitfall 3: Custom colors not working**
- Solution: Use colorPrimary, colorOnPrimary attributes, set up color resource properly, enable materialColorThemeOverlay

**Pitfall 4: Shape appearance not working**
- Solution: Set shapeThemeOverlay in theme, use correct attribute names, check styleable definitions

---

## Best Practices

1. Use Material3 components for new projects
2. Configure colors in themes.xml
3. Use consistent corner radius
4. Enable edge-to-edge properly
5. Support dark theme
6. Follow component guidelines
7. Use proper state descriptions
8. Test accessibility
9. Keep components consistent
10. Update Material library regularly

---

## EXAMPLE 1: COMPLETE MATERIAL ACTIVITY

```kotlin
/**
 * Complete Material Activity Example
 * 
 * Full implementation of Material Design activity.
 */
class MaterialActivity : android.app.Activity() {
    
    private lateinit var binding: ActivityMaterialBinding
    private lateinit var viewModel: MaterialViewModel
    
    override fun onCreate(savedInstanceState: android.os.Bundle?) {
        super.onCreate(savedInstanceState)
        
        // Edge-to-edge
        android.view.WindowCompat.setDecorFitsSystemWindows(window, false)
        
        binding = ActivityMaterialBinding.inflate(layoutInflater)
        setContentView(binding.root)
        
        setupToolbar()
        setupBottomNavigation()
        setupFab()
        setupTextField()
        setupChips()
    }
    
    private fun setupToolbar() {
        setSupportActionBar(binding.toolbar)
        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        
        binding.toolbar.setNavigationOnClickListener {
            onBackPressedDispatcher.onBackPressed()
        }
    }
    
    private fun setupBottomNavigation() {
        binding.bottomNavigation.setOnItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_home -> { 
                    showContent("Home")
                    true 
                }
                R.id.nav_search -> { 
                    showContent("Search")
                    true 
                }
                R.id.nav_settings -> { 
                    showContent("Settings")
                    true 
                }
                else -> false
            }
        }
    }
    
    private fun setupFab() {
        binding.fab.setOnClickListener {
            showSnackbar(binding.root, "FAB clicked")
        }
    }
    
    private fun setupTextField() {
        binding.textInputLayout.setEndIconOnClickListener {
            // Clear text
            binding.textInputEditText.text?.clear()
        }
        
        binding.textInputEditText.setOnFocusChangeListener { _, hasFocus ->
            if (!hasFocus) {
                validateInput()
            }
        }
    }
    
    private fun setupChips() {
        binding.chipGroup.setOnCheckedStateChangeListener { group, checkedIds ->
            val selectedChip = checkedIds.firstOrNull()
            val message = when (selectedChip) {
                R.id.chip_android -> "Selected: Android"
                R.id.chip_ios -> "Selected: iOS"
                R.id.chip_web -> "Selected: Web"
                else -> "No selection"
            }
            showSnackbar(binding.root, message)
        }
    }
    
    private fun validateInput() {
        val input = binding.textInputEditText.text?.toString() ?: ""
        
        if (input.isEmpty()) {
            binding.textInputLayout.error = "Required"
        } else if (!android.util.Patterns.EMAIL_ADDRESS.matcher(input).matches()) {
            binding.textInputLayout.error = "Invalid email"
        } else {
            binding.textInputLayout.error = null
        }
    }
    
    private fun showContent(section: String) {
        // Show appropriate content
        println("Showing: $section")
    }
    
    private fun showSnackbar(view: android.view.View, message: String) {
        com.google.android.material.snackbar.Snackbar.make(view, message, com.google.android.material.snackbar.Snackbar.LENGTH_SHORT)
            .setAnchorView(binding.fab)
            .show()
    }
    
    // Binding placeholder
    class ActivityMaterialBinding(
        val root: android.view.View,
        val toolbar: androidx.appcompat.widget.Toolbar,
        val bottomNavigation: com.google.android.material.bottomnavigation.BottomNavigationView,
        val fab: com.google.android.material.floatingactionbutton.FloatingActionButton,
        val textInputLayout: com.google.android.material.textfield.TextInputLayout,
        val textInputEditText: com.google.android.material.textfield.TextInputEditText,
        val chipGroup: com.google.android.material.chip.ChipGroup
    ) {
        companion object {
            fun inflate(inflater: android.view.LayoutInflater, parent: android.view.ViewGroup?): ActivityMaterialBinding {
                return ActivityMaterialBinding(
                    parent!!,
                    parent.findViewById(R.id.toolbar),
                    parent.findViewById(R.id.bottom_navigation),
                    parent.findViewById(R.id.fab),
                    parent.findViewById(R.id.text_input_layout),
                    parent.findViewById(R.id.text_input_edit_text),
                    parent.findViewById(R.id.chip_group)
                )
            }
        }
    }
    
    class MaterialViewModel : androidx.lifecycle.ViewModel()
}
```

---

## EXAMPLE 2: MATERIAL DIALOG FRAGMENTS

```kotlin
/**
 * Material Dialog Fragments
 * 
 * Different dialog implementations.
 */
class MaterialDialogs {
    
    // Alert Dialog
    class AlertDialogFragment : com.google.android.material.dialog.MaterialAlertDialogFragment() {
        
        override fun onCreateDialog(savedInstanceState: android.os.Bundle?): android.app.Dialog {
            val context = requireContext()
            
            return com.google.android.material.dialog.MaterialAlertDialogBuilder(context)
                .setTitle("Delete Item")
                .setMessage("Are you sure you want to delete this item?")
                .setIcon(android.R.drawable.ic_dialog_alert)
                .setPositiveButton("Delete") { _, _ ->
                    // Delete action
                }
                .setNegativeButton("Cancel", null)
                .setNeutralButton("More Info") { _, _ ->
                    // Show more info
                }
                .create()
        }
    }
    
    // Bottom Sheet Dialog
    class OptionsBottomSheet : com.google.android.material.bottomsheet.BottomSheetDialogFragment() {
        
        private var onOptionSelected: ((String) -> Unit)? = null
        
        fun setOnOptionSelectedListener(listener: (String) -> Unit) {
            onOptionSelected = listener
        }
        
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return inflater.inflate(R.layout.bottom_sheet_options, container, false)
        }
        
        override fun onViewCreated(view: android.view.View, savedInstanceState: android.os.Bundle?) {
            super.onViewCreated(view, savedInstanceState)
            
            view.findViewById<android.view.View>(R.id.option_1).setOnClickListener {
                onOptionSelected?.invoke("Option 1")
                dismiss()
            }
            
            view.findViewById<android.view.View>(R.id.option_2).setOnClickListener {
                onOptionSelected?.invoke("Option 2")
                dismiss()
            }
            
            view.findViewById<android.view.View>(R.id.option_3).setOnClickListener {
                onOptionSelected?.invoke("Option 3")
                dismiss()
            }
        }
    }
    
    // Date Picker
    class DatePickerFragment : com.google.android.material.datepicker.MaterialDatePicker<androidx.core.util.Pair<Long, Long>>() {
        
        override fun onCreateView(
            inflater: android.view.LayoutInflater,
            container: android.view.ViewGroup?,
            savedInstanceState: android.os.Bundle?
        ): android.view.View {
            return super.onCreateView(inflater, container, savedInstanceState)
        }
        
        override fun onSelectionChanged(selection: androidx.core.util.Pair<Long, Long>?) {
            super.onSelectionChanged(selection)
            selection?.let {
                // Handle date selection
                val formatter = java.text.SimpleDateFormat("MM/dd/yyyy", java.util.Locale.getDefault())
                val startDate = formatter.format(java.util.Date(it.first))
                val endDate = formatter.format(java.util.Date(it.second))
                println("Selected: $startDate - $endDate")
            }
        }
    }
}
```

---

## EXAMPLE 3: MATERIAL DESIGN LISTS

```kotlin
/**
 * Material Design Lists
 * 
 * Material-styled list components.
 */
class MaterialLists {
    
    // Material List Item
    fun getListItemLayout(): String {
        return """
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal"
    android:padding="16dp"
    android:gravity="center_vertical"
    android:background="?attr/selectableItemBackground">
    
    <!-- Leading icon -->
    <ImageView
        android:id="@+id/leading_icon"
        android:layout_width="24dp"
        android:layout_height="24dp"
        app:tint="?attr/colorOnSurfaceVariant" />
    
    <!-- Text content -->
    <LinearLayout
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="1"
        android:layout_marginHorizontal="16dp"
        android:orientation="vertical">
        
        <TextView
            android:id="@+id/title"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:textAppearance="?attr/textAppearanceBodyLarge"
            android:textColor="?attr/colorOnSurface" />
        
        <TextView
            android:id="@+id/subtitle"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:textAppearance="?attr/textAppearanceBodyMedium"
            android:textColor="?attr/colorOnSurfaceVariant" />
            
    </LinearLayout>
    
    <!-- Trailing icon -->
    <ImageView
        android:id="@+id/trailing_icon"
        android:layout_width="24dp"
        android:layout_height="24dp"
        app:tint="?attr/colorOnSurfaceVariant" />
    
</LinearLayout>
        """.trimIndent()
    }
    
    // Switch List Item
    fun getSwitchListItem(): String {
        return """
<LinearLayout
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:app="http://schemas.android.com/apk/res-auto"
    android:layout_width="match_parent"
    android:layout_height="wrap_content"
    android:orientation="horizontal"
    android:padding="16dp"
    android:gravity="center_vertical"
    android:background="?attr/selectableItemBackground">
    
    <LinearLayout
        android:layout_width="0dp"
        android:layout_height="wrap_content"
        android:layout_weight="1"
        android:orientation="vertical"
        android:layout_marginEnd="16dp">
        
        <TextView
            android:id="@+id/title"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:textAppearance="?attr/textAppearanceBodyLarge" />
        
        <TextView
            android:id="@+id/subtitle"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:textAppearance="?attr/textAppearanceBodyMedium"
            android:textColor="?attr/colorOnSurfaceVariant" />
            
    </LinearLayout>
    
    <com.google.android.material.materialswitch.MaterialSwitch
        android:id="@+id/switch_widget"
        android:layout_width="wrap_content"
        android:layout_height="wrap_content" />
    
</LinearLayout>
        """.trimIndent()
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Material Components Available:**
- Buttons: Filled, Outlined, Text, Tonal, Icon
- Text Fields: Filled, Outlined, with error/password
- Cards: Elevated, Filled, Outlined
- AppBar: Standard, Small, Large
- Bottom Navigation: With labels
- FAB: Regular, Small, Extended
- Chips: Input, Filter, Suggestion
- Dialogs: Alert, Bottom Sheet
- Snackbar: With action

**Theme Configuration:**
- Primary/Secondary colors
- Surface/Background colors
- Error colors
- Shape appearances
- Dark theme support

**Material Patterns:**
- Collapsing AppBar
- Navigation Drawer
- Tabs
- Search Bar
- Bottom Sheets
- Date/Time Pickers

**Accessibility:**
- Proper content descriptions
- Color contrast
- Touch target sizes
- Screen reader support

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/02_LinearLayout_and_RelativeLayout.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/03_RecyclerView_Implementation.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md

---

## END OF MATERIAL DESIGN IMPLEMENTATION GUIDE
