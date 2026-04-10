# 🎵 Web Audio API Complete Guide

## Audio Processing in the Browser

---

## Table of Contents

1. [Audio Context](#audio-context)
2. [Audio Nodes](#audio-nodes)
3. [Playing Audio](#playing-audio)
4. [Audio Effects](#audio-effects)
5. [Analysis](#analysis)
6. [Recording](#recording)

---

## Audio Context

### Creating Context

```javascript
const audioContext = new (window.AudioContext || window.webkitAudioContext)();
```

### Resume Context

```javascript
async function initAudio() {
  if (audioContext.state === 'suspended') {
    await audioContext.resume();
  }
}
```

---

## Audio Nodes

### Source Nodes

```javascript
// Audio buffer source
const bufferSource = audioContext.createBufferSource();
bufferSource.buffer = audioBuffer;
bufferSource.connect(audioContext.destination);

// Oscillator
const oscillator = audioContext.createOscillator();
oscillator.type = 'sine';
oscillator.frequency.value = 440;
oscillator.connect(audioContext.destination);
oscillator.start();
```

---

## Playing Audio

### Loading and Playing

```javascript
async function playAudio(url) {
  const response = await fetch(url);
  const arrayBuffer = await response.arrayBuffer();
  const audioBuffer = await audioContext.decodeAudioData(arrayBuffer);
  
  const source = audioContext.createBufferSource();
  source.buffer = audioBuffer;
  source.connect(audioContext.destination);
  source.start();
}
```

---

## Audio Effects

### Gain Node

```javascript
const gainNode = audioContext.createGain();
gainNode.gain.value = 0.5;

source.connect(gainNode);
gainNode.connect(audioContext.destination);
```

### Filter Node

```javascript
const filter = audioContext.createBiquadFilter();
filter.type = 'lowpass';
filter.frequency.value = 1000;

source.connect(filter);
filter.connect(audioContext.destination);
```

---

## Analysis

### Analyser Node

```javascript
const analyser = audioContext.createAnalyser();
analyser.fftSize = 2048;

source.connect(analyser);
analyser.connect(audioContext.destination);

const dataArray = new Uint8Array(analyser.frequencyBinCount);

function analyze() {
  analyser.getByteFrequencyData(dataArray);
  console.log(dataArray);
  requestAnimationFrame(analyze);
}
```

---

## Recording

### MediaRecorder

```javascript
const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
const mediaRecorder = new MediaRecorder(stream);

mediaRecorder.ondataavailable = (e) => {
  chunks.push(e.data);
};

mediaRecorder.onstop = () => {
  const blob = new Blob(chunks, { type: 'audio/webm' });
};

mediaRecorder.start();
```

---

## Summary

### Key Takeaways

1. **Context**: Audio processing
2. **Nodes**: Source, effect, destination
3. **Effects**: Gain, filter
4. **Analysis**: Frequency data
5. **Recording**: MediaRecorder API

### Next Steps

- Continue with: [04_WEB_STORAGE_MASTER.md](04_WEB_STORAGE_MASTER.md)
- Study Web Audio examples
- Implement audio apps

---

## Cross-References

- **Previous**: [02_WEB_ASSEMBLY_INTEGRATION.md](02_WEB_ASSEMBLY_INTEGRATION.md)
- **Next**: [04_WEB_STORAGE_MASTER.md](04_WEB_STORAGE_MASTER.md)

---

*Last updated: 2024*