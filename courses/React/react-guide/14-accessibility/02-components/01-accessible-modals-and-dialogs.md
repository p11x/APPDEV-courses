# Accessible Modals and Dialogs

## Overview
Modals are overlays that require user interaction while blocking the main content. Making them accessible requires focus management, keyboard navigation, and proper ARIA attributes.

## Prerequisites
- ARIA basics
- React component knowledge

## Core Concepts

### Modal Component with Focus Management

```tsx
// [File: src/components/Modal/AccessibleModal.tsx]
import React, { useEffect, useRef, useCallback } from 'react';
import { createPortal } from 'react-dom';

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
}

export function AccessibleModal({ isOpen, onClose, title, children }: ModalProps) {
  const modalRef = useRef<HTMLDivElement>(null);
  const previousFocusRef = useRef<HTMLElement | null>(null);

  // Store previously focused element
  useEffect(() => {
    if (isOpen) {
      previousFocusRef.current = document.activeElement as HTMLElement;
      // Focus the modal when opened
      modalRef.current?.focus();
    } else {
      // Return focus to previous element when closed
      previousFocusRef.current?.focus();
    }
  }, [isOpen]);

  // Handle escape key
  const handleKeyDown = useCallback((e: KeyboardEvent) => {
    if (e.key === 'Escape') {
      onClose();
    }
  }, [onClose]);

  useEffect(() => {
    if (isOpen) {
      document.addEventListener('keydown', handleKeyDown);
      // Trap focus within modal
      document.body.style.overflow = 'hidden';
    }
    return () => {
      document.removeEventListener('keydown', handleKeyDown);
      document.body.style.overflow = '';
    };
  }, [isOpen, handleKeyDown]);

  if (!isOpen) return null;

  return createPortal(
    <div 
      role="dialog"
      aria-modal="true"
      aria-labelledby="modal-title"
      className="modal-overlay"
      onClick={onClose}
    >
      <div 
        ref={modalRef}
        className="modal-content"
        tabIndex={-1}
        onClick={(e) => e.stopPropagation()}
      >
        <h2 id="modal-title">{title}</h2>
        <button onClick={onClose} aria-label="Close modal">
          ✕
        </button>
        {children}
      </div>
    </div>,
    document.body
  );
}
```

### Usage

```tsx
// [File: src/App.tsx]
import { useState } from 'react';
import { AccessibleModal } from './components/Modal/AccessibleModal';

function App() {
  const [isModalOpen, setIsModalOpen] = useState(false);

  return (
    <div>
      <button onClick={() => setIsModalOpen(true)}>
        Open Modal
      </button>

      <AccessibleModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Confirm Action"
      >
        <p>Are you sure you want to proceed?</p>
        <button onClick={() => setIsModalOpen(false)}>Cancel</button>
        <button onClick={() => setIsModalOpen(false)}>Confirm</button>
      </AccessibleModal>
    </div>
  );
}
```

## Key Takeaways
- Use role="dialog" and aria-modal="true"
- Focus must move to modal on open
- Escape key should close modal

## What's Next
Continue to [Accessible Forms](02-accessible-forms-and-inputs.md) for form accessibility.