/**
 * Product Management Application
 * Section 4: JavaScript Essentials - Complete Example
 */

// ============================================
// PRODUCT DATA
// ============================================
const products = [
    {
        id: 1,
        name: "Laptop Pro 15",
        description: "High-performance laptop with 16GB RAM",
        price: 1299.99,
        category: "Electronics",
        inStock: true,
        image: "https://via.placeholder.com/300x200?text=Laptop"
    },
    {
        id: 2,
        name: "Wireless Mouse",
        description: "Ergonomic wireless mouse",
        price: 49.99,
        category: "Accessories",
        inStock: true,
        image: "https://via.placeholder.com/300x200?text=Mouse"
    },
    {
        id: 3,
        name: "Office Chair",
        description: "Comfortable ergonomic chair",
        price: 299.99,
        category: "Furniture",
        inStock: false,
        image: "https://via.placeholder.com/300x200?text=Chair"
    },
    {
        id: 4,
        name: "Mechanical Keyboard",
        description: "RGB mechanical keyboard",
        price: 129.99,
        category: "Accessories",
        inStock: true,
        image: "https://via.placeholder.com/300x200?text=Keyboard"
    }
];

// ============================================
// DOM ELEMENTS
// ============================================
const productContainer = document.getElementById('product-container');
const searchInput = document.getElementById('search-input');
const categoryFilter = document.getElementById('category-filter');
const addProductBtn = document.getElementById('add-product-btn');
const productModal = document.getElementById('product-modal');
const closeModal = document.querySelector('.close-modal');
const productForm = document.getElementById('product-form');

// ============================================
// RENDER FUNCTIONS
// ============================================

/**
 * Render a single product card
 */
function createProductCard(product) {
    const card = document.createElement('div');
    card.className = 'product-card';
    card.innerHTML = `
        <div class="product-image-container">
            <img src="${product.image}" alt="${product.name}" class="product-image">
            ${!product.inStock ? '<span class="badge badge-out">Out of Stock</span>' : ''}
        </div>
        <div class="product-details">
            <h3 class="product-name">${product.name}</h3>
            <p class="product-category">${product.category}</p>
            <p class="product-description">${product.description}</p>
            <div class="product-footer">
                <span class="product-price">$${product.price.toFixed(2)}</span>
                <button class="btn btn-add-cart" onclick="addToCart(${product.id})">
                    ${product.inStock ? 'Add to Cart' : 'Notify Me'}
                </button>
            </div>
            <div class="product-actions">
                <button class="btn-icon" onclick="editProduct(${product.id})" title="Edit">
                    ✏️
                </button>
                <button class="btn-icon" onclick="deleteProduct(${product.id})" title="Delete">
                    🗑️
                </button>
            </div>
        </div>
    `;
    return card;
}

/**
 * Render all products
 */
function renderProducts(productsToRender) {
    productContainer.innerHTML = '';
    
    if (productsToRender.length === 0) {
        productContainer.innerHTML = '<p class="no-products">No products found.</p>';
        return;
    }
    
    productsToRender.forEach(product => {
        const card = createProductCard(product);
        productContainer.appendChild(card);
    });
}

// ============================================
// FILTER FUNCTIONS
// ============================================

/**
 * Filter products by search term
 */
function filterBySearch(products, searchTerm) {
    if (!searchTerm) return products;
    
    const term = searchTerm.toLowerCase();
    return products.filter(product => 
        product.name.toLowerCase().includes(term) ||
        product.description.toLowerCase().includes(term)
    );
}

/**
 * Filter products by category
 */
function filterByCategory(products, category) {
    if (!category || category === 'all') return products;
    return products.filter(product => product.category === category);
}

/**
 * Apply all filters
 */
function applyFilters() {
    const searchTerm = searchInput.value;
    const category = categoryFilter.value;
    
    let filtered = filterBySearch(products, searchTerm);
    filtered = filterByCategory(filtered, category);
    
    renderProducts(filtered);
}

// ============================================
// CRUD OPERATIONS
// ============================================

/**
 * Add a new product
 */
function addProduct(productData) {
    const newProduct = {
        id: Date.now(), // Simple ID generation
        ...productData,
        image: "https://via.placeholder.com/300x200?text=New+Product"
    };
    
    products.push(newProduct);
    applyFilters();
    showNotification('Product added successfully!', 'success');
}

/**
 * Edit an existing product
 */
function editProduct(id) {
    const product = products.find(p => p.id === id);
    if (!product) return;
    
    // Populate form
    document.getElementById('product-name').value = product.name;
    document.getElementById('product-description').value = product.description;
    document.getElementById('product-price').value = product.price;
    document.getElementById('product-category').value = product.category;
    document.getElementById('product-stock').checked = product.inStock;
    
    // Store ID for update
    productForm.dataset.editId = id;
    
    // Show modal
    openModal();
}

/**
 * Update a product
 */
function updateProduct(id, productData) {
    const index = products.findIndex(p => p.id === parseInt(id));
    if (index === -1) return;
    
    products[index] = { ...products[index], ...productData };
    applyFilters();
    showNotification('Product updated successfully!', 'success');
}

/**
 * Delete a product
 */
function deleteProduct(id) {
    if (!confirm('Are you sure you want to delete this product?')) return;
    
    const index = products.findIndex(p => p.id === id);
    if (index === -1) return;
    
    products.splice(index, 1);
    applyFilters();
    showNotification('Product deleted successfully!', 'success');
}

/**
 * Add product to cart
 */
function addToCart(productId) {
    const product = products.find(p => p.id === productId);
    if (!product) return;
    
    if (!product.inStock) {
        showNotification('This product is out of stock!', 'warning');
        return;
    }
    
    showNotification(`Added "${product.name}" to cart!`, 'success');
}

// ============================================
// MODAL FUNCTIONS
// ============================================

/**
 * Open the product modal
 */
function openModal() {
    productModal.classList.add('show');
    document.body.style.overflow = 'hidden';
}

/**
 * Close the product modal
 */
function closeModalHandler() {
    productModal.classList.remove('show');
    document.body.style.overflow = 'auto';
    productForm.reset();
    delete productForm.dataset.editId;
}

/**
 * Handle form submission
 */
function handleFormSubmit(event) {
    event.preventDefault();
    
    const productData = {
        name: document.getElementById('product-name').value,
        description: document.getElementById('product-description').value,
        price: parseFloat(document.getElementById('product-price').value),
        category: document.getElementById('product-category').value,
        inStock: document.getElementById('product-stock').checked
    };
    
    const editId = productForm.dataset.editId;
    
    if (editId) {
        updateProduct(editId, productData);
    } else {
        addProduct(productData);
    }
    
    closeModalHandler();
}

// ============================================
// NOTIFICATION SYSTEM
// ============================================

/**
 * Show a notification message
 */
function showNotification(message, type = 'info') {
    // Remove existing notification
    const existing = document.querySelector('.notification');
    if (existing) existing.remove();
    
    // Create notification
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.innerHTML = `
        <span>${message}</span>
        <button onclick="this.parentElement.remove()">×</button>
    `;
    
    // Add to page
    document.body.appendChild(notification);
    
    // Auto remove after 3 seconds
    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// ============================================
// EVENT LISTENERS
// ============================================

// Search input
searchInput.addEventListener('input', applyFilters);

// Category filter
categoryFilter.addEventListener('change', applyFilters);

// Add product button
addProductBtn.addEventListener('click', openModal);

// Close modal
closeModal.addEventListener('click', closeModalHandler);
productModal.addEventListener('click', (e) => {
    if (e.target === productModal) closeModalHandler();
});

// Form submission
productForm.addEventListener('submit', handleFormSubmit);

// Keyboard events
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') closeModalHandler();
});

// ============================================
// INITIALIZATION
// ============================================

// Initialize the app
document.addEventListener('DOMContentLoaded', () => {
    renderProducts(products);
    console.log('Product Management App initialized!');
});
