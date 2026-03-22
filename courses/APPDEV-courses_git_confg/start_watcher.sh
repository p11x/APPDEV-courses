#!/bin/bash

# APPDEV Courses Auto-Commit Watcher Startup Script (Mac/Linux)

echo "🚀 Starting APPDEV Courses Auto-Commit Watcher..."
echo ""

# Check if virtual environment exists
if [ -d "venv" ]; then
    echo "📦 Activating virtual environment..."
    source venv/bin/activate
fi

# Install requirements
echo "📥 Installing dependencies..."
pip install -r requirements.txt

# Check if installation was successful
if [ $? -eq 0 ]; then
    echo "✅ Dependencies installed successfully"
else
    echo "❌ Failed to install dependencies"
    exit 1
fi

echo ""
echo "▶️  Starting file watcher..."
echo "Press Ctrl+C to stop"
echo ""

# Run the watcher
python auto_commit.py
