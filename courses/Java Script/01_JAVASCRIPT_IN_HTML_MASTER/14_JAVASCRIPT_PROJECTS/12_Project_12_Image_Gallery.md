# 🖼️ Project 12: Image Gallery

## 📋 Project Overview

Build an interactive image gallery with lightbox view, filters, lazy loading, and masonry layout. This project demonstrates:
- Dynamic image rendering
- Lightbox modal functionality
- Image filtering
- Lazy loading optimization

---

## 🎯 Core Features

### Gallery Manager

```javascript
class GalleryManager {
    constructor(container) {
        this.container = container;
        this.images = [];
        this.filter = 'all';
        this.loadImages();
    }
    
    loadImages() {
        // Sample images - in production, load from API or folder
        this.images = [
            { id: 1, src: 'img1.jpg', category: 'nature', alt: 'Mountain' },
            { id: 2, src: 'img2.jpg', category: 'city', alt: 'Skyline' },
            { id: 3, src: 'img3.jpg', category: 'nature', alt: 'Beach' },
            { id: 4, src: 'img4.jpg', category: 'people', alt: 'Portrait' },
            { id: 5, src: 'img5.jpg', category: 'city', alt: 'Street' },
            { id: 6, src: 'img6.jpg', category: 'nature', alt: 'Forest' },
        ];
    }
    
    filterImages(category) {
        this.filter = category;
        this.render();
    }
    
    getFilteredImages() {
        if (this.filter === 'all') return this.images;
        return this.images.filter(img => img.category === this.filter);
    }
    
    render() {
        const filtered = this.getFilteredImages();
        this.container.innerHTML = filtered.map(img => `
            <div class="gallery-item" data-category="${img.category}">
                <img src="${img.src}" alt="${img.alt}" loading="lazy">
                <div class="overlay">
                    <span>${img.alt}</span>
                </div>
            </div>
        `).join('');
        
        // Add click handlers for lightbox
        this.container.querySelectorAll('.gallery-item').forEach(item => {
            item.addEventListener('click', () => this.openLightbox(item));
        });
    }
    
    openLightbox(item) {
        const img = item.querySelector('img');
        const lightbox = document.getElementById('lightbox');
        const lightboxImg = lightbox.querySelector('img');
        lightboxImg.src = img.src;
        lightbox.classList.add('active');
    }
}
```

---

## 🔗 Related Topics

- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)
- [05_Element_Creation_and_Manipulation.md](../09_DOM_MANIPULATION/05_Element_Creation_and_Manipulation.md)

---

**Projects Module: 12/32 Complete** 🎉

Continue building more projects or move to testing module?