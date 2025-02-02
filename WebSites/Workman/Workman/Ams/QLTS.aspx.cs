using System.Linq;
using _IQwinwin;
using OfficeOpenXml;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using _4u4m;


public partial class AmsReport : Page
{
    ai onPageLoad = "AmsReport();";
    ai Control_Call_Postback = a.e;

    aint Page_Query_String = 1;
    ai Primary_Column = "ID";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!a.url.IsPageMethods_Undefined)
        //if (!new _4e().Check_PageMethods_Is_undefined())
        {
            //new _4e().Delete_Time_Out_File(Server.MapPath("~/File/"), 0);
            //new _4e().Delete_All_Empty_Directory(Server.MapPath("~/File/"), Server.MapPath("~/File/"));

            Session["Total_Result_Per_Page_For_Report"] = 120;

            if (!IsPostBack)
            {
                lib.Set_Index_Host(Index_Host_hdf, null);
                PageMethods_Path_hdf.Value = "/Ams/AmsReport.aspx";

                ViewState["ReportName"] = a.QueryText("R").aS();

                ai Checked = a.QueryText("CheckedBarcode");
                CheckedBarcode_ddl.rechoice(Checked);

                ai Cong_Ty_ID = a.QueryID("Cong_Ty_ID");
                ams.Creat_Cong_Ty_ddl(Cong_Ty_ddl);
                Cong_Ty_ddl.rechoice(Cong_Ty_ID);

                ai Phong_Ban_ID = a.QueryID("Phong_Ban_ID");
                ams.Creat_Phong_Ban_ddl(Cong_Ty_ddl.SelectedValue, Phong_Ban_ddl);
                Phong_Ban_ddl.rechoice(Phong_Ban_ID);

                ai Loai_Thiet_Bi_ID = a.QueryID("Loai_Thiet_Bi_ID");
                ams.Creat_Loai_Thiet_Bi_ddl(Cong_Ty_ddl.SelectedValue, Phong_Ban_ddl.SelectedValue, Loai_Thiet_Bi_ddl);
                Loai_Thiet_Bi_ddl.rechoice(Loai_Thiet_Bi_ID);

                ai Filter_ID_List = a.QueryText("ID");
                Filter_ID_List_tbx.Text = Filter_ID_List;

                cbxFilterName.Checked = a.QueryID("Search");

                //Date_Time_1
                atime Time_Start = a.QueryText("Time_Start").adate;
                atime Time_Finish = a.QueryText("Time_Finish").adate;
                //31/10/2019`tam rao lai de tesst
                if (Time_Start.Valid)
                    Time_Start_tbx.Text = Time_Start.FormatedDate;

                if (Time_Finish.Valid)
                    Time_Finish_tbx.Text = Time_Finish.FormatedDate;
            }
            else
            {
                /*
                Control_Call_Postback = a.url.PostbackControl;

                if (!Control_Call_Postback.Contains("Report_"))
                {
                    //Creat_Dynamic_Content_To_Edit(sender, e, true);
                }

                Control_Call_Postback = a.url.PostbackControl;
                */
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
         if (!a.url.IsPageMethods_Undefined)
       // if (!new _4e().Check_PageMethods_Is_undefined())
        {
            lib.Add_All_JavaScript_AND_CSS_File_To_Header_Basic(true);
            lib.Add_All_JavaScript_AND_CSS_File_To_Header();

            if (
            //(!Control_Call_Postback.Contains("OK_btn"))
            (!Control_Call_Postback.Contains("Edit_"))
            && (!Control_Call_Postback.Contains("Expand_"))
            && (!Control_Call_Postback.Contains("Report_"))
            //&& (!Control_Call_Postback.Contains("Download_Excel_All_btn"))
            )
            {
                SelectReportContent();
            }

            if (!IsPostBack)
                Page_Body.Attributes.Add("onload", onPageLoad);
            else
                lib.Run_JavaScript(onPageLoad);
        }
    }
    protected override void Render(HtmlTextWriter writer)
    {
         if (!a.url.IsPageMethods_Undefined)
        //if (!new _4e().Check_PageMethods_Is_undefined())

        {
            using (HtmlTextWriter tempWriter = new HtmlTextWriter(new StringWriter()))
            {
                base.Render(tempWriter);

                ai content = tempWriter.InnerWriter.aS();
                lib.Replace_Index_Host(ref content);

                writer.Write(content.aS());
            }
        }
    }

    protected void SelectReportContent()
    {
        ai ReportName = a.aS(ViewState["ReportName"]);
        ai pagingHtml = a.e;

        bool selectAll = a.QueryID("All").Bool;
        ai createBarcode = a.QueryID("Barcode");
        ai checkedBarcode = CheckedBarcode_ddl.SelectedValue;

        ai Cong_Ty_ID = Cong_Ty_ddl.SelectedValue;
        ai Phong_Ban_ID = Phong_Ban_ddl.SelectedValue;
        ai Loai_Thiet_Bi_ID = Loai_Thiet_Bi_ddl.SelectedValue;

        ai query = a.e;
        ai sqlWhere_CheckedBarcode = a.e;
        ai sqlWhere_Cong_Ty = a.e;
        ai sqlWhere_Phong_Ban = a.e;
        ai sqlWhere_Loai_Thiet_Bi = a.e;

        if (checkedBarcode == "1")
            sqlWhere_CheckedBarcode = " AND (Checked = 1)";
        else
        if (checkedBarcode == "0")
            sqlWhere_CheckedBarcode = " AND (Checked IS NULL)";

        if (Phong_Ban_ID.IsID && Phong_Ban_ID.NoZero)
            sqlWhere_Phong_Ban = " AND (HistoryUse.DeptId = '" + Phong_Ban_ID + "')";

        if (sqlWhere_Phong_Ban == a.e && Cong_Ty_ID.IsID && Cong_Ty_ID.NoZero)
            sqlWhere_Cong_Ty = " AND (DeviceAndTool.CompanyId = '" + Cong_Ty_ID + "')";

        if (Loai_Thiet_Bi_ID.IsID && Loai_Thiet_Bi_ID.NoZero)
            sqlWhere_Loai_Thiet_Bi = " AND (DeviceAndTool.DeviceCatId = '" + Loai_Thiet_Bi_ID + "')";

        //Filter_ID_List_tbx
        ai listFilter_ID = Filter_ID_List_tbx.Text;
        listFilter_ID.aRemSpace().aReplace(",", ";", ".", "-");
        Filter_ID_List_tbx.Text = listFilter_ID;

        aray array = listFilter_ID.aray('-', TypeAray.ID, true, true);
        ai sqlWhere_ID = array.ToSqlWhere(true, "DeviceAndTool.ID");

        if (cbxFilterName.Checked)
        {
            sqlWhere_ID = " AND (DeviceAndTool.AssetsCode LIKE N'" + listFilter_ID + " %')";
        }

        //Date_Time
        atime Time_Start = Time_Start_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish = Time_Finish_tbx.Text.ai().aRemove("__/__/____").adate;
        //31/10/2019: change
       if (!Time_Start.Valid)
      // if (Time_Start != "01/01/2009".atime(TypeTime.Date))
        {
            Time_Start = "01/01/2009".atime(TypeTime.Date);

            a.write(3, Time_Start.Valid, Time_Start.FormatedDate);
            Time_Start_tbx.Text = Time_Start.FormatedDate;
        }
        //31/10/2019: change
         if (!Time_Finish.Valid)
        //if (Time_Finish != "01/01/2009".atime(TypeTime.Date))
        {
            Time_Finish = a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year;
            Time_Finish_tbx.Text = Time_Finish.FormatedDate;
        }

        //Count
        if (checkedBarcode == "x")
        {
            query =
                " SELECT 1"
                ;
        }
        else
        {
            //Count
            query =
                "DECLARE @Time_Start NVARCHAR(MAX)"
                + " SET @Time_Start = CONVERT(DATETIME, @Time_Start_Temp, 103) + ' 00:00:00.000'"
                + " DECLARE @Time_Finish NVARCHAR(MAX)"
                + " SET @Time_Finish = CONVERT(DATETIME, @Time_Finish_Temp, 103) + ' 23:59:59.999'"

                + " SELECT COUNT(*)"
                + " FROM [PDSBitexco].[dbo].[DeviceAndTool]"

                + " LEFT JOIN [PDSBitexco].[dbo].HistoryUse"
                + " ON DeviceAndTool.Id = HistoryUse.DeviceToolId"

                + " LEFT JOIN [PDSBitexco].[dbo].Departments"
                + " ON HistoryUse.DeptId = Departments.Id"

                + " WHERE (" + sqlWhere_CheckedBarcode + sqlWhere_Cong_Ty + sqlWhere_Phong_Ban + sqlWhere_Loai_Thiet_Bi + sqlWhere_ID
                //+ " AND (BuyDate >= @Time_Start) AND (BuyDate <= @Time_Finish)"
                + ")"
                ;
        }

        if (query != a.e)
        {
            aint Total_Result = query.asql(
                new nv("Time_Start_Temp", "01-01-2000"),
                new nv("Time_Finish_Temp", "01-01-2020")).Scalar;

            /*
            new nv("@Time_Start_Temp", Time_Start.FormatedDate),
            new nv("@Time_Finish_Temp", Time_Finish.FormatedDate)
            */

            ViewState["Total_Result"] = Total_Result.Valid ? Total_Result.MainInt : 0;

            //Pageing
            if ((int)ViewState["Total_Result"] == 0)
            {
                Change_Page_1_div.Visible = true;
                Change_Page_2_div.Visible = false;

                Page_1_lbl.Visible = false;

                Total_Result_1_lbl.Text = "Not found any result...";
            }
            else
            {
                //Total_Page        
                if ((int)ViewState["Total_Result"] <= (int)Session["Total_Result_Per_Page_For_Report"])
                {
                    ViewState["Total_Page"] = 1;
                }
                else
                {
                    if ((int)ViewState["Total_Result"] % (int)Session["Total_Result_Per_Page_For_Report"] > 0)
                    {
                        ViewState["Total_Page"] = ((int)ViewState["Total_Result"] / (int)Session["Total_Result_Per_Page_For_Report"]) + 1;
                    }
                    else
                        ViewState["Total_Page"] = ((int)ViewState["Total_Result"] / (int)Session["Total_Result_Per_Page_For_Report"]);
                }

                //Total_Page_Group
                if ((int)(ViewState["Total_Page"]) <= 10)
                {
                    ViewState["Total_Page_Group"] = 1;
                }
                else
                {
                    if ((int)(ViewState["Total_Page"]) % 10 > 0)
                    {
                        ViewState["Total_Page_Group"] = ((int)(ViewState["Total_Page"]) / 10) + 1;
                    }
                    else
                        ViewState["Total_Page_Group"] = ((int)(ViewState["Total_Page"]) / 10);
                }

                int Page = 1;

                if (!IsPostBack)
                {
                    Page_Query_String = a.QueryID("Page");
                    Page = Page_Query_String.Valid ? Page_Query_String.MainInt : 1;
                }

                ViewState["Page"] = Page;
                ViewState["Page_Group"] = 1;

                if ((int)(ViewState["Page"]) % 10 > 0)
                    ViewState["Page_Group"] = ((int)(ViewState["Page"]) / 10) + 1;
                else
                    ViewState["Page_Group"] = ((int)(ViewState["Page"]) / 10);

                // 
                //Total_Page        
                if ((int)ViewState["Total_Page"] <= 1)
                {
                    Change_Page_1_div.Visible = false;
                    Change_Page_2_div.Visible = false;
                }
                else
                {
                    Change_Page_1_div.Visible = true;
                    Change_Page_2_div.Visible = true;

                    Page_1_lbl.Visible = true;

                    Total_Result_1_lbl.Text = "Found: " + ViewState["Total_Result"].ai().SplitThousand + " results (divide by " + ViewState["Total_Page"].ai().SplitThousand + " pages)";
                    Total_Result_2_lbl.Text = Total_Result_1_lbl.Text;

                    //
                    aurl url = new aurl();

                    url.Remove("Barcode");

                    url.Update("Cong_Ty_ID", Cong_Ty_ddl.SelectedValue);
                    url.Update("Phong_Ban_ID", Phong_Ban_ddl.SelectedValue);
                    url.Update("Loai_Thiet_Bi_ID", Loai_Thiet_Bi_ddl.SelectedValue);

                    url.Update("CheckedBarcode", CheckedBarcode_ddl.SelectedValue);

                    url.Update("ID", Filter_ID_List_tbx.Text.ai().UrlEncode);
                    url.Update("Search", cbxFilterName.Checked.abool().ToBit);

                    url.Update("Time_Start", Time_Start.FormatedDate.UrlEncode);
                    url.Update("Time_Finish", Time_Finish.FormatedDate.UrlEncode);

                    //
                    int Page_Number = ((((int)ViewState["Page_Group"]) - 1) * 10);

                    if (0 < Page_Number)
                    {
                        url.Update("Page", Page_Number.aS());

                        pagingHtml += "<a href='" + url + "'><img src='/Index/IMG/Button/Mui_Ten_01.jpg'></a> ... ";
                    }

                    for (int i1 = 1; i1 <= 10; i1++)
                    {
                        Page_Number = i1 + ((((int)ViewState["Page_Group"]) - 1) * 10);

                        if (Page_Number <= (int)ViewState["Total_Page"])
                        {
                            url.Update("Page", Page_Number.aS());

                            if (Page_Number == Page)
                            {
                                pagingHtml += "<a href='" + url + "'><span class='B Red' style='font-size: 12pt;'>" + Page_Number + "</span></a>";
                            }
                            else
                            {
                                pagingHtml += "<a href='" + url + "'>" + Page_Number + "</a>";
                            }

                            if ((i1 < 10) && (Page_Number < (int)ViewState["Total_Page"]))
                            {
                                pagingHtml += " - ";
                            }
                        }
                    }

                    Page_Number = 11 + ((((int)ViewState["Page_Group"]) - 1) * 10);

                    if (Page_Number <= (int)ViewState["Total_Page"])
                    {
                        url.Update("Page", Page_Number.aS());

                        pagingHtml += " ... <a href='" + url + "'><img src='/Index/IMG/Button/Mui_Ten_02.jpg'></a>";
                    }
                }

                //
                if (checkedBarcode == "x")
                {
                    //Read data
                    query =

                        "DECLARE @Time_Start NVARCHAR(MAX)"
                        + " SET @Time_Start = CONVERT(DATETIME, @Time_Start_Temp, 103) + ' 00:00:00.000'"
                        + " DECLARE @Time_Finish NVARCHAR(MAX)"
                        + " SET @Time_Finish = CONVERT(DATETIME, @Time_Finish_Temp, 103) + ' 23:59:59.999'"

                        + " USE PDSBitexco"

                        + " SELECT DeviceCatName AS Loai_Tai_San, COUNT(*) AS Tat_Ca, COUNT(Checked) AS Hien_Co, COUNT(*) - COUNT(Checked) AS Khong_Co"//(Checked = 1)

                        + " FROM DeviceAndTool"

                        + " LEFT JOIN DeviceCategory"
                        + " ON DeviceAndTool.DeviceCatId = DeviceCategory.Id"

                        + " LEFT JOIN [PDSBitexco].[dbo].HistoryUse"
                        + " ON DeviceAndTool.Id = HistoryUse.DeviceToolId"

                        + " LEFT JOIN [PDSBitexco].[dbo].Departments"
                        + " ON HistoryUse.DeptId = Departments.Id"

                        + " WHERE (" + sqlWhere_CheckedBarcode + sqlWhere_Cong_Ty + sqlWhere_Phong_Ban + sqlWhere_Loai_Thiet_Bi + sqlWhere_ID
                        //+ " AND (BuyDate >= @Time_Start) AND (BuyDate <= @Time_Finish)"
                        + ")"

                        + " GROUP BY DeviceAndTool.DeviceCatId, DeviceCatName"

                        + " ORDER BY DeviceCatName"
                        ;
                }
                else
                {
                    //Read data
                    query =

                        "DECLARE @Time_Start NVARCHAR(MAX)"
                        + " SET @Time_Start = CONVERT(DATETIME, @Time_Start_Temp, 103) + ' 00:00:00.000'"
                        + " DECLARE @Time_Finish NVARCHAR(MAX)"
                        + " SET @Time_Finish = CONVERT(DATETIME, @Time_Finish_Temp, 103) + ' 23:59:59.999'"

                        + " USE PDSBitexco"

                        + " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY DeviceAndTool.Id, DeviceCatName, AssetsCode, SAPCode) AS Number, DeviceAndTool.ID,"
                        + " AssetsCode AS Ma_Tai_San, SAPCode AS Ma_SAP, DeviceName AS Ten_Tai_San, DeviceCatName AS Loai_Tai_San, Checked, BuyDate AS Ngay_Mua, "
                        + " NameVn AS Don_Vi_Quan_Ly, DeptName AS Phong_Ban, FullName AS Nhan_Vien, LocationName AS Dia_Diem, HandedDate AS Ngay_Ban_Giao, StatusCategory.Name AS Tinh_Trang"

                        + " FROM DeviceAndTool"

                        + " LEFT JOIN HistoryUse"
                        + " ON DeviceAndTool.Id = HistoryUse.DeviceToolId"

                        + " LEFT JOIN DeviceCategory"
                        + " ON DeviceAndTool.DeviceCatId = DeviceCategory.Id"

                        + " LEFT JOIN Company"
                        + " ON DeviceAndTool.CompanyId = Company.Id"

                        + " LEFT JOIN Departments"
                        + " ON HistoryUse.DeptId = Departments.Id"

                        + " LEFT JOIN UserInfo"
                        + " ON HistoryUse.HandedToStaffId = UserInfo.Id"

                        + " LEFT JOIN Location"
                        + " ON HistoryUse.LocationId = Location.Id"

                        + " LEFT JOIN StatusCategory"
                        + " ON HistoryUse.StatusId = StatusCategory.Id"

                        + " WHERE (" + sqlWhere_CheckedBarcode + sqlWhere_Cong_Ty + sqlWhere_Phong_Ban + sqlWhere_Loai_Thiet_Bi + sqlWhere_ID
                        //+ " AND (BuyDate >= @Time_Start) AND (BuyDate <= @Time_Finish)"
                        + ")"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }

                if (query != a.e)
                {
                    //All Page
                    if (selectAll)
                        query.aRemove(" WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))");

                    asql sql = query.asql(
                         new nv("@Time_Start_Temp", "01-01-2000"),
                         new nv("@Time_Finish_Temp", "01-01-2020"));

                    sql.DataReader();

                    Dynamic_Control_Holder_pnl.Controls.Clear();

                    int j1 = 0;
                    int Total_Column = sql.Data.FieldCount;

                    HtmlTable Report_htbl = htable.TBL("Dynamic_Content_To_Edit", "1", "1", 1, 0, 0, a.e);
                    Dynamic_Control_Holder_pnl.Controls.Add(Report_htbl);

                    Report_htbl.Style.Add("border-collapse", "collapse");
                    Report_htbl.Style.Add("width", "98%");

                    //Excel

                    ai folderName = a.EncodedDateTime;
                    afolder folder = apath.AddFolder(Server.MapPath("~/File/Download/"), folderName);
                    folder.Create();

                    afile inputFile = apath.Add(Server.MapPath("~/File/Excel/Ams/KiemKe/"), "Kiem-ke-Tai-san.xlsx");
                    ExcelPackage excel = new ExcelPackage(inputFile);
                    var sheet = excel.Workbook.Worksheets[1];

                    afile outputFile = apath.Add(folder, "Kiem-ke-Tai-san_Result.xlsx");
                    ExcelPackage outputExcel = new ExcelPackage(outputFile);
                    var outputSheet = outputExcel.Workbook.Worksheets.Add("Quản lý Tài sản");

                    afile pdfFile = apath.Add(folder, "Barcode.pdf");

                    int Order = 0;

                    try
                    {
                        aray allPicture = new aray();
                        ai pictureFilePath = a.e;
                        int maxBarcodePerPicture = 12;

                        //using (Bitmap outputPicture = new Bitmap(807, 583 * 2 - 20))
                        using (Bitmap outputPicture = new Bitmap(406, 406))//376 //406, 609
                        {
                            outputPicture.SetResolution(1200, 1200);

                            using (Graphics graphic = Graphics.FromImage(outputPicture))
                            {
                                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                //graphic.TextRenderingHint = TextRenderingHint.AntiAlias;

                                while (sql.Data.Read())
                                {
                                    ai Primary_Key = a.e;
                                    ai Number = a.e;

                                    if (checkedBarcode != "x")
                                    {
                                        if (Primary_Column != a.e)
                                        {
                                            Primary_Key = sql.Data[Primary_Column.aS()].aS();
                                        }

                                        Number = sql.Data["Number"].ai().SplitThousand;
                                    }

                                    //
                                    if (j1 == 0)
                                    {
                                        HtmlTableRow TR = htable.TR(a.e);
                                        Report_htbl.Rows.Add(TR);

                                        if (createBarcode == a.e)
                                        {
                                            if (checkedBarcode != "x")
                                            {
                                                outputSheet.Cells[1, 3].Value = Report_lbl.Text;
                                                outputSheet.Cells[1, 3, 1, 9].Merge = true;

                                                outputSheet.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
                                                outputSheet.Cells[1, 3].Style.Font.Bold = true;
                                                outputSheet.Cells[1, 3].Style.Font.Size = 20;

                                                outputSheet.Cells[3, 3, 3, 9].Merge = true;

                                                outputSheet.Cells[3, 3].Style.Font.Color.SetColor(Color.Blue);
                                                outputSheet.Cells[3, 3].Style.Font.Bold = true;
                                            }
                                        }

                                        //
                                        for (int x1 = 0; x1 < Total_Column; x1++)
                                        {
                                            ai Column_Name = sql.Data.GetName(x1);

                                            HtmlTableCell TD_2 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                                            TR.Cells.Add(TD_2);
                                            TD_2.Controls.Add(html.Literal("<span class='B Red'>" + Column_Name.Replace("_", " ") + "</span>"));

                                            if (createBarcode == a.e)
                                            {
                                                if (checkedBarcode != "x")
                                                {
                                                    outputSheet.Cells[5, 1 + x1].Value = Column_Name;

                                                    outputSheet.Cells[5, 1 + x1].Style.Font.Color.SetColor(Color.Red);
                                                    outputSheet.Cells[5, 1 + x1].Style.Font.Bold = true;
                                                    outputSheet.Cells[5, 1 + x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                                }
                                            }
                                        }
                                    }

                                    //
                                    Order++;

                                    //
                                    HtmlTableRow TR_2 = htable.TR(Primary_Key);
                                    Report_htbl.Rows.Add(TR_2);

                                    if (((Order - 1) % 2) == 0)
                                    {
                                        TR_2.BgColor = "#C0C0C0";
                                    }

                                    for (int x1 = 0; x1 < Total_Column; x1++)
                                    {
                                        ai Column_Name = sql.Data.GetName(x1);
                                        ai Data_Content = sql.Data[Column_Name.aS()].aS();
                                        ai Data_Content_Raw = a.e;

                                        //
                                        HtmlTableCell TD_22 = htable.TD(a.e, "0", "0", "center", "middle", 0, 0, a.e, false, false, a.e);
                                        TR_2.Cells.Add(TD_22);
                                        TD_22.Controls.Add(html.Literal(Data_Content));

                                        //
                                        if (createBarcode == a.e)
                                        {
                                            if (checkedBarcode != "x")
                                            {
                                                outputSheet.Cells[Order + 5, 1 + x1].Value = Data_Content;
                                            }
                                        }
                                    }

                                    //
                                    if (createBarcode == a.e)
                                    {
                                        if (checkedBarcode == "x")
                                        {
                                            ai Loai_Tai_San = sql.Data["Loai_Tai_San"].aS();
                                            ai Tat_Ca = sql.Data["Tat_Ca"].aS();
                                            ai Hien_Co = sql.Data["Hien_Co"].aS();
                                            ai Khong_Co = sql.Data["Khong_Co"].aS();

                                            sheet.Cells[Order + 10, 2].Value = Loai_Tai_San;
                                            sheet.Cells[Order + 10, 5].Value = Tat_Ca;
                                            sheet.Cells[Order + 10, 6].Value = Hien_Co;
                                            sheet.Cells[Order + 10, 7].Value = Khong_Co;
                                        }
                                    }
                                    else
                                    {
                                        //Here
                                        ai ID = sql.Data["ID"].aS();
                                        ai Ma_Tai_San = sql.Data["Ma_Tai_San"].aS();
                                        ai Ten_Tai_San = sql.Data["Ten_Tai_San"].aS();
                                        ai MaSap = sql.Data["Ma_SAP"].aS();

                                        //
                                        if (((j1 + 1) % maxBarcodePerPicture) == 1)
                                        {
                                            Order = 1;

                                            graphic.Clear(Color.White);

                                            pictureFilePath = folder + "/" + ((j1 / maxBarcodePerPicture + 1)).aS() + ".bmp";
                                        }

                                        //
                                        using (Bitmap one = ams.CreatBarcode(ID))
                                        {
                                            int oneWidth = one.Width;
                                            int oneHeight = one.Height;

                                            int Barcode_And_Text_Height = oneHeight + 82;

                                            SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0, 0));

                                            //Text
                                            var Font_Family = new FontFamily("Times New Roman");
                                            var Font_12 = new Font(Font_Family, 12, FontStyle.Regular, GraphicsUnit.Pixel);

                                            StringFormat textFormat = new StringFormat();
                                            textFormat.LineAlignment = StringAlignment.Near;
                                            textFormat.Alignment = StringAlignment.Near;

                                            //
                                            int Title_Left = 0;
                                            int Title_Top = 0;

                                            if ((Order % 3) == 1)
                                            {
                                                Title_Left = 0;
                                                Title_Top = (Order - 1) / 3 * Barcode_And_Text_Height;
                                            }
                                            else
                                                if ((Order % 3) == 2)
                                            {
                                                Title_Left = outputPicture.Width / 3 * 1;
                                                Title_Top = (Order - 2) / 3 * Barcode_And_Text_Height;
                                            }
                                            else
                                                if ((Order % 3) == 0)
                                            {
                                                Title_Left = outputPicture.Width / 3 * 2;
                                                Title_Top = (Order - 3) / 3 * Barcode_And_Text_Height;
                                            }

                                            Title_Left = Title_Left + 10;

                                            //
                                            graphic.DrawString(ID + "-" + Ten_Tai_San, Font_12, brush, new Rectangle(Title_Left, Title_Top, one.Width, 15), textFormat);
                                            graphic.DrawString(Ma_Tai_San, Font_12, brush, new Rectangle(Title_Left, Title_Top + 15, one.Width, 15), textFormat);

                                            //
                                            int Barcode_IMG_Left = Title_Left - 8;
                                            int Barcode_IMG_Top = Title_Top + 30;

                                            graphic.DrawImage(one, Barcode_IMG_Left, Barcode_IMG_Top, one.Width, one.Height);
                                            graphic.DrawString(MaSap, Font_12, brush, new Rectangle(Title_Left, Barcode_IMG_Top + one.Height + 3, one.Width, 15), textFormat);
                                        }

                                        //
                                        if (((j1 + 1) % maxBarcodePerPicture) == 0)
                                        {
                                            graphic.Save();
                                            outputPicture.Save(pictureFilePath, ImageFormat.Bmp);

                                            ams.RotateImage(pictureFilePath, 90f);
                                            //allPicture.Add(pictureFilePath);
                                        }
                                    }

                                    //
                                    j1++;
                                }

                                //Neu chua save (chua den 30, thi cung save lai)
                                if (createBarcode != a.e)
                                {
                                    if ((j1 % maxBarcodePerPicture) != 0)
                                    {
                                        graphic.Save();
                                        outputPicture.Save(pictureFilePath, ImageFormat.Bmp);

                                        ams.RotateImage(pictureFilePath, 90f);
                                        //allPicture.Add(pictureFilePath);
                                    }
                                }
                            }
                        }

                        //
                        //if (createBarcode != a.e)
                        //    ams.CreatPdfFromMultiFile(allPicture, pdfFile, 100f);
                    }
                    catch (SqlException sqlEx)
                    {
                        lib.Log_Error(sqlEx);
                    }
                    finally
                    {
                        sql.Close();
                    }

                    //
                    if (createBarcode == a.e)
                    {
                        if (checkedBarcode != "x")
                        {
                            for (int x1 = 0; x1 < Total_Column; x1++)
                            {
                                outputSheet.Column(1 + x1).AutoFit();
                            }
                        }
                    }

                    //Save
                    if (createBarcode == a.e)
                    {
                        if (selectAll)
                        {
                            if (checkedBarcode != "x")
                            {
                                outputExcel.Save();
                            }
                            else
                            {
                                sheet.Cells[4, 7].Value = Cong_Ty_ddl.SelectedItem;
                                sheet.Cells[5, 7].Value = Phong_Ban_ddl.SelectedItem;
                                sheet.Cells[6, 7].Value = Loai_Thiet_Bi_ddl.SelectedItem;

                                excel.SaveAs(outputFile);
                            }

                            onPageLoad += " window.location.href = '" + a.url.DomainHttp + "/File/Download/" + folderName + "/" + outputFile.Name + "';";
                        }
                    }
                    else
                    {
                        ai shareFolder = @"\\10.15.40.86\Barcode\" + folderName + "\\";

                        //onPageLoad += " window.location.href = '" + a.url.DomainHttp + "/File/Download/" + folderName + "/" + pdfFile.Name + "';";
                        onPageLoad += " alert('Đã tạo file barcode thành công !');";
                    }

                    //Delete_Empty
                    afolder downloadFolder = Server.MapPath("~/File/Download/");
                    downloadFolder.Delete_Empty();
                }
            }
        }

        //
        Change_Page_1_lbl.Text = pagingHtml;
        Change_Page_2_lbl.Text = Change_Page_1_lbl.Text;
    }

    protected void Report_btn_Click(object sender, EventArgs e)
    {
        Submit_btn_Click("0");
    }
    protected void Download_Excel_All_btn_Click(object sender, ImageClickEventArgs e)
    {
        Submit_btn_Click("1");
    }
    protected void Submit_btn_Click(ai all)
    {
        atime Time_Start = Time_Start_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish = Time_Finish_tbx.Text.ai().aRemove("__/__/____").adate;

        if (!Time_Start.Valid)
        {
            Time_Start = "01/01/2009";
            Time_Start_tbx.Text = Time_Start.FormatedDate;
        }

        if (!Time_Finish.Valid)
        {
            Time_Finish = a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year;
            Time_Finish_tbx.Text = Time_Finish.FormatedDate;
        }

        //
        aurl url = new aurl();

        url.Remove("Onload");
        url.Remove("Page");
        url.Update("All", all);
        url.Remove("Barcode");

        url.Update("Cong_Ty_ID", Cong_Ty_ddl.SelectedValue);
        url.Update("Phong_Ban_ID", Phong_Ban_ddl.SelectedValue);
        url.Update("Loai_Thiet_Bi_ID", Loai_Thiet_Bi_ddl.SelectedValue);

        url.Update("CheckedBarcode", CheckedBarcode_ddl.SelectedValue);

        url.Update("ID", Filter_ID_List_tbx.Text.ai().UrlEncode);
        url.Update("Search", cbxFilterName.Checked.abool().ToBit);

        url.Update("Time_Start", Time_Start.FormatedDate.UrlEncode);
        url.Update("Time_Finish", Time_Finish.FormatedDate.UrlEncode);

        Response.Redirect(url);
    }

    protected void Print_Barcode_Page_btn_Click(object sender, EventArgs e)
    {
        Print_Barcode_btn_Click("0");
    }
    protected void Print_Barcode_All_btn_Click(object sender, EventArgs e)
    {
        Print_Barcode_btn_Click("1");
    }
    protected void Print_Barcode_btn_Click(ai all)
    {
        atime Time_Start = Time_Start_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish = Time_Finish_tbx.Text.ai().aRemove("__/__/____").adate;

        if (!Time_Start.Valid)
        {
            Time_Start = "01/01/2009";
            Time_Start_tbx.Text = Time_Start.FormatedDate;
        }

        if (!Time_Finish.Valid)
        {
            Time_Finish = a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year;
            Time_Finish_tbx.Text = Time_Finish.FormatedDate;
        }

        //
        aurl url = new aurl();

        url.Remove("Onload");
        //url.Remove("Page"); //Page giu nguyen theo hien tai
        url.Update("All", all);
        url.Update("Barcode", "1");

        url.Update("Cong_Ty_ID", Cong_Ty_ddl.SelectedValue);
        url.Update("Phong_Ban_ID", Phong_Ban_ddl.SelectedValue);
        url.Update("Loai_Thiet_Bi_ID", Loai_Thiet_Bi_ddl.SelectedValue);

        url.Update("CheckedBarcode", CheckedBarcode_ddl.SelectedValue);

        url.Update("ID", Filter_ID_List_tbx.Text.ai().UrlEncode);
        url.Update("Search", cbxFilterName.Checked.abool().ToBit);

        url.Update("Time_Start", Time_Start.FormatedDate.UrlEncode);
        url.Update("Time_Finish", Time_Finish.FormatedDate.UrlEncode);

        Response.Redirect(url);
    }
}