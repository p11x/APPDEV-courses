# Audio and Video

## Topic Title
Embedding Audio and Video in HTML

## Concept Explanation

### What Are Media Elements?

HTML5 introduced native `<audio>` and `<video>` elements that allow embedding media directly without plugins. These elements provide built-in controls for playback.

### Audio Element

```html
<audio controls>
    <source src="audio.mp3" type="audio/mpeg">
    <source src="audio.ogg" type="audio/ogg">
    Your browser doesn't support audio.
</audio>
```

### Video Element

```html
<video controls width="640" height="480">
    <source src="video.mp4" type="video/mp4">
    <source src="video.webm" type="video/webm">
    Your browser doesn't support video.
</video>
```

### Supported Formats

| Format | Type | Browser Support |
|--------|------|-----------------|
| MP3 | Audio | Universal |
| WAV | Audio | Good |
| OGG | Audio | Good |
| MP4 | Video | Universal |
| WebM | Video | Good |
| OGG | Video | Limited |

## Why This Concept Is Important

Media elements matter because:

1. **Native support** - No plugins needed
2. **Accessibility** - Keyboard accessible controls
3. **Mobile support** - Works on all devices
4. **API access** - Programmatic control via JavaScript
5. **Performance** - Better than Flash/plugin solutions

## Step-by-Step Explanation

### Step 1: Basic Audio

```html
<audio src="music.mp3" controls></audio>
```

### Step 2: Audio with Multiple Sources

```html
<audio controls>
    <source src="music.mp3" type="audio/mpeg">
    <source src="music.ogg" type="audio/ogg">
</audio>
```

### Step 3: Basic Video

```html
<video src="movie.mp4" controls></video>
```

### Step 4: Video with Options

```html
<video controls width="800" poster="poster.jpg">
    <source src="movie.mp4" type="video/mp4">
    <track kind="captions" src="captions.vtt" srclang="en">
</video>
```

### Step 5: Fallback Content

```html
<video controls>
    <source src="video.mp4" type="video/mp4">
    <p>
        Your browser doesn't support video.
        <a href="video.mp4">Download the video</a>
    </p>
</video>
```

## Code Examples

### Example 1: Audio Player

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Audio Demo</title>
</head>
<body>
    <h1>Audio Examples</h1>
    
    <!-- Basic audio -->
    <h2>Basic Audio Player</h2>
    <audio controls>
        <source src="song.mp3" type="audio/mpeg">
    </audio>
    
    <!-- Audio with autoplay and loop -->
    <h2>Audio with Autoplay</h2>
    <audio controls autoplay loop>
        <source src="background.mp3" type="audio/mpeg">
    </audio>
    
    <!-- Audio without controls (programmatic) -->
    <h2>Custom Controls</h2>
    <audio id="myAudio">
        <source src="song.mp3" type="audio/mpeg">
    </audio>
    <button onclick="document.getElementById('myAudio').play()">Play</button>
    <button onclick="document.getElementById('myAudio').pause()">Pause</button>
</body>
</html>
```

### Example 2: Video Player

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Video Demo</title>
</head>
<body>
    <h1>Video Examples</h1>
    
    <!-- Basic video -->
    <h2>Basic Video Player</h2>
    <video controls width="640">
        <source src="video.mp4" type="video/mp4">
    </video>
    
    <!-- Video with poster -->
    <h2>Video with Poster Image</h2>
    <video controls width="640" poster="thumbnail.jpg">
        <source src="video.mp4" type="video/mp4">
    </video>
    
    <!-- Video with multiple sources -->
    <h2>Multiple Formats</h2>
    <video controls width="640">
        <source src="video.mp4" type="video/mp4">
        <source src="video.webm" type="video/webm">
    </video>
    
    <!-- Video with subtitles -->
    <h2>Video with Captions</h2>
    <video controls width="640">
        <source src="video.mp4" type="video/mp4">
        <track kind="captions" src="captions_en.vtt" srclang="en" label="English" default>
        <track kind="captions" src="captions_es.vtt" srclang="es" label="Español">
    </video>
</body>
</html>
```

### Example 3: Complete Media Page

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Media Showcase</title>
</head>
<body>
    <h1>Media Showcase</h1>
    
    <article>
        <h2>Tutorial: Building Your First Website</h2>
        
        <figure>
            <video controls width="800">
                <source src="tutorial.mp4" type="video/mp4">
                <track kind="captions" src="tutorial_captions.vtt" srclang="en" default>
                <p>Your browser doesn't support video. <a href="tutorial.mp4">Download</a></p>
            </video>
            <figcaption>Part 1: Introduction to HTML</figcaption>
        </figure>
        
        <h3>Listen to the Audio Version</h3>
        <audio controls>
            <source src="tutorial_audio.mp3" type="audio/mpeg">
        </audio>
        
        <h3>Resources</h3>
        <ul>
            <li><a href="tutorial.mp4">Download Video</a></li>
            <li><a href="tutorial_audio.mp3">Download Audio</a></li>
            <li><a href="slides.pdf">Download Slides</a></li>
        </ul>
    </article>
</body>
</html>
```

### Example 4: Programmatic Control

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Custom Media Controls</title>
    <style>
        .media-container {
            max-width: 800px;
        }
        .custom-controls {
            margin-top: 10px;
        }
        button {
            padding: 8px 16px;
            margin-right: 5px;
        }
    </style>
</head>
<body>
    <h1>Custom Video Controls</h1>
    
    <div class="media-container">
        <video id="myVideo" width="800">
            <source src="video.mp4" type="video/mp4">
        </video>
        
        <div class="custom-controls">
            <button onclick="playVideo()">Play</button>
            <button onclick="pauseVideo()">Pause</button>
            <button onclick="skip(-10)">-10s</button>
            <button onclick="skip(10)">+10s</button>
            <button onclick="toggleMute()">Mute</button>
            <button onclick="setVolume(0.5)">50% Volume</button>
            <button onclick="setVolume(1)">100% Volume</button>
        </div>
        
        <p>Current Time: <span id="timeDisplay">0</span>s</p>
    </div>
    
    <script>
        const video = document.getElementById('myVideo');
        
        function playVideo() {
            video.play();
        }
        
        function pauseVideo() {
            video.pause();
        }
        
        function skip(seconds) {
            video.currentTime += seconds;
        }
        
        function toggleMute() {
            video.muted = !video.muted;
        }
        
        function setVolume(level) {
            video.volume = level;
        }
        
        video.ontimeupdate = function() {
            document.getElementById('timeDisplay').textContent = 
                Math.floor(video.currentTime);
        };
    </script>
</body>
</html>
```

### Example 5: Angular Media Component

```html
<!-- Angular video component -->
<video [src]="videoUrl" 
       [poster]="posterUrl"
       controls
       (play)="onPlay()"
       (pause)="onPause()">
</video>

<!-- With track for subtitles -->
<video [src]="videoUrl" controls>
    <track *ngFor="let caption of captions"
           [kind]="caption.kind"
           [src]="caption.src"
           [srclang]="caption.lang"
           [label]="caption.label">
</video>
```

## Best Practices

### Audio Best Practices

1. **Provide multiple formats** - Ensure browser compatibility
2. **Use controls attribute** - Default browser controls
3. **Consider autoplay policies** - Browsers block autoplay
4. **Add fallback** - Message for unsupported browsers

### Video Best Practices

1. **Specify dimensions** - Prevents layout shift
2. **Use poster image** - Shows before video loads
3. **Compress videos** - Optimize for web
4. **Add captions** - Accessibility requirement
5. **Provide download** - Fallback for unsupported browsers

### Accessibility Best Practices

1. **Always add captions** - For deaf/hard-of-hearing users
2. **Controls keyboard accessible** - Native controls work
3. **Don't auto-play with sound** - Bad user experience
4. **Provide transcripts** - Alternative to audio content

## Real-World Examples

### Example 1: Online Course Platform

```html
<article class="lesson">
    <h2>Lesson 5: CSS Layouts</h2>
    
    <video controls class="lesson-video">
        <source src="lesson5.mp4" type="video/mp4">
        <track kind="captions" src="lesson5_en.vtt" srclang="en" default>
    </video>
    
    <div class="lesson-resources">
        <h3>Download Resources</h3>
        <ul>
            <li><a href="lesson5.zip">Exercise Files</a></li>
            <li><a href="lesson5_transcript.pdf">Transcript</a></li>
        </ul>
    </div>
</article>
```

### Example 2: Background Video with Overlay

```html
<div class="hero">
    <video autoplay muted loop playsinline class="bg-video">
        <source src="hero.mp4" type="video/mp4">
    </video>
    <div class="overlay">
        <h1>Welcome to Our Site</h1>
        <p>Discover amazing content</p>
    </div>
</div>
```

## Common Mistakes Students Make

### Mistake 1: Missing Source Files

```html
<!-- Wrong - file doesn't exist -->
<audio src="nonexistent.mp3" controls>

<!-- Correct - verify file exists -->
<audio src="my-audio.mp3" controls>
```

### Mistake 2: Wrong MIME Types

```html
<!-- Wrong - incorrect type -->
<source src="video.mp4" type="video/mpeg">

<!-- Correct - correct type -->
<source src="video.mp4" type="video/mp4">
```

### Mistake 3: No Fallback

```html
<!-- Wrong - no fallback -->
<video controls>
    <source src="video.mp4">
</video>

<!-- Correct - with fallback -->
<video controls>
    <source src="video.mp4">
    <p>Your browser doesn't support video.</p>
</video>
```

### Mistake 4: Auto-play Without Mute

```html
<!-- Wrong - will be blocked -->
<video autoplay>
    <source src="video.mp4">
</video>

<!-- Correct - muted autoplay works -->
<video autoplay muted>
    <source src="video.mp4">
</video>
```

## Exercises

### Exercise 1: Add Audio
Add an audio element with controls to a page.

### Exercise 2: Add Video
Add a video element with poster image.

### Exercise 3: Multiple Sources
Provide multiple formats for a video.

### Exercise 4: Custom Controls
Create custom play/pause buttons.

## Mini Practice Tasks

### Task 1: Basic Audio
Add an audio file to a page.

### Task 2: Basic Video
Add a video to a page.

### Task 3: Video Size
Set video width and height.

### Task 4: Captions
Add captions to a video.

### Task 5: Fallback
Add fallback content for unsupported browsers.
