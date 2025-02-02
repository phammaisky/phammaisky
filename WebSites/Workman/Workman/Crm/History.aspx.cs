using _IQwinwin;
using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class History : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SelectAllHistory();
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
    }

    protected void SelectAllHistory()
    {
        try
        {
            ai MaFoodCourt = a.QueryID("Card");

            ai MaQuay = a.QueryText("MaQuay");
            ai MaNV = a.QueryText("MaNV");
            ai CaBan = a.QueryText("CaBan");

            ai SoTien = a.e;
            ai SoTien_Co_The_Dung = a.e;
            ai SoTien_Co_The_Rut = a.e;

            ai TrangThai = a.e;
            ai NgayHetHan = a.e;

            ai Thang_Nam = a.Time.Month.ai().Add0Before(2) + a.Time.Year;

            int Type = 0;

            if (MaFoodCourt.Length == 11)
            {
                Type = 1;
            }
            else
            {
                if ((MaQuay != a.e) && (MaNV != a.e) && (CaBan != a.e))
                {
                    Type = 2;
                }
            }

            ai query = a.e;

            if (Type == 1)
            {
                query =

                    " USE TOPOS_DB"

                    //+ " DECLARE @CTKichHoatID NVARCHAR(MAX) SET @CTKichHoatID = (SELECT TOP 1 CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @MaFoodCourt) ORDER BY NgayKichHoat DESC)"

                    + " SELECT TenHH AS Hang_Hoa, TenHinhThuc AS Hinh_Thuc, CONVERT(BIGINT, TienTruocThanhToan) AS Truoc, CONVERT(BIGINT, ThanhTien) AS So_Tien, CONVERT(BIGINT, TienSauThanhToan) AS Sau, Nap_Rut_Alias.MaHD AS Hoa_Don, CONVERT(DATETIME, NgayGioQuet, 103) AS NgayGioQuet"

                    + " FROM TTT_CTNapTien_HinhThucThanhToan Nap_Rut_Alias"

                    + " LEFT JOIN CTHoaDon" + Thang_Nam + " Hoa_Don_Alias"
                    + " ON Nap_Rut_Alias.HoaDonID = Hoa_Don_Alias.MaHD"

                    + " JOIN HinhThucThanhToan Hinh_Thuc_Thanh_Toan_Alias"
                    + " ON Nap_Rut_Alias.MaHinhThuc = Hinh_Thuc_Thanh_Toan_Alias.MaHinhThuc"

                    + " WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @MaFoodCourt)))"
                    + " ORDER BY Hoa_Don_Alias.NgayGioQuet DESC, Nap_Rut_Alias.CTNapTien_Chi_Tiet_ID DESC"
                    ;
            }
            else
            {
                if (Type == 2)
                {
                    query =

                        " USE TOPOS_DB"

                        + " SELECT TenHH AS Hang_Hoa, TenHinhThuc AS Hinh_Thuc, CONVERT(BIGINT, TienTruocThanhToan) AS Truoc, CONVERT(BIGINT, ThanhTien) AS So_Tien, CONVERT(BIGINT, TienSauThanhToan) AS Sau, Nap_Rut_Alias.MaHD AS Hoa_Don, CONVERT(DATETIME, NgayGioQuet, 103) AS NgayGioQuet, MaFoodCourt"

                        + " FROM TTT_CTNapTien_HinhThucThanhToan Nap_Rut_Alias"

                        + " JOIN TTT_CTKichHoat Kich_Hoat_Alias"
                        + " ON Nap_Rut_Alias.CTKichHoatID = Kich_Hoat_Alias.CTKichHoatID"

                        + " JOIN HoaDon" + Thang_Nam + " Hoa_Don_Alias"
                        + " ON Nap_Rut_Alias.HoaDonID = Hoa_Don_Alias.MaHD"

                        + " JOIN CTHoaDon" + Thang_Nam + " CT_Hoa_Don_Alias"
                        + " ON Nap_Rut_Alias.HoaDonID = CT_Hoa_Don_Alias.MaHD"

                        + " JOIN HinhThucThanhToan Hinh_Thuc_Thanh_Toan_Alias"
                        + " ON Nap_Rut_Alias.MaHinhThuc = Hinh_Thuc_Thanh_Toan_Alias.MaHinhThuc"

                        + " WHERE (Hoa_Don_Alias.MaQuay = @MaQuay) AND (Hoa_Don_Alias.MaNV = @MaNV) AND (Hoa_Don_Alias.CaBan = @CaBan) AND (CONVERT(DATE, Hoa_Don_Alias.NgayBatDau, 103) = CONVERT(DATE, GETDATE(), 103))"// AND (CONVERT(DATE, Hoa_Don_Alias.NgayKetThuc, 103) = CONVERT(DATE, GETDATE(), 103))"
                        + " ORDER BY CT_Hoa_Don_Alias.NgayGioQuet DESC, Nap_Rut_Alias.CTNapTien_Chi_Tiet_ID DESC"
                        ;
                }
            }

            if (query.NoEmpty)
            {
                asql sql = query.asql(
                     new nv("@MaFoodCourt", MaFoodCourt),
                     new nv("@MaQuay", MaQuay),
                     new nv("@MaNV", MaNV),
                     new nv("@CaBan", CaBan)
                     );
                sql.DataReader();

                int j1 = 0;
                Dynamic_Control_Holder_pnl.Controls.Clear();

                HtmlTable Report_htbl = htable.TBL("Dynamic_Content_To_Edit", "1", "1", 1, 0, 0, a.e);
                Dynamic_Control_Holder_pnl.Controls.Add(Report_htbl);

                Report_htbl.Style.Add("border-collapse", "collapse");
                Report_htbl.Style.Add("width", "100%");

                try
                {
                    while (sql.Data.Read())
                    {
                        ai Thao_Tac = sql.Data[0].aS();

                        if (Thao_Tac == "Kích hoạt thẻ thức ăn")
                            Thao_Tac = "Kích hoạt";
                        else
                        if ((Thao_Tac == "Nạp tiền thẻ thức ăn") || (Thao_Tac == "Nạp tiền vào thẻ thức ăn"))
                            Thao_Tac = "Nạp tiền";
                        else
                        if (Thao_Tac == "Rút tiền trong thẻ thức ăn")
                            Thao_Tac = "Rút tiền";

                        ai Hinh_Thuc_Thanh_Toan = sql.Data[1].aS();

                        if (Hinh_Thuc_Thanh_Toan == "Tiền Việt Nam")
                            Hinh_Thuc_Thanh_Toan = "Tiền mặt";
                        else
                        if (Hinh_Thuc_Thanh_Toan == "Foodcourt Card")
                            Hinh_Thuc_Thanh_Toan = "Mua đồ ăn";
                        else
                        if (Hinh_Thuc_Thanh_Toan == "Voucher Foodcourt")
                            Hinh_Thuc_Thanh_Toan = "Voucher";

                        ai So_Tien = sql.Data[3].aS();

                        if (Hinh_Thuc_Thanh_Toan == "Mua đồ ăn")
                        {
                            if (!So_Tien.StartsWith("-"))
                            {
                                So_Tien = "-" + So_Tien;
                            }
                        }

                        //
                        if (j1 == 0)
                        {
                            HtmlTableRow TR = htable.TR(a.e);
                            Report_htbl.Rows.Add(TR);

                            TR.BgColor = "#C0C0C0";

                            HtmlTableCell TD_1 = htable.TD(a.e, "100", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_1);
                            TD_1.Controls.Add(html.Literal("THAO TÁC"));

                            HtmlTableCell TD_2 = htable.TD(a.e, "100", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_2);
                            TD_2.Controls.Add(html.Literal("HÌNH THỨC"));

                            HtmlTableCell TD_3 = htable.TD(a.e, "55", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_3);
                            TD_3.Controls.Add(html.Literal("TRƯỚC"));

                            HtmlTableCell TD_4 = htable.TD(a.e, "55", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_4);
                            TD_4.Controls.Add(html.Literal("TIỀN"));

                            HtmlTableCell TD_5 = htable.TD(a.e, "55", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_5);
                            TD_5.Controls.Add(html.Literal("SAU"));

                            HtmlTableCell TD_6 = htable.TD(a.e, "120", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_6);
                            TD_6.Controls.Add(html.Literal("HÓA ĐƠN"));

                            HtmlTableCell TD_7 = htable.TD(a.e, "125", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR.Cells.Add(TD_7);
                            TD_7.Controls.Add(html.Literal("THỜI GIAN"));

                            if (Type == 2)
                            {
                                HtmlTableCell TD_8 = htable.TD(a.e, "90", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                                TR.Cells.Add(TD_8);
                                TD_8.Controls.Add(html.Literal("SỐ THẺ"));
                            }
                        }

                        HtmlTableRow TR_2 = htable.TR(a.e);
                        Report_htbl.Rows.Add(TR_2);

                        if (((j1 - 1) % 2) == 0)
                        {
                            //TR_2.BgColor = "#C0C0C0";
                        }

                        HtmlTableCell TD_2_1 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_1);
                        TD_2_1.Controls.Add(html.Literal(Thao_Tac));

                        HtmlTableCell TD_2_2 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_2);
                        TD_2_2.Controls.Add(html.Literal(Hinh_Thuc_Thanh_Toan));

                        HtmlTableCell TD_2_3 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_3);
                        TD_2_3.Controls.Add(html.Literal(sql.Data[2].ai().SplitThousand));

                        HtmlTableCell TD_2_4 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_4);
                        TD_2_4.Controls.Add(html.Literal(So_Tien.SplitThousand));

                        HtmlTableCell TD_2_5 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_5);
                        TD_2_5.Controls.Add(html.Literal(sql.Data[4].ai().SplitThousand));

                        HtmlTableCell TD_2_6 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_6);
                        TD_2_6.Controls.Add(html.Literal(sql.Data[5].aS()));

                        HtmlTableCell TD_2_7 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                        TR_2.Cells.Add(TD_2_7);
                        TD_2_7.Controls.Add(html.Literal(sql.Data[6].aS()));

                        if (Type == 2)
                        {
                            HtmlTableCell TD_8 = htable.TD(a.e, "125", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                            TR_2.Cells.Add(TD_8);
                            TD_8.Controls.Add(html.Literal(sql.Data[7].aS()));
                        }

                        j1++;
                    }
                }
                catch (SqlException sqlEx)
                {
                    lib.Log_Error(sqlEx);
                }
                finally
                {
                    sql.Close();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}