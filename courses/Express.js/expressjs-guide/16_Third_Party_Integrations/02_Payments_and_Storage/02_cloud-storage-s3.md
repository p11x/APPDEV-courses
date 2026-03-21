# Cloud Storage S3

## 📌 What You'll Learn

- Uploading to AWS S3
- Presigned URLs

## 💻 Code Example

```js
import { S3Client, PutObjectCommand } from '@aws-sdk/client-s3';

const s3 = new S3Client({});

app.post('/upload', express.raw({ type: 'application/octet-stream' }), async (req, res) => {
  const key = `uploads/${Date.now()}-file`;
  
  await s3.send(new PutObjectCommand({
    Bucket: process.env.S3_BUCKET,
    Key: key,
    Body: req.body
  }));
  
  res.json({ url: `https://${process.env.S3_BUCKET}.s3.amazonaws.com/${key}` });
});
```
