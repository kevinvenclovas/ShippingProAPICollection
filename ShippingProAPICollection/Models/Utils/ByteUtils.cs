using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ShippingProAPICollection.Models.Utils
{
    internal static class ByteUtils
    {
        /// <summary>
        /// Combinate multiple pdfs to one
        /// </summary>
        /// <param name="pdfs"></param>
        /// <returns></returns>
        internal static byte[] MergePDFByteToOnePDF(List<byte[]> pdfs)
        {
            using (var outputDocument = new PdfDocument())
            {
                foreach (var pdfBytes in pdfs)
                {
                    using (var inputStream = new MemoryStream(pdfBytes))
                    {
                        using (var inputDocument = PdfReader.Open(inputStream, PdfDocumentOpenMode.Import))
                        {
                            for (int i = 0; i < inputDocument.PageCount; i++)
                            {
                                outputDocument.AddPage(inputDocument.Pages[i]);
                            }
                        }
                    }
                }

                using (var outputStream = new MemoryStream())
                {
                    outputDocument.Save(outputStream, false);
                    return outputStream.ToArray();
                }
            }
        }
    }
}
