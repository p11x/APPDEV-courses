# 🎵 Project 11: Music Streaming App

## 📋 Project Overview

Build a music streaming application with playlist management, shuffle, repeat modes, and audio visualization. This project demonstrates:
- Web Audio API
- Audio visualization
- Playlist management
- Playback controls

---

## 🎯 Core Features

### Audio Player

```javascript
class MusicPlayer {
    constructor() {
        this.audio = new Audio();
        this.playlist = [];
        this.currentIndex = 0;
        this.isShuffle = false;
        this.repeatMode = 'none'; // none, one, all
        this.initAudioContext();
    }
    
    initAudioContext() {
        this.audioContext = new (window.AudioContext || window.webkitAudioContext)();
        this.analyser = this.audioContext.createAnalyser();
        this.source = this.audioContext.createMediaElementSource(this.audio);
        this.source.connect(this.analyser);
        this.analyser.connect(this.audioContext.destination);
        this.analyser.fftSize = 256;
    }
    
    getFrequencyData() {
        const bufferLength = this.analyser.frequencyBinCount;
        const dataArray = new Uint8Array(bufferLength);
        this.analyser.getByteFrequencyData(dataArray);
        return dataArray;
    }
    
    playTrack(index) {
        if (index >= 0 && index < this.playlist.length) {
            this.currentIndex = index;
            this.audio.src = this.playlist[index].src;
            this.audio.play();
        }
    }
    
    toggleShuffle() {
        this.isShuffle = !this.isShuffle;
    }
    
    toggleRepeat() {
        const modes = ['none', 'all', 'one'];
        const currentModeIndex = modes.indexOf(this.repeatMode);
        this.repeatMode = modes[(currentModeIndex + 1) % modes.length];
    }
    
    next() {
        if (this.isShuffle) {
            this.currentIndex = Math.floor(Math.random() * this.playlist.length);
        } else {
            this.currentIndex = (this.currentIndex + 1) % this.playlist.length;
        }
        this.playTrack(this.currentIndex);
    }
}
```

---

## 🔗 Related Topics

- [10_Event_Loop_Deep_Dive.md](../08_ASYNC_JAVASCRIPT/10_Event_Loop_Deep_Dive.md)
- [11_DOM_Performance_Optimization.md](../09_DOM_MANIPULATION/11_DOM_Performance_Optimization.md)

---

**Next: [Image Gallery](./12_Project_12_Image_Gallery.md)**