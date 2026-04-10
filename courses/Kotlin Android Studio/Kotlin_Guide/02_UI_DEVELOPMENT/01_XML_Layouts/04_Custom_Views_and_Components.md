# LEARNING OBJECTIVES

1. Creating custom views from scratch
2. Extending existing Android views
3. Implementing custom ViewGroups
4. Handling touch events and gestures
5. Drawing custom graphics

```kotlin
package com.android.ui.custom
```

---

## SECTION 1: CUSTOM VIEWS OVERVIEW

```kotlin
/**
 * Custom Views Overview
 * 
 * Creating custom views allows unique UI components and better control.
 */
object CustomViewsOverview {
    
    // Ways to create custom views
    enum class CreationMethod {
        EXTEND_EXISTING,  // Extend TextView, Button, etc.
        COMBINE_VIEWS,    // Extend existing layouts
        DRAW_FROM_SCRATCH // Full custom drawing
    }
    
    // Key methods to override
    object KeyMethods {
        const val ON_MEASURE = "onMeasure() - Measure view size"
        const val ON_LAYOUT = "onLayout() - Position children"
        const val ON_DRAW = "onDraw() - Render content"
        const val ON_TOUCH = "onTouchEvent() - Handle input"
    }
}
```

---

## SECTION 2: EXTENDING EXISTING VIEWS

```kotlin
/**
 * Extending Existing Views
 * 
 * Customizing built-in views for specific functionality.
 */
class ExtendingViews {
    
    // Custom styled TextView
    class StyledTextView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.TextView(context, attrs, defStyleAttr) {
        
        // Custom attributes
        private var strikeThrough: Boolean = false
        private var highlightOnFocus: Boolean = true
        
        init {
            // Read custom attributes
            context.theme.obtainStyledAttributes(
                attrs,
                R.styleable.StyledTextView,
                defStyleAttr,
                0
            ).apply {
                try {
                    strikeThrough = getBoolean(R.styleable.StyledTextView_strikeThrough, false)
                    highlightOnFocus = getBoolean(R.styleable.StyledTextView_highlightOnFocus, true)
                } finally {
                    recycle()
                }
            }
            
            updateStyle()
        }
        
        private fun updateStyle() {
            // Apply strike-through if enabled
            paintFlags = if (strikeThrough) {
                paintFlags or android.graphics.Paint.STRIKE_THRU_TEXT_FLAG
            } else {
                paintFlags and android.graphics.Paint.STRIKE_THRU_TEXT_FLAG.inv()
            }
        }
        
        override fun onFocusChanged(focused: Boolean, direction: Int, previouslyFocusedRect: android.graphics.Rect?) {
            super.onFocusChanged(focused, direction, previouslyFocusedRect)
            if (highlightOnFocus) {
                alpha = if (focused) 0.7f else 1.0f
            }
        }
        
        fun setStrikeThrough(enabled: Boolean) {
            strikeThrough = enabled
            updateStyle()
        }
        
        fun setHighlightOnFocus(enabled: Boolean) {
            highlightOnFocus = enabled
        }
    }
    
    // Custom button with loading state
    class LoadingButton @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.Button(context, attrs, defStyleAttr) {
        
        private var isLoading = false
        private var originalText: CharSequence = ""
        private var loadingText: String = "Loading..."
        
        private val progressDrawable: android.graphics.drawable.ClipDrawable by lazy {
            android.graphics.drawable.ClipDrawable(
                android.graphics.drawable.ColorDrawable(android.graphics.Color.GRAY),
                android.graphics.Gravity.START,
                android.graphics.ClipDrawable.HORIZONTAL
            )
        }
        
        fun setLoading(loading: Boolean) {
            isLoading = loading
            
            if (loading) {
                originalText = text
                text = loadingText
                isEnabled = false
                // Start animation
                startLoadingAnimation()
            } else {
                text = originalText
                isEnabled = true
                // Stop animation
                stopLoadingAnimation()
            }
        }
        
        private fun startLoadingAnimation() {
            // Animation logic
        }
        
        private fun stopLoadingAnimation() {
            // Stop animation
        }
    }
    
    // Custom EditText with validation
    class ValidatedEditText @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.EditText(context, attrs, defStyleAttr) {
        
        enum class ValidationType {
            EMAIL, PHONE, URL, CUSTOM
        }
        
        private var validationType: ValidationType = ValidationType.CUSTOM
        private var isValid: Boolean = true
        private var validationError: String = "Invalid input"
        
        fun setValidationType(type: ValidationType) {
            validationType = type
            validate()
        }
        
        fun setCustomValidation(regex: String, errorMsg: String) {
            validationType = ValidationType.CUSTOM
            validationError = errorMsg
            validate()
        }
        
        private fun validate(): Boolean {
            val text = text.toString()
            isValid = when (validationType) {
                ValidationType.EMAIL -> android.util.Patterns.EMAIL_ADDRESS.matcher(text).matches()
                ValidationType.PHONE -> android.util.Patterns.PHONE.matcher(text).matches()
                ValidationType.URL -> android.util.Patterns.WEB_URL.matcher(text).matches()
                ValidationType.CUSTOM -> true // Handled externally
            }
            
            error = if (isValid) null else validationError
            return isValid
        }
        
        override fun onTextChanged(text: CharSequence?, start: Int, before: Int, count: Int) {
            super.onTextChanged(text, start, before, count)
            if (validationType != ValidationType.CUSTOM) {
                validate()
            }
        }
    }
}
```

---

## SECTION 3: COMPOSITE VIEWS (VIEWGROUPS)

```kotlin
/**
 * Composite Views (ViewGroups)
 * 
 * Combining multiple views into a reusable component.
 */
class CompositeViews {
    
    // Custom card component
    class CustomCardView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.FrameLayout(context, attrs, defStyleAttr) {
        
        private var cardTitle: String = ""
        private var cardContent: String = ""
        private var showShadow: Boolean = true
        
        private val titleView: android.widget.TextView
        private val contentView: android.widget.TextView
        private val actionButton: android.widget.Button?
        
        init {
            // Inflate layout
            android.view.LayoutInflater.from(context).inflate(R.layout.custom_card, this, true)
            
            titleView = findViewById(R.id.card_title)
            contentView = findViewById(R.id.card_content)
            actionButton = findViewById(R.id.card_action)
            
            // Read attributes
            context.theme.obtainStyledAttributes(
                attrs,
                R.styleable.CustomCardView,
                defStyleAttr,
                0
            ).apply {
                try {
                    cardTitle = getString(R.styleable.CustomCardView_cardTitle) ?: ""
                    cardContent = getString(R.styleable.CustomCardView_cardContent) ?: ""
                    showShadow = getBoolean(R.styleable.CustomCardView_cardShadow, true)
                } finally {
                    recycle()
                }
            }
            
            // Apply styling
            if (showShadow) {
                setCardShadow()
            }
            
            // Set initial values
            setTitle(cardTitle)
            setContent(cardContent)
        }
        
        fun setTitle(title: String) {
            cardTitle = title
            titleView.text = title
            titleView.visibility = if (title.isEmpty()) android.view.View.GONE else android.view.View.VISIBLE
        }
        
        fun setContent(content: String) {
            cardContent = content
            contentView.text = content
        }
        
        fun setAction(text: String, listener: () -> Unit) {
            actionButton?.let { button ->
                button.text = text
                button.visibility = android.view.View.VISIBLE
                button.setOnClickListener { listener() }
            }
        }
        
        private fun setCardShadow() {
            // Apply elevation
            elevation = 8f
        }
    }
    
    // Rating bar component
    class RatingBarView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.LinearLayout(context, attrs, defStyleAttr) {
        
        private val stars = mutableListOf<android.widget.ImageView>()
        private var rating: Int = 0
        private var maxRating: Int = 5
        private var onRatingChange: ((Int) -> Unit)? = null
        
        private val starFilled: android.graphics.drawable.Drawable?
        private val starEmpty: android.graphics.drawable.Drawable?
        
        init {
            orientation = android.widget.LinearLayout.HORIZONTAL
            
            // Get star drawables
            starFilled = android.content.res.Resources.getSystem()
                .getDrawable(android.R.drawable.btn_star_big_on, null)
            starEmpty = android.content.res.Resources.getSystem()
                .getDrawable(android.R.drawable.btn_star_big_off, null)
            
            // Create star views
            for (i in 0 until maxRating) {
                val star = android.widget.ImageView(context).apply {
                    layoutParams = android.widget.LinearLayout.LayoutParams(
                        android.view.ViewGroup.LayoutParams.WRAP_CONTENT,
                        android.view.ViewGroup.LayoutParams.WRAP_CONTENT
                    ).apply {
                        setMargins(4, 0, 4, 0)
                    }
                    setImageDrawable(starEmpty)
                    setOnClickListener { setRating(i + 1) }
                }
                stars.add(star)
                addView(star)
            }
        }
        
        fun setRating(value: Int) {
            rating = value.coerceIn(0, maxRating)
            
            stars.forEachIndexed { index, star ->
                star.setImageDrawable(if (index < rating) starFilled else starEmpty)
            }
            
            onRatingChange?.invoke(rating)
        }
        
        fun getRating(): Int = rating
        
        fun setOnRatingChangeListener(listener: (Int) -> Unit) {
            onRatingChange = listener
        }
    }
}
```

---

## SECTION 4: CUSTOM DRAWING

```kotlin
/**
 * Custom Drawing
 * 
 * Creating views with custom canvas drawing.
 */
class CustomDrawing {
    
    // Custom circular progress
    class CircularProgressView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.view.View(context, attrs, defStyleAttr) {
        
        private var progress: Float = 0f
        private var maxProgress: Float = 100f
        private var strokeWidth: Float = 20f
        private var progressColor: Int = android.graphics.Color.BLUE
        private var backgroundColor: Int = android.graphics.Color.LTGRAY
        
        private val paint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG)
        private val backgroundPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG)
        private val rect = android.graphics.RectF()
        
        fun setProgress(value: Float) {
            progress = value.coerceIn(0f, maxProgress)
            invalidate()
        }
        
        fun setProgressColor(color: Int) {
            progressColor = color
            invalidate()
        }
        
        override fun onSizeChanged(w: Int, h: Int, oldw: Int, oldh: Int) {
            super.onSizeChanged(w, h, oldw, oldh)
            
            val padding = strokeWidth / 2
            rect.set(
                padding + paddingLeft,
                padding + paddingTop,
                w - padding - paddingRight,
                h - padding - paddingBottom
            )
        }
        
        override fun onDraw(canvas: android.graphics.Canvas) {
            super.onDraw(canvas)
            
            // Draw background circle
            backgroundPaint.apply {
                color = backgroundColor
                style = android.graphics.Paint.Style.STROKE
                strokeWidth = this@CircularProgressView.strokeWidth
            }
            canvas.drawArc(rect, 0f, 360f, false, backgroundPaint)
            
            // Draw progress arc
            paint.apply {
                color = progressColor
                style = android.graphics.Paint.Style.STROKE
                strokeWidth = this@CircularProgressView.strokeWidth
                strokeCap = android.graphics.Paint.Cap.ROUND
            }
            
            val sweepAngle = (progress / maxProgress) * 360f
            canvas.drawArc(rect, -90f, sweepAngle, false, paint)
        }
    }
    
    // Custom waveform view
    class WaveformView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.view.View(context, attrs, defStyleAttr) {
        
        private val data = mutableListOf<Float>()
        private val linePaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.GREEN
            style = android.graphics.Paint.Style.STROKE
            strokeWidth = 4f
        }
        
        private val fillPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.argb(100, 0, 255, 0)
            style = android.graphics.Paint.Style.FILL
        }
        
        fun setData(newData: List<Float>) {
            data.clear()
            data.addAll(newData)
            invalidate()
        }
        
        override fun onDraw(canvas: android.graphics.Canvas) {
            super.onDraw(canvas)
            
            if (data.isEmpty()) return
            
            val path = android.graphics.Path()
            val fillPath = android.graphics.Path()
            
            val width = width.toFloat()
            val height = height.toFloat()
            val pointWidth = if (data.size > 1) width / (data.size - 1) else width
            
            // Build path
            data.forEachIndexed { index, value ->
                val x = index * pointWidth
                val y = height - (value * height)
                
                if (index == 0) {
                    path.moveTo(x, y)
                    fillPath.moveTo(x, height)
                    fillPath.lineTo(x, y)
                } else {
                    path.lineTo(x, y)
                    fillPath.lineTo(x, y)
                }
            }
            
            // Close fill path
            fillPath.lineTo(width, height)
            fillPath.close()
            
            // Draw fill
            canvas.drawPath(fillPath, fillPaint)
            
            // Draw line
            canvas.drawPath(path, linePaint)
        }
    }
}
```

---

## SECTION 5: TOUCH HANDLING

```kotlin
/**
 * Touch Handling
 * 
 * Implementing custom touch interactions.
 */
class TouchHandling {
    
    // Custom draggable view
    class DraggableView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.view.View(context, attrs, defStyleAttr) {
        
        private var lastX = 0f
        private var lastY = 0f
        private var isDragging = false
        private var listener: OnDragListener? = null
        
        interface OnDragListener {
            fun onDragStarted()
            fun onDrag(x: Float, y: Float)
            fun onDragEnded(x: Float, y: Float)
        }
        
        override fun onTouchEvent(event: android.view.MotionEvent): Boolean {
            when (event.action) {
                android.view.MotionEvent.ACTION_DOWN -> {
                    lastX = event.rawX
                    lastY = event.rawY
                    isDragging = true
                    listener?.onDragStarted()
                    parent?.requestDisallowInterceptTouchEvent(true)
                    return true
                }
                android.view.MotionEvent.ACTION_MOVE -> {
                    if (isDragging) {
                        val dx = event.rawX - lastX
                        val dy = event.rawY - lastY
                        
                        x += dx
                        y += dy
                        
                        lastX = event.rawX
                        lastY = event.rawY
                        
                        listener?.onDrag(event.rawX, event.rawY)
                    }
                    return true
                }
                android.view.MotionEvent.ACTION_UP, android.view.MotionEvent.ACTION_CANCEL -> {
                    if (isDragging) {
                        isDragging = false
                        listener?.onDragEnded(event.rawX, event.rawY)
                        parent?.requestDisallowInterceptTouchEvent(false)
                    }
                    return true
                }
            }
            return super.onTouchEvent(event)
        }
        
        fun setOnDragListener(listener: OnDragListener) {
            this.listener = listener
        }
    }
    
    // Custom zoomable image view
    class ZoomableImageView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.ImageView(context, attrs, defStyleAttr) {
        
        private var scaleFactor = 1f
        private var lastTouchX = 0f
        private var lastTouchY = 0f
        private var mode = NONE
        
        private val NONE = 0
        private val DRAG = 1
        private val ZOOM = 2
        private val MIN_SCALE = 1f
        private val MAX_SCALE = 5f
        
        private var startDistance = 0f
        
        override fun onTouchEvent(event: android.view.MotionEvent): Boolean {
            when (event.actionMasked) {
                android.view.MotionEvent.ACTION_DOWN -> {
                    lastTouchX = event.x
                    lastTouchY = event.y
                    mode = DRAG
                }
                android.view.MotionEvent.ACTION_POINTER_DOWN -> {
                    if (event.pointerCount == 2) {
                        startDistance = spacing(event)
                        if (startDistance > 10f) {
                            mode = ZOOM
                        }
                    }
                }
                android.view.MotionEvent.ACTION_MOVE -> {
                    if (mode == ZOOM && event.pointerCount == 2) {
                        val newDistance = spacing(event)
                        if (newDistance > 10f) {
                            scaleFactor = (newDistance / startDistance).coerceIn(MIN_SCALE, MAX_SCALE)
                            scaleX = scaleFactor
                            scaleY = scaleFactor
                        }
                    } else if (mode == DRAG) {
                        val dx = event.x - lastTouchX
                        val dy = event.y - lastTouchY
                        
                        translationX += dx
                        translationY += dy
                        
                        lastTouchX = event.x
                        lastTouchY = event.y
                    }
                }
                android.view.MotionEvent.ACTION_UP, android.view.MotionEvent.ACTION_POINTER_UP -> {
                    mode = NONE
                }
            }
            return true
        }
        
        private fun spacing(event: android.view.MotionEvent): Float {
            val x = event.getX(0) - event.getX(1)
            val y = event.getY(0) - event.getY(1)
            return kotlin.math.sqrt(x * x + y * y)
        }
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: Custom view not displaying**
- Solution: Override onMeasure() correctly, set layout params properly, check that onDraw() is being called

**Pitfall 2: Touch events not working**
- Solution: Return true from onTouchEvent, ensure parent doesn't intercept, check isClickable

**Pitfall 3: View sizing incorrect**
- Solution: Properly implement onMeasure(), use resolveSize() for dimensions, consider padding

**Pitfall 4: Performance issues in onDraw()**
- Solution: Avoid object creation in onDraw(), use canvas save/restore, cache expensive calculations

---

## Best Practices

1. Always call super methods when overriding
2. Use TypedArray for custom attributes
3. Handle all measure specs properly
4. Use hardware acceleration when possible
5. Cache paints and other expensive objects
6. Don't allocate memory in draw methods
7. Support both XML and programmatic creation
8. Document custom attributes
9. Test on different API levels
10. Follow View naming conventions

---

## EXAMPLE 1: COMPLETE CUSTOM BUTTON

```kotlin
/**
 * Complete Custom Button Example
 * 
 * Full-featured custom button with multiple states.
 */
class CustomButton {
    
    class CustomButton @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.widget.TextView(context, attrs, defStyleAttr) {
        
        // Custom attributes
        private var cornerRadius: Float = 16f
        private var buttonBackground: Int = android.graphics.Color.BLUE
        private var pressedColor: Int = android.graphics.Color.DKGRAY
        private var disabledColor: Int = android.graphics.Color.LTGRAY
        
        private val backgroundPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG)
        private val rect = android.graphics.RectF()
        
        init {
            // Read attributes
            context.theme.obtainStyledAttributes(
                attrs,
                R.styleable.CustomButton,
                defStyleAttr,
                0
            ).apply {
                try {
                    cornerRadius = getDimension(R.styleable.CustomButton_cornerRadius, 16f)
                    buttonBackground = getColor(R.styleable.CustomButton_buttonBackground, android.graphics.Color.BLUE)
                    pressedColor = getColor(R.styleable.CustomButton_pressedColor, android.graphics.Color.DKGRAY)
                    disabledColor = getColor(R.styleable.CustomButton_disabledColor, android.graphics.Color.LTGRAY)
                } finally {
                    recycle()
                }
            }
            
            // Set up view
            isClickable = true
            setTextColor(android.graphics.Color.WHITE)
            textSize = 16f
            gravity = android.view.Gravity.CENTER
            setPadding(32, 16, 32, 16)
            
            // Make background transparent so we can draw custom
            setBackgroundColor(android.graphics.Color.TRANSPARENT)
        }
        
        override fun onSizeChanged(w: Int, h: Int, oldw: Int, oldh: Int) {
            super.onSizeChanged(w, h, oldw, oldh)
            rect.set(0f, 0f, w.toFloat(), h.toFloat())
        }
        
        override fun onDraw(canvas: android.graphics.Canvas) {
            // Determine background color based on state
            backgroundPaint.color = when {
                !isEnabled -> disabledColor
                isPressed -> pressedColor
                else -> buttonBackground
            }
            
            // Draw rounded rectangle background
            canvas.drawRoundRect(rect, cornerRadius, cornerRadius, backgroundPaint)
            
            // Call super to draw text
            super.onDraw(canvas)
        }
        
        // Animation for click
        override fun performClick(): Boolean {
            // Animate
            animate()
                .scaleX(0.95f)
                .scaleY(0.95f)
                .setDuration(100)
                .withEndAction {
                    animate()
                        .scaleX(1f)
                        .scaleY(1f)
                        .setDuration(100)
                        .start()
                }
                .start()
            
            return super.performClick()
        }
    }
}
```

---

## EXAMPLE 2: CUSTOM SLIDER COMPONENT

```kotlin
/**
 * Custom Slider Component
 * 
 * Implementing a custom range slider.
 */
class CustomSlider {
    
    class RangeSlider @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.view.View(context, attrs, defStyleAttr) {
        
        private var minValue = 0f
        private var maxValue = 100f
        private var currentValue = 50f
        
        private var onValueChangeListener: ((Float) -> Unit)? = null
        
        private val trackPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.LTGRAY
            strokeWidth = 8f
            style = android.graphics.Paint.Style.STROKE
        }
        
        private val progressPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.BLUE
            strokeWidth = 8f
            style = android.graphics.Paint.Style.STROKE
        }
        
        private val thumbPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.BLUE
            style = android.graphics.Paint.Style.FILL
        }
        
        private val thumbRadius = 20f
        private var thumbX = 0f
        
        init {
            isClickable = true
            isFocusable = true
        }
        
        fun setValue(value: Float) {
            currentValue = value.coerceIn(minValue, maxValue)
            updateThumbPosition()
            invalidate()
        }
        
        fun getValue(): Float = currentValue
        
        fun setOnValueChangeListener(listener: (Float) -> Unit) {
            onValueChangeListener = listener
        }
        
        private fun updateThumbPosition() {
            val range = maxValue - minValue
            val progress = (currentValue - minValue) / range
            thumbX = paddingLeft + progress * (width - paddingLeft - paddingRight)
        }
        
        override fun onSizeChanged(w: Int, h: Int, oldw: Int, oldh: Int) {
            super.onSizeChanged(w, h, oldw, oldh)
            updateThumbPosition()
        }
        
        override fun onDraw(canvas: android.graphics.Canvas) {
            super.onDraw(canvas)
            
            val trackTop = (height / 2f) - trackPaint.strokeWidth / 2
            
            // Draw track
            canvas.drawLine(
                paddingLeft.toFloat(),
                height / 2f,
                (width - paddingRight).toFloat(),
                height / 2f,
                trackPaint
            )
            
            // Draw progress
            canvas.drawLine(
                paddingLeft.toFloat(),
                height / 2f,
                thumbX,
                height / 2f,
                progressPaint
            )
            
            // Draw thumb
            canvas.drawCircle(thumbX, height / 2f, thumbRadius, thumbPaint)
        }
        
        override fun onTouchEvent(event: android.view.MotionEvent): Boolean {
            when (event.action) {
                android.view.MotionEvent.ACTION_DOWN,
                android.view.MotionEvent.ACTION_MOVE -> {
                    updateValueFromTouch(event.x)
                    return true
                }
            }
            return super.onTouchEvent(event)
        }
        
        private fun updateValueFromTouch(touchX: Float) {
            val progress = ((touchX - paddingLeft) / (width - paddingLeft - paddingRight))
                .coerceIn(0f, 1f)
            currentValue = minValue + progress * (maxValue - minValue)
            updateThumbPosition()
            invalidate()
            onValueChangeListener?.invoke(currentValue)
        }
    }
}
```

---

## EXAMPLE 3: CUSTOM CHART VIEW

```kotlin
/**
 * Custom Chart View
 * 
 * Bar chart with custom drawing.
 */
class CustomChart {
    
    class BarChartView @JvmOverloads constructor(
        context: android.content.Context,
        attrs: android.util.AttributeSet? = null,
        defStyleAttr: Int = 0
    ) : android.view.View(context, attrs, defStyleAttr) {
        
        private var data = listOf<Float>()
        private var labels = listOf<String>()
        
        private val barPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.BLUE
            style = android.graphics.Paint.Style.FILL
        }
        
        private val labelPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.BLACK
            textSize = 24f
            textAlign = android.graphics.Paint.Align.CENTER
        }
        
        private val gridPaint = android.graphics.Paint(android.graphics.Paint.ANTI_ALIAS_FLAG).apply {
            color = android.graphics.Color.LTGRAY
            strokeWidth = 2f
        }
        
        fun setData(values: List<Float>, labels: List<String>) {
            this.data = values
            this.labels = labels
            invalidate()
        }
        
        override fun onDraw(canvas: android.graphics.Canvas) {
            super.onDraw(canvas)
            
            if (data.isEmpty()) return
            
            val chartWidth = width - paddingLeft - paddingRight
            val chartHeight = height - paddingTop - paddingBottom
            val maxValue = data.maxOrNull() ?: 1f
            
            val barWidth = chartWidth / data.size.toFloat()
            val gap = barWidth * 0.2f
            val actualBarWidth = barWidth - gap
            
            // Draw grid lines
            for (i in 0..4) {
                val y = paddingTop + (chartHeight * i / 4)
                canvas.drawLine(
                    paddingLeft.toFloat(),
                    y,
                    (width - paddingRight).toFloat(),
                    y,
                    gridPaint
                )
            }
            
            // Draw bars
            data.forEachIndexed { index, value ->
                val barHeight = (value / maxValue) * chartHeight
                val left = paddingLeft + index * barWidth + gap / 2
                val top = paddingTop + chartHeight - barHeight
                val right = left + actualBarWidth
                val bottom = paddingTop + chartHeight.toFloat()
                
                canvas.drawRect(left, top, right, bottom, barPaint)
                
                // Draw label if available
                if (index < labels.size) {
                    canvas.drawText(
                        labels[index],
                        left + actualBarWidth / 2,
                        height - paddingBottom / 2f,
                        labelPaint
                    )
                }
            }
        }
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**Custom View Types:**
- Extending existing views: StyledTextView, LoadingButton
- Composite views: CustomCardView, RatingBarView
- Full custom drawing: CircularProgressView, WaveformView
- Touch handling: DraggableView, ZoomableImageView

**Key Override Methods:**
- onMeasure: Set view size
- onLayout: Position children (ViewGroup)
- onDraw: Render content
- onTouchEvent: Handle input

**Custom Attributes:**
- TypedArray for XML attributes
- R.styleable for attribute definitions
- defStyleAttr for default styles

**Drawing Components:**
- Paint objects for styling
- Path for custom shapes
- Canvas for drawing operations
- RectF for rounded rectangles

**Touch Handling:**
- MotionEvent for input
- ACTION_DOWN/MOVE/UP states
- Parent touch interception
- Multi-touch support

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/02_LinearLayout_and_RelativeLayout.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/05_Material_Design_Implementation.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md

---

## END OF CUSTOM VIEWS AND COMPONENTS GUIDE
