#!/bin/bash

# APPDEV Courses - Linux systemd Service Setup Script
# Run with sudo

echo "🚀 Setting up APPDEV Courses Auto-Commit Watcher as systemd service..."
echo ""

# Detect current user
CURRENT_USER=$(whoami)
echo "👤 Current user: $CURRENT_USER"

# Get full path to the repo
REPO_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
echo "📁 Repository path: $REPO_PATH"

# Find python3
PYTHON_BIN=$(which python3 2>/dev/null || which python 2>/dev/null)
if [ -z "$PYTHON_BIN" ]; then
    echo "❌ Python not found. Please install Python 3 first."
    exit 1
fi
echo "🐍 Python binary: $PYTHON_BIN"

SERVICE_NAME="appdev-autocommit"
SERVICE_FILE="/etc/systemd/system/${SERVICE_NAME}.service"

echo ""
echo "📝 Creating systemd service file..."

# Create the service file
cat > "$SERVICE_FILE" << EOF
[Unit]
Description=APPDEV Courses Auto Commit Watcher
After=network.target

[Service]
Type=simple
User=${CURRENT_USER}
WorkingDirectory=${REPO_PATH}
ExecStart=${PYTHON_BIN} ${REPO_PATH}/auto_commit.py
Restart=on-failure
RestartSec=10
StandardOutput=append:${REPO_PATH}/auto_commit.log
StandardError=append:${REPO_PATH}/auto_commit.log

[Install]
WantedBy=multi-user.target
EOF

echo "✅ Service file created at: $SERVICE_FILE"

# Reload systemd daemon
echo ""
echo "🔄 Reloading systemd daemon..."
sudo systemctl daemon-reload

# Enable and start the service
echo ""
echo "▶️  Enabling and starting service..."
sudo systemctl enable ${SERVICE_NAME}
sudo systemctl start ${SERVICE_NAME}

# Check status
if sudo systemctl is-active --quiet ${SERVICE_NAME}; then
    echo ""
    echo "✅ Service enabled and started" -ForegroundColor Green
    echo "🔁 Auto-starts on every boot" -ForegroundColor Cyan
    echo "📄 Logs: ${REPO_PATH}/auto_commit.log"
    echo ""
    echo "Useful commands:"
    echo "  - Status: sudo systemctl status ${SERVICE_NAME}"
    echo "  - Logs: sudo journalctl -u ${SERVICE_NAME} -f"
    echo "  - Stop: sudo systemctl stop ${SERVICE_NAME}"
    echo "  - Remove: bash remove_linux_service.sh"
else
    echo "❌ Service failed to start. Check logs with:"
    echo "   sudo journalctl -u ${SERVICE_NAME} -n 50"
    exit 1
fi
