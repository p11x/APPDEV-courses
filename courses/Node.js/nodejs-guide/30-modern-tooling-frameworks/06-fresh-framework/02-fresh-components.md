# Fresh Components

## What You'll Learn

- How Fresh components work
- Server components vs islands
- How to pass data to components
- How to use Preact signals

## Server Components

Components in `components/` are rendered on the server and ship zero JavaScript:

```tsx
// components/Header.tsx

export default function Header({ title }: { title: string }) {
  return (
    <header>
      <h1>{title}</h1>
      <nav>
        <a href="/">Home</a>
        <a href="/about">About</a>
      </nav>
    </header>
  );
}
```

## Layouts

```tsx
// routes/_app.tsx — Global layout

import { AppProps } from '$fresh/server.ts';
import Header from '../components/Header.tsx';

export default function App({ Component }: AppProps) {
  return (
    <html>
      <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
      </head>
      <body>
        <Header title="My Fresh App" />
        <main>
          <Component />  {/* Page content renders here */}
        </main>
      </body>
    </html>
  );
}
```

## Preact Signals

```tsx
// islands/SignalCounter.tsx

import { signal } from '@preact/signals';

// Signals work in islands — reactive without full re-render
const count = signal(0);

export default function SignalCounter() {
  return (
    <div>
      <p>Count: {count.value}</p>
      <button onClick={() => count.value++}>Increment</button>
    </div>
  );
}
```

## Form Handling

```tsx
// islands/ContactForm.tsx

import { useState } from 'preact/hooks';

export default function ContactForm() {
  const [status, setStatus] = useState<'idle' | 'sending' | 'sent'>('idle');

  async function handleSubmit(e: Event) {
    e.preventDefault();
    setStatus('sending');

    const form = new FormData(e.target as HTMLFormElement);

    await fetch('/api/contact', {
      method: 'POST',
      body: form,
    });

    setStatus('sent');
  }

  return (
    <form onSubmit={handleSubmit}>
      <input name="email" type="email" placeholder="Email" required />
      <textarea name="message" placeholder="Message" required />
      <button type="submit" disabled={status === 'sending'}>
        {status === 'sending' ? 'Sending...' : status === 'sent' ? 'Sent!' : 'Send'}
      </button>
    </form>
  );
}
```

## Next Steps

For deployment, continue to [Fresh Deployment](./03-fresh-deployment.md).
