using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for EInvoice
/// </summary>
public class EInvoice
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AddlDocDtl
    {
        public string Url { get; set; }
        public string Docs { get; set; }
        public string Info { get; set; }
    }

    public class AttribDtl
    {
        public string Nm { get; set; }
        public string Val { get; set; }
    }

    public class BchDtls
    {
        public string Nm { get; set; }
        public string Expdt { get; set; }
        public string wrDt { get; set; }
    }

    public class BuyerDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Pos { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }
    }

    public class ContrDtl
    {
        public string RecAdvRefr { get; set; }
        public string RecAdvDt { get; set; }
        public string Tendrefr { get; set; }
        public string Contrrefr { get; set; }
        public string Extrefr { get; set; }
        public string Projrefr { get; set; }
        public string Porefr { get; set; }
        public string PoRefDt { get; set; }
    }

    public class DispDtls
    {
        public string Nm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }

    public class DocDtls
    {
        public string Typ { get; set; }
        public string No { get; set; }
        public string Dt { get; set; }
    }

    public class DocPerdDtls
    {
        public string InvStDt { get; set; }
        public string InvEndDt { get; set; }
    }

    public class EwbDtls
    {
        public string Transid { get; set; }
        public string Transname { get; set; }
        public int Distance { get; set; }
        public string Transdocno { get; set; }
        public string TransdocDt { get; set; }
        public string Vehno { get; set; }
        public string Vehtype { get; set; }
        public string TransMode { get; set; }
    }

    public class ExpDtls
    {
        public string ShipBNo { get; set; }
        public string ShipBDt { get; set; }
        public string Port { get; set; }
        public string RefClm { get; set; }
        public string ForCur { get; set; }
        public string CntCode { get; set; }
    }

    public class ItemList
    {
        public string SlNo { get; set; }
        public string IsServc { get; set; }
        public string PrdDesc { get; set; }
        public string HsnCd { get; set; }
        public string Barcde { get; set; }
        public BchDtls BchDtls { get; set; }
        public double Qty { get; set; }
        public int FreeQty { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double TotAmt { get; set; }
        public int Discount { get; set; }
        public double PreTaxVal { get; set; }
        public double AssAmt { get; set; }
        public int GstRt { get; set; }
        public double SgstAmt { get; set; }
        public double IgstAmt { get; set; }
        public double CgstAmt { get; set; }
        public int CesRt { get; set; }
        public double CesAmt { get; set; }
        public int CesNonAdvlAmt { get; set; }
        public int StateCesRt { get; set; }
        public double StateCesAmt { get; set; }
        public int StateCesNonAdvlAmt { get; set; }
        public int OthChrg { get; set; }
        public double TotItemVal { get; set; }
        public string OrdLineRef { get; set; }
        public string OrgCntry { get; set; }
        public string PrdSlNo { get; set; }
        public List<AttribDtl> AttribDtls { get; set; }
    }

    public class PayDtls
    {
        public string Nm { get; set; }
        public string Accdet { get; set; }
        public string Mode { get; set; }
        public string Fininsbr { get; set; }
        public string Payterm { get; set; }
        public string Payinstr { get; set; }
        public string Crtrn { get; set; }
        public string Dirdr { get; set; }
        public int Crday { get; set; }
        public int Paidamt { get; set; }
        public int Paymtdue { get; set; }
    }

    public class PrecDocDtl
    {
        public string InvNo { get; set; }
        public string InvDt { get; set; }
        public string OthRefNo { get; set; }
    }

    public class RefDtls
    {
        public string InvRm { get; set; }
        public DocPerdDtls DocPerdDtls { get; set; }
        public List<PrecDocDtl> PrecDocDtls { get; set; }
        public List<ContrDtl> ContrDtls { get; set; }
    }

    public class Root
    {
        public string Version { get; set; }
        public TranDtls TranDtls { get; set; }
        public DocDtls DocDtls { get; set; }
        public SellerDtls SellerDtls { get; set; }
        public BuyerDtls BuyerDtls { get; set; }
        public DispDtls DispDtls { get; set; }
        public ShipDtls ShipDtls { get; set; }
        public List<ItemList> ItemList { get; set; }
        public ValDtls ValDtls { get; set; }
        public PayDtls PayDtls { get; set; }
        public RefDtls RefDtls { get; set; }
        public List<AddlDocDtl> AddlDocDtls { get; set; }
        public ExpDtls ExpDtls { get; set; }
        public EwbDtls EwbDtls { get; set; }
    }


    public class SellerDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
        public string Ph { get; set; }
        public string Em { get; set; }
    }

    public class ShipDtls
    {
        public string Gstin { get; set; }
        public string LglNm { get; set; }
        public string TrdNm { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Loc { get; set; }
        public int Pin { get; set; }
        public string Stcd { get; set; }
    }

    public class TranDtls
    {
        public string TaxSch { get; set; }
        public string SupTyp { get; set; }
        public string RegRev { get; set; }
        //public object EcmGstin { get; set; }
        public string IgstOnIntra { get; set; }
    }

    public class ValDtls
    {
        public double AssVal { get; set; }
        public double CgstVal { get; set; }
        public double SgstVal { get; set; }
        public double IgstVal { get; set; }
        public double CesVal { get; set; }
        public double StCesVal { get; set; }
        public int Discount { get; set; }
        public int OthChrg { get; set; }
        public double RndOffAmt { get; set; }
        public double TotInvVal { get; set; }
        public double TotInvValFc { get; set; }
    }

    public double ConvertingLtrs(string qty, string uomqty, string rate)
    {
        //double[] ltrQty_ltrRate = new double[2];
        double givenqty = 0;
        double.TryParse(qty, out givenqty);
        double givenrate = 0;
        double.TryParse(rate, out givenrate);
        double givenuomqty = 0;
        double.TryParse(uomqty, out givenuomqty);

        //double perltrCost = 0;
        //perltrCost = (1000 / givenuomqty) * givenrate;
        //perltrCost = Math.Round(perltrCost, 2);

        double ltrqty = 0;
        ltrqty = givenqty * givenuomqty / 1000;

        //ltrQty_ltrRate[0] = ltrqty;
        return ltrqty;
    }
    public double Converting_Ltr_rate(string qty, string uomqty, string rate)
    {
        double givenqty = 0;
        double.TryParse(qty, out givenqty);
        double givenrate = 0;
        double.TryParse(rate, out givenrate);
        double givenuomqty = 0;
        double.TryParse(uomqty, out givenuomqty);

        double perltrCost = 0;
        perltrCost = (1000 / givenuomqty) * givenrate;
        perltrCost = Math.Round(perltrCost, 2);
        return perltrCost;
    }
    public double ConvertingPackets(string qty, string uomqty, string rate)
    {
        double givenqty = 0;
        double.TryParse(qty, out givenqty);
        double givenrate = 0;
        double.TryParse(rate, out givenrate);
        double givenuomqty = 0;
        double.TryParse(uomqty, out givenuomqty);



        //double pktrate = 0;
        //pktrate = (givenrate * givenuomqty) / 1000;
        //pktrate = Math.Round(pktrate, 2);

        double pktqty = 0;
        pktqty = givenqty * 1000 / givenuomqty;
        pktqty = Math.Round(pktqty, 2);

        //PktQty_pktRate[0] = pktqty;
        //PktQty_pktRate[1] = pktrate;
        return pktqty;
    }

    public double Converting_Packet_rate(string qty, string uomqty, string rate)
    {
        double givenqty = 0;
        double.TryParse(qty, out givenqty);
        double givenrate = 0;
        double.TryParse(rate, out givenrate);
        double givenuomqty = 0;
        double.TryParse(uomqty, out givenuomqty);


        double pktrate = 0;
        pktrate = (givenrate * givenuomqty) / 1000;
        pktrate = Math.Round(pktrate, 2);
        return pktrate;
    }
   
}