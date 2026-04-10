# File Handling

## Learning Objectives

1. Reading and writing files
2. Using internal and external storage
3. Working with JSON files

```kotlin
package com.android.data.files

object FileHandling {
    
    // Internal storage
    class InternalStorage(private val context: android.content.Context) {
        
        fun writeFile(fileName: String, content: String) {
            context.openFileOutput(fileName, android.content.Context.MODE_PRIVATE).use { 
                it.write(content.toByteArray())
            }
        }
        
        fun readFile(fileName: String): String {
            return context.openFileInput(fileName).bufferedReader().use { it.readText() }
        }
        
        fun deleteFile(fileName: String): Boolean {
            return context.deleteFile(fileName)
        }
        
        fun listFiles(): Array<String> {
            return context.fileList()
        }
    }
    
    // External storage
    class ExternalStorage {
        
        fun writeToExternal(fileName: String, content: String) {
            // Check permissions first!
            val dir = android.os.Environment.getExternalStorageDirectory()
            val file = java.io.File(dir, fileName)
            file.writeText(content)
        }
    }
}
```