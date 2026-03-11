# Project 2: Product Landing Page

## Project Overview

### Project Title
Product Landing Page

### Project Description
Create a compelling product landing page that showcases a product with navigation, hero section, product details, features, and call-to-action. This project demonstrates intermediate HTML skills including semantic elements, forms, and responsive structure.

### Learning Objectives

- Create multi-section landing page
- Use semantic HTML5 elements
- Build navigation with anchor links
- Add call-to-action sections
- Include product showcase elements

### Estimated Duration
3-4 hours

---

## Project Requirements

### Required Sections

1. **Navigation Bar**
   - Logo/Brand name
   - Navigation links (anchor links to sections)
   - Sticky or fixed position

2. **Hero Section**
   - Large headline
   - Subheadline
   - Primary CTA button
   - Hero image

3. **Product Features**
   - Feature list with icons (using emojis or images)
   - Benefits of the product

4. **Product Details**
   - Product image
   - Specifications/Technical details
   - Pricing

5. **Testimonials**
   - Customer reviews
   - Star ratings

6. **Call-to-Action (CTA)**
   - Final push to purchase
   - CTA button

7. **Footer**
   - Contact information
   - Social links
   - Copyright

---

## Code Template

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Product Name - Landing Page</title>
    <meta name="description" content="Description of your product">
</head>
<body>
    <!-- Navigation -->
    <nav>
        <div class="logo">Brand</div>
        <ul>
            <li><a href="#features">Features</a></li>
            <li><a href="#details">Details</a></li>
            <li><a href="#testimonials">Reviews</a></li>
            <li><a href="#cta" class="btn">Buy Now</a></li>
        </ul>
    </nav>
    
    <!-- Hero Section -->
    <header id="hero">
        <h1>Product Headline</h1>
        <p>Compelling subheadline</p>
        <a href="#cta" class="btn-primary">Get Started</a>
        <img src="hero-product.jpg" alt="Product Image">
    </header>
    
    <!-- Features -->
    <section id="features">
        <h2>Why Choose Our Product</h2>
        <ul>
            <li>Feature 1</li>
            <li>Feature 2</li>
            <li>Feature 3</li>
        </ul>
    </section>
    
    <!-- Product Details -->
    <section id="details">
        <h2>Product Details</h2>
        <img src="product.jpg" alt="Product Name">
        <div class="specs">
            <h3>Specifications</h3>
            <dl>
                <dt>Specification 1</dt>
                <dd>Value</dd>
            </dl>
        </div>
        <div class="pricing">
            <p class="price">$99.99</p>
            <a href="#cta" class="btn-primary">Order Now</a>
        </div>
    </section>
    
    <!-- Testimonials -->
    <section id="testimonials">
        <h2>What Our Customers Say</h2>
        <blockquote>
            <p>Customer review text</p>
            <cite>Customer Name</cite>
        </blockquote>
    </section>
    
    <!-- CTA -->
    <section id="cta">
        <h2>Ready to Get Started?</h2>
        <p>Limited time offer!</p>
        <a href="#" class="btn-primary">Buy Now</a>
    </section>
    
    <!-- Footer -->
    <footer>
        <p>Contact: email@example.com</p>
        <nav>
            <a href="#">Privacy</a>
            <a href="#">Terms</a>
        </nav>
        <p>&copy; 2024 Company Name</p>
    </footer>
</body>
</html>
```

---

## Complete Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>SmartWatch Pro - The Future on Your Wrist</title>
    <meta name="description" content="Discover SmartWatch Pro - the most advanced smartwatch with health tracking, GPS, and 7-day battery life.">
</head>
<body>
    <!-- Navigation -->
    <nav style="display: flex; justify-content: space-between; align-items: center; padding: 20px 40px; background: white; position: sticky; top: 0; z-index: 100;">
        <div style="font-size: 1.5em; font-weight: bold; color: #333;">
            SmartWatch Pro
        </div>
        <ul style="display: flex; list-style: none; gap: 30px; margin: 0; padding: 0;">
            <li><a href="#features" style="text-decoration: none; color: #555;">Features</a></li>
            <li><a href="#specs" style="text-decoration: none; color: #555;">Specifications</a></li>
            <li><a href="#reviews" style="text-decoration: none; color: #555;">Reviews</a></li>
            <li><a href="#order" style="background: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;">Order Now</a></li>
        </ul>
    </nav>
    
    <!-- Hero Section -->
    <header style="text-align: center; padding: 80px 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white;">
        <h1 style="font-size: 3em; margin-bottom: 20px;">The Future on Your Wrist</h1>
        <p style="font-size: 1.5em; margin-bottom: 40px; opacity: 0.9;">
            Track your health, stay connected, and live smarter with SmartWatch Pro
        </p>
        <a href="#order" style="background: white; color: #667eea; padding: 15px 40px; text-decoration: none; border-radius: 30px; font-weight: bold; font-size: 1.2em;">
            Pre-Order Now
        </a>
        <div style="margin-top: 60px;">
            <img src="smartwatch-hero.png" alt="SmartWatch Pro" style="max-width: 300px; width: 100%;">
        </div>
    </header>
    
    <!-- Features -->
    <section id="features" style="padding: 80px 20px; max-width: 1200px; margin: 0 auto;">
        <h2 style="text-align: center; font-size: 2.5em; margin-bottom: 60px;">Why Choose SmartWatch Pro</h2>
        
        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 40px;">
            <div style="text-align: center; padding: 30px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                <div style="font-size: 3em; margin-bottom: 20px;">❤️</div>
                <h3>Advanced Health Tracking</h3>
                <p>Monitor heart rate, blood oxygen, sleep patterns, and stress levels 24/7</p>
            </div>
            
            <div style="text-align: center; padding: 30px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                <div style="font-size: 3em; margin-bottom: 20px;">🔋</div>
                <h3>7-Day Battery Life</h3>
                <p>Go a full week on a single charge. Quick charge when you need it</p>
            </div>
            
            <div style="text-align: center; padding: 30px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                <div style="font-size: 3em; margin-bottom: 20px;">📍</div>
                <h3>Built-in GPS</h3>
                <p>Track your runs, rides, and hikes without needing your phone</p>
            </div>
            
            <div style="text-align: center; padding: 30px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                <div style="font-size: 3em; margin-bottom: 20px;">💧</div>
                <h3>Water Resistant</h3>
                <p>Swim-proof design rated for 50 meters depth</p>
            </div>
            
            <div style="text-align: center; padding: 30px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                <div style="font-size: 3em; margin-bottom: 20px;">📱</div>
                <h3>Smart Notifications</h3>
                <p>Get calls, texts, and app alerts right on your wrist</p>
            </div>
            
            <div style="text-align: center; padding: 30px; border-radius: 10px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                <div style="font-size: 3em; margin-bottom: 20px;">🎨</div>
                <h3>Customizable Watch Faces</h3>
                <p>Choose from thousands of watch faces or create your own</p>
            </div>
        </div>
    </section>
    
    <!-- Specifications -->
    <section id="specs" style="padding: 80px 20px; background: #f9f9f9;">
        <h2 style="text-align: center; font-size: 2.5em; margin-bottom: 40px;">Technical Specifications</h2>
        
        <div style="max-width: 800px; margin: 0 auto; display: grid; grid-template-columns: 1fr 1fr; gap: 40px;">
            <div>
                <img src="smartwatch-detail.png" alt="SmartWatch Pro Detail" style="width: 100%; border-radius: 10px;">
            </div>
            <div>
                <table style="width: 100%; border-collapse: collapse;">
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">Display</td>
                        <td style="padding: 15px;">1.4" AMOLED, 454x454 pixels</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">Battery</td>
                        <td style="padding: 15px;">7 days (typical use)</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">Water Resistance</td>
                        <td style="padding: 15px;">5ATM (50 meters)</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">GPS</td>
                        <td style="padding: 15px;">Built-in, GLONASS, Galileo</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">Sensors</td>
                        <td style="padding: 15px;">Heart rate, SpO2, accelerometer</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">Connectivity</td>
                        <td style="padding: 15px;">Bluetooth 5.0, Wi-Fi, NFC</td>
                    </tr>
                    <tr style="border-bottom: 1px solid #ddd;">
                        <td style="padding: 15px; font-weight: bold;">Weight</td>
                        <td style="padding: 15px;">32g (without strap)</td>
                    </tr>
                    <tr>
                        <td style="padding: 15px; font-weight: bold;">Price</td>
                        <td style="padding: 15px; font-size: 1.5em; color: #007bff; font-weight: bold;">$299.99</td>
                    </tr>
                </table>
            </div>
        </div>
    </section>
    
    <!-- Reviews -->
    <section id="reviews" style="padding: 80px 20px; max-width: 1000px; margin: 0 auto;">
        <h2 style="text-align: center; font-size: 2.5em; margin-bottom: 60px;">What Customers Say</h2>
        
        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 30px;">
            <blockquote style="padding: 30px; background: #f9f9f9; border-left: 4px solid #007bff; margin: 0;">
                <p style="font-size: 1.1em; margin-bottom: 20px;">"Best smartwatch I've ever owned! The battery life is incredible and health tracking is accurate."</p>
                <cite style="font-weight: bold;">- Sarah M.</cite>
                <div style="color: #ffd700;">★★★★★</div>
            </blockquote>
            
            <blockquote style="padding: 30px; background: #f9f9f9; border-left: 4px solid #007bff; margin: 0;">
                <p style="font-size: 1.1em; margin-bottom: 20px;">"Perfect for my morning runs. GPS works great and I love getting notifications without my phone."</p>
                <cite style="font-weight: bold;">- John D.</cite>
                <div style="color: #ffd700;">★★★★★</div>
            </blockquote>
            
            <blockquote style="padding: 30px; background: #f9f9f9; border-left: 4px solid #007bff; margin: 0;">
                <p style="font-size: 1.1em; margin-bottom: 20px;">"The sleep tracking has helped me improve my rest habits. Highly recommend!"</p>
                <cite style="font-weight: bold;">- Emily R.</cite>
                <div style="color: #ffd700;">★★★★☆</div>
            </blockquote>
        </div>
    </section>
    
    <!-- CTA -->
    <section id="order" style="padding: 100px 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); text-align: center; color: white;">
        <h2 style="font-size: 3em; margin-bottom: 20px;">Ready to Upgrade?</h2>
        <p style="font-size: 1.5em; margin-bottom: 40px; opacity: 0.9;">
            Free shipping on all orders. 30-day money-back guarantee.
        </p>
        <a href="#" style="background: white; color: #667eea; padding: 20px 60px; text-decoration: none; border-radius: 30px; font-weight: bold; font-size: 1.3em; display: inline-block;">
            Order Now - $299.99
        </a>
    </section>
    
    <!-- Footer -->
    <footer style="padding: 40px 20px; background: #333; color: white; text-align: center;">
        <div style="max-width: 600px; margin: 0 auto;">
            <h3>SmartWatch Pro</h3>
            <p>Contact: support@smartwatchpro.com | 1-800-SMARTWATCH</p>
            <nav style="margin: 20px 0;">
                <a href="#" style="color: white; margin: 0 15px;">Privacy Policy</a>
                <a href="#" style="color: white; margin: 0 15px;">Terms of Service</a>
                <a href="#" style="color: white; margin: 0 15px;">Support</a>
            </nav>
            <p>&copy; 2024 SmartWatch Pro. All rights reserved.</p>
        </div>
    </footer>
</body>
</html>
```

---

## Project Checklist

### Structure (15 points)
- [ ] Proper HTML5 document structure
- [ ] Semantic nav, header, section, footer
- [ ] Smooth scroll anchor navigation

### Hero Section (10 points)
- [ ] Compelling headline
- [ ] Subheadline
- [ ] CTA button
- [ ] Hero image

### Features Section (10 points)
- [ ] Feature grid
- [ ] Clear benefit statements
- [ ] Visual icons

### Product Details (10 points)
- [ ] Product image
- [ ] Specification table
- [ ] Clear pricing

### Testimonials (5 points)
- [ ] Customer quotes
- [ ] Star ratings

### Call-to-Action (5 points)
- [ ] Clear CTA message
- [ ] Prominent button

### Footer (5 points)
- [ ] Contact information
- [ ] Navigation links
- [ ] Copyright

---

## Grading Rubric

| Criteria | Excellent (A) | Good (B) | Needs Work (C) |
|----------|---------------|----------|----------------|
| Structure | Perfect semantic HTML | Good structure | Incomplete |
| Content | Compelling, complete | Adequate | Missing sections |
| Design | Professional look | Acceptable | Needs work |
| Code Quality | Clean, well-organized | Readable | Messy |
