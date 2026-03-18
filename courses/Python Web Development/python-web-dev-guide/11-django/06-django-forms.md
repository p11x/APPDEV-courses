# Django Forms

## What You'll Learn
- Creating Django forms
- Form validation
- Model forms
- Handling form submissions

## Prerequisites
- Completed `05-django-templates.md`

## Django Forms Overview

Django forms handle the entire process of collecting and processing user input. They provide built-in validation, CSRF protection, and rendering.

## Creating a Form

```python
# blog/forms.py
from django import forms
from .models import Post

class PostForm(forms.Form):
    title = forms.CharField(max_length=200)
    content = forms.CharField(widget=forms.Textarea)
    email = forms.EmailField()
    
    def clean_title(self):
        title = self.cleaned_data['title']
        if len(title) < 10:
            raise forms.ValidationError("Title must be at least 10 characters")
        return title

class PostModelForm(forms.ModelForm):
    class Meta:
        model = Post
        fields = ['title', 'content', 'published']
        widgets = {
            'content': forms.Textarea(attrs={'rows': 10}),
        }
```

## Using Forms in Views

```python
# blog/views.py
from django.shortcuts import render, redirect
from .forms import PostForm, PostModelForm

def create_post(request):
    if request.method == 'POST':
        form = PostModelForm(request.POST)
        if form.is_valid():
            post = form.save(commit=False)
            post.author = request.user
            post.save()
            return redirect('post_detail', slug=post.slug)
    else:
        form = PostModelForm()
    return render(request, 'blog/create_post.html', {'form': form})
```

## Form Templates

```html
<form method="post">
    {% csrf_token %}
    {{ form.as_p }}
    <button type="submit">Submit</button>
</form>
```

## Summary
- Django forms handle validation automatically
- ModelForms create forms from models
- Use `is_valid()` to check validity
- Access cleaned data via `cleaned_data`
