# 🎬 Project 10: Video Player

## 📋 Project Overview

Build a custom video player with playback controls, progress tracking, volume control, fullscreen mode, and playlist management. This project demonstrates:
- HTML5 Video API
- Custom controls overlay
- Playlist management
- Keyboard shortcuts

---

## 🎯 Core Features

### Video Player

```javascript
class VideoPlayer {
    constructor(videoElement) {
        this.video = videoElement;
        this.playlist = [];
        this.currentIndex = 0;
        this.initControls();
    }
    
    initControls() {
        this.video.addEventListener('timeupdate', () => this.updateProgress());
        this.video.addEventListener('ended', () => this.playNext());
    }
    
    play() { this.video.play(); }
    pause() { this.video.pause(); }
    togglePlay() { this.video.paused ? this.play() : this.pause(); }
    
    seek(time) { this.video.currentTime = time; }
    seekPercent(percent) { this.video.currentTime = (percent / 100) * this.video.duration; }
    
    setVolume(level) { this.video.volume = level; }
    toggleMute() { this.video.muted = !this.video.muted; }
    
    toggleFullscreen() {
        if (document.fullscreenElement) {
            document.exitFullscreen();
        } else {
            this.video.requestFullscreen();
        }
    }
    
    playNext() {
        if (this.currentIndex < this.playlist.length - 1) {
            this.currentIndex++;
            this.loadVideo(this.playlist[this.currentIndex]);
        }
    }
    
    playPrevious() {
        if (this.currentIndex > 0) {
            this.currentIndex--;
            this.loadVideo(this.playlist[this.currentIndex]);
        }
    }
    
    loadVideo(src) {
        this.video.src = src;
        this.play();
    }
    
    updateProgress() {
        const percent = (this.video.currentTime / this.video.duration) * 100;
        // Update progress bar
    }
}
```

---

## 🔗 Related Topics

- [06_Event_Handling_Deep_Dive.md](../09_DOM_MANIPULATION/06_Event_Handling_Deep_Dive.md)
- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)

---

**Next: [Music Streaming](./11_Project_11_Music_Streaming.md)**