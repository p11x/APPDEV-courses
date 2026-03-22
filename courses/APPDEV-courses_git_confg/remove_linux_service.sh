#!/bin/bash

# APPDEV Courses - Linux systemd Service Removal Script
# Run with sudo

SERVICE_NAME="appdev-autocommit"
SERVICE_FILE="/etc/systemd/system/${SERVICE_NAME}.service"

echo "🗑️ Removing APPDEV Courses Auto-Commit Watcher service..."
echo ""

# Stop the service
echo "⏹️  Stopping service..."
sudo systemctl stop ${SERVICE_NAME} 2>/dev/null
echo "✅ Service stopped"

# Disable the service
echo ""
echo "📦 Disabling service..."
sudo systemctl disable ${SERVICE_NAME} 2>/dev/null
echo "✅ Service disabled"

# Remove the service file
echo ""
echo "🗑️ Removing service file..."
if [ -f "$SERVICE_FILE" ]; then
    sudo rm -f "$SERVICE_FILE"
    echo "✅ Service file removed"
else
    echo "Service file not found"
fi

# Reload systemd
echo ""
echo "🔄 Reloading systemd daemon..."
sudo systemctl daemon-reload

echo ""
echo "🗑️ Service removed from systemd" -ForegroundColor Green
echo ""
