# Node.js Server Deployment

## Build for Production

```bash
npm run build
```

## Start Production Server

```bash
npm start
```

The app runs on port 3000 by default.

## PM2 for Production

```bash
npm install -g pm2
pm2 start npm --name "nextjs" -- start
```
