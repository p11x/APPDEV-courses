# GitHub Pages Functionalities

## 1. User/Organization Site

Create at: `username.github.io`

```
username.github.io/
├── index.html
├── about.html
├── css/
│   └── style.css
└── js/
    └── main.js
```

## 2. Project Site

Create in: `username.github.io/repository`

Enable in: Settings → Pages → Source

## 3. Jekyll Configuration

```yaml
# _config.yml
title: My Portfolio
description: My personal website
theme: jekyll-theme-minimal
```

## 4. Custom Domain

Add in Settings → Pages:

```
yourdomain.com
```

DNS Configuration:
- A record: @ → 185.199.108.153
- CNAME: www → username.github.io

---

*Back to [Web Development README](../README.md)*
*Back to [Main README../../README.md)*