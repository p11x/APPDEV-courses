<!-- FILE: 15_flask_admin_panel/05_file_management/02_image_upload_in_admin.md -->

## Overview

Handle image uploads in Flask-Admin.

## Code Walkthrough

```python
# image_upload.py
from flask_admin.form import ImageUploadField

class ProductAdmin(ModelView):
    form_extra_fields = {
        "image": ImageUploadField("Image", base_path="static/uploads")
    }
```
