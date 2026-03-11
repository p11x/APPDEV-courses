# File Upload API

## Concept Title and Overview

In this lesson, you'll learn how to handle file uploads in Spring Boot APIs.

## Real-World Importance and Context

Many applications need to handle file uploads—profile pictures, documents, images. Spring Boot makes this straightforward.

## Detailed Step-by-Step Explanation

### File Upload Configuration

```properties
# application.properties
spring.servlet.multipart.enabled=true
spring.servlet.multipart.max-file-size=10MB
spring.servlet.multipart.max-request-size=10MB
```

### Controller for File Upload

```java
@PostMapping("/upload")
public ResponseEntity<FileResponse> uploadFile(@RequestParam("file") MultipartFile file) {
    if (file.isEmpty()) {
        return ResponseEntity.badRequest().build();
    }
    
    String fileName = file.getOriginalFilename();
    String uploadDir = "uploads/";
    
    try {
        Path path = Paths.get(uploadDir + fileName);
        Files.createDirectories(path.getParent());
        Files.write(path, file.getBytes());
        
        return ResponseEntity.ok(new FileResponse(fileName, "File uploaded successfully"));
    } catch (IOException e) {
        return ResponseEntity.status(500).build();
    }
}
```

## Angular File Upload

```typescript
import { HttpClient } from '@angular/common/http';

uploadFile(file: File): Observable<any> {
  const formData = new FormData();
  formData.append('file', file);
  return this.http.post(`${this.apiUrl}/upload`, formData);
}
```

## Student Hands-On Exercises

### Exercise 1: Basic Upload (Easy)
Implement file upload endpoint.

### Exercise 2: Multiple Files (Medium)
Handle multiple file uploads.

### Exercise 3: Angular Component (Hard)
Create Angular file upload component.

---

## Summary

You've learned:
- Multipart file handling
- File storage
- Angular file upload

---

**Next Lesson**: In the next lesson, we'll explore [Logging](21_Logging.md).
