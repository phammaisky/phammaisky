using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Drawing;
using System.Data.SqlClient;
using System.Security.AccessControl;
using AjaxControlToolkit;
using _4u4m;
using System.Web.Services;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using OfficeOpenXml;

public partial class Report : System.Web.UI.Page
{
    SqlConnection Sql_Connection;
    SqlCommand Sql_Command;

    string Message = string.Empty;

    string Control_Order = string.Empty;
    int Control_Order_int = 0;

    string URL = string.Empty;

    string Time = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss:fff");
    string On_Page_Load = "Report_Onload();";

    string Control_Call_Postback = string.Empty;

    string Page_Query_String = string.Empty;

    string Primary_Column = string.Empty;

    string[] Point_Column_Name_Array = new string[] { "Point", "Total_Point", "Current_Point", "Point_In_Time", "Redeemed", "Mistake", "Other_Leaves", "All_Leaves", "So_Tien", "Truoc", "Sau" };

    string[] Time_Array = new string[] { "10-14", "14-18", "18-22", "10-22" };

    //OK
    protected void Page_Init(object sender, EventArgs e)
    {
        Sql_Connection = new SqlConnection(Application["Sql_Connection_String_DB"].ToString());

        if (Sql_Connection.State != ConnectionState.Open)
        {
            Sql_Connection.Open();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //UserName
        string UserName = new _4e().Read_UserName();

        string Ho_Va_Ten = string.Empty;
        string Phong_Ban = string.Empty;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        if ((!Duoc_Xem_Bao_Cao) && (!Duoc_Tich_Diem))
        {
            Response.Redirect(new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty)));
        }
        else
        {
            Session["Total_Result_Per_Page_For_Report"] = 1000;

            //
            if (!IsPostBack)
            {
                new _4e().Set_Index_Host(Index_Host_hdf, null);
                PageMethods_Path_hdf.Value = "/Tool/Report.aspx";

                if (Request.QueryString["R"] != null)
                {
                    ViewState["Report_Name_Code"] = Request.QueryString["R"].ToString();
                }

                //
                string Report_Name_Code = new _4e().Object_ToString(ViewState["Report_Name_Code"]);

                if (Report_Name_Code == "Add_point")
                {
                    Filter_RDOL_2.Visible = true;

                    if (Duoc_Xem_Bao_Cao)
                    {
                        Filter_RDOL_2.Items.Add(new ListItem("Tất cả tài khoản", "ALL"));
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
                string Time_Start_1 = new _4e().Determine_Query_String_Text(string.Empty, "Time_Start_1");
                string Time_Finish_1 = new _4e().Determine_Query_String_Text(string.Empty, "Time_Finish_1");

                Time_Start_1 = new _4e().Convert_Date_String_To_String(Time_Start_1, string.Empty);
                Time_Finish_1 = new _4e().Convert_Date_String_To_String(Time_Finish_1, string.Empty);

                //
                if (Time_Start_1 != string.Empty)
                {
                    Time_Start_1_tbx.Text = Time_Start_1;
                }

                //
                if (Time_Finish_1 != string.Empty)
                {
                    Time_Finish_1_tbx.Text = Time_Finish_1;
                }

                //Date_Time_2
                string Time_Start_2 = new _4e().Determine_Query_String_Text(string.Empty, "Time_Start_2");
                string Time_Finish_2 = new _4e().Determine_Query_String_Text(string.Empty, "Time_Finish_2");

                Time_Start_2 = new _4e().Convert_Date_String_To_String(Time_Start_2, string.Empty);
                Time_Finish_2 = new _4e().Convert_Date_String_To_String(Time_Finish_2, string.Empty);

                //
                if (Time_Start_2 != string.Empty)
                {
                    Time_Start_2_tbx.Text = Time_Start_2;
                }

                //
                if (Time_Finish_2 != string.Empty)
                {
                    Time_Finish_2_tbx.Text = Time_Finish_2;
                }

                //
                string Shop = new _4e().Determine_Query_String_Text(string.Empty, "Shop");
                string Card = new _4e().Determine_Query_String_Text(string.Empty, "Card");
                string Name = new _4e().Determine_Query_String_Text(string.Empty, "Name");
                string Phone = new _4e().Determine_Query_String_Text(string.Empty, "Phone");
                string Email = new _4e().Determine_Query_String_Text(string.Empty, "Email");
                string Address = new _4e().Determine_Query_String_Text(string.Empty, "Address");

                string Add_By_User = new _4e().Determine_Query_String_Text(string.Empty, "Add_By_User");

                Shop_tbx.Text = Shop;
                Card_tbx.Text = Card;
                Name_tbx.Text = Name;
                Phone_tbx.Text = Phone;
                Email_tbx.Text = Email;
                Address_tbx.Text = Address;

                Add_By_User_tbx.Text = Add_By_User;

                string Filter_Time_3 = new _4e().Determine_Query_String_Text(string.Empty, "Filter_Time_3");

                if (Filter_Time_3 != string.Empty)
                {
                    Filter_Time_3_tbx.Text = Filter_Time_3;
                }

                //
                Read_Report(sender, e);

                if ((Report_Name_Code == "FC_History") || (Report_Name_Code == "FC_ACC") || (Report_Name_Code == "FC_Block"))
                {
                    if (Card.Length == 11)
                    {
                        Read_FoodCourtCard_Info(Card);
                    }
                }
            }
            else
            {
                Control_Call_Postback = new _4e().Determine_Control_Call_Postback();

                if (!Control_Call_Postback.Contains("Report_"))
                {
                    //Creat_Dynamic_Content_To_Edit(sender, e, true);
                }

                Control_Call_Postback = new _4e().Determine_Control_Call_Postback();
            }
        }
    }

    //OK
    protected void Page_PreRender(object sender, EventArgs e)
    {
        new _4e().Add_ALL_JavaScript_AND_CSS_File_To_Header(Index_Host_hdf.Value);
        new _4e().Add_JavaScript_File_To_Body(Page_Body, Index_Host_hdf.Value + "/index/Java_Script/Ajax/4u4m4e_Ajax.js");
        new _4e().Add_ALL_Index_Host_To_IMG(Index_Host_hdf.Value);

        //
        if (
            (!Control_Call_Postback.Contains("Edit_"))
            && (!Control_Call_Postback.Contains("Expand_"))
            && (!Control_Call_Postback.Contains("Report_"))
            //&& (!Control_Call_Postback.Contains("Download_Excel_All_btn"))
            )
        {
            Read_Dynamic_Content_To_Edit(sender, e);
        }

        //
        if (!IsPostBack)
        {
            Page_Body.Attributes.Add("onload", On_Page_Load);
        }
        else
        {
            new _4e().Run_JavaScript(On_Page_Load);
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        if (Sql_Connection != null)
        {
            Sql_Connection.Close(); Sql_Connection.Dispose();
        }
    }

    protected void Open_Sql_Connection_FC()
    {
        Sql_Connection = new SqlConnection("Data Source='10.15.40.181'; User ID='sa'; Password='123@123a'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;");

        if (Sql_Connection.State != ConnectionState.Open)
        {
            Sql_Connection.Open();
        }
    }

    //OK
    protected void Read_Report(object sender, EventArgs e)
    {
        string UserName = new _4e().Read_UserName();

        if (UserName.ToLower() == "Pos2".ToLower())
        {
            Is_POS_hdf.Value = "1";
        }

        string Ho_Va_Ten = string.Empty;
        string Phong_Ban = string.Empty;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        string Report_Name_Code = new _4e().Object_ToString(ViewState["Report_Name_Code"]);

        ViewState["Filter_Time"] = "0";
        ViewState["Filter_Time_2"] = "0";
        ViewState["Filter_Name"] = "0";
        ViewState["Filter_RDOL"] = "0";
        ViewState["Filter_CBXL"] = "0";

        ViewState["Card_Actived_OR_Disabled"] = "0";

        ViewState["Card_FC"] = "0";
        ViewState["SAP"] = "0";

        //
        if (Report_Name_Code == "Add_point")
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
            if (Report_Name_Code == "Statement")
        {
            Report_lbl.Text = "Report: Statement !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
                if (Report_Name_Code == "Current_point")
        {
            Report_lbl.Text = "Report: Current point !";

            //ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
                    if (Report_Name_Code == "Compare")
        {
            Report_lbl.Text = "Report: Compare !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Time_2"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
                        if (Report_Name_Code == "Max_buying_by_Shop")
        {
            Report_lbl.Text = "Report: Max buying by Shop !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";
        }
        else
                            if (Report_Name_Code == "Buying_list")
        {
            Report_lbl.Text = "Report: Buying list !";

            ViewState["Filter_Time"] = "1";
            ViewState["Filter_Name"] = "1";

            ViewState["Card_Actived_OR_Disabled"] = "1";
        }
        else
                                if (Report_Name_Code == "Inquiry_discount")
        {
            Report_lbl.Text = "Report: Inquiry discount !";

            ViewState["Filter_Time"] = "1";
        }
        else
                                    if (Report_Name_Code == "Analys_transaction")
        {
            Report_lbl.Text = "Report: Analys transaction !";

            ViewState["Filter_Time"] = "1";
        }
        else
        if (Report_Name_Code == "Member_list")
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
        if (Report_Name_Code == "FC_History")
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
        if (Report_Name_Code == "FC_ACC")
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
        if (Report_Name_Code == "FC_Block")
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
        if (Report_Name_Code == "SAP")
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
        if (Report_Name_Code == "Member_buy")
        {
            Report_lbl.Text = "Report: Thành viên Mua sắm !";

            ViewState["Filter_Time"] = "1";

            Filter_Time_3_tr.Visible = true;
            Space_Filter_Time_3_tr.Visible = true;
        }
        else
        if (Report_Name_Code == "Sale")
        {
            Report_lbl.Text = "Report: Sale !";
        }
        else
        if (Report_Name_Code == "Card_not_use")
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
        if (Report_Name_Code == "Add_point")
        {
            if (UserName.ToLower() == "Pos2".ToLower())
            {
                Filter_Name_tr.Visible = false;

                Filter_RDOL_1_tr.Visible = false;
                Filter_CBXL_1_tr.Visible = false;

                Download_Excel_All_btn.Visible = false;
            }
        }

        if (Report_Name_Code == "Member_list")
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

        string Card_Actived_OR_Disabled_SelectedValue = new _4e().Determine_Query_String_Text(string.Empty, "Card_Actived_OR_Disabled");
        string Filter_RDOL_1_SelectedValue = new _4e().Determine_Query_String_Text(string.Empty, "RDOL_1");
        string Filter_RDOL_2_SelectedValue = new _4e().Determine_Query_String_Text(string.Empty, "RDOL_2");

        string Filter_CBXL_1_SelectedValue = new _4e().Determine_Query_String_Text(string.Empty, "CBXL_1");

        new _4e().Re_Choice_RDOL(Card_Actived_OR_Disabled_rdol, Card_Actived_OR_Disabled_SelectedValue);
        new _4e().Re_Choice_RDOL(Filter_RDOL_1, Filter_RDOL_1_SelectedValue);
        new _4e().Re_Choice_RDOL(Filter_RDOL_2, Filter_RDOL_2_SelectedValue);

        new _4e().Re_Choice_CBXL(Filter_CBXL_1, Filter_CBXL_1_SelectedValue);

        string ReActive_Card = new _4e().Determine_Query_String_ID(string.Empty, "ReActive_Card");
        ReActive_Card_cbx.Checked = new _4e().Convert_String_To_Boolean(ReActive_Card);

        ////Creat Control: DLL, v.v.
    }

    //OK
    protected void Read_Dynamic_Content_To_Edit(object sender, EventArgs e)
    {
        string Sql_Query = string.Empty;
        string Sql_Join = string.Empty;

        string Sql_Where = string.Empty;
        string Sql_Order = string.Empty;

        string Column_List_1 = string.Empty;
        string Column_List_2 = string.Empty;

        string Change_Page = string.Empty;

        //
        bool Is_one_Page = !new _4e().Convert_String_To_Boolean(new _4e().Determine_Query_String_ID(string.Empty, "ALL"));

        //
        string Report_Name_Code = new _4e().Object_ToString(ViewState["Report_Name_Code"]);

        //UserName
        string IP = Request.UserHostAddress;
        string UserName = new _4e().Read_UserName();

        string Ho_Va_Ten = string.Empty;
        string Phong_Ban = string.Empty;

        bool Duoc_Xem_Bao_Cao = false;
        bool Duoc_Tich_Diem = false;
        bool Duoc_Tru_Diem = false;
        bool Duoc_Doi_Diem = false;

        new _4e().Read_User_Phan_Quyen(UserName, out Ho_Va_Ten, out Phong_Ban, out Duoc_Xem_Bao_Cao, out Duoc_Tich_Diem, out Duoc_Tru_Diem, out Duoc_Doi_Diem);

        //
        string Computer = string.Empty;
        string Domain = string.Empty;
        string UserName_Services = string.Empty;

        string QuayBan = string.Empty;
        string CaBan = string.Empty;
        string NVBan = string.Empty;

        new _4e().Read_Iam(31, out Computer, out Domain, out UserName_Services, out QuayBan, out CaBan, out NVBan);

        if (QuayBan == string.Empty)
        {
            if (Computer != string.Empty)
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
            if (NVBan == string.Empty)
            {
                if (UserName_Services != string.Empty)
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
        string Add_By_User = Add_By_User_tbx.Text;
        Add_By_User = new _4e().Remove_Danger_String(Add_By_User);
        Add_By_User = new _4e().Remove_Space_String(Add_By_User);

        //
        string Add_At = QuayBan;
        string Add_By = string.Empty;
        string Add_Time = Time;

        if (Add_By_User == string.Empty)
        {
            Add_By = NVBan;

            if ((UserName_Services.ToLower() == "pos") || (UserName_Services.ToLower() == "pos2"))
            {
                string Cashier_Code = new _4e().Read_Cashier_Code(NVBan);

                if (Cashier_Code != string.Empty)
                {
                    Add_By = Cashier_Code;
                }
            }
        }
        else
        {
            Add_By = Add_By_User;
        }

        //
        string[] SQL_Parameters_Array = new string[0];
        string[] SQL_Value_Array = new string[0];

        string Sql_Where_Add_By = string.Empty;

        if (Add_By_User == string.Empty)
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

        SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Add_At");
        SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Add_At);

        SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Add_By");
        SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Add_By);

        //Date_Time_1
        string Time_Start_1 = Time_Start_1_tbx.Text.Replace("__/__/____", string.Empty);
        string Time_Finish_1 = Time_Finish_1_tbx.Text.Replace("__/__/____", string.Empty);

        Time_Start_1 = new _4e().Convert_Date_String_To_String(Time_Start_1, string.Empty);
        Time_Finish_1 = new _4e().Convert_Date_String_To_String(Time_Finish_1, string.Empty);

        //
        if (Time_Start_1 == string.Empty)
        {
            Time_Start_1 = "01/" + DateTime.Now.Month + "/" + DateTime.Now.Year;//"01/01/2000";// 
            Time_Start_1_tbx.Text = Time_Start_1;
        }

        //CONVERT(DATETIME, @Time_Start_1_Temp, 103)
        SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Time_Start_1_Temp");
        SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Time_Start_1);

        //
        if (Time_Finish_1 == string.Empty)
        {
            Time_Finish_1 = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Finish_1_tbx.Text = Time_Finish_1;
        }

        //
        SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Time_Finish_1_Temp");
        SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Time_Finish_1);

        //Date_Time_2
        string Time_Start_2 = Time_Start_2_tbx.Text.Replace("__/__/____", string.Empty);
        string Time_Finish_2 = Time_Finish_2_tbx.Text.Replace("__/__/____", string.Empty);

        Time_Start_2 = new _4e().Convert_Date_String_To_String(Time_Start_2, string.Empty);
        Time_Finish_2 = new _4e().Convert_Date_String_To_String(Time_Finish_2, string.Empty);

        //
        if (Time_Start_2 == string.Empty)
        {
            Time_Start_2 = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Start_2_tbx.Text = Time_Start_2;
        }

        //CONVERT(DATETIME, @Time_Start_2_Temp, 103)
        SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Time_Start_2_Temp");
        SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Time_Start_2);

        //
        if (Time_Finish_2 == string.Empty)
        {
            Time_Finish_2 = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Finish_2_tbx.Text = Time_Finish_2;
        }

        //
        SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Time_Finish_2_Temp");
        SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Time_Finish_2);


        //Time_Start_1
        //Time_Finish_1

        string[] Time_Start_1_Array = Time_Start_1.Split('/');
        string[] Time_Finish_1_Array = Time_Finish_1.Split('/');

        string Sql_Join_HD = string.Empty;
        string Sql_Join_CTHD = string.Empty;

        if ((Time_Start_1_Array.Length == 3) && (Time_Finish_1_Array.Length == 3))
        {
            int Month_Start = new _4e().Convert_String_To_Int(Time_Start_1_Array[1], 1);
            int Year_Start = new _4e().Convert_String_To_Int(Time_Start_1_Array[2], 2009);

            int Month_Finish = new _4e().Convert_String_To_Int(Time_Finish_1_Array[1], DateTime.Now.Month);
            int Year_Finish = new _4e().Convert_String_To_Int(Time_Finish_1_Array[2], DateTime.Now.Year);

            //
            if (Year_Start == Year_Finish)
            {
                for (int i2 = Month_Start; i2 <= Month_Finish; i2++)
                {
                    string Sql_Join_HD_Temp = string.Empty;
                    string Sql_Join_CTHD_Temp = string.Empty;

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
                            string Sql_Join_HD_Temp = string.Empty;
                            string Sql_Join_CTHD_Temp = string.Empty;

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
                            string Sql_Join_HD_Temp = string.Empty;
                            string Sql_Join_CTHD_Temp = string.Empty;

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
                            string Sql_Join_HD_Temp = string.Empty;
                            string Sql_Join_CTHD_Temp = string.Empty;

                            Creat_Sql_Join_Card_FC(i2, i1, out Sql_Join_HD_Temp, out Sql_Join_CTHD_Temp);

                            Sql_Join_HD += Sql_Join_HD_Temp;
                            Sql_Join_CTHD += Sql_Join_CTHD_Temp;
                        }
                    }
                }
            }
        }

        //
        Sql_Join_HD = new _4e().Remove_String_Last(Sql_Join_HD, "UNION");
        Sql_Join_CTHD = new _4e().Remove_String_Last(Sql_Join_CTHD, "UNION");

        //
        string Shop = Shop_tbx.Text;
        Shop = new _4e().Remove_Danger_String(Shop);
        Shop = new _4e().Remove_Space_String(Shop);

        //
        string Card = Card_tbx.Text;
        Card = new _4e().Remove_Danger_String(Card);
        Card = new _4e().Remove_Space_String(Card);

        string Name = Name_tbx.Text;
        Name = new _4e().Remove_Danger_String(Name);
        Name = new _4e().Remove_Space_String(Name);

        string Phone = Phone_tbx.Text;
        Phone = new _4e().Remove_Danger_String(Phone);
        Phone = new _4e().Remove_Space_String(Phone);

        Phone = Phone.Replace("-", string.Empty).Replace(".", string.Empty);
        Phone_tbx.Text = Phone;

        string Email = Email_tbx.Text;
        Email = new _4e().Remove_Danger_String(Email);
        Email = new _4e().Remove_Space_String(Email);

        string Address = Address_tbx.Text;
        Address = new _4e().Remove_Danger_String(Address);
        Address = new _4e().Remove_Space_String(Address);

        string Sql_Where_Shop = string.Empty;
        string Sql_Where_Card = string.Empty;
        string Sql_Where_Name = string.Empty;
        string Sql_Where_Phone = string.Empty;
        string Sql_Where_Email = string.Empty;
        string Sql_Where_Address = string.Empty;

        string Sql_Query_ReActive_Card = string.Empty;

        string Sql_Where_Card_FC = string.Empty;

        if (Shop != string.Empty)
        {
            SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Shop");
            SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Shop);

            if (Report_Name_Code == "Add_point")
            {
                Sql_Where_Shop = " AND ((Brand_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%') OR (Gian_Hang_Alias.TenGianHang COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%'))";
            }
            else
            {
                Sql_Where_Shop = " AND (Brand_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Shop) + '%')";
            }
        }

        if (Card != string.Empty)
        {
            SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Card");
            SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Card);

            Sql_Where_Card = " AND (Member_Alias.Mem_Card COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Card) + '%')";


            if (Card_Actived_OR_Disabled_rdol.SelectedValue == "")
            {
                ReActive_Card_cbx.Checked = false;
            }
            else
                if (ReActive_Card_cbx.Checked)
            {
                Card = Card.Replace("-", string.Empty);

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
                        + "     DEALLOCATE MEM_SEQ_2_List"

                        + " END"
                        ;

                    Sql_Query_ReActive_Card = new _4e().Check_Sql_Query(Sql_Query_ReActive_Card);
                    Sql_Command = new SqlCommand(Sql_Query_ReActive_Card, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.Add("@Add_Time_Temp", Add_Time);
                    Sql_Command.ExecuteNonQuery();

                    //
                    ReActive_Card_cbx.Checked = false;
                }
            }
        }

        //
        if (Name != string.Empty)
        {
            SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Name");
            SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Name);

            Sql_Where_Name = " AND (Member_Alias.Mem_Nm COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Name) + '%')";
        }

        if (Phone != string.Empty)
        {
            SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Phone");
            SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Phone);

            Sql_Where_Phone = " AND (REPLACE(Member_Alias.MOBILE_NO, '-', '') COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Phone) + '%')";
        }

        if (Address != string.Empty)
        {
            SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Address");
            SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Address);

            Sql_Where_Address = " AND (Member_Alias.MEM_ADDR COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Address) + '%')";
        }

        if (Email != string.Empty)
        {
            SQL_Parameters_Array = new _4e().Add_Value_To_Array_String(SQL_Parameters_Array, "@Email");
            SQL_Value_Array = new _4e().Add_Value_To_Array_String(SQL_Value_Array, Email);

            Sql_Where_Email = " AND (Member_Alias.E_Mail COLLATE SQL_Latin1_General_CP1_CI_AS LIKE '%' + CONVERT(NVARCHAR(MAX), @Email) + '%')";
        }

        //
        Sql_Order = "Mem_Nm, REG_DT DESC, MOD_DT DESC";

        //
        string Sql_Where_No_Name = string.Empty;
        string Sql_Where_No_ID = string.Empty;
        string Sql_Where_Duplicate_Phone = string.Empty;
        string Sql_Where_Duplicate_Email = string.Empty;

        //
        string CBXL_Selected_Value = new _4e().Determine_CBXL_Selected(Filter_CBXL_1, "value");

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
            Sql_Order = "MOBILE_NO, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Phone = " AND (MOBILE_NO IN (SELECT MOBILE_NO FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10)) GROUP BY MOBILE_NO HAVING (COUNT(MOBILE_NO) > 1)))";
        }
        else
            if (CBXL_Selected_Value.Contains("#51#"))
        {
            Sql_Order = "MOBILE_NO, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Phone = " AND (MOBILE_NO IN (SELECT MOBILE_NO FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10)) GROUP BY MOBILE_NO HAVING (COUNT(MOBILE_NO) = 1)))";
        }

        //
        if (CBXL_Selected_Value.Contains("#6#"))
        {
            Sql_Order = "E_MAIL, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Email = " AND (E_MAIL IN (SELECT E_MAIL FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (E_MAIL IS NOT NULL) AND (E_MAIL NOT LIKE '') AND (LEN(E_MAIL) = DATALENGTH(E_MAIL))) GROUP BY E_MAIL HAVING (COUNT(E_MAIL) > 1)))";
        }
        else
            if (CBXL_Selected_Value.Contains("#61#"))
        {
            Sql_Order = "E_MAIL, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Email = " AND (E_MAIL IN (SELECT E_MAIL FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (E_MAIL IS NOT NULL) AND (E_MAIL NOT LIKE '') AND (LEN(E_MAIL) = DATALENGTH(E_MAIL))) GROUP BY E_MAIL HAVING (COUNT(E_MAIL) = 1)))";
        }

        //
        if ((CBXL_Selected_Value.Contains("#5#")) && (CBXL_Selected_Value.Contains("#6#")))
        {
            Sql_Order = "MOBILE_NO, E_MAIL, REG_DT DESC, MOD_DT DESC";
            Sql_Where_Duplicate_Phone =
                " AND ("
                + " (MOBILE_NO IN (SELECT MOBILE_NO FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (MOBILE_NO IS NOT NULL) AND (MOBILE_NO NOT LIKE '') AND (LEN(MOBILE_NO) = DATALENGTH(MOBILE_NO)) AND (LEN(Member_Alias.MOBILE_NO) >= 10)) GROUP BY MOBILE_NO HAVING (COUNT(MOBILE_NO) > 1)))"
                + " OR (E_MAIL IN (SELECT E_MAIL FROM GARDEN_CRM.DBO.T_MEM_MST WHERE ((Mem_Card NOT LIKE '-%') AND (E_MAIL IS NOT NULL) AND (E_MAIL NOT LIKE '') AND (LEN(E_MAIL) = DATALENGTH(E_MAIL))) GROUP BY E_MAIL HAVING (COUNT(E_MAIL) > 1)))"
                + ")"
                ;

            Sql_Where_Duplicate_Email = string.Empty;
        }

        string Sql_JOIN_Card_Actived_OR_Disabled = " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ";

        if (Card_Actived_OR_Disabled_rdol.SelectedValue == "0")
        {
            Sql_JOIN_Card_Actived_OR_Disabled = " ON Point_Alias.Mem_SEQ_Old = Member_Alias.Mem_SEQ";
        }


        //Count
        Sql_Query =
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
        string Data_Table_Hoa_Don = string.Empty;

        int Month = new _4e().Convert_Date_String_To_Date_Time(Time_Start_1, DateTime.Now).Month;
        int Year = new _4e().Convert_Date_String_To_Date_Time(Time_Start_1, DateTime.Now).Year;

        //if (Report_Name_Code == "Analys_transaction")
        {
            if (Month <= 9)
            {
                Data_Table_Hoa_Don = "HoaDon0" + Month.ToString() + Year.ToString();
            }
            else
            {
                Data_Table_Hoa_Don = "HoaDon" + Month.ToString() + Year.ToString();
            }
        }

        //Sql_Query

        //
        if ((Report_Name_Code == "Member_buy") && (new _4e().Determine_Query_String_Text(string.Empty, "Onload") != "1"))
        {
            string Filter_Time_3 = Filter_Time_3_tbx.Text;

            Filter_Time_3 = Filter_Time_3.Replace(" ", string.Empty).Replace(";", ",").Replace(".", ",");

            string[] Filter_Time_3_Array_ALL = Filter_Time_3.Split(',');

            string[] Time_Array_Temp = new string[0];

            for (int i1 = 0; i1 < Filter_Time_3_Array_ALL.Length; i1++)
            {
                if (Filter_Time_3_Array_ALL[i1].Split('-').Length == 2)
                {
                    Time_Array_Temp = new _4e().Add_Value_To_Array_String(Time_Array_Temp, Filter_Time_3_Array_ALL[i1]);
                }
            }

            if (Time_Array_Temp.Length > 0)
            {
                Time_Array = Time_Array_Temp;
            }

            //Excel

            //
            string Random_Folder = Guid.NewGuid().ToString();
            string Random_Folder_Path = Server.MapPath("~/File/" + Random_Folder + "/");

            new _4e().Creat_Directory(Random_Folder_Path);

            //
            string File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                    + ".xlsx";

            if (Filter_Time_tr.Visible)
            {
                File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                    + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                    + ".xlsx";
            }

            //
            string Folder_Drive = "D:";

            if (Request.IsLocal)
            {
                Folder_Drive = "D:";
            }

            //
            FileInfo File_Info = new FileInfo(Folder_Drive + @"\Websites\Garden\File\" + Random_Folder + "\\" + File_Name);
            ExcelPackage Excel_Package = new ExcelPackage(File_Info);

            //+" AND (DATENAME(dw, Point_Alias.MOD_DT) LIKE 'Monday')"
            string[] Day_Array = new string[] { "All Day", "Monday + Tuesday + Wednesday", "Thursday", "Friday", "Saturday + Sunday" };

            for (int y1 = 0; y1 < Day_Array.Length; y1++)
            {
                string Sql_Where_Day = string.Empty;

                if (Day_Array[y1] != "All Day")
                {
                    if (Day_Array[y1] == "Monday + Tuesday + Wednesday")
                    {
                        Sql_Where_Day = " AND (DATENAME(dw, Point_Alias.MOD_DT) IN ('Monday', 'Tuesday', 'Wednesday'))";
                    }
                    else
                    if (Day_Array[y1] == "Saturday + Sunday")
                    {
                        Sql_Where_Day = " AND (DATENAME(dw, Point_Alias.MOD_DT) IN ('Saturday', 'Sunday'))";
                    }
                    else
                    {
                        Sql_Where_Day = " AND (DATENAME(dw, Point_Alias.MOD_DT) LIKE '" + Day_Array[y1] + "')";
                    }
                }

                var Work_sheets = Excel_Package.Workbook.Worksheets.Add(Day_Array[y1]);

                //
                Work_sheets.Cells[1, 3].Value = Report_lbl.Text;
                Work_sheets.Cells[1, 3, 1, 9].Merge = true;

                Work_sheets.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[1, 3].Style.Font.Bold = true;
                Work_sheets.Cells[1, 3].Style.Font.Size = 20;

                //
                if (Filter_Time_tr.Visible)
                {
                    Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " (" + Day_Array[y1] + ")";
                }

                Work_sheets.Cells[3, 3, 3, 9].Merge = true;

                Work_sheets.Cells[3, 3].Style.Font.Color.SetColor(Color.Blue);
                Work_sheets.Cells[3, 3].Style.Font.Bold = true;

                //
                int Row = 7;

                Row = Row + 2;
                Work_sheets.Cells[Row, 1].Value = "Mua sắm";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 2;
                Work_sheets.Cells[Row, 1].Value = "Nam";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "Nữ";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 2;
                Work_sheets.Cells[Row, 1].Value = "Việt Nam";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "Nước ngoài";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 2;
                Work_sheets.Cells[Row, 1].Value = "Tuổi < 18";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "18--25";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "25--30";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "30--40";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "40--50";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                Row = Row + 1;
                Work_sheets.Cells[Row, 1].Value = "> 50";
                Work_sheets.Cells[Row, 1].Style.Font.Bold = true;

                //
                for (int i1 = 0; i1 < Time_Array.Length; i1++)
                {
                    string Time_one = Time_Array[i1];
                    string[] Time_one_Array = Time_one.Split('-');

                    if (Time_one_Array.Length == 2)
                    {
                        int HOUR_Start = new _4e().Convert_String_To_Int(Time_one_Array[0], 0);
                        int HOUR_Finish = new _4e().Convert_String_To_Int(Time_one_Array[1], 0);

                        if ((HOUR_Start != 0) && (HOUR_Finish != 0))
                        {
                            //
                            Work_sheets.Cells[6, i1 * 3 + 1, 6, i1 * 3 + 3].Merge = true;
                            Work_sheets.Cells[6, i1 * 3 + 1].Style.Font.Color.SetColor(Color.Red);

                            Work_sheets.Cells[6, i1 * 3 + 1].Value = HOUR_Start.ToString() + "h - " + HOUR_Finish.ToString() + "h";
                            Work_sheets.Cells[6, i1 * 3 + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Work_sheets.Cells[6, i1 * 3 + 1].Style.Font.Bold = true;

                            Work_sheets.Cells[7, i1 * 3 + 2].Value = "Thành viên";
                            Work_sheets.Cells[7, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Work_sheets.Cells[7, i1 * 3 + 2].Style.Font.Bold = true;

                            Work_sheets.Cells[7, i1 * 3 + 3].Value = "$$$";
                            Work_sheets.Cells[7, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            Work_sheets.Cells[7, i1 * 3 + 3].Style.Font.Bold = true;

                            //
                            Sql_Query =
                                "DECLARE @Time_Start_1 NVARCHAR(MAX)"
                                + " SET @Time_Start_1 = CONVERT(DATETIME, @Time_Start_1_Temp, 103) + ' 00:00:00.000'"
                                + " DECLARE @Time_Finish_1 NVARCHAR(MAX)"
                                + " SET @Time_Finish_1 = CONVERT(DATETIME, @Time_Finish_1_Temp, 103) + ' 23:59:59.999'"

                                + " DECLARE @Total_Per_Hour NVARCHAR(MAX)"
                                + " DECLARE @Shop_Max NVARCHAR(MAX)"
                                + " DECLARE @Shop_Max_Mem NVARCHAR(MAX)"

                                + " DECLARE @Man NVARCHAR(MAX)"
                                + " DECLARE @Woman NVARCHAR(MAX)"

                                + " DECLARE @Vietnam NVARCHAR(MAX)"
                                + " DECLARE @International NVARCHAR(MAX)"

                                + " DECLARE @Age_18 NVARCHAR(MAX)"
                                + " DECLARE @Age_18_25 NVARCHAR(MAX)"
                                + " DECLARE @Age_25_30 NVARCHAR(MAX)"
                                + " DECLARE @Age_30_40 NVARCHAR(MAX)"
                                + " DECLARE @Age_40_50 NVARCHAR(MAX)"
                                + " DECLARE @Age_50 NVARCHAR(MAX)"

                                //
                                + " DECLARE @Total_Per_Hour_Money NVARCHAR(MAX)"
                                + " DECLARE @Shop_Max_Money NVARCHAR(MAX)"
                                + " DECLARE @Shop_Max_Mem_Money NVARCHAR(MAX)"

                                + " DECLARE @Man_Money NVARCHAR(MAX)"
                                + " DECLARE @Woman_Money NVARCHAR(MAX)"

                                + " DECLARE @Vietnam_Money NVARCHAR(MAX)"
                                + " DECLARE @International_Money NVARCHAR(MAX)"

                                + " DECLARE @Age_18_Money NVARCHAR(MAX)"
                                + " DECLARE @Age_18_25_Money NVARCHAR(MAX)"
                                + " DECLARE @Age_25_30_Money NVARCHAR(MAX)"
                                + " DECLARE @Age_30_40_Money NVARCHAR(MAX)"
                                + " DECLARE @Age_40_50_Money NVARCHAR(MAX)"
                                + " DECLARE @Age_50_Money NVARCHAR(MAX)"

                                //Total_Per_Hour
                                + " SELECT @Total_Per_Hour = COUNT(*), @Total_Per_Hour_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + Sql_Where_Day

                                //Shop_Max
                                + " SELECT TOP 1 @Shop_Max = BRAND_NM, @Shop_Max_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST Shop_Alias"
                                + " ON Sale_Alias.BRD_CD = Shop_Alias.BRD_CD"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + Sql_Where_Day

                                + " GROUP BY BRAND_NM"
                                + " ORDER BY SUM(Sale_Alias.PAY_AMT) DESC"

                                //Shop_Max_Mem
                                + " SELECT TOP 1 @Shop_Max_Mem = BRAND_NM, @Shop_Max_Mem_Money = COUNT(*)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST Shop_Alias"
                                + " ON Sale_Alias.BRD_CD = Shop_Alias.BRD_CD"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + Sql_Where_Day

                                + " GROUP BY BRAND_NM"
                                + " ORDER BY COUNT(*) DESC"

                                //Man
                                + " SELECT @Man = COUNT(*), @Man_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (Member_Alias.SEX_CD LIKE 'M')"
                                + Sql_Where_Day

                                //Woman
                                + " SELECT @Woman = COUNT(*), @Woman_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (Member_Alias.SEX_CD LIKE 'F')"
                                + Sql_Where_Day

                                //Vietnam
                                + " SELECT @Vietnam = COUNT(*), @Vietnam_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (Member_Alias.NAT_CD LIKE 'NAT001')"
                                + Sql_Where_Day

                                //International
                                + " SELECT @International = COUNT(*), @International_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (Member_Alias.NAT_CD NOT LIKE 'NAT001')"
                                + Sql_Where_Day

                                //Age_18
                                + " SELECT @Age_18 = COUNT(*), @Age_18_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) < 18)"
                                + Sql_Where_Day

                                //Age_18_25
                                + " SELECT @Age_18_25 = COUNT(*), @Age_18_25_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) >= 18) AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) < 25)"
                                + Sql_Where_Day

                                //Age_25_30
                                + " SELECT @Age_25_30 = COUNT(*), @Age_25_30_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) >= 25) AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) < 30)"
                                + Sql_Where_Day

                                //Age_30_40
                                + " SELECT @Age_30_40 = COUNT(*), @Age_30_40_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) >= 30) AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) < 40)"
                                + Sql_Where_Day

                                //Age_40_50
                                + " SELECT @Age_40_50 = COUNT(*), @Age_40_50_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) >= 40) AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) < 50)"
                                + Sql_Where_Day

                                //Age_50
                                + " SELECT @Age_50 = COUNT(*), @Age_50_Money = SUM(Sale_Alias.PAY_AMT)"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " JOIN GARDEN_CRM.DBO.T_MEM_MST Member_Alias"
                                + " ON Point_Alias.Mem_SEQ = Member_Alias.Mem_SEQ"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + " AND (YEAR(GETDATE()) - YEAR(Member_Alias.Mem_birthday) >= 50)"
                                + Sql_Where_Day

                                //Final
                                + " SELECT @Total_Per_Hour AS Total_Per_Hour, @Shop_Max AS Shop_Max, @Shop_Max_Mem AS Shop_Max_Mem, @Man AS Man, @Woman AS Woman, @Vietnam AS Vietnam, @International AS International,"
                                + " @Age_18 AS Age_18, @Age_18_25 AS Age_18_25, @Age_25_30 AS Age_25_30, @Age_30_40 AS Age_30_40, @Age_40_50 AS Age_40_50, @Age_50 AS Age_50,"

                                + " @Total_Per_Hour_Money AS Total_Per_Hour_Money, @Shop_Max_Money AS Shop_Max_Money, @Shop_Max_Mem_Money AS Shop_Max_Mem_Money, @Man_Money AS Man_Money, @Woman_Money AS Woman_Money, @Vietnam_Money AS Vietnam_Money, @International_Money AS International_Money,"
                                + " @Age_18_Money AS Age_18_Money, @Age_18_25_Money AS Age_18_25_Money, @Age_25_30_Money AS Age_25_30_Money, @Age_30_40_Money AS Age_30_40_Money, @Age_40_50_Money AS Age_40_50_Money, @Age_50_Money AS Age_50_Money"
                                ;

                            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                            Sql_Command.Parameters.Add("@Time_Start_1_Temp", Time_Start_1);
                            Sql_Command.Parameters.Add("@Time_Finish_1_Temp", Time_Finish_1);

                            Sql_Command.Parameters.Add("@HOUR_Start", HOUR_Start.ToString());
                            Sql_Command.Parameters.Add("@HOUR_Finish", HOUR_Finish.ToString());

                            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

                            string Total_Per_Hour = string.Empty;

                            string Shop_Max = string.Empty;
                            string Shop_Max_Mem = string.Empty;

                            string Man = string.Empty;
                            string Woman = string.Empty;

                            string Vietnam = string.Empty;
                            string International = string.Empty;

                            string Age_18 = string.Empty;
                            string Age_18_25 = string.Empty;
                            string Age_25_30 = string.Empty;
                            string Age_30_40 = string.Empty;
                            string Age_40_50 = string.Empty;
                            string Age_50 = string.Empty;

                            //
                            string Total_Per_Hour_Money = string.Empty;

                            string Shop_Max_Money = string.Empty;
                            string Shop_Max_Mem_Money = string.Empty;

                            string Man_Money = string.Empty;
                            string Woman_Money = string.Empty;

                            string Vietnam_Money = string.Empty;
                            string International_Money = string.Empty;

                            string Age_18_Money = string.Empty;
                            string Age_18_25_Money = string.Empty;
                            string Age_25_30_Money = string.Empty;
                            string Age_30_40_Money = string.Empty;
                            string Age_40_50_Money = string.Empty;
                            string Age_50_Money = string.Empty;

                            try
                            {
                                if (Sql_Data_Reader.Read())
                                {
                                    Total_Per_Hour = Sql_Data_Reader["Total_Per_Hour"].ToString();

                                    Shop_Max = Sql_Data_Reader["Shop_Max"].ToString();
                                    Shop_Max_Mem = Sql_Data_Reader["Shop_Max_Mem"].ToString();

                                    Man = Sql_Data_Reader["Man"].ToString();
                                    Woman = Sql_Data_Reader["Woman"].ToString();

                                    Vietnam = Sql_Data_Reader["Vietnam"].ToString();
                                    International = Sql_Data_Reader["International"].ToString();

                                    Age_18 = Sql_Data_Reader["Age_18"].ToString();
                                    Age_18_25 = Sql_Data_Reader["Age_18_25"].ToString();
                                    Age_25_30 = Sql_Data_Reader["Age_25_30"].ToString();
                                    Age_30_40 = Sql_Data_Reader["Age_30_40"].ToString();
                                    Age_40_50 = Sql_Data_Reader["Age_40_50"].ToString();
                                    Age_50 = Sql_Data_Reader["Age_50"].ToString();

                                    //
                                    Total_Per_Hour_Money = Sql_Data_Reader["Total_Per_Hour_Money"].ToString();

                                    Shop_Max_Money = Sql_Data_Reader["Shop_Max_Money"].ToString();
                                    Shop_Max_Mem_Money = Sql_Data_Reader["Shop_Max_Mem_Money"].ToString();

                                    Man_Money = Sql_Data_Reader["Man_Money"].ToString();
                                    Woman_Money = Sql_Data_Reader["Woman_Money"].ToString();

                                    Vietnam_Money = Sql_Data_Reader["Vietnam_Money"].ToString();
                                    International_Money = Sql_Data_Reader["International_Money"].ToString();

                                    Age_18_Money = Sql_Data_Reader["Age_18_Money"].ToString();
                                    Age_18_25_Money = Sql_Data_Reader["Age_18_25_Money"].ToString();
                                    Age_25_30_Money = Sql_Data_Reader["Age_25_30_Money"].ToString();
                                    Age_30_40_Money = Sql_Data_Reader["Age_30_40_Money"].ToString();
                                    Age_40_50_Money = Sql_Data_Reader["Age_40_50_Money"].ToString();
                                    Age_50_Money = Sql_Data_Reader["Age_50_Money"].ToString();
                                }
                            }
                            catch (SqlException Sql_Exception)
                            {
                            }

                            if (!Sql_Data_Reader.IsClosed)
                            {
                                Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
                            }

                            //
                            Row = 7;

                            //
                            Row = Row + 2;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Total_Per_Hour, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Total_Per_Hour_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 2;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Man, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Man_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Woman, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Woman_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 2;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Vietnam, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Vietnam_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(International, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(International_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 2;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Age_18, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Age_18_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Age_18_25, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Age_18_25_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Age_25_30, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Age_25_30_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Age_30_40, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Age_30_40_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Age_40_50, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Age_40_50_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //
                            Row = Row + 1;
                            Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Age_50, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Age_50_Money, 0);
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";

                            //Shop_Max

                            Sql_Query =
                                "DECLARE @Time_Start_1 NVARCHAR(MAX)"
                                + " SET @Time_Start_1 = CONVERT(DATETIME, @Time_Start_1_Temp, 103) + ' 00:00:00.000'"
                                + " DECLARE @Time_Finish_1 NVARCHAR(MAX)"
                                + " SET @Time_Finish_1 = CONVERT(DATETIME, @Time_Finish_1_Temp, 103) + ' 23:59:59.999'"

                                + " SELECT (ISNULL(Gian_Hang_Alias.TenGianHang COLLATE SQL_Latin1_General_CP1_CI_AS, '') + ISNULL(BRAND_NM COLLATE SQL_Latin1_General_CP1_CI_AS, '')) AS Shop_Max, SUM(Sale_Alias.PAY_AMT) AS Shop_Max_Money, COUNT(*) AS Shop_Max_Mem"
                                + " FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO AS Point_Alias"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_SALE_INFO Sale_Alias"
                                + " ON Point_Alias.SALE_SEQ = Sale_Alias.SALE_SEQ"

                                + " LEFT JOIN GARDEN_CRM.DBO.T_BRAND_MST Shop_Alias"
                                + " ON Sale_Alias.BRD_CD = Shop_Alias.BRD_CD"

                                //AA-StarFitness
                                + " LEFT JOIN TOPOS_DB.DBO.ThueGianHang AS Gian_Hang_Alias"
                                + " ON Sale_Alias.MaThue LIKE Gian_Hang_Alias.MaThue"

                                + " WHERE (Point_Alias.MOD_DT >= @Time_Start_1) AND (Point_Alias.MOD_DT <= @Time_Finish_1)"
                                + " AND (DATEPART(HOUR, Point_Alias.MOD_DT) >= @HOUR_Start) AND (DATEPART(HOUR, Point_Alias.MOD_DT) <= @HOUR_Finish)"
                                + " AND ((Sale_Alias.MaThue IS NULL) OR (Sale_Alias.MaThue NOT LIKE 'AA-StarFitness'))"
                                + Sql_Where_Day

                                + " GROUP BY (ISNULL(Gian_Hang_Alias.TenGianHang COLLATE SQL_Latin1_General_CP1_CI_AS, '') + ISNULL(BRAND_NM COLLATE SQL_Latin1_General_CP1_CI_AS, ''))"
                                + " ORDER BY SUM(Sale_Alias.PAY_AMT) DESC"
                                ;

                            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                            Sql_Command.Parameters.Add("@Time_Start_1_Temp", Time_Start_1);
                            Sql_Command.Parameters.Add("@Time_Finish_1_Temp", Time_Finish_1);

                            Sql_Command.Parameters.Add("@HOUR_Start", HOUR_Start.ToString());
                            Sql_Command.Parameters.Add("@HOUR_Finish", HOUR_Finish.ToString());

                            Sql_Data_Reader = Sql_Command.ExecuteReader();

                            Row = Row + 1;

                            try
                            {
                                while (Sql_Data_Reader.Read())
                                {
                                    Shop_Max = Sql_Data_Reader["Shop_Max"].ToString();
                                    Shop_Max_Money = Sql_Data_Reader["Shop_Max_Money"].ToString();

                                    Shop_Max_Mem = Sql_Data_Reader["Shop_Max_Mem"].ToString();

                                    //
                                    Row = Row + 1;
                                    Work_sheets.Cells[Row, i1 * 3 + 1].Value = Shop_Max;
                                    Work_sheets.Cells[Row, i1 * 3 + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                                    Work_sheets.Cells[Row, i1 * 3 + 2].Value = new _4e().Convert_String_To_BigInt(Shop_Max_Mem, 0);
                                    Work_sheets.Cells[Row, i1 * 3 + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                    Work_sheets.Cells[Row, i1 * 3 + 3].Value = new _4e().Convert_String_To_BigInt(Shop_Max_Money, 0);
                                    Work_sheets.Cells[Row, i1 * 3 + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    Work_sheets.Cells[Row, i1 * 3 + 3].Style.Numberformat.Format = "0,000";
                                }

                            }
                            catch (SqlException Sql_Exception)
                            {
                            }

                            if (!Sql_Data_Reader.IsClosed)
                            {
                                Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
                            }
                        }
                    }
                }

                //
                int Total_Column = (Time_Array.Length * 3);

                //
                for (int x1 = 0; x1 < Total_Column; x1++)
                {
                    //Work_sheets.Column(1 + x1).AutoFit();
                    Work_sheets.Column(1 + x1).Width = 15;
                }
            }

            //Save
            Excel_Package.Save();
            On_Page_Load += " window.location.href = '" + new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty) + "/File/" + Random_Folder + "/" + File_Info.Name) + "';";

            //
            Sql_Query = string.Empty;
        }
        else
        if (Report_Name_Code == "Sale")
        {
            //Excel

            //
            string Random_Folder = Guid.NewGuid().ToString();
            string Random_Folder_Path = Server.MapPath("~/File/" + Random_Folder + "/");

            new _4e().Creat_Directory(Random_Folder_Path);

            //
            string File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                    + ".xlsx";

            //
            string Folder_Drive = "D:";

            if (Request.IsLocal)
            {
                Folder_Drive = "D:";
            }

            //
            FileInfo File_Info = new FileInfo(Folder_Drive + @"\Websites\Garden\File\" + Random_Folder + "\\" + File_Name);
            ExcelPackage Excel_Package = new ExcelPackage(File_Info);

            //
            var Work_sheets_SUM = Excel_Package.Workbook.Worksheets.Add("SUMMARY");

            //
            Work_sheets_SUM.Cells[1, 3].Value = Report_lbl.Text + " (SUMMARY)";
            Work_sheets_SUM.Cells[1, 3, 1, 9].Merge = true;

            Work_sheets_SUM.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
            Work_sheets_SUM.Cells[1, 3].Style.Font.Bold = true;
            Work_sheets_SUM.Cells[1, 3].Style.Font.Size = 20;

            Work_sheets_SUM.Cells[3, 1].Value = "Month";
            Work_sheets_SUM.Cells[3, 1].Style.Font.Color.SetColor(Color.Red);
            Work_sheets_SUM.Cells[3, 1].Style.Font.Bold = true;
            Work_sheets_SUM.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            Work_sheets_SUM.Cells[3, 2].Value = "Day";
            Work_sheets_SUM.Cells[3, 2].Style.Font.Color.SetColor(Color.Red);
            Work_sheets_SUM.Cells[3, 2].Style.Font.Bold = true;
            Work_sheets_SUM.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            //
            for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
            {
                if (Hour_int == 9)
                {
                    Work_sheets_SUM.Cells[3, Hour_int - 6].Value = "< 10";
                }
                else
                if (Hour_int == 22)
                {
                    Work_sheets_SUM.Cells[3, Hour_int - 6].Value = "> 22";
                }
                else
                {
                    Work_sheets_SUM.Cells[3, Hour_int - 6].Value = Hour_int + "--" + (Hour_int + 1);
                }

                Work_sheets_SUM.Cells[3, Hour_int - 6].Style.Font.Color.SetColor(Color.Red);
                Work_sheets_SUM.Cells[3, Hour_int - 6].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[3, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            Work_sheets_SUM.Cells[3, 17].Value = "Total";
            Work_sheets_SUM.Cells[3, 17].Style.Font.Color.SetColor(Color.Red);
            Work_sheets_SUM.Cells[3, 17].Style.Font.Bold = true;
            Work_sheets_SUM.Cells[3, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            int Year_int = 2016;

            int Row_Summary = 3;

            for (int Month_int = 1; Month_int <= 11; Month_int++)
            {
                var Work_sheets = Excel_Package.Workbook.Worksheets.Add(Month_int.ToString() + "-" + Year_int.ToString());

                //
                Work_sheets.Cells[1, 3].Value = Report_lbl.Text + " (" + Month_int.ToString() + "-" + Year_int.ToString() + ")";
                Work_sheets.Cells[1, 3, 1, 9].Merge = true;

                Work_sheets.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[1, 3].Style.Font.Bold = true;
                Work_sheets.Cells[1, 3].Style.Font.Size = 20;

                Work_sheets.Cells[3, 1].Value = "Date";
                Work_sheets.Cells[3, 1].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[3, 1].Style.Font.Bold = true;
                Work_sheets.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                Work_sheets.Cells[3, 2].Value = "Day";
                Work_sheets.Cells[3, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[3, 2].Style.Font.Bold = true;
                Work_sheets.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                {
                    if (Hour_int == 9)
                    {
                        Work_sheets.Cells[3, Hour_int - 6].Value = "< 10";
                    }
                    else
                    if (Hour_int == 22)
                    {
                        Work_sheets.Cells[3, Hour_int - 6].Value = "> 22";
                    }
                    else
                    {
                        Work_sheets.Cells[3, Hour_int - 6].Value = Hour_int + "--" + (Hour_int + 1);
                    }

                    Work_sheets.Cells[3, Hour_int - 6].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets.Cells[3, Hour_int - 6].Style.Font.Bold = true;
                    Work_sheets.Cells[3, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                Work_sheets.Cells[3, 17].Value = "Total";
                Work_sheets.Cells[3, 17].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[3, 17].Style.Font.Bold = true;
                Work_sheets.Cells[3, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                string Thang_Nam = string.Empty;

                //
                if (Month_int < 10)
                {
                    Thang_Nam = "0" + Month_int.ToString() + Year_int.ToString();
                }
                else
                {
                    Thang_Nam = Month_int.ToString() + Year_int.ToString();
                }

                //
                int Row = 3;

                for (int Day_int = 1; Day_int <= 31; Day_int++)
                {
                    if ((!new _4e().Check_Date(Day_int + "/" + Month_int + "/" + Year_int)) || (!new _4e().Check_Date_Less_Than_Today(Day_int + "/" + Month_int + "/" + Year_int)))
                    {
                        break;
                    }
                    else
                    {
                        Row = Row + 1;
                        Work_sheets.Cells[Row, 1].Value = Day_int + "/" + Month_int;
                        Work_sheets.Cells[Row, 1].Style.Font.Bold = true;
                        Work_sheets.Cells[Row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        DateTime Ngay_Thang_Nam = new _4e().Convert_Date_String_To_Date_Time(Day_int + "/" + Month_int + "/" + Year_int, new DateTime());

                        Work_sheets.Cells[Row, 2].Value = Ngay_Thang_Nam.DayOfWeek.ToString();
                        Work_sheets.Cells[Row, 2].Style.Font.Bold = false;
                        Work_sheets.Cells[Row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        //
                        for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                        {
                            Sql_Query =
                                "SELECT SUM(TriGiaBan)"
                                + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                                + " WHERE (CONVERT(DATE, NgayBatDau, 103) =  CONVERT(DATE, @NgayThangNam, 103))"
                                + " AND (GioBatDau / 3600 >= @Hour_Start) AND (GioBatDau / 3600 < @Hour_End)"
                                ;

                            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                            Sql_Command.Parameters.Add("@NgayThangNam", Day_int + "-" + Month_int + "-" + Year_int);
                            Sql_Command.Parameters.Add("@Hour_Start", Hour_int);
                            Sql_Command.Parameters.Add("@Hour_End", Hour_int + 1);

                            string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                            Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                            //
                            if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                            {
                                Work_sheets.Cells[Row, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                                Work_sheets.Cells[Row, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                Work_sheets.Cells[Row, Hour_int - 6].Style.Numberformat.Format = "0,000";
                            }
                        }

                        //
                        if (1 == 1)
                        {
                            Sql_Query =
                                "SELECT SUM(TriGiaBan)"
                                + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                                + " WHERE (CONVERT(DATE, NgayBatDau, 103) =  CONVERT(DATE, @NgayThangNam, 103))"
                                ;

                            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                            Sql_Command.Parameters.Add("@NgayThangNam", Day_int + "-" + Month_int + "-" + Year_int);

                            string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                            Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                            //
                            if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                            {
                                Work_sheets.Cells[Row, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                                Work_sheets.Cells[Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                Work_sheets.Cells[Row, 17].Style.Numberformat.Format = "0,000";
                            }
                        }
                    }
                }

                //Total
                Row = Row + 1;
                Row = Row + 1;
                Work_sheets.Cells[Row, 2].Value = "Total";
                Work_sheets.Cells[Row, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[Row, 2].Style.Font.Bold = true;
                Work_sheets.Cells[Row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                if (Month_int > 1)
                {
                    Row_Summary = Row_Summary + 1;
                }

                Row_Summary = Row_Summary + 1;

                //
                Work_sheets_SUM.Cells[Row_Summary, 1].Value = Month_int.ToString();
                Work_sheets_SUM.Cells[Row_Summary, 1, Row_Summary + 4, 1].Merge = true;

                Work_sheets_SUM.Cells[Row_Summary, 1].Style.Font.Color.SetColor(Color.Black);
                Work_sheets_SUM.Cells[Row_Summary, 1].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[Row_Summary, 1].Style.Font.Size = 20;
                Work_sheets_SUM.Cells[Row_Summary, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                Work_sheets_SUM.Cells[Row_Summary, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                Work_sheets_SUM.Cells[Row_Summary, 2].Value = "Total";
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (GioBatDau / 3600 >= @Hour_Start) AND (GioBatDau / 3600 < @Hour_End)"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.Add("@Hour_Start", Hour_int);
                    Sql_Command.Parameters.Add("@Hour_End", Hour_int + 1);

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, Hour_int - 6].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                    }
                }

                //
                if (1 == 1)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, 17].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.Numberformat.Format = "0,000";
                    }
                }

                //2 + 3 + 4
                Row = Row + 1;
                Work_sheets.Cells[Row, 2].Value = "2 + 3 + 4";
                Work_sheets.Cells[Row, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[Row, 2].Style.Font.Bold = true;
                Work_sheets.Cells[Row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                Row_Summary = Row_Summary + 1;
                Work_sheets_SUM.Cells[Row_Summary, 2].Value = "2 + 3 + 4";
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Monday', 'Tuesday', 'Wednesday'))"
                        + " AND (GioBatDau / 3600 >= @Hour_Start) AND (GioBatDau / 3600 < @Hour_End)"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.Add("@Hour_Start", Hour_int);
                    Sql_Command.Parameters.Add("@Hour_End", Hour_int + 1);

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, Hour_int - 6].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                    }
                }

                //
                if (1 == 1)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Monday', 'Tuesday', 'Wednesday'))"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, 17].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.Numberformat.Format = "0,000";
                    }
                }

                //5
                Row = Row + 1;
                Work_sheets.Cells[Row, 2].Value = "5";
                Work_sheets.Cells[Row, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[Row, 2].Style.Font.Bold = true;
                Work_sheets.Cells[Row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                Row_Summary = Row_Summary + 1;
                Work_sheets_SUM.Cells[Row_Summary, 2].Value = "5";
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Thursday'))"
                        + " AND (GioBatDau / 3600 >= @Hour_Start) AND (GioBatDau / 3600 < @Hour_End)"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.Add("@Hour_Start", Hour_int);
                    Sql_Command.Parameters.Add("@Hour_End", Hour_int + 1);

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, Hour_int - 6].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                    }
                }

                //
                if (1 == 1)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Thursday'))"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, 17].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.Numberformat.Format = "0,000";
                    }
                }

                //6
                Row = Row + 1;
                Work_sheets.Cells[Row, 2].Value = "6";
                Work_sheets.Cells[Row, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[Row, 2].Style.Font.Bold = true;
                Work_sheets.Cells[Row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                Row_Summary = Row_Summary + 1;
                Work_sheets_SUM.Cells[Row_Summary, 2].Value = "6";
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Friday'))"
                        + " AND (GioBatDau / 3600 >= @Hour_Start) AND (GioBatDau / 3600 < @Hour_End)"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.Add("@Hour_Start", Hour_int);
                    Sql_Command.Parameters.Add("@Hour_End", Hour_int + 1);

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, Hour_int - 6].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                    }
                }

                //
                if (1 == 1)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Friday'))"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, 17].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.Numberformat.Format = "0,000";
                    }
                }

                //7 + CN
                Row = Row + 1;
                Work_sheets.Cells[Row, 2].Value = "7 + CN";
                Work_sheets.Cells[Row, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets.Cells[Row, 2].Style.Font.Bold = true;
                Work_sheets.Cells[Row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                Row_Summary = Row_Summary + 1;
                Work_sheets_SUM.Cells[Row_Summary, 2].Value = "7 + CN";
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //
                for (int Hour_int = 9; Hour_int <= 22; Hour_int++)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Saturday', 'Sunday'))"
                        + " AND (GioBatDau / 3600 >= @Hour_Start) AND (GioBatDau / 3600 < @Hour_End)"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    Sql_Command.Parameters.Add("@Hour_Start", Hour_int);
                    Sql_Command.Parameters.Add("@Hour_End", Hour_int + 1);

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, Hour_int - 6].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                    }
                }

                //
                if (1 == 1)
                {
                    Sql_Query =
                        "SELECT SUM(TriGiaBan)"
                        + " FROM TOPOS_DB.DBO.HoaDon" + Thang_Nam
                        + " WHERE (DATENAME(dw, NgayBatDau) IN ('Saturday', 'Sunday'))"
                        ;

                    Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

                    string Doanh_So_Tung_Gio = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());
                    Doanh_So_Tung_Gio = new _4e().Remove_String_Last(Doanh_So_Tung_Gio, ".0000");

                    //
                    if ((Doanh_So_Tung_Gio != "0") && (Doanh_So_Tung_Gio != string.Empty))
                    {
                        Work_sheets.Cells[Row, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets.Cells[Row, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets.Cells[Row, 17].Style.Numberformat.Format = "0,000";

                        Work_sheets_SUM.Cells[Row_Summary, 17].Value = new _4e().Convert_String_To_BigInt(Doanh_So_Tung_Gio, 0);
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        Work_sheets_SUM.Cells[Row_Summary, 17].Style.Numberformat.Format = "0,000";
                    }
                }

                //
                if (Month_int > 1)
                {
                    int Number_Next_Row = 2;

                    if (Month_int > 2)
                    {
                        Number_Next_Row = 3;
                    }

                    Row_Summary = Row_Summary + 1;
                    Row_Summary = Row_Summary + 1;

                    //
                    Work_sheets_SUM.Cells[Row_Summary, 1].Value = (Month_int - 1) + " - " + Month_int.ToString() + " (%)";
                    Work_sheets_SUM.Cells[Row_Summary, 1, Row_Summary + 4, 1].Merge = true;

                    Work_sheets_SUM.Cells[Row_Summary, 1].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets_SUM.Cells[Row_Summary, 1].Style.Font.Bold = true;
                    Work_sheets_SUM.Cells[Row_Summary, 1].Style.Font.Size = 20;
                    Work_sheets_SUM.Cells[Row_Summary, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    Work_sheets_SUM.Cells[Row_Summary, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //
                    Work_sheets_SUM.Cells[Row_Summary, 2].Value = "Total";
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //
                    for (int Hour_int = 9; Hour_int <= 23; Hour_int++)
                    {
                        Int64 Month_1 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6 * Number_Next_Row, Hour_int - 6].Value), 0);
                        Int64 Month_2 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6, Hour_int - 6].Value), 0);

                        if ((Month_1 != 0) && (Month_2 != 0))
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Math.Round((decimal)(Month_2 - Month_1) / (decimal)Month_1 * (decimal)100, 1) + " %";
                        }
                        else
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Month_2 - Month_1;
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                        }

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }

                    Row_Summary = Row_Summary + 1;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Value = "2 + 3 + 4";
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //
                    for (int Hour_int = 9; Hour_int <= 23; Hour_int++)
                    {
                        Int64 Month_1 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6 * Number_Next_Row, Hour_int - 6].Value), 0);
                        Int64 Month_2 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6, Hour_int - 6].Value), 0);

                        if ((Month_1 != 0) && (Month_2 != 0))
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Math.Round((decimal)(Month_2 - Month_1) / (decimal)Month_1 * (decimal)100, 1) + " %";
                        }
                        else
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Month_2 - Month_1;
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                        }

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }

                    Row_Summary = Row_Summary + 1;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Value = "5";
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //
                    for (int Hour_int = 9; Hour_int <= 23; Hour_int++)
                    {
                        Int64 Month_1 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6 * Number_Next_Row, Hour_int - 6].Value), 0);
                        Int64 Month_2 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6, Hour_int - 6].Value), 0);

                        if ((Month_1 != 0) && (Month_2 != 0))
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Math.Round((decimal)(Month_2 - Month_1) / (decimal)Month_1 * (decimal)100, 1) + " %";
                        }
                        else
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Month_2 - Month_1;
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                        }

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }

                    Row_Summary = Row_Summary + 1;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Value = "6";
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //
                    for (int Hour_int = 9; Hour_int <= 23; Hour_int++)
                    {
                        Int64 Month_1 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6 * Number_Next_Row, Hour_int - 6].Value), 0);
                        Int64 Month_2 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6, Hour_int - 6].Value), 0);

                        if ((Month_1 != 0) && (Month_2 != 0))
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Math.Round((decimal)(Month_2 - Month_1) / (decimal)Month_1 * (decimal)100, 1) + " %";
                        }
                        else
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Month_2 - Month_1;
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                        }

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }

                    Row_Summary = Row_Summary + 1;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Value = "7 + CN";
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Color.SetColor(Color.Red);
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.Font.Bold = true;
                    Work_sheets_SUM.Cells[Row_Summary, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    //
                    for (int Hour_int = 9; Hour_int <= 23; Hour_int++)
                    {
                        Int64 Month_1 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6 * Number_Next_Row, Hour_int - 6].Value), 0);
                        Int64 Month_2 = new _4e().Convert_String_To_BigInt(new _4e().Object_ToString(Work_sheets_SUM.Cells[Row_Summary - 6, Hour_int - 6].Value), 0);

                        if ((Month_1 != 0) && (Month_2 != 0))
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Math.Round((decimal)(Month_2 - Month_1) / (decimal)Month_1 * (decimal)100, 1) + " %";
                        }
                        else
                        {
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Value = Month_2 - Month_1;
                            Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.Numberformat.Format = "0,000";
                        }

                        Work_sheets_SUM.Cells[Row_Summary, Hour_int - 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    }
                }

                //
                int Total_Column = 17;

                //
                for (int x1 = 1; x1 <= Total_Column; x1++)
                {
                    Work_sheets.Column(x1).AutoFit();
                    Work_sheets_SUM.Column(x1).AutoFit();
                }

                Work_sheets_SUM.Column(1).Width = 18;
            }

            //Save
            Excel_Package.Save();
            On_Page_Load += " window.location.href = '" + new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty) + "/File/" + Random_Folder + "/" + File_Info.Name) + "';";

            //
            Sql_Query = string.Empty;
        }
        else
        if (Report_Name_Code == "Card_not_use")
        {
            //Excel

            //
            string Random_Folder = Guid.NewGuid().ToString();
            string Random_Folder_Path = Server.MapPath("~/File/" + Random_Folder + "/");

            new _4e().Creat_Directory(Random_Folder_Path);

            //
            string File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                    + ".xlsx";

            //
            string Folder_Drive = "D:";

            if (Request.IsLocal)
            {
                Folder_Drive = "D:";
            }

            //
            FileInfo File_Info = new FileInfo(Folder_Drive + @"\Websites\Garden\File\" + Random_Folder + "\\" + File_Name);
            ExcelPackage Excel_Package = new ExcelPackage(File_Info);

            //
            var Work_sheets = Excel_Package.Workbook.Worksheets.Add(Report_lbl.Text);

            //
            Work_sheets.Cells[1, 3].Value = Report_lbl.Text;
            Work_sheets.Cells[1, 3, 1, 9].Merge = true;

            Work_sheets.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
            Work_sheets.Cells[1, 3].Style.Font.Bold = true;
            Work_sheets.Cells[1, 3].Style.Font.Size = 20;

            //
            Sql_Query =

                " SELECT "
                + " Member_Alias.Mem_Card AS Card, Member_Alias.Mem_Nm AS Name, Member_Alias.MOBILE_NO AS Phone, Member_Alias.REG_DT AS REG_Time,"
                + " Current_Point, Last_Time"

                + " FROM ("
                + "     SELECT Mem_SEQ, Mem_Card, Mem_Nm, MOBILE_NO, REG_DT"
                + "     FROM GARDEN_CRM.DBO.T_MEM_MST"

                + "     WHERE (ISNUMERIC(MEM_CARD) = 1)"
                + "     AND ("
                + "        ((CONVERT(BIGINT, Mem_Card) >= 01010029695) AND (CONVERT(BIGINT, Mem_Card) <= 01010029998))"
                + "     OR ((CONVERT(BIGINT, Mem_Card) >= 01010030000) AND (CONVERT(BIGINT, Mem_Card) <= 01010030339))"
                + "     OR ((CONVERT(BIGINT, Mem_Card) >= 01010030403) AND (CONVERT(BIGINT, Mem_Card) <= 01010030773))"
                + "     OR ((CONVERT(BIGINT, Mem_Card) >= 01010039998) AND (CONVERT(BIGINT, Mem_Card) <= 01010040039))"
                + "     OR ((CONVERT(BIGINT, Mem_Card) >= 01010049600) AND (CONVERT(BIGINT, Mem_Card) <= 01010049990))"
                + "     )"
                + " ) AS Member_Alias"

                + " LEFT JOIN"
                + " ("
                + "     SELECT Mem_SEQ, SUM(CHG_PNT) AS Current_Point"
                + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                + "     GROUP BY Mem_SEQ"
                + " ) AS Current_Point_Temp"
                + " ON Member_Alias.Mem_SEQ = Current_Point_Temp.Mem_SEQ"

                + " LEFT JOIN"
                + " ("
                + "     SELECT Mem_SEQ, MAX(MOD_DT) AS Last_Time"
                + "     FROM GARDEN_CRM.DBO.T_PNT_HIS_INFO"
                + "     GROUP BY Mem_SEQ"
                + " ) AS Last_Time_Temp"
                + " ON Member_Alias.Mem_SEQ = Last_Time_Temp.Mem_SEQ"

                + " ORDER BY Member_Alias.Mem_Card"
                ;

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

            int Row = 3;

            //
            Work_sheets.Cells[Row, 1].Value = "Card";
            Work_sheets.Cells[Row, 2].Value = "Khách hàng";
            Work_sheets.Cells[Row, 3].Value = "Phone";
            Work_sheets.Cells[Row, 4].Value = "Tạo ngày";
            Work_sheets.Cells[Row, 5].Value = "Điểm";
            Work_sheets.Cells[Row, 6].Value = "Gần nhất";

            Row++;

            //
            try
            {
                while (Sql_Data_Reader.Read())
                {
                    string Mem_Card = Sql_Data_Reader["Card"].ToString();
                    string Mem_Name = Sql_Data_Reader["Name"].ToString();
                    string Mem_Phone = Sql_Data_Reader["Phone"].ToString();
                    string Mem_REG_Time = Sql_Data_Reader["REG_Time"].ToString();

                    string Current_Point = Sql_Data_Reader["Current_Point"].ToString();
                    string Last_Time = Sql_Data_Reader["Last_Time"].ToString();

                    //
                    Work_sheets.Cells[Row, 1].Value = Mem_Card;
                    Work_sheets.Cells[Row, 2].Value = Mem_Name;
                    Work_sheets.Cells[Row, 3].Value = Mem_Phone;
                    Work_sheets.Cells[Row, 4].Value = Mem_REG_Time;
                    Work_sheets.Cells[Row, 5].Value = Current_Point;
                    Work_sheets.Cells[Row, 6].Value = Last_Time;

                    //
                    Row++;
                }
            }
            catch (SqlException Sql_Exception)
            {
            }

            if (!Sql_Data_Reader.IsClosed)
            {
                Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
            }

            //
            int Total_Column = 6;

            //
            for (int x1 = 1; x1 <= Total_Column; x1++)
            {
                Work_sheets.Column(x1).AutoFit();
            }

            //Save
            Excel_Package.Save();
            On_Page_Load += " window.location.href = '" + new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty) + "/File/" + Random_Folder + "/" + File_Info.Name) + "';";

            //
            Sql_Query = string.Empty;
        }
        else
                    if (Report_Name_Code == "Add_point")
        {
            if (Filter_RDOL_1.SelectedValue == "2")
            {
                Sql_Where = " AND (CHG_PNT < 0) AND ((PNT_RSN LIKE '%redem%') OR (PNT_RSN LIKE '%redeem%'))";
            }
            else
                if (Filter_RDOL_1.SelectedValue == "3")
            {
                Sql_Where = " AND (CHG_PNT < 0) AND ((PNT_RSN IS NULL) OR ((PNT_RSN NOT LIKE '%redem%') AND (PNT_RSN NOT LIKE '%redeem%')))";
            }

            //
            Sql_Query +=
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

                + Sql_Where

                + Sql_Where_Shop
                + Sql_Where_Card
                + Sql_Where_Name
                + Sql_Where_Phone
                + Sql_Where_Email
                + Sql_Where_Address

                + Sql_Where_Add_By
                ;
        }
        else
            if (Report_Name_Code == "Statement")
        {
            Sql_Query +=
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
        else
                if (Report_Name_Code == "Current_point")
        {
            Sql_Query +=
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
        else
                    if (Report_Name_Code == "Compare")
        {
            Sql_Query +=
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
        else
                        if (Report_Name_Code == "Max_buying_by_Shop")
        {
            Sql_Query +=
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
        else
                            if (Report_Name_Code == "Buying_list")
        {
            Sql_Query +=
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
        else
                                if (Report_Name_Code == "Inquiry_discount")
        {
            if (!new _4e().Check_Exists_Table_POS("TOPOS_DB", Data_Table_Hoa_Don))
            {
                Sql_Query += " SELECT 0";
            }
            else
            {
                Sql_Query +=
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
        else
                                    if (Report_Name_Code == "Analys_transaction")
        {
            if (!new _4e().Check_Exists_Table_POS("TOPOS_DB", Data_Table_Hoa_Don))
            {
                Sql_Query += " SELECT 0";
            }
            else
            {
                Sql_Query +=
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
        else
                                        if (Report_Name_Code == "Member_list")
        {
            Sql_Query +=

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
        else
        if (Report_Name_Code == "FC_History")
        {
            if (Card != string.Empty)
            {
                //
                int Year_int = DateTime.Now.Year;
                int Month_int = DateTime.Now.Month;

                string Thang_Nam = string.Empty;

                //
                if (Month_int < 10)
                {
                    Thang_Nam += "0" + Month_int.ToString();
                }
                else
                {
                    Thang_Nam += Month_int.ToString();
                }

                Thang_Nam += DateTime.Now.Year;

                ////
                //Sql_Query +=
                //    " USE TOPOS_DB"

                //    + " SELECT COUNT(*)"

                //    + " FROM TTT_CTNapTien_HinhThucThanhToan Nap_Rut_Alias"

                //    + " LEFT JOIN (" + Sql_Join_HD + ") Hoa_Don_Alias"
                //    + " ON Nap_Rut_Alias.HoaDonID = Hoa_Don_Alias.MaHD"

                //    + " LEFT JOIN (" + Sql_Join_CTHD + ") CTHoa_Don_Alias"
                //    + " ON Nap_Rut_Alias.HoaDonID = CTHoa_Don_Alias.MaHD"

                //    + " LEFT JOIN HinhThucThanhToan Hinh_Thuc_Thanh_Toan_Alias"
                //    + " ON Nap_Rut_Alias.MaHinhThuc = Hinh_Thuc_Thanh_Toan_Alias.MaHinhThuc"

                //    + " LEFT JOIN ThueGianHang ThueGianHang_Alias"
                //    + " ON Hoa_Don_Alias.MaThue = ThueGianHang_Alias.MaThue"

                //    + " WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card)))"
                //    ;

                Sql_Query +=

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

        bool RUN_SQL = true;

        if ((Report_Name_Code == "FC_ACC") || (Report_Name_Code == "FC_Block") || (Report_Name_Code == "SAP"))
        {
            RUN_SQL = false;
        }

        //
        Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
        //new _4e().Write_To_File("\\Temp\\Sql_Query_1.sql", Sql_Query);

        if ((RUN_SQL) && (Sql_Query != string.Empty))
        {
            if (Report_Name_Code == "FC_History")
            {
                Open_Sql_Connection_FC();
            }

            Sql_Command = new SqlCommand(Sql_Query, Sql_Connection);

            Sql_Command.CommandTimeout = 0;
            new _4e().Add_SQL_Parameters(ref Sql_Command, SQL_Parameters_Array, SQL_Value_Array);

            string Total_Result = new _4e().Check_Sql_Query_Result(Sql_Command.ExecuteScalar());

            //
            ViewState["Total_Result"] = new _4e().Convert_String_To_Int(Total_Result, 0);

            //Pageing
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
                    Page_Query_String = new _4e().Determine_Query_String_ID(string.Empty, "Page");
                    Page = new _4e().Convert_String_To_Int(Page_Query_String, 1);
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

                    Total_Result_1_lbl.Text = "Found: " + new _4e().Split_Thousand(ViewState["Total_Result"].ToString()) + " results (divide by " + new _4e().Split_Thousand(ViewState["Total_Page"].ToString()) + " pages)";
                    Total_Result_2_lbl.Text = Total_Result_1_lbl.Text;

                    //
                    URL = Request.Url.ToString();

                    URL = new _4e().Update_Query_String(URL, "Time_Start_1", HttpUtility.UrlEncode(Time_Start_1));
                    URL = new _4e().Update_Query_String(URL, "Time_Finish_1", HttpUtility.UrlEncode(Time_Finish_1));

                    URL = new _4e().Update_Query_String(URL, "Time_Start_2", HttpUtility.UrlEncode(Time_Start_2));
                    URL = new _4e().Update_Query_String(URL, "Time_Finish_2", HttpUtility.UrlEncode(Time_Finish_2));

                    URL = new _4e().Update_Query_String(URL, "Shop", HttpUtility.UrlEncode(Shop_tbx.Text));
                    URL = new _4e().Update_Query_String(URL, "Card", HttpUtility.UrlEncode(Card_tbx.Text));
                    URL = new _4e().Update_Query_String(URL, "Name", HttpUtility.UrlEncode(Name_tbx.Text));
                    URL = new _4e().Update_Query_String(URL, "Phone", HttpUtility.UrlEncode(Phone_tbx.Text));
                    URL = new _4e().Update_Query_String(URL, "Email", HttpUtility.UrlEncode(Email_tbx.Text));
                    URL = new _4e().Update_Query_String(URL, "Address", HttpUtility.UrlEncode(Address_tbx.Text));

                    URL = new _4e().Update_Query_String(URL, "Card_Actived_OR_Disabled", Card_Actived_OR_Disabled_rdol.SelectedValue);
                    URL = new _4e().Update_Query_String(URL, "RDOL_1", Filter_RDOL_1.SelectedValue);
                    URL = new _4e().Update_Query_String(URL, "RDOL_2", Filter_RDOL_2.SelectedValue);

                    URL = new _4e().Update_Query_String(URL, "CBXL_1", new _4e().Determine_CBXL_Selected(Filter_CBXL_1, "value"));

                    int Page_Number = 0;

                    Page_Number = ((((int)ViewState["Page_Group"]) - 1) * 10);

                    if (0 < Page_Number)
                    {
                        URL = new _4e().Update_Query_String(URL, "Page", Page_Number.ToString());

                        Change_Page += "<a href='" + URL + "'><img src='" + Index_Host_hdf.Value + "/index/Mui_Ten_01.jpg'></a> ... ";
                    }

                    for (int i1 = 1; i1 <= 10; i1++)
                    {
                        Page_Number = i1 + ((((int)ViewState["Page_Group"]) - 1) * 10);

                        if (Page_Number <= (int)ViewState["Total_Page"])
                        {
                            URL = new _4e().Update_Query_String(URL, "Page", Page_Number.ToString());

                            if (Page_Number == Page)
                            {
                                Change_Page += "<a href='" + URL + "'><span class='Bold_Red_Text_css' style='font-sizD: 12pt;'>" + Convert.ToString(Page_Number) + "</span></a>";
                            }
                            else
                            {
                                Change_Page += "<a href='" + URL + "'>" + Convert.ToString(Page_Number) + "</a>";
                            }

                            if ((i1 < 10) && (Page_Number < (int)ViewState["Total_Page"]))
                            {
                                Change_Page += " - ";
                            }
                        }
                    }

                    Page_Number = 11 + ((((int)ViewState["Page_Group"]) - 1) * 10);

                    if (Page_Number <= (int)ViewState["Total_Page"])
                    {
                        URL = new _4e().Update_Query_String(URL, "Page", Page_Number.ToString());

                        Change_Page += " ... <a href='" + URL + "'><img src='" + Index_Host_hdf.Value + "/index/Mui_Ten_02.jpg'></a>";
                    }
                }

                //Read data
                Sql_Query =
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

                if (Report_Name_Code == "Add_point")
                {
                    Sql_Query +=
                        " SELECT * FROM"
                        + " ("

                        + " SELECT PNT_HIS_SEQ AS ID, ROW_NUMBER() OVER(ORDER BY Sale_Alias.MOD_DT DESC) AS Number,"
                        ;

                    if (Card_Actived_OR_Disabled_rdol.SelectedValue == "1")
                    {
                        Sql_Query +=
                            " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) + ' ' + CONVERT(NVARCHAR(MAX), ISNULL(Member_Alias_Old.Mem_Card, '')) AS Card,"
                            ;
                    }
                    else
                    {
                        Sql_Query +=
                            " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) + ' ' + CONVERT(NVARCHAR(MAX), ISNULL(Member_Alias_New.Mem_Card, '')) AS Card,"
                            ;

                        //Sql_Query +=
                        //    " CONVERT(NVARCHAR(MAX), Member_Alias.Mem_Card) AS Card,"
                        //    ;
                    }

                    Sql_Query +=
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

                        + Sql_Where

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
                else
                    if (Report_Name_Code == "Statement")
                {
                    Sql_Query +=

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
                else
                        if (Report_Name_Code == "Current_point")
                {
                    Sql_Query +=

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
                else
                            if (Report_Name_Code == "Compare")
                {
                    Sql_Query +=
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
                else//OK
                                if (Report_Name_Code == "Max_buying_by_Shop")
                {
                    Sql_Query +=
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
                else//OK
                                    if (Report_Name_Code == "Buying_list")
                {
                    Sql_Query +=
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
                else
                                        if (Report_Name_Code == "Inquiry_discount")
                {
                    Sql_Query +=
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
                else//OK
                                            if (Report_Name_Code == "Analys_transaction")
                {
                    Sql_Query +=
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
                else//OK
                                                if (Report_Name_Code == "Member_list")
                {
                    Sql_Query +=

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

                        + " SELECT ROW_NUMBER() OVER(ORDER BY " + Sql_Order + ") AS Number,"
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
                else
                if (Report_Name_Code == "FC_History")
                {
                    if (Card != string.Empty)
                    {
                        //
                        int Year_int = DateTime.Now.Year;
                        int Month_int = DateTime.Now.Month;

                        string Thang_Nam = string.Empty;

                        //
                        if (Month_int < 10)
                        {
                            Thang_Nam += "0" + Month_int.ToString();
                        }
                        else
                        {
                            Thang_Nam += Month_int.ToString();
                        }

                        Thang_Nam += DateTime.Now.Year;

                        //
                        //Sql_Query +=

                        //    " USE TOPOS_DB"

                        //    + " SELECT * FROM"
                        //    + " ("

                        //    + " SELECT ROW_NUMBER() OVER(ORDER BY Nap_Rut_Alias.CTNapTien_Chi_Tiet_ID DESC) AS Number,"

                        //    + " REPLACE(NgayGioQuet, 'T', ' ') AS Thoi_Gian, Nap_Rut_Alias.MaHD AS Hoa_Don, Hoa_Don_Alias.MaQuay, ThueGianHang_Alias.TenGianHang, Hoa_Don_Alias.TenNVBanHang, TenHH AS Hang_Hoa, TenHinhThuc AS Hinh_Thuc, CONVERT(BIGINT, TienTruocThanhToan) AS Truoc, CONVERT(BIGINT, ThanhTien) AS So_Tien, CONVERT(BIGINT, TienSauThanhToan) AS Sau"

                        //    + " FROM TTT_CTNapTien_HinhThucThanhToan Nap_Rut_Alias"

                        //    + " LEFT JOIN (" + Sql_Join_HD + ") Hoa_Don_Alias"
                        //    + " ON Nap_Rut_Alias.HoaDonID = Hoa_Don_Alias.MaHD"

                        //    + " LEFT JOIN (" + Sql_Join_CTHD + ") CTHoa_Don_Alias"
                        //    + " ON Nap_Rut_Alias.HoaDonID = CTHoa_Don_Alias.MaHD"

                        //    + " LEFT JOIN HinhThucThanhToan Hinh_Thuc_Thanh_Toan_Alias"
                        //    + " ON Nap_Rut_Alias.MaHinhThuc = Hinh_Thuc_Thanh_Toan_Alias.MaHinhThuc"

                        //    + " LEFT JOIN ThueGianHang ThueGianHang_Alias"
                        //    + " ON Hoa_Don_Alias.MaThue = ThueGianHang_Alias.MaThue"

                        //    + " WHERE (CTKichHoatID IN (SELECT CTKichHoatID FROM TTT_CTKichHoat WHERE (MaFoodCourt LIKE @Card)))"

                        //    + " )"
                        //    + " AS #Temp_Table"

                        //    + " WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))"
                        //    ;

                        //
                        Sql_Query +=

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

                //
                Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
                //new _4e().Write_To_File("\\Temp\\Sql_Query_2.sql", Sql_Query);

                if (Sql_Query != string.Empty)
                {
                    //All Page
                    if (!Is_one_Page)
                    {
                        Sql_Query = Sql_Query.Replace(" WHERE ((Number >= " + ((int)Session["Total_Result_Per_Page_For_Report"] * ((int)(ViewState["Page"]) - 1) + 1) + ") AND (Number <= " + ((int)Session["Total_Result_Per_Page_For_Report"] * (int)(ViewState["Page"])) + "))", string.Empty);
                    }

                    //
                    Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;
                    new _4e().Add_SQL_Parameters(ref Sql_Command, SQL_Parameters_Array, SQL_Value_Array);

                    SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

                    //
                    int j1 = 0;

                    //
                    Dynamic_Control_Holder_pnl.Controls.Clear();

                    int Total_Column = Sql_Data_Reader.FieldCount;

                    if (ViewState["Primary_Column"] != null)
                    {
                        Primary_Column = ViewState["Primary_Column"].ToString();
                    }

                    //
                    string[] ALL_Shop_Code_Array = new string[0];
                    string[] ALL_Shop_Name_Array = new string[0];

                    if ((Application["ALL_Shop_Code_Array"] == null) || (Application["ALL_Shop_Name_Array"] == null))
                    {
                        new _4e().Read_ALL_Shop_Code_Array(out ALL_Shop_Code_Array, out ALL_Shop_Name_Array);
                    }
                    else
                    {
                        ALL_Shop_Code_Array = (string[])Application["ALL_Shop_Code_Array"];
                        ALL_Shop_Name_Array = (string[])Application["ALL_Shop_Name_Array"];
                    }

                    //
                    HtmlTable Report_htbl = new HtmlTable();

                    //
                    Report_htbl = new _4e().Creat_HTBL("Dynamic_Content_To_Edit", "1", "1", 1, 0, 0, string.Empty);
                    Dynamic_Control_Holder_pnl.Controls.Add(Report_htbl);

                    Report_htbl.Style.Add("border-collapse", "collapse");
                    Report_htbl.Style.Add("width", "98%");

                    //Excel

                    //
                    string Random_Folder = Guid.NewGuid().ToString();
                    string Random_Folder_Path = Server.MapPath("~/File/" + Random_Folder + "/");

                    new _4e().Creat_Directory(Random_Folder_Path);

                    //
                    string File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                        + "__Page_" + ViewState["Page"].ToString() + "_of_" + ViewState["Total_Page"]
                        + ".xlsx";

                    if (!Is_one_Page)
                    {
                        File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                            + "__ALL"
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
                                    File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                                        + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                                        + "__BUT_NOT__At__" + Time_Start_2.Replace("/", "-")
                                        + "__Page_" + ViewState["Page"].ToString() + "_of_" + ViewState["Total_Page"]
                                        + ".xlsx";
                                }
                                else
                                {
                                    File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                                        + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                                        + "__BUT_NOT__At__" + Time_Start_2.Replace("/", "-")
                                        + "__ALL"
                                        + ".xlsx";
                                }
                            }
                            else
                            {
                                if (Is_one_Page)
                                {
                                    File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                                        + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                                        + "__BUT_NOT__From__" + Time_Start_2.Replace("/", "-") + "__to__" + Time_Finish_2.Replace("/", "-")
                                        + "__Page_" + ViewState["Page"].ToString() + "_of_" + ViewState["Total_Page"]
                                        + ".xlsx";
                                }
                                else
                                {
                                    File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                                        + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                                        + "__BUT_NOT__From__" + Time_Start_2.Replace("/", "-") + "__to__" + Time_Finish_2.Replace("/", "-")
                                        + "__ALL"
                                        + ".xlsx";
                                }
                            }
                        }
                        else
                        {
                            if (Is_one_Page)
                            {
                                File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                                    + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                                    + "__Page_" + ViewState["Page"].ToString() + "_of_" + ViewState["Total_Page"]
                                    + ".xlsx";
                            }
                            else
                            {
                                File_Name = new _4e().Creat_Name_Code(Report_lbl.Text)
                                   + "__From__" + Time_Start_1.Replace("/", "-") + "__to__" + Time_Finish_1.Replace("/", "-")
                                   + "__ALL"
                                   + ".xlsx";
                            }
                        }
                    }

                    //
                    string Folder_Drive = "D:";

                    if (Request.IsLocal)
                    {
                        Folder_Drive = "D:";
                    }

                    //
                    FileInfo File_Info = new FileInfo(Folder_Drive + @"\Websites\Garden\File\" + Random_Folder + "\\" + File_Name);
                    ExcelPackage Excel_Package = new ExcelPackage(File_Info);
                    var Work_sheets = Excel_Package.Workbook.Worksheets.Add(Report_lbl.Text);

                    int Order = 0;
                    Int64 Total_Money = 0;
                    Int64 Total_Point = 0;

                    //
                    string[] Column_Name_Array = new string[0];

                    if (Report_Name_Code == "Add_point")
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

                    //
                    try
                    {
                        while (Sql_Data_Reader.Read())
                        {
                            string Primary_Key = string.Empty;

                            if (Primary_Column != string.Empty)
                            {
                                Primary_Key = Sql_Data_Reader[Primary_Column].ToString();
                            }

                            string Number = Sql_Data_Reader["Number"].ToString();
                            Number = new _4e().Split_Thousand(Number);

                            //
                            bool Valid_1 = true;

                            string NEW_Add_Point_Manual_String = string.Empty;

                            string Type = string.Empty;
                            string Department = string.Empty;
                            string Add_By_Group = string.Empty;

                            string POS = string.Empty;
                            string Cashier = string.Empty;

                            //
                            Card = string.Empty;
                            Name = string.Empty;

                            string Point = string.Empty;
                            Add_Time = string.Empty;

                            string Buy_Time = string.Empty;
                            string Point_Raw = string.Empty;

                            string Add_Point_Manual_String = string.Empty;
                            string Add_Point_Manual_String_Backup = string.Empty;
                            bool Is_Manual_Add_Point = false;

                            Shop = string.Empty;
                            string Shop_Code = string.Empty;
                            string Shop_AND_Code = string.Empty;

                            //
                            string SoHopDong_OK = string.Empty;
                            string TenGianHang_OK = string.Empty;

                            string SoHopDong = string.Empty;
                            string TenGianHang = string.Empty;
                            string SoHopDong_2 = string.Empty;
                            string TenGianHang_2 = string.Empty;
                            string Brand_Nm = string.Empty;

                            string MaThue = string.Empty;

                            string Receipt = string.Empty;
                            string Money = string.Empty;
                            string Money_Raw = string.Empty;

                            //
                            if (Report_Name_Code == "Add_point")
                            {
                                Add_By_Group = Sql_Data_Reader["Add_By_Group"].ToString();

                                POS = Sql_Data_Reader["Add_At"].ToString();
                                Cashier = Sql_Data_Reader["Add_By"].ToString();

                                if (POS == string.Empty)
                                {
                                    POS = Sql_Data_Reader["POS_No"].ToString();
                                }

                                //
                                Card = Sql_Data_Reader["Card"].ToString();
                                Name = Sql_Data_Reader["Name"].ToString();

                                Point = Sql_Data_Reader["Point"].ToString();
                                Add_Time = Sql_Data_Reader["Add_Time"].ToString();

                                Add_Point_Manual_String = Sql_Data_Reader["Add_point_Manual_String"].ToString();
                                Add_Point_Manual_String_Backup = Sql_Data_Reader["Add_point_Manual_String_Backup"].ToString();
                                Is_Manual_Add_Point = new _4e().Convert_String_To_Boolean(Sql_Data_Reader["Is_Manual_Add_Point"].ToString());

                                //
                                if (Add_Point_Manual_String_Backup == string.Empty)
                                {
                                    Add_Point_Manual_String_Backup = Add_Point_Manual_String;
                                }

                                Add_Point_Manual_String = Add_Point_Manual_String.Replace("Vicenzo/Pabini", "Vicenzo-Pabini");

                                //
                                SoHopDong = Sql_Data_Reader["SoHopDong"].ToString();
                                TenGianHang = Sql_Data_Reader["TenGianHang"].ToString();

                                SoHopDong_2 = Sql_Data_Reader["SoHopDong_2"].ToString();
                                TenGianHang_2 = Sql_Data_Reader["TenGianHang_2"].ToString();
                                Brand_Nm = Sql_Data_Reader["Brand_Nm"].ToString();

                                MaThue = Sql_Data_Reader["MaThue"].ToString();

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

                                        if (TenGianHang_2 != string.Empty)
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

                                        if (TenGianHang_2 != string.Empty)
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
                                                Shop_Code = string.Empty;
                                                Receipt = Add_Point_Manual_String_Array[3];
                                                Money = string.Empty;
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

                                                Money = string.Empty;
                                            }

                                            if (Add_Point_Manual_String_Array.Length == 6)
                                            {
                                                Shop = Add_Point_Manual_String_Array[2];
                                                Shop_Code = Add_Point_Manual_String_Array[3];
                                                Receipt = Add_Point_Manual_String_Array[4];
                                                Money = string.Empty;
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
                                        Shop_Code = new _4e().Remove_Space_String(Shop_Code);
                                        Shop_Code = new _4e().Remove_From_String_To_End(Shop_Code, " ");

                                        Shop = new _4e().Remove_Space_String(Shop);

                                        //
                                        if (Shop_Code == string.Empty)
                                        {
                                            Shop_Code = new _4e().Get_From_String_To(Shop, "(", ")");
                                        }

                                        //
                                        if (Shop_Code != string.Empty)
                                        {
                                            Shop_Code = new _4e().Remove_From_String_To_End(Shop_Code, ")");

                                            Shop = new _4e().Remove_From_String_To_End(Shop, "(");
                                        }

                                        if (Shop == string.Empty)
                                        {
                                            if (Shop_Code != string.Empty)
                                            {
                                                Shop = new _4e().Find_Value_In_Two_Array_Start_With(ALL_Shop_Code_Array, ALL_Shop_Name_Array, Shop_Code);
                                            }
                                        }

                                        //
                                        if (Shop == string.Empty)
                                        {
                                            Shop = "XXX";
                                        }

                                        //
                                        if (Shop_Code == string.Empty)
                                        {
                                            Shop_Code = "XXX";
                                        }

                                        Shop_AND_Code = Shop + " (" + Shop_Code + ")";

                                        //
                                        SoHopDong_OK = Shop_Code;
                                        TenGianHang_OK = Shop;

                                        //
                                        if (Receipt == string.Empty)
                                        {
                                            Receipt = "XXX";
                                        }

                                        //
                                        if (Money == string.Empty)
                                        {
                                            Money = (new _4e().Convert_String_To_Int(Point, 0) * 15000).ToString();
                                        }

                                        //
                                        Money = Money.Replace(",", string.Empty).Replace(".", string.Empty);

                                        //
                                        Money_Raw = Money;

                                        //
                                        if ((Money != string.Empty) && (Money != "0") && (new _4e().Check_ID(Money)))
                                        {
                                            Total_Money += new _4e().Convert_String_To_BigInt(Money, 0);
                                            Money = new _4e().Split_Thousand(Money) + " VND";
                                        }

                                        //
                                        Point_Raw = Point;

                                        if ((Point != string.Empty) && (Point != "0") && (new _4e().Check_ID(Point)))
                                        {
                                            Total_Point += new _4e().Convert_String_To_BigInt(Point, 0);
                                            Point = new _4e().Split_Thousand(Point);
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
                                            string Receipt_AND_Money = Add_Point_Manual_String_Array[1];

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
                                            string Receipt_AND_Money = Add_Point_Manual_String_Array[1];

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
                                            string Receipt_AND_Money = Add_Point_Manual_String_Array[2];

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
                                        if ((Money.EndsWith("k")) || (Money.EndsWith("K")))
                                        {
                                            Money = new _4e().Remove_String_Last(Money, "k");
                                            Money = new _4e().Remove_String_Last(Money, "K");

                                            Money = Money + "000";
                                        }

                                        //
                                        if (Receipt == string.Empty)
                                        {
                                            Receipt = "XXX";
                                        }

                                        //
                                        if (Money == string.Empty)
                                        {
                                            Money = (new _4e().Convert_String_To_Int(Point, 0) * 15000).ToString();
                                        }

                                        //
                                        Money = Money.Replace(",", string.Empty).Replace(".", string.Empty);

                                        //
                                        Money_Raw = Money;

                                        //
                                        if ((Money != string.Empty) && (Money != "0") && (new _4e().Check_ID(Money)))
                                        {
                                            Total_Money += new _4e().Convert_String_To_BigInt(Money, 0);
                                            Money = new _4e().Split_Thousand(Money) + " VND";
                                        }

                                        //
                                        Point_Raw = Point;

                                        if ((Point != string.Empty) && (Point != "0") && (new _4e().Check_ID(Point)))
                                        {
                                            Total_Point += new _4e().Convert_String_To_BigInt(Point, 0);
                                            Point = new _4e().Split_Thousand(Point);
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
                                        if ((SoHopDong != string.Empty) && (TenGianHang != string.Empty))
                                        {
                                            SoHopDong_OK = SoHopDong;
                                            TenGianHang_OK = TenGianHang;
                                        }
                                        else
                                            if ((SoHopDong_2 != string.Empty) && (TenGianHang_2 != string.Empty))
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
                                    Buy_Time = Sql_Data_Reader["Buy_Time"].ToString();
                                    Point_Raw = Point;

                                    Receipt = Sql_Data_Reader["Receipt"].ToString();
                                    Money = Sql_Data_Reader["Money"].ToString();
                                    Money_Raw = Money;

                                    //
                                    if ((Money != string.Empty) && (Money != "0"))
                                    {
                                        Money = new _4e().Split_Thousand(Money) + " VND";
                                    }

                                    Point = new _4e().Split_Thousand(Point);
                                }
                            }

                            //
                            if (j1 == 0)
                            {
                                HtmlTableRow TR = new HtmlTableRow();

                                TR = new _4e().Creat_HTR(string.Empty);
                                Report_htbl.Rows.Add(TR);

                                //
                                Work_sheets.Cells[1, 3].Value = Report_lbl.Text;
                                Work_sheets.Cells[1, 3, 1, 9].Merge = true;

                                Work_sheets.Cells[1, 3].Style.Font.Color.SetColor(Color.Red);
                                Work_sheets.Cells[1, 3].Style.Font.Bold = true;
                                Work_sheets.Cells[1, 3].Style.Font.Size = 20;

                                //
                                if (Filter_Time_tr.Visible)
                                {
                                    if (Filter_Time_2_tr.Visible)
                                    {
                                        if (Time_Start_2 == Time_Finish_2)
                                        {
                                            if (Is_one_Page)
                                            {
                                                Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- At: " + Time_Start_2 + " (Page " + new _4e().Split_Thousand(ViewState["Page"].ToString()) + " of " + new _4e().Split_Thousand(ViewState["Total_Page"].ToString()) + ")";
                                            }
                                            else
                                            {
                                                Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- At: " + Time_Start_2 + " (ALL)";
                                            }
                                        }
                                        else
                                        {
                                            if (Is_one_Page)
                                            {
                                                Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- FROM: " + Time_Start_2 + " - to - " + Time_Finish_2 + " (Page " + new _4e().Split_Thousand(ViewState["Page"].ToString()) + " of " + new _4e().Split_Thousand(ViewState["Total_Page"].ToString()) + ")";
                                            }
                                            else
                                            {
                                                Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " --- BUT NOT --- FROM: " + Time_Start_2 + " - to - " + Time_Finish_2 + " (ALL)";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Is_one_Page)
                                        {
                                            Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " (Page " + new _4e().Split_Thousand(ViewState["Page"].ToString()) + " of " + new _4e().Split_Thousand(ViewState["Total_Page"].ToString()) + ")";
                                        }
                                        else
                                        {
                                            Work_sheets.Cells[3, 3].Value = "FROM: " + Time_Start_1 + " - to - " + Time_Finish_1 + " (ALL)";
                                        }
                                    }
                                }
                                else
                                {
                                    if (Is_one_Page)
                                    {
                                        Work_sheets.Cells[3, 3].Value = "(Page " + new _4e().Split_Thousand(ViewState["Page"].ToString()) + " of " + new _4e().Split_Thousand(ViewState["Total_Page"].ToString()) + ")";
                                    }
                                    else
                                    {
                                        Work_sheets.Cells[3, 3].Value = "(ALL)";
                                    }
                                }

                                Work_sheets.Cells[3, 3, 3, 9].Merge = true;

                                Work_sheets.Cells[3, 3].Style.Font.Color.SetColor(Color.Blue);
                                Work_sheets.Cells[3, 3].Style.Font.Bold = true;

                                //
                                if (Report_Name_Code == "Add_point")
                                {
                                    for (int x1 = 1; x1 <= Column_Name_Array.Length; x1++)
                                    {
                                        //Order
                                        HtmlTableCell TD_1 = new _4e().Creat_HTH(string.Empty, "0", "0", "center", "middle", 0, string.Empty, false, false, string.Empty);
                                        TR.Cells.Add(TD_1);
                                        TD_1.Controls.Add(new _4e().Creat_Label_ltr("<span class='Bold_Red_Text_css'>" + Column_Name_Array[x1 - 1] + "</span>"));

                                        Work_sheets.Cells[5, x1].Value = Column_Name_Array[x1 - 1];

                                        Work_sheets.Cells[5, x1].Style.Font.Color.SetColor(Color.Red);
                                        Work_sheets.Cells[5, x1].Style.Font.Bold = true;
                                        Work_sheets.Cells[5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    }
                                }
                                else
                                {
                                    //
                                    for (int x1 = 0; x1 < Total_Column; x1++)
                                    {
                                        string Column_Name = Sql_Data_Reader.GetName(x1).ToString();

                                        HtmlTableCell TD_2 = new _4e().Creat_HTD(string.Empty, "0", "0", "center", "middle", 0, string.Empty, false, false, string.Empty);
                                        TR.Cells.Add(TD_2);
                                        TD_2.Controls.Add(new _4e().Creat_Label_ltr("<span class='Bold_Red_Text_css'>" + Column_Name.Replace("_", " ") + "</span>"));

                                        //
                                        Work_sheets.Cells[5, 1 + x1].Value = Column_Name;

                                        Work_sheets.Cells[5, 1 + x1].Style.Font.Color.SetColor(Color.Red);
                                        Work_sheets.Cells[5, 1 + x1].Style.Font.Bold = true;
                                        Work_sheets.Cells[5, 1 + x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
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
                                TR_2 = new _4e().Creat_HTR(Primary_Key);
                                Report_htbl.Rows.Add(TR_2);

                                if (((Order - 1) % 2) == 0)
                                {
                                    TR_2.BgColor = "#C0C0C0";
                                }

                                //
                                if (Report_Name_Code == "Add_point")
                                {
                                    object[] Column_Value_Array = new object[0];

                                    if (UserName.ToLower() == "Pos2".ToLower())
                                    {
                                        Column_Value_Array = new object[] { Name + "<br/>" + Card, TenGianHang_OK + "<br/>" + SoHopDong_OK, Money, Point, Receipt, Buy_Time, Add_Time, POS, Cashier };
                                        //Column_Name_Array = new string[] { "Khách hàng", "Shop", "Tiền", "Điểm", "Hóa đơn", "Ngày mua", "Ngày tích", "POS", "Tài khoản" };
                                    }
                                    else
                                    {
                                        Column_Value_Array = new object[] { Primary_Key, Order.ToString(), Type, Department, Card, Name, TenGianHang_OK, SoHopDong_OK, Money, Point, Receipt, Cashier, Add_Time, POS, Buy_Time, Add_Point_Manual_String };
                                        Column_Name_Array = new string[] { "ID", "Number", "Type", "Department", "Card", "Name", "Shop", "Shop Code", "Money", "Point", "Receipt", "Add By", "Add Time", "Add At", "Buy Time", "Reason" };
                                    }

                                    //
                                    if ((Is_one_Page) || (Order <= 1000))
                                    {
                                        for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                        {
                                            string TD_Align = "center";

                                            if ((Column_Name_Array[x1 - 1] == "Money") || (Column_Name_Array[x1 - 1] == "Point"))
                                            {
                                                TD_Align = "right";
                                            }

                                            HtmlTableCell TD_2 = new _4e().Creat_HTD(string.Empty, "0", "0", TD_Align, "middle", 0, string.Empty, false, false, string.Empty);
                                            TR_2.Cells.Add(TD_2);
                                            TD_2.Controls.Add(new _4e().Creat_Label_ltr(new _4e().Object_ToString(Column_Value_Array[x1 - 1])));
                                        }
                                    }

                                    //
                                    for (int x1 = 1; x1 <= Column_Value_Array.Length; x1++)
                                    {
                                        if (x1 == 9)
                                        {
                                            Work_sheets.Cells[Order + 5, 9].Value = new _4e().Convert_String_To_BigInt(Money_Raw, 0);
                                        }
                                        else
                                            if (x1 == 10)
                                        {
                                            Work_sheets.Cells[Order + 5, 10].Value = new _4e().Convert_String_To_BigInt(Point_Raw, 0);
                                        }
                                        else
                                        {
                                            Work_sheets.Cells[Order + 5, x1].Value = Column_Value_Array[x1 - 1];
                                        }

                                        //
                                        if ((x1 == 1) || (x1 == 2) || (x1 == 3) || (x1 == 4))
                                        {
                                            Work_sheets.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                            if ((x1 == 9) || (x1 == 10))
                                        {
                                            Work_sheets.Cells[Order + 5, x1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                        }
                                    }
                                }
                                else
                                {
                                    //
                                    for (int x1 = 0; x1 < Total_Column; x1++)
                                    {
                                        string Column_Name = Sql_Data_Reader.GetName(x1).ToString();
                                        string Data_Content = Sql_Data_Reader[Column_Name].ToString();
                                        string Data_Content_Raw = string.Empty;

                                        //
                                        if (Column_Name == "Number")
                                        {
                                            Data_Content_Raw = Order.ToString();
                                            Data_Content = Order.ToString();
                                        }
                                        else
                                            if (Column_Name == "Money")
                                        {
                                            Data_Content_Raw = Data_Content;

                                            if ((Data_Content != string.Empty) && (Data_Content != "0"))
                                            {
                                                Data_Content = new _4e().Split_Thousand(Data_Content) + " VND";
                                            }
                                        }
                                        else
                                                if (Column_Name == "Phone")
                                        {
                                            Data_Content_Raw = Data_Content;

                                            if ((Data_Content != string.Empty) && (Data_Content != "0"))
                                            {
                                                string URL = Request.Url.ToString();

                                                URL = new _4e().Remove_Query_String(URL, "Page");
                                                URL = new _4e().Remove_Query_String(URL, "ALL");

                                                URL = new _4e().Update_Query_String(URL, "Phone", HttpUtility.UrlEncode(Data_Content));

                                                URL = new _4e().Update_Query_String(URL, "Time_Start_1", HttpUtility.UrlEncode("01/01/2000"));
                                                URL = new _4e().Update_Query_String(URL, "Time_Finish_1", HttpUtility.UrlEncode("31/12/2099"));

                                                Data_Content = "<a target='_blank' href='" + URL + "'>" + Data_Content + "</a>";
                                            }
                                        }
                                        else
                                                    if (Column_Name == "Email")
                                        {
                                            Data_Content_Raw = Data_Content;

                                            if ((Data_Content != string.Empty) && (Data_Content != "0"))
                                            {
                                                string URL = Request.Url.ToString();

                                                URL = new _4e().Remove_Query_String(URL, "Page");
                                                URL = new _4e().Remove_Query_String(URL, "ALL");

                                                URL = new _4e().Update_Query_String(URL, "Email", HttpUtility.UrlEncode(Data_Content));

                                                URL = new _4e().Update_Query_String(URL, "Time_Start_1", HttpUtility.UrlEncode("01/01/2000"));
                                                URL = new _4e().Update_Query_String(URL, "Time_Finish_1", HttpUtility.UrlEncode("31/12/2020"));

                                                Data_Content = "<a target='_blank' href='" + URL + "'>" + Data_Content + "</a>";
                                            }
                                        }
                                        else
                                                        if (new _4e().Check_Exists_In_Array(Point_Column_Name_Array, Column_Name))
                                        {
                                            Data_Content_Raw = Data_Content;
                                            Data_Content = new _4e().Split_Thousand(Data_Content);
                                        }

                                        //
                                        if (Report_Name_Code == "Analys_transaction")
                                        {
                                            if (Column_Name == "Start_Date")
                                            {
                                                Data_Content = new _4e().Remove_String_Last(Data_Content, " 00:00:00");
                                            }
                                        }

                                        if (Report_Name_Code == "Member_list")
                                        {
                                            if (Column_Name == "Number")
                                            {
                                                Card = Sql_Data_Reader["Card"].ToString();

                                                Data_Content_Raw = Data_Content;
                                                Data_Content = "<input type='checkbox' name='Switch_Point_cbxl' value='" + Card + "'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type='radio' name='Switch_Point_rdol' value='" + Card + "'>";
                                            }
                                        }

                                        //
                                        if (Report_Name_Code == "FC_History")
                                        {
                                            if (Column_Name == "Hang_Hoa")
                                            {
                                                Data_Content = new _4e().Remove_String_Last(Data_Content, ",");
                                                string Thao_Tac = Data_Content;

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
                                                string Hinh_Thuc_Thanh_Toan = Data_Content;

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
                                                string Hinh_Thuc_Thanh_Toan = Sql_Data_Reader["Hinh_Thuc"].ToString();

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
                                            HtmlTableCell TD_22 = new _4e().Creat_HTD(string.Empty, "0", "0", "center", "middle", 0, string.Empty, false, false, string.Empty);
                                            TR_2.Cells.Add(TD_22);
                                            TD_22.Controls.Add(new _4e().Creat_Label_ltr(Data_Content));
                                        }

                                        //
                                        Work_sheets.Cells[Order + 5, 1 + x1].Value = Data_Content;

                                        if (Column_Name == "Number")
                                        {
                                            Work_sheets.Cells[Order + 5, 1 + x1].Value = Data_Content_Raw;
                                            Work_sheets.Cells[Order + 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                            if ((Column_Name == "Phone") || (Column_Name == "Email"))
                                        {
                                            Work_sheets.Cells[Order + 5, 1 + x1].Value = Data_Content_Raw;
                                            Work_sheets.Cells[Order + 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        }
                                        else
                                                if ((Column_Name == "Money") || (new _4e().Check_Exists_In_Array(Point_Column_Name_Array, Column_Name)))
                                        {
                                            Work_sheets.Cells[Order + 5, 1 + x1].Value = new _4e().Convert_String_To_BigInt(Data_Content_Raw, 0);
                                            Work_sheets.Cells[Order + 5, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                        }
                                    }
                                }
                            }

                            //
                            j1++;
                        }
                    }
                    catch (SqlException Sql_Exception)
                    {
                    }

                    if (!Sql_Data_Reader.IsClosed)
                    {
                        Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
                    }

                    //
                    if (Report_Name_Code == "Add_point")
                    {
                        for (int x1 = 1; x1 <= Column_Name_Array.Length; x1++)
                        {
                            Work_sheets.Column(x1).AutoFit();
                        }
                    }
                    else
                        for (int x1 = 0; x1 < Total_Column; x1++)
                        {
                            Work_sheets.Column(1 + x1).AutoFit();
                        }

                    //Save
                    Excel_Package.Save();

                    if (!Is_one_Page)
                    {
                        On_Page_Load += " window.location.href = '" + new _4e().Add_http_To_URL(new _4e().Read_Domain(string.Empty) + "/File/" + Random_Folder + "/" + File_Info.Name) + "';";
                    }
                }
            }
        }

        //
        Change_Page_1_lbl.Text = Change_Page;
        Change_Page_2_lbl.Text = Change_Page_1_lbl.Text;
    }

    protected void Creat_Sql_Join_Card_FC(int Month, int Year, out string Sql_Join_HD, out string Sql_Join_CTHD)
    {
        string Thang_Nam = string.Empty;

        //
        if (Month < 10)
        {
            Thang_Nam = "0" + Month.ToString() + Year;
        }
        else
        {
            Thang_Nam = Month.ToString() + Year;
        }

        //
        Sql_Join_HD = string.Empty;
        Sql_Join_CTHD = string.Empty;

        //
        if (new _4e().Check_Exists_Table_POS("TOPOS_DB", "HoaDon" + Thang_Nam))
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

    private void Read_FoodCourtCard_Info(string MaFoodCourt)
    {
        try
        {
            Money_Current_lbl.Text = string.Empty;
            Money_Useable_lbl.Text = string.Empty;
            Money_Withdrawal_Able_lbl.Text = string.Empty;

            State_lbl.Text = string.Empty;

            //
            string Sql_Query = string.Empty;
            string Sql_Join = string.Empty;

            string Sql_Where = string.Empty;
            string Sql_Order = string.Empty;

            string Column_List_1 = string.Empty;
            string Column_List_2 = string.Empty;

            string SoTien = string.Empty;
            string SoTien_Lock = string.Empty;

            string SoTien_Co_The_Dung = string.Empty;
            string SoTien_Co_The_Rut = string.Empty;

            string TrangThai = string.Empty;
            string NgayHetHan = string.Empty;

            //
            Sql_Query =

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

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Open_Sql_Connection_FC(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@MaFoodCourt", MaFoodCourt);

            SqlDataReader Sql_Data_Reader = Sql_Command.ExecuteReader();

            try
            {
                if (Sql_Data_Reader.Read())
                {
                    SoTien = Sql_Data_Reader["SoTien"].ToString();
                    SoTien_Lock = Sql_Data_Reader["SoTien_Lock"].ToString();

                    TrangThai = Sql_Data_Reader["TrangThai"].ToString();
                    NgayHetHan = Sql_Data_Reader["NgayHetHan"].ToString();

                    SoTien_Co_The_Rut = Sql_Data_Reader["SoTien_Co_The_Rut"].ToString();
                }
            }
            catch (SqlException Sql_Exception)
            {
            }

            if (!Sql_Data_Reader.IsClosed)
            {
                Sql_Data_Reader.Dispose(); Sql_Command.Dispose();
            }

            //
            SoTien = new _4e().Remove_String_Last(SoTien, ".00");
            SoTien_Lock = new _4e().Remove_String_Last(SoTien_Lock, ".00");
            SoTien_Co_The_Rut = new _4e().Remove_String_Last(SoTien_Co_The_Rut, ".00");

            if ((SoTien_Lock != "0") && (SoTien_Lock != string.Empty))
            {
                TrangThai = "<span style='color: red; font-weight: bold;'>Đã khóa (Số tiền lúc Khóa: " + new _4e().Split_Thousand(SoTien_Lock) + ")</span>";
            }
            else
            if ((TrangThai == string.Empty) || (TrangThai == "0"))
            {
                TrangThai = "Chưa kích hoạt";
            }
            else
            {
                TrangThai = "Đã kích hoạt";
            }

            Int64 SoTien_int = new _4e().Convert_String_To_BigInt(SoTien, 0);

            if (SoTien_int > 10000)
            {
                SoTien_Co_The_Dung = (SoTien_int - 10000).ToString();
            }
            else
            {
                SoTien_Co_The_Dung = "0";
            }

            //
            NgayHetHan = new _4e().Remove_String_Last(NgayHetHan, " 12:00:00 AM");
            NgayHetHan = new _4e().Remove_String_Last(NgayHetHan, " 00:00:00");

            string[] NgayHetHan_Array = NgayHetHan.Split('/');

            if (NgayHetHan_Array.Length == 3)
            {
                NgayHetHan = NgayHetHan_Array[1] + "/" + NgayHetHan_Array[0] + "/" + NgayHetHan_Array[2];
            }

            //
            SoTien = new _4e().Split_Thousand(SoTien);
            SoTien_Co_The_Dung = new _4e().Split_Thousand(SoTien_Co_The_Dung);
            SoTien_Co_The_Rut = new _4e().Split_Thousand(SoTien_Co_The_Rut);

            //
            Money_Current_lbl.Text = "Tài khoản hiện có: " + SoTien;
            Money_Useable_lbl.Text = " --- Có thể dùng: " + SoTien_Co_The_Dung;
            Money_Withdrawal_Able_lbl.Text = " --- Có thể rút: " + SoTien_Co_The_Rut;

            if (NgayHetHan != string.Empty)
            {
                State_lbl.Text = "<br/>Trạng thái: " + TrangThai + " --- Hết hạn: " + NgayHetHan;
            }
            else
            {
                State_lbl.Text = "<br/>Trạng thái: " + TrangThai;
            }
        }
        catch (Exception EX)
        {
        }
    }

    //OK
    protected bool Check_Object_Is_True(object Object)
    {
        bool Result = false;

        if (Object != null)
        {
            if ((Object.ToString().ToLower() == "true") || (Object.ToString().ToLower() == "1"))
            {
                Result = true;
            }
        }

        return Result;
    }
    protected void Ajax_ScriptReport_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
    {
        Ajax_ScriptManager.AsyncPostBackErrorMessage = e.Exception.Message;
    }
    protected void Report_btn_Click(object sender, EventArgs e)
    {
        Submit_btn_Click("0");
    }
    protected void Download_Excel_All_btn_Click(object sender, ImageClickEventArgs e)
    {
        Submit_btn_Click("1");
    }

    protected void Submit_btn_Click(string ALL)
    {
        //Date_Time_1
        string Time_Start_1 = Time_Start_1_tbx.Text.Replace("__/__/____", string.Empty);
        string Time_Finish_1 = Time_Finish_1_tbx.Text.Replace("__/__/____", string.Empty);

        Time_Start_1 = new _4e().Convert_Date_String_To_String(Time_Start_1, string.Empty);
        Time_Finish_1 = new _4e().Convert_Date_String_To_String(Time_Finish_1, string.Empty);

        //
        if (Time_Start_1 == string.Empty)
        {
            Time_Start_1 = "01/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Start_1_tbx.Text = Time_Start_1;
        }

        //
        if (Time_Finish_1 == string.Empty)
        {
            Time_Finish_1 = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Finish_1_tbx.Text = Time_Finish_1;
        }

        //Date_Time_2
        string Time_Start_2 = Time_Start_2_tbx.Text.Replace("__/__/____", string.Empty);
        string Time_Finish_2 = Time_Finish_2_tbx.Text.Replace("__/__/____", string.Empty);

        Time_Start_2 = new _4e().Convert_Date_String_To_String(Time_Start_2, string.Empty);
        Time_Finish_2 = new _4e().Convert_Date_String_To_String(Time_Finish_2, string.Empty);

        //
        if (Time_Start_2 == string.Empty)
        {
            Time_Start_2 = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Start_2_tbx.Text = Time_Start_2;
        }

        //
        if (Time_Finish_2 == string.Empty)
        {
            Time_Finish_2 = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            Time_Finish_2_tbx.Text = Time_Finish_2;
        }

        //
        URL = Request.Url.ToString();

        URL = new _4e().Remove_Query_String(URL, "Onload");
        URL = new _4e().Remove_Query_String(URL, "Page");
        URL = new _4e().Update_Query_String(URL, "ALL", ALL);

        URL = new _4e().Update_Query_String(URL, "Time_Start_1", HttpUtility.UrlEncode(Time_Start_1));
        URL = new _4e().Update_Query_String(URL, "Time_Finish_1", HttpUtility.UrlEncode(Time_Finish_1));

        URL = new _4e().Update_Query_String(URL, "Time_Start_2", HttpUtility.UrlEncode(Time_Start_2));
        URL = new _4e().Update_Query_String(URL, "Time_Finish_2", HttpUtility.UrlEncode(Time_Finish_2));

        URL = new _4e().Update_Query_String(URL, "Shop", HttpUtility.UrlEncode(Shop_tbx.Text));
        URL = new _4e().Update_Query_String(URL, "Card", HttpUtility.UrlEncode(Card_tbx.Text));
        URL = new _4e().Update_Query_String(URL, "Name", HttpUtility.UrlEncode(Name_tbx.Text));
        URL = new _4e().Update_Query_String(URL, "Phone", HttpUtility.UrlEncode(Phone_tbx.Text));
        URL = new _4e().Update_Query_String(URL, "Email", HttpUtility.UrlEncode(Email_tbx.Text));
        URL = new _4e().Update_Query_String(URL, "Address", HttpUtility.UrlEncode(Address_tbx.Text));

        URL = new _4e().Update_Query_String(URL, "Add_By_User", HttpUtility.UrlEncode(Add_By_User_tbx.Text));

        URL = new _4e().Update_Query_String(URL, "Card_Actived_OR_Disabled", Card_Actived_OR_Disabled_rdol.SelectedValue);
        URL = new _4e().Update_Query_String(URL, "RDOL_1", Filter_RDOL_1.SelectedValue);
        URL = new _4e().Update_Query_String(URL, "RDOL_2", Filter_RDOL_2.SelectedValue);

        URL = new _4e().Update_Query_String(URL, "CBXL_1", new _4e().Determine_CBXL_Selected(Filter_CBXL_1, "value"));

        URL = new _4e().Update_Query_String(URL, "ReActive_Card", new _4e().Convert_Boolean_To_Bit_String(ReActive_Card_cbx.Checked));

        URL = new _4e().Update_Query_String(URL, "Filter_Time_3", HttpUtility.UrlEncode(Filter_Time_3_tbx.Text));

        Response.Redirect(URL);
    }

    //
    [WebMethod(enableSession: true)]
    public static string Submit_Switch_Multi_Point(string Card_1_List, string Card_2)
    {
        return new _4e().Submit_Switch_Multi_Point(Card_1_List, Card_2);
    }

    protected void FC_Block_btn_Click(object sender, EventArgs e)
    {
        string Card = Card_tbx.Text;
        Card = new _4e().Remove_Space_String(Card);

        if (Card.Length == 11)
        {
            FC_Block_btn.Enabled = false;
            FC_UnBlock_btn.Enabled = false;

            string Sql_Query =

                " UPDATE [TOPOS_DB].[dbo].[TTT_DanhMuc]"
                + " SET SoTien_Lock = SoTien, SoTien = 0"//TrangThai = 0
                + " WHERE ((MaFoodCourt = @MaFoodCourt) AND (SoTien > 0) AND ((SoTien_Lock = 0) OR (SoTien_Lock IS NULL)))"
                ;

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Open_Sql_Connection_FC(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@MaFoodCourt", Card);
            Sql_Command.ExecuteNonQuery();

            //
            Read_FoodCourtCard_Info(Card);

            Message_lbl.Text = "Đã KHÓA thẻ: " + Card;

            FC_UnBlock_btn.Enabled = true;
        }
    }

    protected void FC_UnBlock_btn_Click(object sender, EventArgs e)
    {
        string Card = Card_tbx.Text;
        Card = new _4e().Remove_Space_String(Card);

        if (Card.Length == 11)
        {
            FC_Block_btn.Enabled = false;
            FC_UnBlock_btn.Enabled = false;

            string Sql_Query =

                " UPDATE [TOPOS_DB].[dbo].[TTT_DanhMuc]"
                + " SET SoTien = SoTien_Lock, SoTien_Lock = 0"//TrangThai = 1
                + " WHERE ((MaFoodCourt = @MaFoodCourt) AND (SoTien_Lock > 0) AND ((SoTien = 0) OR (SoTien IS NULL)))"
                ;

            Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
            Open_Sql_Connection_FC(); Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

            Sql_Command.Parameters.Add("@MaFoodCourt", Card);
            //Sql_Command.ExecuteNonQuery();

            //
            Read_FoodCourtCard_Info(Card);

            Message_lbl.Text = "Đã MỞ thẻ: " + Card;

            FC_Block_btn.Enabled = true;
        }
    }

    protected void SAP_btn_Click(object sender, EventArgs e)
    {
        string MaSAP = Ma_SAP_tbx.Text;
        string SoHopDong = So_Hop_Dong_tbx.Text;

        MaSAP = new _4e().Remove_Space_String(MaSAP);
        SoHopDong = new _4e().Remove_Space_String(SoHopDong);

        Ma_SAP_tbx.Enabled = false;
        So_Hop_Dong_tbx.Enabled = false;
        SAP_btn.Enabled = false;

        string Sql_Query =

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

        new _4e().Write_To_File(@"D:\MaSAP.txt", MaSAP + " : " + SoHopDong);

        Sql_Query = new _4e().Check_Sql_Query(Sql_Query);
        Sql_Command = new SqlCommand(Sql_Query, Sql_Connection); Sql_Command.CommandTimeout = 0;

        Sql_Command.Parameters.Add("@MaSAP", MaSAP);
        Sql_Command.Parameters.Add("@SoHopDong", SoHopDong);

        Sql_Command.ExecuteNonQuery();

        //
        Message_lbl.Text = "Đã add thành công mã SAP: " + MaSAP + " và Số hợp đồng: " + SoHopDong;
    }
}