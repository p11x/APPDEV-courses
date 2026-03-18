# Deploy to Render

## What You'll Learn
- Deploying to Render.com
- Setting up environment variables

## Prerequisites
- Completed Docker deployment

## Steps

1. Push code to GitHub
2. Create account on render.com
3. Create new Web Service
4. Connect GitHub repository
5. Set environment variables

## Settings

- Build Command: `pip install -r requirements.txt`
- Start Command: `uvicorn main:app --host 0.0.0.0 --port 8000`

## Summary
- Connect GitHub to Render
- Set environment variables in dashboard
- Automatic deployments on push
