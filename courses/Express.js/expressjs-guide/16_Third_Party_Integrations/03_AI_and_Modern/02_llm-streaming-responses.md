# LLM Streaming Responses

## 📌 What You'll Learn

- Streaming LLM output via SSE
- Handling abort
- Backpressure

## 💻 Code Example

```js
import OpenAI from 'openai';

const openai = new OpenAI();

app.get('/stream', async (req, res) => {
  res.setHeader('Content-Type', 'text/event-stream');
  
  const stream = await openai.chat.completions.create({
    model: 'gpt-4',
    messages: [{ role: 'user', content: 'Tell me a story' }],
    stream: true
  });
  
  for await (const chunk of stream) {
    if (res.writableEnded) break;
    res.write(`data: ${chunk.choices[0]?.delta?.content || ''}\n\n`);
  }
  
  res.end();
});
```
