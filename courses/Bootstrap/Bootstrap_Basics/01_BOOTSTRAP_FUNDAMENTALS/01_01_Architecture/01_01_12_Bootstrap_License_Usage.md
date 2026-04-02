---
title: "Bootstrap License & Usage"
lesson: "01_01_12"
difficulty: "1"
topics: ["mit-license", "attribution", "commercial-usage", "trademark"]
estimated_time: "15 minutes"
---

# Bootstrap License & Usage

## Overview

Bootstrap is released under the MIT License, one of the most permissive open-source licenses available. This means you can freely use, modify, distribute, and sublicense Bootstrap in both personal and commercial projects without paying fees or releasing your source code. The only requirement is including the original copyright notice and license text. The Bootstrap name and logo, however, are trademarks with separate usage guidelines.

Understanding the licensing terms ensures legal compliance in commercial deployments and clarifies the boundaries around branding and attribution.

## Basic Implementation

### MIT License Text

```
The MIT License (MIT)

Copyright (c) 2011-2024 The Bootstrap Authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
```

### Including Attribution

```html
<!-- In your project's source comments or LICENSE file -->
<!--
  This project uses Bootstrap (https://getbootstrap.com/).
  Licensed under the MIT License.
  Copyright (c) 2011-2024 The Bootstrap Authors
-->
```

### LICENSE File in Your Project

```
THIRD-PARTY LICENSES

Bootstrap v5.3.3
License: MIT
Copyright: The Bootstrap Authors
URL: https://getbootstrap.com/
```

## Advanced Variations

### Commercial SaaS Application

```text
You CAN:
- Use Bootstrap in a paid SaaS product
- Modify Bootstrap's source code for your product
- Charge customers for applications built with Bootstrap
- Keep your application source code proprietary

You MUST:
- Include Bootstrap's MIT license notice in your distribution
- Not use the Bootstrap name/logo to imply endorsement
```

### Distribution with Attribution

```html
<!-- Footer attribution (optional but good practice) -->
<footer class="text-center py-3 text-muted">
  Built with <a href="https://getbootstrap.com/" rel="noopener">Bootstrap</a>
</footer>
```

### Template/Theme Redistribution

```text
You CAN:
- Sell Bootstrap-based themes and templates
- Modify and redistribute Bootstrap's SCSS source
- Include Bootstrap in GPL or proprietary licensed projects

You MUST:
- Retain the MIT license text for Bootstrap's code
- Clearly indicate which parts are Bootstrap vs your own
```

## Best Practices

1. **Include a LICENSE or THIRD-PARTY file in your repository** - Documents all dependencies and their licenses.
2. **Keep the MIT license notice intact in minified files** - The `bootstrap.min.css` header comment already contains it.
3. **Do not remove copyright headers from SCSS source files** - Each file contains attribution.
4. **Use the Bootstrap name accurately** - Say "built with Bootstrap" not "Bootstrap Certified."
5. **Check license compatibility with your project** - MIT is compatible with GPL, Apache, and proprietary licenses.
6. **Document your Bootstrap version in your LICENSE file** - Helps with auditing.
7. **Review the license on each major upgrade** - License terms occasionally get clarified.
8. **Avoid using the Bootstrap logo without permission** - The logo is trademarked.
9. **Credit contributors if redistributing modified source** - Follow open-source community norms.
10. **Include license notices in compiled/bundled output** - Bundlers can strip comments; preserve legal text.

## Common Pitfalls

1. **Assuming "free" means no attribution required** - MIT requires including the copyright notice.
2. **Using the Bootstrap logo in your product branding** - The logo is a trademark; using it implies official endorsement.
3. **Removing license headers from minified CSS/JS** - Legally you must retain them in copies.
4. **Confusing MIT license with Bootstrap trademark policy** - The license covers code use; the trademark covers branding.
5. **Assuming Bootstrap requires GPL-style copyleft** - MIT does not require you to open-source your code.

## Accessibility Considerations

The Bootstrap license does not impose accessibility requirements on your implementation. However, as a best practice, when using an open-source framework, you should maintain the accessibility features Bootstrap provides rather than removing them. The MIT license grants you the freedom to strip ARIA attributes, but doing so is strongly discouraged for ethical and legal accessibility compliance (ADA, WCAG).

## Responsive Behavior

Licensing has no impact on Bootstrap's responsive behavior. All responsive grid, utility, and component functionality is available under the MIT license without restriction. You are free to modify the breakpoint system, add custom breakpoints, or remove responsive features entirely in both commercial and personal projects.
