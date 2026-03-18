# Class-Based Views in Django

## What You'll Learn
- Understanding CBVs
- Mixins
- Building complex views

## Prerequisites
- Completed `08-django-rest-framework.md`

## CBV Overview

Class-based views provide an alternative to function-based views. They organize related logic into classes with methods for each HTTP method.

## Basic CBVs

```python
from django.views.generic import ListView, DetailView, CreateView, UpdateView, DeleteView
from .models import Post
from .forms import PostForm

class PostListView(ListView):
    model = Post
    template_name = 'blog/post_list.html'
    context_object_name = 'posts'
    paginate_by = 10

class PostDetailView(DetailView):
    model = Post
    template_name = 'blog/post_detail.html'

class PostCreateView(CreateView):
    model = Post
    form_class = PostForm
    template_name = 'blog/post_form.html'
    success_url = '/blog/'

class PostUpdateView(UpdateView):
    model = Post
    form_class = PostForm
    template_name = 'blog/post_form.html'

class PostDeleteView(DeleteView):
    model = Post
    template_name = 'blog/post_confirm_delete.html'
    success_url = '/blog/'
```

## Mixins

Mixins add functionality:

```python
from django.contrib.auth.mixins import LoginRequiredMixin, UserPassesTestMixin

class PostCreateView(LoginRequiredMixin, CreateView):
    model = Post
    form_class = PostForm
    
    def form_valid(self, form):
        form.instance.author = self.request.user
        return super().form_valid(form)

class PostUpdateView(LoginRequiredMixin, UserPassesTestMixin, UpdateView):
    model = Post
    form_class = PostForm
    
    def test_func(self):
        return self.get_object().author == self.request.user
```

## Summary
- CBVs organize code into classes
- Use mixins for reusability
- CBVs provide built-in handling for common patterns
