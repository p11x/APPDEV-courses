# Bootstrap Functionalities

## 1. Grid System Usage

```html
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6 col-lg-4">
      Content here
    </div>
  </div>
</div>
```

## 2. Responsive Navbar

```html
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
  <a class="navbar-brand" href="#">Brand</a>
  <button class="navbar-toggler" data-toggle="collapse" data-target="#menu">
    <span class="navbar-toggler-icon"></span>
  </button>
  <div class="collapse navbar-collapse" id="menu">
    <ul class="navbar-nav ml-auto">
      <li class="nav-item"><a class="nav-link" href="#">Home</a></li>
      <li class="nav-item"><a class="nav-link" href="#">About</a></li>
    </ul>
  </div>
</nav>
```

## 3. Card Components

```html
<div class="card" style="width: 18rem;">
  <img class="card-img-top" src="image.jpg" alt="Card image">
  <div class="card-body">
    <h5 class="card-title">Card Title</h5>
    <p class="card-text">Card content</p>
    <a href="#" class="btn btn-primary">Action</a>
  </div>
</div>
```

## 4. Form Styling

```html
<form>
  <div class="form-group">
    <label>Email</label>
    <input type="email" class="form-control" placeholder="Enter email">
  </div>
  <div class="form-group">
    <label>Password</label>
    <input type="password" class="form-control" placeholder="Password">
  </div>
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

## 5. Modal Dialog

```html
<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">
  Open Modal
</button>

<div class="modal" id="myModal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h4 class="modal-title">Modal Title</h4>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>
      <div class="modal-body">Content here</div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
```

## 6. Buttons and Utilities

| Class | Purpose |
|-------|---------|
| `btn btn-primary` | Primary button |
| `btn btn-secondary` | Secondary button |
| `btn btn-success` | Success action |
| `btn btn-danger` | Danger action |
| `btn btn-outline-primary` | Outlined button |
| `btn btn-lg` | Large button |
| `btn btn-sm` | Small button |

---

*Back to [Web Development README](../README.md)*
*Back to [Main README](../../README.md)*