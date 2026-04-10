# Layout Inspector

## Learning Objectives

1. Understanding Layout Inspector functionality
2. Using Layout Inspector for view hierarchy analysis
3. Identifying layout performance issues
4. Debugging rendering problems
5. Optimizing layout performance

```kotlin
package com.kotlin.debugging.layout
```

---

## Prerequisites

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 09_PERFORMANCE/01_Performance_Optimization/04_Animation_Performance.md

---

## Core Concepts

### Layout Inspector Overview

- **View Tree**: Complete hierarchy of views
- **Properties Panel**: View attributes and values
- **3D View**: Layer visualization
- **Overdraw Visualization**: Identify overdraw

### SECTION 1: View Hierarchy Analysis

```kotlin
/**
 * View Hierarchy Analysis
 * 
 * Analyzing view hierarchy programmatically.
 */
class ViewHierarchyAnalyzer {
    
    // Get view hierarchy as tree
    class HierarchyPrinter {
        
        fun printHierarchy(view: android.view.View, indent: String = "") {
            println("$indent${view.javaClass.simpleName} [${getViewId(view)}]")
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    printHierarchy(view.getChildAt(i), "$indent  ")
                }
            }
        }
        
        private fun getViewId(view: android.view.View): String {
            return view.id.let { 
                if (it != android.view.View.NO_ID) {
                    view.context.resources.getResourceEntryName(it)
                } else {
                    "no_id"
                }
            }
        }
        
        fun getHierarchyAsString(view: android.view.View): String {
            return buildString {
                buildHierarchy(view, "")
            }
        }
        
        private fun buildHierarchy(view: android.view.View, indent: String): String {
            val sb = StringBuilder()
            sb.append("$indent${view.javaClass.simpleName} (${getViewId(view)})\n")
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    sb.append(buildHierarchy(view.getChildAt(i), "$indent  "))
                }
            }
            
            return sb.toString()
        }
    }
    
    // Find specific views in hierarchy
    class ViewFinder {
        
        fun findViewByType(root: android.view.View, targetType: Class<*>): List<android.view.View> {
            val results = mutableListOf<android.view.View>()
            findViewsByType(root, targetType, results)
            return results
        }
        
        private fun findViewsByType(
            view: android.view.View,
            targetType: Class<*>,
            results: MutableList<android.view.View>
        ) {
            if (targetType.isInstance(view)) {
                results.add(view)
            }
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    findViewsByType(view.getChildAt(i), targetType, results)
                }
            }
        }
        
        fun findViewsByTag(root: android.view.View, tag: String): List<android.view.View> {
            val results = mutableListOf<android.view.View>()
            findViewsByTag(root, tag, results)
            return results
        }
        
        private fun findViewsByTag(
            view: android.view.View,
            tag: String,
            results: MutableList<android.view.View>
        ) {
            if (tag == view.tag) {
                results.add(view)
            }
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    findViewsByTag(view.getChildAt(i), tag, results)
                }
            }
        }
        
        fun findViewsWithContentDescription(
            root: android.view.View,
            description: String
        ): List<android.view.View> {
            val results = mutableListOf<android.view.View>()
            findByContentDescription(root, description, results)
            return results
        }
        
        private fun findByContentDescription(
            view: android.view.View,
            description: String,
            results: MutableList<android.view.View>
        ) {
            if (description == view.contentDescription) {
                results.add(view)
            }
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    findByContentDescription(view.getChildAt(i), description, results)
                }
            }
        }
    }
    
    // View property inspector
    class ViewInspector {
        
        fun inspectView(view: android.view.View): ViewProperties {
            return ViewProperties(
                id = view.id,
                idName = getIdName(view),
                width = view.width,
                height = view.height,
                measuredWidth = view.measuredWidth,
                measuredHeight = view.measuredHeight,
                visibility = view.visibility,
                alpha = view.alpha,
                translationX = view.translationX,
                translationY = view.translationY,
                scaleX = view.scaleX,
                scaleY = view.scaleY,
                rotation = view.rotation,
                background = getBackgroundDescription(view),
                padding = "${view.paddingLeft}, ${view.paddingTop}, ${view.paddingRight}, ${view.paddingBottom}",
                margin = getMarginDescription(view)
            )
        }
        
        private fun getIdName(view: android.view.View): String {
            return try {
                view.context.resources.getResourceEntryName(view.id)
            } catch (e: Exception) {
                "no_id"
            }
        }
        
        private fun getBackgroundDescription(view: android.view.View): String {
            return view.background?.let { 
                "Drawable: ${it.javaClass.simpleName}" 
            } ?: "null"
        }
        
        private fun getMarginDescription(view: android.view.View): String {
            val params = view.layoutParams
            return if (params is android.view.ViewGroup.MarginLayoutParams) {
                "${params.leftMargin}, ${params.topMargin}, ${params.rightMargin}, ${params.bottomMargin}"
            } else {
                "N/A"
            }
        }
        
        data class ViewProperties(
            val id: Int,
            val idName: String,
            val width: Int,
            val height: Int,
            val measuredWidth: Int,
            val measuredHeight: Int,
            val visibility: Int,
            val alpha: Float,
            val translationX: Float,
            val translationY: Float,
            val scaleX: Float,
            val scaleY: Float,
            val rotation: Float,
            val background: String,
            val padding: String,
            val margin: String
        )
    }
}
```

---

## SECTION 2: Layout Performance Analysis

```kotlin
/**
 * Layout Performance Analysis
 * 
 * Analyzing layout performance issues.
 */
class LayoutPerformanceAnalyzer {
    
    // Measure layout pass performance
    class LayoutMeasurer {
        
        private var layoutStartTime = 0L
        
        fun startMeasure() {
            layoutStartTime = System.nanoTime()
        }
        
        fun endMeasure(): Long {
            return System.nanoTime() - layoutStartTime
        }
        
        // Add custom layout tracing
        class CustomLayoutTracer {
            
            fun traceLayout(view: android.view.View) {
                android.os.Trace.beginSection("Layout:${view.javaClass.simpleName}")
                // Layout work here
                android.os.Trace.endSection()
            }
        }
    }
    
    // Detect layout issues
    class LayoutIssueDetector {
        
        fun detectNestedWeights(view: android.view.View): List<LayoutIssue> {
            val issues = mutableListOf<LayoutIssue>()
            
            if (view is android.widget.LinearLayout) {
                var hasWeightChild = false
                
                for (i in 0 until view.childCount) {
                    val child = view.getChildAt(i)
                    val params = child.layoutParams
                    
                    if (params is android.widget.LinearLayout.LayoutParams) {
                        if (params.weight > 0) {
                            if (hasWeightChild && view.orientation == android.widget.LinearLayout.VERTICAL) {
                                issues.add(LayoutIssue(
                                    type = IssueType.NESTED_WEIGHT,
                                    severity = Severity.WARNING,
                                    message = "Nested weight in vertical LinearLayout",
                                    view = child
                                ))
                            }
                            hasWeightChild = true
                        }
                    }
                }
            }
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    issues.addAll(detectNestedWeights(view.getChildAt(i)))
                }
            }
            
            return issues
        }
        
        fun detectDeepHierarchy(view: android.view.View, maxDepth: Int = 15): List<LayoutIssue> {
            val issues = mutableListOf<LayoutIssue>()
            checkDepth(view, 0, maxDepth, issues)
            return issues
        }
        
        private fun checkDepth(
            view: android.view.View,
            depth: Int,
            maxDepth: Int,
            issues: MutableList<LayoutIssue>
        ) {
            if (depth > maxDepth) {
                issues.add(LayoutIssue(
                    type = IssueType.DEEP_HIERARCHY,
                    severity = Severity.WARNING,
                    message = "View hierarchy too deep ($depth levels)",
                    view = view
                ))
            }
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    checkDepth(view.getChildAt(i), depth + 1, maxDepth, issues)
                }
            }
        }
        
        fun detectMissingContentDescription(view: android.view.View): List<LayoutIssue> {
            val issues = mutableListOf<LayoutIssue>()
            
            if (view is android.widget.ImageView) {
                val imageView = view as android.widget.ImageView
                if (imageView.contentDescription == null || imageView.contentDescription.isEmpty()) {
                    issues.add(LayoutIssue(
                        type = IssueType.MISSING_CONTENT_DESCRIPTION,
                        severity = Severity.WARNING,
                        message = "ImageView missing content description",
                        view = view
                    ))
                }
            }
            
            if (view is android.view.ViewGroup) {
                for (i in 0 until view.childCount) {
                    issues.addAll(detectMissingContentDescription(view.getChildAt(i)))
                }
            }
            
            return issues
        }
        
        enum class IssueType {
            NESTED_WEIGHT,
            DEEP_HIERARCHY,
            MISSING_CONTENT_DESCRIPTION,
            HARD_CODED_SIZE,
            UNUSED_VIEW
        }
        
        enum class Severity { INFO, WARNING, ERROR }
        
        data class LayoutIssue(
            val type: IssueType,
            val severity: Severity,
            val message: String,
            val view: android.view.View
        )
    }
    
    // Layout optimization recommendations
    class OptimizationRecommendations {
        
        fun analyzeAndRecommend(view: android.view.View): List<Recommendation> {
            val recommendations = mutableListOf<Recommendation>()
            
            val issues = LayoutIssueDetector().detectNestedWeights(view)
            issues.forEach { issue ->
                recommendations.add(Recommendation(
                    title = "Replace nested weights",
                    description = "Use ConstraintLayout instead of nested LinearLayouts with weights",
                    priority = Priority.HIGH
                ))
            }
            
            val deepIssues = LayoutIssueDetector().detectDeepHierarchy(view)
            if (deepIssues.isNotEmpty()) {
                recommendations.add(Recommendation(
                    title = "Flatten view hierarchy",
                    description = "Reduce nesting depth for better performance",
                    priority = Priority.HIGH
                ))
            }
            
            return recommendations
        }
        
        data class Recommendation(
            val title: String,
            val description: String,
            val priority: Priority
        )
        
        enum class Priority { LOW, MEDIUM, HIGH }
    }
}
```

---

## SECTION 3: Render Debugging

```kotlin
/**
 * Render Debugging
 * 
 * Debugging rendering issues with Layout Inspector.
 */
class RenderDebugger {
    
    // Enable debug drawing
    class DebugDrawing {
        
        fun enableDebugDraw(view: android.view.View) {
            android.os.Debug.startMethodTracing("debug_draw")
            
            view.viewTreeObserver.addOnPreDrawListener {
                // Called before each frame
                true
            }
        }
        
        fun setLayerType(view: android.view.View, layerType: Int) {
            view.setLayerType(layerType, null)
        }
    }
    
    // Visualize overdraw
    class OverdrawVisualizer {
        
        // Enable overdraw visualization (in Developer Options)
        // Settings > Developer Options > Debug GPU overdraw > Show overdraw areas
        
        // Color coding:
        // Blue = 1x overdraw
        // Green = 2x overdraw
        // Pink = 3x overdraw
        // Red = 4x+ overdraw
        
        fun analyzeOverdraw(view: android.view.View): OverdrawAnalysis {
            var viewCount = 0
            var totalDrawCount = 0
            
            fun countDraws(v: android.view.View) {
                viewCount++
                totalDrawCount += 1
                
                if (v is android.view.ViewGroup) {
                    for (i in 0 until v.childCount) {
                        countDraws(v.getChildAt(i))
                    }
                }
            }
            
            countDraws(view)
            
            return OverdrawAnalysis(
                totalViews = viewCount,
                estimatedOverdraw = if (viewCount > 0) totalDrawCount.toFloat() / viewCount else 1f,
                recommendation = if (totalDrawCount > viewCount * 2) "Reduce overdraw" else "OK"
            )
        }
        
        data class OverdrawAnalysis(
            val totalViews: Int,
            val estimatedOverdraw: Float,
            val recommendation: String
        )
    }
    
    // Profile frame rendering
    class FrameProfiler {
        
        private val frameTimes = mutableListOf<Long>()
        
        fun profileFrame(view: android.view.View) {
            val startTime = System.nanoTime()
            
            view.viewTreeObserver.addOnPreDrawListener {
                val frameTime = System.nanoTime() - startTime
                frameTimes.add(frameTime)
                
                val frameTimeMs = frameTime / 1_000_000
                if (frameTimeMs > 16.67) {
                    // Frame took too long
                }
                
                true
            }
        }
        
        fun getFrameStats(): FrameStats {
            if (frameTimes.isEmpty()) {
                return FrameStats(0, 0, 0, 0f)
            }
            
            val avgFrameTime = frameTimes.average()
            val maxFrameTime = frameTimes.maxOrNull() ?: 0
            val slowFrames = frameTimes.count { it > 16.67 * 1_000_000 }
            
            return FrameStats(
                totalFrames = frameTimes.size,
                averageMs = avgFrameTime.toLong(),
                maxMs = maxFrameTime / 1_000_000,
                slowFramePercent = (slowFrames.toFloat() / frameTimes.size) * 100
            )
        }
        
        data class FrameStats(
            val totalFrames: Int,
            val averageMs: Long,
            val maxMs: Long,
            val slowFramePercent: Float
        )
    }
}
```

---

## Best Practices

1. **Use ConstraintLayout**: Flatten hierarchy, better performance
2. **Avoid Deep Nesting**: Limit view hierarchy depth
3. **Use ViewStub**: Defer inflate of rarely-used views
4. **Avoid Overdraw**: Remove unnecessary backgrounds
5. **Optimize Images**: Use proper sizes, avoid too large
6. **Use Include Tag**: Reuse layouts efficiently
7. **Enable RTL Support**: Test for all locales
8. **Check Accessibility**: Content descriptions
9. **Use Merge Tag**: Reduce root containers
10. **Profile with systrace**: Find rendering bottlenecks

---

## Common Pitfalls and Solutions

### Pitfall 1: Deep View Hierarchy
- **Problem**: Too many nested layouts
- **Solution**: Use ConstraintLayout, merge includes

### Pitfall 2: Unnecessary Backgrounds
- **Problem**: Multiple backgrounds cause overdraw
- **Solution**: Remove redundant backgrounds

### Pitfall 3: Wrong Layout Choice
- **Problem**: Using RelativeLayout when LinearLayout sufficient
- **Solution**: Choose appropriate layout for use case

### Pitfall 4: Hardcoded Dimensions
- **Problem**: Using dp values that don't scale
- **Solution**: Use match_parent, wrap_content

### Pitfall 5: Not Using ViewStub
- **Problem**: inflating rarely-used layouts
- **Solution**: Use ViewStub for deferred inflation

---

## Troubleshooting Guide

### Issue: Layout Not Updating
- **Steps**: 1. Check visibility 2. Invalidate properly 3. Check layout params

### Issue: Performance Issues
- **Steps**: 1. Profile with Layout Inspector 2. Check overdraw 3. Flatten hierarchy

---

## EXAMPLE 1: Layout Inspector Analysis

```kotlin
/**
 * Layout Inspector Analysis Example
 * 
 * Using Layout Inspector findings to fix issues.
 */
class LayoutInspectorAnalysis {
    
    // Analyze Layout Inspector dump
    class AnalyzeInspectorDump {
        
        fun analyzeDump(viewHierarchy: ViewHierarchy): AnalysisResult {
            val issues = mutableListOf<Issue>()
            
            // Check hierarchy depth
            val maxDepth = findMaxDepth(viewHierarchy.root)
            if (maxDepth > 15) {
                issues.add(Issue(
                    type = "DEEP_HIERARCHY",
                    message = "Hierarchy depth: $maxDepth",
                    suggestion = "Use ConstraintLayout to flatten"
                ))
            }
            
            // Check layout count
            val layoutCount = countLayouts(viewHierarchy.root)
            if (layoutCount > 30) {
                issues.add(Issue(
                    type = "TOO_MANY_LAYOUTS",
                    message = "Total layouts: $layoutCount",
                    suggestion = "Optimize layout structure"
                ))
            }
            
            // Check for nested LinearLayouts
            if (hasNestedLinearLayouts(viewHierarchy.root)) {
                issues.add(Issue(
                    type = "NESTED_LINEAR_LAYOUTS",
                    message = "Found nested LinearLayouts with weights",
                    suggestion = "Replace with ConstraintLayout"
                ))
            }
            
            return AnalysisResult(issues)
        }
        
        private fun findMaxDepth(view: ViewNode): Int {
            if (view.children.isEmpty()) return 1
            return 1 + view.children.maxOfOrNull { findMaxDepth(it) } ?: 0
        }
        
        private fun countLayouts(view: ViewNode): Int {
            var count = if (view.isLayout) 1 else 0
            view.children.forEach { count += countLayouts(it) }
            return count
        }
        
        private fun hasNestedLinearLayouts(view: ViewNode): Boolean {
            if (view.isLayout && view.type.contains("LinearLayout")) {
                val hasChildLinear = view.children.any { 
                    it.isLayout && it.type.contains("LinearLayout") 
                }
                if (hasChildLinear) return true
            }
            return view.children.any { hasNestedLinearLayouts(it) }
        }
        
        data class Issue(
            val type: String,
            val message: String,
            val suggestion: String
        )
        
        data class AnalysisResult(val issues: List<Issue>)
        
        data class ViewHierarchy(val root: ViewNode)
        
        data class ViewNode(
            val type: String,
            val isLayout: Boolean,
            val children: List<ViewNode>
        )
    }
    
    // Fix identified issues
    class FixLayoutIssues {
        
        // Before: Nested LinearLayouts with weights
        /*
        <LinearLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:orientation="vertical">
            
            <LinearLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:orientation="horizontal">
                
                <TextView
                    android:layout_width="0dp"
                    android:layout_height="wrap_content"
                    android:layout_weight="1"/>
                    
                <TextView
                    android:layout_width="0dp"
                    android:layout_height="wrap_content"
                    android:layout_weight="1"/>
            </LinearLayout>
        </LinearLayout>
        */
        
        // After: ConstraintLayout (flat hierarchy)
        /*
        <androidx.constraintlayout.widget.ConstraintLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content">
            
            <TextView
                android:id="@+id/text1"
                android:layout_width="0dp"
                android:layout_height="wrap_content"
                app:layout_constraintStart_toStartOf="parent"
                app:layout_constraintEnd_toStartOf="@id/text2"
                app:layout_constraintHorizontal_weight="1"/>
                
            <TextView
                android:id="@+id/text2"
                android:layout_width="0dp"
                android:layout_height="wrap_content"
                app:layout_constraintStart_toEndOf="@id/text1"
                app:layout_constraintEnd_toEndOf="parent"
                app:layout_constraintHorizontal_weight="1"/>
        </androidx.constraintlayout.widget.ConstraintLayout>
        */
    }
}
```

---

## EXAMPLE 2: Compose Layout Analysis

```kotlin
/**
 * Jetpack Compose Layout Analysis
 * 
 * Analyzing Compose layouts with Layout Inspector.
 */
class ComposeLayoutAnalysis {
    
    // Compose-specific view info
    class ComposeViewInfo {
        
        // In Compose, use Layout Inspector to see:
        // - Composable functions in tree
        // - Recomposition counts
        // - Skipped composables
        
        @Composable
        fun ExampleComposable() {
            // View info in inspector shows:
            // - Composable name
            // - Parameters
            // - Recompose count
        }
        
        // Check recomposition
        @Composable
        fun RecompositionTracker() {
            var recomposeCount by androidx.compose.runtime.remember { 
                mutableStateOf(0) 
            }
            
            androidx.compose.runtime.SideEffect {
                recomposeCount++
            }
            
            Text("Recompositions: $recomposeCount")
        }
        
        // Detect unnecessary recompositions
        class UnnecessaryRecompositionDetector {
            
            @Composable
            fun ProblematicCode() {
                var state by mutableStateOf(0)
                
                // BAD: Captures lambda that changes
                Button(onClick = { state++ }) {
                    Text("Click")
                }
            }
            
            @Composable
            fun OptimizedCode() {
                var state by mutableStateOf(0)
                
                // GOOD: Stable callback
                val onClick = androidx.compose.runtime.remember(state) {
                    { state++ }
                }
                
                Button(onClick = onClick) {
                    Text("Click")
                }
            }
        }
    }
    
    // Compose performance tips
    class ComposePerformance {
        
        // Use stable annotations
        @Stable
        class StableData(val value: String)
        
        // Use remember for expensive operations
        @Composable
        fun ExpensiveOperation() {
            val result = androidx.compose.runtime.remember(key1) {
                expensiveComputation()
            }
        }
        
        private fun expensiveComputation(): String = "result"
        
        // Use derivedStateOf for derived state
        @Composable
        fun DerivedState(items: List<Item>) {
            val filteredItems by androidx.compose.runtime.remember(items) {
                androidx.compose.runtime.derivedStateOf {
                    items.filter { it.enabled }
                }
            }
        }
        
        // Use keys for lists
        @Composable
        fun LazyList(items: List<Item>) {
            LazyColumn {
                items(
                    items = items,
                    key = { it.id }  // Stable key
                ) { item ->
                    ItemRow(item)
                }
            }
        }
        
        data class Item(val id: String, val enabled: Boolean)
        
        @Composable
        private fun ItemRow(item: Item) {}
    }
    
    // Compose Layout Inspector interpretation
    class ComposeInspector {
        
        // What to look for:
        // 1. High recompose counts
        // 2. Skipped composables (stable params)
        // 3. Deep modifier chains
        // 4. Expensive composables
        
        fun analyzeComposeHierarchy(hierarchy: ComposeNode): ComposeAnalysis {
            return ComposeAnalysis(
                totalComposables = countComposables(hierarchy),
                maxDepth = findMaxDepth(hierarchy),
                suggestions = listOf(
                    "Use remember for expensive operations",
                    "Use derivedStateOf for derived state",
                    "Avoid capturing lambdas in composables"
                )
            )
        }
        
        private fun countComposables(node: ComposeNode): Int {
            return 1 + node.children.sumOf { countComposables(it) }
        }
        
        private fun findMaxDepth(node: ComposeNode): Int {
            if (node.children.isEmpty()) return 1
            return 1 + node.children.maxOfOrNull { findMaxDepth(it) } ?: 0
        }
        
        data class ComposeNode(val name: String, val children: List<ComposeNode>)
        
        data class ComposeAnalysis(
            val totalComposables: Int,
            val maxDepth: Int,
            val suggestions: List<String>
        )
    }
}
```

---

## EXAMPLE 3: Real-World Layout Fixes

```kotlin
/**
 * Real-World Layout Fixes
 * 
 * Common layout issues and solutions found with Layout Inspector.
 */
class RealWorldLayoutFixes {
    
    // Fix 1: Inefficient list item layout
    class ListItemFix {
        
        // Before: Nested layouts
        /*
        <LinearLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:orientation="horizontal"
            android:padding="16dp">
            
            <ImageView
                android:layout_width="48dp"
                android:layout_height="48dp"/>
                
            <LinearLayout
                android:layout_width="0dp"
                android:layout_weight="1"
                android:layout_height="wrap_content"
                android:orientation="vertical">
                
                <TextView
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content"/>
                    
                <TextView
                    android:layout_width="match_parent"
                    android:layout_height="wrap_content"/>
            </LinearLayout>
        </LinearLayout>
        */
        
        // After: ConstraintLayout
        /*
        <androidx.constraintlayout.widget.ConstraintLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:padding="16dp">
            
            <ImageView
                android:id="@+id/icon"
                android:layout_width="48dp"
                android:layout_height="48dp"
                app:layout_constraintStart_toStartOf="parent"
                app:layout_constraintTop_toTopOf="parent"/>
                
            <TextView
                android:id="@+id/title"
                android:layout_width="0dp"
                android:layout_height="wrap_content"
                app:layout_constraintStart_toEndOf="@id/icon"
                app:layout_constraintEnd_toEndOf="parent"
                app:layout_constraintTop_toTopOf="parent"/>
                
            <TextView
                android:id="@+id/subtitle"
                android:layout_width="0dp"
                android:layout_height="wrap_content"
                app:layout_constraintStart_toEndOf="@id/icon"
                app:layout_constraintEnd_toEndOf="parent"
                app:layout_constraintTop_toBottomOf="@id/title"/>
        </androidx.constraintlayout.widget.ConstraintLayout>
        */
    }
    
    // Fix 2: Double background overdraw
    class BackgroundFix {
        
        // Before: Background on parent and child
        /*
        <FrameLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:background="@color/white">
            
            <LinearLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content"
                android:background="@color/white">  // Double overdraw!
                
                <TextView android:layout_width="wrap_content"/>
            </LinearLayout>
        </FrameLayout>
        */
        
        // After: Single background
        /*
        <FrameLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:background="@color/white">
            
            <LinearLayout
                android:layout_width="match_parent"
                android:layout_height="wrap_content">  // No background
                
                <TextView android:layout_width="wrap_content"/>
            </LinearLayout>
        </FrameLayout>
        */
    }
    
    // Fix 3: Inefficient visibility toggle
    class VisibilityFix {
        
        // Before: Using Gone causes layout pass
        /*
        <LinearLayout
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:visibility="gone">  // Layout pass!
        */
        
        // After: Use ViewStub or INVISIBLE
        /*
        <ViewStub
            android:id="@+id/stub"
            android:layout_width="match_parent"
            android:layout_height="wrap_content"
            android:inflatedId="@+id/content"/>
        
        // Or:
        android:visibility="invisible"  // No layout, just invisible
        */
    }
    
    // Fix 4: Optimizing complex form
    class FormOptimization {
        
        // Before: Many nested ScrollView + LinearLayout
        /*
        <ScrollView>
            <LinearLayout>
                <LinearLayout>...field 1...</LinearLayout>
                <LinearLayout>...field 2...</LinearLayout>
                <LinearLayout>...field 3...</LinearLayout>
                ...
            </LinearLayout>
        </ScrollView>
        */
        
        // After: Single ConstraintLayout
        /*
        <androidx.core.widget.NestedScrollView>
            <androidx.constraintlayout.widget.ConstraintLayout>
                <TextView android:id="@+id/field1"/>
                <TextView
                    app:layout_constraintTop_toBottomOf="@id/field1"/>
                ...
            </androidx.constraintlayout.widget.ConstraintLayout>
        </androidx.core.widget.NestedScrollView>
        */
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Layout Inspector Findings:**
- View tree hierarchy
- View properties and values
- 3D layer visualization
- Overdraw color overlay

**Common Issues Found:**
- Deep view hierarchy
- Nested weights in LinearLayout
- Unnecessary backgrounds
- Missing content descriptions
- Hardcoded dimensions

**Layout Performance Tips:**
- Use ConstraintLayout
- Avoid nested weights
- Flatten hierarchy
- Remove unnecessary backgrounds
- Use ViewStub for deferred inflation

**Overdraw Colors:**
- Blue = 1x (OK)
- Green = 2x
- Pink = 3x
- Red = 4x+ (needs fix)

---

## Advanced Tips

- **Tip 1: Enable GPU rendering** - Show overdraw in Developer Options
- **Tip 2: Use systrace** - Profile layout passes
- **Tip 3: Check hierarchy offline** - Use view server
- **Tip 4: Compare layouts** - Before/after optimization
- **Tip 5: Use Compose inspection** - Layout Inspector for Compose

---

## Troubleshooting Guide (FAQ)

**Q: How do I open Layout Inspector?**
A: Tools > Layout Inspector in Android Studio

**Q: Why is layout slow?**
A: Deep hierarchy, nested weights, unnecessary layouts

**Q: What does overdraw mean?**
A: Pixel drawn multiple times - shows as colors in inspector

**Q: How do I fix deep hierarchy?**
A: Use ConstraintLayout, merge includes, flatten structure

---

## Advanced Tips and Tricks

- **Tip 1: Use 3D view** - Rotate to see layer issues
- **Tip 2: Export hierarchy** - Save for offline analysis
- **Tip 3: Compare snapshots** - Before/after changes
- **Tip 4: Filter by type** - Focus on specific view types
- **Tip 5: Check properties** - Find layout issues in properties

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 09_PERFORMANCE/02_Debugging_Tools/01_Android_Profiler.md
- See: 09_PERFORMANCE/01_Performance_Optimization/04_Animation_Performance.md

---

## END OF LAYOUT INSPECTOR GUIDE

(End of file - total 682 lines)