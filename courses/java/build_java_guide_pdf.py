"""
build_java_guide_pdf.py
Converts CoreJava_Basics/ into a single styled PDF.
"""

import re
import shutil
from pathlib import Path
from reportlab.lib import colors
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle
from reportlab.lib.units import mm
from reportlab.lib.enums import TA_LEFT, TA_CENTER, TA_JUSTIFY
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, PageBreak,
    Table, TableStyle, HRFlowable, ListFlowable, ListItem
)
from reportlab.platypus.flowables import Flowable

# ── Paths ──────────────────────────────────────────────────────────────────────
SOURCE     = Path("CoreJava_Basics")
OUTPUT_PDF = Path("Java_Guide_Complete.pdf")

# ── Colour Palette — Java/Coffee theme ────────────────────────────────────────
C_PRIMARY    = colors.HexColor("#1a1a2e")   # Deep navy
C_JAVA       = colors.HexColor("#f89820")   # Java orange (official Java colour)
C_ACCENT     = colors.HexColor("#5382a1")   # Java blue
C_DARK       = colors.HexColor("#2c2c54")   # Dark purple-navy
C_CODE_BG    = colors.HexColor("#f6f8fa")   # Light grey code background
C_CODE_BORD  = colors.HexColor("#d0d7de")   # Code border
C_JAVA_BG    = colors.HexColor("#fff8f0")   # Warm orange tint for .java files
C_JAVA_BORD  = colors.HexColor("#f89820")   # Java orange border for .java blocks
C_BODY       = colors.HexColor("#24292f")   # Near-black body text
C_MUTED      = colors.HexColor("#57606a")   # Muted grey
C_RULE       = colors.HexColor("#e1e4e8")   # Rule lines
C_BADGE_BG   = colors.HexColor("#fff3cd")   # Amber badge bg
C_BADGE_TEXT = colors.HexColor("#856404")   # Amber badge text

# Topic accent colours — one per topic group
TOPIC_COLOURS = [
    colors.HexColor("#f89820"),  # 00-04  Introduction/Setup    — Java orange
    colors.HexColor("#5382a1"),  # 05-09  Syntax/Basics         — Java blue
    colors.HexColor("#e94560"),  # 10-14  OOP                   — Red
    colors.HexColor("#0d7377"),  # 15-19  Control/Strings/IO    — Teal
    colors.HexColor("#533483"),  # 20-24  Collections/Generics  — Purple
    colors.HexColor("#14a085"),  # 25-29  Lambdas/Modern Java   — Green
    colors.HexColor("#d63031"),  # 30-33  Spring/DB/REST        — Deep red
]

def get_topic_colour(topic_num: int) -> colors.HexColor:
    if   topic_num <= 4:  return TOPIC_COLOURS[0]
    elif topic_num <= 9:  return TOPIC_COLOURS[1]
    elif topic_num <= 14: return TOPIC_COLOURS[2]
    elif topic_num <= 19: return TOPIC_COLOURS[3]
    elif topic_num <= 24: return TOPIC_COLOURS[4]
    elif topic_num <= 29: return TOPIC_COLOURS[5]
    else:                 return TOPIC_COLOURS[6]


# ── Page Registry for running headers ─────────────────────────────────────────
PAGE_TOPIC_MAP: dict[int, str] = {}

class TopicBookmark(Flowable):
    """Zero-height flowable that records the current topic name per page."""
    def __init__(self, topic_name: str):
        Flowable.__init__(self)
        self.topic_name = topic_name
        self.width = self.height = 0

    def draw(self):
        PAGE_TOPIC_MAP[self.canv.getPageNumber()] = self.topic_name


# ── Custom CodeBlock (splittable across pages) ─────────────────────────────────
class CodeBlock(Flowable):
    """
    Splittable code block with background and border.
    bg_colour and border_colour differ for .md vs .java files.
    """
    def __init__(self, code_text: str, max_width=155*mm,
                 bg=None, border=None, label: str = ""):
        Flowable.__init__(self)
        self.max_width  = max_width
        self.bg         = bg     or C_CODE_BG
        self.border     = border or C_CODE_BORD
        self.label      = label   # e.g. "Example.java"
        self.width      = max_width

        # Truncate long lines
        lines = []
        for line in code_text.splitlines():
            if len(line) > 100:
                line = line[:97] + "..."
            lines.append(line)
        self.lines = lines
        self.height = len(self.lines) * 13 + (22 if label else 16)

    def wrap(self, available_width, available_height):
        self.width = min(self.max_width, available_width)
        return self.width, self.height

    def split(self, available_width, available_height):
        if available_height < 50:
            return [PageBreak(), self]
        lines_fit = max(1, int((available_height - (22 if self.label else 16)) / 13))
        if lines_fit >= len(self.lines):
            return [self]
        first  = CodeBlock("\n".join(self.lines[:lines_fit]),
                           self.max_width, self.bg, self.border, self.label)
        second = CodeBlock("\n".join(self.lines[lines_fit:]),
                           self.max_width, self.bg, self.border, "")
        return [first, second]

    def draw(self):
        c = self.canv
        # Background
        c.setFillColor(self.bg)
        c.roundRect(0, 0, self.width, self.height, 4, fill=1, stroke=0)
        # Border
        c.setStrokeColor(self.border)
        c.setLineWidth(0.8)
        c.roundRect(0, 0, self.width, self.height, 4, fill=0, stroke=1)
        # Optional filename label tab
        if self.label:
            c.setFillColor(self.border)
            c.roundRect(8, self.height - 16, len(self.label) * 6 + 12, 14,
                        3, fill=1, stroke=0)
            c.setFillColor(colors.white)
            c.setFont("Helvetica-Bold", 7)
            c.drawString(14, self.height - 10, self.label)
        # Code text
        c.setFillColor(C_BODY)
        c.setFont("Courier", 8.5)
        y = self.height - (22 if self.label else 13)
        for line in self.lines:
            c.drawString(10, y, line)
            y -= 13


# ── Page Canvas (header + footer) ─────────────────────────────────────────────
class DocCanvas:
    def __call__(self, canv, doc):
        canv.saveState()
        page_w, page_h = A4
        page = doc.page

        # Look up topic for this page
        topic_name  = "Java Guide"
        topic_colour = C_JAVA
        for p in sorted(PAGE_TOPIC_MAP.keys(), reverse=True):
            if p <= page:
                topic_name = PAGE_TOPIC_MAP[p]
                break

        # Map topic name to colour
        m = re.match(r"^(\d+)", topic_name)
        if m:
            topic_colour = get_topic_colour(int(m.group(1)))

        # Header bar
        canv.setFillColor(topic_colour)
        canv.rect(0, page_h - 18*mm, page_w, 8*mm, fill=1, stroke=0)
        canv.setFillColor(colors.white)
        canv.setFont("Helvetica-Bold", 8)
        canv.drawString(20*mm, page_h - 13*mm, "Java Guide")
        canv.setFont("Helvetica", 8)
        # Show clean topic name (strip numeric prefix)
        display = re.sub(r"^\d+_", "", topic_name).replace("_", " ")
        canv.drawRightString(page_w - 20*mm, page_h - 13*mm, display)

        # Footer
        canv.setFillColor(C_MUTED)
        canv.setFont("Helvetica", 8)
        canv.drawCentredString(page_w / 2, 10*mm, str(page))
        canv.setStrokeColor(C_RULE)
        canv.setLineWidth(0.5)
        canv.line(20*mm, 14*mm, page_w - 20*mm, 14*mm)

        canv.restoreState()


# ── Styles ─────────────────────────────────────────────────────────────────────
def build_styles() -> dict:
    return {
        "cover_title": ParagraphStyle(
            "cover_title", fontName="Helvetica-Bold", fontSize=42,
            textColor=colors.white, leading=50, alignment=TA_CENTER),
        "cover_subtitle": ParagraphStyle(
            "cover_subtitle", fontName="Helvetica", fontSize=15,
            textColor=C_MUTED, leading=22, alignment=TA_CENTER),
        "cover_meta": ParagraphStyle(
            "cover_meta", fontName="Helvetica", fontSize=11,
            textColor=C_MUTED, alignment=TA_CENTER),
        "h1": ParagraphStyle(
            "h1", fontName="Helvetica-Bold", fontSize=22,
            textColor=C_PRIMARY, leading=28, spaceBefore=16, spaceAfter=8),
        "h2": ParagraphStyle(
            "h2", fontName="Helvetica-Bold", fontSize=16,
            textColor=C_ACCENT, leading=22, spaceBefore=14, spaceAfter=6),
        "h3": ParagraphStyle(
            "h3", fontName="Helvetica-Bold", fontSize=13,
            textColor=C_DARK, leading=18, spaceBefore=10, spaceAfter=4),
        "h4": ParagraphStyle(
            "h4", fontName="Helvetica-BoldOblique", fontSize=11,
            textColor=C_BODY, leading=16, spaceBefore=8, spaceAfter=2),
        "body": ParagraphStyle(
            "body", fontName="Helvetica", fontSize=10.5,
            textColor=C_BODY, leading=16, spaceBefore=3,
            spaceAfter=3, alignment=TA_JUSTIFY),
        "bullet": ParagraphStyle(
            "bullet", fontName="Helvetica", fontSize=10.5,
            textColor=C_BODY, leading=16, leftIndent=16, spaceBefore=2, spaceAfter=2),
        "note": ParagraphStyle(
            "note", fontName="Helvetica-Oblique", fontSize=10,
            textColor=colors.HexColor("#5a4a00"), leading=15,
            leftIndent=12, rightIndent=12),
        "file_label": ParagraphStyle(
            "file_label", fontName="Courier", fontSize=8,
            textColor=C_MUTED, leading=12, spaceAfter=4),
        "toc_section": ParagraphStyle(
            "toc_section", fontName="Helvetica-Bold", fontSize=11,
            textColor=C_PRIMARY, leading=18, spaceBefore=6, spaceAfter=2),
        "toc_sub": ParagraphStyle(
            "toc_sub", fontName="Helvetica", fontSize=10,
            textColor=C_MUTED, leading=14, leftIndent=16),
        "java_file_heading": ParagraphStyle(
            "java_file_heading", fontName="Helvetica-Bold", fontSize=11,
            textColor=C_JAVA, leading=16, spaceBefore=10, spaceAfter=4),
    }


# ── Cover Page ─────────────────────────────────────────────────────────────────
def build_cover(styles: dict, topic_count: int, java_count: int) -> list:
    f = []
    f.append(Spacer(1, 40*mm))

    cover_table = Table(
        [[Paragraph("Java Guide", styles["cover_title"])]],
        colWidths=[170*mm],
    )
    cover_table.setStyle(TableStyle([
        ("BACKGROUND",    (0,0),(-1,-1), C_PRIMARY),
        ("TOPPADDING",    (0,0),(-1,-1), 22),
        ("BOTTOMPADDING", (0,0),(-1,-1), 22),
        ("LEFTPADDING",   (0,0),(-1,-1), 20),
        ("RIGHTPADDING",  (0,0),(-1,-1), 20),
    ]))
    f.append(cover_table)
    f.append(Spacer(1, 10*mm))

    # Java orange accent line
    f.append(HRFlowable(width="100%", thickness=4, color=C_JAVA))
    f.append(Spacer(1, 8*mm))

    f.append(Paragraph(
        "Core Java — Complete Reference",
        styles["cover_subtitle"]
    ))
    f.append(Spacer(1, 8*mm))

    # Stats badges
    badge_data = [[
        f"{topic_count} Topics",
        f"{java_count}+ Java Files",
        "OOP to Spring Boot",
        "Java 8–17"
    ]]
    badge_table = Table(badge_data, colWidths=[42*mm]*4)
    badge_table.setStyle(TableStyle([
        ("BACKGROUND",    (0,0),(-1,-1), C_BADGE_BG),
        ("TEXTCOLOR",     (0,0),(-1,-1), C_BADGE_TEXT),
        ("FONTNAME",      (0,0),(-1,-1), "Helvetica-Bold"),
        ("FONTSIZE",      (0,0),(-1,-1), 10),
        ("ALIGN",         (0,0),(-1,-1), "CENTER"),
        ("TOPPADDING",    (0,0),(-1,-1), 8),
        ("BOTTOMPADDING", (0,0),(-1,-1), 8),
        ("BOX",           (0,0),(-1,-1), 0.5, C_BADGE_TEXT),
        ("INNERGRID",     (0,0),(-1,-1), 0.5, C_BADGE_TEXT),
    ]))
    f.append(badge_table)
    f.append(Spacer(1, 40*mm))
    f.append(Paragraph(
        "Generated from CoreJava_Basics/ — Java 8 through 17",
        styles["cover_meta"]
    ))
    f.append(PageBreak())
    return f


# ── Table of Contents ──────────────────────────────────────────────────────────
def build_toc(topic_dirs: list, styles: dict) -> list:
    f = []
    toc_title = ParagraphStyle(
        "toc_page_title", fontName="Helvetica-Bold", fontSize=22,
        textColor=C_PRIMARY, leading=28, spaceAfter=16, alignment=TA_CENTER)
    f.append(Paragraph("Table of Contents", toc_title))
    f.append(HRFlowable(width="100%", thickness=1, color=C_RULE))
    f.append(Spacer(1, 8*mm))

    for folder in topic_dirs:
        num_match = re.match(r"^(\d+)", folder.name)
        num       = int(num_match.group(1)) if num_match else 99
        colour    = get_topic_colour(num)
        name      = re.sub(r"^\d+_", "", folder.name).replace("_", " ")

        sec_style = ParagraphStyle(
            f"toc_{folder.name}", fontName="Helvetica-Bold",
            fontSize=11, textColor=colour, leading=18,
            spaceBefore=6, spaceAfter=1)
        f.append(Paragraph(f"{folder.name[:2]}  {name}", sec_style))

        # List .java files as sub-items
        examples_dir = folder / "examples"
        if examples_dir.exists():
            java_files = sorted(examples_dir.glob("*.java"))
            for jf in java_files:
                f.append(Paragraph(f"    • {jf.name}", styles["toc_sub"]))

    f.append(PageBreak())
    return f


# ── Topic Divider Page ─────────────────────────────────────────────────────────
def build_topic_divider(folder_name: str, colour, styles: dict) -> list:
    num_match = re.match(r"^(\d+)", folder_name)
    num_str   = num_match.group(1) if num_match else "?"
    name      = re.sub(r"^\d+_", "", folder_name).replace("_", " ")

    return [
        PageBreak(),
        Spacer(1, 35*mm),
        Paragraph(num_str, ParagraphStyle(
            "dn", fontName="Helvetica-Bold", fontSize=80,
            textColor=colors.HexColor("#f0f0f0"),
            alignment=TA_CENTER, leading=90)),
        Spacer(1, 4*mm),
        Paragraph(name, ParagraphStyle(
            "dt", fontName="Helvetica-Bold", fontSize=28,
            textColor=colour, alignment=TA_CENTER, leading=36)),
        Spacer(1, 6*mm),
        HRFlowable(width="60%", thickness=3, color=colour),
        PageBreak(),
    ]


# ── Inline Markdown Formatter ──────────────────────────────────────────────────
def inline_fmt(text: str) -> str:
    text = text.replace("&","&amp;").replace("<","&lt;").replace(">","&gt;")
    text = re.sub(r"\*\*\*(.+?)\*\*\*", r"<b><i>\1</i></b>", text)
    text = re.sub(r"\*\*(.+?)\*\*",     r"<b>\1</b>",         text)
    text = re.sub(r"__(.+?)__",         r"<b>\1</b>",         text)
    text = re.sub(r"\*(.+?)\*",         r"<i>\1</i>",         text)
    text = re.sub(r"`(.+?)`",
        r'<font name="Courier" color="#5382a1">\1</font>', text)
    text = re.sub(r"\[(.+?)\]\(.+?\)", r"\1", text)
    return text


# ── Markdown → Flowables ───────────────────────────────────────────────────────
def md_to_flowables(md_text: str, styles: dict) -> list:
    flowables = []
    lines = md_text.splitlines()
    i = 0

    while i < len(lines):
        line    = lines[i]
        stripped = line.strip()

        # Fenced code block
        if stripped.startswith("```"):
            code_lines = []
            i += 1
            while i < len(lines) and not lines[i].strip().startswith("```"):
                l = lines[i]
                if len(l) > 100:
                    l = l[:97] + "..."
                code_lines.append(l)
                i += 1
            flowables.append(Spacer(1, 4))
            flowables.append(CodeBlock(
                "\n".join(code_lines), bg=C_CODE_BG, border=C_CODE_BORD))
            flowables.append(Spacer(1, 6))
            i += 1
            continue

        # Headings
        if stripped.startswith("#### "):
            flowables.append(Paragraph(inline_fmt(stripped[5:]), styles["h4"]))
        elif stripped.startswith("### "):
            flowables.append(Paragraph(inline_fmt(stripped[4:]), styles["h3"]))
        elif stripped.startswith("## "):
            flowables.append(Paragraph(inline_fmt(stripped[3:]), styles["h2"]))
        elif stripped.startswith("# "):
            flowables.append(Paragraph(inline_fmt(stripped[2:]), styles["h1"]))

        # HR
        elif stripped in ("---", "***", "___"):
            flowables.append(HRFlowable(
                width="100%", thickness=0.5, color=C_RULE))

        # Blockquote
        elif stripped.startswith("> "):
            qt = Table([[Paragraph(inline_fmt(stripped[2:]), styles["note"])]],
                       colWidths=[155*mm])
            qt.setStyle(TableStyle([
                ("BACKGROUND",   (0,0),(-1,-1), colors.HexColor("#fff8c5")),
                ("LINEBEFORE",   (0,0),(-1,-1), 3, C_JAVA),
                ("LEFTPADDING",  (0,0),(-1,-1), 12),
                ("TOPPADDING",   (0,0),(-1,-1), 6),
                ("BOTTOMPADDING",(0,0),(-1,-1), 6),
            ]))
            flowables.append(qt)

        # Bullet list
        elif stripped.startswith("- ") or stripped.startswith("* "):
            items = []
            while i < len(lines) and (
                lines[i].strip().startswith("- ") or
                lines[i].strip().startswith("* ")
            ):
                items.append(ListItem(
                    Paragraph(inline_fmt(lines[i].strip()[2:]),
                               styles["bullet"]),
                    bulletColor=C_ACCENT))
                i += 1
            flowables.append(ListFlowable(
                items, bulletType="bullet",
                bulletFontName="Helvetica", bulletFontSize=10,
                bulletColor=C_ACCENT, start="•", leftIndent=16))
            continue

        # Numbered list
        elif re.match(r"^\d+\.\s", stripped):
            items = []
            n = 1
            while i < len(lines) and re.match(r"^\d+\.\s", lines[i].strip()):
                items.append(ListItem(
                    Paragraph(inline_fmt(
                        re.sub(r"^\d+\.\s","",lines[i].strip())),
                        styles["bullet"]),
                    value=n))
                n += 1
                i += 1
            flowables.append(ListFlowable(items, bulletType="1"))
            continue

        # Markdown table
        elif stripped.startswith("|") and "|" in stripped:
            table_lines = []
            while i < len(lines) and lines[i].strip().startswith("|"):
                table_lines.append(lines[i].strip())
                i += 1
            rows = [r for r in table_lines
                    if not re.match(r"^\|[-| :]+\|$", r)]
            if rows:
                col_count = min(max(len(r.strip("|").split("|")) for r in rows), 6)
                col_w = 155*mm / col_count
                data = []
                for row in rows:
                    cells = [c.strip() for c in row.strip("|").split("|")]
                    cells = cells[:col_count]
                    while len(cells) < col_count:
                        cells.append("")
                    data.append([Paragraph(inline_fmt(c), styles["body"])
                                 for c in cells])
                tbl = Table(data, colWidths=[col_w]*col_count, repeatRows=1)
                tbl.setStyle(TableStyle([
                    ("BACKGROUND",    (0,0),(-1,0), C_PRIMARY),
                    ("TEXTCOLOR",     (0,0),(-1,0), colors.white),
                    ("FONTNAME",      (0,0),(-1,0), "Helvetica-Bold"),
                    ("FONTSIZE",      (0,0),(-1,0), 9),
                    ("ROWBACKGROUNDS",(0,1),(-1,-1), [colors.white, C_CODE_BG]),
                    ("GRID",          (0,0),(-1,-1), 0.4, C_RULE),
                    ("TOPPADDING",    (0,0),(-1,-1), 5),
                    ("BOTTOMPADDING", (0,0),(-1,-1), 5),
                    ("LEFTPADDING",   (0,0),(-1,-1), 8),
                ]))
                flowables.append(Spacer(1, 6))
                flowables.append(tbl)
                flowables.append(Spacer(1, 6))
            continue

        # Empty line
        elif not stripped:
            flowables.append(Spacer(1, 5))

        # Regular paragraph
        else:
            try:
                flowables.append(Paragraph(inline_fmt(stripped), styles["body"]))
            except Exception:
                flowables.append(Paragraph(stripped[:200], styles["body"]))

        i += 1

    return flowables


# ── Java File → Flowables ──────────────────────────────────────────────────────
def java_to_flowables(java_path: Path, styles: dict) -> list:
    """Reads a .java file and returns a labelled CodeBlock."""
    try:
        code = java_path.read_text(encoding="utf-8", errors="replace")
    except Exception as e:
        return [Paragraph(f"[Could not read {java_path.name}: {e}]",
                          styles["body"])]

    flowables = []
    flowables.append(Spacer(1, 6))
    flowables.append(Paragraph(
        f"<b>{java_path.name}</b>", styles["java_file_heading"]))
    flowables.append(CodeBlock(
        code,
        bg=C_JAVA_BG,
        border=C_JAVA_BORD,
        label=java_path.name,
    ))
    flowables.append(Spacer(1, 8))
    return flowables


# ── Folder Sort Key ───────────────────────────────────────────────────────────
def folder_sort_key(p: Path) -> tuple:
    m = re.match(r"^(\d+)", p.name)
    num = int(m.group(1)) if m else 999
    return (num, p.name)   # secondary sort: alphabetical (handles duplicates)


# ── Main Build ─────────────────────────────────────────────────────────────────
def build_pdf():
    styles = build_styles()

    # Collect all topic folders
    topic_dirs = sorted(
        [d for d in SOURCE.iterdir()
         if d.is_dir() and not d.name.startswith(".")],
        key=folder_sort_key
    )

    # Count .java files for cover badge
    java_count = sum(
        len(list((d / "examples").glob("*.java")))
        for d in topic_dirs
        if (d / "examples").exists()
    )

    print(f"Found {len(topic_dirs)} topic folders, {java_count} .java files.\n")

    doc = SimpleDocTemplate(
        str(OUTPUT_PDF),
        pagesize=A4,
        leftMargin=20*mm, rightMargin=20*mm,
        topMargin=22*mm,  bottomMargin=22*mm,
        title="Java Guide — Complete Documentation",
        author="CoreJava_Basics",
        subject="Core Java from Introduction to Spring Boot",
    )

    story = []

    # Cover
    story += build_cover(styles, len(topic_dirs), java_count)

    # TOC
    story += build_toc(topic_dirs, styles)

    # Handle root-level 00_Complete_Learning_Path.md if it exists
    root_md = SOURCE / "00_Complete_Learning_Path.md"
    if root_md.exists():
        story.append(TopicBookmark("00_Complete_Learning_Path"))
        story += build_topic_divider("00_Complete_Learning_Path",
                                      C_JAVA, styles)
        md_text = root_md.read_text(encoding="utf-8", errors="replace")
        story += md_to_flowables(md_text, styles)

    processed = 0

    for folder in topic_dirs:
        # Skip if no examples subfolder
        examples_dir = folder / "examples"
        if not examples_dir.exists():
            continue

        num_match    = re.match(r"^(\d+)", folder.name)
        topic_num    = int(num_match.group(1)) if num_match else 99
        topic_colour = get_topic_colour(topic_num)
        topic_name   = folder.name

        # Topic bookmark for running header
        story.append(TopicBookmark(topic_name))

        # Topic divider page
        story += build_topic_divider(topic_name, topic_colour, styles)

        # .md file in examples/
        md_files = sorted(examples_dir.glob("*.md"))
        for md_file in md_files:
            story.append(Paragraph(
                str(md_file.relative_to(SOURCE)),
                styles["file_label"]))
            try:
                md_text = md_file.read_text(encoding="utf-8", errors="replace")
                story += md_to_flowables(md_text, styles)
            except Exception as e:
                story.append(Paragraph(
                    f"[Parse error in {md_file.name}: {e}]", styles["body"]))

        # Separator between .md and .java files
        story.append(Spacer(1, 6*mm))
        story.append(HRFlowable(
            width="100%", thickness=1.5, color=topic_colour))
        story.append(Paragraph(
            "Code Examples",
            ParagraphStyle("ce", fontName="Helvetica-Bold", fontSize=13,
                           textColor=topic_colour, leading=18,
                           spaceBefore=6, spaceAfter=6)))

        # Every .java file in examples/
        java_files = sorted(examples_dir.glob("*.java"))
        for jf in java_files:
            story += java_to_flowables(jf, styles)

        processed += 1
        if processed % 5 == 0:
            topic_display = re.sub(r"^\d+_", "", topic_name).replace("_", " ")
            print(f"  [Progress] {processed}/{len(topic_dirs)} topics — {topic_display}")

    # Build
    print(f"\nBuilding PDF from {processed} topics...")
    doc.multiBuild(
        story,
        onFirstPage=DocCanvas(),
        onLaterPages=DocCanvas(),
    )

    print(f"\n{'='*55}")
    print(f"DONE.")
    print(f"  Topics processed : {processed}")
    print(f"  Java files       : {java_count}")
    print(f"  Output           : {OUTPUT_PDF.resolve()}")
    print(f"{'='*55}")


if __name__ == "__main__":
    build_pdf()
