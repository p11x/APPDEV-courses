"""
compile_homework_pdf.py
-----------------------
Compiles all Java files in the Home_Work folder into one PDF.
Each file is shown as:
  - File name header
  - Source code
  - Program output (compiled & run automatically)

Usage:
    python compile_homework_pdf.py

Requirements:
    pip install reportlab

Make sure this script is placed in the SAME directory that contains
your Home_Work/ folder, then run it from that directory.
"""

import os
import re
import shutil
import subprocess
import sys
import tempfile

# Add local Lib path for bundled packages
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
LIB_PATH = os.path.join(os.path.dirname(SCRIPT_DIR), "Lib", "site-packages")
if os.path.exists(LIB_PATH):
    sys.path.insert(0, LIB_PATH)

from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import mm
from reportlab.lib import colors
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, Preformatted,
    HRFlowable, PageBreak, KeepTogether
)
from reportlab.lib.enums import TA_LEFT, TA_CENTER

# ── Configuration ──────────────────────────────────────────────────────────────
SOURCE_FOLDER = "Home_Work"          # folder containing .java files
OUTPUT_PDF    = "Homework_All.pdf"   # output PDF name
PAGE_SIZE     = A4
COMPILE_TIMEOUT = 15                 # seconds per file
RUN_TIMEOUT     = 10                 # seconds per file
# ───────────────────────────────────────────────────────────────────────────────


def sorted_java_files(folder):
    """Return .java files sorted numerically (Home1 < Home2 < ... < Home10)."""
    files = [f for f in os.listdir(folder) if f.endswith(".java")]
    def natural_key(name):
        return [int(c) if c.isdigit() else c.lower()
                for c in re.split(r'(\d+)', name)]
    return sorted(files, key=natural_key)


def compile_and_run(java_file, src_folder):
    """
    Compile and run a Java file in a temp directory.
    Returns (stdout, stderr) strings.
    """
    with tempfile.TemporaryDirectory() as tmpdir:
        # Copy source file
        src_path = os.path.join(src_folder, java_file)
        dst_path = os.path.join(tmpdir, java_file)
        shutil.copy2(src_path, dst_path)

        # Compile
        compile_result = subprocess.run(
            ["javac", java_file],
            cwd=tmpdir,
            capture_output=True,
            text=True,
            timeout=COMPILE_TIMEOUT
        )

        if compile_result.returncode != 0:
            err = compile_result.stderr.strip()
            return None, f"[COMPILATION ERROR]\n{err}"

        # Detect main class name from filename
        class_name = java_file.replace(".java", "")

        # Run
        run_result = subprocess.run(
            ["java", class_name],
            cwd=tmpdir,
            capture_output=True,
            text=True,
            timeout=RUN_TIMEOUT,
            input=""           # provide empty stdin to avoid hanging
        )

        stdout = run_result.stdout.strip()
        stderr = run_result.stderr.strip()

        if not stdout and stderr:
            return None, f"[RUNTIME ERROR]\n{stderr}"
        if not stdout:
            stdout = "(No output)"
        return stdout, None


def build_styles():
    base = getSampleStyleSheet()

    file_header = ParagraphStyle(
        "FileHeader",
        parent=base["Heading1"],
        fontSize=14,
        textColor=colors.HexColor("#1a237e"),
        spaceAfter=4,
        spaceBefore=10,
        fontName="Helvetica-Bold",
    )

    section_label = ParagraphStyle(
        "SectionLabel",
        parent=base["Normal"],
        fontSize=9,
        textColor=colors.white,
        backColor=colors.HexColor("#37474f"),
        fontName="Helvetica-Bold",
        leftIndent=4,
        rightIndent=4,
        spaceBefore=6,
        spaceAfter=2,
    )

    code_style = ParagraphStyle(
        "CodeBlock",
        parent=base["Code"],
        fontSize=8,
        fontName="Courier",
        leading=11,
        leftIndent=6,
        backColor=colors.HexColor("#f5f5f5"),
        borderColor=colors.HexColor("#e0e0e0"),
        borderWidth=0.5,
        borderPadding=4,
    )

    output_style = ParagraphStyle(
        "OutputBlock",
        parent=base["Code"],
        fontSize=8,
        fontName="Courier",
        leading=11,
        leftIndent=6,
        textColor=colors.HexColor("#1b5e20"),
        backColor=colors.HexColor("#f1f8e9"),
        borderColor=colors.HexColor("#c5e1a5"),
        borderWidth=0.5,
        borderPadding=4,
    )

    error_style = ParagraphStyle(
        "ErrorBlock",
        parent=base["Code"],
        fontSize=8,
        fontName="Courier",
        leading=11,
        leftIndent=6,
        textColor=colors.HexColor("#b71c1c"),
        backColor=colors.HexColor("#fce4ec"),
        borderColor=colors.HexColor("#ef9a9a"),
        borderWidth=0.5,
        borderPadding=4,
    )

    return {
        "file_header": file_header,
        "section_label": section_label,
        "code": code_style,
        "output": output_style,
        "error": error_style,
        "normal": base["Normal"],
        "title": base["Title"],
    }


def escape_xml(text):
    """Escape special XML/HTML characters for ReportLab Paragraph."""
    return (text
            .replace("&", "&amp;")
            .replace("<", "&lt;")
            .replace(">", "&gt;"))


def make_pdf(source_folder, output_pdf):
    styles = build_styles()
    story  = []

    # ── Cover title ──
    story.append(Spacer(1, 30 * mm))
    story.append(Paragraph("Java Homework Submission", styles["title"]))
    story.append(Spacer(1, 6 * mm))
    story.append(HRFlowable(width="100%", thickness=1.5,
                             color=colors.HexColor("#1a237e")))
    story.append(Spacer(1, 4 * mm))
    story.append(Paragraph(
        f"Source folder: <b>{source_folder}</b>",
        styles["normal"]))
    story.append(PageBreak())

    java_files = sorted_java_files(source_folder)
    total      = len(java_files)
    print(f"Found {total} Java files. Building PDF …\n")

    for idx, java_file in enumerate(java_files, 1):
        print(f"  [{idx:3d}/{total}]  {java_file}", end="", flush=True)

        # Read source
        src_path = os.path.join(source_folder, java_file)
        with open(src_path, "r", encoding="utf-8", errors="replace") as fh:
            source_code = fh.read()

        # Compile & run
        try:
            stdout, err = compile_and_run(java_file, source_folder)
            print("  [OK]" if not err else "  [ERR] (see PDF)")
        except subprocess.TimeoutExpired:
            stdout, err = None, "[TIMEOUT] Program took too long to run."
            print("  [TIMEOUT]")
        except FileNotFoundError:
            stdout, err = None, ("[ERROR] 'javac' or 'java' not found.\n"
                                 "Make sure Java JDK is installed and on PATH.")
            print("  [ERROR] java not found")

        block = []

        # File name header
        block.append(Paragraph(
            f"[FILE] {java_file}",
            styles["file_header"]
        ))
        block.append(HRFlowable(width="100%", thickness=0.8,
                                 color=colors.HexColor("#1a237e"),
                                 spaceAfter=4))

        # ── Source code ──
        block.append(Paragraph("SOURCE CODE", styles["section_label"]))
        block.append(Preformatted(source_code, styles["code"]))
        block.append(Spacer(1, 4))

        # ── Output ──
        block.append(Paragraph("OUTPUT", styles["section_label"]))
        if err:
            block.append(Preformatted(err, styles["error"]))
        else:
            block.append(Preformatted(stdout or "(No output)", styles["output"]))

        block.append(Spacer(1, 8))

        story.append(KeepTogether(block[:4]))   # try to keep header+code together
        story.extend(block[4:])

        # New page between files (except after last)
        if idx < total:
            story.append(PageBreak())

    # Build
    doc = SimpleDocTemplate(
        output_pdf,
        pagesize=PAGE_SIZE,
        leftMargin=15 * mm,
        rightMargin=15 * mm,
        topMargin=15 * mm,
        bottomMargin=15 * mm,
    )
    doc.build(story)
    print(f"\n[DONE] PDF saved as: {output_pdf}")


if __name__ == "__main__":
    if not os.path.isdir(SOURCE_FOLDER):
        print(f"❌  Folder '{SOURCE_FOLDER}' not found.")
        print("    Place this script in the same directory as your Home_Work/ folder.")
    else:
        make_pdf(SOURCE_FOLDER, OUTPUT_PDF)
