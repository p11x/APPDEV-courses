# LEARNING OBJECTIVES

1. Understanding RecyclerView architecture
2. Implementing list adapters with ViewHolder pattern
3. Managing different view types
4. Implementing item decorations and animations
5. Optimizing list performance

```kotlin
package com.android.ui.recyclerview
```

---

## SECTION 1: RECYCLERVIEW OVERVIEW

```kotlin
/**
 * RecyclerView Overview
 * 
 * RecyclerView is Android's most efficient list display widget.
 * It implements the ViewHolder pattern and supports various layouts.
 */
object RecyclerViewOverview {
    
    // Key components
    object Components {
        const val RECYCLERVIEW = "androidx.recyclerview:recyclerview:1.3.2"
        
        // Dependencies needed
        val dependencies = listOf(
            "androidx.recyclerview:recyclerview:1.3.2",
            "androidx.recyclerview:recyclerview-selection:1.1.0"
        )
    }
    
    // Layout managers
    enum class LayoutManager(val className: String) {
        LINEAR_VERTICAL("LinearLayoutManager"),
        LINEAR_HORIZONTAL("LinearLayoutManager"),
        GRID("GridLayoutManager"),
        STAGGERED_GRID("StaggeredGridLayoutManager")
    }
    
    // Key features
    val features = listOf(
        "ViewHolder pattern for efficient recycling",
        "Multiple layout managers (List, Grid, Staggered)",
        "Item animations",
        "Item decorations",
        "Selection support",
        "DiffUtil for efficient updates"
    )
}
```

---

## SECTION 2: ADAPTER IMPLEMENTATION

```kotlin
/**
 * RecyclerView Adapter Implementation
 * 
 * Complete adapter with ViewHolder pattern.
 */
class RecyclerViewAdapter {
    
    // Data model
    data class Item(val id: Int, val title: String, val subtitle: String, val imageUrl: String?)
    
    // ViewHolder
    class ViewHolder(private val binding: ItemLayoutBinding) : 
        androidx.recyclerview.widget.RecyclerView.ViewHolder(binding.root) {
        
        fun bind(item: Item) {
            binding.titleText.text = item.title
            binding.subtitleText.text = item.subtitle
            
            item.imageUrl?.let { url ->
                // Load image with your image library
                // Glide.with(binding.root).load(url).into(binding.imageView)
                binding.imageView.visibility = android.view.View.VISIBLE
            } ?: run {
                binding.imageView.visibility = android.view.View.GONE
            }
        }
        
        fun bindClickListener(listener: (Item) -> Unit, item: Item) {
            binding.root.setOnClickListener { listener(item) }
        }
    }
    
    // Adapter
    class Adapter(private var items: List<Item> = emptyList()) : 
        androidx.recyclerview.widget.RecyclerView.Adapter<ViewHolder>() {
        
        private var onItemClick: ((Item) -> Unit)? = null
        
        override fun onCreateViewHolder(parent: android.view.ViewGroup, viewType: Int): ViewHolder {
            val binding = ItemLayoutBinding.inflate(
                android.view.LayoutInflater.from(parent.context),
                parent,
                false
            )
            return ViewHolder(binding)
        }
        
        override fun onBindViewHolder(holder: ViewHolder, position: Int) {
            val item = items[position]
            holder.bind(item)
            holder.bindClickListener({ onItemClick?.invoke(it) }, item)
        }
        
        override fun getItemCount(): Int = items.size
        
        fun setItems(newItems: List<Item>) {
            val diffCallback = DiffUtilCallback(items, newItems)
            val diffResult = androidx.recyclerview.widget.DiffUtil.calculateDiff(diffCallback)
            items = newItems
            diffResult.dispatchUpdatesTo(this)
        }
        
        fun setOnItemClickListener(listener: (Item) -> Unit) {
            onItemClick = listener
        }
        
        // DiffUtil for efficient updates
        class DiffUtilCallback(
            private val oldList: List<Item>,
            private val newList: List<Item>
        ) : androidx.recyclerview.widget.DiffUtil.Callback() {
            
            override fun getOldListSize(): Int = oldList.size
            override fun getNewListSize(): Int = newList.size
            
            override fun areItemsTheSame(oldPos: Int, newPos: Int): Boolean {
                return oldList[oldPos].id == newList[newPos].id
            }
            
            override fun areContentsTheSame(oldPos: Int, newPos: Int): Boolean {
                return oldList[oldPos] == newList[newPos]
            }
        }
    }
    
    // View Binding class (placeholder)
    class ItemLayoutBinding(
        val root: android.view.View,
        val titleText: android.widget.TextView,
        val subtitleText: android.widget.TextView,
        val imageView: android.widget.ImageView
    ) {
        companion object {
            fun inflate(inflater: android.view.LayoutInflater, parent: android.view.ViewGroup, attachToParent: Boolean): ItemLayoutBinding {
                val view = inflater.inflate(R.layout.item_layout, parent, attachToParent)
                return ItemLayoutBinding(
                    view,
                    view.findViewById(R.id.title_text),
                    view.findViewById(R.id.subtitle_text),
                    view.findViewById(R.id.image_view)
                )
            }
        }
    }
}
```

---

## SECTION 3: LAYOUT MANAGERS

```kotlin
/**
 * Layout Managers
 * 
 * Different layout managers for various display patterns.
 */
class LayoutManagers {
    
    // Linear Layout Manager
    fun getLinearLayoutManager(context: android.content.Context): androidx.recyclerview.widget.LinearLayoutManager {
        return androidx.recyclerview.widget.LinearLayoutManager(context).apply {
            // Vertical (default)
            orientation = androidx.recyclerview.widget.LinearLayoutManager.VERTICAL
            
            // Reverse layout
            reverseLayout = false
            
            // Stack from end
            stackFromEnd = false
        }
    }
    
    // Horizontal
    fun getHorizontalLayoutManager(context: android.content.Context): androidx.recyclerview.widget.LinearLayoutManager {
        return androidx.recyclerview.widget.LinearLayoutManager(
            context,
            androidx.recyclerview.widget.LinearLayoutManager.HORIZONTAL,
            false
        )
    }
    
    // Grid Layout Manager
    fun getGridLayoutManager(context: android.content.Context, spanCount: Int): androidx.recyclerview.widget.GridLayoutManager {
        return androidx.recyclerview.widget.GridLayoutManager(context, spanCount).apply {
            // Span size lookup for different item sizes
            spanSizeLookup = object : androidx.recyclerview.widget.GridLayoutManager.SpanSizeLookup() {
                override fun getSpanSize(position: Int): Int {
                    // Make first item span full width
                    return if (position == 0) spanCount else 1
                }
            }
        }
    }
    
    // Staggered Grid
    fun getStaggeredGridLayoutManager(spanCount: Int, orientation: Int): androidx.recyclerview.widget.StaggeredGridLayoutManager {
        return androidx.recyclerview.widget.StaggeredGridLayoutManager(
            spanCount,
            orientation
        ).apply {
            // Gap strategy
            gapStrategy = androidx.recyclerview.widget.StaggeredGridLayoutManager.GAP_HANDLING_MOVE_ITEMS_BETWEEN_SPANS
        }
    }
}
```

---

## SECTION 4: ITEM DECORATIONS AND ANIMATIONS

```kotlin
/**
 * Item Decorations and Animations
 * 
 * Customizing list appearance with decorations and animations.
 */
class DecorationsAndAnimations {
    
    // Custom ItemDecoration
    class DividerDecoration(context: android.content.Context) : 
        androidx.recyclerview.widget.RecyclerView.ItemDecoration() {
        
        private val divider = android.graphics.drawable.ColorDrawable(
            context.getColor(android.R.color.darker_gray)
        ).apply {
            alpha = 50
        }
        
        override fun onDraw(c: android.graphics.Canvas, parent: androidx.recyclerview.widget.RecyclerView, state: androidx.recyclerview.widget.RecyclerView.State) {
            val left = parent.paddingLeft
            val right = parent.width - parent.paddingRight
            
            for (i in 0 until parent.childCount - 1) {
                val child = parent.getChildAt(i)
                val params = child.layoutParams as androidx.recyclerview.widget.RecyclerView.LayoutParams
                val top = child.bottom + params.bottomMargin
                val bottom = top + 1 // divider height
                
                divider.setBounds(left, top, right, bottom)
                divider.draw(c)
            }
        }
    }
    
    // Spacing decoration
    class SpacingDecoration(private val spacing: Int) : 
        androidx.recyclerview.widget.RecyclerView.ItemDecoration() {
        
        override fun getItemOffsets(
            outRect: android.graphics.Rect,
            view: android.view.View,
            parent: androidx.recyclerview.widget.RecyclerView,
            state: androidx.recyclerview.widget.RecyclerView.State
        ) {
            val position = parent.getChildAdapterPosition(view)
            val itemCount = state.itemCount
            
            // Add spacing between items
            outRect.left = spacing
            outRect.right = spacing
            outRect.bottom = spacing
            
            // Add top spacing for first item only
            if (position == 0) {
                outRect.top = spacing
            }
        }
    }
    
    // Custom animator
    class CustomItemAnimator : androidx.recyclerview.widget.DefaultItemAnimator() {
        override fun animateAdd(holder: androidx.recyclerview.widget.RecyclerView.ViewHolder): Boolean {
            holder.itemView.alpha = 0f
            holder.itemView.translationY = holder.itemView.height.toFloat() / 4
            
            holder.itemView.animate()
                .alpha(1f)
                .translationY(0f)
                .setDuration(addDuration)
                .setInterpolator(android.view.animation.DecelerateInterpolator())
                .start()
            
            return true
        }
    }
}
```

---

## SECTION 5: VIEW TYPES AND SELECTION

```kotlin
/**
 * View Types and Selection
 * 
 * Handling multiple view types and user selection.
 */
class ViewTypesAndSelection {
    
    // Multiple view types adapter
    class MultiTypeAdapter : androidx.recyclerview.widget.RecyclerView.Adapter<androidx.recyclerview.widget.RecyclerView.ViewHolder>() {
        
        companion object {
            const val TYPE_HEADER = 0
            const val TYPE_ITEM = 1
            const val TYPE_FOOTER = 2
        }
        
        private val items = mutableListOf<Any>()
        
        override fun getItemViewType(position: Int): Int {
            return when (items[position]) {
                is String -> TYPE_HEADER  // Header
                is Footer -> TYPE_FOOTER
                else -> TYPE_ITEM
            }
        }
        
        override fun onCreateViewHolder(parent: android.view.ViewGroup, viewType: Int): androidx.recyclerview.widget.RecyclerView.ViewHolder {
            return when (viewType) {
                TYPE_HEADER -> HeaderViewHolder(
                    android.view.LayoutInflater.from(parent.context)
                        .inflate(R.layout.item_header, parent, false)
                )
                TYPE_FOOTER -> FooterViewHolder(
                    android.view.LayoutInflater.from(parent.context)
                        .inflate(R.layout.item_footer, parent, false)
                )
                else -> ItemViewHolder(
                    android.view.LayoutInflater.from(parent.context)
                        .inflate(R.layout.item_normal, parent, false)
                )
            }
        }
        
        override fun onBindViewHolder(holder: androidx.recyclerview.widget.RecyclerView.ViewHolder, position: Int) {
            when (holder) {
                is HeaderViewHolder -> holder.bind(items[position] as String)
                is FooterViewHolder -> holder.bind(items[position] as Footer)
                is ItemViewHolder -> holder.bind(items[position] as Item)
            }
        }
        
        override fun getItemCount(): Int = items.size
        
        class HeaderViewHolder(view: android.view.View) : 
            androidx.recyclerview.widget.RecyclerView.ViewHolder(view) {
            private val textView: android.widget.TextView = view.findViewById(R.id.header_text)
            fun bind(header: String) {
                textView.text = header
            }
        }
        
        class FooterViewHolder(view: android.view.View) : 
            androidx.recyclerview.widget.RecyclerView.ViewHolder(view) {
            private val button: android.widget.Button = view.findViewById(R.id.load_more)
            fun bind(footer: Footer) {
                button.text = footer.buttonText
                button.setOnClickListener { footer.onClick() }
            }
        }
        
        class ItemViewHolder(view: android.view.View) : 
            androidx.recyclerview.widget.RecyclerView.ViewHolder(view) {
            fun bind(item: Item) { /* bind item */ }
        }
        
        data class Item(val id: Int, val name: String)
        data class Footer(val buttonText: String, val onClick: () -> Unit)
    }
    
    // Selection support
    class SelectableAdapter : androidx.recyclerview.widget.ListAdapter<Item, SelectableAdapter.ViewHolder>(
        ItemDiffCallback()
    ) {
        
        private val selectedItems = mutableSetOf<Int>()
        var onSelectionChanged: ((Set<Int>) -> Unit)? = null
        
        fun toggleSelection(position: Int) {
            val id = getItem(position).id
            if (selectedItems.contains(id)) {
                selectedItems.remove(id)
            } else {
                selectedItems.add(id)
            }
            notifyItemChanged(position)
            onSelectionChanged?.invoke(selectedItems)
        }
        
        fun clearSelection() {
            selectedItems.clear()
            notifyDataSetChanged()
        }
        
        fun isSelected(position: Int): Boolean {
            return selectedItems.contains(getItem(position).id)
        }
        
        override fun onBindViewHolder(holder: ViewHolder, position: Int) {
            val item = getItem(position)
            holder.bind(item, isSelected(position))
            holder.itemView.setOnClickListener { toggleSelection(position) }
            holder.itemView.setOnLongClickListener { 
                toggleSelection(position)
                true
            }
        }
        
        class ViewHolder(view: android.view.View) : 
            androidx.recyclerview.widget.RecyclerView.ViewHolder(view) {
            
            private val textView: android.widget.TextView = view.findViewById(R.id.text)
            private val checkView: android.view.View = view.findViewById(R.id.check)
            
            fun bind(item: Item, selected: Boolean) {
                textView.text = item.name
                checkView.visibility = if (selected) 
                    android.view.View.VISIBLE else android.view.View.GONE
                
                // Visual feedback for selection
                holder.itemView.alpha = if (selected) 0.7f else 1.0f
            }
        }
        
        class ItemDiffCallback : androidx.recyclerview.widget.DiffUtil.ItemCallback<Item>() {
            override fun areItemsTheSame(oldItem: Item, newItem: Item) = oldItem.id == newItem.id
            override fun areContentsTheSame(oldItem: Item, newItem: Item) = oldItem == newItem
        }
    }
}
```

---

## Common Pitfalls and Solutions

**Pitfall 1: List not updating**
- Solution: Call notifyDataSetChanged() or DiffUtil, don't modify list directly, create new one, ensure adapter is attached

**Pitfall 2: Wrong item count**
- Solution: Override getItemCount() correctly, return correct number of items, check adapter is set to RecyclerView

**Pitfall 3: Performance issues**
- Solution: Use DiffUtil for updates, enable stable IDs if possible, avoid complex layouts in ViewHolder, use setHasFixedSize appropriately

**Pitfall 4: Nested scrolling**
- Solution: Set nestedScrollingEnabled = false for nested RecyclerViews, use CoordinatorLayout for scrollable content

---

## Best Practices

1. Use ViewHolder pattern
2. Implement DiffUtil for updates
3. Use stable IDs when possible
4. Set fixed size when appropriate
5. Use item decorations for spacing
6. Handle view types for mixed content
7. Use selection carefully
8. Consider ViewBinding in ViewHolder
9. Use onViewRecycled for cleanup
10. Test with large datasets

---

## EXAMPLE 1: COMPLETE RECYCLERVIEW SETUP

```kotlin
/**
 * Complete RecyclerView Setup Example
 * 
 * Full implementation with all components.
 */
class CompleteRecyclerViewSetup {
    
    // Activity/Fragment setup
    class Setup : android.app.Activity() {
        
        private lateinit var recyclerView: androidx.recyclerview.widget.RecyclerView
        private lateinit var adapter: RecyclerViewAdapter.Adapter
        private lateinit var layoutManager: androidx.recyclerview.widget.LinearLayoutManager
        
        override fun onCreate(savedInstanceState: android.os.Bundle?) {
            super.onCreate(savedInstanceState)
            setContentView(R.layout.activity_main)
            
            setupRecyclerView()
            loadData()
        }
        
        private fun setupRecyclerView() {
            recyclerView = findViewById(R.id.recycler_view)
            
            // Layout manager
            layoutManager = androidx.recyclerview.widget.LinearLayoutManager(this)
            recyclerView.layoutManager = layoutManager
            
            // Adapter
            adapter = RecyclerViewAdapter.Adapter()
            recyclerView.adapter = adapter
            
            // Optional: Fixed size for performance
            recyclerView.setHasFixedSize(true)
            
            // Item decoration
            recyclerView.addItemDecoration(
                DecorationsAndAnimations.SpacingDecoration(16)
            )
            
            // Animation
            recyclerView.itemAnimator = androidx.recyclerview.widget.DefaultItemAnimator()
            
            // Click listener
            adapter.setOnItemClickListener { item ->
                // Handle click
                println("Clicked: ${item.title}")
            }
        }
        
        private fun loadData() {
            val items = listOf(
                RecyclerViewAdapter.Item(1, "Item 1", "Description 1", null),
                RecyclerViewAdapter.Item(2, "Item 2", "Description 2", null),
                RecyclerViewAdapter.Item(3, "Item 3", "Description 3", null)
            )
            adapter.setItems(items)
        }
    }
}
```

---

## EXAMPLE 2: GRID WITH HEADER

```kotlin
/**
 * Grid with Header Example
 * 
 * Implementing a grid with section headers.
 */
class GridWithHeader {
    
    // Section data
    data class Section(val title: String, val items: List<String>)
    
    // Adapter with header support
    class SectionedGridAdapter : androidx.recyclerview.widget.RecyclerView.Adapter<androidx.recyclerview.widget.RecyclerView.ViewHolder>() {
        
        companion object {
            private const val TYPE_SECTION_HEADER = 0
            private const val TYPE_ITEM = 1
        }
        
        private var sections = listOf<Section>()
        private var allItems = mutableListOf<Any>()
        
        init {
            rebuildItems()
        }
        
        private fun rebuildItems() {
            allItems.clear()
            sections.forEach { section ->
                allItems.add(section.title) // Header
                allItems.addAll(section.items) // Items
            }
        }
        
        fun setSections(newSections: List<Section>) {
            sections = newSections
            rebuildItems()
            notifyDataSetChanged()
        }
        
        override fun getItemViewType(position: Int): Int {
            return if (allItems[position] is String) TYPE_SECTION_HEADER else TYPE_ITEM
        }
        
        override fun onCreateViewHolder(parent: android.view.ViewGroup, viewType: Int): androidx.recyclerview.widget.RecyclerView.ViewHolder {
            return when (viewType) {
                TYPE_SECTION_HEADER -> HeaderViewHolder(
                    android.view.LayoutInflater.from(parent.context)
                        .inflate(R.layout.item_header, parent, false)
                )
                else -> ItemViewHolder(
                    android.view.LayoutInflater.from(parent.context)
                        .inflate(R.layout.item_grid, parent, false)
                )
            }
        }
        
        override fun onBindViewHolder(holder: androidx.recyclerview.widget.RecyclerView.ViewHolder, position: Int) {
            when (holder) {
                is HeaderViewHolder -> holder.bind(allItems[position] as String)
                is ItemViewHolder -> holder.bind(allItems[position] as String)
            }
        }
        
        override fun getItemCount(): Int = allItems.size
        
        // Span size for header
        fun getSpanSize(position: Int): Int {
            return if (getItemViewType(position) == TYPE_SECTION_HEADER) 2 else 1
        }
        
        class HeaderViewHolder(view: android.view.View) : 
            androidx.recyclerview.widget.RecyclerView.ViewHolder(view) {
            val textView: android.widget.TextView = view.findViewById(R.id.header_text)
            fun bind(title: String) { textView.text = title }
        }
        
        class ItemViewHolder(view: android.view.View) : 
            androidx.recyclerview.widget.RecyclerView.ViewHolder(view) {
            val textView: android.widget.TextView = view.findViewById(R.id.item_text)
            fun bind(item: String) { textView.text = item }
        }
    }
}
```

---

## EXAMPLE 3: SWIPE TO DELETE WITH DRAG

```kotlin
/**
 * Swipe to Delete with Drag and Drop
 * 
 * Implementing swipe and drag support.
 */
class SwipeAndDragAdapter {
    
    // ItemTouchHelper callback for swipe and drag
    class ItemTouchHelperCallback(
        private val adapter: androidx.recyclerview.widget.RecyclerView.Adapter<androidx.recyclerview.widget.RecyclerView.ViewHolder>,
        private val onSwipeDelete: (Int) -> Unit,
        private val onMoveComplete: () -> Unit = {}
    ) : androidx.recyclerview.widget.ItemTouchHelper.Callback() {
        
        private var dragFrom = -1
        private var dragTo = -1
        
        override fun getMovementFlags(
            recyclerView: androidx.recyclerview.widget.RecyclerView,
            viewHolder: androidx.recyclerview.widget.RecyclerView.ViewHolder
        ): Int {
            // Drag and swipe flags
            val dragFlags = androidx.recyclerview.widget.ItemTouchHelper.UP or 
                           androidx.recyclerview.widget.ItemTouchHelper.DOWN
            val swipeFlags = androidx.recyclerview.widget.ItemTouchHelper.START or 
                            androidx.recyclerview.widget.ItemTouchHelper.END
            return makeMovementFlags(dragFlags, swipeFlags)
        }
        
        override fun onMove(
            recyclerView: androidx.recyclerview.widget.RecyclerView,
            viewHolder: androidx.recyclerview.widget.RecyclerView.ViewHolder,
            target: androidx.recyclerview.widget.RecyclerView.ViewHolder
        ): Boolean {
            val fromPos = viewHolder.adapterPosition
            val toPos = target.adapterPosition
            
            if (dragFrom == -1) {
                dragFrom = fromPos
            }
            dragTo = toPos
            
            // Notify adapter for visual feedback
            adapter.notifyItemMoved(fromPos, toPos)
            return true
        }
        
        override fun onSwiped(viewHolder: androidx.recyclerview.widget.RecyclerView.ViewHolder, direction: Int) {
            val position = viewHolder.adapterPosition
            onSwipeDelete(position)
        }
        
        override fun clearView(recyclerView: androidx.recyclerview.widget.RecyclerView, viewHolder: androidx.recyclerview.widget.RecyclerView.ViewHolder) {
            super.clearView(recyclerView, viewHolder)
            
            // Drag completed
            if (dragFrom != -1 && dragTo != -1 && dragFrom != dragTo) {
                onMoveComplete()
            }
            
            dragFrom = -1
            dragTo = -1
        }
        
        override fun isLongPressDragEnabled(): Boolean = true
        override fun isItemViewSwipeEnabled(): Boolean = true
    }
    
    // Usage
    fun setupSwipeAndDrag(recyclerView: androidx.recyclerview.widget.RecyclerView) {
        val callback = ItemTouchHelperCallback(
            recyclerView.adapter!!,
            onSwipeDelete = { position ->
                // Handle delete
                println("Swiped position: $position")
            },
            onMoveComplete = {
                println("Drag complete")
            }
        )
        
        val touchHelper = androidx.recyclerview.widget.ItemTouchHelper(callback)
        touchHelper.attachToRecyclerView(recyclerView)
    }
}
```

---

## OUTPUT STATEMENT RESULTS

**RecyclerView Components:**
- RecyclerView: The main widget
- LayoutManager: List, Grid, Staggered
- Adapter: ViewHolder implementation
- ItemDecoration: Visual dividers/spacing
- ItemAnimator: Add/remove animations

**Adapter Implementation:**
- ViewHolder: Efficient view binding
- DiffUtil: Efficient updates
- onCreateViewHolder: Create views
- onBindViewHolder: Bind data

**Layout Managers:**
- LinearLayoutManager: List view
- GridLayoutManager: Grid view
- StaggeredGridLayoutManager: Staggered items
- SpanSizeLookup: Variable span sizes

**Item Decorations:**
- DividerDecoration: Line dividers
- SpacingDecoration: Uniform spacing
- Custom decorations for any visual effect

**View Types:**
- getItemViewType for multiple types
- Selection support with toggle
- Stable IDs for better performance

---

## CROSS-REFERENCES

- See: 02_UI_DEVELOPMENT/01_XML_Layouts/01_ConstraintLayout_Fundamentals.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/02_LinearLayout_and_RelativeLayout.md
- See: 02_UI_DEVELOPMENT/01_XML_Layouts/04_Custom_Views_and_Components.md
- See: 02_UI_DEVELOPMENT/02_Jetpack_Compose/01_Compose_Basics_and_Setup.md

---

## END OF RECYCLERVIEW IMPLEMENTATION GUIDE
