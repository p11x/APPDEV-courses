"""
build_python_guide_pdf.py
Converts the entire python-guide/ folder into a single styled PDF.
"""

import os
import re
import shutil
from pathlib import Path
from reportlab.lib import colors
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import mm, cm
from reportlab.lib.enums import TA_LEFT, TA_CENTER, TA_JUSTIFY
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Preformatted, Spacer, PageBreak,
    Table, TableStyle, HRFlowable, KeepTogether, ListFlowable, ListItem
)
from reportlab.platypus.flowables import Flowable
from reportlab.platypus.tableofcontents import TableOfContents
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.pdfgen import canvas
import markdown2

# ── Paths ─────────────────────────────────────────────────────────────────────
SOURCE      = Path("python-guide")
COPY        = Path("python-guide-pdf-build")
OUTPUT_PDF  = Path("Python_Guide_Complete.pdf")

# ── Colour Palette ─────────────────────────────────────────────────────────────
# Modern dark-accent documentation palette
C_PRIMARY    = colors.HexColor("#1a1a2e")   # Deep navy — main headings
C_ACCENT     = colors.HexColor("#16213e")   # Dark blue — section headers
C_HIGHLIGHT  = colors.HexColor("#0f3460")   # Medium blue — subsection headers
C_CODE_BG    = colors.HexColor("#f6f8fa")   # Light grey — code block background
C_CODE_BORD  = colors.HexColor("#d0d7de")   # Grey — code block border
C_RULE       = colors.HexColor("#e1e4e8")   # Light rule lines
C_BODY       = colors.HexColor("#24292f")   # Near-black body text
C_MUTED      = colors.HexColor("#57606a")   # Muted grey — subtitles, captions
C_BADGE_BG   = colors.HexColor("#ddf4ff")   # Light blue badge background
C_BADGE_TEXT = colors.HexColor("#0969da")   # Blue badge text
C_WARN_BG    = colors.HexColor("#fff8c5")   # Yellow — note/warning blocks
C_WARN_BORD  = colors.HexColor("#d4a72c")   # Yellow border
C_GREEN      = colors.HexColor("#1f883d")   # Green — tips
C_PAGE_BG    = colors.HexColor("#ffffff")   # White page

# Section accent colours — one per main topic number
SECTION_COLOURS = [
    colors.HexColor("#0f3460"),   # 01 Foundations
    colors.HexColor("#533483"),   # 02 Control Flow
    colors.HexColor("#e94560"),   # 03 Functions
    colors.HexColor("#0d7377"),   # 04 Data Structures
    colors.HexColor("#14a085"),   # 05 OOP
    colors.HexColor("#f8931f"),   # 06 Modules
    colors.HexColor("#d63031"),   # 07 Advanced Python
    colors.HexColor("#6c5ce7"),   # 08 Projects
    colors.HexColor("#00b894"),   # 09 Data Science
    colors.HexColor("#fd79a8"),   # 10 Machine Learning
    colors.HexColor("#e17055"),   # 11 Deep Learning
    colors.HexColor("#2d3436"),   # 12 Data Projects
    colors.HexColor("#0984e3"),   # 13 Cutting Edge
    colors.HexColor("#a29bfe"),   # 14 AI & LLM
    colors.HexColor("#55efc4"),   # 15 Platforms
    colors.HexColor("#fdcb6e"),   # 16 Automation
    colors.HexColor("#e84393"),   # 17 Performance
    colors.HexColor("#ff7675"),   # 18 Security
    colors.HexColor("#b2bec3"),   # 19 Scraping
    colors.HexColor("#636e72"),   # 20 System Design
    colors.HexColor("#74b9ff"),   # 21 Interview Prep
]

# Global registry for page sections (fix 3)
PAGE_SECTION_MAP: dict[int, str] = {}


# ── Styles ─────────────────────────────────────────────────────────────────────

def build_styles():
    base = getSampleStyleSheet()

    styles = {

        # Cover page
        "cover_title": ParagraphStyle(
            "cover_title",
            fontName="Helvetica-Bold",
            fontSize=40,
            textColor=C_PRIMARY,
            leading=48,
            alignment=TA_CENTER,
            spaceAfter=8,
        ),
        "cover_subtitle": ParagraphStyle(
            "cover_subtitle",
            fontName="Helvetica",
            fontSize=16,
            textColor=C_MUTED,
            leading=22,
            alignment=TA_CENTER,
            spaceAfter=4,
        ),
        "cover_meta": ParagraphStyle(
            "cover_meta",
            fontName="Helvetica",
            fontSize=11,
            textColor=C_MUTED,
            alignment=TA_CENTER,
        ),

        # Section divider page
        "section_number": ParagraphStyle(
            "section_number",
            fontName="Helvetica-Bold",
            fontSize=72,
            textColor=colors.HexColor("#f0f0f0"),
            leading=80,
            alignment=TA_CENTER,
        ),
        "section_title": ParagraphStyle(
            "section_title",
            fontName="Helvetica-Bold",
            fontSize=32,
            textColor=C_PRIMARY,
            leading=40,
            alignment=TA_CENTER,
            spaceAfter=12,
        ),
        "section_desc": ParagraphStyle(
            "section_desc",
            fontName="Helvetica",
            fontSize=13,
            textColor=C_MUTED,
            leading=20,
            alignment=TA_CENTER,
        ),

        # Content headings
        "h1": ParagraphStyle(
            "h1",
            fontName="Helvetica-Bold",
            fontSize=22,
            textColor=C_PRIMARY,
            leading=28,
            spaceBefore=20,
            spaceAfter=10,
            borderPadding=(0, 0, 6, 0),
        ),
        "h2": ParagraphStyle(
            "h2",
            fontName="Helvetica-Bold",
            fontSize=16,
            textColor=C_ACCENT,
            leading=22,
            spaceBefore=16,
            spaceAfter=6,
        ),
        "h3": ParagraphStyle(
            "h3",
            fontName="Helvetica-Bold",
            fontSize=13,
            textColor=C_HIGHLIGHT,
            leading=18,
            spaceBefore=12,
            spaceAfter=4,
        ),
        "h4": ParagraphStyle(
            "h4",
            fontName="Helvetica-BoldOblique",
            fontSize=11,
            textColor=C_BODY,
            leading=16,
            spaceBefore=8,
            spaceAfter=2,
        ),

        # Body text
        "body": ParagraphStyle(
            "body",
            fontName="Helvetica",
            fontSize=10.5,
            textColor=C_BODY,
            leading=16,
            spaceBefore=4,
            spaceAfter=4,
            alignment=TA_JUSTIFY,
        ),

        # Inline code in body text
        "body_code": ParagraphStyle(
            "body_code",
            fontName="Courier",
            fontSize=9.5,
            textColor=C_HIGHLIGHT,
            leading=14,
        ),

        # Code block
        "code": ParagraphStyle(
            "code",
            fontName="Courier",
            fontSize=8.5,
            textColor=C_BODY,
            leading=13,
            leftIndent=0,
            spaceBefore=6,
            spaceAfter=6,
            backColor=C_CODE_BG,
        ),

        # Bullet list item
        "bullet": ParagraphStyle(
            "bullet",
            fontName="Helvetica",
            fontSize=10.5,
            textColor=C_BODY,
            leading=16,
            leftIndent=16,
            spaceBefore=2,
            spaceAfter=2,
            bulletIndent=6,
        ),

        # Numbered list item
        "numbered": ParagraphStyle(
            "numbered",
            fontName="Helvetica",
            fontSize=10.5,
            textColor=C_BODY,
            leading=16,
            leftIndent=20,
            spaceBefore=2,
            spaceAfter=2,
        ),

        # Note / tip / warning block
        "note": ParagraphStyle(
            "note",
            fontName="Helvetica-Oblique",
            fontSize=10,
            textColor=colors.HexColor("#5a4a00"),
            leading=15,
            leftIndent=12,
            rightIndent=12,
        ),

        # Table of contents entries
        "toc_h1": ParagraphStyle(
            "toc_h1",
            fontName="Helvetica-Bold",
            fontSize=11,
            textColor=C_PRIMARY,
            leading=16,
            spaceBefore=6,
        ),
        "toc_h2": ParagraphStyle(
            "toc_h2",
            fontName="Helvetica",
            fontSize=10,
            textColor=C_BODY,
            leading=14,
            leftIndent=16,
        ),

        # File label at top of each .md file
        "file_label": ParagraphStyle(
            "file_label",
            fontName="Courier",
            fontSize=8,
            textColor=C_MUTED,
            leading=12,
            spaceBefore=0,
            spaceAfter=4,
        ),
    }

    return styles


# ── Page Template ──────────────────────────────────────────────────────────────

class CodeBlock(Flowable):
    """
    A splittable code block flowable with background and border.
    Unlike Table, this CAN be split across pages by ReportLab.
    """
    def __init__(self, code_text: str, max_width=155*mm, style=None):
        Flowable.__init__(self)
        self.code_text = code_text
        self.max_width = max_width
        self._style    = style
        self.width     = max_width
        # Truncate very long lines to prevent horizontal overflow
        lines = []
        for line in code_text.splitlines():
            if len(line) > 95:
                line = line[:92] + "..."
            lines.append(line)
        self.lines = lines

    def wrap(self, available_width, available_height):
        self.width = min(self.max_width, available_width)
        # 13pt leading per line + 16pt top/bottom padding
        self.height = len(self.lines) * 13 + 16
        return self.width, self.height

    def split(self, available_width, available_height):
        """Split this code block across two pages if it's too tall."""
        if available_height < 40:
            return [PageBreak(), self]

        lines_that_fit = max(1, int((available_height - 16) / 13))

        if lines_that_fit >= len(self.lines):
            return [self]

        first_block = CodeBlock(
            "\n".join(self.lines[:lines_that_fit]),
            self.max_width, self._style
        )
        second_block = CodeBlock(
            "\n".join(self.lines[lines_that_fit:]),
            self.max_width, self._style
        )
        return [first_block, second_block]

    def draw(self):
        canv = self.canv
        # Background
        canv.setFillColor(C_CODE_BG)
        canv.roundRect(0, 0, self.width, self.height, 4, fill=1, stroke=0)
        # Border
        canv.setStrokeColor(C_CODE_BORD)
        canv.setLineWidth(0.8)
        canv.roundRect(0, 0, self.width, self.height, 4, fill=0, stroke=1)
        # Text
        canv.setFillColor(C_BODY)
        canv.setFont("Courier", 8.5)
        y = self.height - 13  # start from top with padding
        for line in self.lines:
            canv.drawString(10, y, line)
            y -= 13


class SectionBookmark(Flowable):
    """Zero-height invisible flowable that records the current section name."""
    def __init__(self, section_name: str):
        Flowable.__init__(self)
        self.section_name = section_name
        self.width  = 0
        self.height = 0

    def draw(self):
        # Called during rendering — record what page this section starts on
        page = self.canv.getPageNumber()
        PAGE_SECTION_MAP[page] = self.section_name


class DocCanvas:
    """Adds running header, footer, and page number to every page."""

    def __call__(self, canv, doc):
        canv.saveState()
        page_w, page_h = A4
        page = doc.page

        # Look up which section this page belongs to
        # Walk backwards through known section pages to find current section
        section_name = "Python Guide"
        for p in sorted(PAGE_SECTION_MAP.keys(), reverse=True):
            if p <= page:
                section_name = PAGE_SECTION_MAP[p]
                break

        # Determine section colour
        sec_colour = C_PRIMARY
        for idx, name in enumerate(SECTION_DESCRIPTIONS.keys()):
            clean = re.sub(r"^\d+_", "", name).replace("_", " ").lower()
            if clean == section_name.lower().strip():
                sec_colour = SECTION_COLOURS[idx % len(SECTION_COLOURS)]
                break

        # Header bar
        canv.setFillColor(sec_colour)
        canv.rect(0, page_h - 18*mm, page_w, 8*mm, fill=1, stroke=0)
        canv.setFillColor(colors.white)
        canv.setFont("Helvetica-Bold", 8)
        canv.drawString(20*mm, page_h - 13*mm, "Python Guide")
        canv.setFont("Helvetica", 8)
        canv.drawRightString(page_w - 20*mm, page_h - 13*mm, section_name)

        # Footer
        canv.setFillColor(C_MUTED)
        canv.setFont("Helvetica", 8)
        canv.drawCentredString(page_w / 2, 10*mm, str(page))
        canv.setStrokeColor(C_RULE)
        canv.setLineWidth(0.5)
        canv.line(20*mm, 14*mm, page_w - 20*mm, 14*mm)

        canv.restoreState()


# ── Markdown Parser ───────────────────────────────────────────────────────────

def md_to_flowables(md_text: str, styles: dict, filepath: str = "") -> list:
    """
    Converts a markdown string to a list of ReportLab flowables.
    Handles: headings (H1-H4), paragraphs, code blocks, bullet lists,
    numbered lists, bold/italic inline, horizontal rules, blockquotes.
    """
    flowables = []

    # File breadcrumb label
    if filepath:
        label = filepath.replace("\\", "/")
        flowables.append(Paragraph(label, styles["file_label"]))

    lines = md_text.splitlines()
    i = 0

    while i < len(lines):
        line = lines[i]
        stripped = line.strip()

        # ── Fenced code block ── ` ``` `
        if stripped.startswith("```"):
            lang = stripped[3:].strip()
            code_lines = []
            i += 1
            while i < len(lines) and not lines[i].strip().startswith("```"):
                # Truncate lines that are too long for the page
                line = lines[i]
                if len(line) > 95:
                    line = line[:92] + "..."
                code_lines.append(line)
                i += 1

            code_text = "\n".join(code_lines)

            flowables.append(Spacer(1, 4))
            flowables.append(CodeBlock(code_text, max_width=155*mm))
            flowables.append(Spacer(1, 6))
            i += 1
            continue

        # ── Headings ──
        if stripped.startswith("#### "):
            text = inline_format(stripped[5:])
            flowables.append(Paragraph(text, styles["h4"]))
            i += 1
            continue
        if stripped.startswith("### "):
            text = inline_format(stripped[4:])
            flowables.append(Paragraph(text, styles["h3"]))
            i += 1
            continue
        if stripped.startswith("## "):
            text = inline_format(stripped[3:])
            flowables.append(Paragraph(text, styles["h2"]))
            i += 1
            continue
        if stripped.startswith("# "):
            text = inline_format(stripped[2:])
            flowables.append(Paragraph(text, styles["h1"]))
            i += 1
            continue

        # ── Horizontal rule ──
        if stripped in ("---", "***", "___"):
            flowables.append(HRFlowable(width="100%", thickness=0.5, color=C_RULE))
            flowables.append(Spacer(1, 4))
            i += 1
            continue

        # ── Blockquote ──
        if stripped.startswith("> "):
            quote_text = inline_format(stripped[2:])
            quote_table = Table(
                [[Paragraph(quote_text, styles["note"])]],
                colWidths=[155*mm],
            )
            quote_table.setStyle(TableStyle([
                ("BACKGROUND",    (0, 0), (-1, -1), C_WARN_BG),
                ("LINEAFTER",     (0, 0), (-1, -1), 0),
                ("LINEBEFORE",    (0, 0), (-1, -1), 3, C_WARN_BORD),
                ("LEFTPADDING",   (0, 0), (-1, -1), 12),
                ("RIGHTPADDING",  (0, 0), (-1, -1), 10),
                ("TOPPADDING",    (0, 0), (-1, -1), 6),
                ("BOTTOMPADDING", (0, 0), (-1, -1), 6),
            ]))
            flowables.append(quote_table)
            flowables.append(Spacer(1, 4))
            i += 1
            continue

        # ── Bullet list ──
        if stripped.startswith("- ") or stripped.startswith("* "):
            items = []
            while i < len(lines) and (
                lines[i].strip().startswith("- ") or
                lines[i].strip().startswith("* ")
            ):
                item_text = inline_format(lines[i].strip()[2:])
                items.append(ListItem(
                    Paragraph(item_text, styles["bullet"]),
                    bulletColor=C_ACCENT,
                ))
                i += 1
            flowables.append(ListFlowable(
                items,
                bulletType="bullet",
                bulletFontName="Helvetica",
                bulletFontSize=10,
                bulletColor=C_ACCENT,
                start="•",
                leftIndent=16,
                bulletIndent=6,
            ))
            continue

        # ── Numbered list ──
        if re.match(r"^\d+\.\s", stripped):
            items = []
            num = 1
            while i < len(lines) and re.match(r"^\d+\.\s", lines[i].strip()):
                item_text = inline_format(re.sub(r"^\d+\.\s", "", lines[i].strip()))
                items.append(ListItem(
                    Paragraph(item_text, styles["numbered"]),
                    value=num,
                ))
                num += 1
                i += 1
            flowables.append(ListFlowable(items, bulletType="1"))
            continue

        # ── Table (simple markdown | table | format) ──
        if stripped.startswith("|") and "|" in stripped:
            table_lines = []
            while i < len(lines) and lines[i].strip().startswith("|"):
                table_lines.append(lines[i].strip())
                i += 1
            # Skip separator row (|----|)
            rows = [row for row in table_lines if not re.match(r"^\|[-| :]+\|$", row)]
            table_data = []
            for row in rows:
                cells = [inline_format(c.strip()) for c in row.strip("|").split("|")]
                table_data.append(cells)
            if table_data:
                col_count = max(len(r) for r in table_data)
                col_count = min(col_count, 6)   # cap at 6 columns max
                col_w = max(20*mm, 155*mm / col_count)

                # Truncate cell content for very wide tables
                MAX_CELL_CHARS = 80
                table_data_safe = []
                for row in table_data:
                    safe_row = []
                    for cell in row:
                        if len(cell) > MAX_CELL_CHARS:
                            cell = cell[:MAX_CELL_CHARS - 3] + "..."
                        safe_row.append(cell)
                    table_data_safe.append(safe_row)
                table_data = table_data_safe

                tbl = Table(
                    [[Paragraph(c, styles["body"]) for c in row] for row in table_data],
                    colWidths=[col_w] * col_count,
                    repeatRows=1,
                )
                tbl.setStyle(TableStyle([
                    ("BACKGROUND",   (0, 0), (-1, 0), C_PRIMARY),
                    ("TEXTCOLOR",    (0, 0), (-1, 0), colors.white),
                    ("FONTNAME",     (0, 0), (-1, 0), "Helvetica-Bold"),
                    ("FONTSIZE",     (0, 0), (-1, 0), 9),
                    ("BACKGROUND",   (0, 1), (-1, -1), C_PAGE_BG),
                    ("ROWBACKGROUNDS",(0, 1), (-1, -1), [C_PAGE_BG, C_CODE_BG]),
                    ("GRID",         (0, 0), (-1, -1), 0.4, C_RULE),
                    ("TOPPADDING",   (0, 0), (-1, -1), 5),
                    ("BOTTOMPADDING",(0, 0), (-1, -1), 5),
                    ("LEFTPADDING",  (0, 0), (-1, -1), 8),
                ]))
                flowables.append(Spacer(1, 6))
                flowables.append(tbl)
                flowables.append(Spacer(1, 6))
            continue

        # ── Empty line ──
        if not stripped:
            flowables.append(Spacer(1, 5))
            i += 1
            continue

        # ── Regular paragraph ──
        text = inline_format(stripped)
        flowables.append(Paragraph(text, styles["body"]))
        i += 1

    return flowables


def inline_format(text: str) -> str:
    """Convert inline markdown (bold, italic, code) to ReportLab XML."""
    # Escape XML first
    text = text.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")
    # Bold+italic
    text = re.sub(r"\*\*\*(.+?)\*\*\*", r"<b><i>\1</i></b>", text)
    # Bold
    text = re.sub(r"\*\*(.+?)\*\*", r"<b>\1</b>", text)
    text = re.sub(r"__(.+?)__", r"<b>\1</b>", text)
    # Italic
    text = re.sub(r"\*(.+?)\*", r"<i>\1</i>", text)
    text = re.sub(r"_(.+?)_", r"<i>\1</i>", text)
    # Inline code
    text = re.sub(
        r"`(.+?)`",
        r'<font name="Courier" color="#0f3460">\1</font>',
        text
    )
    # Markdown links → just the label
    text = re.sub(r"\[(.+?)\]\(.+?\)", r"\1", text)
    return text


# ── Cover Page ─────────────────────────────────────────────────────────────────

def build_cover(styles: dict) -> list:
    flowables = []
    flowables.append(Spacer(1, 50*mm))

    # Big coloured title bar (simulated with a Table)
    cover_title_white = ParagraphStyle(
        "cover_title_white",
        fontName="Helvetica-Bold",
        fontSize=40,
        textColor=colors.white,
        leading=48,
        alignment=TA_CENTER,
        spaceAfter=8,
    )
    title_table = Table(
        [[Paragraph("Python Guide", cover_title_white)]],
        colWidths=[170*mm],
    )
    title_table.setStyle(TableStyle([
        ("BACKGROUND",    (0, 0), (-1, -1), C_PRIMARY),
        ("TOPPADDING",    (0, 0), (-1, -1), 20),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 20),
        ("LEFTPADDING",   (0, 0), (-1, -1), 20),
        ("RIGHTPADDING",  (0, 0), (-1, -1), 20),
    ]))
    flowables.append(title_table)
    flowables.append(Spacer(1, 12*mm))

    flowables.append(Paragraph(
        "Complete Python Documentation — Foundations to AI",
        styles["cover_subtitle"]
    ))
    flowables.append(Spacer(1, 6*mm))

    # Stats badges
    badge_data = [["21 Sections", "58+ Subtopics", "501 Files", "All Topics"]]
    badge_table = Table(badge_data, colWidths=[40*mm]*4)
    badge_table.setStyle(TableStyle([
        ("BACKGROUND",    (0, 0), (-1, -1), C_BADGE_BG),
        ("TEXTCOLOR",     (0, 0), (-1, -1), C_BADGE_TEXT),
        ("FONTNAME",      (0, 0), (-1, -1), "Helvetica-Bold"),
        ("FONTSIZE",      (0, 0), (-1, -1), 10),
        ("ALIGN",         (0, 0), (-1, -1), "CENTER"),
        ("TOPPADDING",    (0, 0), (-1, -1), 8),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 8),
        ("BOX",           (0, 0), (-1, -1), 0.5, C_BADGE_TEXT),
        ("INNERGRID",     (0, 0), (-1, -1), 0.5, C_BADGE_TEXT),
    ]))
    flowables.append(badge_table)
    flowables.append(Spacer(1, 40*mm))
    flowables.append(Paragraph(
        "Generated from python-guide/ — Python 3.10+",
        styles["cover_meta"]
    ))
    flowables.append(PageBreak())
    return flowables


# ── Section Divider Page ───────────────────────────────────────────────────────

SECTION_DESCRIPTIONS = {
    "01_Foundations":                   "Variables, types, operators and your first Python scripts",
    "02_Control_Flow":                  "Conditionals, loops, exceptions, pattern matching",
    "03_Functions":                     "Defining, decorating, and composing functions",
    "04_Data_Structures":               "Lists, dicts, sets, comprehensions, and algorithms",
    "05_OOP":                           "Classes, inheritance, design patterns, magic methods",
    "06_Modules_and_Packages":          "Imports, packages, virtual environments, PyPI",
    "07_Advanced_Python":               "Generators, async, typing, memory, concurrency",
    "08_Projects_and_Practices":        "Testing, best practices, project structure, docs",
    "09_Data_Science_Foundations":      "NumPy, Pandas, Matplotlib, Seaborn, statistics",
    "10_Machine_Learning":              "Sklearn, algorithms, evaluation, pipelines, production",
    "11_Deep_Learning_Intro":           "Neural networks, PyTorch, CNNs, NLP, transformers",
    "12_Data_Projects_and_Notebooks":   "Jupyter, EDA, end-to-end project templates",
    "13_Cutting_Edge_Python":           "Python 3.12/3.13, metaprogramming, type system",
    "14_AI_and_LLM_Apps":               "Claude API, chatbots, RAG, agents, prompt engineering",
    "15_Python_for_Platforms":          "FastAPI, databases, Docker, queues, cloud, gRPC",
    "16_Automation_and_Scripting":      "Files, web automation, CLI tools, scheduling",
    "17_Performance_Python":            "Profiling, Cython, Numba, async performance",
    "18_Python_Security":               "Secrets, cryptography, secure coding, web security",
    "19_Advanced_Web_Scraping":         "Scrapy, data pipelines, anti-detection, projects",
    "20_System_Design_and_Architecture":"SOLID, architecture patterns, scalability, case studies",
    "21_Interview_Prep_and_Open_Source":"Coding patterns, interview Q&A, open source, career",
}


def build_section_divider(section_folder: str, section_num: int, styles: dict) -> list:
    colour = SECTION_COLOURS[(section_num - 1) % len(SECTION_COLOURS)]
    # Strip numeric prefix for display
    name = re.sub(r"^\d+_", "", section_folder).replace("_", " ")
    desc = SECTION_DESCRIPTIONS.get(section_folder, "")
    num_str = f"{section_num:02d}"

    # Full-page coloured background via a wide Table
    content = [
        Spacer(1, 30*mm),
        Paragraph(num_str, ParagraphStyle(
            "sn", fontName="Helvetica-Bold", fontSize=80,
            textColor=colors.HexColor("#e8e8f0"), alignment=TA_CENTER, leading=90
        )),
        Spacer(1, 4*mm),
        Paragraph(name, ParagraphStyle(
            "st", fontName="Helvetica-Bold", fontSize=28,
            textColor=colour, alignment=TA_CENTER, leading=36
        )),
        Spacer(1, 6*mm),
        HRFlowable(width="60%", thickness=2, color=colour),
        Spacer(1, 6*mm),
        Paragraph(desc, ParagraphStyle(
            "sd", fontName="Helvetica", fontSize=13,
            textColor=C_MUTED, alignment=TA_CENTER, leading=20
        )),
    ]

    return [PageBreak()] + content + [PageBreak()]


# ── Subsection Header ──────────────────────────────────────────────────────────

def build_manual_toc(section_folders: list, styles: dict) -> list:
    """Builds a simple manual table of contents from section folder names."""
    toc_flowables = []

    toc_title_style = ParagraphStyle(
        "toc_page_title",
        fontName="Helvetica-Bold",
        fontSize=22,
        textColor=C_PRIMARY,
        leading=28,
        spaceBefore=10,
        spaceAfter=20,
        alignment=TA_CENTER,
    )
    toc_flowables.append(Paragraph("Table of Contents", toc_title_style))
    toc_flowables.append(HRFlowable(width="100%", thickness=1, color=C_RULE))
    toc_flowables.append(Spacer(1, 8*mm))

    for sec_idx, section_dir in enumerate(section_folders):
        sec_num   = sec_idx + 1
        sec_colour = SECTION_COLOURS[(sec_num - 1) % len(SECTION_COLOURS)]
        sec_name   = re.sub(r"^\d+_", "", section_dir.name).replace("_", " ")

        # Section entry
        sec_style = ParagraphStyle(
            f"toc_sec_{sec_num}",
            fontName="Helvetica-Bold",
            fontSize=11,
            textColor=sec_colour,
            leading=18,
            spaceBefore=8,
            spaceAfter=2,
        )
        toc_flowables.append(Paragraph(
            f"{sec_num:02d}  {sec_name}",
            sec_style
        ))

        # Subsection entries
        sub_dirs = [d for d in get_sorted_children(section_dir) if d.is_dir()]
        for sub_dir in sub_dirs:
            sub_name = re.sub(r"^\d+_", "", sub_dir.name).replace("_", " ")
            toc_flowables.append(Paragraph(
                f"    • {sub_name}",
                styles["toc_h2"]
            ))

        toc_flowables.append(Spacer(1, 2*mm))

    toc_flowables.append(PageBreak())
    return toc_flowables


def build_subsection_header(folder_name: str, section_colour, styles: dict) -> list:
    name = re.sub(r"^\d+_", "", folder_name).replace("_", " ")
    header_table = Table(
        [[Paragraph(name, ParagraphStyle(
            "subfh", fontName="Helvetica-Bold", fontSize=15,
            textColor=colors.white, leading=20
        ))]],
        colWidths=[155*mm],
    )
    header_table.setStyle(TableStyle([
        ("BACKGROUND",    (0, 0), (-1, -1), section_colour),
        ("TOPPADDING",    (0, 0), (-1, -1), 10),
        ("BOTTOMPADDING", (0, 0), (-1, -1), 10),
        ("LEFTPADDING",   (0, 0), (-1, -1), 14),
    ]))
    return [Spacer(1, 8*mm), header_table, Spacer(1, 4*mm)]


# ── Main Build Function ────────────────────────────────────────────────────────

def get_sorted_children(path: Path) -> list:
    """Sort by numeric prefix so 01_, 02_, 10_ order correctly."""
    def sort_key(p):
        m = re.match(r"^(\d+)", p.name)
        return int(m.group(1)) if m else 999
    return sorted(path.iterdir(), key=sort_key)


def build_pdf():
    # ── Step 1: Copy source folder ──────────────────────────────────────
    if COPY.exists():
        shutil.rmtree(COPY)
    print(f"Copying {SOURCE} -> {COPY} ...")
    shutil.copytree(SOURCE, COPY)
    print("Copy complete.\n")

    styles = build_styles()

    # ── Step 2: Collect all section folders ─────────────────────────────
    section_folders = [
        d for d in get_sorted_children(COPY)
        if d.is_dir() and re.match(r"^\d+_", d.name)
    ]
    print(f"Found {len(section_folders)} main sections.\n")

    # ── Step 3: Build document ──────────────────────────────────────────
    doc = SimpleDocTemplate(
        str(OUTPUT_PDF),
        pagesize=A4,
        leftMargin=20*mm,
        rightMargin=20*mm,
        topMargin=22*mm,
        bottomMargin=22*mm,
        title="Python Guide — Complete Documentation",
        author="python-guide",
        subject="Python programming from fundamentals to AI",
    )

    story = []

    # Cover page
    story += build_cover(styles)

    # Manual table of contents
    story += build_manual_toc(section_folders, styles)

    files_processed = 0

    # ── Iterate sections ─────────────────────────────────────────────────
    for sec_idx, section_dir in enumerate(section_folders):
        sec_num = sec_idx + 1
        sec_colour = SECTION_COLOURS[(sec_num - 1) % len(SECTION_COLOURS)]
        sec_display = re.sub(r"^\d+_", "", section_dir.name).replace("_", " ")

        print(f"\n[Section {sec_num:02d}] {sec_display}")

        # Section divider page
        story += build_section_divider(section_dir.name, sec_num, styles)
        story.append(SectionBookmark(sec_display))

        # ── Iterate subsection folders ───────────────────────────────────
        subsection_dirs = [
            d for d in get_sorted_children(section_dir)
            if d.is_dir()
        ]

        # Also handle .md files directly in section root
        root_mds = sorted(
            [f for f in section_dir.iterdir() if f.suffix == ".md"],
            key=lambda p: int(re.match(r"^(\d+)", p.name).group(1)) if re.match(r"^(\d+)", p.name) else 999
        )

        for md_file in root_mds:
            try:
                md_text = md_file.read_text(encoding="utf-8", errors="replace")
                rel_path = str(md_file.relative_to(COPY))
                story += md_to_flowables(md_text, styles, rel_path)
                files_processed += 1
            except Exception as e:
                print(f"  Error processing {md_file}: {e}")
                files_processed += 1
            if files_processed % 10 == 0:
                print(f"  [Progress] {files_processed} files processed")

        for sub_dir in subsection_dirs:
            sub_display = re.sub(r"^\d+_", "", sub_dir.name).replace("_", " ")
            print(f"  - {sub_display}")

            # Subsection header bar
            story += build_subsection_header(sub_dir.name, sec_colour, styles)

            # .md files in this subsection
            md_files = sorted(
                [f for f in sub_dir.iterdir() if f.suffix == ".md"],
                key=lambda p: int(re.match(r"^(\d+)", p.name).group(1)) if re.match(r"^(\d+)", p.name) else 999
            )

            for md_file in md_files:
                try:
                    md_text = md_file.read_text(encoding="utf-8", errors="replace")
                    rel_path = str(md_file.relative_to(COPY))

                    # Thin separator between files within same subsection
                    story.append(HRFlowable(
                        width="100%", thickness=0.3,
                        color=C_RULE, spaceAfter=4
                    ))
                    story += md_to_flowables(md_text, styles, rel_path)
                    files_processed += 1
                except Exception as e:
                    print(f"  Error processing {md_file}: {e}")
                    files_processed += 1

                if files_processed % 10 == 0:
                    print(f"  [Progress] {files_processed} files processed")

            # Check for nested subsub folders (e.g. 04_Comprehensions inside 04_Data_Structures)
            nested_dirs = [d for d in get_sorted_children(sub_dir) if d.is_dir()]
            for nested_dir in nested_dirs:
                nested_display = re.sub(r"^\d+_", "", nested_dir.name).replace("_", " ")
                story += build_subsection_header(nested_dir.name, sec_colour, styles)

                nested_mds = sorted(
                    [f for f in nested_dir.iterdir() if f.suffix == ".md"],
                    key=lambda p: int(re.match(r"^(\d+)", p.name).group(1)) if re.match(r"^(\d+)", p.name) else 999
                )
                for md_file in nested_mds:
                    try:
                        md_text = md_file.read_text(encoding="utf-8", errors="replace")
                        rel_path = str(md_file.relative_to(COPY))
                        story.append(HRFlowable(width="100%", thickness=0.3, color=C_RULE, spaceAfter=4))
                        story += md_to_flowables(md_text, styles, rel_path)
                        files_processed += 1
                    except Exception as e:
                        print(f"  Error processing {md_file}: {e}")
                        files_processed += 1
                    if files_processed % 10 == 0:
                        print(f"  [Progress] {files_processed} files processed")

    # ── Step 4: Build PDF ────────────────────────────────────────────────
    print(f"\nBuilding PDF from {files_processed} files...")

    doc.multiBuild(
        story,
        onFirstPage=DocCanvas(),
        onLaterPages=DocCanvas(),
    )

    print(f"\n{'='*55}")
    print(f"DONE.")
    print(f"  Files processed : {files_processed}")
    print(f"  Output          : {OUTPUT_PDF.resolve()}")
    print(f"  Build copy      : {COPY.resolve()}")
    print(f"{'='*55}")


if __name__ == "__main__":
    build_pdf()