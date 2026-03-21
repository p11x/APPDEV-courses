# PDF Generation

## 📌 What You'll Learn

- Generating PDFs with Puppeteer/pdfkit

## 💻 Code Example

```js
import pdfkit from 'pdfkit';

app.get('/invoice/:id/pdf', (req, res) => {
  const doc = new pdfkit();
  
  res.setHeader('Content-Type', 'application/pdf');
  res.setHeader('Content-Disposition', 'attachment; filename=invoice.pdf');
  
  doc.pipe(res);
  
  doc.fontSize(25).text('Invoice #' + req.params.id);
  doc.text('Amount: $100.00');
  
  doc.end();
});
```
