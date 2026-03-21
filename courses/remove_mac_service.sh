#!/bin/bash

# APPDEV Courses - Mac LaunchAgent Removal Script

LAUNCHAGENT_DIR="$HOME/Library/LaunchAgents"
LAUNCHAGENT_FILE="$LAUNCHAGENT_DIR/com.appdev.autocommit.plist"

echo "🗑️ Removing APPDEV Courses Auto-Commit Watcher LaunchAgent..."
echo ""

# Unload the LaunchAgent
echo "⏹️  Unloading LaunchAgent..."
launchctl unload "$LAUNCHAGENT_FILE" 2>/dev/null
echo "✅ LaunchAgent unloaded"

# Remove the plist file
echo ""
echo "🗑️ Removing LaunchAgent file..."
if [ -f "$LAUNCHAGENT_FILE" ]; then
    rm -f "$LAUNCHAGENT_FILE"
    echo "✅ LaunchAgent file removed"
else
    echo "LaunchAgent file not found"
fi

echo ""
echo "🗑️ LaunchAgent removed" -ForegroundColor Green
echo ""
