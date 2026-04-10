/**
 * Category: 03_PRACTICAL Subcategory: 01_DEVELOPMENT_TOOLS Concept: 10_Performance_Profiling Topic: 10_Core_Web_Vitals Purpose: Core Web Vitals definitions Difficulty: beginner UseCase: web, frontend Version: TS 5.0+ Compatibility: Browsers Performance: low Security: N/A */

declare namespace CoreWebVitals {
  interface WebVitalsMetric {
    id: string;
    name: 'CLS' | 'FCP' | 'FID' | 'LCP' | 'TTFB' | 'INP';
    value: number;
    delta: number;
    rating: 'good' | 'needs-improvement' | 'poor';
  }

  interface CLSMetric extends WebVitalsMetric {
    name: 'CLS';
  }

  interface FCPMetric extends WebVitalsMetric {
    name: 'FCP';
  }

  interface FIDMetric extends WebVitalsMetric {
    name: 'FID';
  }

  interface LCPMetric extends WebVitalsMetric {
    name: 'LCP';
  }

  interface TTFBMetric extends WebVitalsMetric {
    name: 'TTFB';
  }

  interface INPMetric extends WebVitalsMetric {
    name: 'INP';
    entries: PerformanceEventTiming[];
  }

  interface PerformanceEventTiming extends PerformanceEntry {
    processingStart: number;
    duration: number;
    interactionId?: number;
  }

  interface PerformanceEntry {
    name: string;
    entryType: string;
    startTime: number;
    duration: number;
  }
}

console.log('\n=== Core Web Vitals Complete ===');
console.log('Next: 11_Performance_Budgets.ts');