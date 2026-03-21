# res.download() - File Downloads

## 📌 What You'll Learn
- How to serve files for download
- Setting custom filenames
- Using file paths

## 🧠 Concept Explained (Plain English)

The `res.download()` method tells the browser to download a file instead of displaying it. Think of it like attaching a file to an email - instead of showing the content in the message, it forces the recipient to save it.

This is useful for:
- PDF reports
- Generated documents
- Media files
- Exported data

## 💻 Code Example

```javascript
// ES Module - Using res.download()

import express from 'express';
import path from 'path';

const app = express();

// Basic file download
app.get('/download/report', (req, res) => {
    // req = request object, res = response object
    const filePath = path.join(process.cwd(), 'files', 'report.pdf');
    res.download(filePath);
});

// Download with custom filename
app.get('/download/invoice/:id', (req, res) => {
    const filePath = path.join(process.cwd(), 'invoices', `${req.params.id}.pdf`);
    // Second argument sets the downloaded filename
    res.download(filePath, `invoice-${req.params.id}.pdf`);
});

// Download with error handling
app.get('/download/:filename', (req, res) => {
    const filePath = path.join(process.cwd(), 'files', req.params.filename);
    
    // Check if file exists
    // In real app, use fs.existsSync() or async methods
    res.download(filePath, (err) => {
        if (err) {
            res.status(404).json({ error: 'File not found' });
        }
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 14 | `res.download(filePath);` | Serve file for download |
| 20 | `res.download(filePath, 'custom-name.pdf');` | Download with custom filename |
| 30 | `res.download(filePath, (err) => {...})` | Download with error handling |

## ✅ Quick Recap

- Use res.download(filePath) to serve downloads
- Optional second argument sets custom filename
- Always handle errors for missing files

## 🔗 What's Next

Learn about [res.setHeaders()](./res-set-headers.md)