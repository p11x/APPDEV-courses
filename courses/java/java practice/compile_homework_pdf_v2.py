"""
compile_homework_pdf_v2.py
--------------------------
Compiles all Java files in the Home_Work folder into one PDF.
- Title changed to "Java Homework"
- All Scanner-based programs are fed sample inputs so full output is shown
- Sample input used is displayed alongside the output

Usage:
    python compile_homework_pdf_v2.py

Requirements:
    pip install reportlab

Place this script next to your Home_Work/ folder and run it.
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

# ── Configuration ──────────────────────────────────────────────────────────────
SOURCE_FOLDER   = "Home_Work"
OUTPUT_PDF      = "Homework_All.pdf"
PAGE_SIZE       = A4
COMPILE_TIMEOUT = 15
RUN_TIMEOUT     = 10
# ───────────────────────────────────────────────────────────────────────────────

# Sample inputs for every program that uses Scanner.
# Each entry is a newline-separated string that will be fed via stdin.
SAMPLE_INPUTS = {
    # Home3  – divisible by 5 and 11
    "Home3.java":  "55\n",
    # Home4  – even or odd
    "Home4.java":  "7\n",
    # Home5  – leap year
    "Home5.java":  "2000\n",
    # Home7  – vowel check
    "Home7.java":  "a\n",
    # Home8  – alphabet/digit/special
    "Home8.java":  "3\n",
    # Home9  – upper/lowercase
    "Home9.java":  "G\n",
    # Home10 – day of week
    "Home10.java": "4\n",
    # Home11 – days in month
    "Home11.java": "7\n",
    # Home12 – notes in amount
    "Home12.java": "2575\n",
    # Home13 – triangle valid (angles)
    "Home13.java": "60\n60\n60\n",
    # Home14 – triangle valid (sides)
    "Home14.java": "3\n4\n5\n",
    # Home15 – equilateral/isosceles/scalene
    "Home15.java": "5\n5\n5\n",
    # Home16 – quadratic roots
    "Home16.java": "1\n-5\n6\n",
    # Home17 – profit or loss
    "Home17.java": "500\n750\n",
    # Home18 – percentage and grade
    "Home18.java": "85\n90\n78\n92\n88\n",
    # Home19 – gross salary
    "Home19.java": "15000\n",
    # Home20 – electricity bill
    "Home20.java": "200\n",
    # Home21 – read/print array
    "Home21.java": "5\n10\n20\n30\n40\n50\n",
    # Home22 – negative elements
    "Home22.java": "6\n-1\n2\n-3\n4\n-5\n6\n",
    # Home23 – sum of array
    "Home23.java": "5\n1\n2\n3\n4\n5\n",
    # Home24 – max and min
    "Home24.java": "5\n3\n1\n4\n1\n5\n",
    # Home25 – second largest
    "Home25.java": "5\n10\n40\n30\n20\n50\n",
    # Home26 – even/odd count
    "Home26.java": "6\n1\n2\n3\n4\n5\n6\n",
    # Home27 – negative count
    "Home27.java": "5\n-1\n2\n-3\n4\n-5\n",
    # Home28 – copy array
    "Home28.java": "4\n7\n8\n9\n10\n",
    # Home29 – insert element
    "Home29.java": "4\n10\n20\n30\n40\n2\n15\n",
    # Home30 – delete element
    "Home30.java": "5\n10\n20\n30\n40\n50\n3\n",
    # Home31 – frequency of elements
    "Home31.java": "7\n1\n2\n2\n3\n3\n3\n4\n",
    # Home32 – unique elements
    "Home32.java": "6\n1\n2\n2\n3\n4\n4\n",
    # Home33 – count duplicates
    "Home33.java": "6\n1\n2\n2\n3\n3\n4\n",
    # Home34 – delete duplicates
    "Home34.java": "6\n1\n2\n2\n3\n3\n4\n",
    # Home35 – merge two arrays
    "Home35.java": "3\n1\n2\n3\n3\n4\n5\n6\n",
    # Home36 – reverse array
    "Home36.java": "5\n1\n2\n3\n4\n5\n",
    # Home37 – separate even/odd
    "Home37.java": "6\n1\n2\n3\n4\n5\n6\n",
    # Home38 – linear search
    "Home38.java": "5\n10\n20\n30\n40\n50\n30\n",
    # Home39 – sort ascending/descending
    "Home39.java": "5\n3\n1\n4\n1\n5\n1\n",
    # Home40 – sort even/odd separately
    "Home40.java": "6\n5\n2\n8\n1\n4\n7\n",
    # Home41 – left rotate
    "Home41.java": "5\n1\n2\n3\n4\n5\n2\n",
    # Home42 – right rotate
    "Home42.java": "5\n1\n2\n3\n4\n5\n2\n",
    # Home43 – add matrices (2x2)
    "Home43.java": "2\n2\n1\n2\n3\n4\n5\n6\n7\n8\n",
    # Home44 – subtract matrices (2x2)
    "Home44.java": "2\n2\n9\n8\n7\n6\n1\n2\n3\n4\n",
    # Home45 – scalar multiplication
    "Home45.java": "2\n2\n1\n2\n3\n4\n3\n",
    # Home46 – multiply matrices (2x2 * 2x2)
    "Home46.java": "2\n2\n1\n2\n3\n4\n2\n2\n5\n6\n7\n8\n",
    # Home47 – check equal matrices
    "Home47.java": "2\n2\n1\n2\n3\n4\n1\n2\n3\n4\n",
    # Home48 – sum main diagonal
    "Home48.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home49 – sum minor diagonal
    "Home49.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home50 – sum of rows and columns
    "Home50.java": "2\n3\n1\n2\n3\n4\n5\n6\n",
    # Home51 – interchange diagonals
    "Home51.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home52 – upper triangular
    "Home52.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home53 – lower triangular
    "Home53.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home54 – sum upper triangular
    "Home54.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home55 – sum lower triangular
    "Home55.java": "3\n1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home56 – transpose
    "Home56.java": "2\n3\n1\n2\n3\n4\n5\n6\n",
    # Home57 – determinant 3x3
    "Home57.java": "1\n2\n3\n4\n5\n6\n7\n8\n9\n",
    # Home58 – identity matrix
    "Home58.java": "3\n1\n0\n0\n0\n1\n0\n0\n0\n1\n",
    # Home59 – symmetric matrix
    "Home59.java": "3\n1\n2\n3\n2\n5\n6\n3\n6\n9\n",
    # Home62 – multiplication table
    "Home62.java": "7\n",
    # Home63 – natural numbers reverse
    "Home63.java": "5\n",
    # Home64 – sum of digits
    "Home64.java": "1234\n",
    # Home65 – sum even numbers
    "Home65.java": "10\n",
    # Home66 – sum odd numbers
    "Home66.java": "10\n",
    # Home67 – swap first and last digit
    "Home67.java": "1234\n",
    # Home68 – sum first and last digit
    "Home68.java": "5673\n",
    # Home69 – first and last digit
    "Home69.java": "9823\n",
    # Home70 – product of digits
    "Home70.java": "234\n",
    # Home71 – reverse number
    "Home71.java": "12345\n",
    # Home72 – calculate power
    "Home72.java": "2\n10\n",
    # Home73 – factorial
    "Home73.java": "6\n",
    # Home74 – armstrong check
    "Home74.java": "153\n",
    # Home75 – all armstrong numbers
    "Home75.java": "500\n",
    # Home76 – compound interest
    "Home76.java": "10000\n5\n3\n",
    # Home77 – prime check
    "Home77.java": "17\n",
    # Home78 – palindrome
    "Home78.java": "121\n",
    # Home79 – number in words
    "Home79.java": "4782\n",
    # Home80 – HCF
    "Home80.java": "36\n48\n",
    # Home81 – LCM
    "Home81.java": "12\n18\n",
}

# Human-readable label shown in the PDF next to the output
SAMPLE_INPUT_LABELS = {
    "Home3.java":  "Input: 55",
    "Home4.java":  "Input: 7",
    "Home5.java":  "Input: 2000",
    "Home7.java":  "Input: a",
    "Home8.java":  "Input: 3",
    "Home9.java":  "Input: G",
    "Home10.java": "Input: 4",
    "Home11.java": "Input: 7",
    "Home12.java": "Input: 2575",
    "Home13.java": "Input: 60, 60, 60",
    "Home14.java": "Input: 3, 4, 5",
    "Home15.java": "Input: 5, 5, 5",
    "Home16.java": "Input: a=1, b=-5, c=6",
    "Home17.java": "Input: cost=500, selling=750",
    "Home18.java": "Input: 85, 90, 78, 92, 88",
    "Home19.java": "Input: basic=15000",
    "Home20.java": "Input: 200 units",
    "Home21.java": "Input: size=5, [10,20,30,40,50]",
    "Home22.java": "Input: size=6, [-1,2,-3,4,-5,6]",
    "Home23.java": "Input: size=5, [1,2,3,4,5]",
    "Home24.java": "Input: size=5, [3,1,4,1,5]",
    "Home25.java": "Input: size=5, [10,40,30,20,50]",
    "Home26.java": "Input: size=6, [1,2,3,4,5,6]",
    "Home27.java": "Input: size=5, [-1,2,-3,4,-5]",
    "Home28.java": "Input: size=4, [7,8,9,10]",
    "Home29.java": "Input: size=4, [10,20,30,40], pos=2, elem=15",
    "Home30.java": "Input: size=5, [10,20,30,40,50], pos=3",
    "Home31.java": "Input: size=7, [1,2,2,3,3,3,4]",
    "Home32.java": "Input: size=6, [1,2,2,3,4,4]",
    "Home33.java": "Input: size=6, [1,2,2,3,3,4]",
    "Home34.java": "Input: size=6, [1,2,2,3,3,4]",
    "Home35.java": "Input: arr1=[1,2,3], arr2=[4,5,6]",
    "Home36.java": "Input: size=5, [1,2,3,4,5]",
    "Home37.java": "Input: size=6, [1,2,3,4,5,6]",
    "Home38.java": "Input: size=5, [10,20,30,40,50], search=30",
    "Home39.java": "Input: size=5, [3,1,4,1,5], Ascending",
    "Home40.java": "Input: size=6, [5,2,8,1,4,7]",
    "Home41.java": "Input: size=5, [1,2,3,4,5], rotate=2",
    "Home42.java": "Input: size=5, [1,2,3,4,5], rotate=2",
    "Home43.java": "Input: 2x2 matrices [[1,2],[3,4]] + [[5,6],[7,8]]",
    "Home44.java": "Input: 2x2 matrices [[9,8],[7,6]] - [[1,2],[3,4]]",
    "Home45.java": "Input: [[1,2],[3,4]], scalar=3",
    "Home46.java": "Input: [[1,2],[3,4]] * [[5,6],[7,8]]",
    "Home47.java": "Input: [[1,2],[3,4]] == [[1,2],[3,4]]",
    "Home48.java": "Input: 3x3 [[1..9]]",
    "Home49.java": "Input: 3x3 [[1..9]]",
    "Home50.java": "Input: 2x3 [[1,2,3],[4,5,6]]",
    "Home51.java": "Input: 3x3 [[1..9]]",
    "Home52.java": "Input: 3x3 [[1..9]]",
    "Home53.java": "Input: 3x3 [[1..9]]",
    "Home54.java": "Input: 3x3 [[1..9]]",
    "Home55.java": "Input: 3x3 [[1..9]]",
    "Home56.java": "Input: 2x3 [[1,2,3],[4,5,6]]",
    "Home57.java": "Input: [[1,2,3],[4,5,6],[7,8,9]]",
    "Home58.java": "Input: 3x3 Identity [[1,0,0],[0,1,0],[0,0,1]]",
    "Home59.java": "Input: 3x3 Symmetric [[1,2,3],[2,5,6],[3,6,9]]",
    "Home62.java": "Input: 7",
    "Home63.java": "Input: 5",
    "Home64.java": "Input: 1234",
    "Home65.java": "Input: 10",
    "Home66.java": "Input: 10",
    "Home67.java": "Input: 1234",
    "Home68.java": "Input: 5673",
    "Home69.java": "Input: 9823",
    "Home70.java": "Input: 234",
    "Home71.java": "Input: 12345",
    "Home72.java": "Input: base=2, exp=10",
    "Home73.java": "Input: 6",
    "Home74.java": "Input: 153",
    "Home75.java": "Input: 500",
    "Home76.java": "Input: P=10000, R=5%, T=3 years",
    "Home77.java": "Input: 17",
    "Home78.java": "Input: 121",
    "Home79.java": "Input: 4782",
    "Home80.java": "Input: 36, 48",
    "Home81.java": "Input: 12, 18",
}


def sorted_java_files(folder):
    files = [f for f in os.listdir(folder) if f.endswith(".java")]
    def natural_key(name):
        return [int(c) if c.isdigit() else c.lower()
                for c in re.split(r'(\d+)', name)]
    return sorted(files, key=natural_key)


def compile_and_run(java_file, src_folder, stdin_data=""):
    with tempfile.TemporaryDirectory() as tmpdir:
        src_path = os.path.join(src_folder, java_file)
        dst_path = os.path.join(tmpdir, java_file)
        shutil.copy2(src_path, dst_path)

        compile_result = subprocess.run(
            ["javac", java_file],
            cwd=tmpdir,
            capture_output=True,
            text=True,
            timeout=COMPILE_TIMEOUT
        )

        if compile_result.returncode != 0:
            return None, f"[COMPILATION ERROR]\n{compile_result.stderr.strip()}"

        class_name = java_file.replace(".java", "")

        run_result = subprocess.run(
            ["java", class_name],
            cwd=tmpdir,
            capture_output=True,
            text=True,
            timeout=RUN_TIMEOUT,
            input=stdin_data
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
    input_label = ParagraphStyle(
        "InputLabel",
        parent=base["Normal"],
        fontSize=8,
        textColor=colors.HexColor("#4a148c"),
        backColor=colors.HexColor("#f3e5f5"),
        fontName="Helvetica-Oblique",
        leftIndent=4,
        borderColor=colors.HexColor("#ce93d8"),
        borderWidth=0.5,
        borderPadding=3,
        spaceBefore=2,
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
        "input_label": input_label,
        "code": code_style,
        "output": output_style,
        "error": error_style,
        "normal": base["Normal"],
        "title": base["Title"],
    }


def make_pdf(source_folder, output_pdf):
    styles = build_styles()
    story  = []

    # Cover page
    story.append(Spacer(1, 30 * mm))
    story.append(Paragraph("Java Homework", styles["title"]))
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
    print(f"Found {total} Java files. Building PDF ...\n")

    for idx, java_file in enumerate(java_files, 1):
        print(f"  [{idx:3d}/{total}]  {java_file}", end="", flush=True)

        src_path = os.path.join(source_folder, java_file)
        with open(src_path, "r", encoding="utf-8", errors="replace") as fh:
            source_code = fh.read()

        stdin_data    = SAMPLE_INPUTS.get(java_file, "")
        input_label   = SAMPLE_INPUT_LABELS.get(java_file, "")

        try:
            stdout, err = compile_and_run(java_file, source_folder, stdin_data)
            print("  OK" if not err else "  ERROR (see PDF)")
        except subprocess.TimeoutExpired:
            stdout, err = None, "[TIMEOUT] Program took too long to run."
            print("  TIMEOUT")
        except FileNotFoundError:
            stdout, err = None, ("[ERROR] 'javac' or 'java' not found.\n"
                                 "Make sure Java JDK is installed and on PATH.")
            print("  java not found")

        block = []

        block.append(Paragraph(f"  {java_file}", styles["file_header"]))
        block.append(HRFlowable(width="100%", thickness=0.8,
                                 color=colors.HexColor("#1a237e"),
                                 spaceAfter=4))

        # Source code
        block.append(Paragraph("  SOURCE CODE", styles["section_label"]))
        block.append(Preformatted(source_code, styles["code"]))
        block.append(Spacer(1, 4))

        # Output
        block.append(Paragraph("  OUTPUT", styles["section_label"]))
        if input_label:
            block.append(Paragraph(f"  {input_label}", styles["input_label"]))
        if err:
            block.append(Preformatted(err, styles["error"]))
        else:
            block.append(Preformatted(stdout or "(No output)", styles["output"]))

        block.append(Spacer(1, 8))

        story.append(KeepTogether(block[:4]))
        story.extend(block[4:])

        if idx < total:
            story.append(PageBreak())

    doc = SimpleDocTemplate(
        output_pdf,
        pagesize=PAGE_SIZE,
        leftMargin=15 * mm,
        rightMargin=15 * mm,
        topMargin=15 * mm,
        bottomMargin=15 * mm,
    )
    doc.build(story)
    print(f"\nDone! PDF saved as: {output_pdf}")


if __name__ == "__main__":
    if not os.path.isdir(SOURCE_FOLDER):
        print(f"ERROR: Folder '{SOURCE_FOLDER}' not found.")
        print("Place this script in the same directory as your Home_Work/ folder.")
    else:
        make_pdf(SOURCE_FOLDER, OUTPUT_PDF)
