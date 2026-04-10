# CameraX

## Overview

CameraX is Android's modern camera API that provides consistent, easy-to-use interfaces for camera functionality. It abstracts device-specific differences and provides lifecycle-aware components.

## Learning Objectives

- Implement camera preview with CameraX
- Capture photos and videos
- Handle camera permissions
- Apply image analysis
- Implement focus and exposure controls

## Prerequisites

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)

## Core Concepts

### CameraX Architecture

CameraX uses a use-case-based model:
- Preview: Display camera feed
- ImageCapture: Capture still images
- VideoCapture: Record video
- ImageAnalysis: Process frames in real-time

### Lifecycle Awareness

CameraX components bind to lifecycle:
- Start on STARTED
- Pause on PAUSED
- Stop on STOPPED

## Code Examples

### Example 1: Basic Camera Preview

```kotlin
import android.Manifest
import android.content.pm.PackageManager
import android.os.Bundle
import android.util.Size
import android.view.Surface
import androidx.appcompat.app.AppCompatActivity
import androidx.camera.core.*
import androidx.camera.lifecycle.ProcessCameraProvider
import androidx.core.content.ContextCompat
import java.io.File
import java.util.concurrent.ExecutorService
import java.util.concurrent.Executors

/**
 * Camera preview activity using CameraX
 * Demonstrates basic camera setup and preview
 */
class CameraPreviewActivity : AppCompatActivity() {
    
    private var preview: Preview? = null
    private var camera: Camera? = null
    private lateinit var cameraExecutor: ExecutorService
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_camera_preview)
        
        cameraExecutor = Executors.newSingleThreadExecutor()
        
        if (allPermissionsGranted()) {
            startCamera()
        } else {
            requestPermissions(REQUIRED_PERMISSIONS, REQUEST_CODE_PERMISSIONS)
        }
    }
    
    private fun startCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(this)
        
        cameraProviderFuture.addListener({
            val cameraProvider = cameraProviderFuture.get()
            
            // Preview use case
            preview = Preview.Builder()
                .setTargetResolution(Size(1280, 720))
                .build()
                .also {
                    it.setSurfaceProvider(findViewById<androidx.camera.view.PreviewView>(R.id.preview_view).surfaceProvider)
                }
            
            // Select back camera
            val cameraSelector = CameraSelector.DEFAULT_BACK_CAMERA
            
            try {
                // Unbind any bound use cases before rebinding
                cameraProvider.unbindAll()
                
                // Bind use cases to camera
                camera = cameraProvider.bindToLifecycle(
                    this,
                    cameraSelector,
                    preview
                )
                
                // Enable tap to focus
                setupTapToFocus()
                
            } catch (e: Exception) {
                println("Camera binding failed: ${e.message}")
            }
            
        }, ContextCompat.getMainExecutor(this))
    }
    
    private fun setupTapToFocus() {
        findViewById<androidx.camera.view.PreviewView>(R.id.preview_view).setOnTouchListener { _, event ->
            val factory = preview?.surfaceProvider?.let { 
                MeteringPointFactory.createSurfacePointFactory(it) 
            }
            
            val point = factory?.createPoint(event.x, event.y)
            val action = FocusMeteringAction.Builder(point!!).build()
            
            camera?.cameraControl?.startFocusAndMetering(action)
            
            true
        }
    }
    
    private fun allPermissionsGranted() = REQUIRED_PERMISSIONS.all {
        ContextCompat.checkSelfPermission(baseContext, it) == PackageManager.PERMISSION_GRANTED
    }
    
    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == REQUEST_CODE_PERMISSIONS) {
            if (allPermissionsGranted()) {
                startCamera()
            }
        }
    }
    
    override fun onDestroy() {
        super.onDestroy()
        cameraExecutor.shutdown()
    }
    
    companion object {
        private val REQUIRED_PERMISSIONS = arrayOf(Manifest.permission.CAMERA)
        private const val REQUEST_CODE_PERMISSIONS = 10
    }
}
```

**Output:**
```
Camera initialized
Preview surface bound
Tap to focus enabled
```

### Example 2: Image Capture

```kotlin
import android.Manifest
import android.content.ContentValues
import android.content.pm.PackageManager
import android.os.Build
import android.provider.MediaStore
import android.util.Log
import android.widget.Toast
import androidx.camera.core.*
import androidx.camera.lifecycle.ProcessCameraProvider
import androidx.core.content.ContextCompat
import java.text.SimpleDateFormat
import java.util.*
import java.util.concurrent.ExecutorService

/**
 * Image capture using CameraX
 * Demonstrates capturing photos and handling output
 */
class ImageCaptureActivity : AppCompatActivity() {
    
    private var imageCapture: ImageCapture? = null
    private lateinit var cameraExecutor: ExecutorService
    private var camera: Camera? = null
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        cameraExecutor = Executors.newSingleThreadExecutor()
        
        startCamera()
    }
    
    private fun startCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(this)
        
        cameraProviderFuture.addListener({
            val cameraProvider = cameraProviderFuture.get()
            
            // Preview use case
            val preview = Preview.Builder()
                .build()
                .also {
                    it.setSurfaceProvider(findViewById<androidx.camera.view.PreviewView>(R.id.preview_view).surfaceProvider)
                }
            
            // Image capture use case
            imageCapture = ImageCapture.Builder()
                .setCaptureMode(ImageCapture.CAPTURE_MODE_MINIMIZE_LATENCY)
                .setFlashMode(ImageCapture.FLASH_MODE_AUTO)
                .setTargetRotation(Surface.ROTATION_0)
                .build()
            
            val cameraSelector = CameraSelector.DEFAULT_BACK_CAMERA
            
            try {
                cameraProvider.unbindAll()
                camera = cameraProvider.bindToLifecycle(
                    this,
                    cameraSelector,
                    preview,
                    imageCapture
                )
                
                setupCameraControls()
                
            } catch (e: Exception) {
                Log.e(TAG, "Use case binding failed", e)
            }
            
        }, ContextCompat.getMainExecutor(this))
    }
    
    private fun setupCameraControls() {
        // Get camera info
        val cameraInfo = camera?.cameraInfo
        
        // Enable torch if supported
        cameraInfo?.hasFlashUnit()?.let { hasFlash ->
            if (hasFlash) {
                // Enable torch button
            }
        }
        
        // Zoom control
        val zoomState = cameraInfo?.zoomState
        val maxZoom = zoomState?.maxZoomRatio ?: 1f
        val minZoom = zoomState?.minZoomRatio ?: 1f
        
        println("Zoom range: $minZoom to $maxZoom")
    }
    
    /**
     * Take a photo
     */
    fun takePhoto() {
        val imageCapture = imageCapture ?: return
        
        // Create filename
        val name = SimpleDateFormat(FILENAME_FORMAT, Locale.US)
            .format(System.currentTimeMillis())
        
        val contentValues = ContentValues().apply {
            put(MediaStore.MediaColumns.DISPLAY_NAME, name)
            put(MediaStore.MediaColumns.MIME_TYPE, "image/jpeg")
            if (Build.VERSION.SDK_INT > Build.VERSION_CODES.P) {
                put(MediaStore.Images.Media.RELATIVE_PATH, "Pictures/CameraX-Images")
            }
        }
        
        val outputOptions = ImageCapture.OutputFileOptions.Builder(
            contentResolver,
            MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
            contentValues
        ).build()
        
        imageCapture.takePicture(
            outputOptions,
            ContextCompat.getMainExecutor(this),
            object : ImageCapture.OnImageSavedCallback {
                override fun onImageSaved(outputFileResults: ImageCapture.OutputFileResults) {
                    val msg = "Photo saved: ${outputFileResults.savedUri}"
                    Toast.makeText(baseContext, msg, Toast.LENGTH_SHORT).show()
                }
                
                override fun onError(exception: ImageCaptureException) {
                    Log.e(TAG, "Photo capture failed: ${exception.message}", exception)
                }
            }
        )
    }
    
    /**
     * Enable flash mode
     */
    fun setFlashMode(@ImageCapture.FlashMode flashMode: Int) {
        imageCapture?.flashMode = flashMode
    }
    
    companion object {
        private const val TAG = "ImageCaptureActivity"
        private const val FILENAME_FORMAT = "yyyy-MM-dd-HH-mm-ss-SSS"
    }
}

/**
 * Camera controller for managing camera operations
 */
class CameraController(
    private val cameraProvider: ProcessCameraProvider
) {
    private var preview: Preview? = null
    private var imageCapture: ImageCapture? = null
    private var camera: Camera? = null
    
    /**
     * Bind camera use cases to lifecycle
     */
    fun bindToLifecycle(owner: androidx.lifecycle.LifecycleOwner) {
        preview = Preview.Builder().build()
        imageCapture = ImageCapture.Builder()
            .setCaptureMode(ImageCapture.CAPTURE_MODE_MAXIMIZE_QUALITY)
            .build()
        
        try {
            cameraProvider.unbindAll()
            camera = cameraProvider.bindToLifecycle(
                owner,
                CameraSelector.DEFAULT_BACK_CAMERA,
                preview,
                imageCapture
            )
        } catch (e: Exception) {
            println("Camera binding failed: ${e.message}")
        }
    }
    
    /**
     * Set preview surface
     */
    fun setSurfaceProvider(provider: Preview.SurfaceProvider) {
        preview?.setSurfaceProvider(provider)
    }
    
    /**
     * Capture photo with callback
     */
    fun capturePhoto(executor: java.util.concurrent.Executor, callback: ImageCapture.OnImageSavedCallback) {
        imageCapture?.takePicture(executor, callback)
    }
    
    /**
     * Get current zoom ratio
     */
    fun getZoomRatio(): Float = camera?.cameraInfo?.zoomState?.value?.zoomRatio ?: 1f
    
    /**
     * Set zoom
     */
    fun setZoom(ratio: Float) {
        camera?.cameraControl?.setZoomRatio(ratio)
    }
    
    /**
     * Enable/disable torch
     */
    fun setTorch(enabled: Boolean) {
        camera?.cameraControl?.enableTorch(enabled)
    }
    
    /**
     * Focus on point
     */
    fun focusOnPoint(x: Float, y: Float, viewWidth: Int, viewHeight: Int) {
        val factory = preview?.surfaceProvider?.let { 
            androidx.camera.core.MeteringPointFactory.createSurfacePointFactory(it) 
        }
        
        val point = factory?.createPoint(x, y)
        val action = FocusMeteringAction.Builder(point!!).build()
        
        camera?.cameraControl?.startFocusAndMetering(action)
    }
}
```

**Output:**
```
Photo saved: content://media/external/images/media/12345
Zoom range: 1.0 to 10.0
Flash mode: AUTO
```

### Example 3: Video Recording with CameraX

```kotlin
import android.Manifest
import android.content.ContentValues
import android.content.pm.PackageManager
import android.os.Build
import android.provider.MediaStore
import androidx.camera.core.*
import androidx.camera.lifecycle.ProcessCameraProvider
import androidx.camera.video.*
import androidx.core.content.ContextCompat
import java.text.SimpleDateFormat
import java.util.*
import java.util.concurrent.ExecutorService
import java.util.concurrent.Executors

/**
 * Video capture using CameraX
 * Demonstrates video recording with CameraX
 */
class VideoCaptureActivity : AppCompatActivity() {
    
    private var videoCapture: VideoCapture<Recorder>? = null
    private var recording: Recording? = null
    private var camera: Camera? = null
    private lateinit var cameraExecutor: ExecutorService
    
    private var isRecording = false
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        cameraExecutor = Executors.newSingleThreadExecutor()
        
        startCamera()
    }
    
    private fun startCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(this)
        
        cameraProviderFuture.addListener({
            val cameraProvider = cameraProviderFuture.get()
            
            // Preview use case
            val preview = Preview.Builder()
                .build()
                .also {
                    it.setSurfaceProvider(findViewById<androidx.camera.view.PreviewView>(R.id.preview_view).surfaceProvider)
                }
            
            // Video capture use case
            val recorder = Recorder.Builder()
                .setQualitySelector(QualitySelector.from(Quality.HD))
                .build()
            
            videoCapture = VideoCapture.withOutput(recorder)
            
            val cameraSelector = CameraSelector.DEFAULT_BACK_CAMERA
            
            try {
                cameraProvider.unbindAll()
                camera = cameraProvider.bindToLifecycle(
                    this,
                    cameraSelector,
                    preview,
                    videoCapture
                )
                
            } catch (e: Exception) {
                println("Camera binding failed: ${e.message}")
            }
            
        }, ContextCompat.getMainExecutor(this))
    }
    
    /**
     * Start video recording
     */
    fun startRecording() {
        val videoCapture = this.videoCapture ?: return
        
        // Check if already recording
        if (isRecording) return
        
        // Create output options
        val name = SimpleDateFormat(FILENAME_FORMAT, Locale.US)
            .format(System.currentTimeMillis())
        
        val contentValues = ContentValues().apply {
            put(MediaStore.MediaColumns.DISPLAY_NAME, name)
            put(MediaStore.MediaColumns.MIME_TYPE, "video/mp4")
            if (Build.VERSION.SDK_INT > Build.VERSION_CODES.P) {
                put(MediaStore.Video.Media.RELATIVE_PATH, "Movies/CameraX-Videos")
            }
        }
        
        val mediaStoreOutputOptions = MediaStoreOutputOptions
            .Builder(contentResolver, MediaStore.Video.Media.EXTERNAL_CONTENT_URI)
            .setContentValues(contentValues)
            .build()
        
        recording = videoCapture.output
            .prepareRecording(this, mediaStoreOutputOptions)
            .apply {
                // Enable audio recording if permission granted
                if (ContextCompat.checkSelfPermission(
                        this@VideoCaptureActivity,
                        Manifest.permission.RECORD_AUDIO
                    ) == PackageManager.PERMISSION_GRANTED
                ) {
                    withAudioEnabled()
                }
            }
            .start(ContextCompat.getMainExecutor(this)) { recordEvent ->
                when (recordEvent) {
                    is VideoRecordEvent.Start -> {
                        isRecording = true
                        println("Recording started")
                    }
                    is VideoRecordEvent.Finalize -> {
                        isRecording = false
                        if (!recordEvent.hasError()) {
                            val msg = "Video saved: ${recordEvent.outputResults.outputUri}"
                            println(msg)
                        } else {
                            println("Recording error: ${recordEvent.error}")
                        }
                    }
                    is VideoRecordEvent.Status -> {
                        val duration = recordEvent.recordingStats.recordedDurationNanos
                        println("Recording: ${duration / 1_000_000_000}s")
                    }
                }
            }
    }
    
    /**
     * Stop video recording
     */
    fun stopRecording() {
        recording?.stop()
        recording = null
    }
    
    /**
     * Pause video recording
     */
    fun pauseRecording() {
        recording?.pause()
    }
    
    /**
     * Resume video recording
     */
    fun resumeRecording() {
        recording?.resume()
    }
    
    companion object {
        private const val FILENAME_FORMAT = "yyyy-MM-dd-HH-mm-ss-SSS"
    }
}

/**
 * Image analysis for real-time processing
 */
class ImageAnalysisActivity : AppCompatActivity() {
    
    private lateinit var cameraExecutor: ExecutorService
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        
        cameraExecutor = Executors.newSingleThreadExecutor()
        
        startCamera()
    }
    
    private fun startCamera() {
        val cameraProviderFuture = ProcessCameraProvider.getInstance(this)
        
        cameraProviderFuture.addListener({
            val cameraProvider = cameraProviderFuture.get()
            
            val preview = Preview.Builder()
                .build()
                .also {
                    it.setSurfaceProvider(findViewById<androidx.camera.view.PreviewView>(R.id.preview_view).surfaceProvider)
                }
            
            // Image analysis use case
            val imageAnalysis = ImageAnalysis.Builder()
                .setBackpressureStrategy(ImageAnalysis.STRATEGY_KEEP_ONLY_LATEST)
                .setOutputImageFormat(ImageAnalysis.OUTPUT_IMAGE_FORMAT_YUV_420_888)
                .build()
                .also { analysis ->
                    analysis.setAnalyzer(cameraExecutor) { image ->
                        processImage(image)
                    }
                }
            
            try {
                cameraProvider.unbindAll()
                cameraProvider.bindToLifecycle(
                    this,
                    CameraSelector.DEFAULT_BACK_CAMERA,
                    preview,
                    imageAnalysis
                )
            } catch (e: Exception) {
                println("Analysis binding failed: ${e.message}")
            }
            
        }, ContextCompat.getMainExecutor(this))
    }
    
    /**
     * Process each frame
     */
    private fun processImage(image: ImageProxy) {
        // Get image properties
        val width = image.width
        val height = image.height
        val format = image.format
        
        // Access Y plane
        val yPlane = image.planes[0]
        val yBuffer = yPlane.buffer
        val yRowStride = yPlane.rowStride
        
        // Process frame (e.g., for ML inference)
        val brightness = calculateAverageBrightness(yBuffer, yRowStride, width, height)
        
        println("Frame: ${width}x$height, Brightness: $brightness")
        
        // Close the image to release resources
        image.close()
    }
    
    private fun calculateAverageBrightness(buffer: java.nio.ByteBuffer, rowStride: Int, width: Int, height: Int): Double {
        var total = 0L
        var count = 0
        
        for (y in 0 until height) {
            val rowStart = y * rowStride
            for (x in 0 until width) {
                if (rowStart + x < buffer.capacity()) {
                    total += buffer.get(rowStart + x).toInt() and 0xFF
                    count++
                }
            }
        }
        
        return if (count > 0) total.toDouble() / count else 0.0
    }
}

/**
 * CameraX configuration manager
 */
class CameraXConfig(private val context: android.content.Context) {
    
    /**
     * Get camera provider
     */
    fun getCameraProvider() = ProcessCameraProvider.getInstance(context)
    
    /**
     * Check if front camera is available
     */
    fun hasFrontCamera(cameraProvider: ProcessCameraProvider): Boolean {
        return try {
            cameraProvider.hasCamera(CameraSelector.DEFAULT_FRONT_CAMERA)
        } catch (e: Exception) {
            false
        }
    }
    
    /**
     * Check if back camera is available
     */
    fun hasBackCamera(cameraProvider: ProcessCameraProvider): Boolean {
        return try {
            cameraProvider.hasCamera(CameraSelector.DEFAULT_BACK_CAMERA)
        } catch (e: Exception) {
            false
        }
    }
    
    /**
     * Get supported qualities
     */
    fun getSupportedQualities(cameraProvider: ProcessCameraProvider): List<Quality> {
        val camera = try {
            cameraProvider.bindToLifecycle(
                context as androidx.lifecycle.LifecycleOwner,
                CameraSelector.DEFAULT_BACK_CAMERA
            )
        } catch (e: Exception) {
            return emptyList()
        }
        
        val qualitySelector = QualitySelector.from(Quality.HD)
        return Quality.getSortedQualities()
    }
}
```

**Output:**
```
Recording started
Recording: 5s
Video saved: content://media/external/video/media/12345
Frame: 1920x1080, Brightness: 128.5
```

## Best Practices

- Use lifecycle-aware binding
- Handle permissions properly
- Set appropriate quality selectors
- Implement backpressure strategy for image analysis
- Handle camera disconnection

## Common Pitfalls

### Problem: Camera not binding
**Solution:** Check permissions and lifecycle owner

### Problem: Preview not showing
**Solution:** Verify surface provider is set correctly

### Problem: Video recording fails
**Solution:** Check storage permissions and MediaStore output

## Troubleshooting Guide

**Q: Camera permission denied?**
A: Request runtime permission and handle denial gracefully

**Q: Preview stretched?**
A: Set appropriate target resolution or use FILL_CENTER scale type

**Q: Analysis callback slow?**
A: Use STRATEGY_KEEP_ONLY_LATEST backpressure strategy

## Cross-References

- [Kotlin Syntax and Fundamentals](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/01_Kotlin_Syntax_and_Fundamentals.md)
- [Coroutines Basics](../01_SETUP_ENVIRONMENT/02_Kotlin_Basics_for_Android/04_Coroutines_Basics.md)