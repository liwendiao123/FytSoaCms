using Microsoft.AspNetCore.Http;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FytSoa.Common.OfficeHelper
{
   public class WordHelper
    {

        //读段落
        public static string ExcuteWordText(string path)
        {
            StringBuilder sb = new StringBuilder();
            using (FileStream stream = File.OpenRead(path))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                foreach (var para in doc.Paragraphs)
                {
                    string text = para.ParagraphText; //获得文本
                    var runs = para.Runs;
                    string styleid = para.Style;
                    for (int i = 0; i < runs.Count; i++)
                    {
                        var run = runs[i];
                        text = run.ToString(); //获得run的文本
                        sb.Append(text + ",");
                    }
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 设置单元格格式
        /// </summary>
        /// <param name="doc">doc对象</param>
        /// <param name="table">表格对象</param>
        /// <param name="setText">要填充的文字</param>
        /// <param name="align">文字对齐方式</param>
        /// <param name="textPos">rows行的高度</param>
        /// <returns></returns>
        public XWPFParagraph SetCellText(XWPFDocument doc, XWPFTable table, string setText, ParagraphAlignment align, int textPos)
        {
            CT_P para = new CT_P();
            XWPFParagraph pCell = new XWPFParagraph(para, table.Body);
            //pCell.Alignment = ParagraphAlignment.LEFT;//字体
            pCell.Alignment = align;

            XWPFRun r1c1 = pCell.CreateRun();
            r1c1.SetText(setText);
            r1c1.FontSize = 12;
            r1c1.SetFontFamily("华文楷体", FontCharRange.None);//设置雅黑字体
            r1c1.SetTextPosition(textPos);//设置高度

            return pCell;
        }


        /// <summary>
        /// 设置字体格式
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="table"></param>
        /// <param name="setText"></param>
        /// <returns></returns>
        public XWPFParagraph SetCellText(XWPFDocument doc, XWPFTable table, string setText)
        {
            //table中的文字格式设置
            CT_P para = new CT_P();
            XWPFParagraph pCell = new XWPFParagraph(para, table.Body);
            pCell.Alignment = ParagraphAlignment.CENTER;//字体居中
            pCell.VerticalAlignment = TextAlignment.CENTER;//字体居中

            XWPFRun r1c1 = pCell.CreateRun();
            r1c1.SetText(setText);
            r1c1.FontSize = 12;
            r1c1.SetFontFamily("华文楷体", FontCharRange.None);//设置雅黑字体
            r1c1.SetTextPosition(20);//设置高度

            return pCell;
        }



 

    }
}
