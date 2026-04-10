# 📚 String Internationalization

## 📋 Overview

Internationalization (i18n) and localization (l10n) are essential for building applications that serve global audiences. JavaScript provides robust APIs for handling Unicode, locale-specific formatting, and text processing across different languages and regions.

This comprehensive guide covers Unicode fundamentals, the Intl API for formatting and localization, text encoding considerations, and practical implementation patterns for internationalized applications. Understanding these concepts is critical for modern web development where applications must work seamlessly across different languages, scripts, and cultural conventions.

---

## 🔤 Table of Contents

1. [Unicode Fundamentals](#unicode-fundamentals)
2. [JavaScript Unicode Handling](#javascript-unicode-handling)
3. [The Intl API Overview](#the-intl-api-overview)
4. [Locale-Specific Formatting](#locale-specific-formatting)
5. [String Comparison and Sorting](#string-comparison-and-sorting)
6. [Text Encoding](#text-encoding)
7. [Practical Internationalization Patterns](#practical-internationalization-patterns)
8. [Key Takeaways](#key-takeaways)
9. [Common Pitfalls](#common-pitfalls)

---

## 🔠 Unicode Fundamentals

Unicode is a universal character encoding standard that assigns a unique code point to every character from every writing system in the world. Understanding Unicode is fundamental to working with internationalized strings.

### Code Points and Code Units

- **Code Point**: A unique number assigned to each character (e.g., 'A' = U+0041, '你' = U+4F60)
- **Code Unit**: The basic unit used for encoding (UTF-16 uses 16-bit units)

**Code Example 1: Understanding Unicode Code Points**

```javascript
// file: examples/unicode-basics.js
// Description: Unicode code points in JavaScript

// Using codePointAt() to get code point value
const char = "A";
console.log(char.codePointAt(0));      // 65
console.log(char.codePointAt(0).toString(16)); // "41" (hex)

// Using fromCodePoint() to convert back
console.log(String.fromCodePoint(65)); // "A"

// Unicode escape sequences
console.log("\u0041");  // "A"
console.log("\u{1F600}"); // "😀" (emoji with surrogate pair)

// Characters from different scripts
console.log("Hello".codePointAt(0));   // 72 (Latin)
console.log("Привет".codePointAt(0)); // 1055 (Cyrillic)
console.log("你好".codePointAt(0));     // 20320 (Chinese)

// Combining characters
const accented = "é";
console.log(accented.codePointAt(0)); // 233 (single code point)
console.log(accented.length);         // 1

// Decomposed form (e + combining accent)
const decomposed = "e\u0301";
console.log(decomposed.codePointAt(0)); // 101 (e)
console.log(decomposed.length);          // 2 (two code units)
```

### Grapheme Clusters

Grapheme clusters are what users perceive as a single character, including combining characters and emoji.

**Code Example 2: Working with Grapheme Clusters**

```javascript
// file: examples/grapheme-clusters.js
// Description: Handling grapheme clusters correctly

// Count visible characters (grapheme clusters) rather than code units
function countGraphemes(str) {
    if (typeof Intl !== "undefined" && Intl.Segmenter) {
        const segmenter = new Intl.Segmenter(undefined, { granularity: "grapheme" });
        return [...segmenter.segment(str)].length;
    }
    
    // Fallback: simplistic approach
    return [...str].length;
}

const text = "é"; // Single grapheme
console.log(text.length);           // 1 (code unit)
console.log(countGraphemes(text));   // 1

const decomposed = "e\u0301"; // Two code units, one grapheme
console.log(decomposed.length);          // 2
console.log(countGraphemes(decomposed)); // 1

// Emoji with skin tone modifiers
const emoji = "👋🏾"; // Waving hand + dark skin tone
console.log(emoji.length);         // 4 (two surrogate pairs)
console.log(countGraphemes(emoji)); // 1 (one visual character)

// Family emoji (multiple emoji combined)
const family = "👨‍👩‍👧‍👦"; // Man, woman, girl, boy
console.log(family.length);         // 8 (four ZWJ sequences)
console.log(countGraphemes(family)); // 1 (one family unit)

// Iterate over grapheme clusters
function iterateGraphemes(str) {
    const graphemes = [];
    for (const segment of Intl.Segmenter.prototype.segment.call({} , str)) {
        graphemes.push(segment.segment);
    }
    return graphemes;
}

console.log(iterateGraphemes("abc"));       // ["a", "b", "c"]
console.log(iterateGraphemes("é"));          // ["é"]
console.log(iterateGraphemes("👨‍👩‍👧‍👦"));      // ["👨‍👩‍👧‍👦"]
```

---

## 📚 JavaScript Unicode Handling

Modern JavaScript (ES6+) provides robust tools for Unicode handling, including proper emoji support and normalization.

### String Normalization

Unicode normalization converts different representations of the same text into a standard form.

**Code Example 3: Unicode Normalization**

```javascript
// file: examples/unicode-normalization.js
// Description: Using Unicode normalization

// NFC: Canonical Decomposition, followed by Canonical Composition
const composed = "é";
const decomposed = "e\u0301";

console.log(composed.normalize("NFC")); // "é" (composed)
console.log(decomposed.normalize("NFC")); // "é" (composed)

// NFD: Canonical Decomposition
console.log(composed.normalize("NFD")); // "é" (decomposed: e + combining accent)
console.log(decomposed.normalize("NFD")); // "é" (same - already decomposed)

// NFKC: Compatibility Decomposition + Canonical Composition
// Handles compatibility characters like fullwidth forms
const fullwidth = "Ａ";
const normal = "A";
console.log(fullwidth.normalize("NFKC")); // "A" (normalized)
console.log(fullwidth.normalize("NFC"));  // "Ａ" (not changed)

// NFKD: Compatibility Decomposition
console.log(fullwidth.normalize("NFKD")); // "A" (converted to normal form)

// Practical use: comparing user input
function normalizeForComparison(str) {
    return str.normalize("NFC").toLowerCase().trim();
}

const userInput = "café"; // Composed
const stored = "café";    // Composed

console.log(userInput === stored);             // true (already equal)
console.log(normalizeForComparison(userInput) === normalizeForComparison(stored)); // true

// Handling different inputs
const input1 = "résumé";  // Composed
const input2 = "résumé"; // Also composed
const input3 = "resume\u0301"; // Decomposed

console.log(input1 === input2); // true
console.log(input1 === input3);  // false (different representations)
console.log(input1.normalize() === input3.normalize()); // true
```

### Working with Surrogate Pairs

**Code Example 4: Surrogate Pair Handling**

```javascript
// file: examples/surrogate-pairs.js
// Description: Handling characters outside BMP

// Characters beyond Basic Multilingual Plane (BMP) need surrogate pairs
const emoji = "😀";
console.log(emoji.length);         // 2 (surrogate pair)
console.log(emoji.codePointAt(0)); // 128512 (actual code point)

// Incorrect handling with charCodeAt
console.log(emoji.charCodeAt(0)); // 55357 (high surrogate)
console.log(emoji.charCodeAt(1)); // 56832 (low surrogate)

// Correct handling with codePointAt
console.log(emoji.codePointAt(0)); // 128512 (complete code point)

// Iterating with codePointAt
function iterateCodePoints(str) {
    const codePoints = [];
    for (let i = 0; i < str.length; i++) {
        const code = str.codePointAt(i);
        codePoints.push(code);
        // Skip extra code units for characters > 0xFFFF
        if (code > 0xFFFF) i++;
    }
    return codePoints;
}

console.log(iterateCodePoints("A😀B")); // [65, 128512, 66]

// Check if character is surrogate
function isSurrogate(codeUnit) {
    return codeUnit >= 0xD800 && codeUnit <= 0xDFFF;
}

console.log(isSurrogate(0xD83D)); // true (high surrogate)
console.log(isSurrogate(0xDE00)); // true (low surrogate)
console.log(isSurrogate(0x0041)); // false (regular character)

// Create string from code points
const fromPoints = String.fromCodePoint(65, 128512, 66);
console.log(fromPoints); // "A😀B"
```

---

## 🌍 The Intl API Overview

The Intl object provides access to the ECMAScript Internationalization API, which includes formatters for dates, numbers, currencies, and strings.

### Intl Object Overview

```javascript
// Access different Intl APIs
console.log(Intl.DateTimeFormat);  // Date formatting
console.log(Intl.NumberFormat);    // Number formatting
console.log(Intl.Collator);        // String comparison
console.log(Intl.Segmenter);       // Text segmentation
console.log(Intl.ListFormat);      // List formatting
```

**Code Example 5: Intl API Overview**

```javascript
// file: examples/intl-overview.js
// Description: Exploring the Intl API

// Check available locales
console.log(Intl.DateTimeFormat().resolvedOptions().locale); // "en-US" (default)

// List supported locales for a function
const locales = Intl.DateTimeFormat.supportedLocalesOf(["de", "en", "fr"]);
console.log(locales); // ["de", "en", "fr"] (system dependent)

// Basic date formatting
const dateFormatter = new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "long",
    day: "numeric"
});

console.log(dateFormatter.format(new Date())); // "April 3, 2026"

// Number formatting
const numberFormatter = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD"
});

console.log(numberFormatter.format(1234.56)); // "$1,234.56"

// Collator for string sorting
const collator = new Intl.Collator("en");
console.log(collator.compare("apple", "banana")); // -1 (apple comes before banana)

// List formatter (ES2020+)
const listFormatter = new Intl.ListFormat("en", { style: "long", type: "conjunction" });
console.log(listFormatter.format(["apple", "banana", "cherry"])); // "apple, banana, and cherry"
```

---

## 📅 Locale-Specific Formatting

### Date and Time Formatting

**Code Example 6: Date Formatting**

```javascript
// file: examples/date-formatting.js
// Description: Locale-specific date formatting

const date = new Date("2024-01-15T14:30:00");

// US format
const usDate = new Intl.DateTimeFormat("en-US", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit"
});
console.log(usDate.format(date)); // "01/15/2024"

// German format
const deDate = new Intl.DateTimeFormat("de-DE", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit"
});
console.log(deDate.format(date)); // "15.01.2024"

// Japanese format
const jaDate = new Intl.DateTimeFormat("ja-JP", {
    year: "numeric",
    month: "long",
    day: "numeric"
});
console.log(jaDate.format(date)); // "2024年1月15日"

// Different formatting options
const options = { dateStyle: "full" };
console.log("en-US:", new Intl.DateTimeFormat("en-US", options).format(date));
// "Monday, January 15, 2024"
console.log("de-DE:", new Intl.DateTimeFormat("de-DE", options).format(date));
// "Montag, 15. Januar 2024"

// Time formatting
const timeOptions = {
    hour: "2-digit",
    minute: "2-digit",
    hour12: true
};
console.log("en-US:", new Intl.DateTimeFormat("en-US", timeOptions).format(date)); // "2:30 PM"
console.log("de-DE:", new Intl.DateTimeFormat("de-DE", timeOptions).format(date)); // "14:30"

// Relative time formatting
const rtf = new Intl.RelativeTimeFormat("en", { numeric: "auto" });
console.log(rtf.format(-1, "day"));    // "yesterday"
console.log(rtf.format(1, "day"));     // "tomorrow"
console.log(rtf.format(-3, "month"));  // "3 months ago"
```

### Number and Currency Formatting

**Code Example 7: Number Formatting**

```javascript
// file: examples/number-formatting.js
// Description: Locale-specific number formatting

const number = 1234567.89;

// Default formatting
console.log(new Intl.NumberFormat("en-US").format(number)); // "1,234,567.89"
console.log(new Intl.NumberFormat("de-DE").format(number)); // "1.234.567,89"

// Currency formatting
const currencyOptions = {
    style: "currency",
    currency: "USD",
    currencyDisplay: "symbol"
};

console.log("USD:", new Intl.NumberFormat("en-US", currencyOptions).format(1234.56));
// "$1,234.56"

currencyOptions.currency = "EUR";
console.log("EUR:", new Intl.NumberFormat("de-DE", currencyOptions).format(1234.56));
// "1.234,56 €"

currencyOptions.currency = "JPY";
console.log("JPY:", new Intl.NumberFormat("ja-JP", currencyOptions).format(1234));
// "¥1,234"

// Compact notation
const compactOptions = {
    notation: "compact",
    compactDisplay: "short"
};

console.log("en-US:", new Intl.NumberFormat("en-US", compactOptions).format(1234567));
// "1.2M"
console.log("de-DE:", new Intl.NumberFormat("de-DE", compactOptions).format(1234567));
// "1,2 Mio."

// Percentages
const percentOptions = {
    style: "percent",
    minimumFractionDigits: 2
};

console.log("en-US:", new Intl.NumberFormat("en-US", percentOptions).format(0.1234));
// "12.34%"
console.log("de-DE:", new Intl.NumberFormat("de-DE", percentOptions).format(0.1234));
// "12,34%"
```

### Plural Rules

**Code Example 8: Plural Rules**

```javascript
// file: examples/plural-rules.js
// Description: Handling plural forms across languages

const pr = new Intl.PluralRules("en-US");

console.log(pr.select(0));   // "other" (zero)
console.log(pr.select(1));    // "one"   (one)
console.log(pr.select(2));    // "other" (few)
console.log(pr.select(5));    // "other" (many)

// Russian plural rules (three forms: one, few, many)
const ruPlural = new Intl.PluralRules("ru-RU");
console.log(ruPlural.select(1));   // "one"
console.log(PluralRules.sel  2));    // "few"
console.log(ruPlural.select(5));    // "many"

// Using plural rules for i18n
function getPluralCategory(count, locale = "en") {
    const pr = new Intl.PluralRules(locale);
    return pr.select(count);
}

function formatItemCount(count, locale = "en") {
    const category = getPluralCategory(count, locale);
    
    const templates = {
        en: {
            one: "{count} item",
            other: "{count} items"
        },
        ru: {
            one: "{count} элемент",
            few: "{count} элемента",
            many: "{count} элементов"
        }
    };
    
    const template = templates[locale]?.[category] || templates.en.other;
    return template.replace("{count}", count);
}

console.log(formatItemCount(1, "en"));  // "1 item"
console.log(formatItemCount(5, "en"));  // "5 items"
console.log(formatItemCount(1, "ru"));  // "1 элемент"
console.log(formatItemCount(5, "ru"));  // "5 элементов"
```

---

## 🔤 String Comparison and Sorting

### Using Intl.Collator

**Code Example 9: String Comparison**

```javascript
// file: examples/string-comparison.js
// Description: Locale-aware string comparison

const collator = new Intl.Collator("en");

// Basic comparison
console.log(collator.compare("apple", "banana")); // -1 (apple < banana)
console.log(collator.compare("banana", "apple")); // 1 (banana > apple)
console.log(collator.compare("apple", "apple"));  // 0 (equal)

// Case-insensitive comparison
const caseInsensitive = new Intl.Collator("en", { sensitivity: "base" });
console.log(caseInsensitive.compare("apple", "Apple")); // 0 (equal)

// Accent-insensitive comparison
const accentInsensitive = new Intl.Collator("en", { sensitivity: "accent" });
console.log(accentInsensitive.compare("café", "cafe")); // 0 (equal)

// Sorting arrays
const fruits = ["banana", "Apple", "Cherry", "apricot"];
const sorted = [...fruits].sort(new Intl.Collator("en").compare);
console.log(sorted); // ["Apple", "apricot", "banana", "Cherry"] (alphabetical)

// Sort with options
const sortedIgnoreCase = [...fruits].sort(
    new Intl.Collator("en", { sensitivity: "base", ignorePunctuation: true }).compare
);
console.log(sortedIgnoreCase); // ["Apple", "apricot", "banana", "Cherry"]

// German sorting (umlauts after o)
const german = new Intl.Collator("de-DE");
console.log(german.compare("o", "ö")); // -1 (o comes before ö)

// Swedish sorting (å, ä, ö at end of alphabet)
const swedish = new Intl.Collator("sv-SE");
console.log(swedish.compare("z", "ö")); // -1 (z comes before ö)
```

### Collation Options

**Code Example 10: Collation Options**

```javascript
// file: examples/collation-options.js
// Description: Advanced collation options

// Numeric collation (natural sorting)
const naturalSort = new Intl.Collator("en", { numeric: true });
console.log(naturalSort.compare("file10", "file2"));  // 1 (file2 < file10)
console.log(naturalSort.compare("file2", "file10")); // -1

// Ignore punctuation
const ignorePunct = new Intl.Collator("en", { ignorePunctuation: true });
console.log(ignorePunct.compare("don't", "dont"));   // 0

// Japanese collation
const japanese = new Intl.Collator("ja", { sensitivity: "variant" });
// Japanese typically uses different sort orders based on context

// Custom sort with weights (if supported)
const custom = new Intl.Collator("en", {
    collation: "search",
    usage: "search" // Optimized for search rather than sorting
});

// Performance optimization for large datasets
const largeList = ["item1", "item10", "item2", "item20", "item3"];

// Create collator once, reuse for many comparisons
const quickCollator = new Intl.Collator("en", { numeric: true });
const sortedList = largeList.sort(quickCollator.compare);
console.log(sortedList); // ["item1", "item2", "item3", "item10", "item20"]
```

---

## 📝 Text Encoding

### TextEncoder and TextDecoder

**Code Example 11: Text Encoding**

```javascript
// file: examples/text-encoding.js
// Description: Working with text encoders and decoders

const encoder = new TextEncoder();
const decoder = new TextDecoder();

// Encoding strings to Uint8Array
const encoded = encoder.encode("Hello");
console.log(encoded); // Uint8Array(5) [72, 101, 108, 108, 111]

// Decoding back to string
const decoded = decoder.decode(encoded);
console.log(decoded); // "Hello"

// Different encodings
const utf16Encoder = new TextEncoder("utf-16", { fatal: true });
// Note: TextEncoder only supports utf-8

// Using TextDecoder with different encodings
const latin1Decoder = new TextDecoder("iso-8859-1");
const latin1Bytes = new Uint8Array([72, 101, 108, 108, 111]); // ASCII is same in Latin-1
console.log(latin1Decoder.decode(latin1Bytes)); // "Hello"

// Handling malformed data
const utf8Decoder = new TextDecoder("utf-8", { fatal: false });

// Invalid UTF-8 sequence (replacement character used)
const invalidBytes = new Uint8Array([72, 101, 108, 108, 0xFF, 111]);
console.log(utf8Decoder.decode(invalidBytes)); // "Helloo" (with replacement)

// Streaming decoding
const streamData = [encoder.encode("Hello "), encoder.encode("World")];
let streamDecoder = new TextDecoder();
let result = "";
for (const chunk of streamData) {
    result += streamDecoder.decode(chunk, { stream: true });
}
result += streamDecoder.decode(); // Flush
console.log(result); // "Hello World"
```

### Handling Binary Data

**Code Example 12: Binary String Handling**

```javascript
// file: examples/binary-strings.js
// Description: Converting between strings and binary

// Convert string to base64
function stringToBase64(str) {
    const bytes = new TextEncoder().encode(str);
    let binary = "";
    for (const byte of bytes) {
        binary += String.fromCharCode(byte);
    }
    return btoa(binary);
}

// Convert base64 to string
function base64ToString(base64) {
    const binary = atob(base64);
    const bytes = new Uint8Array(binary.length);
    for (let i = 0; i < binary.length; i++) {
        bytes[i] = binary.charCodeAt(i);
    }
    return new TextDecoder().decode(bytes);
}

console.log(stringToBase64("Hello World")); // "SGVsbG8gV29ybGQ="
console.log(base64ToString("SGVsbG8gV29ybGQ=")); // "Hello World"

// Modern approach using Uint8Array
function encodeBase64(str) {
    return btoa(String.fromCodePoint(...new TextEncoder().encode(str)));
}

function decodeBase64(base64) {
    return new TextDecoder().decode(
        Uint8Array.from(atob(base64), c => c.charCodeAt(0))
    );
}

// Hex encoding
function stringToHex(str) {
    return [...new TextEncoder().encode(str)]
        .map(b => b.toString(16).padStart(2, "0"))
        .join("");
}

function hexToString(hex) {
    const bytes = new Uint8Array(hex.match(/.{1,2}/g).map(byte => parseInt(byte, 16)));
    return new TextDecoder().decode(bytes);
}

console.log(stringToHex("Hello")); // "48656c6c6f"
console.log(hexToString("48656c6c6f")); // "Hello"
```

---

## 🏗️ Practical Internationalization Patterns

### i18n Implementation

**Code Example 13: Basic i18n System**

```javascript
// file: examples/i18n-basic.js
// Description: Simple internationalization system

class I18n {
    constructor(locale = "en", translations = {}) {
        this.locale = locale;
        this.translations = translations;
        this.fallbackLocale = "en";
    }

    setLocale(locale) {
        this.locale = locale;
    }

    t(key, params = {}) {
        const translation = this.findTranslation(key, this.locale) 
            || this.findTranslation(key, this.fallbackLocale)
            || key;
        
        return this.interpolate(translation, params);
    }

    findTranslation(locale, key) {
        const parts = key.split(".");
        let value = this.translations[locale];
        
        for (const part of parts) {
            if (value && typeof value === "object") {
                value = value[part];
            } else {
                return null;
            }
        }
        
        return value;
    }

    interpolate(template, params) {
        return template.replace(/\{(\w+)\}/g, (match, key) => {
            return params[key] !== undefined ? params[key] : match;
        });
    }
}

// Translation data
const translations = {
    en: {
        greeting: "Hello, {name}!",
        items: "{count, plural, =0 {No items} one {One item} other {# items}}",
        date: "{date, date, long}"
    },
    de: {
        greeting: "Hallo, {name}!",
        items: "{count, plural, =0 {Keine Elemente} one {Ein Element} other {# Elemente}}",
        date: "{date, date, long}"
    },
    ja: {
        greeting: "こんにちは、{name}さん！",
        items: "{count, plural, =0 {アイテムなし} other {#個のアイテム}}"
    }
};

// Usage
const i18n = new I18n("en", translations);

console.log(i18n.t("greeting", { name: "World" })); // "Hello, World!"

i18n.setLocale("de");
console.log(i18n.t("greeting", { name: "Welt" })); // "Hallo, Welt!"

i18n.setLocale("ja");
console.log(i18n.t("greeting", { name: "世界" })); // "こんにちは、世界さん！"
```

### Locale Detection

**Code Example 14: Locale Detection**

```javascript
// file: examples/locale-detection.js
// Description: Detecting and handling user locale

// Get user's preferred languages
function getUserLocales() {
    const nav = typeof navigator !== "undefined" ? navigator : null;
    if (!nav) return ["en"];
    
    return nav.languages || [nav.language || "en"];
}

console.log(getUserLocales()); // ["en-US", "en", "zh-CN", "zh"]

// Find best matching locale from available translations
function findBestLocale(userLocales, availableLocales) {
    for (const userLocale of userLocales) {
        // Exact match
        if (availableLocales.includes(userLocale)) {
            return userLocale;
        }
        
        // Language-only match (e.g., "en" matches "en-US")
        const language = userLocale.split("-")[0];
        const match = availableLocales.find(l => l.startsWith(language + "-"));
        if (match) {
            return match;
        }
    }
    
    return availableLocales[0] || "en";
}

const available = ["en", "en-US", "de", "fr", "zh-CN"];
console.log(findBestLocale(["fr-CA", "en-US"], available)); // "fr"
console.log(findBestLocale(["zh-TW", "en"], available));    // "zh-CN"

// Detect timezone
function getUserTimezone() {
    try {
        return Intl.DateTimeFormat().resolvedOptions().timeZone;
    } catch {
        return "UTC";
    }
}

console.log(getUserTimezone()); // "America/New_York"

// Format relative to user's locale
function formatRelativeTime(date, locale = "en") {
    const rtf = new Intl.RelativeTimeFormat(locale, { numeric: "auto" });
    const diff = date - new Date();
    const seconds = Math.floor(diff / 1000);
    
    if (Math.abs(seconds) < 60) return rtf.format(seconds, "second");
    if (Math.abs(seconds) < 3600) return rtf.format(Math.floor(seconds / 60), "minute");
    if (Math.abs(seconds) < 86400) return rtf.format(Math.floor(seconds / 3600), "hour");
    return rtf.format(Math.floor(seconds / 86400), "day");
}

console.log(formatRelativeTime(new Date(Date.now() - 3600000), "en")); // "1 hour ago"
```

### Complete i18n Framework

**Code Example 15: Professional i18n Implementation**

```javascript
// file: examples/i18n-professional.js
// Description: Production-ready i18n implementation

class IntlManager {
    constructor(options = {}) {
        this.locale = options.locale || "en";
        this.fallbackLocale = options.fallbackLocale || "en";
        this.translations = new Map();
        this.numberFormatters = new Map();
        this.dateFormatters = new Map();
        this.collator = null;
    }

    addTranslations(locale, translations) {
        this.translations.set(locale, translations);
    }

    setLocale(locale) {
        if (!this.translations.has(locale)) {
            console.warn(`No translations for locale: ${locale}`);
        }
        this.locale = locale;
        this.collator = null; // Reset collator
    }

    t(key, options = {}) {
        const { params = {}, count, default: defaultValue } = options;
        const translations = this.translations.get(this.locale) 
            || this.translations.get(this.fallbackLocale)
            || {};
        
        let translation = this.getNestedValue(translations, key) 
            || this.getNestedValue(this.translations.get(this.fallbackLocale), key)
            || defaultValue
            || key;

        // Handle plural
        if (count !== undefined && typeof translation === "object") {
            const pr = new Intl.PluralRules(this.locale);
            const category = pr.select(count);
            translation = translation[category] || translation.other || key;
            translation = translation.replace("#", count);
        }

        // Interpolate parameters
        return translation.replace(/\{(\w+)\}/g, (match, param) => {
            return params[param] !== undefined ? params[param] : match;
        });
    }

    getNestedValue(obj, path) {
        return path.split(".").reduce((current, key) => {
            return current && current[key] !== undefined ? current[key] : null;
        }, obj);
    }

    formatNumber(number, options = {}) {
        const key = JSON.stringify({ locale: this.locale, ...options });
        
        if (!this.numberFormatters.has(key)) {
            this.numberFormatters.set(key, new Intl.NumberFormat(this.locale, options));
        }
        
        return this.numberFormatters.get(key).format(number);
    }

    formatCurrency(amount, currency, options = {}) {
        return this.formatNumber(amount, {
            style: "currency",
            currency,
            ...options
        });
    }

    formatDate(date, options = {}) {
        const key = JSON.stringify({ locale: this.locale, ...options });
        
        if (!this.dateFormatters.has(key)) {
            this.dateFormatters.set(key, new Intl.DateTimeFormat(this.locale, options));
        }
        
        return this.dateFormatters.get(key).format(date);
    }

    formatRelative(date, options = {}) {
        const rtf = new Intl.RelativeTimeFormat(this.locale, options);
        const diff = date - new Date();
        const absDiff = Math.abs(diff);
        
        const units = [
            ["day", 86400000],
            ["hour", 3600000],
            ["minute", 60000],
            ["second", 1000]
        ];
        
        for (const [unit, ms] of units) {
            if (absDiff >= ms) {
                const value = Math.floor(diff / ms);
                return rtf.format(value, unit);
            }
        }
        
        return rtf.format(Math.floor(diff / 1000), "second");
    }

    sort(array, key) {
        if (!this.collator) {
            this.collator = new Intl.Collator(this.locale);
        }
        
        return [...array].sort((a, b) => {
            const aVal = key ? a[key] : a;
            const bVal = key ? b[key] : b;
            return this.collator.compare(aVal, bVal);
        });
    }
}

// Usage
const i18n = new IntlManager({ locale: "de", fallbackLocale: "en" });

i18n.addTranslations("en", {
    greeting: "Hello, {name}!",
    items: {
        one: "{count} item",
        other: "{count} items"
    },
    price: "Price: {price}",
    welcome: "Welcome to our app"
});

i18n.addTranslations("de", {
    greeting: "Hallo, {name}!",
    items: {
        one: "{count} Artikel",
        other: "{count} Artikel"
    },
    price: "Preis: {price}",
    welcome: "Willkommen in unserer App"
});

console.log(i18n.t("greeting", { params: { name: "World" } })); // "Hallo, World!"
console.log(i18n.t("items", { count: 5 })); // "5 Artikel"
console.log(i18n.formatCurrency(99.99, "EUR")); // "99,99 €"
console.log(i18n.formatDate(new Date(), { dateStyle: "full" }));
```

---

## 📊 Key Takeaways

1. **Unicode is Fundamental**: Understanding code points, grapheme clusters, and normalization is essential for internationalized applications.

2. **Use Intl API**: The Intl API provides robust, locale-aware formatting for dates, numbers, currencies, and strings.

3. **Normalization**: Always normalize strings before comparison or storage to handle different Unicode representations.

4. **Grapheme Clusters**: Use Intl.Segmenter for accurate character counting that respects user-perceived characters.

5. **Locale Detection**: Detect user's preferred locale and provide appropriate fallbacks for missing translations.

6. **Sorting**: Use Intl.Collator for locale-aware sorting rather than default string comparison.

7. **Encoding**: Use TextEncoder and TextDecoder for proper text encoding and decoding.

---

## ⚠️ Common Pitfalls

1. **Using .length**: String.length counts UTF-16 code units, not visible characters or grapheme clusters.

2. **Ignoring Normalization**: Different Unicode representations of the same text can cause comparison failures.

3. **Hardcoded Formats**: Always use locale-aware formatting for dates, numbers, and currencies.

4. **Assuming ASCII**: Don't assume text is ASCII; handle multi-byte characters correctly.

5. **Incomplete i18n**: Don't just translate strings; consider date formats, number formats, and sorting.

6. **Missing Fallbacks**: Always provide fallback locales when translations are incomplete.

7. **Not Testing**: Test with actual multi-byte characters and different locales.

---

## 🔗 Related Files

- **[String Methods Comprehensive](./01_STRING_METHODS_COMPREHENSIVE.md)** - Basic string manipulation
- **[Regular Expressions Master](./02_REGULAR_EXPRESSIONS_MASTER.md)** - Unicode regex patterns
- **[Template Literals](./03_STRING_TEMPLATES_AND_INTERPOLATION.md)** - String template security

---

## 📚 Further Reading

- [MDN: Internationalization](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Intl)
- [Unicode Consortium](https://unicode.org/)
- [ICU Project](https://icu4c-demos.unicode.org/icu-bin/locexp?d_=en_US&_=en_US)