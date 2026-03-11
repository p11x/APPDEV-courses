/**
 * Product List Component - Section 7: Angular Setup
 */
import { Component, OnInit } from '@angular/core';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  loading = false;
  error: string | null = null;

  constructor() { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.error = null;
    
    // Sample data - will be replaced with API call
    setTimeout(() => {
      this.products = [
        { id: 1, name: 'Laptop Pro', description: 'High-performance laptop', price: 1299.99, category: 'Electronics', inStock: true },
        { id: 2, name: 'Wireless Mouse', description: 'Ergonomic mouse', price: 49.99, category: 'Accessories', inStock: true },
        { id: 3, name: 'Office Chair', description: 'Comfortable chair', price: 299.99, category: 'Furniture', inStock: false }
      ];
      this.loading = false;
    }, 500);
  }

  deleteProduct(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.products = this.products.filter(p => p.id !== id);
    }
  }
}
