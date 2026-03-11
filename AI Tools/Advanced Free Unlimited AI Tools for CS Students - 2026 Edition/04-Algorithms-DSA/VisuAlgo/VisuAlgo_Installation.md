# VisuAlgo - Installation Guide

## Access Method

VisuAlgo is a **browser-based platform** that requires no installation. Users can access it directly through their web browser.

## Quick Start

### Step 1: Open Browser

Open any modern web browser:
- Google Chrome (Recommended)
- Mozilla Firefox
- Apple Safari
- Microsoft Edge

### Step 2: Navigate to VisuAlgo

Visit the official website:

```
https://visualgo.net/en
```

### Step 3: Select Algorithm

1. Browse the list of available visualizations
2. Click on the desired algorithm category
3. Select specific algorithm to study

## Supported Browsers

| Browser | Version | Support Status |
|---------|---------|----------------|
| Chrome | 80+ | Full Support |
| Firefox | 75+ | Full Support |
| Safari | 13+ | Full Support |
| Edge | 80+ | Full Support |

## Mobile Access

### Supported Devices

- Smartphones (iOS/Android)
- Tablets (iPad/Android)

### Limitations on Mobile

- Optimized for desktop viewing
- Some interactive features may be limited
- Recommend landscape mode for better experience

## Offline Usage

### Local Clone Setup

For offline access, you can clone the GitHub repository:

```bash
# Clone the repository
git clone https://github.com/williamngan/visualgo.git

# Navigate to directory
cd visualgo

# Open in browser (any of these methods)
# Method 1: Using Python
python -m http.server 8000

# Method 2: Using Node.js
npx http-server

# Method 3: Simply open index.html in browser
```

### Running Locally

```bash
# Start local server
cd visualgo
python -m http.server 8080

# Access via browser
# http://localhost:8080
```

## Bookmark Setup

### Chrome

1. Navigate to https://visualgo.net/en
2. Press `Ctrl + D` (Windows) or `Cmd + D` (Mac)
3. Enter bookmark name: "VisuAlgo"
4. Click "Done"

### Firefox

1. Navigate to https://visualgo.net/en
2. Press `Ctrl + D` (Windows) or `Cmd + D` (Mac)
3. Confirm bookmark creation

## Language Selection

VisuAlgo supports multiple languages:

| Language | URL |
|----------|-----|
| English | https://visualgo.net/en |
| Chinese | https://visualgo.net/zh |
| Indonesian | https://visualgo.net/id |
| Vietnamese | https://visualgo.net/vn |

## Quick Navigation

### Main Menu Options

- **sl:** Sorting visualization
- **ll:** Linked List visualization
- **bst:** Binary Search Tree
- **heap:** Heap data structure
- **graph:** Graph algorithms

### Direct URL Access

Navigate directly to specific visualizations:

```
https://visualgo.net/en/sorting
https://visualgo.net/en/ll
https://visualgo.net/en/bst
https://visualgo.net/en/heap
https://visualgo.net/en/graph
```

---

*Back to [Algorithms DSA README](../README.md)*