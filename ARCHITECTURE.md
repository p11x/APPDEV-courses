# 🏗️ Architecture

## How This Repo Works

```
Your PC (lightweight only)          GitHub Servers (all heavy work)
────────────────────────          ──────────────────────────────
auto_commit.py                      GitHub Repository
  └── Watches for file changes   →    └── Stores all 15,142 files
  └── git push (small packets)   →    GitHub Actions
                                        └── Builds & deploys site
                                        GitHub Pages
                                        └── Hosts site 24/7
                                        └── https://p11x.github.io/APPDEV-courses
```

## What runs on your PC
| Process | CPU | RAM | Network |
|---------|-----|-----|---------|
| auto_commit.py | < 0.1% | ~15MB | Only on push (1x/hour) |

## What runs on GitHub servers
| Service | What it does |
|---------|-------------|
| GitHub Repository | Stores all files |
| GitHub Actions | Auto-deploys on every push |
| GitHub Pages | Hosts site 24/7 for free |

## Result
- ✅ Site is online 24/7
- ✅ Zero local server
- ✅ Zero port usage on your PC
- ✅ Auto-updates on every file change
- ✅ PC impact: ~15MB RAM, <0.1% CPU