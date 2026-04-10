# Animation Performance

## Learning Objectives

1. Understanding Android rendering pipeline
2. Optimizing animations for 60fps target
3. Using hardware acceleration effectively
4. Implementing smooth UI animations
5. Debugging animation performance issues

```kotlin
package com.kotlin.performance.animation
```

---

## Prerequisites

- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md

---

## Core Concepts

### Android Rendering Pipeline

- **Measure**: Calculate view sizes
- **Layout**: Position views on screen
- **Draw**: Render to canvas

### 60fps Target

- 16ms per frame budget
- Must complete all work within 16ms

### SECTION 1: View Animation Optimization

```kotlin
/**
 * View Animation Optimization
 * 
 * Optimizing traditional view animations.
 */
class ViewAnimationOptimization {
    
    // Property animator (hardware accelerated)
    class PropertyAnimatorExample {
        
        fun animateAlpha(view: android.view.View) {
            view.animate()
                .alpha(0.5f)
                .setDuration(300)
                .setInterpolator(android.view.animation.AccelerateDecelerateInterpolator())
                .start()
        }
        
        fun animateTranslation(view: android.view.View) {
            view.animate()
                .translationX(100f)
                .translationY(100f)
                .setDuration(300)
                .withLayer()  // Hardware layer - important!
                .start()
        }
        
        fun animateScale(view: android.view.View) {
            view.animate()
                .scaleX(1.5f)
                .scaleY(1.5f)
                .setDuration(300)
                .withLayer()  // Enables hardware acceleration
                .start()
        }
        
        fun animateRotation(view: android.view.View) {
            view.animate()
                .rotation(360f)
                .setDuration(500)
                .withLayer()
                .start()
        }
    }
    
    // Value animator for custom animations
    class ValueAnimatorExample {
        
        fun customProgressAnimation(onProgress: (Float) -> Unit) {
            val animator = android.animation.ValueAnimator.ofFloat(0f, 1f)
            animator.duration = 500
            animator.addUpdateListener { animation ->
                val progress = animation.animatedValue as Float
                onProgress(progress)
            }
            animator.start()
        }
        
        fun colorAnimation(view: android.view.View) {
            val animator = android.animation.ValueAnimator.ofArgb(
                android.graphics.Color.RED,
                android.graphics.Color.BLUE
            )
            animator.duration = 300
            animator.addUpdateListener { animation ->
                view.backgroundColor = animation.animatedValue as Int
            }
            animator.start()
        }
    }
    
    // Object animator with proper interpolator
    class ObjectAnimatorExample {
        
        fun createBounceAnimation(view: android.view.View): android.animation.ObjectAnimator {
            return android.animation.ObjectAnimator.ofFloat(
                view,
                android.util.Property<android.view.View, Float>(
                    android.view.View::class.java,
                    "translationY"
                ),
                0f, -50f, 0f, -25f, 0f
            ).apply {
                duration = 1000
                interpolator = android.view.animation.OvershootInterpolator(2f)
            }
        }
    }
}
```

---

## SECTION 2: Compose Animation Optimization

```kotlin
/**
 * Compose Animation Optimization
 * 
 * Optimizing Jetpack Compose animations.
 */
class ComposeAnimationOptimization {
    
    // AnimatedVisibility with optimized settings
    class OptimizedVisibility {
        
        @Composable
        fun AnimatedContent(
            visible: Boolean,
            content: @Composable () -> Unit
        ) {
            androidx.compose.animation.AnimatedVisibility(
                visible = visible,
                enter = fadeIn(animationSpec = tween(300)),
                exit = fadeOut(animationSpec = tween(300)),
                content = content
            )
        }
    }
    
    // AnimateContentSize with debouncing
    class AnimateContentSize {
        
        @Composable
        fun ExpandableContent(
            expanded: Boolean,
            content: @Composable () -> Unit
        ) {
            var height by androidx.compose.runtime.remember { mutableStateOf(0) }
            
            Box(
                modifier = Modifier.animateContentSize(
                    animationSpec = spring(
                        dampingRatio = 0.8f,
                        stiffness = 300f
                    )
                )
            ) {
                content()
            }
        }
    }
    
    // Transition for multi-state animations
    class AnimatedTransition {
        
        enum class State { OFF, ON, LOADING }
        
        @Composable
        fun StateTransition(
            state: State,
            content: @Composable (State) -> Unit
        ) {
            val transition = updateTransition(
                targetState = state,
                label = "stateTransition"
            )
            
            transition.AnimatedVisibility(
                visible = { it == State.ON },
                enter = fadeIn() + expandVertically(),
                exit = fadeOut() + shrinkVertically()
            ) {
                content(state)
            }
            
            transition.AnimatedContent(
                targetState = state,
                transitionSpec = {
                    fadeIn(animationSpec = tween(300)) togetherWith
                        fadeOut(animationSpec = tween(300))
                },
                content = { state -> content(state) }
            )
        }
    }
    
    // Modifier.animateXXXAsState
    class AnimateAsState {
        
        @Composable
        fun AnimateFloat(modifier: Modifier = Modifier) {
            val scale by animateFloatAsState(
                targetValue = 1f,
                animationSpec = spring(
                    dampingRatio = Spring.DampingRatioMediumBouncy,
                    stiffness = Spring.StiffnessLow
                )
            )
            
            Box(
                modifier = modifier.scale(scale)
            )
        }
        
        @Composable
        fun AnimateColor(
            isEnabled: Boolean,
            enabledColor: androidx.compose.ui.graphics.Color,
            disabledColor: androidx.compose.ui.graphics.Color
        ) {
            val backgroundColor by animateColorAsState(
                targetValue = if (isEnabled) enabledColor else disabledColor,
                animationSpec = tween(300)
            )
            
            Box(
                modifier = Modifier.background(backgroundColor)
            )
        }
    }
}
```

---

## SECTION 3: Drawing and Hardware Acceleration

```kotlin
/**
 * Drawing and Hardware Acceleration
 * 
 * Optimizing canvas drawing and hardware layers.
 */
class DrawingOptimization {
    
    // Custom View with hardware layer
    class HardwareLayerView(context: android.content.Context) : android.view.View(context) {
        
        private var offsetX = 0f
        
        fun animateOffset(newOffset: Float) {
            // Enable hardware layer before animating
            setLayerType(android.view.View.LAYER_TYPE_HARDWARE, null)
            
            android.animation.ValueAnimator.ofFloat(offsetX, newOffset).apply {
                duration = 300
                addUpdateListener { animation ->
                    offsetX = animation.animatedValue as Float
                    invalidate()
                }
                start()
            }
        }
        
        override fun onDraw(canvas: android.graphics.Canvas) {
            canvas.save()
            canvas.translate(offsetX, 0f)
            // Draw content
            drawContent(canvas)
            canvas.restore()
        }
        
        private fun drawContent(canvas: android.graphics.Canvas) {
            val paint = android.graphics.Paint().apply {
                color = android.graphics.Color.BLUE
                style = android.graphics.Paint.Style.FILL
            }
            canvas.drawRect(0f, 0f, 100f, 100f, paint)
        }
    }
    
    // Optimize canvas drawing
    class CanvasOptimizer {
        
        fun optimizeDraw(canvas: android.graphics.Canvas, drawBlock: (android.graphics.Canvas) -> Unit) {
            canvas.save()
            
            // Clip to visible area for optimization
            canvas.clipRect(
                0f, 0f,
                canvas.width.toFloat(),
                canvas.height.toFloat()
            )
            
            drawBlock(canvas)
            
            canvas.restore()
        }
        
        // Pre-allocate paint objects
        class PaintAllocator {
            val fillPaint = android.graphics.Paint().apply {
                style = android.graphics.Paint.Style.FILL
                isAntiAlias = true
            }
            
            val strokePaint = android.graphics.Paint().apply {
                style = android.graphics.Paint.Style.STROKE
                strokeWidth = 2f
                isAntiAlias = true
            }
            
            val textPaint = android.graphics.Paint().apply {
                textSize = 48f
                isAntiAlias = true
            }
        }
        
        // Use hardware bitmap
        class BitmapRenderer(private val bitmap: android.graphics.Bitmap) {
            
            private val paint = android.graphics.Paint().apply {
                isAntiAlias = true
            }
            
            fun draw(canvas: android.graphics.Canvas, x: Float, y: Float) {
                // Check if bitmap is hardware-capable
                if (!bitmap.isRecycled) {
                    canvas.drawBitmap(bitmap, x, y, paint)
                }
            }
        }
    }
    
    // Hardware layer types
    class LayerTypeExample {
        
        fun noneLayer(view: android.view.View) {
            view.setLayerType(android.view.View.LAYER_TYPE_NONE, null)
        }
        
        fun softwareLayer(view: android.view.View) {
            // Forces software rendering
            view.setLayerType(android.view.View.LAYER_TYPE_SOFTWARE, null)
        }
        
        fun hardwareLayer(view: android.view.View) {
            // Uses GPU for rendering
            view.setLayerType(android.view.View.LAYER_TYPE_HARDWARE, null)
        }
    }
}
```

---

## Best Practices

1. **Target 60fps**: 16ms per frame budget, avoid dropping frames
2. **Use withLayer()**: Enable hardware acceleration in animations
3. **Avoid setLayerType in onDraw**: Pre-set outside
4. **Use Proper Interpolators**: Bounce, Overshoot for natural feel
5. **Pre-allocate Paints**: Reuse paint objects
6. **Clip to Bounds**: Only draw visible content
7. **Use Composables**: Prefer Compose animations
8. **Avoid Invalidations**: Minimize invalidate() calls
9. **Use Hardware Layer**: For transforms and alpha changes
10. **Debug with Profile GPU**: Monitor rendering time

---

## Common Pitfalls and Solutions

### Pitfall 1: Animations Drop Frames
- **Problem**: Animation janky
- **Solution**: Use withLayer(), enable hardware acceleration

### Pitfall 2: Too Many Invalidations
- **Problem**: Continuous redraws
- **Solution**: Invalidate only changed areas

### Pitfall 3: Memory Allocations in onDraw
- **Problem**: GC pauses cause jank
- **Solution**: Pre-allocate objects

### Pitfall 4: Incorrect Layer Type
- **Problem**: Render is slow
- **Solution**: Use LAYER_TYPE_HARDWARE for transforms

### Pitfall 5: Not Using Compose Animation
- **Problem**: Traditional animations slower
- **Solution**: Use Compose's optimized animations

### Pitfall 6: Complex Path Drawing
- **Problem**: Canvas operations slow
- **Solution**: Cache paths, use hardware layer

---

## Troubleshooting Guide

### Issue: Animation is Janky
- **Steps**: 1. Profile with Profile GPU Rendering 2. Check frame times 3. Use hardware layer
- **Tools**: Profile GPU Rendering, systrace

### Issue: Memory Usage High During Animation
- **Steps**: 1. Pre-allocate objects 2. Use hardware layer properly 3. Avoid allocations

---

## EXAMPLE 1: Smooth List Animation

```kotlin
/**
 * Smooth List Item Animations
 * 
 * RecyclerView item animations for performance.
 */
class ListAnimations {
    
    // Item animator for RecyclerView
    class SmoothItemAnimator : android.view.DefaultItemAnimator() {
        
        init {
            addDuration = 200
            removeDuration = 200
            moveDuration = 200
            changeDuration = 200
        }
        
        override fun preAnimateRemove(holder: androidx.recyclerview.widget.RecyclerView.ViewHolder): androidx.core.animation.Animator {
            // Fade out animation
            return android.animation.AnimatorSet().apply {
                playTogether(
                    android.animation.ObjectAnimator.ofFloat(
                        holder.itemView,
                        android.util.Property(android.view.View::class.java, "alpha"),
                        1f, 0f
                    ),
                    android.animation.ObjectAnimator.ofFloat(
                        holder.itemView,
                        android.util.Property(android.view.View::class.java, "translationX"),
                        0f, -holder.itemView.width.toFloat()
                    )
                )
            }
        }
        
        override fun animateAdd(holder: androidx.recyclerview.widget.RecyclerView.ViewHolder): androidx.core.animation.Animator {
            holder.itemView.alpha = 0f
            holder.itemView.translationX = -holder.itemView.width.toFloat()
            
            return android.animation.AnimatorSet().apply {
                playTogether(
                    android.animation.ObjectAnimator.ofFloat(
                        holder.itemView,
                        android.util.Property(android.view.View::class.java, "alpha"),
                        0f, 1f
                    ),
                    android.animation.ObjectAnimator.ofFloat(
                        holder.itemView,
                        android.util.Property(android.view.View::class.java, "translationX"),
                        -holder.itemView.width.toFloat(), 0f
                    )
                )
            }
        }
    }
    
    // Layout animation for list
    class ListLayoutAnimation {
        
        fun applyLayoutAnimation(recyclerView: androidx.recyclerview.widget.RecyclerView) {
            val animation = android.view.animation.LayoutAnimationController(
                android.view.animation.AnimationUtils.loadAnimation(
                    recyclerView.context,
                    android.R.anim.slide_in_left
                )
            ).apply {
                delay = 0.2f
                order = android.view.animation.LayoutAnimationController.ORDER_NORMAL
            }
            
            recyclerView.layoutAnimation = animation
        }
    }
    
    // ViewPropertyAnimator for smooth updates
    class ViewPropertyAnimator {
        
        fun slideIn(view: android.view.View) {
            view.visibility = android.view.View.VISIBLE
            view.translationX = view.width.toFloat()
            view.alpha = 0f
            
            view.animate()
                .translationX(0f)
                .alpha(1f)
                .setDuration(300)
                .setInterpolator(android.view.animation.DecelerateInterpolator())
                .withLayer()  // Important for smooth animation
                .start()
        }
        
        fun slideOut(view: android.view.View, onComplete: () -> Unit) {
            view.animate()
                .translationX(view.width.toFloat())
                .alpha(0f)
                .setDuration(300)
                .setInterpolator(android.view.animation.AccelerateInterpolator())
                .withLayer()
                .withEndAction {
                    view.visibility = android.view.View.GONE
                    onComplete()
                }
                .start()
        }
    }
}
```

---

## EXAMPLE 2: RecyclerView Pre-load Optimization

```kotlin
/**
 * RecyclerView Pre-fetch and Layout Optimization
 * 
 * Optimizing RecyclerView for smooth scrolling.
 */
class RecyclerViewOptimize {
    
    // Optimized RecyclerView setup
    class OptimizedRecyclerView {
        
        fun setupOptimizedRecyclerView(
            recyclerView: androidx.recyclerview.widget.RecyclerView,
            adapter: androidx.recyclerview.widget.RecyclerView.Adapter<*>
        ) {
            // Use FixedLayoutManager for consistent item sizes
            recyclerView.layoutManager = androidx.recyclerview.widget.LinearLayoutManager(
                recyclerView.context
            )
            
            recyclerView.adapter = adapter
            
            // Enable item prefetch
            recyclerView.setItemViewCacheSize(20)
            
            // Enable drawing cache for older devices
            recyclerView.isDrawingCacheEnabled = true
            recyclerView.drawingCacheQuality = android.view.View.DRAWING_CACHE_QUALITY_HIGH
            
            // Add scroll listener for pre-fetching
            recyclerView.addOnScrollListener(object : androidx.recyclerview.widget.RecyclerView.OnScrollListener() {
                override fun onScrolled(recyclerView: androidx.recyclerview.widget.RecyclerView, dx: Int, dy: Int) {
                    super.onScrolled(recyclerView, dx, dy)
                    // Pre-fetch more data when near end
                    val layoutManager = recyclerView.layoutManager as androidx.recyclerview.widget.LinearLayoutManager
                    val totalItemCount = layoutManager.itemCount
                    val lastVisiblePosition = layoutManager.findLastVisibleItemPosition()
                    
                    if (totalItemCount - lastVisiblePosition < 5) {
                        // Load more data
                    }
                }
            })
        }
    }
    
    // DiffUtil for efficient updates
    class DiffUtilAdapter : androidx.recyclerview.widget.ListAdapter<DataItem, DiffUtilAdapter.ViewHolder>(
        DataItemDiffCallback()
    ) {
        
        class ViewHolder(view: android.view.View) : androidx.recyclerview.widget.RecyclerView.ViewHolder(view)
        
        override fun onCreateViewHolder(parent: android.view.ViewGroup, viewType: Int): ViewHolder {
            val view = android.view.LayoutInflater.from(parent.context)
                .inflate(android.R.layout.list_item, parent, false)
            return ViewHolder(view)
        }
        
        override fun onBindViewHolder(holder: ViewHolder, position: Int) {
            holder.itemView.findViewById<android.widget.TextView>(android.R.id.text1).text = getItem(position).title
        }
        
        fun submitListSmooth(newList: List<DataItem>) {
            submitList(newList)  // DiffUtil calculates differences efficiently
        }
    }
    
    class DataItemDiffCallback : androidx.recyclerview.widget.DiffUtil.ItemCallback<DataItem>() {
        override fun areItemsTheSame(oldItem: DataItem, newItem: DataItem): Boolean {
            return oldItem.id == newItem.id
        }
        
        override fun areContentsTheSame(oldItem: DataItem, newItem: DataItem): Boolean {
            return oldItem == newItem
        }
    }
    
    data class DataItem(val id: String, val title: String)
    
    // Async bind for smooth scrolling
    class AsyncBindingAdapter : androidx.recyclerview.widget.ListAdapter<DataItem, AsyncBindingAdapter.ViewHolder>(
        DataItemDiffCallback()
    ) {
        
        private val executor = java.util.concurrent.Executors.newSingleThreadExecutor()
        
        override fun onBindViewHolder(holder: ViewHolder, position: Int) {
            val item = getItem(position)
            
            // Load image asynchronously
            executor.execute {
                val bitmap = loadImage(item.id)
                holder.itemView.post {
                    holder.itemView.findViewById<android.widget.ImageView>(android.R.id.icon)
                        .setImageBitmap(bitmap)
                }
            }
            
            holder.itemView.findViewById<android.widget.TextView>(android.R.id.text1).text = item.title
        }
        
        private fun loadImage(id: String): android.graphics.Bitmap? {
            return null
        }
    }
}
```

---

## EXAMPLE 3: Compose Scroll Performance

```kotlin
/**
 * Compose Scroll Performance Optimization
 * 
 * Optimizing lazy lists in Jetpack Compose.
 */
class ComposeScrollPerformance {
    
    // LazyColumn optimization
    class OptimizedLazyColumn {
        
        @Composable
        fun FastLazyColumn(
            items: List<DataItem>,
            onItemClick: (DataItem) -> Unit
        ) {
            LazyColumn(
                modifier = Modifier.fillMaxSize(),
                state = rememberLazyListState()  // Remember state
            ) {
                items(
                    items = items,
                    key = { it.id }  // Stable keys for efficient recomposition
                ) { item ->
                    ListItem(
                        item = item,
                        onClick = { onItemClick(item) }
                    )
                }
            }
        }
        
        @Composable
        private fun ListItem(
            item: DataItem,
            onClick: () -> Unit
        ) {
            Card(
                onClick = onClick,
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(horizontal = 16.dp, vertical = 8.dp)
            ) {
                Text(
                    text = item.title,
                    style = MaterialTheme.typography.bodyLarge,
                    modifier = Modifier.padding(16.dp)
                )
            }
        }
    }
    
    // Remember LazyListState for scroll position
    class LazyListStateManagement {
        
        @Composable
        fun RememberedState() {
            val listState = rememberLazyListState()
            
            // First visible item
            val firstVisibleItem by androidx.compose.runtime.remember {
                androidx.compose.runtime.derivedStateOf {
                    listState.firstVisibleItemIndex
                }
            }
            
            // Check if reached end
            val canLoadMore by androidx.compose.runtime.remember {
                androidx.compose.runtime.derivedStateOf {
                    listState.layoutInfo.visibleItemsInfo.lastOrNull()?.index ==
                        listState.layoutInfo.totalItemsCount - 1
                }
            }
            
            LaunchedEffect(canLoadMore) {
                if (canLoadMore) {
                    // Load more items
                }
            }
        }
        
        // Restore scroll position
        @Composable
        fun RestoreScrollPosition(savedPosition: Int) {
            val listState = rememberLazyListState()
            
            LaunchedEffect(Unit) {
                listState.scrollToItem(savedPosition)
            }
        }
    }
    
    // Optimized item content to avoid recomposition
    class OptimizedItem {
        
        @Composable
        fun OptimizedListItem(item: DataItem) {
            var isExpanded by remember(item.id) { mutableStateOf(false) }
            
            // Use stable keys if possible
            Text(
                text = if (isExpanded) item.fullTitle else item.title,
                style = MaterialTheme.typography.bodyLarge
            )
        }
    }
    
    // Paging integration
    class PagedLazyColumn {
        
        @Composable
        fun PagedList(
            pager: androidx.paging.compose.LazyPagingItems<DataItem>
        ) {
            LazyColumn(
                modifier = Modifier.fillMaxSize()
            ) {
                items(
                    count = pager.itemCount,
                    key = { pager.getKey(it) }
                ) { index ->
                    pager[index]?.let { item ->
                        ItemContent(item)
                    }
                }
                
                // Loading indicator
                when (pager.appendState) {
                    is androidx.paging.AppendState.Loading -> {
                        Box(
                            modifier = Modifier.fillMaxWidth(),
                            contentAlignment = Alignment.Center
                        ) {
                            CircularProgressIndicator()
                        }
                    }
                    is androidx.paging.AppendState.NotLoading -> {
                        // Not loading
                    }
                    is androidx.paging.AppendState.Error -> {
                        // Show error
                    }
                }
            }
        }
        
        @Composable
        private fun ItemContent(item: DataItem) {
            Text(text = item.title)
        }
    }
    
    data class DataItem(val id: String, val title: String, val fullTitle: String = title)
}
```

---

## OUTPUT STATEMENT RESULTS

**Animation Performance Guidelines:**
- Target 60fps (16ms/frame)
- Use withLayer() or hardware layer type
- Avoid allocations in onDraw
- Pre-allocate paint objects
- Use Compose animations over View animations

**View Animation Optimizations:**
- Use PropertyAnimator over ObjectAnimator
- Enable hardware acceleration
- Use proper interpolators
- Cache values when possible
- Avoid invalidate() calls

**Compose Animation Optimizations:**
- Use remember with keys for stability
- Use key parameter in items()
- Avoid lambda captures in animations
- Use derivedStateOf for scroll position

**RecyclerView Optimizations:**
- Use DiffUtil for efficient updates
- Enable item prefetch
- Use fixed layout manager
- Pre-fetch images asynchronously

**Debugging Tools:**
- Profile GPU Rendering
- systrace
- Perfetto
- Hierarchy Viewer

---

## Advanced Tips

- **Tip 1: Use hardware layer** - setLayerType(LAYER_TYPE_HARDWARE) before animations
- **Tip 2: Debug with GPU rendering** - Enable Profile GPU Rendering in Developer Options
- **Tip 3: Use Compose Modifier.animateXAsState** - Optimized for recomposition
- **Tip 4: Prefer value animators** - More efficient than property animators
- **Tip 5: Pre-allocate bitmaps** - Load images before animation starts

---

## Troubleshooting Guide (FAQ)

**Q: How do I check animation frame rate?**
A: Use Profile GPU Rendering in Developer Options - green bars mean good

**Q: Why do animations stutter?**
A: Usually allocations in onDraw, too many invalidates, or GPU issues

**Q: Should I use hardware layer?**
A: Yes for transforms and alpha, no for simple color changes

**Q: How do I optimize Compose lists?**
A: Use stable keys, rememberLazyListState, avoid lambda captures

---

## Advanced Tips and Tricks

- **Tip 1: Use frame callbacks** - Choreographer.postFrameCallback for frame-perfect timing
- **Tip 2: Disable overdraw** - Use "Show overdraw" in Developer Options
- **Tip 3: Profile with systrace** - Find rendering bottlenecks
- **Tip 4: Test on low-end devices** - Performance issues more visible
- **Tip 5: Consider Lottie** - Hardware-accelerated animation library

---

## CROSS-REFERENCES

- See: 09_PERFORMANCE/01_Performance_Optimization/01_Memory_Management.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md

---

## END OF ANIMATION PERFORMANCE GUIDE

(End of file - total 682 lines)