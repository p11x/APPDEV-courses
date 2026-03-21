# File Upload Handling in React Forms

## Overview

File uploads are a common requirement in web applications, from profile pictures to document attachments. While React Hook Form primarily handles text-based form data, it provides excellent support for file inputs. This guide covers handling file inputs with React Hook Form, implementing image previews, creating drag-and-drop upload zones, and tracking upload progress with axios.

## Prerequisites

- Understanding of React Hook Form basics
- Familiarity with TypeScript
- Basic knowledge of the File API in browsers
- Understanding of form submission patterns

## Core Concepts

### Basic File Input with React Hook Form

File inputs in React Hook Form work similarly to other input types. The key difference is that file inputs return a FileList object (or null) rather than a string value.

```tsx
// File: src/components/BasicFileUpload.tsx

import { useForm } from 'react-hook-form';

// Define the form data shape - avatar is a FileList
interface ProfileData {
  avatar: FileList;
  name: string;
}

function BasicFileUpload() {
  const { 
    register, 
    handleSubmit, 
    formState: { errors }
  } = useForm<ProfileData>();

  const onSubmit = (data: ProfileData) => {
    // Get the first file from the FileList
    const file = data.avatar?.[0];
    
    if (file) {
      console.log("File name:", file.name);
      console.log("File size:", file.size, "bytes");
      console.log("File type:", file.type);
      
      // You can now upload this file to your server
      // We'll see how to do this later
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Profile Picture Upload</h2>

      <div className="field">
        <label htmlFor="avatar">Profile Picture</label>
        {/* File input - accept only images */}
        <input
          id="avatar"
          type="file"
          accept="image/*"
          {...register("avatar", {
            // Validation: file is required
            required: "Please select a profile picture",
            // Validate file type and size
            validate: {
              // Check that it's an image
              isImage: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true; // let required handle empty
                return file.type.startsWith("image/") || "Must be an image file";
              },
              // Check file size (max 5MB)
              isSmallEnough: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true;
                return file.size <= 5 * 1024 * 1024 || "File must be under 5MB";
              }
            }
          })}
        />
        {errors.avatar && (
          <span className="error">{errors.avatar.message}</span>
        )}
      </div>

      <div className="field">
        <label htmlFor="name">Display Name</label>
        <input
          id="name"
          {...register("name", { required: "Name is required" })}
        />
        {errors.name && (
          <span className="error">{errors.name.message}</span>
        )}
      </div>

      <button type="submit">Upload Profile</button>
    </form>
  );
}

export default BasicFileUpload;
```

### Image Preview with FileReader

Before uploading a file, users typically want to see a preview of what they're uploading. The FileReader API allows us to read file contents and display them as data URLs.

```tsx
// File: src/components/ImageUploadWithPreview.tsx

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';

interface ImageUploadData {
  image: FileList;
  caption: string;
}

function ImageUploadWithPreview() {
  const { 
    register, 
    handleSubmit, 
    watch,
    formState: { errors }
  } = useForm<ImageUploadData>();
  
  // Watch the file input to get the selected file
  const watchedFile = watch("image");
  
  // State for the preview URL
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);

  // When the watched file changes, create a preview
  useEffect(() => {
    const file = watchedFile?.[0];
    
    if (file) {
      // Create an Object URL - more efficient than FileReader for previews
      // This creates a blob: URL that can be used as an img src
      const objectUrl = URL.createObjectURL(file);
      setPreviewUrl(objectUrl);
      
      // Clean up the URL when the component unmounts or file changes
      // This prevents memory leaks
      return () => URL.revokeObjectURL(objectUrl);
    } else {
      setPreviewUrl(null);
    }
  }, [watchedFile]);

  const onSubmit = (data: ImageUploadData) => {
    const file = data.image?.[0];
    if (file) {
      console.log("Uploading:", file.name, "with caption:", data.caption);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Upload Image with Preview</h2>

      {/* Preview area */}
      <div className="preview-area">
        {previewUrl ? (
          <img 
            src={previewUrl} 
            alt="Preview" 
            className="image-preview"
          />
        ) : (
          <div className="preview-placeholder">
            No image selected
          </div>
        )}
      </div>

      {/* File input */}
      <div className="field">
        <label htmlFor="image">Select Image</label>
        <input
          id="image"
          type="file"
          accept="image/png,image/jpeg,image/gif"
          {...register("image", {
            required: "Please select an image",
            validate: {
              isImage: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true;
                return file.type.startsWith("image/") || "Must be an image";
              },
              sizeLimit: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true;
                return file.size <= 10 * 1024 * 1024 || "Max 10MB";
              }
            }
          })}
        />
        {errors.image && (
          <span className="error">{errors.image.message}</span>
        )}
      </div>

      {/* Caption input */}
      <div className="field">
        <label htmlFor="caption">Caption</label>
        <input
          id="caption"
          {...register("caption", { 
            required: "Caption is required",
            maxLength: {
              value: 200,
              message: "Caption too long"
            }
          })}
          placeholder="Describe your image"
        />
        {errors.caption && (
          <span className="error">{errors.caption.message}</span>
        )}
      </div>

      {/* Show file info when selected */}
      {previewUrl && (
        <div className="file-info">
          <p>Selected: {watchedFile?.[0]?.name}</p>
          <p>Size: {((watchedFile?.[0]?.size ?? 0) / 1024).toFixed(1)} KB</p>
        </div>
      )}

      <button type="submit">Upload</button>
    </form>
  );
}

export default ImageUploadWithPreview;
```

### Drag and Drop Upload Zone

Modern upload interfaces often include drag-and-drop functionality. We can create a custom drop zone that accepts files via drag events or click-to-browse.

```tsx
// File: src/components/DragDropUpload.tsx

import { useState, useCallback } from 'react';
import { useForm } from 'react-hook-form';

interface UploadData {
  document: FileList;
  description: string;
}

function DragDropUpload() {
  const { 
    register, 
    handleSubmit, 
    setValue,
    watch,
    formState: { errors }
  } = useForm<UploadData>();

  const [isDragging, setIsDragging] = useState(false);
  
  // Watch the file to display info
  const watchedFile = watch("document");

  // Handle drag events
  const handleDragEnter = useCallback((e: React.DragEvent) => {
    e.preventDefault(); // Prevent default to allow drop
    e.stopPropagation();
    setIsDragging(true);
  }, []);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
  }, []);

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault(); // Necessary to allow dropping
    e.stopPropagation();
  }, []);

  const handleDrop = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);

    // Get the dropped files
    const files = e.dataTransfer.files;
    
    if (files && files.length > 0) {
      // Create a new DataTransfer object to set the FileList
      // This is necessary because FileList is read-only
      const dataTransfer = new DataTransfer();
      dataTransfer.items.add(files[0]);
      
      // Set the file in React Hook Form
      setValue("document", dataTransfer.files, { 
        shouldValidate: true 
      });
    }
  }, [setValue]);

  // Handle click to browse files
  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files && files.length > 0) {
      setValue("document", files, { shouldValidate: true });
    }
  };

  const onSubmit = (data: UploadData) => {
    const file = data.document?.[0];
    if (file) {
      console.log("Uploading:", file.name, data.description);
    }
  };

  const removeFile = () => {
    // Set to empty FileList to clear the selection
    setValue("document", new DataTransfer().files, { shouldValidate: true });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Document Upload</h2>

      {/* Drag and Drop Zone */}
      <div
        className={`drop-zone ${isDragging ? 'dragging' : ''} ${watchedFile?.length ? 'has-file' : ''}`}
        onDragEnter={handleDragEnter}
        onDragLeave={handleDragLeave}
        onDragOver={handleDragOver}
        onDrop={handleDrop}
        onClick={() => document.getElementById('file-input')?.click()}
      >
        {/* Hidden file input - triggered by clicking the zone */}
        <input
          id="file-input"
          type="file"
          accept=".pdf,.doc,.docx,.txt"
          style={{ display: 'none' }}
          {...register("document", {
            required: "Please upload a document",
            validate: {
              isValidType: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true;
                const validTypes = [
                  'application/pdf',
                  'application/msword',
                  'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
                  'text/plain'
                ];
                return validTypes.includes(file.type) || "Invalid file type";
              },
              isNotTooLarge: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true;
                return file.size <= 25 * 1024 * 1024 || "Max 25MB";
              }
            }
          })}
        />

        {/* Show preview or drop prompt */}
        {watchedFile?.length ? (
          <div className="file-selected">
            <div className="file-icon">📄</div>
            <div className="file-details">
              <p className="file-name">{watchedFile[0].name}</p>
              <p className="file-size">
                {((watchedFile[0].size ?? 0) / 1024 / 1024).toFixed(2)} MB
              </p>
            </div>
            <button 
              type="button" 
              className="remove-btn"
              onClick={(e) => {
                e.stopPropagation(); // Prevent triggering file select
                removeFile();
              }}
            >
              ×
            </button>
          </div>
        ) : (
          <div className="drop-prompt">
            <div className="upload-icon">📁</div>
            <p>Drag and drop your file here</p>
            <p className="sub-text">or click to browse</p>
            <p className="file-types">PDF, DOC, DOCX, TXT (max 25MB)</p>
          </div>
        )}
      </div>

      {/* Show errors */}
      {errors.document && (
        <span className="error">{errors.document.message}</span>
      )}

      {/* Description field */}
      <div className="field">
        <label htmlFor="description">Description</label>
        <textarea
          id="description"
          {...register("description", {
            required: "Description is required",
            maxLength: {
              value: 500,
              message: "Description too long"
            }
          })}
          placeholder="What is this document about?"
        />
        {errors.description && (
          <span className="error">{errors.description.message}</span>
        )}
      </div>

      <button type="submit">Upload Document</button>
    </form>
  );
}

export default DragDropUpload;
```

### Upload Progress with Axios

When uploading files, especially large ones, showing upload progress is essential for good user experience. Axios provides an onUploadProgress callback for tracking upload progress.

```tsx
// File: src/components/UploadWithProgress.tsx

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import axios from 'axios';

interface UploadData {
  file: FileList;
  title: string;
}

// Define a custom type for axios progress event
interface UploadProgress {
  loaded: number;
  total: number;
  percentage: number;
}

function UploadWithProgress() {
  const { 
    register, 
    handleSubmit, 
    watch,
    reset,
    formState: { errors }
  } = useForm<UploadData>();

  // Track upload progress state
  const [uploadProgress, setUploadProgress] = useState<number | null>(null);
  const [isUploading, setIsUploading] = useState(false);
  const [uploadComplete, setUploadComplete] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const watchedFile = watch("file");

  const onSubmit = async (data: UploadData) => {
    const file = data.file?.[0];
    if (!file) return;

    // Reset states for new upload
    setIsUploading(true);
    setUploadProgress(0);
    setUploadComplete(false);
    setError(null);

    try {
      // Create FormData to send file to server
      const formData = new FormData();
      formData.append('file', file);
      formData.append('title', data.title);

      // Upload with progress tracking
      await axios.post('/api/upload', formData, {
        // onUploadProgress receives a ProgressEvent
        onUploadProgress: (progressEvent: ProgressEvent) => {
          // Calculate percentage
          const percentage = Math.round(
            (progressEvent.loaded * 100) / (progressEvent.total ?? 1)
          );
          setUploadProgress(percentage);
        }
      });

      // Upload complete
      setUploadComplete(true);
      alert('Upload successful!');
      
      // Reset form after successful upload
      reset();
      setUploadProgress(null);
    } catch (err) {
      // Handle upload error
      if (axios.isAxiosError(err)) {
        setError(err.response?.data?.message || 'Upload failed');
      } else {
        setError('An unexpected error occurred');
      }
    } finally {
      setIsUploading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Upload File with Progress</h2>

      {/* File Input */}
      <div className="field">
        <label htmlFor="file">Select File</label>
        <input
          id="file"
          type="file"
          {...register("file", {
            required: "Please select a file",
            validate: {
              size: (fileList) => {
                const file = fileList?.[0];
                if (!file) return true;
                return file.size <= 100 * 1024 * 1024 || "Max 100MB";
              }
            }
          })}
          disabled={isUploading}
        />
        {errors.file && (
          <span className="error">{errors.file.message}</span>
        )}
      </div>

      {/* Title Input */}
      <div className="field">
        <label htmlFor="title">Title</label>
        <input
          id="title"
          {...register("title", { required: "Title is required" })}
          disabled={isUploading}
        />
        {errors.title && (
          <span className="error">{errors.title.message}</span>
        )}
      </div>

      {/* File Info */}
      {watchedFile?.[0] && (
        <div className="file-info">
          <p>Selected: {watchedFile[0].name}</p>
          <p>Size: {((watchedFile[0].size ?? 0) / 1024 / 1024).toFixed(2)} MB</p>
        </div>
      )}

      {/* Progress Bar */}
      {isUploading && (
        <div className="progress-container">
          <div className="progress-bar">
            <div 
              className="progress-fill"
              style={{ width: `${uploadProgress ?? 0}%` }}
            />
          </div>
          <p className="progress-text">
            {uploadProgress}% uploaded
          </p>
        </div>
      )}

      {/* Error Message */}
      {error && (
        <div className="error-banner" role="alert">
          {error}
        </div>
      )}

      {/* Success Message */}
      {uploadComplete && (
        <div className="success-banner">
          ✓ Upload completed successfully!
        </div>
      )}

      {/* Submit Button */}
      <button 
        type="submit" 
        disabled={isUploading || !watchedFile?.length}
      >
        {isUploading ? `Uploading... ${uploadProgress ?? 0}%` : "Upload"}
      </button>
    </form>
  );
}

export default UploadWithProgress;
```

### Multiple File Upload

Some applications require uploading multiple files at once. Here's how to handle multiple file selection and display previews for each.

```tsx
// File: src/components/MultipleFileUpload.tsx

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';

interface MultipleUploadData {
  images: FileList;
  title: string;
}

function MultipleFileUpload() {
  const { 
    register, 
    handleSubmit,
    watch,
    setValue,
    formState: { errors }
  } = useForm<MultipleUploadData>();

  const [previews, setPreviews] = useState<string[]>([]);
  const watchedFiles = watch("images");

  // Create previews for all selected images
  useEffect(() => {
    const files = watchedFiles;
    if (!files || files.length === 0) {
      setPreviews([]);
      return;
    }

    // Create preview URLs for all files
    const newPreviews = Array.from(files).map(file => 
      URL.createObjectURL(file)
    );
    
    setPreviews(newPreviews);

    // Cleanup
    return () => {
      newPreviews.forEach(url => URL.revokeObjectURL(url));
    };
  }, [watchedFiles]);

  const removeFile = (index: number) => {
    const files = watchedFiles;
    if (!files) return;

    // Create new FileList without the removed file
    const dataTransfer = new DataTransfer();
    Array.from(files).forEach((file, i) => {
      if (i !== index) dataTransfer.items.add(file);
    });
    
    setValue("images", dataTransfer.files, { shouldValidate: true });
  };

  const onSubmit = (data: MultipleUploadData) => {
    const files = Array.from(data.images);
    console.log("Uploading", files.length, "files with title:", data.title);
    
    // Create FormData for each file
    files.forEach(file => {
      console.log(" -", file.name, file.size, "bytes");
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>Upload Multiple Images</h2>

      <div className="field">
        <label htmlFor="images">Select Images</label>
        <input
          id="images"
          type="file"
          multiple
          accept="image/*"
          {...register("images", {
            required: "Please select at least one image",
            validate: {
              minFiles: (fileList) => {
                return (fileList?.length ?? 0) >= 1 || "Select at least 1 image";
              },
              maxFiles: (fileList) => {
                return (fileList?.length ?? 0) <= 10 || "Maximum 10 files allowed";
              },
              allImages: (fileList) => {
                const files = fileList ? Array.from(fileList) : [];
                const allImages = files.every(f => f.type.startsWith('image/'));
                return allImages || "All files must be images";
              }
            }
          })}
        />
        {errors.images && (
          <span className="error">{errors.images.message}</span>
        )}
      </div>

      <div className="field">
        <label htmlFor="title">Album Title</label>
        <input
          id="title"
          {...register("title", { required: "Title is required" })}
        />
        {errors.title && (
          <span className="error">{errors.title.message}</span>
        )}
      </div>

      {/* Image Previews Grid */}
      {previews.length > 0 && (
        <div className="preview-grid">
          {previews.map((preview, index) => (
            <div key={index} className="preview-item">
              <img src={preview} alt={`Preview ${index + 1}`} />
              <button
                type="button"
                className="remove-btn"
                onClick={() => removeFile(index)}
              >
                ×
              </button>
              <span className="file-name">
                {watchedFiles?.[index]?.name}
              </span>
            </div>
          ))}
        </div>
      )}

      {watchedFiles?.length && (
        <p className="file-count">
          {watchedFiles.length} file(s) selected
        </p>
      )}

      <button type="submit">
        Upload {watchedFiles?.length || 0} Images
      </button>
    </form>
  );
}

export default MultipleFileUpload;
```

## Common Mistakes

### Mistake 1: Trying to Modify FileList Directly

FileList is a read-only API. You cannot directly push files to it or modify it. Instead, use DataTransfer to create a new FileList.

```tsx
// ❌ WRONG - FileList is read-only
const handleFileSelect = (e) => {
  const fileList = e.target.files;
  fileList.push(newFile); // This will fail!
};

// ✅ CORRECT - Use DataTransfer to create a new FileList
const handleFileSelect = (e) => {
  const file = e.target.files[0];
  const dataTransfer = new DataTransfer();
  dataTransfer.items.add(file);
  // Now you can use dataTransfer.files as a FileList
  setValue("file", dataTransfer.files);
};
```

### Mistake 2: Not Revoking Object URLs

When using URL.createObjectURL(), you must revoke the URL when done to prevent memory leaks.

```tsx
// ❌ WRONG - Memory leak from unreleased URLs
useEffect(() => {
  if (file) {
    const url = URL.createObjectURL(file);
    setPreview(url);
  }
}, [file]);

// ✅ CORRECT - Revoke URL when component unmounts or file changes
useEffect(() => {
  if (file) {
    const url = URL.createObjectURL(file);
    setPreview(url);
    
    // Cleanup function
    return () => URL.revokeObjectURL(url);
  }
}, [file]);
```

### Mistake 3: Not Setting shouldValidate After Programmatic Updates

When setting file values programmatically (like in drag-and-drop), you need to trigger validation manually.

```tsx
// ❌ WRONG - Validation doesn't run after setValue
const handleDrop = (e) => {
  const files = e.dataTransfer.files;
  setValue("file", files); // No validation triggered!
};

// ✅ CORRECT - Trigger validation with setValue
const handleDrop = (e) => {
  const files = e.dataTransfer.files;
  setValue("file", files, { shouldValidate: true }); // Validation runs!
};
```

## Real-World Example

Here's a complete document management form that combines all the concepts: drag-and-drop, multiple file uploads, previews, progress tracking, and server communication.

```tsx
// File: src/components/DocumentManager.tsx

import { useState, useCallback, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import axios from 'axios';

interface FileWithPreview {
  file: File;
  preview: string;
  progress: number;
  status: 'pending' | 'uploading' | 'complete' | 'error';
}

interface DocumentUploadData {
  title: string;
  category: string;
  tags: string;
}

function DocumentManager() {
  const { 
    register, 
    handleSubmit, 
    reset,
    setValue,
    watch,
    formState: { errors }
  } = useForm<DocumentUploadData>();

  // Track files with their metadata
  const [files, setFiles] = useState<FileWithPreview[]>([]);
  const [isDragging, setIsDragging] = useState(false);
  const [uploadQueue, setUploadQueue] = useState<string[]>([]);

  const watchedFiles = watch("files");

  // Handle file selection from input
  const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newFiles = Array.from(e.target.files ?? []);
    addFiles(newFiles);
  };

  // Add files with previews
  const addFiles = (newFiles: File[]) => {
    const filesWithPreviews: FileWithPreview[] = newFiles.map(file => ({
      file,
      preview: file.type.startsWith('image/') 
        ? URL.createObjectURL(file) 
        : getFileIcon(file.type),
      progress: 0,
      status: 'pending'
    }));

    setFiles(prev => [...prev, ...filesWithPreviews]);
  };

  // Get icon for non-image files
  const getFileIcon = (type: string) => {
    if (type.includes('pdf')) return '📕';
    if (type.includes('word') || type.includes('document')) return '📘';
    if (type.includes('sheet') || type.includes('excel')) return '📗';
    if (type.includes('presentation') || type.includes('powerpoint')) return '📙';
    return '📄';
  };

  // Remove file from list
  const removeFile = (index: number) => {
    setFiles(prev => {
      const updated = [...prev];
      // Revoke URL to prevent memory leak
      if (updated[index].preview.startsWith('blob:')) {
        URL.revokeObjectURL(updated[index].preview);
      }
      updated.splice(index, 1);
      return updated;
    });
  };

  // Drag and drop handlers
  const handleDragEnter = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    setIsDragging(true);
  }, []);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    setIsDragging(false);
  }, []);

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
  }, []);

  const handleDrop = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    setIsDragging(false);
    
    const droppedFiles = Array.from(e.dataTransfer.files);
    addFiles(droppedFiles);
  }, []);

  // Upload a single file
  const uploadFile = async (fileWithPreview: FileWithPreview, index: number) => {
    const formData = new FormData();
    formData.append('file', fileWithPreview.file);
    formData.append('title', watchedFiles?.[index]?.file.name ?? 'Untitled');

    try {
      // Update status to uploading
      setFiles(prev => prev.map((f, i) => 
        i === index ? { ...f, status: 'uploading' as const } : f
      ));

      await axios.post('/api/documents/upload', formData, {
        onUploadProgress: (progressEvent) => {
          const progress = Math.round(
            (progressEvent.loaded * 100) / (progressEvent.total ?? 1)
          );
          setFiles(prev => prev.map((f, i) => 
            i === index ? { ...f, progress } : f
          ));
        }
      });

      // Mark as complete
      setFiles(prev => prev.map((f, i) => 
        i === index ? { ...f, status: 'complete' as const } : f
      ));
    } catch (error) {
      // Mark as error
      setFiles(prev => prev.map((f, i) => 
        i === index ? { ...f, status: 'error' as const } : f
      ));
    }
  };

  // Upload all pending files
  const uploadAllFiles = async () => {
    const pendingFiles = files.filter(f => f.status === 'pending');
    
    for (const file of pendingFiles) {
      const index = files.findIndex(f => f.file.name === file.file.name);
      await uploadFile(file, index);
    }
  };

  const onSubmit = async (data: DocumentUploadData) => {
    console.log("Form data:", data);
    console.log("Files to upload:", files.length);
    
    // Upload all files
    await uploadAllFiles();
  };

  // Cleanup on unmount
  useEffect(() => {
    return () => {
      files.forEach(f => {
        if (f.preview.startsWith('blob:')) {
          URL.revokeObjectURL(f.preview);
        }
      });
    };
  }, []);

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="document-manager">
      <h1>Document Manager</h1>

      {/* Form Fields */}
      <div className="form-row">
        <div className="field">
          <label htmlFor="title">Document Title</label>
          <input
            id="title"
            {...register("title", { required: "Title is required" })}
            placeholder="Enter document title"
          />
          {errors.title && <span className="error">{errors.title.message}</span>}
        </div>

        <div className="field">
          <label htmlFor="category">Category</label>
          <select
            id="category"
            {...register("category", { required: "Category is required" })}
          >
            <option value="">Select category</option>
            <option value="invoice">Invoice</option>
            <option value="contract">Contract</option>
            <option value="report">Report</option>
            <option value="other">Other</option>
          </select>
          {errors.category && <span className="error">{errors.category.message}</span>}
        </div>
      </div>

      <div className="field">
        <label htmlFor="tags">Tags (comma separated)</label>
        <input
          id="tags"
          {...register("tags")}
          placeholder="invoice, important, q1"
        />
      </div>

      {/* Drop Zone */}
      <div
        className={`drop-zone ${isDragging ? 'dragging' : ''}`}
        onDragEnter={handleDragEnter}
        onDragLeave={handleDragLeave}
        onDragOver={handleDragOver}
        onDrop={handleDrop}
        onClick={() => document.getElementById('file-input')?.click()}
      >
        <input
          id="file-input"
          type="file"
          multiple
          style={{ display: 'none' }}
          onChange={handleFileSelect}
        />
        
        <div className="drop-content">
          <div className="drop-icon">📁</div>
          <p>Drag files here or click to browse</p>
          <p className="hint">Supports PDF, DOC, images (max 50MB each)</p>
        </div>
      </div>

      {/* File List */}
      {files.length > 0 && (
        <div className="file-list">
          <h3>Files ({files.length})</h3>
          
          {files.map((fileWithPreview, index) => (
            <div key={index} className={`file-item ${fileWithPreview.status}`}>
              <span className="file-icon">{fileWithPreview.preview}</span>
              
              <div className="file-info">
                <p className="file-name">{fileWithPreview.file.name}</p>
                <p className="file-meta">
                  {(fileWithPreview.file.size / 1024 / 1024).toFixed(2)} MB
                  {' • '}
                  {fileWithPreview.status === 'pending' && 'Ready to upload'}
                  {fileWithPreview.status === 'uploading' && `${fileWithPreview.progress}%`}
                  {fileWithPreview.status === 'complete' && '✓ Uploaded'}
                  {fileWithPreview.status === 'error' && '✗ Failed'}
                </p>
                
                {fileWithPreview.status === 'uploading' && (
                  <div className="mini-progress">
                    <div 
                      className="mini-progress-fill"
                      style={{ width: `${fileWithPreview.progress}%` }}
                    />
                  </div>
                )}
              </div>

              <button
                type="button"
                className="remove-btn"
                onClick={() => removeFile(index)}
                disabled={fileWithPreview.status === 'uploading'}
              >
                ×
              </button>
            </div>
          ))}
        </div>
      )}

      {/* Actions */}
      <div className="form-actions">
        <button
          type="button"
          className="secondary-btn"
          onClick={() => {
            reset();
            setFiles([]);
          }}
        >
          Clear All
        </button>
        
        <button 
          type="submit" 
          className="primary-btn"
          disabled={files.length === 0}
        >
          Upload {files.length} File{files.length !== 1 ? 's' : ''}
        </button>
      </div>
    </form>
  );
}

export default DocumentManager;
```

## Key Takeaways

- File inputs in React Hook Form return a FileList object, not a string
- Use URL.createObjectURL() for efficient image previews, and always clean up with URL.revokeObjectURL()
- The FileList API is read-only—use DataTransfer to programmatically add or remove files
- Drag-and-drop requires handling dragEnter, dragLeave, dragOver, and drop events
- Set shouldValidate: true when calling setValue to trigger validation after programmatic updates
- Axios provides onUploadProgress for tracking upload progress
- Always validate file types and sizes on both client and server sides
- Multiple file inputs use the multiple attribute and return an array of files

## What's Next

Continue to [Debounced Search Input](/07-forms/03-form-patterns/02-debounced-search-input.md) to learn how to create performant search inputs that avoid excessive API calls through debouncing, and how to handle loading and empty states.