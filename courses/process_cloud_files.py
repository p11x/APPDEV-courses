import os
import re
import json

BASE_DIR = r"C:\Users\p11x\Desktop\Git Repo\courses\Cloud Computing"

def extract_yaml_metadata(file_path):
    """Extract YAML frontmatter from a markdown file."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
            lines = content.split('\n')
            line_count = len(lines)
            
            # Check for YAML frontmatter
            if content.startswith('---'):
                # Find the closing ---
                yaml_content = []
                start_found = False
                for line in lines:
                    if line.strip() == '---':
                        if not start_found:
                            start_found = True
                            continue
                        else:
                            break
                    if start_found:
                        yaml_content.append(line)
                
                # Parse YAML-like content - handle multiple key case variations
                metadata = {}
                for line in yaml_content:
                    match = re.match(r'^(\w+):\s*(.*)$', line)
                    if match:
                        key = match.group(1).strip()
                        value = match.group(2).strip()
                        # Normalize key to lowercase for consistent lookup
                        key_lower = key.lower()
                        # Map various YAML key names to our standard fields
                        if key_lower in ['concept', 'title']:
                            metadata['Concept'] = value
                        elif key_lower == 'difficulty':
                            metadata['Difficulty'] = value
                        elif key_lower in ['certificationexam', 'certification_exam', 'certexam', 'exam']:
                            metadata['CertificationExam'] = value
                        elif key_lower in ['purpose', 'description']:
                            metadata['Purpose'] = value
                        else:
                            # Store as-is for other keys
                            metadata[key] = value
                
                return metadata, line_count, lines
            else:
                # No YAML, try to extract concept from first heading
                concept = None
                for line in lines[1:10]:  # Check first few lines
                    if line.startswith('# '):
                        concept = line.replace('# ', '').strip()
                        break
                
                return {'concept': concept or os.path.basename(file_path).replace('.md', '')}, line_count, lines
    except Exception as e:
        return {'error': str(e)}, 0, []

def get_relative_path(full_path):
    """Get relative path from Cloud Computing folder."""
    return full_path.replace(BASE_DIR + '\\', '').replace('\\', '/')

def process_all_files():
    """Process all markdown files and create JSON array."""
    results = []
    
    for root, dirs, files in os.walk(BASE_DIR):
        for file in files:
            if file.endswith('.md'):
                file_path = os.path.join(root, file)
                metadata, line_count, lines = extract_yaml_metadata(file_path)
                
                relative_path = get_relative_path(file_path)
                
                entry = {
                    'path': relative_path,
                    'concept': metadata.get('Concept', ''),
                    'difficulty': metadata.get('Difficulty', ''),
                    'certExam': metadata.get('CertificationExam', ''),
                    'lines': line_count
                }
                
                # Fallback: extract concept from first heading if missing
                if not entry['concept']:
                    for line in lines[1:10]:  # Check first few lines
                        if line.startswith('# '):
                            entry['concept'] = line.replace('# ', '').strip()
                            break
                
                results.append(entry)
    
    # Sort by path
    results.sort(key=lambda x: x['path'])
    
    return results

if __name__ == '__main__':
    results = process_all_files()
    
    # Save to file (use forward slashes for path)
    output_file = r"C:\Users\p11x\Desktop\Git Repo\courses\consolidated_files.json"
    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(results, f, indent=2)
    
    print(f"Saved {len(results)} entries to {output_file}")