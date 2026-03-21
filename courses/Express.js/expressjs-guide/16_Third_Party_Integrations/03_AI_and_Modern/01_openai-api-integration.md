# OpenAI API Integration

## 📌 What You'll Learn

- Using OpenAI SDK
- Chat completions endpoint

## 💻 Code Example

```js
import OpenAI from 'openai';

const openai = new OpenAI({ apiKey: process.env.OPENAI_API_KEY });

app.post('/chat', async (req, res) => {
  const { message } = req.body;
  
  const completion = await openai.chat.completions.create({
    model: 'gpt-4',
    messages: [{ role: 'user', content: message }]
  });
  
  res.json({ reply: completion.choices[0].message.content });
});
```
