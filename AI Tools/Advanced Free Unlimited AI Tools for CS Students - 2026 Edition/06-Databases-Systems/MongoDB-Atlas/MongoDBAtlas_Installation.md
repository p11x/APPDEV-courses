# MongoDB Atlas - Installation

## Browser-Based Setup

1. Visit [mongodb.com/atlas](https://www.mongodb.com/atlas)
2. Click "Start Free" button
3. Create MongoDB account (use .edu email for benefits)
4. Choose "Build a Database" (Free tier)
5. Select AWS as provider (free tier available)
6. Choose nearest region
7. Create cluster name
8. Wait for deployment (2-3 minutes)

## Connect to Cluster

1. Create database user with username/password
2. Network access: Add IP 0.0.0.0/0 for anywhere access
3. Click "Connect" button
4. Choose connection method:
   - MongoDB Shell
   - Compass
   - Drivers

## Install MongoDB Compass (Optional GUI)

```bash
# Windows
Download from mongodb.com//products/compass

# macOS
brew install mongodb-community-shell

# Ubuntu
sudo apt install mongodb-compass
```

---

*Back to [Databases & Systems README](../README.md)*