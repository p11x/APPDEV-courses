#!/usr/bin/env python3
"""
Refactor all ExampleN.py files to the new coding style.
"""

import re
import shutil
import subprocess
import sys
import tempfile
from pathlib import Path

# Configuration
EXAMPLES_DIR = Path("Python_Pratice")
BACKUP_DIR = EXAMPLES_DIR / "_backups"


def has_nested_functions(content: str) -> bool:
    """Check if file has nested functions inside main()."""
    lines = content.splitlines()
    in_main = False
    main_indent = 0
    
    for line in lines:
        if re.match(r'^def main\(\)\s*->\s*None\s*:', line):
            in_main = True
            continue
        
        if in_main:
            if 'if __name__' in line:
                break
            if line.strip() and not line.startswith(' ') and not line.startswith('\t'):
                break
            
            # Check for nested function definition
            if line.strip().startswith('def '):
                indent = len(line) - len(line.lstrip())
                if indent > 0:  # Has indentation, is nested
                    return True
    
    return False


def split_lines_preserving_multiline(content: str) -> list:
    """Split content into lines, keeping multi-line strings intact."""
    lines = []
    current_line = ""
    in_triple_quote = False
    triple_char = ""
    i = 0
    
    while i < len(content):
        char = content[i]
        
        if not in_triple_quote:
            if content[i:i+3] in ['"""', "'''"]:
                current_line += content[i:i+3]
                triple_char = content[i:i+3]
                in_triple_quote = True
                i += 3
                continue
        else:
            current_line += char
            if i >= 2 and content[i-2:i+1] == triple_char:
                in_triple_quote = False
                lines.append(current_line)
                current_line = ""
                i += 1
                continue
            i += 1
            continue
        
        if char == '\n' and not in_triple_quote:
            lines.append(current_line)
            current_line = ""
        else:
            current_line += char
        i += 1
    
    if current_line:
        lines.append(current_line)
    
    return lines


def is_inside_string(line: str, pos: int) -> bool:
    """Check if position pos in line is inside a string literal."""
    in_single = False
    in_double = False
    i = 0
    while i < pos:
        c = line[i]
        if c == '\\' and i + 1 < len(line):
            i += 2
            continue
        if c == "'" and not in_double:
            in_single = not in_single
        elif c == '"' and not in_single:
            in_double = not in_double
        i += 1
    return in_single or in_double


def find_fstring_positions(line: str) -> list:
    """Find positions of actual f-strings in a line."""
    positions = []
    i = 0
    
    while i < len(line):
        if line[i:i+2] in ['f"', "f'"]:
            if is_inside_string(line, i):
                i += 1
                continue
            
            if i == 0 or line[i-1] in '=([{,: ':
                quote = line[i:i+2]
                start = i
                i += 2
                quote_char = quote[1]
                while i < len(line):
                    if line[i] == '\\':
                        i += 2
                        continue
                    if line[i] == quote_char:
                        if i > 0 and line[i-1] == '\\':
                            i += 1
                            continue
                        positions.append((start, i + 1))
                        break
                    i += 1
            else:
                i += 1
        else:
            i += 1
    
    return positions


def is_complex_fstring_in_line(line: str) -> bool:
    """Check if any f-string in the line is complex."""
    positions = find_fstring_positions(line)
    
    for start, end in positions:
        fstring = line[start:end]
        
        if '"""' in fstring or "'''" in fstring:
            return True
        
        content = fstring[2:-1]
        in_brace = False
        brace_depth = 0
        for c in content:
            if c == '{':
                brace_depth += 1
                in_brace = True
            elif c == '}':
                brace_depth -= 1
                if brace_depth == 0:
                    in_brace = False
            elif in_brace and c in '[]()':
                return True
        
        if '{{' in fstring or '}}' in fstring:
            return True
    
    return False


def convert_fstring_in_line(line: str) -> str:
    """Convert simple f-strings in a line."""
    positions = find_fstring_positions(line)
    
    if not positions:
        return line
    
    result = line
    for start, end in reversed(positions):
        fstring = result[start:end]
        
        if '"""' in fstring or "'''" in fstring:
            continue
        
        content = fstring[2:-1]
        
        has_brackets = False
        in_brace = False
        brace_depth = 0
        for c in content:
            if c == '{':
                brace_depth += 1
                in_brace = True
            elif c == '}':
                brace_depth -= 1
                if brace_depth == 0:
                    in_brace = False
            elif in_brace and c in '[]()':
                has_brackets = True
                break
        
        if has_brackets or '{{' in fstring or '}}' in fstring:
            continue
        
        parts = []
        current_text = ""
        in_expr = False
        brace_depth = 0
        expr_content = ""
        
        for char in content:
            if char == '{' and not in_expr:
                if current_text:
                    parts.append(f'"{current_text}"')
                    current_text = ""
                in_expr = True
                expr_content = ""
                brace_depth = 1
            elif char == '{' and in_expr:
                expr_content += char
                brace_depth += 1
            elif char == '}' and in_expr:
                brace_depth -= 1
                if brace_depth == 0:
                    if ':' in expr_content:
                        var_part = expr_content.split(':')[0]
                        parts.append(f"str({var_part})")
                    else:
                        parts.append(f"str({expr_content})")
                    in_expr = False
                    expr_content = ""
                else:
                    expr_content += char
            elif in_expr:
                expr_content += char
            else:
                current_text += char
        
        if current_text:
            parts.append(f'"{current_text}"')
        
        if len(parts) == 1:
            converted = parts[0]
        elif len(parts) == 0:
            converted = '""'
        else:
            converted = " + ".join(parts)
        
        result = result[:start] + converted + result[end:]
    
    return result


def refactor(content: str, filename: str) -> str:
    """Apply all transformation rules."""
    lines = split_lines_preserving_multiline(content)
    
    example_num = re.search(r'Example(\d+)\.py', filename)
    example_id = example_num.group(1) if example_num else "?"
    
    topic_match = re.search(r'# Topic:\s*(.+)', content)
    topic = topic_match.group(1).strip() if topic_match else "Topic"
    
    new_lines = []
    new_lines.append(f"# Example{example_id}.py")
    new_lines.append(f"# Topic: {topic}")
    new_lines.append("")
    
    # Find and extract main() content
    main_start = -1
    main_end = -1
    in_main = False
    
    for i, line in enumerate(lines):
        if re.match(r'^def main\(\)\s*->\s*None\s*:', line):
            main_start = i + 1
            in_main = True
            continue
        
        if in_main:
            if 'if __name__' in line:
                main_end = i
                break
            if line.strip() and not line.startswith(' ') and not line.startswith('\t'):
                main_end = i
                break
    
    if main_end == -1:
        main_end = len(lines)
    
    if main_start != -1:
        main_content = lines[main_start:main_end]
        
        if main_content and ('"""' in main_content[0] or "'''" in main_content[0]):
            docstring_type = '"""' if '"""' in main_content[0] else "'''"
            for j in range(1, len(main_content)):
                if docstring_type in main_content[j]:
                    main_content = main_content[j+1:]
                    break
        
        if main_content:
            min_indent = float('inf')
            for line in main_content:
                if line.strip():
                    if line.strip().startswith('def '):
                        continue
                    indent = len(line) - len(line.lstrip())
                    min_indent = min(min_indent, indent)
            
            if min_indent == float('inf'):
                min_indent = 0
            
            if min_indent > 0:
                dedented = []
                for line in main_content:
                    if line.strip():
                        if line.strip().startswith('def '):
                            current_indent = len(line) - len(line.lstrip())
                            extra = current_indent - min_indent
                            if extra > 0:
                                dedented.append(line[extra:])
                            else:
                                dedented.append(line)
                        else:
                            dedented.append(line[min_indent:])
                    else:
                        dedented.append(line)
                main_content = dedented
    else:
        main_content = []
        skip_header = False
        for line in lines:
            if line.startswith('# Example'):
                skip_header = True
                continue
            if skip_header and line.startswith('# Topic:'):
                continue
            if skip_header and not line.strip():
                continue
            if skip_header and line.strip() and not line.startswith('#'):
                skip_header = False
            if not skip_header:
                main_content.append(line)
    
    processed_lines = []
    i = 0
    while i < len(main_content):
        line = main_content[i]
        
        if 'if __name__' in line:
            break
        
        if re.match(r'^#\s*===.*===', line):
            section_name = re.sub(r'^#\s*===', '', line).replace('===', '').strip()
            section_name = section_name.title().replace('_', ' ')
            processed_lines.append(f"# {section_name}")
            i += 1
            continue
        
        if not line.strip() and i + 1 < len(main_content):
            next_line = main_content[i + 1]
            if re.match(r'^#\s*===.*===', next_line):
                i += 1
                continue
        
        if 'f"' in line or "f'" in line:
            if not is_complex_fstring_in_line(line):
                line = convert_fstring_in_line(line)
        
        processed_lines.append(line)
        i += 1
    
    main_content = '\n'.join(processed_lines)
    lines = main_content.splitlines()
    
    new_lines_processed = []
    for line in lines:
        if re.match(r'^def\s+\w+.*:\s*$', line):
            new_lines_processed.append(line)
            continue
        
        match = re.match(r'^(\s*)(\w+)\s*:\s*(\w+)\s*=\s*(.+)$', line)
        if match:
            indent, var_name, var_type, value = match.groups()
            type_comment = ""
            if var_type == 'str':
                type_comment = ' # str  — text, always wrapped in quotes'
            elif var_type == 'int':
                type_comment = ' # int  — whole number, no quotes'
            elif var_type == 'float':
                type_comment = ' # float — decimal number'
            elif var_type == 'bool':
                type_comment = ' # bool — can only be True or False'
            
            new_line = f"{indent}{var_name} = {value}{type_comment}"
            new_lines_processed.append(new_line)
        else:
            new_lines_processed.append(line)
    
    final_lines = []
    
    for line in new_lines_processed:
        stripped = line.strip()
        
        if stripped.startswith('#') and not stripped.startswith('# '):
            section_text = stripped.lstrip('#').strip()
            section_text = section_text.title().replace('_', ' ')
            
            if 'creat' in section_text.lower():
                final_lines.append("# Creating — define a variable with a name and value")
            elif 'read' in section_text.lower():
                final_lines.append("# Reading — use the variable's value anywhere")
            elif 'chang' in section_text.lower():
                final_lines.append("# Changing — assign a new value to replace the old one")
            elif 'practical' in section_text.lower() or 'example' in section_text.lower():
                final_lines.append("# Real-world example")
            elif 'bad' in section_text.lower():
                final_lines.append("# Naming styles to avoid")
            elif 'good' in section_text.lower():
                final_lines.append("# Recommended naming convention")
            elif 'keyword' in section_text.lower():
                final_lines.append("# Reserved words that cannot be variable names")
            
            final_lines.append(f"# {section_text}")
            continue
        
        if stripped.startswith('#') and len(stripped) > 1 and stripped[1] == '-':
            continue
        
        if stripped.startswith('print('):
            print_match = re.match(r'print\((.+)\)$', stripped)
            if print_match:
                print_arg = print_match.group(1).strip()
                
                if print_arg.startswith('"') and print_arg.endswith('"'):
                    output_val = print_arg[1:-1]
                elif print_arg.startswith("'") and print_arg.endswith("'"):
                    output_val = print_arg[1:-1]
                else:
                    output_val = None
                
                if output_val is not None:
                    padding = " " * (50 - len(stripped))
                    final_lines.append(f"{line}{padding}# {output_val}")
                else:
                    final_lines.append(line)
            else:
                final_lines.append(line)
        else:
            final_lines.append(line)
    
    has_realworld = any('real-world' in line.lower() or 'real world' in line.lower() 
                        for line in final_lines)
    
    if not has_realworld:
        final_lines.append("")
        final_lines.append("# Real-world example:")
    
    result = '\n'.join(new_lines) + '\n' + '\n'.join(final_lines)
    result = re.sub(r'\n{3,}', '\n\n', result)
    
    return result


def validate_syntax(code: str) -> tuple:
    """Validate Python syntax."""
    try:
        with tempfile.NamedTemporaryFile(mode='w', suffix='.py', delete=False, encoding='utf-8') as f:
            f.write(code)
            temp_path = f.name
        
        result = subprocess.run(
            [sys.executable, "-m", "py_compile", temp_path],
            capture_output=True,
            text=True
        )
        
        try:
            Path(temp_path).unlink()
        except:
            pass
        
        if result.returncode != 0:
            return False, result.stderr
        return True, ""
    except Exception as e:
        return False, str(e)


def main():
    """Main function to refactor all Example files."""
    BACKUP_DIR.mkdir(exist_ok=True)
    
    files = sorted(EXAMPLES_DIR.glob("Example*.py"))
    print(f"Found {len(files)} Example files.\n")
    
    ok, failed = 0, 0
    
    for path in files:
        try:
            # Check for nested functions - skip if found
            content = path.read_text(encoding="utf-8")
            if has_nested_functions(content):
                print(f"[SKIP] {path.name} — has nested functions (requires manual refactoring)")
                continue
            
            backup = BACKUP_DIR / (path.name + ".bak")
            shutil.copy2(path, backup)
            
            refactored = refactor(content, path.name)
            
            valid, error = validate_syntax(refactored)
            if not valid:
                raise SyntaxError(error)
            
            path.write_text(refactored, encoding="utf-8")
            print(f"[OK] Refactored: {path.name}")
            ok += 1
            
        except Exception as e:
            print(f"[FAIL] Error: {path.name} — {e}")
            failed += 1
    
    print(f"\nDone. {ok} refactored, {failed} failed.")
    print(f"Backups saved to: {BACKUP_DIR}")


if __name__ == "__main__":
    main()
