using ClosedXML.Excel;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.ListingModels;
using DietiEstate.Core.Enums;

namespace DietiEstate.Infrastructure.Services;

public class ExcelService : IExcelService
{
    public async Task<byte[]> GetReportForAgent(IList<Listing>  agentListings)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Report");

        var totalViews = 0;
        var totalOffers = 0;
        var totalBookings = 0;
        var j = 1;
        var i = 1;

        worksheet.Cell(i, j++).Value = "Nome";
        worksheet.Cell(i, j++).Value = "Indirizzo";
        worksheet.Cell(i, j++).Value = "Tipologia";
        worksheet.Cell(i, j++).Value = "Data vendita";
        worksheet.Cell(i, j++).Value = "Visualizzazioni";
        worksheet.Cell(i, j++).Value = "Offerte ricevute";
        worksheet.Cell(i, j++).Value = "Visite prenotate";
        worksheet.Row(i).Style.Font.Bold = true;
        j = 1;
        for (i=0; i < agentListings.Count(); i++)
        {
            worksheet.Cell(i+2, j++).Value = agentListings[i].Name;
            worksheet.Cell(i+2, j++).Value = agentListings[i].Address;
            worksheet.Cell(i+2, j++).Value = agentListings[i].Type.Name;
            worksheet.Cell(i+2, j++).Value = agentListings[i].Available ? "" : $"{agentListings[i].ListingOffers.First(o => o.Status==OfferStatus.Accepted).Date:dd/MM/yyyy}";
            worksheet.Cell(i+2, j++).Value = agentListings[i].Views;
            worksheet.Cell(i+2, j++).Value = agentListings[i].ListingOffers.Count;
            worksheet.Cell(i+2, j).Value = agentListings[i].ListingBookings.Count;
            
            totalViews += agentListings[i].Views;
            totalOffers += agentListings[i].ListingOffers.Count;
            totalBookings += agentListings[i].ListingBookings.Count;
            j = 1;
        }
        i+=3;
        worksheet.Cell(i, j++).Value = "Visualizzazioni totali";
        worksheet.Cell(i, j++).Value = "Offerte totali ricevute";
        worksheet.Cell(i++, j).Value = "Visite totali prenotate";
        j = 1;
        worksheet.Cell(i, j++).Value = totalViews;
        worksheet.Cell(i, j++).Value = totalOffers;
        worksheet.Cell(i, j++).Value = totalBookings;

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

}