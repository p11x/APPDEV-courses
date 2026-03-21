# Styled Components with Server Components

## Setup

```bash
npm install styled-components
```

```typescript
// next.config.js
const nextConfig = {
  compiler: {
    styledComponents: true,
  },
};

module.exports = nextConfig;
```

## Usage

```typescript
// src/app/page.tsx
"use client";

import styled from "styled-components";

const Container = styled.div`
  padding: 2rem;
`;

const Title = styled.h1`
  font-size: 2rem;
  color: #333;
`;

export default function Page() {
  return (
    <Container>
      <Title>Hello</Title>
    </Container>
  );
}
```

Note: Styled Components require "use client" directive.
