/**
 * Product Entity - Section 2 Code Example
 * This is the Java/Spring Boot equivalent of our TypeScript Product model
 * 
 * In a full-stack app, this represents the data structure that:
 * - Maps to the SQL Server database table
 * - Gets converted to/from JSON for API communication
 * - Used by Angular frontend
 */

package com.example.product.model;

import javax.persistence.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Entity class that maps to the "products" table in SQL Server
 * 
 * @Entity tells Spring this is a database entity
 * @Table specifies the database table name
 */
@Entity
@Table(name = "products")
public class Product {
    
    // Primary key - auto-generated
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    // Product name - required field
    @Column(nullable = false, length = 100)
    private String name;
    
    // Product description
    @Column(length = 500)
    private String description;
    
    // Price - using BigDecimal for accurate currency
    @Column(nullable = false, precision = 10, scale = 2)
    private BigDecimal price;
    
    // Category
    @Column(length = 50)
    private String category;
    
    // Stock status
    @Column(nullable = false)
    private Boolean inStock = true;
    
    // Image URL
    @Column(length = 255)
    private String imageUrl;
    
    // Timestamps
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @Column(name = "updated_at")
    private LocalDateTime updatedAt;
    
    // Default constructor (required by JPA)
    public Product() {
    }
    
    // Parameterized constructor
    public Product(String name, String description, BigDecimal price, 
                   String category, Boolean inStock) {
        this.name = name;
        this.description = description;
        this.price = price;
        this.category = category;
        this.inStock = inStock;
    }
    
    // Getters and Setters
    
    public Long getId() {
        return id;
    }
    
    public void setId(Long id) {
        this.id = id;
    }
    
    public String getName() {
        return name;
    }
    
    public void setName(String name) {
        this.name = name;
    }
    
    public String getDescription() {
        return description;
    }
    
    public void setDescription(String description) {
        this.description = description;
    }
    
    public BigDecimal getPrice() {
        return price;
    }
    
    public void setPrice(BigDecimal price) {
        this.price = price;
    }
    
    public String getCategory() {
        return category;
    }
    
    public void setCategory(String category) {
        this.category = category;
    }
    
    public Boolean getInStock() {
        return inStock;
    }
    
    public void setInStock(Boolean inStock) {
        this.inStock = inStock;
    }
    
    public String getImageUrl() {
        return imageUrl;
    }
    
    public void setImageUrl(String imageUrl) {
        this.imageUrl = imageUrl;
    }
    
    public LocalDateTime getCreatedAt() {
        return createdAt;
    }
    
    public void setCreatedAt(LocalDateTime createdAt) {
        this.createdAt = createdAt;
    }
    
    public LocalDateTime getUpdatedAt() {
        return updatedAt;
    }
    
    public void setUpdatedAt(LocalDateTime updatedAt) {
        this.updatedAt = updatedAt;
    }
    
    // Lifecycle callbacks - automatically set timestamps
    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
        updatedAt = LocalDateTime.now();
    }
    
    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
    
    // toString method for debugging
    @Override
    public String toString() {
        return "Product{" +
                "id=" + id +
                ", name='" + name + '\'' +
                ", price=" + price +
                ", category='" + category + '\'' +
                ", inStock=" + inStock +
                '}';
    }
}
