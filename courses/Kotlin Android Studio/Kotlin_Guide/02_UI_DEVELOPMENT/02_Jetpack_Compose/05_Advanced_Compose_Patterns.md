# LEARNING OBJECTIVES

1. Understanding advanced Compose patterns
2. Implementing complex animations
3. Creating custom layouts
4. Using Graphics layer
5. Performance optimization

```kotlin
package com.android.compose.advanced
```

---

## SECTION 1: ANIMATIONS

```kotlin
/**
 * Animations in Compose
 * 
 * Declarative animations for modern UI.
 */
object Animations {
    
    // AnimatedVisibility
    @Composable
    fun AnimatedVisibilityExample() {
        var visible by androidx.compose.runtime.mutableStateOf(true)
        
        Column {
            androidx.compose.material3.Button(onClick = { visible = !visible }) {
                androidx.compose.material3.Text("Toggle")
            }
            
            androidx.compose.animation.AnimatedVisibility(
                visible = visible,
                enter = androidx.compose.animation.fadeIn() + 
                        androidx.compose.animation.slideInVertically(),
                exit = androidx.compose.animation.fadeOut() + 
                       androidx.compose.animation.slideOutVertically()
            ) {
                androidx.compose.material3.Text("Animated Content")
            }
        }
    }
    
    // AnimateContentSize
    @Composable
    fun AnimateContentSizeExample() {
        var expanded by androidx.compose.runtime.mutableStateOf(false)
        
        androidx.compose.foundation.clickable { expanded = !expanded }
        
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .animateContentSize()
        ) {
            androidx.compose.material3.Text(if (expanded) "Less" else "More")
            
            if (expanded) {
                androidx.compose.material3.Text("Additional content here")
            }
        }
    }
    
    // AnimateFloatAsState
    @Composable
    fun AnimateFloatExample() {
        var enabled by androidx.compose.runtime.mutableStateOf(false)
        
        val alpha by androidx.compose.animation.animateFloatAsState(
            targetValue = if (enabled) 1f else 0.3f,
            animationSpec = androidx.compose.animation.core.tween(durationMillis = 300),
            label = "alpha"
        )
        
        androidx.compose.material3.Button(
            onClick = { enabled = !enabled },
            enabled = enabled
        ) {
            androidx.compose.material3.Text("Animated Button")
        }
    }
    
    // AnimatedDP
    @Composable
    fun AnimatedSize() {
        var small by androidx.compose.runtime.mutableStateOf(true)
        
        val size by androidx.compose.animation.animateDpAsState(
            targetValue = if (small) 50.dp else 100.dp,
            animationSpec = androidx.compose.animation.core.spring(
                dampingRatio = androidx.compose.animation.core.Spring.DampingRatioMediumBouncy
            ),
            label = "size"
        )
        
        androidx.compose.foundation.layout.Box(
            modifier = Modifier
                .size(size)
                .background(androidx.compose.material3.MaterialTheme.colorScheme.primary)
                .clickable { small = !small }
        )
    }
    
    // Crossfade
    @Composable
    fun CrossfadeExample() {
        var screen by androidx.compose.runtime.mutableStateOf("home")
        
        androidx.compose.animation.Crossfade(
            targetState = screen,
            label = "screen"
        ) { target ->
            when (target) {
                "home" -> androidx.compose.material3.Text("Home")
                "details" -> androidx.compose.material3.Text("Details")
                "settings" -> androidx.compose.material3.Text("Settings")
            }
        }
    }
    
    // Infinite transition
    @Composable
    fun InfiniteTransitionExample() {
        val infiniteTransition = androidx.compose.animation.core.rememberInfiniteTransition(
            label = "infinite"
        )
        
        val color by infiniteTransition.animateColor(
            initialValue = androidx.compose.ui.graphics.Color.Red,
            targetValue = androidx.compose.ui.graphics.Color.Blue,
            animationSpec = androidx.compose.animation.core.tween(1000),
            label = "color"
        )
        
        androidx.compose.foundation.layout.Box(
            modifier = Modifier
                .size(100.dp)
                .background(color)
        )
    }
}
```

---

## SECTION 2: CUSTOM LAYOUTS

```kotlin
/**
 * Custom Layouts
 * 
 * Creating custom layout composables.
 */
object CustomLayouts {
    
    // Custom layout composable
    @Composable
    fun CustomLayout(
        modifier: Modifier = Modifier,
        content: @Composable () -> Unit
    ) {
        androidx.compose.ui.layout.Layout(
            modifier = modifier,
            content = content
        ) { measurables, constraints ->
            // Measure each child
            val placeables = measurables.map { measurable ->
                measurable.measure(constraints)
            }
            
            // Calculate layout size
            val width = placeables.maxOfOrNull { it.width } ?: 0
            val height = placeables.sumOf { it.height }
            
            // Layout children
            layout(width, height.coerceAtMost(constraints.maxHeight)) {
                var yPosition = 0
                placeables.forEach { placeable ->
                    placeable.placeRelative(x = 0, y = yPosition)
                    yPosition += placeable.height
                }
            }
        }
    }
    
    // Grid layout
    @Composable
    fun CustomGridLayout(
        columns: Int,
        modifier: Modifier = Modifier,
        content: @Composable () -> Unit
    ) {
        androidx.compose.ui.layout.Layout(
            modifier = modifier,
            content = content
        ) { measurables, constraints ->
            val columnWidth = constraints.maxWidth / columns
            val placeables = measurables.map { it.measure(
                androidx.compose.ui.layoutConstraints(
                    minWidth = columnWidth,
                    maxWidth = columnWidth
                )
            )}
            
            val rows = (placeables.size + columns - 1) / columns
            val height = rows * (placeables.firstOrNull()?.height ?: 0)
            
            layout(constraints.maxWidth, height) {
                placeables.forEachIndexed { index, placeable ->
                    val column = index % columns
                    val row = index / columns
                    placeable.placeRelative(
                        x = column * columnWidth,
                        y = row * (placeables.first().height)
                    )
                }
            }
        }
    }
    
    // Flow layout
    @Composable
    fun FlowLayout(
        modifier: Modifier = Modifier,
        content: @Composable () -> Unit
    ) {
        // Use foundation FlowRow
        androidx.compose.foundation.layout.FlowRow(
            modifier = modifier,
            mainAxisSpacing = 8.dp,
            crossAxisSpacing = 8.dp
        ) {
            content()
        }
    }
}
```

---

## SECTION 3: GRAPHICS AND CANVAS

```kotlin
/**
 * Graphics and Canvas
 * 
 * Drawing custom graphics in Compose.
 */
object GraphicsCanvas {
    
    // Custom Canvas drawing
    @Composable
    fun CanvasDrawing() {
        androidx.compose.foundation.Canvas(
            modifier = Modifier.size(200.dp)
        ) {
            // Draw circle
            drawCircle(
                color = androidx.compose.ui.graphics.Color.Blue,
                radius = 100f
            )
            
            // Draw rectangle
            drawRect(
                color = androidx.compose.ui.graphics.Color.Red,
                topLeft = androidx.compose.ui.geometry.Offset(50f, 50f),
                size = androidx.compose.ui.geometry.Size(100f, 100f)
            )
            
            // Draw line
            drawLine(
                color = androidx.compose.ui.graphics.Color.Black,
                start = androidx.compose.ui.geometry.Offset(0f, 0f),
                end = androidx.compose.ui.geometry.Offset(200f, 200f),
                strokeWidth = 5f
            )
            
            // Draw arc
            drawArc(
                color = androidx.compose.ui.graphics.Color.Green,
                startAngle = 0f,
                sweepAngle = 90f,
                useCenter = true,
                topLeft = androidx.compose.ui.geometry.Offset(100f, 100f),
                size = androidx.compose.ui.geometry.Size(50f, 50f)
            )
        }
    }
    
    // Custom drawing with paths
    @Composable
    fun PathDrawing() {
        androidx.compose.foundation.Canvas(
            modifier = Modifier.size(200.dp)
        ) {
            val path = androidx.compose.ui.graphics.Path().apply {
                moveTo(0f, 0f)
                lineTo(100f, 100f)
                lineTo(200f, 0f)
                close()
            }
            
            drawPath(
                path = path,
                color = androidx.compose.ui.graphics.Color.Magenta
            )
        }
    }
    
    // Drawing with ImageBitmap
    @Composable
    fun ImageDrawing() {
        // Load and draw image
        val imageBitmap = androidx.compose.ui.graphics.ImageBitmap
        // Load from resources or network
        
        androidx.compose.foundation.Canvas(modifier = Modifier.size(200.dp)) {
            // Draw image (requires loading first)
            // drawImage(imageBitmap, ...)
        }
    }
}
```

---

## SECTION 4: GESTURE HANDLING

```kotlin
/**
 * Gesture Handling
 * 
 * Touch and gesture handling in Compose.
 */
object GestureHandling {
    
    // Clickable
    @Composable
    fun ClickableExample() {
        androidx.compose.foundation.clickable(
            interactionSource = androidx.compose.foundation.interaction.MutableInteractionSource(),
            indication = androidx.compose.material.ripple.rememberRipple()
        ) {
            println("Clicked!")
        }
        
        // Or use clickable modifier
        Modifier.clickable { println("Clicked!") }
    }
    
    // Combined clickable (click and long click)
    @Composable
    fun CombinedClickable() {
        androidx.compose.foundation.gestures.detectTapGestures(
            onTap = { println("Tapped") },
            onLongPress = { println("Long pressed") },
            onDoubleTap = { println("Double tapped") }
        )
    }
    
    // Draggable
    @Composable
    fun DraggableExample() {
        var offset by remember { mutableStateOf(androidx.compose.ui.geometry.Offset.Zero) }
        
        androidx.compose.foundation.Box(
            modifier = Modifier
                .offset(
                    x = androidx.compose.ui.unit.Dp(offset.x),
                    y = androidx.compose.ui.unit.Dp(offset.y)
                )
                .draggable(
                    orientation = androidx.compose.foundation.gestures.Orientation.Horizontal,
                    state = rememberDraggableState { delta ->
                        offset = offset.copy(x = offset.x + delta)
                    }
                )
        ) {
            androidx.compose.material3.Text("Drag me")
        }
    }
    
    // Transformable (pinch zoom, rotate)
    @Composable
    fun TransformableExample() {
        var scale by remember { mutableFloatStateOf(1f) }
        var rotation by remember { mutableFloatStateOf(0f) }
        var offset by remember { mutableStateOf(androidx.compose.ui.geometry.Offset.Zero) }
        
        val transformableState = androidx.compose.foundation.gestures.TransformableState { 
            change, transform ->
            scale = (scale * transform.scaleChange).coerceIn(0.5f, 3f)
            rotation += transform.rotationChange
            offset += transform.positionChange
        }
        
        androidx.compose.foundation.Box(
            modifier = Modifier
                .graphicsLayer {
                    scaleX = scale
                    scaleY = scale
                    rotationZ = rotation
                    translationX = offset.x
                    translationY = offset.y
                }
                .transformable(state = transformableState)
                .size(200.dp)
                .background(androidx.compose.material3.MaterialTheme.colorScheme.primary)
        )
    }
    
    // Scrollable
    @Composable
    fun ScrollableExample() {
        var scrollOffset by remember { mutableFloatStateOf(0f) }
        
        val state = androidx.compose.foundation.rememberScrollableState { delta ->
            scrollOffset += delta
            delta
        }
        
        Box(
            modifier = Modifier
                .scrollable(state, Orientation.Vertical)
                .size(200.dp)
        ) {
            // Content
        }
    }
}
```

---

## SECTION 5: PERFORMANCE

```kotlin
/**
 * Compose Performance
 * 
 * Optimizing Compose applications.
 */
object ComposePerformance {
    
    // Use stable types
    @androidx.compose.runtime.Stable
    class StableData(val value: String)
    
    // Use remember to avoid recomputation
    @Composable
    fun ExpensiveOperation(input: String) {
        // Remember the result
        val result = remember(input) {
            // Expensive computation
            input.uppercase()
        }
        
        // Derived state for expensive computations
        val derivedResult = remember(input) {
            input.split("").map { it.uppercase() }.joinToString("")
        }
        
        androidx.compose.material3.Text(result)
    }
    
    // Use key for LazyColumn
    @Composable
    fun LazyColumnPerformance() {
        androidx.compose.foundation.lazy.LazyColumn {
            items(
                items = listOf("A", "B", "C"),
                key = { it } // Stable key
            ) { item ->
                Text(item)
            }
        }
    }
    
    // Avoid unnecessary recomposition
    @Composable
    fun OptimizeRecomposition() {
        val state = remember { MyState() }
        
        // Use stable callback
        val stableCallback = remember { { handleClick() } }
        
        Button(onClick = stableCallback) { Text("Click") }
    }
    
    fun handleClick() {}
    
    // Use derivedStateOf
    @Composable
    fun DerivedStatePerformance() {
        val list = listOf(1, 2, 3, 4, 5)
        val filter = remember { mutableStateOf("") }
        
        // Only recomputes when list or filter changes
        val filteredList = remember(list, filter.value) {
            list.filter { it.toString().contains(filter.value) }
        }
    }
    
    // Benchmarking
    @OptIn(androidx.compose.animation.ExperimentalAnimationApi::class)
    @Composable
    fun EnableDebugFlags() {
        // Enable recomposition tracking in debug
        // Add: android:enableJetifier=true in gradle.properties
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Animations:**
- AnimatedVisibility: Show/hide with animation
- animateContentSize: Size transitions
- animateFloatAsState: Value animations
- Crossfade: Crossfade between content
- InfiniteTransition: Repeated animations

**Custom Layouts:**
- Layout composable: Custom positioning
- Grid layouts: Row and column grids
- Flow layouts: Wrap content

**Graphics:**
- Canvas: Low-level drawing
- Path: Custom shapes
- ImageBitmap: Image drawing
- GraphicsLayer: Transforms

**Gestures:**
- Clickable: Tap handling
- Draggable: Drag gestures
- Transformable: Pinch/rotate
- Scrollable: Scroll handling

**Performance:**
- Stable types: Reduce recomposition
- remember: Cache computations
- derivedStateOf: Expensive calculations
- LazyColumn keys: Efficient updates

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/02_Composable_Functions.md

---

## END OF ADVANCED COMPOSE PATTERNS GUIDE
