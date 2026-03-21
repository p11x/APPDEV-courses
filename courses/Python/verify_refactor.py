#!/usr/bin/env python3
"""Verify refactored files meet the requirements."""

from pathlib import Path

EXAMPLES_DIR = Path("Python_Pratice")

# Files that were skipped (not refactored)
SKIPPED_FILES = {"Example15.py"}

issues = []

for path in sorted(EXAMPLES_DIR.glob("Example*.py")):
    # Skip files that weren't refactored
    if path.name in SKIPPED_FILES:
        continue
        
    content = path.read_text(encoding="utf-8")
    lines = content.splitlines()
    
    # Check for main() function definition at top level
    has_main = any(line.strip().startswith('def main()') for line in lines)
    
    # Check for comment banners with === (not inside strings)
    has_banners = any(
        line.strip().startswith('#') and '===' in line and line.strip().endswith('===')
        for line in lines
    )
    
    # Check for if __name__ block
    has_name_block = any('if __name__' in line for line in lines)
    
    # Check for header
    has_header = len(lines) > 0 and lines[0].startswith("# Example")
    
    # Check for topic line
    has_topic = len(lines) > 1 and lines[1].startswith("# Topic:")
    
    checks = {
        "no main()": not has_main,
        "no comment banners": not has_banners,
        "has header": has_header,
        "has topic line": has_topic,
        "no if __name__": not has_name_block,
    }
    
    failed = [k for k, v in checks.items() if not v]
    if failed:
        issues.append((path.name, failed))

if issues:
    print("Files with issues:")
    for name, fails in issues:
        print(f"  {name}: {', '.join(fails)}")
else:
    print("All refactored files passed verification!")
    
print(f"\nSkipped files (require manual refactoring): {', '.join(sorted(SKIPPED_FILES))}")
print("\nNote: Complex f-strings with nested brackets were intentionally left as-is.")
