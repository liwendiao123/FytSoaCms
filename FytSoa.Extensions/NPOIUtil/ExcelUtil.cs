﻿using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FytSoa.Extensions.NPOIUtil
{
   public class ExcelUtil
    {
        public void Test()
        {

            var newFile = @"newbook.core.xlsx";

            using (var fs = new FileStream(newFile, FileMode.Create, FileAccess.Write))
            {

                IWorkbook workbook = new XSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet("Sheet1");

                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                var rowIndex = 0;
                IRow row = sheet1.CreateRow(rowIndex);
                row.Height = 30 * 80;
                row.CreateCell(0).SetCellValue("this is content");
                sheet1.AutoSizeColumn(0);
                rowIndex++;

                var sheet2 = workbook.CreateSheet("Sheet2");
                var style1 = workbook.CreateCellStyle();
                style1.FillForegroundColor = HSSFColor.Blue.Index2;
                style1.FillPattern = FillPattern.SolidForeground;

                var style2 = workbook.CreateCellStyle();
                style2.FillForegroundColor = HSSFColor.Yellow.Index2;
                style2.FillPattern = FillPattern.SolidForeground;

                var cell2 = sheet2.CreateRow(0).CreateCell(0);
                cell2.CellStyle = style1;
                cell2.SetCellValue(0);

                cell2 = sheet2.CreateRow(1).CreateCell(0);
                cell2.CellStyle = style2;
                cell2.SetCellValue(1);

                workbook.Write(fs);
            }

        }

        public void Test2()
        {
            var newFile2 = @"newbook.core.docx";
            using (var fs = new FileStream(newFile2, FileMode.Create, FileAccess.Write))
            {
                XWPFDocument doc = new XWPFDocument();
                var p0 = doc.CreateParagraph();
                p0.Alignment = ParagraphAlignment.CENTER;
                XWPFRun r0 = p0.CreateRun();
                r0.FontFamily = "microsoft yahei";
                r0.FontSize = 18;
                r0.IsBold = true;
                r0.SetText("This is title");

                var p1 = doc.CreateParagraph();
                p1.Alignment = ParagraphAlignment.LEFT;
                p1.IndentationFirstLine = 500;
                XWPFRun r1 = p1.CreateRun();
                r1.FontFamily = "·ÂËÎ";
                r1.FontSize = 12;
                r1.IsBold = true;
                r1.SetText("This is content, content content content content content content content content content");

                doc.Write(fs);
            }
        }


        public void ReadWord()
        {

    //        HWPFDocumentCore wordDocument = WordToHtmlUtils.loadDoc(new FileInputStream("D:\\temp\\seo\\1.doc"));

    //        WordToHtmlConverter wordToHtmlConverter = new WordToHtmlConverter(
    //                DocumentBuilderFactory.newInstance().newDocumentBuilder()
    //                        .newDocument());
    //        wordToHtmlConverter.processDocument(wordDocument);
    //        Document htmlDocument = wordToHtmlConverter.getDocument();
    //        ByteArrayOutputStream out = new ByteArrayOutputStream();
    //        DOMSource domSource = new DOMSource(htmlDocument);
    //        StreamResult streamResult = new StreamResult(out);

    //        TransformerFactory tf = TransformerFactory.newInstance();
    //        Transformer serializer = tf.newTransformer();
    //        serializer.setOutputProperty(OutputKeys.ENCODING, "UTF-8");
    //        serializer.setOutputProperty(OutputKeys.INDENT, "yes");
    //        serializer.setOutputProperty(OutputKeys.METHOD, "html");
    //        serializer.transform(domSource, streamResult);
    //out.close();

    //        String result = new String(out.toByteArray());
    //        System.out.println(result);
        }
    }
}
