# 📄 Project 15: Document Editor

## 📋 Project Overview

Build a rich text document editor with formatting options, document management, local storage autosave, and export capabilities. This project demonstrates:
- ContentEditable API
- Rich text formatting
- Document management
- Auto-save functionality

---

## 🎯 Core Features

```javascript
class DocumentEditor {
    constructor(editorElement) {
        this.editor = editorElement;
        this.documents = [];
        this.currentDocument = null;
        this.autoSaveInterval = null;
        this.loadFromStorage();
    }
    
    init() {
        this.editor.contentEditable = true;
        this.editor.addEventListener('input', () => this.onContentChange());
    }
    
    execCommand(command, value = null) {
        document.execCommand(command, false, value);
        this.onContentChange();
    }
    
    formatBold() { this.execCommand('bold'); }
    formatItalic() { this.execCommand('italic'); }
    formatUnderline() { this.execCommand('underline'); }
    formatHeading(level) { this.execCommand('formatBlock', `h${level}`); }
    formatBulletList() { this.execCommand('insertUnorderedList'); }
    formatNumberedList() { this.execCommand('insertOrderedList'); }
    
    createDocument(title) {
        const doc = {
            id: this.generateId(),
            title,
            content: '',
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString()
        };
        
        this.documents.push(doc);
        this.saveToStorage();
        return doc;
    }
    
    saveCurrentDocument() {
        if (!this.currentDocument) return;
        
        this.currentDocument.content = this.editor.innerHTML;
        this.currentDocument.updatedAt = new Date().toISOString();
        this.saveToStorage();
    }
    
    loadDocument(docId) {
        const doc = this.documents.find(d => d.id === docId);
        if (doc) {
            this.currentDocument = doc;
            this.editor.innerHTML = doc.content;
        }
    }
    
    onContentChange() {
        this.startAutoSave();
    }
    
    startAutoSave() {
        clearTimeout(this.autoSaveInterval);
        this.autoSaveInterval = setTimeout(() => {
            this.saveCurrentDocument();
        }, 2000);
    }
    
    exportToHTML() {
        return this.editor.innerHTML;
    }
    
    exportToText() {
        return this.editor.innerText;
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('documents', JSON.stringify(this.documents));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('documents');
        if (stored) {
            try {
                this.documents = JSON.parse(stored);
            } catch (e) {
                this.documents = [];
            }
        }
    }
}
```

---

## 🔗 Related Topics

- [06_Event_Handling_Deep_Dive.md](../09_DOM_MANIPULATION/06_Event_Handling_Deep_Dive.md)
- [08_Event_Delegation_Patterns.md](../09_DOM_MANIPULATION/08_Event_Delegation_Patterns.md)

---

**Projects Module: 15/32 Complete** 🎉

Now let's start the Testing Module!