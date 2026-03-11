/**
 * Product Model - Section 2 Code Example
 * This TypeScript model represents a Product in our application
 * 
 * In the full-stack architecture, this model is used in:
 * - Angular frontend (TypeScript)
 * - Spring Boot backend (Java entity)
 * - JSON data transfer
 */

// Define the Product interface
export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  category: string;
  inStock: boolean;
  imageUrl?: string;
  createdAt?: Date;
  updatedAt?: Date;
}

// Example products that match our database structure
export const sampleProducts: Product[] = [
  {
    id: 1,
    name: "Laptop Pro 15",
    description: "High-performance laptop for professionals",
    price: 1299.99,
    category: "Electronics",
    inStock: true,
    imageUrl: "https://example.com/images/laptop.jpg"
  },
  {
    id: 2,
    name: "Wireless Mouse",
    description: "Ergonomic wireless mouse",
    price: 49.99,
    category: "Electronics",
    inStock: true,
    imageUrl: "https://example.com/images/mouse.jpg"
  },
  {
    id: 3,
    name: "Office Chair",
    description: "Comfortable office chair with lumbar support",
    price: 299.99,
    category: "Furniture",
    inStock: false,
    imageUrl: "https://example.com/images/chair.jpg"
  }
];

/**
 * Category interface for organizing products
 */
export interface Category {
  id: number;
  name: string;
  description: string;
}

/**
 * Sample categories
 */
export const sampleCategories: Category[] = [
  { id: 1, name: "Electronics", description: "Electronic devices and accessories" },
  { id: 2, name: "Furniture", description: "Home and office furniture" },
  { id: 3, name: "Books", description: "Books and publications" },
  { id: 4, name: "Clothing", description: "Apparel and accessories" }
];

/**
 * API Response wrapper
 * All API responses will follow this format
 */
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  timestamp: Date;
}

/**
 * Example of how the API response looks for products
 */
export const sampleApiResponse: ApiResponse<Product[]> = {
  success: true,
  message: "Products retrieved successfully",
  data: sampleProducts,
  timestamp: new Date()
};

/**
 * Product Filter interface for search functionality
 */
export interface ProductFilter {
  searchTerm?: string;
  category?: string;
  minPrice?: number;
  maxPrice?: number;
  inStockOnly?: boolean;
}
