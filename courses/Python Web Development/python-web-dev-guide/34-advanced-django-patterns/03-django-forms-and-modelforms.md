# Django Forms and ModelForms

## What You'll Learn
- Django Form class and field validation
- ModelForm for database-backed forms
- Custom form validation
- Form rendering and widgets
- Handling file uploads with forms

## Prerequisites
- Completed `02-mixins-and-mro.md` — Mixin patterns
- Understanding of Django models
- Basic HTML forms knowledge

## Django Form Basics

A Django Form is a class that defines form fields and handles validation:

```python
# forms.py
from django import forms

class ContactForm(forms.Form):
    """Contact form with validation."""
    
    name = forms.CharField(
        max_length=100,
        widget=forms.TextInput(attrs={
            "class": "form-control",
            "placeholder": "Your name"
        })
    )
    
    email = forms.EmailField(
        widget=forms.EmailInput(attrs={"class": "form-control"})
    )
    
    message = forms.CharField(
        widget=forms.Textarea(attrs={
            "class": "form-control",
            "rows": 5
        })
    )
    
    # Custom validation
    def clean_message(self):
        """Validate message field."""
        message = self.cleaned_data.get("message")
        
        if len(message) < 10:
            raise forms.ValidationError("Message too short!")
        
        # Check for spam (simple example)
        if "buy now" in message.lower():
            raise forms.ValidationError("No spam allowed!")
        
        return message
```

🔍 **Line-by-Line Breakdown:**
1. `forms.Form` — Base class for regular forms
2. `forms.CharField` — Text input field with max length
3. `widget` — Controls HTML rendering
4. `clean_message` — Custom validation method (called after basic validation)

## Using Forms in Views

```python
# views.py
from django.shortcuts import render
from .forms import ContactForm

def contact_view(request):
    """Handle contact form."""
    
    if request.method == "POST":
        form = ContactForm(request.POST)
        
        if form.is_valid():
            # Process form data
            name = form.cleaned_data["name"]
            email = form.cleaned_data["email"]
            message = form.cleaned_data["message"]
            
            # Send email, save to database, etc.
            send_email(name, email, message)
            
            # Redirect after success
            return render(request, "success.html")
    
    else:
        # GET request - show empty form
        form = ContactForm()
    
    return render(request, "contact.html", {"form": form})
```

```html
<!-- template: contact.html -->
<form method="post">
    {% csrf_token %}
    
    {{ form.as_p }}
    
    <button type="submit">Send</button>
</form>

<!-- Or manually render each field -->
<form method="post">
    {% csrf_token %}
    
    <div class="form-group">
        {{ form.name.errors }}
        <label>{{ form.name.label }}</label>
        {{ form.name }}
    </div>
    
    <div class="form-group">
        {{ form.email.errors }}
        <label>{{ form.email.label }}</label>
        {{ form.email }}
    </div>
    
    <div class="form-group">
        {{ form.message.errors }}
        <label>{{ form.message.label }}</label>
        {{ form.message }}
    </div>
    
    <button type="submit">Send</button>
</form>
```

## ModelForm

ModelForm automatically generates a form from a Django model:

```python
# models.py
from django.db import models

class Article(models.Model):
    title = models.CharField(max_length=200)
    slug = models.SlugField(max_length=200, unique=True)
    content = models.TextField()
    author = models.ForeignKey("auth.User", on_delete=models.CASCADE)
    status = models.CharField(max_length=10, choices=[
        ("draft", "Draft"),
        ("published", "Published"),
    ], default="draft")
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)
    
    def __str__(self):
        return self.title
```

```python
# forms.py
from django import forms
from .models import Article

class ArticleForm(forms.ModelForm):
    """Form for creating/editing articles."""
    
    class Meta:
        model = Article
        fields = ["title", "slug", "content", "status"]
        widgets = {
            "title": forms.TextInput(attrs={"class": "form-control"}),
            "slug": forms.TextInput(attrs={"class": "form-control"}),
            "content": forms.Textarea(attrs={"class": "form-control", "rows": 10}),
            "status": forms.Select(attrs={"class": "form-control"}),
        }
```

🔍 **Line-by-Line Breakdown:**
1. `class Meta:` — Configuration for ModelForm
2. `model = Article` — Which model to use
3. `fields = [...]` — Which fields to include (or use `exclude`)
4. `widgets` — Customize HTML rendering for each field

## ModelForm with CreateView

```python
# views.py
from django.views.generic import CreateView
from django.urls import reverse_lazy
from .models import Article
from .forms import ArticleForm

class ArticleCreateView(CreateView):
    """Create new article with form."""
    model = Article
    form_class = ArticleForm
    template_name = "blog/article_form.html"
    success_url = reverse_lazy("article-list")
    
    def form_valid(self, form):
        """Set author to current user before saving."""
        form.instance.author = self.request.user
        return super().form_valid(form)
```

## Custom Field Validation

### Field-Level Validation

```python
class ArticleForm(forms.ModelForm):
    
    class Meta:
        model = Article
        fields = ["title", "slug", "content"]
    
    def clean_slug(self):
        """Validate slug is URL-safe."""
        slug = self.cleaned_data.get("slug")
        
        # Only allow alphanumeric and hyphens
        if not slug.replace("-", "").isalnum():
            raise forms.ValidationError(
                "Slug can only contain letters, numbers, and hyphens"
            )
        
        # Check for duplicates
        if Article.objects.filter(slug=slug).exists():
            raise forms.ValidationError("This slug is already taken")
        
        return slug
```

### Cross-Field Validation

```python
class OrderForm(forms.Form):
    """Form where fields depend on each other."""
    
    order_type = forms.ChoiceField(choices=[("standard", "Standard"), ("express", "Express")])
    shipping_address = forms.CharField(required=False)
    express_shipping = forms.BooleanField(required=False)
    
    def clean(self):
        """Validate relationship between fields."""
        cleaned_data = super().clean()
        order_type = cleaned_data.get("order_type")
        shipping_address = cleaned_data.get("shipping_address")
        
        if order_type == "express" and not shipping_address:
            raise forms.ValidationError(
                "Shipping address required for express delivery"
            )
        
        return cleaned_data
```

## File Uploads with Forms

```python
# forms.py
from django import forms

class DocumentForm(forms.Form):
    """Form for uploading documents."""
    
    title = forms.CharField(max_length=200)
    document = forms.FileField(
        widget=forms.ClearableFileInput(attrs={
            "accept": ".pdf,.doc,.docx"
        })
    )
    
    def clean_document(self):
        """Validate uploaded file."""
        document = self.cleaned_data.get("document")
        
        # Check file size (max 10MB)
        if document.size > 10 * 1024 * 1024:
            raise forms.ValidationError("File too large (max 10MB)")
        
        # Check file extension
        import os
        ext = os.path.splitext(document.name)[1].lower()
        allowed = [".pdf", ".doc", ".docx"]
        
        if ext not in allowed:
            raise forms.ValidationError(f"Invalid file type. Allowed: {allowed}")
        
        return document
```

```python
# views.py
from django.views.generic import FormView

class DocumentUploadView(FormView):
    form_class = DocumentForm
    template_name = "upload.html"
    success_url = "/success/"
    
    def form_valid(self, form):
        # Save file to disk
        document = form.cleaned_data["document"]
        
        # Generate safe filename
        from django.utils.text import slugify
        import time
        filename = f"{int(time.time())}-{slugify(document.name)}"
        
        # Save to MEDIA_ROOT
        save_path = Path(settings.MEDIA_ROOT) / "documents" / filename
        save_path.parent.mkdir(parents=True, exist_ok=True)
        
        with open(save_path, "wb") as f:
            for chunk in document.chunks():
                f.write(chunk)
        
        return super().form_valid(form)
```

## Form Widgets

```python
# Different ways to render fields

# TextInput
forms.CharField(widget=forms.TextInput(attrs={"class": "form-control"}))

# Textarea
forms.CharField(widget=forms.Textarea(attrs={"rows": 4}))

# Select dropdown
forms.ChoiceField(widget=forms.Select(attrs={"class": "form-control"}))

# Checkbox
forms.BooleanField(
    widget=forms.CheckboxInput(attrs={"class": "form-check-input"})
)

# Radio buttons
forms.ChoiceField(
    widget=forms.RadioSelect,
    choices=[("a", "Option A"), ("b", "Option B")]
)

# Multiple checkboxes
forms.MultipleChoiceField(
    widget=forms.CheckboxSelectMultiple,
    choices=[("a", "Option A"), ("b", "Option B")]
)
```

## Production Considerations

- **CSRF**: Always include `{% csrf_token %}` in forms
- **File uploads**: Configure `DATA_UPLOAD_MAX_MEMORY_SIZE` in settings
- **Validation**: Use both client-side (JavaScript) and server-side (Django) validation
- **Accessibility**: Use `<label>` tags with `for` attribute

## Common Mistakes & How to Avoid Them

### ❌ Mistake 1: Not checking is_valid()

**Wrong:**
```python
def contact_view(request):
    form = ContactForm(request.POST)
    # Directly access data without validation!
    name = form["name"]  # WRONG!
```

**Why it fails:** Data isn't cleaned/sanitized, potential security issue.

**Fix:**
```python
def contact_view(request):
    form = ContactForm(request.POST)
    if form.is_valid():  # Always validate!
        name = form.cleaned_data["name"]
```

### ❌ Mistake 2: Using wrong widget for ModelForm

**Wrong:**
```python
class ArticleForm(forms.ModelForm):
    # Forgot widgets - uses default!
    class Meta:
        model = Article
        fields = "__all__"
```

**Why it fails:** Default widgets may not match your design.

**Fix:**
```python
class ArticleForm(forms.ModelForm):
    class Meta:
        model = Article
        fields = "__all__"
        widgets = {
            "title": forms.TextInput(attrs={"class": "form-control"}),
            # Add all fields...
        }
```

### ❌ Mistake 3: Not handling form errors in template

**Wrong:**
```python
<!-- Just rendering form, no error display! -->
{{ form }}
```

**Why it fails:** Users don't see validation errors.

**Fix:**
```python
{{ form.field_name.errors }}
{{ form.field_name }}
```

## Summary

- Django Forms handle validation, cleaning, and rendering
- ModelForm auto-generates forms from Django models
- Use `clean_<fieldname>()` for field-specific validation
- Use `clean()` for cross-field validation
- Always check `form.is_valid()` before accessing cleaned data

## Next Steps

→ Continue to `04-django-orm-queries.md` to learn advanced Django ORM queries.
