# AWS Deployment

## What You'll Learn

- Deploying Node.js to AWS EC2
- Using AWS Elastic Beanstalk
- Environment variable management

## EC2 Deployment

```bash
# 1. SSH into your EC2 instance
ssh -i key.pem ec2-user@your-instance-ip

# 2. Install Node.js
curl -fsSL https://rpm.nodesource.com/setup_20.x | sudo bash -
sudo yum install -y nodejs git

# 3. Clone and setup
git clone https://github.com/your/repo.git
cd repo
npm ci --only=production

# 4. Start with PM2
npm install -g pm2
pm2 start ecosystem.config.js --env production
pm2 save
pm2 startup
```

## Next Steps

For Vercel, continue to [Vercel Deploy](./02-vercel-deploy.md).
