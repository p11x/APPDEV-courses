---
title: "Ruby on Rails + Bootstrap"
slug: "ruby-rails-bootstrap"
difficulty: 2
tags: ["bootstrap", "rails", "ruby", "turbo", "hotwire"]
prerequisites:
  - "06_04_03_Angular_Bootstrap"
  - "06_04_04_Svelte_Bootstrap"
related:
  - "06_04_09_Qwik_Bootstrap"
  - "06_03_12_Docker_Bootstrap"
duration: "30 minutes"
---

# Ruby on Rails + Bootstrap

## Overview

Ruby on Rails integrates Bootstrap through the asset pipeline (Sprockets or Propshaft) or import maps, providing server-rendered Bootstrap UI enhanced with Hotwire (Turbo + Stimulus). Turbo Drive accelerates navigation by converting page links into AJAX requests, while Turbo Frames and Streams enable partial page updates without custom JavaScript. Stimulus controllers add interactivity to Bootstrap components like modals, dropdowns, and toasts. This combination delivers fast, server-driven UI with minimal client-side JavaScript.

## Basic Implementation

Set up a Rails application with Bootstrap via import maps or the asset pipeline.

```bash
rails new my_bootstrap_app --css=bootstrap --skip-javascript
cd my_bootstrap_app
bin/rails importmap:pin bootstrap
```

```ruby
# Gemfile
gem "importmap-rails"
gem "stimulus-rails"
gem "turbo-rails"
gem "bootstrap", "~> 5.3"
gem "sassc-rails"
```

```scss
// app/assets/stylesheets/application.bootstrap.scss
@import "bootstrap/scss/bootstrap";
@import "bootstrap-icons/font/bootstrap-icons.css";

$primary: #dc3545;
$enable-rounded: true;
```

```erb
<!-- app/views/layouts/application.html.erb -->
<!DOCTYPE html>
<html>
  <head>
    <title>My Bootstrap App</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <%= csrf_meta_tags %>
    <%= csp_meta_tag %>
    <%= stylesheet_link_tag "application", "data-turbo-track": "reload" %>
    <%= javascript_importmap_tags %>
  </head>
  <body>
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
      <div class="container">
        <%= link_to "Bootstrap App", root_path, class: "navbar-brand" %>
        <button class="navbar-toggler" data-bs-toggle="collapse" data-bs-target="#nav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="nav">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item"><%= link_to "Home", root_path, class: "nav-link" %></li>
            <li class="nav-item"><%= link_to "Posts", posts_path, class: "nav-link" %></li>
          </ul>
        </div>
      </div>
    </nav>
    <main class="container mt-4">
      <% if notice %>
        <div class="alert alert-success alert-dismissible fade show" role="alert">
          <%= notice %>
          <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
      <% end %>
      <%= yield %>
    </main>
  </body>
</html>
```

## Advanced Variations

### Turbo Frames with Bootstrap Cards

Partial page updates using Turbo Frames wrapping Bootstrap components.

```erb
<!-- app/views/posts/index.html.erb -->
<div class="row">
  <div class="col-md-8">
    <h2>Posts</h2>
    <turbo-frame id="posts_list">
      <%= render @posts %>
    </turbo-frame>
  </div>
  <div class="col-md-4">
    <turbo-frame id="post_form">
      <div class="card">
        <div class="card-header"><h5 class="mb-0">New Post</h5></div>
        <div class="card-body">
          <%= render "form", post: @post %>
        </div>
      </div>
    </turbo-frame>
  </div>
</div>
```

```erb
<!-- app/views/posts/_post.html.erb -->
<div class="card mb-3" id="<%= dom_id(post) %>">
  <div class="card-body">
    <h5 class="card-title"><%= post.title %></h5>
    <p class="card-text"><%= truncate(post.body, length: 200) %></p>
    <div class="d-flex gap-2">
      <%= link_to "Read More", post, class: "btn btn-primary btn-sm",
          data: { turbo_frame: "_top" } %>
      <%= link_to "Edit", edit_post_path(post), class: "btn btn-outline-secondary btn-sm" %>
    </div>
  </div>
  <div class="card-footer text-muted">
    <small><i class="bi bi-clock"></i> <%= time_ago_in_words(post.created_at) %> ago</small>
  </div>
</div>
```

### Turbo Streams for Real-Time Updates

Update Bootstrap UI in real-time when data changes.

```ruby
# app/controllers/posts_controller.rb
class PostsController < ApplicationController
  def create
    @post = Post.new(post_params)

    if @post.save
      respond_to do |format|
        format.turbo_stream {
          render turbo_stream: [
            turbo_stream.prepend("posts_list", partial: "post", locals: { post: @post }),
            turbo_stream.replace("post_form", partial: "form", locals: { post: Post.new }),
            turbo_stream.replace("flash", partial: "flash", locals: { notice: "Post created!" })
          ]
        }
        format.html { redirect_to posts_path, notice: "Post created!" }
      end
    end
  end

  def destroy
    @post = Post.find(params[:id])
    @post.destroy

    respond_to do |format|
      format.turbo_stream {
        render turbo_stream: turbo_stream.remove(@post)
      }
      format.html { redirect_to posts_path }
    end
  end
end
```

```erb
<!-- app/views/posts/create.turbo_stream.erb -->
<%= turbo_stream.prepend "posts_list" do %>
  <%= render partial: "post", locals: { post: @post } %>
<% end %>

<%= turbo_stream.replace "post_form" do %>
  <turbo-frame id="post_form">
    <div class="card">
      <div class="card-header"><h5 class="mb-0">New Post</h5></div>
      <div class="card-body">
        <%= render "form", post: Post.new %>
      </div>
    </div>
  </turbo-frame>
<% end %>
```

### Stimulus Controllers for Bootstrap Components

Use Stimulus to manage Bootstrap JavaScript components.

```javascript
// app/javascript/controllers/modal_controller.js
import { Controller } from "@hotwired/stimulus"
import { Modal } from "bootstrap"

export default class extends Controller {
  static targets = ["container"]

  connect() {
    this.modal = new Modal(this.containerTarget)
  }

  open() {
    this.modal.show()
  }

  close() {
    this.modal.hide()
  }

  disconnect() {
    this.modal.dispose()
  }
}
```

```erb
<!-- Usage -->
<div data-controller="modal">
  <button class="btn btn-primary" data-action="click->modal#open">
    <i class="bi bi-plus-lg"></i> New Item
  </button>

  <div class="modal fade" data-modal-target="container" tabindex="-1">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Create Item</h5>
          <button type="button" class="btn-close" data-action="click->modal#close"></button>
        </div>
        <div class="modal-body">
          <%= render "items/form" %>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Toast Notifications with Stimulus

```javascript
// app/javascript/controllers/toast_controller.js
import { Controller } from "@hotwired/stimulus"
import { Toast } from "bootstrap"

export default class extends Controller {
  static values = { delay: { type: Number, default: 5000 } }

  connect() {
    this.toast = new Toast(this.element, { delay: this.delayValue })
    this.toast.show()
  }

  disconnect() {
    this.toast.dispose()
  }

  hide() {
    this.toast.hide()
  }
}
```

```erb
<!-- app/views/shared/_flash.html.erb -->
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <% flash.each do |type, message| %>
    <div class="toast" role="alert" data-controller="toast">
      <div class="toast-header">
        <i class="bi bi-<%= type == 'notice' ? 'check-circle text-success' : 'exclamation-triangle text-danger' %> me-2"></i>
        <strong class="me-auto"><%= type.capitalize %></strong>
        <button type="button" class="btn-close" data-action="click->toast#hide"></button>
      </div>
      <div class="toast-body"><%= message %></div>
    </div>
  <% end %>
</div>
```

## Best Practices

1. Use import maps for Bootstrap JS in Rails 7+ instead of Webpack/esbuild
2. Pin only the Bootstrap components you need with import maps
3. Use Turbo Frames to scope Bootstrap component updates to specific page sections
4. Wrap Bootstrap cards, tables, and forms in `<turbo-frame>` for partial updates
5. Use Stimulus controllers to manage Bootstrap JavaScript component lifecycle
6. Dispose Bootstrap component instances in Stimulus `disconnect()` callbacks
7. Use `dom_id` helper for consistent element IDs across Turbo Stream updates
8. Configure Turbo Drive for seamless navigation with Bootstrap layout preservation
9. Use `respond_to` blocks to handle both Turbo Stream and HTML formats
10. Keep Bootstrap SCSS in `app/assets/stylesheets` for Propshaft compilation
11. Use `data-bs-dismiss` attributes alongside Stimulus actions for Bootstrap modals
12. Test Turbo Stream broadcasts with Action Cable for real-time Bootstrap updates
13. Use `bootstrap` gem for version management alongside import maps
14. Precompile Bootstrap assets in production with `config.assets.precompile`

## Common Pitfalls

1. **Double CSS loading**: Including Bootstrap via both gem asset pipeline and import maps
2. **Turbo caching**: Bootstrap state persisting incorrectly due to Turbo's page cache
3. **Missing Stimulus disconnect**: Bootstrap components not disposed, causing memory leaks
4. **Form resubmission**: Turbo Drive resubmitting forms that create Bootstrap modals
5. **Missing CSRF tokens**: Turbo requests failing without proper CSRF meta tags
6. **Asset pipeline conflicts**: Sprockets and Propshaft fighting over Bootstrap asset compilation
7. **Import map pinning**: Forgetting to pin Bootstrap submodules like `bootstrap/js/dist/modal`

## Accessibility Considerations

Rails view helpers generate semantic HTML that pairs well with Bootstrap's accessibility features. Use `aria-*` attributes in ERB templates for screen reader support. Ensure Turbo Stream updates maintain ARIA live region announcements. Use `role="alert"` on flash messages rendered via Turbo Streams. Test that Stimulus-managed Bootstrap components preserve keyboard navigation. Use Rails `content_tag` helpers to generate accessible Bootstrap markup. Verify that Turbo Drive navigation announces page changes to screen readers.

```erb
<div role="status" aria-live="polite" class="visually-hidden" id="turbo-announce">
  Page updated
</div>
```

## Responsive Behavior

Bootstrap's responsive grid works identically in Rails ERB templates. Use standard Bootstrap responsive classes (`col-md-6`, `d-lg-none`) in view files. Turbo Frames maintain responsive layout during partial updates. Test responsive behavior with Rails system tests using Capybara at different viewport sizes. Ensure Bootstrap responsive tables use `table-responsive` wrapper in ERB partials. Use Rails asset pipeline to compile responsive SCSS with all Bootstrap breakpoints included.
