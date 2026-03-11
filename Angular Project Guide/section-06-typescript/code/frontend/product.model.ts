/**
 * Product Model - TypeScript Interface
 * Section 6: TypeScript Fundamentals
 */

// Basic Product Interface
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

// Create Product DTO (Data Transfer Object)
export interface CreateProductDto {
  name: string;
  description: string;
  price: number;
  category: string;
  inStock: boolean;
  imageUrl?: string;
}

// Update Product DTO (all fields optional)
export interface UpdateProductDto {
  name?: string;
  description?: string;
  price?: number;
  category?: string;
  inStock?: boolean;
  imageUrl?: string;
}

// API Response wrapper
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  timestamp: Date;
}

// Paginated Response
export interface PaginatedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

// Product Filter
export interface ProductFilter {
  searchTerm?: string;
  category?: string;
  minPrice?: number;
  maxPrice?: number;
  inStockOnly?: boolean;
  sortBy?: 'name' | 'price' | 'createdAt';
  sortOrder?: 'asc' | 'desc';
}
