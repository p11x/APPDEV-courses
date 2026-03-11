# Section 14: Spring Boot Data Access with JPA

## JPA Repository

### ProductRepository.java
```java
package com.example.product.repository;

import com.example.product.model.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ProductRepository extends JpaRepository<Product, Long> {
    
    // Custom query methods
    List<Product> findByCategory(String category);
    
    List<Product> findByNameContaining(String name);
    
    List<Product> findByPriceBetween(Double minPrice, Double maxPrice);
    
    List<Product> findByInStockTrue();
}
```

---

## Service Layer

### ProductService.java
```java
package com.example.product.service;

import com.example.product.model.Product;
import com.example.product.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@Service
@Transactional
public class ProductService {

    @Autowired
    private ProductRepository productRepository;

    public List<Product> getAllProducts() {
        return productRepository.findAll();
    }

    public Optional<Product> getProductById(Long id) {
        return productRepository.findById(id);
    }

    public Product saveProduct(Product product) {
        return productRepository.save(product);
    }

    public void deleteProduct(Long id) {
        productRepository.deleteById(id);
    }

    public List<Product> getProductsByCategory(String category) {
        return productRepository.findByCategory(category);
    }

    public List<Product> searchProducts(String searchTerm) {
        return productRepository.findByNameContaining(searchTerm);
    }
}
```

---

## Summary

JpaRepository + Entity + Service = Data Access Layer
