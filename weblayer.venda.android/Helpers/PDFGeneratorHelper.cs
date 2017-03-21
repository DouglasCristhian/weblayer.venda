using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.Helpers
{
    public class PDFGeneratorHelper
    {
        IList<PedidoItem> listaPedItem;
        PdfPTable table0 = new PdfPTable(1);
        PdfPTable table = new PdfPTable(6);
        PdfPTable table2 = new PdfPTable(1);

        public string GeneratePDF(Pedido pedido)
        {
            PedidoItemRepository repo = new PedidoItemRepository();
            listaPedItem = repo.ListPedItem(pedido.id);

            #region DefinirDiretorioEDoc
            var directory = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory, "W Venda Pro - PDFs").ToString();
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var path = Path.Combine(directory, "Pedido " + pedido.id_codigo.ToString() + ", Cliente " + pedido.ds_cliente.ToString() + ".pdf");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var fs = new System.IO.FileStream(path, System.IO.FileMode.CreateNew);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            #endregion DefinirDiretorioEDoc

            document.Add(table0);

            Paragraph ParagrafoCapa = PopularCapaPedido(pedido);
            document.Add(ParagrafoCapa);

            HeaderItens();
            PopularListaPedidoItem();
            table.SetTotalWidth(new float[] { 10, 5, 5, 5, 5, 5 });
            document.Add(table);

            HeaderResumo();
            document.Add(table2);

            Paragraph ParagrafoResumo = PopularResumoPedido(pedido);
            document.Add(ParagrafoResumo);

            document.Close();
            writer.Close();
            fs.Close();

            return path;
        }

        private void PopularListaPedidoItem()
        {
            foreach (var item in listaPedItem)
            {
                double vl_total = item.vl_Venda * item.nr_quantidade;

                PdfPCell cellProd = new PdfPCell(new Phrase(item.ds_produto.ToString(), new Font(Font.HELVETICA, 10f, Font.NORMAL, Color.BLACK)));
                cellProd.HorizontalAlignment = 0;
                table.AddCell(cellProd);

                PdfPCell cellVlLista = new PdfPCell(new Phrase(item.vl_Lista.ToString("##,##0.00"), new Font(Font.HELVETICA, 10f, Font.NORMAL, Color.BLACK)));
                cellVlLista.HorizontalAlignment = 2;
                table.AddCell(cellVlLista);

                PdfPCell cellVlVenda = new PdfPCell(new Phrase(item.vl_Venda.ToString("##,##0.00"), new Font(Font.HELVETICA, 10f, Font.NORMAL, Color.BLACK)));
                cellVlVenda.HorizontalAlignment = 2;
                table.AddCell(cellVlVenda);

                PdfPCell cellQTD = new PdfPCell(new Phrase(item.nr_quantidade.ToString(), new Font(Font.HELVETICA, 10f, Font.NORMAL, Color.BLACK)));
                cellQTD.HorizontalAlignment = 2;
                table.AddCell(cellQTD);

                PdfPCell cellDesc = new PdfPCell(new Phrase(item.vl_Desconto.ToString("##,##0.00"), new Font(Font.HELVETICA, 10f, Font.NORMAL, Color.BLACK)));
                cellDesc.HorizontalAlignment = 2;
                table.AddCell(cellDesc);

                PdfPCell cellVlTotal = new PdfPCell(new Phrase(vl_total.ToString("##,##0.00"), new Font(Font.HELVETICA, 10f, Font.NORMAL, Color.BLACK)));
                cellVlTotal.HorizontalAlignment = 2;
                table.AddCell(cellVlTotal);

                //table.AddCell(new Phrase("Resumo do Pedido", new Font(Font.NORMAL, 15f, Font.BOLD, Color.BLACK)));
                //table.AddCell(item.ds_produto.ToString());
                //table.AddCell(item.vl_Lista.ToString("##,##0.00"));
                //table.AddCell(item.vl_Venda.ToString("##,##0.00"));
                //table.AddCell(item.nr_quantidade.ToString());
                //table.AddCell(item.vl_Desconto.ToString("##,##0.00"));
                //table.AddCell(vl_total.ToString("##,##0.00"));
            }
        }

        private void HeaderItens()
        {
            PdfPCell headerItens = new PdfPCell(new Phrase("Itens", new Font(Font.NORMAL, 15f, Font.NORMAL, Color.BLACK)));
            headerItens.BackgroundColor = new iTextSharp.text.Color(211, 211, 211);
            headerItens.Colspan = 6;
            headerItens.HorizontalAlignment = 1;
            table.SpacingBefore = 30f;
            table.SpacingAfter = 30f;
            table.AddCell(headerItens);

            PdfPCell cellProd = new PdfPCell(new Phrase("Produto", new Font(Font.HELVETICA, 10f, Font.BOLD, Color.BLACK)));
            cellProd.BackgroundColor = new Color(211, 211, 211);
            table.AddCell(cellProd);

            PdfPCell cellVlLista = new PdfPCell(new Phrase("Vlr. Lista", new Font(Font.HELVETICA, 10f, Font.BOLD, Color.BLACK)));
            cellVlLista.BackgroundColor = new Color(211, 211, 211);
            table.AddCell(cellVlLista);

            PdfPCell cellVlVenda = new PdfPCell(new Phrase("Vlr. Venda", new Font(Font.HELVETICA, 10f, Font.BOLD, Color.BLACK)));
            cellVlVenda.BackgroundColor = new Color(211, 211, 211);
            table.AddCell(cellVlVenda);

            PdfPCell cellQTD = new PdfPCell(new Phrase("Qtd.", new Font(Font.HELVETICA, 10f, Font.BOLD, Color.BLACK)));
            cellQTD.BackgroundColor = new Color(211, 211, 211);
            table.AddCell(cellQTD);

            PdfPCell cellDesc = new PdfPCell(new Phrase("Desc.", new Font(Font.HELVETICA, 10f, Font.BOLD, Color.BLACK)));
            cellDesc.BackgroundColor = new Color(211, 211, 211);
            table.AddCell(cellDesc);

            PdfPCell cellVlTotal = new PdfPCell(new Phrase("Vlr. Total", new Font(Font.HELVETICA, 10f, Font.BOLD, Color.BLACK)));
            cellVlTotal.BackgroundColor = new Color(211, 211, 211);
            table.AddCell(cellVlTotal);
        }

        private void HeaderResumo()
        {
            PdfPCell headerResumo = new PdfPCell(new Phrase("Resumo do Pedido", new Font(Font.NORMAL, 15f, Font.BOLD, Color.BLACK)));
            headerResumo.BackgroundColor = new iTextSharp.text.Color(211, 211, 211);
            headerResumo.Colspan = 6;
            headerResumo.HorizontalAlignment = 1;
            table2.SpacingAfter = 20f;
            table2.AddCell(headerResumo);
        }

        private Paragraph PopularCapaPedido(Pedido pedido)
        {
            Paragraph paragraph = new Paragraph("Cliente: " + pedido.ds_cliente
                                            + "\n\nVendedor: " + pedido.ds_vendedor
                                              + "\n\nData de Emissão: " + pedido.dt_emissao.Value.ToString("dd/MM/yyyy"));
            paragraph.IndentationLeft = 50f;

            return paragraph;
        }

        private Paragraph PopularResumoPedido(Pedido pedido)
        {
            double ValorLiquido = 0;

            ValorLiquido += (pedido.vl_total - pedido.vl_descontoTotal);

            Paragraph paragraph = new Paragraph("Valor Total: " + pedido.vl_total.ToString("##,##0.00")
                                            + "\n\nValor Desconto: " + pedido.vl_descontoTotal.ToString("##,##0.00")
                                              + "\n\nValor Líquido: " + ValorLiquido.ToString("##,##0.00")
                                              + "\n\nVolume: " + pedido.vl_volume
                                              + "\n\nMensagem Pedido: " + pedido.ds_MsgPedido
                                              + "\n\nMensagem NF: " + pedido.ds_MsgNF);
            paragraph.IndentationLeft = 50f;

            return paragraph;
        }
    }
}