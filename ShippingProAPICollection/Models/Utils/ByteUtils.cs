using iText.Kernel.Pdf;
using iText.Kernel.Utils;

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
            using (var writerMemoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(writerMemoryStream))
                {
                    using (var mergedDocument = new PdfDocument(writer))
                    {
                        var merger = new PdfMerger(mergedDocument);

                        foreach (var pdfBytes in pdfs)
                        {
                            using (var copyFromMemoryStream = new MemoryStream(pdfBytes))
                            {
                                using (var reader = new PdfReader(copyFromMemoryStream))
                                {
                                    reader.SetUnethicalReading(true);
                                    using (var copyFromDocument = new PdfDocument(reader))
                                    {
                                        merger.Merge(copyFromDocument, 1, copyFromDocument.GetNumberOfPages());
                                    }
                                }
                            }
                        }
                    }
                }

                return writerMemoryStream.ToArray();
            }

        }
    }
}
