using _IQwinwin;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class CrmReport : Page
{
    ai onPageLoad = "Crm_Report_Onload();";

    ai Control_Call_Postback = a.e;
    ai Page_Query_String = a.e;
    ai Primary_Column = a.e;

    aray allPointColumn = new aray { "Point", "Total_Point", "Current_Point", "Point_In_Time", "Redeemed", "Mistake", "Other_Leaves", "All_Leaves", "So_Tien", "Truoc", "Sau" };

    protected void Page_Load(object sender, EventArgs e)
    {
        //UserName
        ai UserName = lib.Read_UserName();

        ai Ho_Va_Ten = a.e;
        ai Phong_Ban = a.e;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        crm.Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        if ((!Duoc_Xem_Bao_Cao) && (!Duoc_Tich_Diem))
        {
            Response.Redirect(a.url.DomainHttp);
        }
        else
        {
            Session["Total_Result_Per_Page_For_Report"] = 1000;

            //
            if (!IsPostBack)
            {
                lib.Set_Index_Host(Index_Host_hdf, null);
                PageMethods_Path_hdf.Value = "/Tool/Report.aspx";

                ViewState["ReportName"] = a.QueryText("R").aS();
                ai ReportName = a.aS(ViewState["ReportName"]);

                if (ReportName == "Add_point")
                {
                    Filter_RDOL_2.Visible = true;

                    if (Duoc_Xem_Bao_Cao)
                    {
                        Filter_RDOL_2.Items.Add(new ListItem("Tất cả tài khoản", "All"));
                        Filter_RDOL_2.Items.Add(new ListItem("Của chính mình", "1"));
                        Filter_RDOL_2.SelectedIndex = 0;
                    }
                    else
                    {
                        if (Duoc_Tich_Diem)
                        {
                            Filter_RDOL_2.Items.Add(new ListItem("Của chính mình", "1"));
                            Filter_RDOL_2.SelectedIndex = 0;
                        }
                    }
                }
                else
                if ((ReportName == "Add_point_by_member") || (ReportName == "Total_Add_point_by_member"))
                {
                    Filter_RDOL_2.Visible = true;

                    if (Duoc_Xem_Bao_Cao)
                    {
                        Filter_RDOL_2.Items.Add(new ListItem("Tất cả tài khoản", "All"));
                        Filter_RDOL_2.Items.Add(new ListItem("Của chính mình", "1"));
                        Filter_RDOL_2.SelectedIndex = 0;
                    }
                    else
                    {
                        if (Duoc_Tich_Diem)
                        {
                            Filter_RDOL_2.Items.Add(new ListItem("Của chính mình", "1"));
                            Filter_RDOL_2.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    Filter_RDOL_2.Visible = false;
                }

                if (Duoc_Doi_Diem)
                {
                    ReActive_Card_cbx.Visible = true;
                }

                //Date_Time_1
                atime Time_Start_1 = a.QueryText("Time_Start_1").adate;
                atime Time_Finish_1 = a.QueryText("Time_Finish_1").adate;

                if (Time_Start_1.Valid)
                    Time_Start_1_tbx.Text = Time_Start_1.FormatedDate;

                if (Time_Finish_1.Valid)
                    Time_Finish_1_tbx.Text = Time_Finish_1.FormatedDate;

                //Date_Time_2
                atime Time_Start_2 = a.QueryText("Time_Start_2").adate;
                atime Time_Finish_2 = a.QueryText("Time_Finish_2").adate;

                if (Time_Start_2.Valid)
                    Time_Start_2_tbx.Text = Time_Start_2.FormatedDate;

                if (Time_Finish_2.Valid)
                    Time_Finish_2_tbx.Text = Time_Finish_2.FormatedDate;

                //
                ai Shop = a.QueryText("Shop");
                ai Card = a.QueryText("Card");
                ai Name = a.QueryText("Name");
                ai Phone = a.QueryText("Phone");
                ai Email = a.QueryText("Email");
                ai Address = a.QueryText("Address");

                ai Add_By_User = a.QueryText("Add_By_User");

                Shop_tbx.Text = Shop;
                Card_tbx.Text = Card;
                Name_tbx.Text = Name;
                Phone_tbx.Text = Phone;
                Email_tbx.Text = Email;
                Address_tbx.Text = Address;

                Add_By_User_tbx.Text = Add_By_User;

                ai Filter_Time_3 = a.QueryText("Filter_Time_3");
                Filter_Time_3_tbx.Text = Filter_Time_3;

                //
                Read_Report();

                if ((ReportName == "FC_History") || (ReportName == "FC_ACC") || (ReportName == "FC_Block"))
                {
                    if (Card.Length == 11)
                    {
                        Read_FoodCourtCard_Info(Card);
                    }
                }
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
        lib.Add_All_JavaScript_AND_CSS_File_To_Header_Basic(true);
        lib.Add_All_JavaScript_AND_CSS_File_To_Header();
        lib.Add_JavaScript_File_To_Body(Page_Body, Index_Host_hdf.Value + "/index/Java_Script/Ajax/4u4m4e_Ajax.js");

        //
        if (
            (!Control_Call_Postback.Contains("Edit_"))
            && (!Control_Call_Postback.Contains("Expand_"))
            && (!Control_Call_Postback.Contains("Report_"))
            //&& (!Control_Call_Postback.Contains("Download_Excel_All_btn"))
            )
        {
            SelectReportContent();
        }

        //
        if (!IsPostBack)
            Page_Body.Attributes.Add("onload", onPageLoad);
        else
            lib.Run_JavaScript(onPageLoad);
    }

    protected void Read_Report()
    {
        ai UserName = lib.Read_UserName();

        if (UserName.ToLower() == "Pos2".ToLower())
        {
            Is_POS_hdf.Value = "1";
        }

        ai Ho_Va_Ten = a.e;
        ai Phong_Ban = a.e;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        crm.Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        ai ReportName = a.aS(ViewState["ReportName"]);

        ViewState["Filter_Time"] = "0";
        ViewState["Filter_Time_2"] = "0";
        ViewState["Filter_Name"] = "0";
        ViewState["Filter_RDOL"] = "0";
        ViewState["Filter_CBXL"] = "0";

        ViewState["Card_Actived_OR_Disabled"] = "0";

        ViewState["Card_FC"] = "0";
        ViewState["SAP"] = "0";

        //
        if (ReportName == "Add_point")
        {
            ViewState["Primary_Column"] = "ID";

            Report_lbl.Text = "Report: Add point !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_RDOL"] = "1";
            ViewState["Filter_Name"] = "1";

            Filter_RDOL_1.Items.Add(new ListItem("All point", "1"));
            Filter_RDOL_1.Items.Add(new ListItem("Redeemed", "2"));
            Filter_RDOL_1.Items.Add(new ListItem("Mistake", "3"));

            Filter_RDOL_1.SelectedIndex = 0;

            ViewState["Card_Actived_OR_Disabled"] = "1";
        }
        else
        if (ReportName == "Add_point_by_member")
        {
            ViewState["Primary_Column"] = "ID";

            Report_lbl.Text = "Report: Add point by Member !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_RDOL"] = "1";
            ViewState["Filter_Name"] = "1";

            Filter_RDOL_1.Items.Add(new ListItem("All point", "1"));
            Filter_RDOL_1.Items.Add(new ListItem("Redeemed", "2"));
            Filter_RDOL_1.Items.Add(new ListItem("Mistake", "3"));

            Filter_RDOL_1.SelectedIndex = 0;

            ViewState["Card_Actived_OR_Disabled"] = "1";
        }
        else
        if (ReportName == "Total_Add_point_by_member")
        {
            ViewState["Primary_Column"] = "Number";

            Report_lbl.Text = "Report: Add point by Member (Total) !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_RDOL"] = "1";
            ViewState["Filter_Name"] = "1";

            Filter_RDOL_1.Items.Add(new ListItem("All point", "1"));
            Filter_RDOL_1.Items.Add(new ListItem("Redeemed", "2"));
            Filter_RDOL_1.Items.Add(new ListItem("Mistake", "3"));

            Filter_RDOL_1.SelectedIndex = 0;

            ViewState["Card_Actived_OR_Disabled"] = "1";
        }
        else
        if (ReportName == "Statement")
        {
            Report_lbl.Text = "Report: Statement !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
        if (ReportName == "Current_point")
        {
            Report_lbl.Text = "Report: Current point !";

            //ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
        if (ReportName == "Compare")
        {
            Report_lbl.Text = "Report: Compare !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Time_2"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
        if (ReportName == "Max_buying_by_Shop")
        {
            Report_lbl.Text = "Report: Max buying by Shop !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
        if (ReportName == "Buying_list")
        {
            Report_lbl.Text = "Report: Buying list !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";

            ViewState["Card_Actived_OR_Disabled"] = "1";
        }
        else
        if (ReportName == "Inquiry_discount")
        {
            Report_lbl.Text = "Report: Inquiry discount !";

            ViewState["Filter_Time"] = "1";
        }
        else
        if (ReportName == "Analys_transaction")
        {
            Report_lbl.Text = "Report: Analys transaction !";

            ViewState["Filter_Time"] = "1";
        }
        else
        if (ReportName == "Member_list")
        {
            Report_lbl.Text = "Report: Member list !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_CBXL"] = "1";
            ViewState["Filter_Name"] = "1";

            Filter_CBXL_1.Items.Add(new ListItem("No Name", "1"));
            Filter_CBXL_1.Items.Add(new ListItem("Have Name", "11"));

            Filter_CBXL_1.Items.Add(new ListItem("No ID", "2"));
            Filter_CBXL_1.Items.Add(new ListItem("Have ID", "21"));

            Filter_CBXL_1.Items.Add(new ListItem("No Phone", "3"));
            Filter_CBXL_1.Items.Add(new ListItem("Have Phone", "31"));

            Filter_CBXL_1.Items.Add(new ListItem("No Email", "4"));
            Filter_CBXL_1.Items.Add(new ListItem("Have Email", "41"));

            Filter_CBXL_1.Items.Add(new ListItem("Duplicate Phone", "5"));
            Filter_CBXL_1.Items.Add(new ListItem("One Phone", "51"));

            Filter_CBXL_1.Items.Add(new ListItem("Duplicate Email", "6"));
            Filter_CBXL_1.Items.Add(new ListItem("One Email", "61"));

            Filter_CBXL_1.RepeatColumns = 3;
        }
        else
        if (ReportName == "FC_History")
        {
            Report_lbl.Text = "Report: Lịch sử Thẻ FC !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";

            ViewState["Card_FC"] = "1";

            Shop_tbx.Visible = false;
            Name_tbx.Visible = false;
            Phone_tbx.Visible = false;
            Email_tbx.Visible = false;
            Address_tbx.Visible = false;

            Shop_td.Visible = false;
            Name_td.Visible = false;
            Phone_td.Visible = false;
            Email_td.Visible = false;
            Address_td.Visible = false;

            ReActive_Card_cbx.Visible = false;
        }
        else
        if (ReportName == "FC_ACC")
        {
            Report_lbl.Text = "Report: Số dư Thẻ FC !";

            ViewState["Filter_Name"] = "1";

            ViewState["Card_FC"] = "1";

            Shop_tbx.Visible = false;
            Name_tbx.Visible = false;
            Phone_tbx.Visible = false;
            Email_tbx.Visible = false;
            Address_tbx.Visible = false;

            Shop_td.Visible = false;
            Name_td.Visible = false;
            Phone_td.Visible = false;
            Email_td.Visible = false;
            Address_td.Visible = false;

            ReActive_Card_cbx.Visible = false;
        }
        else
        if (ReportName == "FC_Block")
        {
            Report_lbl.Text = "Report: Khóa Thẻ FC !";

            ViewState["Filter_Name"] = "1";

            ViewState["Card_FC"] = "1";

            FC_Block_btn.Visible = true;
            //FC_UnBlock_btn.Visible = true;

            Submit_tr.Visible = false;

            Shop_tbx.Visible = false;
            Name_tbx.Visible = false;
            Phone_tbx.Visible = false;
            Email_tbx.Visible = false;
            Address_tbx.Visible = false;

            Shop_td.Visible = false;
            Name_td.Visible = false;
            Phone_td.Visible = false;
            Email_td.Visible = false;
            Address_td.Visible = false;

            ReActive_Card_cbx.Visible = false;
        }
        else
        if (ReportName == "SAP")
        {
            Report_lbl.Text = "Add mã SAP !";

            ViewState["SAP"] = "1";

            Submit_tr.Visible = false;

            Shop_tbx.Visible = false;
            Name_tbx.Visible = false;
            Phone_tbx.Visible = false;
            Email_tbx.Visible = false;
            Address_tbx.Visible = false;

            Shop_td.Visible = false;
            Name_td.Visible = false;
            Phone_td.Visible = false;
            Email_td.Visible = false;
            Address_td.Visible = false;

            ReActive_Card_cbx.Visible = false;
        }
        else
        if (ReportName == "Member_buy")
        {
            Report_lbl.Text = "Report: Thành viên Mua sắm !";

            ViewState["Filter_Time"] = "1";

            Filter_Time_3_tr.Visible = true;
            Space_Filter_Time_3_tr.Visible = true;
        }
        else
        if (ReportName == "Sale")
        {
            Report_lbl.Text = "Report: Sale !";
        }
        else
        if (ReportName == "Card_not_use")
        {
            Report_lbl.Text = "Report: Card not use !";
        }

        //TR
        Filter_Time_tr.Visible = Check_Object_Is_True(ViewState["Filter_Time"]);
        Filter_Time_2_tr.Visible = Check_Object_Is_True(ViewState["Filter_Time_2"]);
        Filter_Name_tr.Visible = Check_Object_Is_True(ViewState["Filter_Name"]);
        Filter_RDOL_1_tr.Visible = Check_Object_Is_True(ViewState["Filter_RDOL"]);
        Filter_CBXL_1_tr.Visible = Check_Object_Is_True(ViewState["Filter_CBXL"]);

        Card_Actived_OR_Disabled_tr.Visible = Check_Object_Is_True(ViewState["Card_Actived_OR_Disabled"]);

        Card_FC_Infor_tr.Visible = Check_Object_Is_True(ViewState["Card_FC"]);

        SAP_tr.Visible = Check_Object_Is_True(ViewState["SAP"]);

        //
        if (ReportName == "Add_point")
        {
            if (UserName.ToLower() == "Pos2".ToLower())
            {
                Filter_Name_tr.Visible = false;

                Filter_RDOL_1_tr.Visible = false;
                Filter_CBXL_1_tr.Visible = false;

                Download_Excel_All_btn.Visible = false;
            }
        }
        else
        if ((ReportName == "Add_point_by_member") || (ReportName == "Total_Add_point_by_member"))
        {
            if (UserName.ToLower() == "Pos2".ToLower())
            {
                Filter_Name_tr.Visible = false;

                Filter_RDOL_1_tr.Visible = false;
                Filter_CBXL_1_tr.Visible = false;

                Download_Excel_All_btn.Visible = false;
            }
        }
        else
        if (ReportName == "Member_list")
        {
            if (Duoc_Doi_Diem)
            {
                Switch_Point_td.Visible = true;
            }
        }

        //Space
        Space_Filter_Time_tr.Visible = Filter_Time_tr.Visible;
        Space_Filter_Time_2_tr.Visible = Filter_Time_2_tr.Visible;
        Space_Filter_Name_tr.Visible = Filter_Name_tr.Visible;
        Space_Filter_RDOL_1_tr.Visible = Filter_RDOL_1_tr.Visible;
        Space_Filter_CBXL_1_tr.Visible = Filter_CBXL_1_tr.Visible;

        Space_Card_FC_Infor_tr.Visible = Card_FC_Infor_tr.Visible;

        Space_SAP_tr.Visible = SAP_tr.Visible;

        ai Card_Actived_OR_Disabled_SelectedValue = a.QueryText("Card_Actived_OR_Disabled");
        ai Filter_RDOL_1_SelectedValue = a.QueryText("RDOL_1");
        ai Filter_RDOL_2_SelectedValue = a.QueryText("RDOL_2");

        ai Filter_CBXL_1_SelectedValue = a.QueryText("CBXL_1");

        Card_Actived_OR_Disabled_rdol.rechoice(Card_Actived_OR_Disabled_SelectedValue);
        Filter_RDOL_1.rechoice(Filter_RDOL_1_SelectedValue);
        Filter_RDOL_2.rechoice(Filter_RDOL_2_SelectedValue);

        Filter_CBXL_1.rechoice(Filter_CBXL_1_SelectedValue);

        ai ReActive_Card = a.QueryID("ReActive_Card");
        ReActive_Card_cbx.Checked = ReActive_Card;

        ////Creat Control: DLL, v.v.
    }
    protected void SelectReportContent()
    {
        ai ReportName = a.aS(ViewState["ReportName"]);
        ai pagingHtml = a.e;

        ai sqlQuery = a.e;
        ai sqlJoin = a.e;

        ai sqlWhere = a.e;
        ai sqlOrder = a.e;

        bool Is_one_Page = !a.QueryID("All").abool;

        //UserName
        ai IP = sys.IP;
        ai UserName = lib.Read_UserName();

        ai Ho_Va_Ten = a.e;
        ai Phong_Ban = a.e;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        crm.Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        //
        ai Computer = a.e;
        ai Domain = a.e;
        ai UserName_Services = a.e;

        ai QuayBan = a.e;
        ai CaBan = a.e;
        ai NVBan = a.e;

        crm.Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

        if (QuayBan == a.e)
        {
            if (Computer != a.e)
            {
                QuayBan = Computer;
            }
            else
            {
                QuayBan = IP;
            }
        }

        if ((UserName.ToLower() == "pos2") || (UserName_Services.ToLower() == "pos2"))
        {
            if (NVBan == a.e)
            {
                if (UserName_Services != a.e)
                {
                    NVBan = UserName_Services;
                }
                else
                {
                    NVBan = UserName;
                }
            }
        }
        else
        {
            NVBan = UserName;
        }

        //
        ai Add_By_User = Add_By_User_tbx.Text.ai().Get_ValidInput;
        ai Add_At = QuayBan;
        ai Add_By = a.e;
        ai Add_Time = a.Time;

        if (Add_By_User == a.e)
        {
            Add_By = NVBan;

            if ((UserName_Services.ToLower() == "pos") || (UserName_Services.ToLower() == "pos2"))
            {
                ai Cashier_Code = crm.Read_Cashier_Code(NVBan);

                if (Cashier_Code != a.e)
                {
                    Add_By = Cashier_Code;
                }
            }
        }
        else
        {
            Add_By = Add_By_User;
        }

        List<nv> allParam = new List<nv>();

        ai Sql_Where_Add_By = a.e;

        if (Add_By_User == a.e)
        {
            if (UserName.ToLower() == "Pos2".ToLower())
            {
                Sql_Where_Add_By = " AND ((Point_Alias.MOD_ID = @Add_By) OR (Point_Alias.Add_At = @Add_At))";
            }
            else
                if (Filter_RDOL_2.Visible)
            {
                if (Filter_RDOL_2.SelectedValue == "1")
                {
                    Sql_Where_Add_By = " AND (Point_Alias.MOD_ID = @Add_By)";
                }
            }
        }
        else
        {
            Sql_Where_Add_By = " AND (Point_Alias.MOD_ID = @Add_By)";
        }

        if (Phong_Ban.ToLower() == "Star Fitness".ToLower())
        {
            Sql_Where_Add_By += " AND (Point_Alias.Add_By_Group LIKE 'Star Fitness')";
        }

        allParam.Add(new nv("Add_At", Add_At));
        allParam.Add(new nv("Add_By", Add_By));

        //Date_Time_1
        atime Time_Start_1 = Time_Start_1_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish_1 = Time_Finish_1_tbx.Text.ai().aRemove("__/__/____").adate;

        if (!Time_Start_1.Valid)
        {
            Time_Start_1 = "01/" + a.Time.Month + "/" + a.Time.Year;//"01/01/2000";// 
            Time_Start_1_tbx.Text = Time_Start_1.FormatedDate;
        }

        allParam.Add(new nv("Time_Start_1_Temp", Time_Start_1));

        //
        if (!Time_Finish_1.Valid)
        {
            Time_Finish_1 = a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year;
            Time_Finish_1_tbx.Text = Time_Finish_1.FormatedDate;
        }

        //
        allParam.Add(new nv("Time_Finish_1_Temp", Time_Finish_1));

        //Date_Time_2
        atime Time_Start_2 = Time_Start_2_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish_2 = Time_Finish_2_tbx.Text.ai().aRemove("__/__/____").adate;

        if (!Time_Start_2.Valid)
        {
            Time_Start_2 = a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year;
            Time_Start_2_tbx.Text = Time_Start_2.FormatedDate;
        }

        allParam.Add(new nv("Time_Start_2_Temp", Time_Start_2));

        if (!Time_Finish_2.Valid)
        {
            Time_Finish_2 = a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year;
            Time_Finish_2_tbx.Text = Time_Finish_2.FormatedDate;
        }

        //
        allParam.Add(new nv("Time_Finish_2_Temp", Time_Finish_2));

        ai Sql_Join_HD = a.e;
        ai Sql_Join_CTHD = a.e;

        int Month_Start = Time_Start_1.Valid ? Time_Start_1.Month : 1;
        int Year_Start = Time_Start_1.Valid ? Time_Start_1.Year : 2009;

        int Month_Finish = Time_Finish_1.Valid ? Time_Finish_1.Month : a.Time.Month;
        int Year_Finish = Time_Finish_1.Valid ? Time_Finish_1.Year : a.Time.Year;

        //
        if (Year_Start == Year_Finish)
        {
            for (int i2 = Month_Start; i2 <= Month_Finish; i2++)
            {
                ai Sql_Join_HD_Temp = a.e;
                ai Sql_Join_CTHD_Temp = a.e;

                Creat_Sql_Join_Card_FC(i2, Year_Finish, out Sql_Join_HD_Temp, out Sql_Join_CTHD_Temp);

                Sql_Join_HD += Sql_Join_HD_Temp;
                Sql_Join_CTHD += Sql_Join_CTHD_Temp;
            }
        }
        else
        if (Year_Start < Year_Finish)
        {
            for (int i1 = Year_Start; i1 <= Year_Finish; i1++)
            {
                if (i1 == Year_Start)
                {
                    for (int i2 = Month_Start; i2 <= 12; i2++)
                    {
                        ai Sql_Join_HD_Temp = a.e;
                        ai Sql_Join_CTHD_Temp = a.e;

                        Creat_Sql_Join_Card_FC(i2, i1, out Sql_Join_HD_Temp, out Sql_Join_CTHD_Temp);

                        Sql_Join_HD += Sql_Join_HD_Temp;
                        Sql_Join_CTHD += Sql_Join_CTHD_Temp;
                    }
                }
                else
                if (i1 < Year_Finish)
                {
                    for (int i2 = 1; i2 <= 12; i2++)
                    {
                        ai Sql_Join_HD_Temp = a.e;
                        ai Sql_Join_CTHD_Temp = a.e;

                        Creat_Sql_Join_Card_FC(i2, i1, out Sql_Join_HD_Temp, out Sql_Join_CTHD_Temp);

                        Sql_Join_HD += Sql_Join_HD_Temp;
                        Sql_Join_CTHD += Sql_Join_CTHD_Temp;
                    }
                }
                else
                if (i1 == Year_Finish)
                {
                    for (int i2 = 1; i2 <= Month_Finish; i2++)
                    {
                        ai Sql_Join_HD_Temp = a.e;
                        ai Sql_Join_CTHD_Temp = a.e;

                        Creat_Sql_Join_Card_FC(i2, i1, out Sql_Join_HD_Temp, out Sql_Join_CTHD_Temp);

                        Sql_Join_HD += Sql_Join_HD_Temp;
                        Sql_Join_CTHD += Sql_Join_CTHD_Temp;
                    }
                }
            }
        }

        //
        Sql_Join_HD -= "UNION";
        Sql_Join_CTHD -= "UNION";

        //
        ai Shop = Shop_tbx.Text.ai().Get_ValidInput;
        ai Card = Card_tbx.Text.ai().Get_ValidInput;
        ai Name = Name_tbx.Text.ai().Get_ValidInput;
        ai Phone = Phone_tbx.Text.ai().Get_ValidInput.aRemove("-", ".");
        Phone_tbx.Text = Phone;

        ai Email = Email_tbx.Text.ai().Get_ValidInput;
        ai Address = Address_tbx.Text.ai().Get_ValidInput;

        ai Sql_Where_Shop = a.e;
        ai Sql_Where_Card = a.e;
        ai Sql_Where_Name = a.e;
        ai Sql_Where_Phone = a.e;
        ai Sql_Where_Email = a.e;
        ai Sql_Where_Address = a.e;

        ai Sql_Query_ReActive_Card = a.e;

        ai Sql_Where_Card_FC = a.e;

        if (Shop != a.e)
        {
            allParam.Add(new nv("Shop", Shop));

            if (ReportName == "Add_point")
            {
                Sql_Where_Shop = " AND ((Brand_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%') OR (Gian_Hang_Alias.TenGianHang COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%'))";
            }
            else
            if ((ReportName == "Add_point_by_member") || (ReportName == "Total_Add_point_by_member"))
            {
                Sql_Where_Shop = " AND ((Brand_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%') OR (Gian_Hang_Alias.TenGianHang COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%'))";
            }
            else
            {
                Sql_Where_Shop = " AND (Brand_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%')";
            }
        }

        if (Card != a.e)
        {
            allParam.Add(new nv("Card", Card));

            Sql_Where_Card = " AND (Member_Alias.Mem_Card COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Card) + '%')";

            if (Card_Actived_OR_Disabled_rdol.SelectedValue == "")
            {
                ReActive_Card_cbx.Checked = false;
            }
            else
            if (ReActive_Card_cbx.Checked)
            {
                Card.aRemove("-");

                if (Card.Length == 11)
                {
                    Sql_Query_ReActive_Card =
                        "USE GARDEN_CRM"

                        + " DECLARE @Add_At NVARCHAR(MAX)"
                        + " DECLARE @Add_By NVARCHAR(MAX)"
                        + " DECLARE @Add_Time NVARCHAR(MAX)"

                        + " DECLARE @Card NVARCHAR(MAX)"
                        + " DECLARE @Mem_SEQ_1 NVARCHAR(MAX)"
                        + " DECLARE @Mem_SEQ_2 NVARCHAR(MAX)"

                        + " SET @Add_At = N'" + Add_At + "'"
                        + " SET @Add_By = N'" + Add_By + "'"
                        + " SET @Add_Time = CONVERT(DATETIME, @Add_Time_Temp, 103)"

                        + " SET @Card = N'" + Card + "'"

                        + " DECLARE @Total_Point NVARCHAR(MAX)"

                        + " SET @Mem_SEQ_1 = (SELECT TOP 1 Mem_SEQ FROM T_MEM_MST WHERE (Mem_Card COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '-' + CONVERT(NVARCHAR(MAX), @Card)))"

                        + " IF (@Mem_SEQ_1 COLLATE SQL_Latin1_General_CP1_CI_AS IS NOT NULL)"
                        + " BEGIN"

                        + "     DECLARE MEM_SEQ_2_List CURSOR FOR"
                        + "     SELECT DISTINCT(MEM_SEQ) FROM T_PNT_HIS_INFO"
                        + "     WHERE (Mem_SEQ_Old = @Mem_SEQ_1)"

                        + "     UPDATE T_SALE_INFO"
                        + "     SET Mem_SEQ = Mem_SEQ_Old, Mem_SEQ_Old = NULL, Mem_SEQ_Edit_At = @Add_At, Mem_SEQ_Edit_By = @Add_By, Mem_SEQ_Edit_Time = @Add_Time"
                        + "     WHERE (Mem_SEQ_Old = @Mem_SEQ_1)"

                        + "     UPDATE T_PNT_HIS_INFO"
                        + "     SET Mem_SEQ = Mem_SEQ_Old, Mem_SEQ_Old = NULL, Mem_SEQ_Edit_At = @Add_At, Mem_SEQ_Edit_By = @Add_By, Mem_SEQ_Edit_Time = @Add_Time"
                        + "     WHERE (Mem_SEQ_Old = @Mem_SEQ_1)"

                        + "     UPDATE T_MEM_MST"
                        + "     SET Mem_Card = Mem_Card_Old, Mem_Card_Old = NULL, Mem_Card_Edit_At = @Add_At, Mem_Card_Edit_By = @Add_By, Mem_Card_Edit_Time = @Add_Time"
                        + "     WHERE (Mem_SEQ = @Mem_SEQ_1)"

                        + "     SET @Total_Point = (SELECT SUM(CHG_PNT) FROM T_PNT_HIS_INFO WHERE (MEM_SEQ = @Mem_SEQ_1))"

                        + "     IF EXISTS (SELECT * FROM T_MEM_NOW_PNT WHERE (MEM_SEQ = @Mem_SEQ_1))"
                        + "         UPDATE T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point WHERE (MEM_SEQ = @Mem_SEQ_1)"
                        + "     ELSE"
                        + "         INSERT INTO T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@Mem_SEQ_1, @Total_Point)"

                        + "     OPEN MEM_SEQ_2_List"
                        + "     FETCH NEXT FROM MEM_SEQ_2_List INTO @MEM_SEQ_2"

                        + "     WHILE @@FETCH_STATUS = 0"
                        + "     BEGIN"
                        + "         SET @Total_Point = (SELECT SUM(CHG_PNT) FROM T_PNT_HIS_INFO WHERE (MEM_SEQ = @Mem_SEQ_2))"

                        + "         IF EXISTS (SELECT * FROM T_MEM_NOW_PNT WHERE (MEM_SEQ = @Mem_SEQ_2))"
                        + "             UPDATE T_MEM_NOW_PNT SET NOW_TOT_PNT = @Total_Point WHERE (MEM_SEQ = @Mem_SEQ_2)"
                        + "         ELSE"
                        + "             INSERT INTO T_MEM_NOW_PNT (MEM_SEQ, NOW_TOT_PNT) VALUES (@Mem_SEQ_2, @Total_Point)"

                        + "         FETCH NEXT FROM MEM_SEQ_2_List INTO @MEM_SEQ_2"
                        + "     END"

                        + "     CLOSE MEM_SEQ_2_List"
                        + "     DEAllOCATE MEM_SEQ_2_List"

                        + " END"
                        ;

                    Sql_Query_ReActive_Card.asql(new nv("Add_Time_Temp", Add_Time)).NonQuery();
                    ReActive_Card_cbx.Checked = false;
                }
            }
        }

        //
        if (Name != a.e)
        {
            allParam.Add(new nv("Name", Name));
            Sql_Where_Name = " AND (Member_Alias.Mem_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Name) + '%')";
        }

        if (Phone != a.e)
        {
            allParam.Add(new nv("Phone", Phone));
            Sql_Where_Phone = " AND (REPLACE(Member_Alias.MOBILE_NO, '-', '') COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Phone) + '%')";
        }

        if (Address != a.e)
        {
            allParam.Add(new nv("Address", Address));
            Sql_Where_Address = " AND (Member_Alias.MEM_ADDR COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Address) + '%')";
        }

        if (Email != a.e)
        {
            allParam.Add(new nv("Email", Email));
            Sql_Where_Email = " AND (Member_Alias.E_Mail COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Email) + '%')";
        }

        //
        sqlOrder = "Mem_Nm, REG_DT DESC, MOD_DT DESC";

        //
        ai Sql_Where_No_Name = a.e;
        ai Sql_Where_No_ID = a.e;
        ai Sql_Where_Duplicate_Phone = a.e;
        ai Sql_Where_Duplicate_Email = a.e;

        //
        ai CBXL_Selected_Value = aselected.CBXL(Filter_CBXL_1, "value");

        //
        if (CBXL_Selected_Value.Contains("#11#"))
        {
            Sql_Where_No_Name = " AND ((Member_Alias.Mem_Nm NOT LIKE 'X') AND (Member_Alias.Mem_Nm NOT LIKE 'A') AND (Member_Alias.Mem_Nm IS NOT NULL) AND (Member_Alias.Mem_Nm NOT LIKE '') AND (LEN(Member_Alias.Mem_Nm) = DATALENGTH(Member_Alias.Mem_Nm)))";
        }
        else
        if (CBXL_Selected_Value.Contains("#1#"))
        {
            Sql_Where_No_Name = " AND ((Member_Alias.Mem_Nm LIKE 'X') OR (Member_Alias.Mem_Nm LIKE 'A') OR (Member_Alias.Mem_Nm IS NULL) OR (Member_Alias.Mem_Nm LIKE '') OR (LEN(Member_Alias.Mem_Nm) <> DATALENGTH(Member_Alias.Mem_Nm)))";
        }

        //
        if (CBXL_Selected_Value.Contains("#21#"))
        {
            Sql_Where_No_ID = " AND ((Member_Alias.CERTI_NO IS NOT NULL) AND (Member_Alias.CERTI_NO NOT LIKE '') AND (LEN(Member_Alias.CERTI_NO) = DATALENGTH(Member_Alias.CERTI_NO)))";
        }
        else
        if (CBXL_Selected_Value.Contains("#2#"))
        {
            Sql_Where_No_ID = " AND ((Member_Alias.CERTI_NO IS NULL) OR (Member_Alias.CERTI_NO LIKE '') OR (LEN(Member_Alias.CERTI_NO) <> DATALENGTH(Member_Alias.CERTI_NO)))";
        }

        //
        if (CBXL_Selected_Value.Contains("#31#"))
        {
            Sql_Where_No_ID = " AND ((Member_Alias.MOBILE_NO IS NOT NULL) AND (Member_Alias.MOBILE_NO NOT LIKE '') AND (LEN(Member_Alias.MOBILE_NO) = DATALENGTH(Member_Alias.MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10))";
        }
        else
        if (CBXL_Selected_Value.Contains("#3#"))
        {
            Sql_Where_No_ID = " AND ((Member_Alias.MOBILE_NO IS NULL) OR (Member_Alias.MOBILE_NO LIKE '') OR (LEN(Member_Alias.MOBILE_NO) <> DATALENGTH(Member_Alias.MOBILE_NO)) OR (LEN(Member_Alias.MOBILE_NO) < 10))";
        }

        //
        if (CBXL_Selected_Value.Contains("#41#"))
        {
            Sql_Where_No_ID = " AND ((Member_Alias.E_MAIL IS NOT NULL) AND (Member_Alias.E_MAIL NOT LIKE '') AND (LEN(Member_Alias.E_MAIL) = DATALENGTH(Member_Alias.E_MAIL)))";
        }
        else
        if (CBXL_Selected_Value.Contains("#4#"))
        {
            Sql_Where_No_ID = " AND ((Member_Alias.E_MAIL IS NULL) OR (Member_Alias.E_MAIL LIKE '') OR (LEN(Member_Alias.E_MAIL) <> DATALENGTH(Member_Alias.E_MAIL)))";
        }

        //
        if (CBXL_Selected_Value.Contains("#5#"))
        {
            sqlOrder = "MOBILE_NO, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Phone = " AND (MOBILE_NO IN (SELECT MOBILE_NO FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10)) GROUP BY MOBILE_NO HAVING (COUNT(MOBILE_NO) > 1)))";
        }
        else
        if (CBXL_Selected_Value.Contains("#51#"))
        {
            sqlOrder = "MOBILE_NO, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Phone = " AND (MOBILE_NO IN (SELECT MOBILE_NO FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10)) GROUP BY MOBILE_NO HAVING (COUNT(MOBILE_NO) = 1)))";
        }

        //
        if (CBXL_Selected_Value.Contains("#6#"))
        {
            sqlOrder = "E_MAIL, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Email = " AND (E_MAIL IN (SELECT E_MAIL FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (E_MAIL IS NOT NULL) AND (E_MAIL NOT LIKE '') AND (LEN(E_MAIL) = DATALENGTH(E_MAIL))) GROUP BY E_MAIL HAVING (COUNT(E_MAIL) > 1)))";
        }
        else
        if (CBXL_Selected_Value.Contains("#61#"))
        {
            sqlOrder = "E_MAIL, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Email = " AND (E_MAIL IN (SELECT E_MAIL FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (E_MAIL IS NOT NULL) AND (E_MAIL NOT LIKE '') AND (LEN(E_MAIL) = DATALENGTH(E_MAIL))) GROUP BY E_MAIL HAVING (COUNT(E_MAIL) = 1)))";
        }

        //
        if ((CBXL_Selected_Value.Contains("#5#")) && (CBXL_Selected_Value.Contains("#6#")))
        {
            sqlOrder = "MOBILE_NO, E_MAIL, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Phone =
                " AND ("
                + " (MOBILE_NO IN (SELECT MOBILE_NO FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10)) GROUP BY MOBILE_NO HAVING (COUNT(MOBILE_NO) > 1)))"
                + " OR (E_MAIL IN (SELECT E_MAIL FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (E_MAIL IS NOT NULL) AND (E_MAIL NOT LIKE '') AND (LEN(E_MAIL) = DATALENGTH(E_MAIL))) GROUP BY E_MAIL HAVING (COUNT(E_MAIL) > 1)))"
                + ")"
                ;

            Sql_Where_Duplicate_Email = a.e;
        }

        ai Sql_JOIN_Card_Actived_OR_Disabled = " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ";

        if (Card_Actived_OR_Disabled_rdol.SelectedValue == "0")
        {
            Sql_JOIN_Card_Actived_OR_Disabled = " ON Point_Alias.Mem_SEQ_Old = Member_Alias.Mem_SEQ";
        }

        //Count
        sqlQuery =
            "DECLARE @Time_Start_1 NVARCHAR(MAX)"
            + " SET @Time_Start_1 = CONVERT(DATETIME, @Time_Start_1_Temp, 103) + ' 00:00:00.000'"
            + " DECLARE @Time_Finish_1 NVARCHAR(MAX)"
            + " SET @Time_Finish_1 = CONVERT(DATETIME, @Time_Finish_1_Temp, 103) + ' 23:59:59.999'"

            + " DECLARE @Time_Start_2 NVARCHAR(MAX)"
            + " SET @Time_Start_2 = CONVERT(DATETIME, @Time_Start_2_Temp, 103) + ' 00:00:00.000'"
            + " DECLARE @Time_Finish_2 NVARCHAR(MAX)"
            + " SET @Time_Finish_2 = CONVERT(DATETIME, @Time_Finish_2_Temp, 103) + ' 23:59:59.999'"
            ;

        //
        ai Data_Table_Hoa_Don = a.e;

        int Month = Time_Start_1.Valid ? Time_Start_1.Month : a.Time.Month;
        int Year = Time_Start_1.Valid ? Time_Start_1.Year : a.Time.Year;

        //if (ReportName == "Analys_transaction")
        {
            if (Month <= 9)
            {
                Data_Table_Hoa_Don = "HoaDon0" + Month.aS() + Year.aS();
            }
            else
            {
                Data_Table_Hoa_Don = "HoaDon" + Month.aS() + Year.aS();
            }
        }

        //Sql_Query

        #region Add_point
        if (ReportName == "Add_point")
        {
            if (Filter_RDOL_1.SelectedValue == "2")
            {
                sqlWhere = " AND (CHG_PNT < 0) AND ((PNT_RSN LIKE '%redem%') OR (PNT_RSN LIKE '%redeem%'))";
            }
            else
            if (Filter_RDOL_1.SelectedValue == "3")
            {
                sqlWhere = " AND (CHG_PNT < 0) AND ((PNT_RSN IS NULL) OR ((PNT_RSN NOT LIKE '%redem%') AND (PNT_RSN NOT LIKE '%redeem%')))";
            }

            //
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO AS Sale_Alias"
                + " ON Point_Alias.Sale_SEQ = Sale_Alias.Sale_SEQ"

                + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"
                + " ON Sale_Alias.Brd_CD = Shop_Alias.Brd_CD"

                + " LEFT JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                + " ON Sale_Alias.MaThue LIKE Gian_Hang_Alias.MaThue"

                + " JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"
                + Sql_JOIN_Card_Actived_OR_Disabled

                + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_Old"
                + " ON Point_Alias.Mem_SEQ_Old = Member_Alias_Old.Mem_SEQ"

                + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_New"
                + " ON Point_Alias.Mem_SEQ = Member_Alias_New.Mem_SEQ"

                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                + " AND (Point_Alias.CHG_PNT IS NOT NULL)"

                + sqlWhere

                + Sql_Where_Shop
                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address

                + Sql_Where_Add_By
                ;
        }
        #endregion
        else
        #region Add_point_by_member
        if ((ReportName == "Add_point_by_member") || (ReportName == "Total_Add_point_by_member"))
        {
            if (Filter_RDOL_1.SelectedValue == "2")
            {
                sqlWhere = " AND (CHG_PNT < 0) AND ((PNT_RSN LIKE '%redem%') OR (PNT_RSN LIKE '%redeem%'))";
            }
            else
            if (Filter_RDOL_1.SelectedValue == "3")
            {
                sqlWhere = " AND (CHG_PNT < 0) AND ((PNT_RSN IS NULL) OR ((PNT_RSN NOT LIKE '%redem%') AND (PNT_RSN NOT LIKE '%redeem%')))";
            }

            //
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO AS Sale_Alias"
                + " ON Point_Alias.Sale_SEQ = Sale_Alias.Sale_SEQ"

                + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"
                + " ON Sale_Alias.Brd_CD = Shop_Alias.Brd_CD"

                + " LEFT JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                + " ON Sale_Alias.MaThue LIKE Gian_Hang_Alias.MaThue"

                + " JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"
                + Sql_JOIN_Card_Actived_OR_Disabled

                + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_Old"
                + " ON Point_Alias.Mem_SEQ_Old = Member_Alias_Old.Mem_SEQ"

                + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_New"
                + " ON Point_Alias.Mem_SEQ = Member_Alias_New.Mem_SEQ"

                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                + " AND (Point_Alias.CHG_PNT IS NOT NULL)"

                + sqlWhere

                + Sql_Where_Shop
                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address

                + Sql_Where_Add_By
                ;
        }
        #endregion
        else
        #region Statement
        if (ReportName == "Statement")
        {
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                + "     WHERE ("

                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address
                + "     )"
                ;
        }
        #endregion
        else
        #region Current_point
        if (ReportName == "Current_point")
        {
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                + "     WHERE ("

                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address
                + "     )"
                ;
        }
        #endregion
        else
        #region Compare
        if (ReportName == "Compare")
        {
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"

                + " JOIN "
                + " ("

                + " SELECT * FROM"
                + " ("
                + "     SELECT Member_Alias.Mem_Card, SUM(Sale_Alias.ADD_PNT) AS Point, MAX(Sale_Alias.Sale_DT) AS Recent"
                + "     FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                + "     JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                + "     ON Member_Alias.Mem_SEQ = Sale_Alias.Mem_SEQ"
                + "     WHERE ((Sale_Alias.Sale_DT >= @Time_Start_1) AND (Sale_Alias.Sale_DT <= @Time_Finish_1))"
                + "     GROUP BY Member_Alias.Mem_Card"
                + " ) AS Temp_Table_0"

                + " WHERE Mem_Card NOT IN ("
                + "         SELECT Member_Alias.Mem_Card"
                + "         FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                + "         JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                + "         ON Member_Alias.Mem_SEQ = Sale_Alias.Mem_SEQ"
                + "         WHERE ((Sale_Alias.Sale_DT >= @Time_Start_2) AND (Sale_Alias.Sale_DT <= @Time_Finish_2))"
                + "         GROUP BY Member_Alias.Mem_Card"
                + "         )"
                + " ) AS Temp_Table_1"

                + " ON Member_Alias.Mem_Card = Temp_Table_1.Mem_Card"

                + " WHERE ("
                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address
                + " )"
                ;
        }
        #endregion
        else
        #region Max_buying_by_Shop
        if (ReportName == "Max_buying_by_Shop")
        {
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"

                + " JOIN"
                + " ("
                + " 		SELECT Temp_Table_1.*,Temp_Table_2.Maxi FROM"
                + " 		(SELECT Brd_cd,Mem_SEQ,SUM(pay_amt) AS Money,COUNT(sale_dt) AS Transactions FROM GARDEN_CRM.DBO.T_SALE_INFO"
                + " 		WHERE ((SALE_DT >= @Time_Start_1) AND (Sale_Dt <= @Time_Finish_1) AND (Mem_SEQ IS NOT NULL) AND (Mem_SEQ <> 0))"
                + " 		GROUP BY Mem_SEQ,Brd_cd)AS Temp_Table_1"

                + " 		JOIN"
                + " 		("
                + " 		SELECT Brd_cd"
                + " 		,MAX(Money) Maxi FROM"
                + " 		("
                + " 		SELECT Brd_cd,Mem_SEQ,SUM(pay_amt) AS Money,COUNT(sale_dt) AS Transactions FROM GARDEN_CRM.DBO.T_SALE_INFO"
                + " 		WHERE ((SALE_DT >= @Time_Start_1) AND (Sale_Dt <= @Time_Finish_1) AND (Mem_SEQ IS NOT NULL) AND (Mem_SEQ <> 0))"
                + " 		GROUP BY Mem_SEQ,Brd_cd"
                + " 		) AS tbl"
                + " 		GROUP BY Brd_cd"
                + " 		)AS Temp_Table_2 ON Temp_Table_1.Brd_cd=Temp_Table_2.Brd_cd AND Temp_Table_1.Money = Temp_Table_2.maxi"

                + " ) AS Temp_Table_1"
                + " ON Shop_Alias.Brd_CD = Temp_Table_1.Brd_cd"

                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                + " ON Temp_Table_1.Mem_SEQ = Member_Alias.Mem_SEQ"

                + Sql_Where_Shop
                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address
                ;
        }
        #endregion
        else
        #region Buying_list
        if (ReportName == "Buying_list")
        {
            sqlQuery +=
                " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"

                + " JOIN GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"
                + Sql_JOIN_Card_Actived_OR_Disabled

                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"

                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address

                + " GROUP BY Member_Alias.Mem_Card, Member_Alias.Mem_Nm"
                ;
        }
        #endregion
        else
        #region Inquiry_discount
        if (ReportName == "Inquiry_discount")
        {
            if (!crm.Check_Exists_Table_POS("TOPOS_DB", Data_Table_Hoa_Don))
            {
                sqlQuery += " SELECT 0";
            }
            else
            {
                sqlQuery +=
                    " SELECT COUNT(*)"
                    + " FROM"
                    + "     (SELECT ID, MaTheKHTT, TenKhachHang, TenGianHang, SUM(TienCK) as Discount, SUM(Hoa_Don_Alias.TriGiaBan) as Amount,"
                    + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0011') AS Point,"
                    + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0012') AS VC_GARDEN,"
                    + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0023') AS VC_SHOP,"
                    + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0025') AS Ticket"

                    + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don + " AS Hoa_Don_Alias"

                    + "     JOIN TOPOS_DB.DBO.ThanhToan" + Data_Table_Hoa_Don + " AS Thanh_Toan_Alias"
                    + "     ON Hoa_Don_Alias.MaHD = Thanh_Toan_Alias.MaHD"

                    + "     JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                    + "     ON Hoa_Don_Alias.MaThue = Gian_Hang_Alias.MaThue"

                    + "     WHERE ((NgayBatDau >= @Time_Start_1) AND (NgayBatDau <= @Time_Finish_1))"
                    + "     GROUP BY ID, MaTheKHTT, TenKhachHang, MaHinhThuc, Gian_Hang_Alias.MaThue, Gian_Hang_Alias.TenGianHang"

                    + " ) AS Temp_Table_1"
                    + " GROUP BY ID, MaTheKHTT, TenGianHang, TenKhachHang, Amount, Discount"
                    ;
            }
        }
        #endregion
        else
        #region Analys_transaction
        if (ReportName == "Analys_transaction")
        {
            if (!crm.Check_Exists_Table_POS("TOPOS_DB", Data_Table_Hoa_Don))
            {
                sqlQuery += " SELECT 0";
            }
            else
            {
                sqlQuery +=
                    " SELECT COUNT(*)"
                    + " FROM"
                    + " ("
                    + "     SELECT NgayBatDau, SUM(TriGiaBan) AS AM, COUNT(MaHD) as Trans"
                    + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                    + "     GROUP BY NgayBatDau"
                    + " ) AS Temp_Table_1"

                    + " LEFT JOIN"
                    + " ("
                    + "     SELECT NgayBatDau, SubString(MaTheKHTT, 1, 2) AS CardNo, SUM(TriGiaBan) AS AM, COUNT(MaHD) AS TranBozon"
                    + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                    + "     WHERE SubString(MaTheKHTT, 1, 4) = '0101'"
                    + "     GROUP BY NgayBatDau, SubString(MaTheKHTT, 1, 2)"
                    + " ) AS The_Xanh_tbl"
                    + " ON Temp_Table_1.NgayBatDau = The_Xanh_tbl.NgayBatDau"

                    + " LEFT JOIN"
                    + " ("
                    + "     SELECT NgayBatDau, SubString(MaTheKHTT, 1, 2) AS CardNo, SUM(TriGiaBan) AS AM, COUNT(MaHD) AS TranBozon"
                    + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                    + "     WHERE SubString(MaTheKHTT, 1, 4) = '0102'"
                    + "     GROUP BY NgayBatDau, SubString(MaTheKHTT, 1, 2)"
                    + " ) AS The_Vang_tbl"
                    + " ON Temp_Table_1.NgayBatDau = The_Vang_tbl.NgayBatDau"

                    + " LEFT JOIN"
                    + " ("
                    + "     SELECT NgayBatDau, SubString(MaTheKHTT, 1, 2) AS CardNo, SUM(TriGiaBan) AS AM, COUNT(MaHD) AS TranBozon"
                    + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                    + "     WHERE SubString(MaTheKHTT, 1, 4) = '0103'"
                    + "     GROUP BY NgayBatDau, SubString(MaTheKHTT, 1, 2)"
                    + " ) AS The_Kim_Cuong_tbl"
                    + " ON Temp_Table_1.NgayBatDau = The_Kim_Cuong_tbl.NgayBatDau"
                    ;
            }
        }
        #endregion
        else
        #region Member_list
        if (ReportName == "Member_list")
        {
            sqlQuery +=

                " UPDATE GARDEN_CRM.DBO.T_MEM_MST"
                + " SET Mem_Nm = SUBSTRING(Mem_Nm, 1, LEN(Mem_Nm) - 1)"
                + " WHERE (LEN(Mem_Nm) <> DATALENGTH(Mem_Nm))"

                + " UPDATE GARDEN_CRM.DBO.T_MEM_MST"
                + " SET MOBILE_NO = SUBSTRING(MOBILE_NO, 1, LEN(MOBILE_NO) - 1)"
                + " WHERE (LEN(MOBILE_NO) <> DATALENGTH(MOBILE_NO))"

                + " SELECT COUNT(*)"
                + " FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                + " WHERE (MOD_DT >= @Time_Start_1) AND (MOD_DT <= @Time_Finish_1)"

                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address

                + Sql_Where_No_Name
                + Sql_Where_No_ID
                + Sql_Where_Duplicate_Phone
                + Sql_Where_Duplicate_Email
                ;
        }
        #endregion
        else
        #region FC_History
        if (ReportName == "FC_History")
        {
            if (Card != a.e)
            {
                int Year_int = a.Time.Year;
                int Month_int = a.Time.Month;

                ai Thang_Nam = a.e;

                //
                if (Month_int < 10)
                {
                    Thang_Nam += "0" + Month_int.aS();
                }
                else
                {
                    Thang_Nam += Month_int.aS();
                }

                Thang_Nam += a.Time.Year;

                sqlQuery +=

                    " USE TOPOS_DB"

                    + " DECLARE @CTKichHoatID NVARCHAR(MAX) SET @CTKichHoatID = (SELECT TOP 1 CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card) ORDER BY NgayKichHoat DESC)"

                    + " SELECT COUNT(*)"

                    + " FROM TTT_CTNapTien_HinhThucThanhToan Nap_Rut_Alias"

                    + " LEFT JOIN CTHoaDon" + Thang_Nam + " Hoa_Don_Alias"
                    + " ON Nap_Rut_Alias.HoaDonID = Hoa_Don_Alias.MaHD"

                    + " LEFT JOIN HinhThucThanhToan Hinh_Thuc_Thanh_Toan_Alias"
                    + " ON Nap_Rut_Alias.MaHinhThuc = Hinh_Thuc_Thanh_Toan_Alias.MaHinhThuc"

                    + " WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card)))"
                    ;
            }
        }
        #endregion

        bool run = true;

        if ((ReportName == "FC_ACC") || (ReportName == "FC_Block") || (ReportName == "SAP"))
        {
            run = false;
        }

        if (run && (sqlQuery != a.e))
        {
            if (ReportName == "FC_History")
            {
                Open_Sql_Connection_FC();
            }

            //lib.Add_SQL_Parameters(ref Sql_Command, SQL_Parameters_Array, SQL_Value_Array);
            asql sql = sqlQuery.asql();
            ai Total_Result = sql.Scalar;

            ViewState["Total_Result"] = lib.Convert_String_To_Int(Total_Result, 0);

            #region Pageing
            if ((int)ViewState["Total_Result"] == 0)
            {
                Change_Page_1_div.Visible = true;
                Change_Page_2_div.Visible = false;

                Page_1_lbl.Visible = false;

                Total_Result_1_lbl.Text = "Not found any result with your choice...";
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
                    //
                    Page_Query_String = a.QueryID("Page");
                    Page = lib.Convert_String_To_Int(Page_Query_String, 1);
                }

                ViewState["Page"] = Page;
                ViewState["Page_Group"] = 1;

                if ((int)(ViewState["Page"]) % 10 > 0)
                {
                    ViewState["Page_Group"] = ((int)(ViewState["Page"]) / 10) + 1;
                }
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

                    aurl url = new aurl();

                    url.Update("Time_Start_1", Time_Start_1.FormatedDate);
                    url.Update("Time_Finish_1", Time_Finish_1.FormatedDate);

                    url.Update("Time_Start_2", Time_Start_2.FormatedDate);
                    url.Update("Time_Finish_2", Time_Finish_2.FormatedDate);

                    url.Update("Shop", Shop_tbx.Text);
                    url.Update("Card", Card_tbx.Text);
                    url.Update("Name", Name_tbx.Text);
                    url.Update("Phone", Phone_tbx.Text);
                    url.Update("Email", Email_tbx.Text);
                    url.Update("Address", Address_tbx.Text);

                    url.Update("Card_Actived_OR_Disabled", Card_Actived_OR_Disabled_rdol.SelectedValue);
                    url.Update("RDOL_1", Filter_RDOL_1.SelectedValue);
                    url.Update("RDOL_2", Filter_RDOL_2.SelectedValue);

                    url.Update("CBXL_1", aselected.CBXL(Filter_CBXL_1, "value"));

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
                                pagingHtml += "<a href='" + url + "'><span class='B Red' style='font-size: 12pt;'>" + Convert.ToString(Page_Number) + "</span></a>";
                            }
                            else
                            {
                                pagingHtml += "<a href='" + url + "'>" + Convert.ToString(Page_Number) + "</a>";
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
                #endregion

                //Read data
                sqlQuery =
                    "DECLARE @Time_Start_1 NVARCHAR(MAX)"
                    + " SET @Time_Start_1 = CONVERT(DATETIME, @Time_Start_1_Temp, 103) + ' 00:00:00.000'"
                    + " DECLARE @Time_Finish_1 NVARCHAR(MAX)"
                    + " SET @Time_Finish_1 = CONVERT(DATETIME, @Time_Finish_1_Temp, 103) + ' 23:59:59.999'"

                    + " DECLARE @Time_Start_2 NVARCHAR(MAX)"
                    + " SET @Time_Start_2 = CONVERT(DATETIME, @Time_Start_2_Temp, 103) + ' 00:00:00.000'"
                    + " DECLARE @Time_Finish_2 NVARCHAR(MAX)"
                    + " SET @Time_Finish_2 = CONVERT(DATETIME, @Time_Finish_2_Temp, 103) + ' 23:59:59.999'"
                    ;

                //Sql_Query
                #region Add_point
                if (ReportName == "Add_point")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT PNT_HIS_SEQ AS ID, ROW_NUMBER() OVER(ORDER BY Point_Alias.MOD_DT DESC) AS Number,"
                        ;

                    if (Card_Actived_OR_Disabled_rdol.SelectedValue == "1")
                    {
                        sqlQuery +=
                            " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) + ' ' + CONVERT(NVARCHAR(MAX), ISNULL(Member_Alias_Old.Mem_Card, '')) AS Card,"
                            ;
                    }
                    else
                    {
                        sqlQuery +=
                            " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) + ' ' + CONVERT(NVARCHAR(MAX), ISNULL(Member_Alias_New.Mem_Card, '')) AS Card,"
                            ;

                        //Sql_Query +=
                        //    " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) AS Card,"
                        //    ;
                    }

                    sqlQuery +=
                        " Member_Alias.Mem_Nm AS Name,"
                        + " Point_Alias.PNT_RSN AS Add_Point_Manual_String, Point_Alias.PNT_RSN_Backup AS Add_Point_Manual_String_Backup,"

                        + " Gian_Hang_Alias.SoHopDong, Gian_Hang_Alias.TenGianHang, Sale_Alias.MaThue,"
                        + " Shop_Alias.SoHopDong AS SoHopDong_2, Shop_Alias.TenGianHang AS TenGianHang_2, Shop_Alias.Brand_Nm,"

                        + " Sale_Alias.PAY_AMT AS Money, Point_Alias.CHG_PNT AS Point, Sale_Alias.Recp_No AS Receipt,"
                        + " Point_Alias.Add_At, Point_Alias.Add_By_Group, Point_Alias.MOD_ID AS Add_By, Point_Alias.MOD_DT AS Add_Time, Sale_Alias.POS_No, Sale_Alias.BIZ_Day AS Buy_Time,"
                        + " Point_Alias.Is_Manual_Add_Point"

                        + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO AS Sale_Alias"
                        + " ON Point_Alias.Sale_SEQ = Sale_Alias.Sale_SEQ"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"
                        + " ON Sale_Alias.Brd_CD = Shop_Alias.Brd_CD"

                        + " LEFT JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                        + " ON Sale_Alias.MaThue LIKE Gian_Hang_Alias.MaThue"

                        + " JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"
                        + Sql_JOIN_Card_Actived_OR_Disabled

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_Old"
                        + " ON Point_Alias.Mem_SEQ_Old = Member_Alias_Old.Mem_SEQ"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_New"
                        + " ON Point_Alias.Mem_SEQ = Member_Alias_New.Mem_SEQ"

                        + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                        + " AND (Point_Alias.CHG_PNT IS NOT NULL)"

                        + sqlWhere

                        + Sql_Where_Shop
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address

                        + Sql_Where_Add_By

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Add_point_by_member
                if (ReportName == "Add_point_by_member")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT PNT_HIS_SEQ AS ID, ROW_NUMBER() OVER(ORDER BY Member_Alias.Mem_Seq, Sale_Alias.MOD_DT DESC) AS Number,"
                        ;

                    if (Card_Actived_OR_Disabled_rdol.SelectedValue == "1")
                    {
                        sqlQuery +=
                            " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) + ' ' + CONVERT(NVARCHAR(MAX), ISNULL(Member_Alias_Old.Mem_Card, '')) AS Card,"
                            ;
                    }
                    else
                    {
                        sqlQuery +=
                            " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) + ' ' + CONVERT(NVARCHAR(MAX), ISNULL(Member_Alias_New.Mem_Card, '')) AS Card,"
                            ;

                        //Sql_Query +=
                        //    " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) AS Card,"
                        //    ;
                    }

                    sqlQuery +=
                        " Member_Alias.Mem_Nm AS Name, Member_Alias.MOBILE_NO AS Phone,"
                        + " Point_Alias.PNT_RSN AS Add_Point_Manual_String, Point_Alias.PNT_RSN_Backup AS Add_Point_Manual_String_Backup,"

                        + " Gian_Hang_Alias.SoHopDong, Gian_Hang_Alias.TenGianHang, Sale_Alias.MaThue,"
                        + " Shop_Alias.SoHopDong AS SoHopDong_2, Shop_Alias.TenGianHang AS TenGianHang_2, Shop_Alias.Brand_Nm,"

                        + " Sale_Alias.PAY_AMT AS Money, Point_Alias.CHG_PNT AS Point, Sale_Alias.Recp_No AS Receipt,"
                        + " Point_Alias.Add_At, Point_Alias.Add_By_Group, Point_Alias.MOD_ID AS Add_By, Point_Alias.MOD_DT AS Add_Time, Sale_Alias.POS_No, Sale_Alias.BIZ_Day AS Buy_Time,"
                        + " Point_Alias.Is_Manual_Add_Point"

                        + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO AS Sale_Alias"
                        + " ON Point_Alias.Sale_SEQ = Sale_Alias.Sale_SEQ"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"
                        + " ON Sale_Alias.Brd_CD = Shop_Alias.Brd_CD"

                        + " LEFT JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                        + " ON Sale_Alias.MaThue LIKE Gian_Hang_Alias.MaThue"

                        + " JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"
                        + Sql_JOIN_Card_Actived_OR_Disabled

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_Old"
                        + " ON Point_Alias.Mem_SEQ_Old = Member_Alias_Old.Mem_SEQ"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_New"
                        + " ON Point_Alias.Mem_SEQ = Member_Alias_New.Mem_SEQ"

                        + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                        + " AND (Point_Alias.CHG_PNT IS NOT NULL)"

                        + sqlWhere

                        + Sql_Where_Shop
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address

                        + Sql_Where_Add_By

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Total_Add_point_by_member
                if (ReportName == "Total_Add_point_by_member")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("
                        + " SELECT ROW_NUMBER() OVER(ORDER BY SUM(Sale_Alias.PAY_AMT) DESC) AS Number,"

                        + " Member_Alias.Mem_Card AS Card, Member_Alias.Mem_Nm AS Name, Member_Alias.MOBILE_NO AS Phone,"
                        + " SUM(Sale_Alias.PAY_AMT) AS Money, SUM(Point_Alias.CHG_PNT) AS Point"

                        + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO AS Sale_Alias"
                        + " ON Point_Alias.Sale_SEQ = Sale_Alias.Sale_SEQ"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"
                        + " ON Sale_Alias.Brd_CD = Shop_Alias.Brd_CD"

                        + " LEFT JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                        + " ON Sale_Alias.MaThue LIKE Gian_Hang_Alias.MaThue"

                        + " JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"
                        + Sql_JOIN_Card_Actived_OR_Disabled

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_Old"
                        + " ON Point_Alias.Mem_SEQ_Old = Member_Alias_Old.Mem_SEQ"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias_New"
                        + " ON Point_Alias.Mem_SEQ = Member_Alias_New.Mem_SEQ"

                        + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                        + " AND (Point_Alias.CHG_PNT IS NOT NULL)"

                        + sqlWhere

                        + Sql_Where_Shop
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address

                        + Sql_Where_Add_By

                        + " GROUP BY Member_Alias.Mem_Seq, Member_Alias.Mem_Card, Member_Alias.Mem_Nm, Member_Alias.MOBILE_NO"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Statement
                if (ReportName == "Statement")
                {
                    sqlQuery +=

                        " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY Total_Point DESC) AS Number,"
                        + " Member_Alias.Mem_Card AS Card, Member_Alias.Mem_Nm AS Name,"
                        + " MEM_BIRTHDAY AS Birthday, YEAR(GETDATE()) - YEAR(Mem_birthday) AS Age, Sex_CD AS Sex, MOBILE_NO AS Phone, E_MAIL AS Email, CERTI_NO as ID, MEM_ADDR AS Address,"
                        + " Total_Point, Redeemed, Mistake, Other_Leaves, Redeemed + Mistake + Other_Leaves AS All_Leaves, Point_In_Time, Current_Point"

                        + " FROM ("

                        + "     SELECT Member_Alias.Mem_SEQ, Member_Alias.Mem_Card, Member_Alias.Mem_Nm, Member_Alias.MEM_BIRTHDAY, Member_Alias.Sex_CD, Member_Alias.MOBILE_NO, Member_Alias.E_MAIL, Member_Alias.CERTI_NO, Member_Alias.MEM_ADDR"
                        + "     FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                        + "     WHERE ("
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address
                        + "     )"

                        + "     GROUP BY Member_Alias.Mem_SEQ, Member_Alias.Mem_Card, Member_Alias.Mem_Nm, Member_Alias.MEM_BIRTHDAY, Member_Alias.Sex_CD, Member_Alias.MOBILE_NO, Member_Alias.E_MAIL, Member_Alias.CERTI_NO, Member_Alias.MEM_ADDR"
                        + " ) AS Member_Alias"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Total_Point"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     WHERE (Mod_DT >= @Time_Start_1) AND (Mod_DT <= @Time_Finish_1) AND (CHG_PNT > 0)"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Total_Point_Temp"
                        + " ON Member_Alias.Mem_SEQ = Total_Point_Temp.Mem_SEQ"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Redeemed"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     WHERE (Mod_DT >= @Time_Start_1) AND (Mod_DT <= @Time_Finish_1) AND (CHG_PNT < 0) AND ((PNT_RSN LIKE '%redem%') OR (PNT_RSN LIKE '%redeem%'))"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Redeemed_Temp"
                        + " ON Member_Alias.Mem_SEQ = Redeemed_Temp.Mem_SEQ"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Mistake"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     WHERE (Mod_DT >= @Time_Start_1) AND (Mod_DT <= @Time_Finish_1) AND (CHG_PNT < 0) AND (PNT_RSN NOT LIKE '%redem%') AND (PNT_RSN NOT LIKE '%redeem%') AND (PNT_RSN LIKE '%Mistake%')"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Mistake_Temp"
                        + " ON Member_Alias.Mem_SEQ = Mistake_Temp.Mem_SEQ"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Other_Leaves"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     WHERE (Mod_DT >= @Time_Start_1) AND (Mod_DT <= @Time_Finish_1) AND (CHG_PNT < 0) AND ((PNT_RSN IS NULL) OR ((PNT_RSN NOT LIKE '%redem%') AND (PNT_RSN NOT LIKE '%redeem%')) AND (PNT_RSN NOT LIKE '%Mistake%'))"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Other_Leaves_Temp"
                        + " ON Member_Alias.Mem_SEQ = Other_Leaves_Temp.Mem_SEQ"

                        //+ " LEFT JOIN"
                        //+ " ("
                        //+ "     SELECT Mem_SEQ, SUM(CHG_PNT) AS All_Leaves"
                        //+ "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        //+ "     WHERE (CHG_PNT < 0)"
                        //+ "     GROUP BY Mem_SEQ"
                        //+ " ) AS All_Leaves_Temp"
                        //+ " ON Member_Alias.Mem_SEQ = All_Leaves_Temp.Mem_SEQ"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Point_In_Time"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     WHERE (Mod_DT >= @Time_Start_1) AND (Mod_DT <= @Time_Finish_1)"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Point_In_Time_Temp"
                        + " ON Member_Alias.Mem_SEQ = Point_In_Time_Temp.Mem_SEQ"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Current_Point"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Current_Point_Temp"
                        + " ON Member_Alias.Mem_SEQ = Current_Point_Temp.Mem_SEQ"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Current_point
                if (ReportName == "Current_point")
                {
                    sqlQuery +=

                        " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY Current_Point DESC) AS Number,"
                        + " Member_Alias.Mem_Card AS Card, Member_Alias.Mem_Nm AS Name,"
                        + " Current_Point"

                        + " FROM ("

                        + "     SELECT Member_Alias.Mem_SEQ, Member_Alias.Mem_Card, Member_Alias.Mem_Nm"
                        + "     FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                        + "     WHERE ("
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address
                        + "     )"

                        + "     GROUP BY Member_Alias.Mem_SEQ, Member_Alias.Mem_Card, Member_Alias.Mem_Nm"
                        + " ) AS Member_Alias"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Current_Point"
                        + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                        + "     GROUP BY Mem_SEQ"
                        + " ) AS Current_Point_Temp"
                        + " ON Member_Alias.Mem_SEQ = Current_Point_Temp.Mem_SEQ"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Compare
                if (ReportName == "Compare")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY Point DESC) AS Number,"
                        + " Member_Alias.Mem_Card AS Card, Member_Alias.Mem_Nm AS Name,"
                        + " Temp_Table_1.Point, Temp_Table_1.Recent"

                        + " FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"

                        + " JOIN"
                        + " ("

                        + " SELECT * FROM"
                        + " ("
                        + "     SELECT Member_Alias.Mem_Card, SUM(Sale_Alias.ADD_PNT) AS Point, MAX(Sale_Alias.Sale_DT) AS Recent"
                        + "     FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                        + "     JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                        + "     ON Member_Alias.Mem_SEQ = Sale_Alias.Mem_SEQ"
                        + "     WHERE ((Sale_Alias.Sale_DT >= @Time_Start_1) AND (Sale_Alias.Sale_DT <= @Time_Finish_1))"
                        + "     GROUP BY Member_Alias.Mem_Card"
                        + " ) AS Temp_Table_0"

                        + " WHERE Mem_Card NOT IN ("
                        + "         SELECT Member_Alias.Mem_Card"
                        + "         FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                        + "         JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                        + "         ON Member_Alias.Mem_SEQ = Sale_Alias.Mem_SEQ"
                        + "         WHERE ((Sale_Alias.Sale_DT >= @Time_Start_2) AND (Sale_Alias.Sale_DT <= @Time_Finish_2))"
                        + "         GROUP BY Member_Alias.Mem_Card"
                        + "         )"
                        + " ) AS Temp_Table_1"

                        + " ON Member_Alias.Mem_Card = Temp_Table_1.Mem_Card"

                        + " WHERE ("
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address
                        + " )"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Max_buying_by_Shop
                if (ReportName == "Max_buying_by_Shop")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY Money DESC) AS Number,"
                        + " Mem_Card AS Card, Mem_Nm AS Name, Shop_Alias.Brand_Nm AS Shop, Money, Transactions"

                        + " FROM GARDEN_CRM.DBO.T_BRAND_MST AS Shop_Alias"

                        + " JOIN"
                        + " ("
                        + " 		SELECT Temp_Table_1.*,Temp_Table_2.Maxi FROM"
                        + " 		(SELECT Brd_cd,Mem_SEQ,SUM(pay_amt) AS Money,COUNT(sale_dt) AS Transactions FROM GARDEN_CRM.DBO.T_SALE_INFO"
                        + " 		WHERE ((SALE_DT >= @Time_Start_1) AND (Sale_Dt <= @Time_Finish_1) AND (Mem_SEQ IS NOT NULL) AND (Mem_SEQ <> 0))"
                        + " 		GROUP BY Mem_SEQ,Brd_cd)AS Temp_Table_1"

                        + " 		JOIN"
                        + " 		("
                        + " 		SELECT Brd_cd"
                        + " 		,MAX(Money) Maxi FROM"
                        + " 		("
                        + " 		SELECT Brd_cd,Mem_SEQ,SUM(pay_amt) AS Money,COUNT(sale_dt) AS Transactions FROM GARDEN_CRM.DBO.T_SALE_INFO"
                        + " 		WHERE ((SALE_DT >= @Time_Start_1) AND (Sale_Dt <= @Time_Finish_1) AND (Mem_SEQ IS NOT NULL) AND (Mem_SEQ <> 0))"
                        + " 		GROUP BY Mem_SEQ,Brd_cd"
                        + " 		) AS tbl"
                        + " 		GROUP BY Brd_cd"
                        + " 		)AS Temp_Table_2 ON Temp_Table_1.Brd_cd=Temp_Table_2.Brd_cd AND Temp_Table_1.Money = Temp_Table_2.maxi"

                        + " ) AS Temp_Table_1"
                        + " ON Shop_Alias.Brd_CD = Temp_Table_1.Brd_cd"

                        + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                        + " ON Temp_Table_1.Mem_SEQ = Member_Alias.Mem_SEQ"

                        + Sql_Where_Shop
                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Buying_list
                if (ReportName == "Buying_list")
                {
                    sqlQuery +=
                       " SELECT * FROM"
                       + " ("

                       + " SELECT ROW_NUMBER() OVER(ORDER BY SUM(Point_Alias.CHG_PNT) DESC) AS Number,"
                       + " Member_Alias.Mem_Card AS Card, Member_Alias.Mem_Nm AS Name, MEM_BIRTHDAY AS Birthday, YEAR(GETDATE()) - YEAR(Mem_birthday) AS Age, Sex_CD AS Sex, MOBILE_NO AS Phone, E_MAIL AS Email, CERTI_NO as ID, MEM_ADDR AS Address,"
                       + " SUM(Point_Alias.CHG_PNT) AS Point, COUNT(Point_Alias.CHG_PNT) AS Transactions"

                       + " FROM GARDEN_CRM.DBO.T_MEM_MST AS Member_Alias"

                       + " JOIN GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"
                       + Sql_JOIN_Card_Actived_OR_Disabled

                       + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1) AND (Point_Alias.CHG_PNT >= 0)"//

                       + Sql_Where_Card
                       + Sql_Where_Name
                       + Sql_Where_Phone
                       + Sql_Where_Email
                       + Sql_Where_Address

                       + " GROUP BY Member_Alias.Mem_Card, Member_Alias.Mem_Nm, MEM_BIRTHDAY, Sex_CD, MOBILE_NO, E_MAIL, CERTI_NO, MEM_ADDR"

                       + " )"
                       + " AS #Temp_Table"

                       + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                       ;
                }
                #endregion
                else
                #region Inquiry_discount
                if (ReportName == "Inquiry_discount")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY Amount DESC) AS Number,"
                        + " MatheKHTT AS Card, TenKhachHang AS Name,"
                        + " TenGianHang AS Shop,"
                        + " REPLACE(Amount, '.0000', '') AS Money, REPLACE(Discount, '.00', '') AS Discount,"
                        + " REPLACE(SUM(Point), '.00', '') AS Point,"
                        + " REPLACE(SUM(VC_Garden), '.00', '') AS VC_Garden, REPLACE(SUM(VC_shop), '.00', '') AS VC_Shop,"
                        + " REPLACE(SUM(Ticket), '.00', '') AS Ticket, ID AS Receipt"

                        + " FROM"
                        + "     (SELECT ID, MaTheKHTT, TenKhachHang, TenGianHang, SUM(TienCK) as Discount, SUM(Hoa_Don_Alias.TriGiaBan) as Amount,"
                        + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0011') AS Point,"
                        + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0012') AS VC_GARDEN,"
                        + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0023') AS VC_SHOP,"
                        + "     (SELECT SUM(Thanh_Toan_Alias.ThanhTien) WHERE MaHinhThuc LIKE '0025') AS Ticket"

                        + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don + " AS Hoa_Don_Alias"

                        + "     LEFT JOIN TOPOS_DB.DBO.ThanhToan" + Data_Table_Hoa_Don + " AS Thanh_Toan_Alias"
                        + "     ON Hoa_Don_Alias.MaHD = Thanh_Toan_Alias.MaHD"

                        + "     JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                        + "     ON Hoa_Don_Alias.MaThue = Gian_Hang_Alias.MaThue"

                        + "     WHERE (NgayBatDau >= @Time_Start_1) AND (NgayBatDau <= @Time_Finish_1)"
                        + "     GROUP BY ID, MaTheKHTT, TenKhachHang, MaHinhThuc, Gian_Hang_Alias.MaThue, Gian_Hang_Alias.TenGianHang"

                        + " ) AS Temp_Table_1"
                        + " GROUP BY ID, MaTheKHTT, TenGianHang, TenKhachHang, Amount, Discount"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Analys_transaction
                if (ReportName == "Analys_transaction")
                {
                    sqlQuery +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY Temp_Table_1.NgayBatDau) AS Number,"
                        + " Temp_Table_1.NgayBatDau AS Start_Date, REPLACE(Temp_Table_1.AM, '.0000', '') AS AM, Temp_Table_1.Trans,"
                        + " REPLACE(The_Kim_Cuong_tbl.AM, '.0000', '') AS AM_Diamon, The_Kim_Cuong_tbl.TranBozon AS Trans_Diamon, REPLACE(The_Vang_tbl.AM, '.0000', '') AS AM_Gold, The_Vang_tbl.TranBozon AS Trans_Gold, REPLACE(The_Xanh_tbl.AM, '.0000', '') AS AM_silver, The_Xanh_tbl.TranBozon AS Trans_silver"

                        + " FROM"
                        + " ("
                        + "     SELECT NgayBatDau, SUM(TriGiaBan) AS AM, COUNT(MaHD) as Trans"
                        + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                        + "     GROUP BY NgayBatDau"
                        + " ) AS Temp_Table_1"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT NgayBatDau, SubString(MaTheKHTT, 1, 2) AS CardNo, SUM(TriGiaBan) AS AM, COUNT(MaHD) AS TranBozon"
                        + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                        + "     WHERE SubString(MaTheKHTT, 1, 4) = '0101'"
                        + "     GROUP BY NgayBatDau, SubString(MaTheKHTT, 1, 2)"
                        + " ) AS The_Xanh_tbl"
                        + " ON Temp_Table_1.NgayBatDau = The_Xanh_tbl.NgayBatDau"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT NgayBatDau, SubString(MaTheKHTT, 1, 2) AS CardNo, SUM(TriGiaBan) AS AM, COUNT(MaHD) AS TranBozon"
                        + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                        + "     WHERE SubString(MaTheKHTT, 1, 4) = '0102'"
                        + "     GROUP BY NgayBatDau, SubString(MaTheKHTT, 1, 2)"
                        + " ) AS The_Vang_tbl"
                        + " ON Temp_Table_1.NgayBatDau = The_Vang_tbl.NgayBatDau"

                        + " LEFT JOIN"
                        + " ("
                        + "     SELECT NgayBatDau, SubString(MaTheKHTT, 1, 2) AS CardNo, SUM(TriGiaBan) AS AM, COUNT(MaHD) AS TranBozon"
                        + "     FROM TOPOS_DB.DBO." + Data_Table_Hoa_Don
                        + "     WHERE SubString(MaTheKHTT, 1, 4) = '0103'"
                        + "     GROUP BY NgayBatDau, SubString(MaTheKHTT, 1, 2)"
                        + " ) AS The_Kim_Cuong_tbl"
                        + " ON Temp_Table_1.NgayBatDau = The_Kim_Cuong_tbl.NgayBatDau"

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region Member_list
                if (ReportName == "Member_list")
                {
                    sqlQuery +=

                        " USE GARDEN_CRM"

                        + " DELETE T_MEM_NOW_PNT"

                        + " INSERT INTO T_MEM_NOW_PNT"

                        + " SELECT Member_Alias.Mem_SEQ, SUM(Point_Alias.CHG_PNT) AS NOW_TOT_PNT"
                        + " FROM T_MEM_MST AS Member_Alias"

                        + " JOIN T_PNT_HIS_INFO AS Point_Alias"
                        + " ON Member_Alias.Mem_SEQ = Point_Alias.Mem_SEQ"

                        + " GROUP BY Member_Alias.Mem_SEQ"
                        + " ORDER BY Member_Alias.Mem_SEQ"

                        //
                        + " SELECT * FROM"
                        + " ("

                        + " SELECT ROW_NUMBER() OVER(ORDER BY " + sqlOrder + ") AS Number,"
                        + " Mem_Card AS Card, Mem_Nm AS Name, NOW_TOT_PNT AS Point, MEM_BIRTHDAY AS Birthday, YEAR(GETDATE()) - YEAR(Mem_birthday) AS Age, Sex_CD AS Sex, MOBILE_NO AS Phone, E_MAIL AS Email, CERTI_NO as ID, MEM_ADDR AS Address,"
                        + " REG_ID, REG_DT, MOD_ID, MOD_DT"

                        + " FROM GARDEN_CRM.DBO.T_MEM_MST Member_Alias"

                        + " LEFT JOIN GARDEN_CRM.DBO.T_MEM_NOW_PNT AS Point_NOW_Alias"
                        + " ON Member_Alias.Mem_SEQ = Point_NOW_Alias.Mem_SEQ "

                        + " WHERE (REG_DT >= @Time_Start_1) AND (REG_DT <= @Time_Finish_1)"

                        + Sql_Where_Card
                        + Sql_Where_Name
                        + Sql_Where_Phone
                        + Sql_Where_Email
                        + Sql_Where_Address

                        + Sql_Where_No_Name
                        + Sql_Where_No_ID
                        + Sql_Where_Duplicate_Phone
                        + Sql_Where_Duplicate_Email

                        + " )"
                        + " AS #Temp_Table"

                        + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        ;
                }
                #endregion
                else
                #region FC_History
                if (ReportName == "FC_History")
                {
                    if (Card != a.e)
                    {
                        //
                        int Year_int = a.Time.Year;
                        int Month_int = a.Time.Month;

                        ai Thang_Nam = a.e;

                        //
                        if (Month_int < 10)
                        {
                            Thang_Nam += "0" + Month_int.aS();
                        }
                        else
                        {
                            Thang_Nam += Month_int.aS();
                        }

                        Thang_Nam += a.Time.Year;

                        sqlQuery +=

                            " USE TOPOS_DB"

                            + " DECLARE @CTKichHoatID NVARCHAR(MAX) SET @CTKichHoatID = (SELECT TOP 1 CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card) ORDER BY NgayKichHoat DESC)"

                            + " SELECT * FROM"
                            + " ("

                            + " SELECT ROW_NUMBER() OVER(ORDER BY Hoa_Don_Alias.NgayGioQuet DESC, Nap_Rut_Alias.CTNapTien_Chi_Tiet_ID DESC) AS Number,"

                            + " TenHH AS Hang_Hoa, TenHinhThuc AS Hinh_Thuc, CONVERT(BIGINT, TienTruocThanhToan) AS Truoc, CONVERT(BIGINT, ThanhTien) AS So_Tien, CONVERT(BIGINT, TienSauThanhToan) AS Sau, Nap_Rut_Alias.MaHD AS Hoa_Don, CONVERT(DATETIME, NgayGioQuet, 103) AS NgayGioQuet"

                            + " FROM TTT_CTNapTien_HinhThucThanhToan Nap_Rut_Alias"

                            + " LEFT JOIN CTHoaDon" + Thang_Nam + " Hoa_Don_Alias"
                            + " ON Nap_Rut_Alias.HoaDonID = Hoa_Don_Alias.MaHD"

                            + " LEFT JOIN HinhThucThanhToan Hinh_Thuc_Thanh_Toan_Alias"
                            + " ON Nap_Rut_Alias.MaHinhThuc = Hinh_Thuc_Thanh_Toan_Alias.MaHinhThuc"

                            + " WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card)))"

                            + " )"
                            + " AS #Temp_Table"

                            + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                            ;
                    }
                }
                #endregion

                if (sqlQuery != a.e)
                {
                    //All Page
                    if (!Is_one_Page)
                        sqlQuery.aRemove(" WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))");

                    //lib.Add_SQL_Parameters(ref Sql_Command, SQL_Parameters_Array, SQL_Value_Array);
                    sql = sqlQuery.asql();
                    sql.DataReader();

                    int j1 = 0;
                    Dynamic_Control_Holder_pnl.Controls.Clear();

                    int Total_Column = sql.Data.FieldCount;

                    Primary_Column = a.aS(ViewState["Primary_Column"]);

                    //
                    aray allShopCode = new aray();
                    aray allShopName = new aray();

                    if ((Application["allShopCode"] == null) || (Application["allShopName"] == null))
                    {
                        crm.Read_allShopCode(allShopCode, allShopName);
                    }
                    else
                    {
                        allShopCode = (string[])Application["allShopCode"];
                        allShopName = (string[])Application["allShopName"];
                    }

                    //
                    HtmlTable Report_htbl = htable.TBL("Dynamic_Content_To_Edit", "1", "1", 1, 0, 0, a.e);
                    Dynamic_Control_Holder_pnl.Controls.Add(Report_htbl);

                    Report_htbl.Style.Add("border-collapse", "collapse");
                    Report_htbl.Style.Add("width", "98%");

                    //Excel
                    ai folderName = a.Guid;
                    afolder folder = apath.AddFolder(Server.MapPath("~/File/Download/"), folderName);
                    folder.Create();

                    ai fileName = Report_lbl.Text.ai().EncodeSpaceVar
                        + "__Page_" + ViewState["Page"].aS() + "_of_" + ViewState["Total_Page"]
                        + ".xlsx";

                    if (!Is_one_Page)
                    {
                        fileName = Report_lbl.Text.ai().EncodeSpaceVar
                            + "__All"
                            + ".xlsx";
                    }

                    if (Filter_Time_tr.Visible)
                    {
                        if (Filter_Time_2_tr.Visible)
                        {
                            if (Time_Start_2 == Time_Finish_2)
                            {
                                if (Is_one_Page)
                                {
                                    fileName = Report_lbl.Text.ai().EncodeSpaceVar
                                        + "__From__" + Time_Start_1.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_1.EncodedDate.Replace("/", "-")
                                        + "__BUT_NOT__At__" + Time_Start_2.EncodedDate.Replace("/", "-")
                                        + "__Page_" + ViewState["Page"].aS() + "_of_" + ViewState["Total_Page"]
                                        + ".xlsx";
                                }
                                else
                                {
                                    fileName = Report_lbl.Text.ai().EncodeSpaceVar
                                        + "__From__" + Time_Start_1.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_1.EncodedDate.Replace("/", "-")
                                        + "__BUT_NOT__At__" + Time_Start_2.EncodedDate.Replace("/", "-")
                                        + "__All"
                                        + ".xlsx";
                                }
                            }
                            else
                            {
                                if (Is_one_Page)
                                {
                                    fileName = Report_lbl.Text.ai().EncodeSpaceVar
                                        + "__From__" + Time_Start_1.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_1.EncodedDate.Replace("/", "-")
                                        + "__BUT_NOT__From__" + Time_Start_2.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_2.EncodedDate.Replace("/", "-")
                                        + "__Page_" + ViewState["Page"].aS() + "_of_" + ViewState["Total_Page"]
                                        + ".xlsx";
                                }
                                else
                                {
                                    fileName = Report_lbl.Text.ai().EncodeSpaceVar
                                        + "__From__" + Time_Start_1.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_1.EncodedDate.Replace("/", "-")
                                        + "__BUT_NOT__From__" + Time_Start_2.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_2.EncodedDate.Replace("/", "-")
                                        + "__All"
                                        + ".xlsx";
                                }
                            }
                        }
                        else
                        {
                            if (Is_one_Page)
                            {
                                fileName = Report_lbl.Text.ai().EncodeSpaceVar
                                    + "__From__" + Time_Start_1.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_1.EncodedDate.Replace("/", "-")
                                    + "__Page_" + ViewState["Page"].aS() + "_of_" + ViewState["Total_Page"]
                                    + ".xlsx";
                            }
                            else
                            {
                                fileName = Report_lbl.Text.ai().EncodeSpaceVar
                                   + "__From__" + Time_Start_1.EncodedDate.Replace("/", "-") + "__to__" + Time_Finish_1.EncodedDate.Replace("/", "-")
                                   + "__All"
                                   + ".xlsx";
                            }
                        }
                    }

                    afile file = apath.Add(folder, fileName);
                    ExcelPackage excel = new ExcelPackage(file);
                    var sheet = excel.Workbook.Worksheets.Add(Report_lbl.Text);

                    int Order = 0;
                    long Total_Money = 0;
                    long Total_Point = 0;

                    //
                    string[] Column_Name_Array = new string[0];

                    if (ReportName == "Add_point")
                    {
                        if (UserName.ToLower() == "Pos2".ToLower())
                        {
                            Column_Name_Array = new string[] { "Khách hàng", "Shop", "Tiền", "Điểm", "Hóa đơn", "Ngày mua", "Ngày tích", "POS", "Tài khoản" };
                        }
                        else
                        {
                            Column_Name_Array = new string[] { "ID", "Number", "Type", "Department", "Card", "Name", "Shop", "Shop Code", "Money", "Point", "Receipt", "Add By", "Add Time", "Add At", "Buy Time", "Reason" };
                        }
                    }
                    else
                        if (ReportName == "Add_point_by_member")
                    {
                        if (UserName.ToLower() == "Pos2".ToLower())
                        {
                            Column_Name_Array = new string[] { "Khách hàng", "Shop", "Tiền", "Điểm", "Hóa đơn", "Ngày mua", "Ngày tích", "POS", "Tài khoản" };
                        }
                        else
                        {
                            Column_Name_Array = new string[] { "ID", "Number", "Type", "Department", "Card", "Name", "Phone", "Shop", "Shop Code", "Money", "Point", "Receipt", "Add By", "Add Time", "Add At", "Buy Time", "Reason" };
                        }
                    }
                    else
                        if (ReportName == "Total_Add_point_by_member")
                    {
                        Column_Name_Array = new string[] { "Number", "Card", "Name", "Phone", "Money", "Point" };
                    }

                    //
                    try
                    {
                        while (sql.Data.Read())
                        {
                            ai Primary_Key = a.e;

                            if (Primary_Column != a.e)
                            {
                                Primary_Key = sql.Data[Primary_Column.aS()].aS();
                            }

                            ai Number = sql.Data["Number"].ai().SplitThousand;

                            bool Valid_1 = true;

                            ai NEW_Add_Point_Manual_String = a.e;

                            ai Type = a.e;
                            ai Department = a.e;
                            ai Add_By_Group = a.e;

                            ai POS = a.e;
                            ai Cashier = a.e;

                            //
                            Card = a.e;
                            Name = a.e;

                            ai Point = a.e;
                            Add_Time = a.e;

                            ai Buy_Time = a.e;
                            ai Point_Raw = a.e;

                            ai Add_Point_Manual_String = a.e;
                            ai Add_Point_Manual_String_Backup = a.e;
                            bool Is_Manual_Add_Point = false;

                            Shop = a.e;
                            ai Shop_Code = a.e;
                            ai Shop_AND_Code = a.e;

                            //
                            ai SoHopDong_OK = a.e;
                            ai TenGianHang_OK = a.e;

                            ai SoHopDong = a.e;
                            ai TenGianHang = a.e;
                            ai SoHopDong_2 = a.e;
                            ai TenGianHang_2 = a.e;
                            ai Brand_Nm = a.e;

                            ai MaThue = a.e;

                            ai Receipt = a.e;
                            ai Money = a.e;
                            ai Money_Raw = a.e;

                            #region Add_point
                            if (ReportName == "Add_point")
                            {
                                Add_By_Group = sql.Data["Add_By_Group"].aS();

                                POS = sql.Data["Add_At"].aS();
                                Cashier = sql.Data["Add_By"].aS();

                                if (POS == a.e)
                                {
                                    POS = sql.Data["POS_No"].aS();
                                }

                                //
                                Card = sql.Data["Card"].aS();
                                Name = sql.Data["Name"].aS();

                                Point = sql.Data["Point"].aS();
                                Add_Time = sql.Data["Add_Time"].aS();

                                Add_Point_Manual_String = sql.Data["Add_point_Manual_String"].aS();
                                Add_Point_Manual_String_Backup = sql.Data["Add_point_Manual_String_Backup"].aS();
                                Is_Manual_Add_Point = sql.Data["Is_Manual_Add_Point"].ai();

                                if (Add_Point_Manual_String_Backup == a.e)
                                {
                                    Add_Point_Manual_String_Backup = Add_Point_Manual_String;
                                }

                                Add_Point_Manual_String = Add_Point_Manual_String.Replace("Vicenzo/Pabini", "Vicenzo-Pabini");

                                //
                                SoHopDong = sql.Data["SoHopDong"].aS();
                                TenGianHang = sql.Data["TenGianHang"].aS();

                                SoHopDong_2 = sql.Data["SoHopDong_2"].aS();
                                TenGianHang_2 = sql.Data["TenGianHang_2"].aS();
                                Brand_Nm = sql.Data["Brand_Nm"].aS();

                                MaThue = sql.Data["MaThue"].aS();

                                //+ " AND ((Point_Alias.PNT_RSN LIKE 'AA%|%') OR (Point_Alias.PNT_RSN LIKE 'Ten%Quay%|%'))"
                                //+ " AND ((Point_Alias.PNT_RSN IS NULL) OR ((Point_Alias.PNT_RSN NOT LIKE 'AA%|%') AND (Point_Alias.PNT_RSN NOT LIKE 'Ten%Quay%|%')))"

                                bool USE_Add_Point_Manual_String = false;

                                if (!Is_Manual_Add_Point)
                                {
                                    if ((POS.ToLower().StartsWith("aa")) && (Cashier.ToLower().StartsWith("aa")))
                                    {
                                        Type = "Tự động";
                                        Department = "POS";

                                        SoHopDong_OK = SoHopDong_2;

                                        if (TenGianHang_2 != a.e)
                                        {
                                            TenGianHang_OK = TenGianHang_2;
                                        }
                                        else
                                        {
                                            TenGianHang_OK = Brand_Nm;
                                        }
                                    }
                                    else
                                        if ((POS.ToLower().StartsWith("aa")) && (!Cashier.ToLower().StartsWith("aa")))
                                    {
                                        Type = "Tự động";
                                        Department = "CRM";

                                        SoHopDong_OK = SoHopDong_2;

                                        if (TenGianHang_2 != a.e)
                                        {
                                            TenGianHang_OK = TenGianHang_2;
                                        }
                                        else
                                        {
                                            TenGianHang_OK = Brand_Nm;
                                        }
                                    }
                                    else
                                            if (
                                                (Add_Point_Manual_String.Contains("|"))
                                                && ((Add_Point_Manual_String.ToLower().StartsWith("aa")) || (Add_Point_Manual_String.ToLower().StartsWith("ten")))
                                                )
                                    {
                                        USE_Add_Point_Manual_String = true;

                                        Type = "Thủ công";
                                        Department = "Phần mềm (Cũ)";

                                        string[] Add_Point_Manual_String_Array = Add_Point_Manual_String.Split('|');

                                        if (Add_Point_Manual_String_Array.Length > 2)
                                        {
                                            POS = Add_Point_Manual_String_Array[0];
                                            Cashier = Add_Point_Manual_String_Array[1];

                                            if (Add_Point_Manual_String_Array.Length == 4)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = a.e;
                                                Receipt = Add_Point_Manual_String_Array[3];
                                                Money = a.e;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 5)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];

                                                string[] Shop_Code_Array = new string[0];

                                                if (Shop_Code.Contains(")"))
                                                {
                                                    Shop_Code_Array = Shop_Code.Split(')');
                                                }
                                                else
                                                    if (Shop_Code.Contains(" "))
                                                {
                                                    Shop_Code_Array = Shop_Code.Split(' ');
                                                }

                                                //
                                                if (Shop_Code_Array.Length == 2)
                                                {
                                                    Shop_Code = Shop_Code_Array[0];
                                                    Receipt = Shop_Code_Array[1];
                                                }

                                                Money = a.e;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 6)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];
                                                Receipt = Add_Point_Manual_String_Array[4];
                                                Money = a.e;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 7)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];
                                                Receipt = Add_Point_Manual_String_Array[4];
                                                Money = Add_Point_Manual_String_Array[5];
                                            }
                                        }

                                        //
                                        Shop_Code.aRemSpace();
                                        Shop_Code.aRemFrom_Text_End(a.Space);

                                        Shop.aRemSpace();

                                        if (Shop_Code == a.e)
                                        {
                                            Shop_Code = Shop.New.aGetFrom_Text_Text("(", ")");
                                        }

                                        //
                                        if (Shop_Code != a.e)
                                        {
                                            Shop_Code.aRemFrom_Text_End(")");
                                            Shop.aRemFrom_Text_End("(");
                                        }

                                        if (Shop == a.e)
                                        {
                                            if (Shop_Code != a.e)
                                            {
                                                Shop = lib.Find_Value_In_Two_Array_Start_With(allShopCode, allShopName, Shop_Code);
                                            }
                                        }

                                        //
                                        if (Shop == a.e)
                                        {
                                            Shop = "XXX";
                                        }

                                        //
                                        if (Shop_Code == a.e)
                                        {
                                            Shop_Code = "XXX";
                                        }

                                        Shop_AND_Code = Shop + " (" + Shop_Code + ")";

                                        //
                                        SoHopDong_OK = Shop_Code;
                                        TenGianHang_OK = Shop;

                                        //
                                        if (Receipt == a.e)
                                        {
                                            Receipt = "XXX";
                                        }

                                        //
                                        if (Money == a.e)
                                        {
                                            Money = (lib.Convert_String_To_Int(Point, 0) * 15000).aS();
                                        }

                                        //
                                        Money = Money.Replace(",", a.e).Replace(".", a.e);

                                        //
                                        Money_Raw = Money;

                                        if (Money.NoZero && Money.IsID)
                                        {
                                            Total_Money += lib.Convert_String_To_BigInt(Money, 0);
                                            Money.SplitThousand += " VND";
                                        }

                                        //
                                        Point_Raw = Point;

                                        if (Point.NoZero && Point.IsID)
                                        {
                                            Total_Point += lib.Convert_String_To_BigInt(Point, 0);
                                            a.run(Point.SplitThousand);
                                        }

                                        //
                                        Buy_Time = Add_Point_Manual_String_Array[Add_Point_Manual_String_Array.Length - 1];

                                        if (Buy_Time.Contains(" "))
                                        {
                                            //
                                            string[] Add_Point_Manual_String_Backup_Array = Add_Point_Manual_String_Backup.Split('|');

                                            if (Add_Point_Manual_String_Backup_Array.Length > 0)
                                            {
                                                Buy_Time = Add_Point_Manual_String_Backup_Array[Add_Point_Manual_String_Backup_Array.Length - 1];
                                            }
                                        }

                                        if (Buy_Time.Contains(" "))
                                        {
                                            Buy_Time = Buy_Time.Split(' ')[0];
                                        }
                                    }
                                    else
                                    //if (
                                    //(Add_Point_Manual_String.Contains("/"))
                                    //&& ((!Add_Point_Manual_String.ToLower().StartsWith("aa")) && (!Add_Point_Manual_String.ToLower().StartsWith("ten")))
                                    //)
                                    {
                                        USE_Add_Point_Manual_String = true;

                                        Type = "Thủ công";
                                        Department = "CRM (Cũ)";
                                        Department = "Phần mềm (Cũ)";

                                        //
                                        string[] Add_Point_Manual_String_Array = Add_Point_Manual_String.Split('/');

                                        //Ko co (/)
                                        if (Add_Point_Manual_String_Array.Length == 1)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String.ToLower();
                                        }
                                        else
                                        if (Add_Point_Manual_String_Array.Length == 2)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String_Array[0];
                                            ai Receipt_AND_Money = Add_Point_Manual_String_Array[1];

                                            string[] Receipt_AND_Money_Array = Receipt_AND_Money.Split('-');

                                            if (Receipt_AND_Money_Array.Length == 1)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0];
                                            }
                                            else
                                                if (Receipt_AND_Money_Array.Length == 2)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0];
                                                Money = Receipt_AND_Money_Array[1];
                                            }
                                        }
                                        else
                                        if (Add_Point_Manual_String_Array.Length == 3)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String_Array[0];
                                            ai Receipt_AND_Money = Add_Point_Manual_String_Array[1];

                                            string[] Receipt_AND_Money_Array = Receipt_AND_Money.Split('-');

                                            if (Receipt_AND_Money_Array.Length == 1)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[2] + ")";
                                            }
                                            else
                                                if (Receipt_AND_Money_Array.Length == 2)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[2] + ")";
                                                Money = Receipt_AND_Money_Array[1];
                                            }
                                        }
                                        else
                                        if (Add_Point_Manual_String_Array.Length >= 4)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String_Array[0] + " (" + Add_Point_Manual_String_Array[1] + ")";
                                            ai Receipt_AND_Money = Add_Point_Manual_String_Array[2];

                                            string[] Receipt_AND_Money_Array = Receipt_AND_Money.Split('-');

                                            if (Receipt_AND_Money_Array.Length == 1)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[3] + ")";
                                            }
                                            else
                                                if (Receipt_AND_Money_Array.Length == 2)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[3] + ")";
                                                Money = Receipt_AND_Money_Array[1];
                                            }
                                        }

                                        //
                                        //SoHopDong_OK = Shop_AND_Code;
                                        TenGianHang_OK = Shop_AND_Code;

                                        //
                                        if (Money.aEndsWith("k"))
                                        {
                                            Money -= "k";
                                            Money += "000";
                                        }

                                        //
                                        if (Receipt == a.e)
                                        {
                                            Receipt = "XXX";
                                        }

                                        //
                                        if (Money == a.e)
                                        {
                                            Money = (lib.Convert_String_To_Int(Point, 0) * 15000).aS();
                                        }

                                        //
                                        Money = Money.Replace(",", a.e).Replace(".", a.e);

                                        //
                                        Money_Raw = Money;

                                        if (Money.NoZero && Money.IsID)
                                        {
                                            Total_Money += lib.Convert_String_To_BigInt(Money, 0);
                                            Money.SplitThousand += " VND";
                                        }

                                        //
                                        Point_Raw = Point;

                                        if (Point.NoZero && Point.IsID)
                                        {
                                            Total_Point += lib.Convert_String_To_BigInt(Point, 0);
                                            a.run(Point.SplitThousand);
                                        }
                                    }
                                }
                                else//Thủ công (MỚI)
                                {
                                    Type = "Thủ công";
                                    Department = Add_By_Group;

                                    if (MaThue == "AA-StarFitness")
                                    {
                                        SoHopDong_OK = "StarFitness";
                                        TenGianHang_OK = "StarFitness";
                                    }
                                    else
                                    {
                                        if ((SoHopDong != a.e) && (TenGianHang != a.e))
                                        {
                                            SoHopDong_OK = SoHopDong;
                                            TenGianHang_OK = TenGianHang;
                                        }
                                        else
                                            if ((SoHopDong_2 != a.e) && (TenGianHang_2 != a.e))
                                        {
                                            SoHopDong_OK = SoHopDong_2;
                                            TenGianHang_OK = TenGianHang_2;
                                        }
                                        else
                                        {
                                            TenGianHang_OK = Brand_Nm;
                                        }
                                    }
                                }

                                //
                                if (!USE_Add_Point_Manual_String)
                                {
                                    Buy_Time = sql.Data["Buy_Time"].aS();
                                    Point_Raw = Point;

                                    Receipt = sql.Data["Receipt"].aS();
                                    Money = sql.Data["Money"].aS();
                                    Money_Raw = Money;

                                    //
                                    if ((Money != a.e) && (Money != "0"))
                                    {
                                        Money.SplitThousand += " VND";
                                    }

                                    a.run(Point.SplitThousand);
                                }
                            }
                            #endregion

                            #region Add_point_by_member
                            if (ReportName == "Add_point_by_member")
                            {
                                Add_By_Group = sql.Data["Add_By_Group"].aS();

                                POS = sql.Data["Add_At"].aS();
                                Cashier = sql.Data["Add_By"].aS();

                                if (POS == a.e)
                                {
                                    POS = sql.Data["POS_No"].aS();
                                }

                                //
                                Card = sql.Data["Card"].aS();
                                Name = sql.Data["Name"].aS();
                                Phone = sql.Data["Phone"].aS();

                                Point = sql.Data["Point"].aS();
                                Add_Time = sql.Data["Add_Time"].aS();

                                Add_Point_Manual_String = sql.Data["Add_point_Manual_String"].aS();
                                Add_Point_Manual_String_Backup = sql.Data["Add_point_Manual_String_Backup"].aS();
                                Is_Manual_Add_Point = sql.Data["Is_Manual_Add_Point"].ai();

                                //
                                if (Add_Point_Manual_String_Backup == a.e)
                                {
                                    Add_Point_Manual_String_Backup = Add_Point_Manual_String;
                                }

                                Add_Point_Manual_String = Add_Point_Manual_String.Replace("Vicenzo/Pabini", "Vicenzo-Pabini");

                                //
                                SoHopDong = sql.Data["SoHopDong"].aS();
                                TenGianHang = sql.Data["TenGianHang"].aS();

                                SoHopDong_2 = sql.Data["SoHopDong_2"].aS();
                                TenGianHang_2 = sql.Data["TenGianHang_2"].aS();
                                Brand_Nm = sql.Data["Brand_Nm"].aS();

                                MaThue = sql.Data["MaThue"].aS();

                                //+ " AND ((Point_Alias.PNT_RSN LIKE 'AA%|%') OR (Point_Alias.PNT_RSN LIKE 'Ten%Quay%|%'))"
                                //+ " AND ((Point_Alias.PNT_RSN IS NULL) OR ((Point_Alias.PNT_RSN NOT LIKE 'AA%|%') AND (Point_Alias.PNT_RSN NOT LIKE 'Ten%Quay%|%')))"

                                bool USE_Add_Point_Manual_String = false;

                                if (!Is_Manual_Add_Point)
                                {
                                    if ((POS.ToLower().StartsWith("aa")) && (Cashier.ToLower().StartsWith("aa")))
                                    {
                                        Type = "Tự động";
                                        Department = "POS";

                                        SoHopDong_OK = SoHopDong_2;

                                        if (TenGianHang_2 != a.e)
                                        {
                                            TenGianHang_OK = TenGianHang_2;
                                        }
                                        else
                                        {
                                            TenGianHang_OK = Brand_Nm;
                                        }
                                    }
                                    else
                                        if ((POS.ToLower().StartsWith("aa")) && (!Cashier.ToLower().StartsWith("aa")))
                                    {
                                        Type = "Tự động";
                                        Department = "CRM";

                                        SoHopDong_OK = SoHopDong_2;

                                        if (TenGianHang_2 != a.e)
                                        {
                                            TenGianHang_OK = TenGianHang_2;
                                        }
                                        else
                                        {
                                            TenGianHang_OK = Brand_Nm;
                                        }
                                    }
                                    else
                                    if (
                                        (Add_Point_Manual_String.Contains("|"))
                                        && ((Add_Point_Manual_String.ToLower().StartsWith("aa")) || (Add_Point_Manual_String.ToLower().StartsWith("ten")))
                                        )
                                    {
                                        USE_Add_Point_Manual_String = true;

                                        Type = "Thủ công";
                                        Department = "Phần mềm (Cũ)";

                                        string[] Add_Point_Manual_String_Array = Add_Point_Manual_String.Split('|');

                                        if (Add_Point_Manual_String_Array.Length > 2)
                                        {
                                            POS = Add_Point_Manual_String_Array[0];
                                            Cashier = Add_Point_Manual_String_Array[1];

                                            if (Add_Point_Manual_String_Array.Length == 4)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = a.e;
                                                Receipt = Add_Point_Manual_String_Array[3];
                                                Money = a.e;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 5)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];

                                                string[] Shop_Code_Array = new string[0];

                                                if (Shop_Code.Contains(")"))
                                                {
                                                    Shop_Code_Array = Shop_Code.Split(')');
                                                }
                                                else
                                                    if (Shop_Code.Contains(" "))
                                                {
                                                    Shop_Code_Array = Shop_Code.Split(' ');
                                                }

                                                //
                                                if (Shop_Code_Array.Length == 2)
                                                {
                                                    Shop_Code = Shop_Code_Array[0];
                                                    Receipt = Shop_Code_Array[1];
                                                }

                                                Money = a.e;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 6)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];
                                                Receipt = Add_Point_Manual_String_Array[4];
                                                Money = a.e;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 7)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];
                                                Receipt = Add_Point_Manual_String_Array[4];
                                                Money = Add_Point_Manual_String_Array[5];
                                            }
                                        }

                                        //
                                        Shop_Code.aRemSpace();
                                        Shop_Code.aRemFrom_Text_End(a.Space);

                                        Shop.aRemSpace();

                                        //
                                        if (Shop_Code == a.e)
                                        {
                                            Shop_Code = Shop.aGetFrom_Text_Text("(", ")");
                                        }

                                        //
                                        if (Shop_Code != a.e)
                                        {
                                            Shop_Code.aRemFrom_Text_End(")");
                                            Shop.aRemFrom_Text_End("(");
                                        }

                                        if (Shop == a.e)
                                        {
                                            if (Shop_Code != a.e)
                                            {
                                                Shop = lib.Find_Value_In_Two_Array_Start_With(allShopCode, allShopName, Shop_Code);
                                            }
                                        }

                                        //
                                        if (Shop == a.e)
                                        {
                                            Shop = "XXX";
                                        }

                                        //
                                        if (Shop_Code == a.e)
                                        {
                                            Shop_Code = "XXX";
                                        }

                                        Shop_AND_Code = Shop + " (" + Shop_Code + ")";

                                        //
                                        SoHopDong_OK = Shop_Code;
                                        TenGianHang_OK = Shop;

                                        //
                                        if (Receipt == a.e)
                                        {
                                            Receipt = "XXX";
                                        }

                                        //
                                        if (Money == a.e)
                                        {
                                            Money = (lib.Convert_String_To_Int(Point, 0) * 15000).aS();
                                        }

                                        //
                                        Money = Money.Replace(",", a.e).Replace(".", a.e);

                                        //
                                        Money_Raw = Money;

                                        //
                                        if (Money.NoZero && Money.IsID)
                                        {
                                            Total_Money += lib.Convert_String_To_BigInt(Money, 0);
                                            Money.SplitThousand += " VND";
                                        }

                                        //
                                        Point_Raw = Point;

                                        if (Point.NoZero && Point.IsID)
                                        {
                                            Total_Point += lib.Convert_String_To_BigInt(Point, 0);
                                            a.run(Point.SplitThousand);
                                        }

                                        //
                                        Buy_Time = Add_Point_Manual_String_Array[Add_Point_Manual_String_Array.Length - 1];

                                        if (Buy_Time.Contains(" "))
                                        {
                                            //
                                            string[] Add_Point_Manual_String_Backup_Array = Add_Point_Manual_String_Backup.Split('|');

                                            if (Add_Point_Manual_String_Backup_Array.Length > 0)
                                            {
                                                Buy_Time = Add_Point_Manual_String_Backup_Array[Add_Point_Manual_String_Backup_Array.Length - 1];
                                            }
                                        }

                                        if (Buy_Time.Contains(" "))
                                        {
                                            Buy_Time = Buy_Time.Split(' ')[0];
                                        }
                                    }
                                    else
                                    //if (
                                    //(Add_Point_Manual_String.Contains("/"))
                                    //&& ((!Add_Point_Manual_String.ToLower().StartsWith("aa")) && (!Add_Point_Manual_String.ToLower().StartsWith("ten")))
                                    //)
                                    {
                                        USE_Add_Point_Manual_String = true;

                                        Type = "Thủ công";
                                        Department = "CRM (Cũ)";
                                        Department = "Phần mềm (Cũ)";

                                        //
                                        string[] Add_Point_Manual_String_Array = Add_Point_Manual_String.Split('/');

                                        //Ko co (/)
                                        if (Add_Point_Manual_String_Array.Length == 1)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String.ToLower();
                                        }
                                        else
                                        if (Add_Point_Manual_String_Array.Length == 2)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String_Array[0];
                                            ai Receipt_AND_Money = Add_Point_Manual_String_Array[1];

                                            string[] Receipt_AND_Money_Array = Receipt_AND_Money.Split('-');

                                            if (Receipt_AND_Money_Array.Length == 1)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0];
                                            }
                                            else
                                            if (Receipt_AND_Money_Array.Length == 2)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0];
                                                Money = Receipt_AND_Money_Array[1];
                                            }
                                        }
                                        else
                                        if (Add_Point_Manual_String_Array.Length == 3)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String_Array[0];
                                            ai Receipt_AND_Money = Add_Point_Manual_String_Array[1];

                                            string[] Receipt_AND_Money_Array = Receipt_AND_Money.Split('-');

                                            if (Receipt_AND_Money_Array.Length == 1)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[2] + ")";
                                            }
                                            else
                                                if (Receipt_AND_Money_Array.Length == 2)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[2] + ")";
                                                Money = Receipt_AND_Money_Array[1];
                                            }
                                        }
                                        else
                                        if (Add_Point_Manual_String_Array.Length >= 4)
                                        {
                                            Shop_AND_Code = Add_Point_Manual_String_Array[0] + " (" + Add_Point_Manual_String_Array[1] + ")";
                                            ai Receipt_AND_Money = Add_Point_Manual_String_Array[2];

                                            string[] Receipt_AND_Money_Array = Receipt_AND_Money.Split('-');

                                            if (Receipt_AND_Money_Array.Length == 1)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[3] + ")";
                                            }
                                            else
                                            if (Receipt_AND_Money_Array.Length == 2)
                                            {
                                                Receipt = Receipt_AND_Money_Array[0] + " (" + Add_Point_Manual_String_Array[3] + ")";
                                                Money = Receipt_AND_Money_Array[1];
                                            }
                                        }

                                        //
                                        //SoHopDong_OK = Shop_AND_Code;
                                        TenGianHang_OK = Shop_AND_Code;

                                        //
                                        if (Money.aEndsWith("k"))
                                        {
                                            Money -= "k";
                                            Money += "000";
                                        }

                                        //
                                        if (Receipt == a.e)
                                        {
                                            Receipt = "XXX";
                                        }

                                        //
                                        if (Money == a.e)
                                        {
                                            Money = (lib.Convert_String_To_Int(Point, 0) * 15000).aS();
                                        }

                                        //
                                        Money = Money.Replace(",", a.e).Replace(".", a.e);

                                        //
                                        Money_Raw = Money;

                                        //
                                        if (Money.NoZero && Money.IsID)
                                        {
                                            Total_Money += lib.Convert_String_To_BigInt(Money, 0);
                                            Money.SplitThousand += " VND";
                                        }

                                        //
                                        Point_Raw = Point;

                                        if (Point.NoZero && Point.IsID)
                                        {
                                            Total_Point += lib.Convert_String_To_BigInt(Point, 0);
                                            a.run(Point.SplitThousand);
                                        }
                                    }
                                }
                                else//Thủ công (MỚI)
                                {
                                    Type = "Thủ công";
                                    Department = Add_By_Group;

                                    if (MaThue == "AA-StarFitness")
                                    {
                                        SoHopDong_OK = "StarFitness";
                                        TenGianHang_OK = "StarFitness";
                                    }
                                    else
                                    {
                                        if ((SoHopDong != a.e) && (TenGianHang != a.e))
                                        {
                                            SoHopDong_OK = SoHopDong;
                                            TenGianHang_OK = TenGianHang;
                                        }
                                        else
                                        if ((SoHopDong_2 != a.e) && (TenGianHang_2 != a.e))
                                        {
                                            SoHopDong_OK = SoHopDong_2;
                                            TenGianHang_OK = TenGianHang_2;
                                        }
                                        else
                                        {
                                            TenGianHang_OK = Brand_Nm;
                                        }
                                    }
                                }

                                //
                                if (!USE_Add_Point_Manual_String)
                                {
                                    Buy_Time = sql.Data["Buy_Time"].aS();
                                    Point_Raw = Point;

                                    Receipt = sql.Data["Receipt"].aS();
                                    Money = sql.Data["Money"].aS();
                                    Money_Raw = Money;

                                    //
                                    if (Money.NoZero)
                                    {
                                        Money.SplitThousand += " VND";
                                    }

                                    a.run(Point.SplitThousand);
                                }
                            }
                            #endregion

                            #region Total_Add_point_by_member
                            if (ReportName == "Total_Add_point_by_member")
                            {
                                Card = sql.Data["Card"].aS();
                                Name = sql.Data["Name"].aS();
                                Phone = sql.Data["Phone"].aS();

                                Money = sql.Data["Money"].aS();
                                Point = sql.Data["Point"].aS();

                                Money_Raw = Money;
                                Point_Raw = Point;
                            }
                            #endregion

                            //
                            if (j1 == 0)
                            {
                                HtmlTableRow TR = htable.TR(a.e);
                                Report_htbl.Rows.Add(TR);

                                sheet.Cells[1, 3].Value = Report_lbl.Text;
                                sheet.Cells[1, 3, 1, 9].Merge = true;

                                sheet.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
                                sheet.Cells[1, 3].Style.Font.Bold = true;
                                sheet.Cells[1, 3].Style.Font.Size = 20;

                                //
                                if (Filter_Time_tr.Visible)
                                {
                                    if (Filter_Time_2_tr.Visible)
                                    {
                                        if (Time_Start_2 == Time_Finish_2)
                                        {
                                            if (Is_one_Page)
                                            {
                                                sheet.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- At: " + Time_Start_2 + " (Page " + lib.Split_Thousand(ViewState["Page"].aS()) + " of " + lib.Split_Thousand(ViewState["Total_Page"].aS()) + ")";
                                            }
                                            else
                                            {
                                                sheet.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- At: " + Time_Start_2 + " (All)";
                                            }
                                        }
                                        else
                                        {
                                            if (Is_one_Page)
                                            {
                                                sheet.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- FROM: " + Time_Start_2 + " - to - " + Time_Finish_2 + " (Page " + lib.Split_Thousand(ViewState["Page"].aS()) + " of " + lib.Split_Thousand(ViewState["Total_Page"].aS()) + ")";
                                            }
                                            else
                                            {
                                                sheet.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- FROM: " + Time_Start_2 + " - to - " + Time_Finish_2 + " (All)";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Is_one_Page)
                                        {
                                            sheet.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " (Page " + lib.Split_Thousand(ViewState["Page"].aS()) + " of " + lib.Split_Thousand(ViewState["Total_Page"].aS()) + ")";
                                        }
                                        else
                                        {
                                            sheet.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " (All)";
                                        }
                                    }
                                }
                                else
                                {
                                    if (Is_one_Page)
                                    {
                                        sheet.Cells[3, 3].Value = "(Page " + ViewState["Page"].ai().SplitThousand + " of " + ViewState["Total_Page"].ai().SplitThousand + ")";
                                    }
                                    else
                                    {
                                        sheet.Cells[3, 3].Value = "(All)";
                                    }
                                }

                                sheet.Cells[3, 3, 3, 9].Merge = true;

                                sheet.Cells[3, 3].Style.Font.Color.SetColor(Color.Blue);
                                sheet.Cells[3, 3].Style.Font.Bold = true;

                                //
                                if (ReportName == "Add_point")
                                {
                                    for (int x1 = 1; x1 <= Column_Name_Array.Length; x1++)
                                    {
                                        //Order
                                        HtmlTableCell TD_1 = lib.Creat_HTH(a.e, "0", "0", "center", "middle", 0, a.e, false, false, a.e);
                                        TR.Cells.Add(TD_1);
                                        TD_1.Controls.Add(lib.Creat_Label_ltr("<span class='B Red'>" + Column_Name_Array[x1 - 1] + "</span>"));

                                        sheet.Cells[5, x1].Value = Column_Name_Array[x1 - 1];

                                        sheet.Cells[5, x1].Style.Font.Color.SetColor(Color.Red);
                                        sheet.Cells[5, x1].Style.Font.Bold = true;
                                        sheet.Cells[5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }
                                else
                                if ((ReportName == "Add_point_by_member") || (ReportName == "Total_Add_point_by_member"))
                                {
                                    for (int x1 = 1; x1 <= Column_Name_Array.Length; x1++)
                                    {
                                        //Order
                                        HtmlTableCell TD_1 = lib.Creat_HTH(a.e, "0", "0", "center", "middle", 0, a.e, false, false, a.e);
                                        TR.Cells.Add(TD_1);
                                        TD_1.Controls.Add(lib.Creat_Label_ltr("<span class='B Red'>" + Column_Name_Array[x1 - 1] + "</span>"));

                                        sheet.Cells[5, x1].Value = Column_Name_Array[x1 - 1];

                                        sheet.Cells[5, x1].Style.Font.Color.SetColor(Color.Red);
                                        sheet.Cells[5, x1].Style.Font.Bold = true;
                                        sheet.Cells[5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }
                                else
                                {
                                    //
                                    for (int x1 = 0; x1 < Total_Column; x1++)
                                    {
                                        ai Column_Name = sql.Data.GetName(x1).aS();

                                        HtmlTableCell TD_2 = lib.Creat_HTD(a.e, "0", "0", "center", "middle", 0, a.e, false, false, a.e);
                                        TR.Cells.Add(TD_2);
                                        TD_2.Controls.Add(lib.Creat_Label_ltr("<span class='B Red'>" + Column_Name.Replace("_", " ") + "</span>"));

                                        //
                                        sheet.Cells[5, 1 + x1].Value = Column_Name;

                                        sheet.Cells[5, 1 + x1].Style.Font.Color.SetColor(Color.Red);
                                        sheet.Cells[5, 1 + x1].Style.Font.Bold = true;
                                        sheet.Cells[5, 1 + x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }
                            }

                            //
                            if (Valid_1)
                            {
                                Order++;

                                //
                                HtmlTableRow TR_2 = new HtmlTableRow();

                                //
                                TR_2 = lib.Creat_HTR(Primary_Key);
                                Report_htbl.Rows.Add(TR_2);

                                if (((Order - 1) % 2) == 0)
                                {
                                    TR_2.BgColor = "#C0C0C0";
                                }

                                #region Add_point
                                if (ReportName == "Add_point")
                                {
                                    object[] Column_Value_Array = new object[0];

                                    if (UserName.ToLower() == "Pos2".ToLower())
                                    {
                                        Column_Value_Array = new object[] { Name + "<br/>" + Card, TenGianHang_OK + "<br/>" + SoHopDong_OK, Money, Point, Receipt, Buy_Time, Add_Time, POS, Cashier };
                                        //Column_Name_Array = new string[] { "Khách hàng", "Shop", "Tiền", "Điểm", "Hóa đơn", "Ngày mua", "Ngày tích", "POS", "Tài khoản" };
                                    }
                                    else
                                    {
                                        Column_Value_Array = new object[] { Primary_Key, Order.aS(), Type, Department, Card, Name, TenGianHang_OK, SoHopDong_OK, Money, Point, Receipt, Cashier, Add_Time, POS, Buy_Time, Add_Point_Manual_String };
                                        Column_Name_Array = new string[] { "ID", "Number", "Type", "Department", "Card", "Name", "Shop", "Shop Code", "Money", "Point", "Receipt", "Add By", "Add Time", "Add At", "Buy Time", "Reason" };
                                    }

                                    //
                                    if ((Is_one_Page) || (Order <= 1000))
                                    {
                                        for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                        {
                                            ai TD_Align = "center";

                                            if ((Column_Name_Array[x1 - 1] == "Money") || (Column_Name_Array[x1 - 1] == "Point"))
                                            {
                                                TD_Align = "right";
                                            }

                                            HtmlTableCell TD_2 = lib.Creat_HTD(a.e, "0", "0", TD_Align, "middle", 0, a.e, false, false, a.e);
                                            TR_2.Cells.Add(TD_2);
                                            TD_2.Controls.Add(lib.Creat_Label_ltr(a.aS(Column_Value_Array[x1 - 1])));
                                        }
                                    }

                                    //
                                    for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                    {
                                        if (x1 == 9)
                                        {
                                            sheet.Cells[Order + 5, x1].Value = lib.Convert_String_To_BigInt(Money_Raw, 0);
                                        }
                                        else
                                            if (x1 == 10)
                                        {
                                            sheet.Cells[Order + 5, x1].Value = lib.Convert_String_To_BigInt(Point_Raw, 0);
                                        }
                                        else
                                        {
                                            sheet.Cells[Order + 5, x1].Value = Column_Value_Array[x1 - 1];
                                        }

                                        //
                                        if ((x1 == 1) || (x1 == 2) || (x1 == 3) || (x1 == 4))
                                        {
                                            sheet.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                            if ((x1 == 9) || (x1 == 10))
                                        {
                                            sheet.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }
                                    }
                                }
                                #endregion

                                #region Add_point_by_member
                                else
                                if (ReportName == "Add_point_by_member")
                                {
                                    object[] Column_Value_Array = new object[0];

                                    if (UserName.ToLower() == "Pos2".ToLower())
                                    {
                                        Column_Value_Array = new object[] { Name + "<br/>" + Card, TenGianHang_OK + "<br/>" + SoHopDong_OK, Money, Point, Receipt, Buy_Time, Add_Time, POS, Cashier };
                                        //Column_Name_Array = new string[] { "Khách hàng", "Shop", "Tiền", "Điểm", "Hóa đơn", "Ngày mua", "Ngày tích", "POS", "Tài khoản" };
                                    }
                                    else
                                    {
                                        Column_Value_Array = new object[] { Primary_Key, Order.aS(), Type, Department, Card, Name, Phone, TenGianHang_OK, SoHopDong_OK, Money, Point, Receipt, Cashier, Add_Time, POS, Buy_Time, Add_Point_Manual_String };
                                        //Column_Name_Array = new string[] { "ID", "Number", "Type", "Department", "Card", "Name", "Phone", "Shop", "Shop Code", "Money", "Point", "Receipt", "Add By", "Add Time", "Add At", "Buy Time", "Reason" };
                                    }

                                    //
                                    if ((Is_one_Page) || (Order <= 1000))
                                    {
                                        for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                        {
                                            ai TD_Align = "center";

                                            if ((Column_Name_Array[x1 - 1] == "Money") || (Column_Name_Array[x1 - 1] == "Point"))
                                            {
                                                TD_Align = "right";
                                            }

                                            HtmlTableCell TD_2 = lib.Creat_HTD(a.e, "0", "0", TD_Align, "middle", 0, a.e, false, false, a.e);
                                            TR_2.Cells.Add(TD_2);
                                            TD_2.Controls.Add(lib.Creat_Label_ltr(a.aS(Column_Value_Array[x1 - 1])));
                                        }
                                    }

                                    //
                                    for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                    {
                                        if (x1 == 10)
                                        {
                                            sheet.Cells[Order + 5, x1].Value = lib.Convert_String_To_BigInt(Money_Raw, 0);
                                        }
                                        else
                                            if (x1 == 11)
                                        {
                                            sheet.Cells[Order + 5, x1].Value = lib.Convert_String_To_BigInt(Point_Raw, 0);
                                        }
                                        else
                                        {
                                            sheet.Cells[Order + 5, x1].Value = Column_Value_Array[x1 - 1];
                                        }

                                        //
                                        if ((x1 == 1) || (x1 == 2) || (x1 == 3) || (x1 == 4) || (x1 == 5) || (x1 == 6) || (x1 == 7))
                                        {
                                            sheet.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                            if ((x1 == 10) || (x1 == 11))
                                        {
                                            sheet.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }
                                    }
                                }
                                #endregion

                                #region Total_Add_point_by_member
                                else
                                if (ReportName == "Total_Add_point_by_member")
                                {
                                    object[] Column_Value_Array = new object[0];

                                    Column_Value_Array = new object[] { Order.aS(), Card, Name, Phone, Money, Point };
                                    //Column_Name_Array = new string[] { "Number", "Card", "Name", "Phone", "Money", "Point" };

                                    //
                                    if ((Is_one_Page) || (Order <= 1000))
                                    {
                                        for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                        {
                                            ai TD_Align = "center";

                                            if ((Column_Name_Array[x1 - 1] == "Money") || (Column_Name_Array[x1 - 1] == "Point"))
                                            {
                                                TD_Align = "right";
                                            }

                                            HtmlTableCell TD_2 = lib.Creat_HTD(a.e, "0", "0", TD_Align, "middle", 0, a.e, false, false, a.e);
                                            TR_2.Cells.Add(TD_2);
                                            TD_2.Controls.Add(lib.Creat_Label_ltr(a.aS(Column_Value_Array[x1 - 1])));
                                        }
                                    }

                                    //
                                    for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                    {
                                        if (x1 == 5)
                                        {
                                            sheet.Cells[Order + 5, x1].Value = lib.Convert_String_To_BigInt(Money_Raw, 0);
                                        }
                                        else
                                            if (x1 == 6)
                                        {
                                            sheet.Cells[Order + 5, x1].Value = lib.Convert_String_To_BigInt(Point_Raw, 0);
                                        }
                                        else
                                        {
                                            sheet.Cells[Order + 5, x1].Value = Column_Value_Array[x1 - 1];
                                        }

                                        //
                                        if ((x1 == 1) || (x1 == 2) || (x1 == 3) || (x1 == 4))
                                        {
                                            sheet.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                            if ((x1 == 5) || (x1 == 6))
                                        {
                                            sheet.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }
                                    }
                                }
                                #endregion

                                else
                                {
                                    //
                                    for (int x1 = 0; x1 < Total_Column; x1++)
                                    {
                                        ai Column_Name = sql.Data.GetName(x1).aS();
                                        ai Data_Content = sql.Data[Column_Name.aS()].aS();
                                        ai Data_Content_Raw = a.e;

                                        //
                                        if (Column_Name == "Number")
                                        {
                                            Data_Content_Raw = Order.aS();
                                            Data_Content = Order.aS();
                                        }
                                        else
                                        if (Column_Name == "Money")
                                        {
                                            Data_Content_Raw = Data_Content;

                                            if ((Data_Content != a.e) && (Data_Content != "0"))
                                            {
                                                Data_Content.SplitThousand += " VND";
                                            }
                                        }
                                        else
                                        if (Column_Name == "Phone")
                                        {
                                            Data_Content_Raw = Data_Content;

                                            if ((Data_Content != a.e) && (Data_Content != "0"))
                                            {
                                                aurl url = new aurl();

                                                url.Remove("Page");
                                                url.Remove("All");

                                                url.Update("Phone", HttpUtility.UrlEncode(Data_Content));

                                                url.Update("Time_Start_1", HttpUtility.UrlEncode("01/01/2000"));
                                                url.Update("Time_Finish_1", HttpUtility.UrlEncode("31/12/2099"));

                                                Data_Content = "<a target='_blank' href='" + url + "'>" + Data_Content + "</a>";
                                            }
                                        }
                                        else
                                        if (Column_Name == "Email")
                                        {
                                            Data_Content_Raw = Data_Content;

                                            if ((Data_Content != a.e) && (Data_Content != "0"))
                                            {
                                                aurl url = new aurl();

                                                url.Remove("Page");
                                                url.Remove("All");

                                                url.Update("Email", HttpUtility.UrlEncode(Data_Content));

                                                url.Update("Time_Start_1", HttpUtility.UrlEncode("01/01/2000"));
                                                url.Update("Time_Finish_1", HttpUtility.UrlEncode("31/12/2020"));

                                                Data_Content = "<a target='_blank' href='" + url + "'>" + Data_Content + "</a>";
                                            }
                                        }
                                        else
                                        if (lib.Check_Exists_In_Array(allPointColumn, Column_Name))
                                        {
                                            Data_Content_Raw = Data_Content;
                                            a.run(Data_Content.SplitThousand);
                                        }

                                        //
                                        if (ReportName == "Analys_transaction")
                                        {
                                            if (Column_Name == "Start_Date")
                                            {
                                                Data_Content -= " 00:00:00";
                                            }
                                        }

                                        if (ReportName == "Member_list")
                                        {
                                            if (Column_Name == "Number")
                                            {
                                                Card = sql.Data["Card"].aS();

                                                Data_Content_Raw = Data_Content;
                                                Data_Content = "<input type='checkbox' name='Switch_Point_cbxl' value='" + Card + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type='radio' name='Switch_Point_rdol' value='" + Card + "'>";
                                            }
                                        }

                                        //
                                        if (ReportName == "FC_History")
                                        {
                                            if (Column_Name == "Hang_Hoa")
                                            {
                                                Data_Content -= ",";
                                                ai Thao_Tac = Data_Content;

                                                if (Thao_Tac == "Kích hoạt thẻ thức ăn")
                                                {
                                                    Thao_Tac = "Kích hoạt";
                                                }
                                                else
                                                if ((Thao_Tac == "Nạp tiền thẻ thức ăn") || (Thao_Tac == "Nạp tiền vào thẻ thức ăn"))
                                                {
                                                    Thao_Tac = "Nạp tiền";
                                                }
                                                else
                                                if (Thao_Tac == "Rút tiền trong thẻ thức ăn")
                                                {
                                                    Thao_Tac = "Rút tiền";
                                                }

                                                //
                                                Data_Content = Thao_Tac;
                                                Data_Content_Raw = Thao_Tac;
                                            }

                                            //
                                            if (Column_Name == "Hinh_Thuc")
                                            {
                                                ai Hinh_Thuc_Thanh_Toan = Data_Content;

                                                if (Hinh_Thuc_Thanh_Toan.ToLower() == "Tiền Việt Nam".ToLower())
                                                {
                                                    Hinh_Thuc_Thanh_Toan = "Tiền mặt";
                                                }
                                                else
                                                if (Hinh_Thuc_Thanh_Toan.ToLower() == "Foodcourt Card".ToLower())
                                                {
                                                    Hinh_Thuc_Thanh_Toan = "Mua đồ ăn";
                                                }
                                                else
                                                        if (Hinh_Thuc_Thanh_Toan.ToLower() == "Voucher Foodcourt".ToLower())
                                                {
                                                    Hinh_Thuc_Thanh_Toan = "Voucher";
                                                }

                                                //
                                                Data_Content = Hinh_Thuc_Thanh_Toan;
                                                Data_Content_Raw = Hinh_Thuc_Thanh_Toan;
                                            }

                                            //
                                            if (Column_Name == "So_Tien")
                                            {
                                                ai Hinh_Thuc_Thanh_Toan = sql.Data["Hinh_Thuc"].aS();

                                                if (Hinh_Thuc_Thanh_Toan.ToLower() == "Foodcourt Card".ToLower())
                                                {
                                                    if (!Data_Content.StartsWith("-"))
                                                    {
                                                        Data_Content = "-" + Data_Content;
                                                    }

                                                    if (!Data_Content_Raw.StartsWith("-"))
                                                    {
                                                        Data_Content_Raw = "-" + Data_Content_Raw;
                                                    }
                                                }
                                            }
                                        }

                                        //
                                        if ((Is_one_Page) || (Order <= 1000))
                                        {
                                            HtmlTableCell TD_22 = lib.Creat_HTD(a.e, "0", "0", "center", "middle", 0, a.e, false, false, a.e);
                                            TR_2.Cells.Add(TD_22);
                                            TD_22.Controls.Add(lib.Creat_Label_ltr(Data_Content));
                                        }

                                        //
                                        sheet.Cells[Order + 5, 1 + x1].Value = Data_Content;

                                        if (Column_Name == "Number")
                                        {
                                            sheet.Cells[Order + 5, 1 + x1].Value = Data_Content_Raw;
                                            sheet.Cells[Order + 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                        if ((Column_Name == "Phone") || (Column_Name == "Email"))
                                        {
                                            sheet.Cells[Order + 5, 1 + x1].Value = Data_Content_Raw;
                                            sheet.Cells[Order + 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                        if ((Column_Name == "Money") || (lib.Check_Exists_In_Array(allPointColumn, Column_Name)))
                                        {
                                            sheet.Cells[Order + 5, 1 + x1].Value = lib.Convert_String_To_BigInt(Data_Content_Raw, 0);
                                            sheet.Cells[Order + 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                        }
                                    }
                                }
                            }

                            //
                            j1++;
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                    }
                    finally
                    {
                        sql.Close();
                    }

                    //
                    if (ReportName == "Add_point")
                    {
                        for (int x1 = 1; x1 <= Column_Name_Array.Length; x1++)
                        {
                            sheet.Column(x1).AutoFit();
                        }
                    }
                    else
                    if ((ReportName == "Add_point_by_member") || (ReportName == "Total_Add_point_by_member"))
                    {
                        for (int x1 = 1; x1 <= Column_Name_Array.Length; x1++)
                        {
                            sheet.Column(x1).AutoFit();
                        }
                    }
                    else
                        for (int x1 = 0; x1 < Total_Column; x1++)
                        {
                            sheet.Column(1 + x1).AutoFit();
                        }

                    //Save
                    excel.Save();

                    if (!Is_one_Page)
                        onPageLoad += " window.location.href = '" + a.url.DomainHttp + "/File/" + foldeName + "/" + File_Info.Name + "';";
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
        //Date_Time_1
        atime Time_Start_1 = Time_Start_1_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish_1 = Time_Finish_1_tbx.Text.ai().aRemove("__/__/____").adate;

        if (!Time_Start_1.Valid)
        {
            Time_Start_1 = ("01/" + a.Time.Month + "/" + a.Time.Year).atime(TypeTime.Date);
            Time_Start_1_tbx.Text = Time_Start_1.FormatedDate;
        }

        if (!Time_Finish_1.Valid)
        {
            Time_Finish_1 = (a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year).atime(TypeTime.Date);
            Time_Finish_1_tbx.Text = Time_Finish_1.FormatedDate;
        }

        //Date_Time_2
        atime Time_Start_2 = Time_Start_2_tbx.Text.ai().aRemove("__/__/____").adate;
        atime Time_Finish_2 = Time_Finish_2_tbx.Text.ai().aRemove("__/__/____").adate;

        if (!Time_Start_2.Valid)
        {
            Time_Start_2 = (a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year).atime(TypeTime.Date);
            Time_Start_2_tbx.Text = Time_Start_2.FormatedDate;
        }

        if (!Time_Finish_2.Valid)
        {
            Time_Finish_2 = (a.Time.Day + "/" + a.Time.Month + "/" + a.Time.Year).atime(TypeTime.Date);
            Time_Finish_2_tbx.Text = Time_Finish_2.FormatedDate;
        }

        aurl url = new aurl();

        url.Remove("Onload");
        url.Remove("Page");
        url.Update("All", all);

        url.Update("Time_Start_1", Time_Start_1.ai().UrlEncode);
        url.Update("Time_Finish_1", Time_Finish_1.ai().UrlEncode);

        url.Update("Time_Start_2", Time_Start_2.ai().UrlEncode);
        url.Update("Time_Finish_2", Time_Finish_2.ai().UrlEncode);

        url.Update("Shop", Shop_tbx.Text.ai().UrlEncode);
        url.Update("Card", Card_tbx.Text.ai().UrlEncode);
        url.Update("Name", Name_tbx.Text.ai().UrlEncode);
        url.Update("Phone", Phone_tbx.Text.ai().UrlEncode);
        url.Update("Email", Email_tbx.Text.ai().UrlEncode);
        url.Update("Address", Address_tbx.Text.ai().UrlEncode);

        url.Update("Add_By_User", Add_By_User_tbx.Text.ai().UrlEncode);

        url.Update("Card_Actived_OR_Disabled", Card_Actived_OR_Disabled_rdol.SelectedValue.abool().ToBit);
        url.Update("RDOL_1", Filter_RDOL_1.SelectedValue);
        url.Update("RDOL_2", Filter_RDOL_2.SelectedValue);

        url.Update("CBXL_1", aselected.CBXL(Filter_CBXL_1, "value"));

        url.Update("ReActive_Card", ReActive_Card_cbx.Checked.abool().ToBit);

        url.Update("Filter_Time_3", Filter_Time_3_tbx.Text.ai().UrlEncode);

        Response.Redirect(url);
    }

    protected void Creat_Sql_Join_Card_FC(int Month, int Year, out ai Sql_Join_HD, out ai Sql_Join_CTHD)
    {
        ai Thang_Nam = a.e;

        //
        if (Month < 10)
        {
            Thang_Nam = "0" + Month.aS() + Year;
        }
        else
        {
            Thang_Nam = Month.aS() + Year;
        }

        //
        Sql_Join_HD = a.e;
        Sql_Join_CTHD = a.e;

        //
        if (crm.Check_Exists_Table_POS("TOPOS_DB", "HoaDon" + Thang_Nam))
        {
            //
            Sql_Join_HD =
                " SELECT MaHD, MaQuay, TenNVBanHang, MaThue FROM HoaDon" + Thang_Nam
                + " WHERE (MaHD IN (SELECT HoaDonID FROM TTT_CTNapTien_HinhThucThanhToan WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card)))))"
                + " UNION"
                ;

            Sql_Join_CTHD =

                " SELECT MaHD, (SELECT MAX(NgayGioQuet) FROM CTHoaDon" + Thang_Nam + " WHERE (MaHD = CTHoaDon_Alias_GroupBy.MaHD) FOR XML PATH('')) as NgayGioQuet, (SELECT TenHH + ', ' FROM CTHoaDon" + Thang_Nam + " WHERE (MaHD = CTHoaDon_Alias_GroupBy.MaHD) FOR XML PATH('')) as TenHH"

                + " FROM CTHoaDon" + Thang_Nam + " CTHoaDon_Alias_GroupBy"

                + " WHERE (MaHD IN (SELECT HoaDonID FROM TTT_CTNapTien_HinhThucThanhToan WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card)))))"

                + " GROUP BY MaHD"

                + " UNION"
                ;
        }
    }
    private void Read_FoodCourtCard_Info(ai MaFoodCourt)
    {
        try
        {
            Money_Current_lbl.Text = a.e;
            Money_Useable_lbl.Text = a.e;
            Money_Withdrawal_Able_lbl.Text = a.e;

            State_lbl.Text = a.e;

            //
            ai SoTien = a.e;
            ai SoTien_Lock = a.e;

            ai SoTien_Co_The_Dung = a.e;
            ai SoTien_Co_The_Rut = a.e;

            ai TrangThai = a.e;
            ai NgayHetHan = a.e;

            //
            ai query =

                " USE TOPOS_DB"

                + " DECLARE @SoTien BIGINT"
                + " DECLARE @SoTien_Lock BIGINT"

                + " DECLARE @TrangThai NVARCHAR(MAX)"
                + " DECLARE @NgayHetHan NVARCHAR(MAX)"

                + " DECLARE @SoTien_Co_The_Rut BIGINT"

                + " DECLARE @CTKichHoatID NVARCHAR(MAX)"

                + " SET @CTKichHoatID = (SELECT TOP 1 CTKichHoatID FROM TTT_CTKichHoat"
                + " WHERE MaFoodCourt = @MaFoodCourt"
                + " ORDER BY NgayKichHoat DESC)"

                //
                + " UPDATE TOPOS_DB.DBO.TTT_DanhMuc"
                + " SET NgayHetHan = CONVERT(DATE, '12-12-2099', 103)"
                + " WHERE (MaFoodCourt = @MaFoodCourt) AND (NgayHetHan IS NULL)"

                + " SELECT @SoTien = SoTien, @SoTien_Lock = SoTien_Lock, @TrangThai = TrangThai, @NgayHetHan = NgayHetHan"
                + " FROM TOPOS_DB.DBO.TTT_DanhMuc"
                + " WHERE (MaFoodCourt = @MaFoodCourt)"

                + " SELECT @SoTien_Co_The_Rut = TongTien"
                + " FROM TOPOS_DB.DBO.TTT_CTKichHoat_NhomThanhToan WHERE ((CTKichHoatID = @CTKichHoatID) AND (MaNhomThanhToan = '001'))"

                + " IF (@SoTien_Co_The_Rut > @SoTien)"
                + " SET @SoTien_Co_The_Rut = @SoTien"

                + " SELECT @SoTien AS SoTien, @SoTien_Lock AS SoTien_Lock, @TrangThai AS TrangThai, CONVERT(DATE, @NgayHetHan, 103) AS NgayHetHan, @SoTien_Co_The_Rut AS SoTien_Co_The_Rut"
                ;

            //Open_Sql_Connection_FC();
            asql sql = query.asql(new nv("MaFoodCourt", MaFoodCourt));
            sql.DataReader();

            try
            {
                if (sql.Data.Read())
                {
                    SoTien = sql.Data["SoTien"].aS();
                    SoTien_Lock = sql.Data["SoTien_Lock"].aS();

                    TrangThai = sql.Data["TrangThai"].aS();
                    NgayHetHan = sql.Data["NgayHetHan"].aS();

                    SoTien_Co_The_Rut = sql.Data["SoTien_Co_The_Rut"].aS();
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

            //
            SoTien -= ".00";
            SoTien_Lock -= ".00";
            SoTien_Co_The_Rut -= ".00";

            if (SoTien_Lock.NoZero)
            {
                TrangThai = "<span style='color: red; font-weight: bold;'>Đã khóa (Số tiền lúc Khóa: " + SoTien_Lock.New.SplitThousand + ")</span>";
            }
            else
            if ((TrangThai == a.e) || (TrangThai == "0"))
            {
                TrangThai = "Chưa kích hoạt";
            }
            else
            {
                TrangThai = "Đã kích hoạt";
            }

            long SoTien_int = SoTien;

            if (SoTien_int > 10000)
            {
                SoTien_Co_The_Dung = (SoTien_int - 10000).aS();
            }
            else
            {
                SoTien_Co_The_Dung = "0";
            }

            //
            a.run(NgayHetHan.Remove_SqlDateTime00AM);

            string[] NgayHetHan_Array = NgayHetHan.Split('/');

            if (NgayHetHan_Array.Length == 3)
            {
                NgayHetHan = NgayHetHan_Array[1] + "/" + NgayHetHan_Array[0] + "/" + NgayHetHan_Array[2];
            }

            //
            a.run(SoTien.SplitThousand);
            a.run(SoTien_Co_The_Dung.SplitThousand);
            a.run(SoTien_Co_The_Rut.SplitThousand);

            //
            Money_Current_lbl.Text = "Tài khoản hiện có: " + SoTien;
            Money_Useable_lbl.Text = " --- Có thể dùng: " + SoTien_Co_The_Dung;
            Money_Withdrawal_Able_lbl.Text = " --- Có thể rút: " + SoTien_Co_The_Rut;

            if (NgayHetHan != a.e)
            {
                State_lbl.Text = "<br/>Trạng thái: " + TrangThai + " --- Hết hạn: " + NgayHetHan;
            }
            else
            {
                State_lbl.Text = "<br/>Trạng thái: " + TrangThai;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void FC_Block_btn_Click(object sender, EventArgs e)
    {
        ai Card = Card_tbx.Text.ai().Get_ValidInput;

        if (Card.Length == 11)
        {
            FC_Block_btn.Enabled = false;
            FC_UnBlock_btn.Enabled = false;

            ai query =

                " UPDATE [TOPOS_DB].[dbo].[TTT_DanhMuc]"
                + " SET SoTien_Lock = SoTien, SoTien = 0"//TrangThai = 0
                + " WHERE ((MaFoodCourt = @MaFoodCourt) AND (SoTien > 0) AND ((SoTien_Lock = 0) OR (SoTien_Lock IS NULL)))"
                ;

            //Open_Sql_Connection_FC();
            query.asql(new nv("MaFoodCourt", Card)).NonQuery();

            Read_FoodCourtCard_Info(Card);

            Message_lbl.Text = "Đã KHÓA thẻ: " + Card;

            FC_UnBlock_btn.Enabled = true;
        }
    }
    protected void FC_UnBlock_btn_Click(object sender, EventArgs e)
    {
        ai Card = Card_tbx.Text.ai().Get_ValidInput;

        if (Card.Length == 11)
        {
            FC_Block_btn.Enabled = false;
            FC_UnBlock_btn.Enabled = false;

            ai query =

                " UPDATE [TOPOS_DB].[dbo].[TTT_DanhMuc]"
                + " SET SoTien = SoTien_Lock, SoTien_Lock = 0"//TrangThai = 1
                + " WHERE ((MaFoodCourt = @MaFoodCourt) AND (SoTien_Lock > 0) AND ((SoTien = 0) OR (SoTien IS NULL)))"
                ;

            //Open_Sql_Connection_FC();
            query.asql(new nv("MaFoodCourt", Card)).NonQuery();

            Read_FoodCourtCard_Info(Card);

            Message_lbl.Text = "Đã MỞ thẻ: " + Card;

            FC_Block_btn.Enabled = true;
        }
    }
    protected void SAP_btn_Click(object sender, EventArgs e)
    {
        ai MaSAP = Ma_SAP_tbx.Text.ai().Get_ValidInput;
        ai SoHopDong = So_Hop_Dong_tbx.Text.ai().Get_ValidInput;

        Ma_SAP_tbx.Enabled = false;
        So_Hop_Dong_tbx.Enabled = false;
        SAP_btn.Enabled = false;

        ai query =

                " USE TOPOS_DB"

                + " DECLARE @MaThue NVARCHAR(MAX)"
                + " DECLARE @TenKhachThue NVARCHAR(MAX)"

                + " SELECT @MaThue = MaThue, @TenKhachThue = TenGianHang FROM TOPOS_DB.dbo.ThueGianHang"
                + " WHERE SoHopDong = @SoHopDong"

                + " IF @MaThue IS NOT NULL"

                + " IF NOT EXISTS (SELECT * FROM SAP WHERE MaSAP = @MaSAP)"
                + " 	INSERT INTO SAP"
                + " 	(MaSAP, MaThue, SoHopDong, TenKhachThue, NgayTao, GhiChu)"
                + " 	VALUES"
                + " 	(@MaSAP, @MaThue, @SoHopDong, @TenKhachThue, GETDATE(), '')"
                + " ELSE"
                + " 	UPDATE SAP"
                + " 	SET MaThue = @MaThue"
                + " 	WHERE MaSAP = @MaSAP"

                ;

        query.asql(new nv("MaSAP", MaSAP), new nv("SoHopDong", SoHopDong)).NonQuery();
        Message_lbl.Text = "Đã add thành công mã SAP: " + MaSAP + " và Số hợp đồng: " + SoHopDong;
    }

    protected bool Check_Object_Is_True(object Object)
    {
        bool Result = false;

        if (Object != null)
        {
            if ((Object.aS().ToLower() == "true") || (Object.aS().ToLower() == "1"))
            {
                Result = true;
            }
        }

        return Result;
    }
    protected void Ajax_ScriptManager_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        Ajax_ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message;
    }

    [WebMethod(enableSession: true)]
    public static string Submit_Switch_Multi_Point(string Card_1_List, string Card_2)
    {
        return crm.Submit_Switch_Multi_Point(Card_1_List, Card_2);
    }
}