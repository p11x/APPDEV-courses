#!/bin/bash

# APPDEV Courses - Mac LaunchAgent Setup Script

echo "🚀 Setting up APPDEV Courses Auto-Commit Watcher as LaunchAgent..."
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

LAUNCHAGENT_DIR="$HOME/Library/LaunchAgents"
LAUNCHAGENT_FILE="$LAUNCHAGENT_DIR/com.appdev.autocommit.plist"

echo ""
echo "📝 Creating LaunchAgent plist file..."

# Create LaunchAgents directory if it doesn't exist
mkdir -p "$LAUNCHAGENT_DIR"

# Create the plist file
cat > "$LAUNCHAGENT_FILE" << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>Label</key>
    <string>com.appdev.autocommit</string>
    <key>ProgramArguments</key>
    <array>
        <string>${PYTHON_BIN}</string>
        <string>${REPO_PATH}/auto_commit.py</string>
    </array>
    <key>WorkingDirectory</key>
    <string>${REPO_PATH}</string>
    <key>RunAtLoad</key>
    <true/>
    <key>KeepAlive</key>
    <true/>
    <key>StandardOutPath</key>
    <string>${REPO_PATH}/auto_commit.log</string>
    <key>StandardErrorPath</key>
    <string>${REPO_PATH}/auto_commit.log</string>
    <key>ProcessType</key>
    <string>Background</string>
</dict>
</plist>
EOF

echo "✅ LaunchAgent plist created at: $LAUNCHAGENT_FILE"

# Load the LaunchAgent
echo ""
echo "▶️  Loading LaunchAgent..."
launchctl load "$LAUNCHAGENT_FILE"

# Verify it's loaded
if launchctl list | grep -q "com.appdev.autocommit"; then
    echo ""
    echo "✅ LaunchAgent registered" -ForegroundColor Green
    echo "🔁 Auto-starts on every login" -ForegroundColor Cyan
    echo "📄 Logs: ${REPO_PATH}/auto_commit.log"
    echo ""
    echo "Useful commands:"
    echo "  - Status: launchctl list | grep appdev"
    echo "  - Stop: launchctl stop com.appdev.autocommit"
    echo "  - Start: launchctl start com.appdev.autocommit"
    echo "  - Remove: bash remove_mac_service.sh"
else
    echo "❌ Failed to load LaunchAgent"
    exit 1
fi
