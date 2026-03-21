# Emotion Setup in Next.js

## Installation

```bash
npm install @emotion/react @emotion/styled
```

## Usage

```typescript
/** @jsxImportSource @emotion/react */
import { css } from "@emotion/react";

const style = css`
  padding: 1rem;
  background: blue;
`;

export default function Page() {
  return <div css={style}>Hello</div>;
}
```

Note: Requires "use client" for best SSR support.
