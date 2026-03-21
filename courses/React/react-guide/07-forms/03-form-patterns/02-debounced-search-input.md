# Debounced Search Input

## Overview

Search functionality is ubiquitous in modern web applications, but making an API call on every keystroke would overwhelm your servers and create a poor user experience. Debouncing is the technique of delaying function execution until after a user stops typing for a specified duration. This guide covers building a custom useDebounce hook, integrating it with React Hook Form, handling stale requests with AbortController, and displaying proper loading and empty states.

## Prerequisites

- Understanding of React hooks (useState, useEffect)
- Basic familiarity with React Hook Form
- Knowledge of the Fetch API and async/await
- Understanding of React's useCallback hook

## Core Concepts

### Creating a Custom useDebounce Hook

The debounce pattern delays executing a function until a specified time has passed since the last call. This is essential for search inputs where you want to wait for the user to stop typing before making an API request.

```tsx
// File: src/hooks/useDebounce.ts

import { useState, useEffect } from 'react';

/**
 * A custom hook that delays updating a value until after a specified
 * delay has elapsed since the last change. This is useful for search
 * inputs where you want to wait for the user to stop typing.
 * 
 * @param value - The value to debounce
 * @param delay - The delay in milliseconds (default: 500ms)
 * @returns The debounced value
 */
function useDebounce<T>(value: T, delay: number = 500): T {
  // State to store the debounced value
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    // Create a timer that updates the debounced value after the delay
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    // Cleanup: clear the timer if value changes before delay completes
    // This ensures we only update after user stops typing
    return () => {
      clearTimeout(handler);
    };
  }, [value, delay]); // Re-run effect when value or delay changes

  return debouncedValue;
}

export default useDebounce;
```

### Using useDebounce with React Hook Form

Now let's combine the debounce hook with React Hook Form to create a performant search input.

```tsx
// File: src/components/DebouncedSearch.tsx

import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import useDebounce from '../hooks/useDebounce';

interface SearchData {
  query: string;
}

interface SearchResult {
  id: number;
  title: string;
  description: string;
}

function DebouncedSearch() {
  const { register, watch, formState: { errors } } = useForm<SearchData>({
    defaultValues: { query: "" }
  });

  // Watch the query field
  const query = watch("query");
  
  // Debounce the query - only update after 500ms of no typing
  const debouncedQuery = useDebounce(query, 500);

  // State for search results and loading status
  const [results, setResults] = useState<SearchResult[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [hasSearched, setHasSearched] = useState(false);

  // Effect to fetch results when debounced query changes
  useEffect(() => {
    // Don't search if query is empty
    if (!debouncedQuery.trim()) {
      setResults([]);
      setHasSearched(false);
      return;
    }

    const fetchResults = async () => {
      setIsLoading(true);
      setHasSearched(true);

      try {
        // Simulate API call
        const response = await fetch(
          `/api/search?q=${encodeURIComponent(debouncedQuery)}`
        );
        const data = await response.json();
        setResults(data);
      } catch (error) {
        console.error("Search error:", error);
        setResults([]);
      } finally {
        setIsLoading(false);
      }
    };

    // Execute the search
    fetchResults();
  }, [debouncedQuery]);

  return (
    <form className="search-form">
      <h2>Search</h2>

      <div className="search-input-wrapper">
        {/* Search input */}
        <input
          {...register("query", {
            required: "Please enter a search term",
            minLength: {
              value: 2,
              message: "Search term must be at least 2 characters"
            }
          })}
          type="search"
          placeholder="Search for items..."
          className="search-input"
        />
        
        {/* Loading indicator - show when typing but not yet searching */}
        {query !== debouncedQuery && (
          <span className="typing-indicator">Typing...</span>
        )}
      </div>

      {errors.query && (
        <span className="error">{errors.query.message}</span>
      )}

      {/* Results section */}
      <div className="results-section">
        {isLoading && (
          <div className="loading">
            <span>Searching for "{debouncedQuery}"...</span>
          </div>
        )}

        {!isLoading && hasSearched && results.length === 0 && (
          <div className="empty-state">
            <p>No results found for "{debouncedQuery}"</p>
            <p className="hint">Try different keywords</p>
          </div>
        )}

        {!isLoading && results.length > 0 && (
          <ul className="results-list">
            {results.map(result => (
              <li key={result.id} className="result-item">
                <h3>{result.title}</h3>
                <p>{result.description}</p>
              </li>
            ))}
          </ul>
        )}

        {!hasSearched && query.length < 2 && (
          <div className="initial-state">
            <p>Enter at least 2 characters to search</p>
          </div>
        )}
      </div>
    </form>
  );
}

export default DebouncedSearch;
```

### Handling Stale Requests with AbortController

When a user types quickly, previous search requests may resolve after newer ones, causing outdated results to appear. The AbortController allows us to cancel in-flight requests when new ones are made.

```tsx
// File: src/components/SearchWithAbortController.tsx

import { useState, useEffect, useRef } from 'react';
import { useForm } from 'react-hook-form';
import useDebounce from '../hooks/useDebounce';

interface SearchData {
  query: string;
}

interface SearchResult {
  id: number;
  name: string;
  category: string;
}

// Mock API function that simulates search
async function searchApi(query: string, signal: AbortSignal): Promise<SearchResult[]> {
  // Simulate network delay (random between 200-800ms)
  await new Promise(resolve => setTimeout(resolve, Math.random() * 600 + 200));
  
  // Check if request was aborted
  if (signal.aborted) {
    throw new DOMException('Aborted', 'AbortError');
  }

  // Mock results - in real app this would be an API call
  const mockDatabase = [
    { id: 1, name: 'React', category: 'Framework' },
    { id: 2, name: 'Vue', category: 'Framework' },
    { id: 3, name: 'Angular', category: 'Framework' },
    { id: 4, name: 'Svelte', category: 'Framework' },
    { id: 5, name: 'TypeScript', category: 'Language' },
    { id: 6, name: 'JavaScript', category: 'Language' },
    { id: 7, name: 'Python', category: 'Language' },
    { id: 8, name: 'Node.js', category: 'Runtime' },
  ];

  // Filter based on query
  return mockDatabase.filter(item =>
    item.name.toLowerCase().includes(query.toLowerCase()) ||
    item.category.toLowerCase().includes(query.toLowerCase())
  );
}

function SearchWithAbortController() {
  const { register, watch } = useForm<SearchData>({
    defaultValues: { query: "" }
  });

  const query = watch("query");
  const debouncedQuery = useDebounce(query, 300);

  const [results, setResults] = useState<SearchResult[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Use a ref to store the AbortController
  // This ensures we can access the latest controller in the effect
  const abortControllerRef = useRef<AbortController | null>(null);

  useEffect(() => {
    // Clear results if query is empty
    if (!debouncedQuery.trim()) {
      setResults([]);
      setError(null);
      return;
    }

    // Cancel any in-flight request before starting a new one
    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
    }

    // Create new AbortController for this request
    const abortController = new AbortController();
    abortControllerRef.current = abortController;

    // Set loading state
    setIsLoading(true);
    setError(null);

    // Fetch results
    searchApi(debouncedQuery, abortController.signal)
      .then(data => {
        // Only update state if request wasn't aborted
        if (!abortController.signal.aborted) {
          setResults(data);
        }
      })
      .catch(error => {
        // Ignore abort errors - they're expected when cancelling requests
        if (error.name !== 'AbortError') {
          setError('Failed to search. Please try again.');
          console.error('Search error:', error);
        }
      })
      .finally(() => {
        // Only update loading state if request wasn't aborted
        if (!abortController.signal.aborted) {
          setIsLoading(false);
        }
      });

    // Cleanup: abort request when effect re-runs or component unmounts
    return () => {
      abortController.abort();
    };
  }, [debouncedQuery]);

  return (
    <div className="search-container">
      <h2>Product Search</h2>

      <div className="search-input-group">
        <input
          {...register("query")}
          type="search"
          placeholder="Search products..."
          className="search-input"
        />
        
        {/* Show spinner while loading */}
        {isLoading && <span className="spinner" />}
      </div>

      {/* Error message */}
      {error && (
        <div className="error-message" role="alert">
          {error}
        </div>
      )}

      {/* Results */}
      <div className="results">
        {isLoading && (
          <p className="loading-message">Searching...</p>
        )}

        {!isLoading && debouncedQuery && results.length === 0 && (
          <p className="empty-message">
            No products found for "{debouncedQuery}"
          </p>
        )}

        {!isLoading && results.length > 0 && (
          <ul className="results-list">
            {results.map(result => (
              <li key={result.id} className="result-item">
                <span className="product-name">{result.name}</span>
                <span className="product-category">{result.category}</span>
              </li>
            ))}
          </ul>
        )}
      </div>

      {/* Search info */}
      <p className="search-info">
        {debouncedQuery && !isLoading && (
          <>Found {results.length} result{results.length !== 1 ? 's' : ''}</>
        )}
      </p>
    </div>
  );
}

export default SearchWithAbortController;
```

### Advanced: Combining Debounce with React Query

For production applications, combining debouncing with a data fetching library like TanStack Query (React Query) provides caching, retry logic, and better state management out of the box.

```tsx
// File: src/components/AdvancedSearchWithReactQuery.tsx

import { useForm } from 'react-hook-form';
import { useQuery } from '@tanstack/react-query';
import useDebounce from '../hooks/useDebounce';

interface SearchData {
  query: string;
}

interface Product {
  id: number;
  name: string;
  price: number;
  inStock: boolean;
}

// API function with error handling
async function searchProducts(query: string): Promise<Product[]> {
  const response = await fetch(`/api/products?q=${encodeURIComponent(query)}`);
  
  if (!response.ok) {
    throw new Error('Failed to fetch products');
  }
  
  return response.json();
}

function AdvancedSearchWithReactQuery() {
  const { register, watch } = useForm<SearchData>({
    defaultValues: { query: "" }
  });

  const query = watch("query");
  const debouncedQuery = useDebounce(query, 400);

  // Use React Query for data fetching
  // This handles caching, loading states, error handling automatically
  const { 
    data: products, 
    isLoading, 
    isError, 
    error,
    isFetching 
  } = useQuery({
    queryKey: ['products', debouncedQuery],
    queryFn: () => searchProducts(debouncedQuery),
    // Don't run query if query is empty
    enabled: debouncedQuery.length >= 2,
    // Cache results for 5 minutes
    staleTime: 5 * 60 * 1000,
    // Don't retry too many times
    retry: 2,
    // Cancel previous request when new query comes in
    cancelPreviousOnNewQuery: true
  });

  return (
    <div className="product-search">
      <h2>Product Search</h2>

      {/* Search Input */}
      <div className="search-wrapper">
        <input
          {...register("query")}
          type="search"
          placeholder="Search products (min 2 characters)..."
          className="search-input"
          aria-label="Search products"
        />
        
        {/* Show typing indicator */}
        {query !== debouncedQuery && (
          <span className="typing">Searching...</span>
        )}
      </div>

      {/* Loading States */}
      {isFetching && (
        <div className="loading-overlay">
          <span className="loading-spinner" />
          <p>Finding products...</p>
        </div>
      )}

      {/* Error State */}
      {isError && (
        <div className="error-card" role="alert">
          <p>⚠️ {error?.message || 'Something went wrong'}</p>
          <p className="hint">Please try again later</p>
        </div>
      )}

      {/* Results */}
      <div className="results-container">
        {!isLoading && !isFetching && debouncedQuery.length >= 2 && (
          <>
            {products?.length === 0 ? (
              <div className="empty-results">
                <p>No products found for "{debouncedQuery}"</p>
                <p className="suggestion">Try different keywords or check spelling</p>
              </div>
            ) : (
              <ul className="product-list">
                {products?.map(product => (
                  <li key={product.id} className="product-card">
                    <div className="product-info">
                      <h3>{product.name}</h3>
                      <span className={`stock ${product.inStock ? 'in-stock' : 'out-of-stock'}`}>
                        {product.inStock ? '✓ In Stock' : '✗ Out of Stock'}
                      </span>
                    </div>
                    <span className="product-price">
                      ${product.price.toFixed(2)}
                    </span>
                  </li>
                ))}
              </ul>
            )}
          </>
        )}
      </div>

      {/* Search metadata */}
      <div className="search-meta">
        {debouncedQuery.length < 2 && (
          <p className="hint">Enter at least 2 characters to search</p>
        )}
        
        {debouncedQuery.length >= 2 && !isFetching && (
          <p className="result-count">
            {products?.length ?? 0} product{products?.length !== 1 ? 's' : ''} found
          </p>
        )}
      </div>
    </div>
  );
}

export default AdvancedSearchWithReactQuery;
```

## Common Mistakes

### Mistake 1: Not Debouncing the Input

Making API calls on every keystroke without debouncing causes excessive requests and potential race conditions.

```tsx
// ❌ WRONG - Makes API call on every keystroke
const [query, setQuery] = useState("");
const handleChange = (e) => {
  setQuery(e.target.value);
  fetch(`/api/search?q=${e.target.value}`); // Too many requests!
};

// ✅ CORRECT - Wait for user to stop typing
const debouncedQuery = useDebounce(query, 500);
useEffect(() => {
  if (debouncedQuery) {
    fetch(`/api/search?q=${debouncedQuery}`);
  }
}, [debouncedQuery]);
```

### Mistake 2: Not Handling Race Conditions

When a user types quickly, older requests may resolve after newer ones, showing stale results.

```tsx
// ❌ WRONG - Race condition possible
useEffect(() => {
  fetch(`/api/search?q=${query}`)
    .then(res => res.json())
    .then(setResults); // Could set stale results!
}, [query]);

// ✅ CORRECT - Use AbortController to cancel stale requests
useEffect(() => {
  const controller = new AbortController();
  
  fetch(`/api/search?q=${query}`, { signal: controller.signal })
    .then(res => res.json())
    .then(setResults)
    .catch(err => {
      if (err.name !== 'AbortError') throw err;
    });
    
  return () => controller.abort();
}, [query]);
```

### Mistake 3: Not Showing Appropriate Loading States

Users need feedback when their search is processing.

```tsx
// ❌ WRONG - No feedback during loading
const [results, setResults] = useState([]);
useEffect(() => {
  setIsLoading(true);
  fetch(`/api/search?q=${query}`)
    .then(res => res.json())
    .then(setResults)
    .finally(() => setIsLoading(false));
}, [query]);

// Return doesn't show loading state at all
return <div>{results.map(...)}</div>;

// ✅ CORRECT - Show loading, empty, and error states
return (
  <div>
    {isLoading && <Spinner />}
    {!isLoading && results.length === 0 && query && <EmptyState />}
    {!isLoading && results.length > 0 && <ResultsList items={results} />}
  </div>
);
```

## Real-World Example

Here's a complete search component with all best practices: debouncing, abort controller, loading states, error handling, keyboard navigation, and accessibility features.

```tsx
// File: src/components/CompleteSearchExample.tsx

import { useState, useEffect, useRef, useCallback } from 'react';
import { useForm } from 'react-hook-form';
import useDebounce from '../hooks/useDebounce';

// Types
interface SearchFormData {
  searchQuery: string;
}

interface SearchResult {
  id: string;
  title: string;
  subtitle: string;
  type: 'article' | 'product' | 'category';
  url: string;
}

// Mock API function
async function performSearch(query: string, signal: AbortSignal): Promise<SearchResult[]> {
  await new Promise(resolve => setTimeout(resolve, 300 + Math.random() * 500));
  
  if (signal.aborted) throw new DOMException('Aborted', 'AbortError');

  const mockData: SearchResult[] = [
    { id: '1', title: 'React Tutorial', subtitle: 'Learn React from scratch', type: 'article', url: '/articles/react-tutorial' },
    { id: '2', title: 'React Hooks Guide', subtitle: 'Master useState and useEffect', type: 'article', url: '/articles/react-hooks' },
    { id: '3', title: 'React Components', subtitle: 'Building reusable UI', type: 'article', url: '/articles/components' },
    { id: '4', title: 'JavaScript Framework', subtitle: 'Frontend Development', type: 'category', url: '/categories/javascript' },
    { id: '5', title: 'React Book', subtitle: '$29.99 - In Stock', type: 'product', url: '/products/react-book' },
    { id: '6', title: 'TypeScript Course', subtitle: '$49.99 - Pre-order', type: 'product', url: '/products/typescript-course' },
  ];

  return mockData.filter(item => 
    item.title.toLowerCase().includes(query.toLowerCase()) ||
    item.subtitle.toLowerCase().includes(query.toLowerCase())
  );
}

// Main Search Component
function CompleteSearchExample() {
  const { register, watch, reset } = useForm<SearchFormData>({
    defaultValues: { searchQuery: "" }
  });

  const searchQuery = watch("searchQuery");
  const debouncedQuery = useDebounce(searchQuery, 350);

  const [results, setResults] = useState<SearchResult[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedIndex, setSelectedIndex] = useState(-1);
  
  const abortControllerRef = useRef<AbortController | null>(null);
  const resultsRef = useRef<HTMLUListElement>(null);

  // Fetch results effect
  useEffect(() => {
    if (!debouncedQuery.trim()) {
      setResults([]);
      setError(null);
      setSelectedIndex(-1);
      return;
    }

    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
    }

    const abortController = new AbortController();
    abortControllerRef.current = abortController;

    setIsLoading(true);
    setError(null);

    performSearch(debouncedQuery, abortController.signal)
      .then(data => {
        if (!abortController.signal.aborted) {
          setResults(data);
          setSelectedIndex(data.length > 0 ? 0 : -1);
        }
      })
      .catch(err => {
        if (err.name !== 'AbortError') {
          setError('Search failed. Please try again.');
        }
      })
      .finally(() => {
        if (!abortController.signal.aborted) {
          setIsLoading(false);
        }
      });

    return () => abortController.abort();
  }, [debouncedQuery]);

  // Keyboard navigation
  const handleKeyDown = useCallback((e: React.KeyboardEvent) => {
    if (!results.length) return;

    switch (e.key) {
      case 'ArrowDown':
        e.preventDefault();
        setSelectedIndex(prev => Math.min(prev + 1, results.length - 1));
        break;
      case 'ArrowUp':
        e.preventDefault();
        setSelectedIndex(prev => Math.max(prev - 1, 0));
        break;
      case 'Enter':
        e.preventDefault();
        if (results[selectedIndex]) {
          window.location.href = results[selectedIndex].url;
        }
        break;
      case 'Escape':
        reset();
        setResults([]);
        break;
    }
  }, [results, selectedIndex, reset]);

  // Highlight matching text
  const highlightMatch = (text: string, query: string) => {
    if (!query) return text;
    const regex = new RegExp(`(${query})`, 'gi');
    const parts = text.split(regex);
    return parts.map((part, i) => 
      regex.test(part) ? <mark key={i}>{part}</mark> : part
    );
  };

  return (
    <div className="complete-search">
      <h2>Search Everything</h2>

      {/* Search Input */}
      <div className="search-input-wrapper">
        <span className="search-icon">🔍</span>
        <input
          {...register("searchQuery")}
          type="text"
          placeholder="Search articles, products, categories..."
          className="search-input"
          onKeyDown={handleKeyDown}
          aria-label="Search"
          aria-expanded={results.length > 0}
          aria-controls="search-results"
          aria-activedescendant={selectedIndex >= 0 ? `result-${selectedIndex}` : undefined}
        />
        
        {searchQuery && (
          <button 
            type="button" 
            className="clear-btn"
            onClick={() => {
              reset();
              setResults([]);
            }}
            aria-label="Clear search"
          >
            ×
          </button>
        )}

        {isLoading && <span className="loading-spinner" aria-label="Searching" />}
      </div>

      {/* Error Message */}
      {error && (
        <div className="error-message" role="alert">
          {error}
        </div>
      )}

      {/* Results Dropdown */}
      {results.length > 0 && (
        <ul 
          id="search-results"
          className="search-results"
          ref={resultsRef}
          role="listbox"
        >
          {results.map((result, index) => (
            <li 
              key={result.id}
              id={`result-${index}`}
              className={`result-item ${index === selectedIndex ? 'selected' : ''}`}
              role="option"
              aria-selected={index === selectedIndex}
              onClick={() => window.location.href = result.url}
              onMouseEnter={() => setSelectedIndex(index)}
            >
              <span className={`result-type ${result.type}`}>
                {result.type === 'article' && '📄'}
                {result.type === 'product' && '🛒'}
                {result.type === 'category' && '📁'}
              </span>
              <div className="result-content">
                <span className="result-title">
                  {highlightMatch(result.title, searchQuery)}
                </span>
                <span className="result-subtitle">
                  {highlightMatch(result.subtitle, searchQuery)}
                </span>
              </div>
              <span className="arrow">→</span>
            </li>
          ))}
        </ul>
      )}

      {/* Empty State */}
      {!isLoading && debouncedQuery && results.length === 0 && !error && (
        <div className="empty-results">
          <p>No results for "{debouncedQuery}"</p>
          <p className="suggestion">Try different keywords</p>
        </div>
      )}

      {/* Keyboard Hints */}
      {results.length > 0 && (
        <div className="keyboard-hints">
          <span>↑↓ Navigate</span>
          <span>↵ Select</span>
          <span>ESC Close</span>
        </div>
      )}
    </div>
  );
}

export default CompleteSearchExample;
```

## Key Takeaways

- Debouncing delays executing a function until the user stops typing, preventing excessive API calls
- The useDebounce hook returns the debounced value after a specified delay
- Always use AbortController to cancel in-flight requests when new queries are made
- Show appropriate loading states, empty states, and error messages for good UX
- Consider combining debouncing with React Query for production applications
- Implement keyboard navigation (arrow keys, Enter, Escape) for accessibility
- Highlight matching text in search results for better user feedback

## What's Next

Continue to [Form Accessibility](/07-forms/03-form-patterns/03-form-accessibility.md) to learn how to make your forms accessible to all users using ARIA attributes, proper labeling, focus management, and keyboard navigation patterns.