/**
 * Product Service - Section 9: Angular Services & DI
 */
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { Product, ProductFilter } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:8080/api/products';

  constructor(private http: HttpClient) { }

  // Get all products
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  // Get product by ID
  getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Create new product
  createProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Update product
  updateProduct(id: number, product: Product): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/${id}`, product)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Delete product
  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Search products
  searchProducts(filter: ProductFilter): Observable<Product[]> {
    let params = '';
    if (filter.searchTerm) params += `search=${filter.searchTerm}&`;
    if (filter.category) params += `category=${filter.category}&`;
    if (filter.minPrice) params += `minPrice=${filter.minPrice}&`;
    if (filter.maxPrice) params += `maxPrice=${filter.maxPrice}&`;
    
    return this.http.get<Product[]>(`${this.apiUrl}/search?${params}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  // Error handling
  private handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
