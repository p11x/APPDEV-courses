# Next.js Markdown Rendering

## What You'll Learn

- How to render AI-generated markdown
- How to add syntax highlighting
- How to stream markdown content
- How to handle code blocks

## Setup

```bash
npm install react-markdown remark-gfm react-syntax-highlighter
```

## Markdown Component

```tsx
// components/Markdown.tsx

import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneDark } from 'react-syntax-highlighter/dist/esm/styles/prism';

interface MarkdownProps {
  content: string;
}

export function Markdown({ content }: MarkdownProps) {
  return (
    <ReactMarkdown
      remarkPlugins={[remarkGfm]}
      components={{
        code({ node, className, children, ...props }) {
          const match = /language-(\w+)/.exec(className || '');

          return match ? (
            <SyntaxHighlighter
              style={oneDark}
              language={match[1]}
              PreTag="div"
            >
              {String(children).replace(/\n$/, '')}
            </SyntaxHighlighter>
          ) : (
            <code className={className} {...props}>
              {children}
            </code>
          );
        },
      }}
    >
      {content}
    </ReactMarkdown>
  );
}
```

## Usage in Chat

```tsx
// components/ChatUI.tsx

import { Markdown } from './Markdown';

// In the message rendering:
<div className="whitespace-pre-wrap">
  <Markdown content={m.content} />
</div>
```

## Next Steps

For prompt engineering, continue to [Prompt Engineering](./04-nextjs-prompt-engineering.md).
