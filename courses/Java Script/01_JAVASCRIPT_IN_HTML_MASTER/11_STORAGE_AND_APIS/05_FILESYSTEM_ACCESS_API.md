# 💻 File System Access API Complete Guide

## Native File Operations in Browser

---

## Table of Contents

1. [Introduction to File System Access API](#introduction-to-file-system-access-api)
2. [Basic File Operations](#basic-file-operations)
3. [Directory Handling](#directory-handling)
4. [Reading Files](#reading-files)
5. [Writing Files](#writing-files)
6. [File Handles](#file-handles)
7. [Drag and Drop Integration](#drag-and-drop-integration)
8. [Security Considerations](#security-considerations)
9. [Professional Use Cases](#professional-use-cases)
10. [Common Pitfalls](#common-pitfalls)
11. [Key Takeaways](#key-takeaways)

---

## Introduction to File System Access API

The File System Access API enables web applications to interact with the user's local file system. It provides:

- **Read files**: Open and read local files
- **Write files**: Save changes back to files
- **Directory access**: Read directory contents
- **File handles**: Track open files for later access
- **Drag and drop**: Handle file drops natively

### Browser Support

| Browser | Support |
|---------|---------|
| Chrome  | Full (86+) |
| Edge    | Full (86+) |
| Opera   | Full (72+) |
| Firefox | Not supported |
| Safari  | Partial (15.4+) |

Use feature detection:
```javascript
const supported = 'showOpenFilePicker' in window;
```

---

## Basic File Operations

### Opening Files

```javascript
// ===== File: file-open.js =====
// Opening Files

// Open single file
async function openFile() {
    try {
        const [fileHandle] = await window.showOpenFilePicker({
            types: [
                {
                    description: 'Text Files',
                    accept: { 'text/plain': ['.txt'] }
                },
                {
                    description: 'All Files',
                    accept: { '*/*': [] }
                }
            ],
            multiple: false
        });
        
        const file = await fileHandle.getFile();
        return { handle: fileHandle, file };
    } catch (error) {
        if (error.name === 'AbortError') {
            console.log('File selection cancelled');
            return null;
        }
        throw error;
    }
}

// Open multiple files
async function openMultipleFiles() {
    const fileHandles = await window.showOpenFilePicker({
        multiple: true,
        types: [
            {
                description: 'Images',
                accept: { 'image/*': ['.png', '.jpg', '.gif', '.webp'] }
            }
        ]
    });
    
    const files = await Promise.all(
        fileHandles.map(handle => handle.getFile())
    );
    
    return files;
}
```

### Saving Files

```javascript
// ===== File: file-save.js =====
// Saving Files

// Save file (show save dialog)
async function saveFile(content, defaultName = 'document.txt') {
    try {
        const fileHandle = await window.showSaveFilePicker({
            suggestedName: defaultName,
            types: [
                {
                    description: 'Text File',
                    accept: { 'text/plain': ['.txt'] }
                }
            ]
        });
        
        const writable = await fileHandle.createWritable();
        await writable.write(content);
        await writable.close();
        
        return fileHandle;
    } catch (error) {
        if (error.name === 'AbortError') {
            return null;
        }
        throw error;
    }
}

// Save with content type
async function saveJSON(data, defaultName = 'data.json') {
    const json = JSON.stringify(data, null, 2);
    
    const fileHandle = await window.showSaveFilePicker({
        suggestedName: defaultName,
        types: [
            {
                description: 'JSON File',
                accept: { 'application/json': ['.json'] }
            }
        ]
    });
    
    const writable = await fileHandle.createWritable();
    await writable.write(json);
    await writable.close();
    
    return fileHandle;
}

// Save with specific MIME type
async function saveAsHTML(content) {
    return saveFile(content, 'document.html');
}
```

---

## Directory Handling

### Opening Directories

```javascript
// ===== File: directory-open.js =====
// Opening Directories

// Open directory
async function openDirectory() {
    try {
        const dirHandle = await window.showDirectoryPicker({
            mode: 'readwrite'
        });
        
        return dirHandle;
    } catch (error) {
        if (error.name === 'AbortError') {
            return null;
        }
        throw error;
    }
}

// List directory contents
async function listDirectory(dirHandle) {
    const entries = [];
    
    for await (const entry of dirHandle.values()) {
        entries.push({
            name: entry.name,
            kind: entry.kind, // 'file' or 'directory'
            handle: entry
        });
    }
    
    return entries;
}

// Recursive directory listing
async function listDirectoryRecursive(dirHandle, path = '') {
    const entries = [];
    
    for await (const entry of dirHandle.values()) {
        const entryPath = path ? `${path}/${entry.name}` : entry.name;
        
        if (entry.kind === 'directory') {
            const subEntries = await listDirectoryRecursive(entry, entryPath);
            entries.push(...subEntries);
        } else {
            entries.push(entryPath);
        }
    }
    
    return entries;
}
```

### Creating Directories

```javascript
// ===== File: directory-create.js =====
// Creating Directories

// Create subdirectory
async function createDirectory(parentDirHandle, name) {
    try {
        const dirHandle = await parentDirHandle.getDirectoryHandle(name, {
            create: true
        });
        
        return dirHandle;
    } catch (error) {
        console.error('Failed to create directory:', error);
        throw error;
    }
}

// Create nested directories
async function createNestedDirectories(parentDirHandle, path) {
    const parts = path.split('/');
    let currentDir = parentDirHandle;
    
    for (const part of parts) {
        currentDir = await currentDir.getDirectoryHandle(part, {
            create: true
        });
    }
    
    return currentDir;
}
```

---

## Reading Files

### Reading File Content

```javascript
// ===== File: file-read.js =====
// Reading File Content

// Read text file
async function readTextFile(fileHandle) {
    const file = await fileHandle.getFile();
    const text = await file.text();
    return text;
}

// Read JSON file
async function readJSONFile(fileHandle) {
    const text = await readTextFile(fileHandle);
    return JSON.parse(text);
}

// Read with FileReader (more control)
async function readFileAsDataURL(fileHandle) {
    const file = await fileHandle.getFile();
    
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        
        reader.onload = () => resolve(reader.result);
        reader.onerror = () => reject(reader.error);
        
        reader.readAsDataURL(file);
    });
}

// Stream large files
async function streamRead(fileHandle) {
    const file = await fileHandle.getFile();
    const stream = file.stream();
    const reader = stream.getReader();
    
    const chunks = [];
    
    while (true) {
        const { done, value } = await reader.read();
        
        if (done) {
            break;
        }
        
        chunks.push(value);
    }
    
    const allChunks = new Uint8Array(
        chunks.reduce((acc, chunk) => acc + chunk.length, 0)
    );
    
    let position = 0;
    
    for (const chunk of chunks) {
        allChunks.set(chunk, position);
        position += chunk.length;
    }
    
    return new TextDecoder().decode(allChunks);
}
```

### Reading Specific Parts

```javascript
// ===== File: file-slice.js =====
// Reading File Slices

// Read first N bytes
async function readFileStart(fileHandle, length = 1024) {
    const file = await fileHandle.getFile();
    const slice = file.slice(0, length);
    
    return slice.text();
}

// Read last N bytes
async function readFileEnd(fileHandle, length = 1024) {
    const file = await fileHandle.getFile();
    const start = Math.max(0, file.size - length);
    const slice = file.slice(start);
    
    return slice.text();
}

// Read byte range
async function readFileRange(fileHandle, start, end) {
    const file = await fileHandle.getFile();
    const slice = file.slice(start, end);
    
    return slice.text();
}

// Binary read
async function readFileBinary(fileHandle) {
    const file = await fileHandle.getFile();
    const buffer = await file.arrayBuffer();
    
    return new Uint8Array(buffer);
}
```

---

## Writing Files

### Basic Write Operations

```javascript
// ===== File: file-write.js =====
// Writing Files

// Write text to file
async function writeTextFile(fileHandle, content) {
    const writable = await fileHandle.createWritable();
    await writable.write(content);
    await writable.close();
}

// Write JSON to file
async function writeJSONFile(fileHandle, data) {
    const json = JSON.stringify(data, null, 2);
    await writeTextFile(fileHandle, json);
}

// Append to file
async function appendToFile(fileHandle, content) {
    const file = await fileHandle.getFile();
    const writable = await fileHandle.createWritable({
        keepExistingBytes: true
    });
    
    await writable.seek(file.size);
    await writable.write(content);
    await writable.close();
}

// Write with encoding
async function writeWithEncoding(fileHandle, content, encoding = 'utf-8') {
    const encoder = new TextEncoder();
    const data = encoder.encode(content);
    
    const writable = await fileHandle.createWritable();
    await writable.write(data);
    await writable.close();
}
```

### Transactional Writes

```javascript
// ===== File: file-transaction.js =====
// Transactional Writes

// Write with backup
async function safeWrite(fileHandle, content) {
    const file = await fileHandle.getFile();
    const originalContent = await file.text();
    
    try {
        const writable = await fileHandle.createWritable();
        await writable.write(content);
        await writable.close();
    } catch (error) {
        // Restore original on error
        const writable = await fileHandle.createWritable();
        await writable.write(originalContent);
        await writable.close();
        
        throw error;
    }
}

// Write with confirmation check
async function writeWithVerification(fileHandle, content) {
    // Write
    const writable = await fileHandle.createWritable();
    await writable.write(content);
    await writable.close();
    
    // Verify
    const savedFile = await fileHandle.getFile();
    const savedContent = await savedFile.text();
    
    if (savedContent !== content) {
        throw new Error('Write verification failed');
    }
}
```

---

## File Handles

### Persisting File Handles

```javascript
// ===== File: handle-persist.js =====
// Persisting File Handles

// Store handle in IndexedDB
async function saveHandle(storeName, handle) {
    const handleData = await handle;
    
    const db = await openDatabase('handles', 1);
    const transaction = db.transaction(storeName, 'readwrite');
    const store = transaction.objectStore(storeName);
    
    await store.add(handleData);
}

async function openDatabase(name, version) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(name, version);
        
        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve(request.result);
        
        request.onupgradeneeded = (event) => {
            const db = event.target.result;
            
            if (!db.objectStoreNames.contains('handles')) {
                db.createObjectStore('handles', { keyPath: 'name' });
            }
        };
    });
}

// Restore handle
async function restoreHandle(storeName, name) {
    const db = await openDatabase('handles', 1);
    const transaction = db.transaction(storeName, 'readonly');
    const store = transaction.objectStore(storeName);
    
    return new Promise((resolve, reject) => {
        const request = store.get(name);
        
        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve(request.result);
    });
}

// Check if permission granted
async function checkPermission(handle, readWrite = false) {
    const permission = await handle.queryPermission({
        mode: readWrite ? 'readwrite' : 'read'
    });
    
    return permission === 'granted';
}
```

### Request Permissions

```javascript
// ===== File: permissions.js =====
// Request Permissions

// Request file permission
async function requestFilePermission(fileHandle) {
    const options = {
        mode: 'readwrite'
    };
    
    try {
        const permission = await fileHandle.requestPermission(options);
        return permission === 'granted';
    } catch (error) {
        console.error('Permission denied:', error);
        return false;
    }
}

// Request directory permission
async function requestDirectoryPermission(dirHandle) {
    const options = {
        mode: 'readwrite'
    };
    
    try {
        const permission = await dirHandle.requestPermission(options);
        return permission === 'granted';
    } catch (error) {
        console.error('Permission denied:', error);
        return false;
    }
}

// Request permission for stored handle
async function requestStoredPermission(handle, readWrite = false) {
    const permission = await handle.queryPermission({
        mode: readWrite ? 'readwrite' : 'read'
    });
    
    if (permission === 'prompt') {
        return handle.requestPermission({
            mode: readWrite ? 'readwrite' : 'read'
        });
    }
    
    return permission === 'granted';
}
```

---

## Drag and Drop Integration

### Drop Handler

```javascript
// ===== File: drag-drop.js =====
// Drag and Drop

// Handle file drop
async function handleFileDrop(event) {
    event.preventDefault();
    
    const items = event.dataTransfer.items;
    const files = [];
    
    for (const item of items) {
        if (item.kind === 'file') {
            const file = item.getAsFile();
            files.push(file);
        }
    }
    
    return files;
}

// Handle directory drop
async function handleDirectoryDrop(event) {
    event.preventDefault();
    
    const items = event.dataTransfer.items;
    const handles = [];
    
    for (const item of items) {
        const entry = item.webkitGetAsEntry?.();
        
        if (entry) {
            handles.push(entry);
        }
    }
    
    return handles;
}

// Enable drop zone
function enableDropZone(element) {
    element.addEventListener('dragover', (event) => {
        event.preventDefault();
    });
    
    element.addEventListener('drop', handleFileDrop);
}
```

### Drag and Drop with File System Access

```javascript
// ===== File: drag-drop-fs.js =====
// Drag and Drop with File System Access

// Handle file drop with File System Access API
async function handleFSADrop(event, dirHandle) {
    event.preventDefault();
    
    const items = event.dataTransfer.items;
    
    for (const item of items) {
        const entry = item.webkitGetAsEntry?.();
        
        if (entry && entry.isFile) {
            const file = await getFileFromEntry(entry);
            
            // Copy to directory
            await copyFileToDirectory(dirHandle, file.name, file);
        }
    }
}

// Get file from entry
async function getFileFromEntry(entry) {
    return new Promise((resolve, reject) => {
        entry.file((file) => resolve(file), reject);
    });
}

// Copy file to directory
async function copyFileToDirectory(dirHandle, name, file) {
    const fileHandle = await dirHandle.getFileHandle(name, {
        create: true
    });
    
    const writable = await fileHandle.createWritable();
    await writable.write(file);
    await writable.close();
}
```

---

## Security Considerations

### Best Practices

```javascript
// ===== File: security.js =====
// Security Best Practices

// Verify user gesture
async function verifyUserIntent() {
    // File System Access API requires user gesture
    // Check if called from user-initiated action
    
    // This should only be called from click handler
    if (!document.hasStorageAccess()) {
        console.warn('No user gesture detected');
        return false;
    }
    
    return true;
}

// Handle malicious files
function sanitizeFilename(name) {
    // Remove path components
    const sanitized = name.replace(/^.*[\\/]/, '');
    
    // Remove null bytes
    return sanitized.replace(/\0/g, '');
}

// Validate file size
async function validateFileSize(fileHandle, maxSize = 100 * 1024 * 1024) {
    const file = await fileHandle.getFile();
    
    if (file.size > maxSize) {
        throw new Error('File too large');
    }
    
    return true;
}

// Check file type
async function validateFileType(fileHandle, allowedTypes) {
    const file = await fileHandle.getFile();
    const type = file.type;
    
    const allowed = allowedTypes.some(allowed => type.includes(allowed));
    
    if (!allowed) {
        throw new Error('File type not allowed');
    }
    
    return true;
}
```

---

## Professional Use Cases

### Use Case 1: Text Editor

```javascript
// ===== File: use-case-editor.js =====
// Simple Text Editor

class TextEditor {
    constructor() {
        this.fileHandle = null;
        this.isModified = false;
        this.content = '';
    }
    
    async open() {
        const [handle] = await window.showOpenFilePicker({
            types: [{
                description: 'Text Files',
                accept: { 'text/plain': ['.txt', '.md', '.js', '.html', '.css'] }
            }]
        });
        
        const file = await handle.getFile();
        this.content = await file.text();
        this.fileHandle = handle;
        
        return this.content;
    }
    
    async save() {
        if (!this.fileHandle) {
            return this.saveAs();
        }
        
        const writable = await this.fileHandle.createWritable();
        await writable.write(this.content);
        await writable.close();
        
        this.isModified = false;
    }
    
    async saveAs() {
        const handle = await window.showSaveFilePicker({
            suggestedName: 'document.txt',
            types: [{
                description: 'Text Files',
                accept: { 'text/plain': ['.txt'] }
            }]
        });
        
        this.fileHandle = handle;
        
        const writable = await handle.createWritable();
        await writable.write(this.content);
        await writable.close();
        
        this.isModified = false;
    }
    
    updateContent(content) {
        this.content = content;
        this.isModified = true;
    }
}
```

### Use Case 2: Image Processor

```javascript
// ===== File: use-case-image.js =====
// Image Processor

class ImageProcessor {
    async loadImage() {
        const [handle] = await window.showOpenFilePicker({
            types: [{
                description: 'Images',
                accepts: { 'image/*': ['.png', '.jpg', '.jpeg', '.gif', '.webp'] }
            }]
        });
        
        const file = await handle.getFile();
        const url = URL.createObjectURL(file);
        
        return { handle, url, file };
    }
    
    async processImage(handle, transform) {
        const file = await handle.getFile();
        const bitmap = await createImageBitmap(file);
        
        const canvas = document.createElement('canvas');
        canvas.width = bitmap.width;
        canvas.height = bitmap.height;
        
        const ctx = canvas.getContext('2d');
        transform(ctx, bitmap);
        
        const processedFile = await new Promise(resolve => {
            canvas.toBlob(blob => resolve(blob));
        });
        
        return processedFile;
    }
    
    async saveImage(imageBlob) {
        return window.showSaveFilePicker({
            suggestedName: 'processed-image.png',
            types: [{
                description: 'PNG Image',
                accept: { 'image/png': ['.png'] }
            }]
        });
    }
}
```

### Use Case 3: File Manager

```javascript
// ===== File: use-case-manager.js =====
// File Manager

class FileManager {
    async openFolder() {
        this.folderHandle = await window.showDirectoryPicker({
            mode: 'readwrite'
        });
        
        return this.getFiles();
    }
    
    async getFiles() {
        const files = [];
        
        for await (const entry of this.folderHandle.values()) {
            files.push({
                name: entry.name,
                kind: entry.kind,
                handle: entry
            });
        }
        
        return files;
    }
    
    async createFile(name, content) {
        const handle = await this.folderHandle.getFileHandle(name, {
            create: true
        });
        
        const writable = await handle.createWritable();
        await writable.write(content);
        await writable.close();
        
        return handle;
    }
    
    async createFolder(name) {
        return this.folderHandle.getDirectoryHandle(name, {
            create: true
        });
    }
    
    async delete(name) {
        await this.folderHandle.removeEntry(name);
    }
    
    async rename(oldName, newName) {
        const entry = await this.folderHandle.getEntry(oldName);
        
        // Copy to new name
        if (entry.kind === 'file') {
            const file = await entry.getFile();
            const handle = await this.folderHandle.getFileHandle(newName, {
                create: true
            });
            
            const writable = await handle.createWritable();
            await writable.write(file);
            await writable.close();
        } else {
            await this.folderHandle.getDirectoryHandle(newName, {
                create: true
            });
        }
        
        // Delete old
        await this.folderHandle.removeEntry(oldName);
    }
}
```

---

## Common Pitfalls

1. **No user gesture**: Must be triggered by user action
2. **Not checking support**: Browser compatibility issues
3. **Permission not requested**: Forgot to request readwrite
4. **Stale handles**: Handles invalidated after app restart
5. **Not handling errors**: File access errors throw exceptions
6. **Large file streaming**: Memory issues with large files
7. **Encoding issues**: Text encoding problems
8. **Path traversal**: Security vulnerabilities
9. **Not verifying writes**: Data may not be written
10. **Not cleaning up URLs**: Object URLs leak memory

---

## Key Takeaways

- File System Access API needs user gesture to work
- Always check browser support with feature detection
- Request permissions after file handle retrieval
- Store handles in IndexedDB for persistence
- Use streaming for large files
- Handle errors gracefully
- Keep file handles in memory during session
- Always verify writes
- Close writable streams properly
- Clean up Object URLs to prevent memory leaks

---

## Related Files

- [01_LOCAL_STORAGE_SESSION_STORAGE.md](./01_LOCAL_STORAGE_SESSION_STORAGE.md) - For simple storage
- [02_INDEXEDDB_ADVANCED.md](./02_INDEXEDDB_ADVANCED.md) - For storing handles
- [06_STORAGE_SECURITY_BEST_PRACTICES.md](./06_STORAGE_SECURITY_BEST_PRACTICES.md) - For security