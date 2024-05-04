function Check_Is_Mobile() {

    //(function(a){jQuery.browser.mobile=/android.+mobile|avantgo|bada/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)/|plucker|pocket|psp|symbian|treo|up.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw-(n|u)|c55/|capi|ccwa|cdm-|cell|chtm|cldc|cmd-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc-s|devi|dica|dmob|do(c|p)o|ds(12|-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(-|_)|g1 u|g560|gene|gf-5|g-mo|go(.w|od)|gr(ad|un)|haie|hcit|hd-(m|p|t)|hei-|hi(pt|ta)|hp( i|ip)|hs-c|ht(c(-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i-(20|go|ma)|i230|iac( |-|/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |/)|klon|kpt |kwc-|kyo(c|k)|le(no|xi)|lg( g|/(k|l|u)|50|54|e-|e/|-[a-w])|libw|lynx|m1-w|m3ga|m50/|ma(te|ui|xo)|mc(01|21|ca)|m-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|-([1-8]|c))|phil|pire|pl(ay|uc)|pn-2|po(ck|rt|se)|prox|psio|pt-g|qa-a|qc(07|12|21|32|60|-[2-7]|i-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55/|sa(ge|ma|mm|ms|ny|va)|sc(01|h-|oo|p-)|sdk/|se(c(-|0|1)|47|mc|nd|ri)|sgh-|shar|sie(-|m)|sk-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h-|v-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl-|tdg-|tel(i|m)|tim-|t-mo|to(pl|sh)|ts(70|m-|m3|m5)|tx-9|up(.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(-|2|g)|yas-|your|zeto|zte-/i.test(a.substr(0,4))})(navigator.userAgent||navigator.vendor||window.opera);

    var Browser = '';

    if (navigator.userAgent) {
        Browser = navigator.userAgent.toLowerCase();
    }
    else
        if (navigator.platform) {
            Browser = navigator.platform.toLowerCase();
        }
        else
            if (navigator.vendor) {
                Browser = navigator.vendor.toLowerCase();
            }
            else
                if (window.opera) {
                    Browser = window.opera.toLowerCase();
                }

    //
    var agents = ['android', 'webos', 'iphone', 'ipad', 'ipod', 'BlackBerry', 'IEMobile', 'Opera Mini', 'windows phone'];

    //
    for (i in agents) {

        if (Check_String_Contain(Browser, agents[i].toLowerCase())) {
            return true;
        }
    }

    return false;
}

function Creat_DDL_Text(DDL_Text_ID, Valid_Test_Drive, Any_Select_Test_Drive_Title) {

    Hide_Element(DDL_Text_ID + '_tr');
    Hide_Element(DDL_Text_ID);
    $('#' + DDL_Text_ID).empty();

    var Test_Drive_Array = Split_List_To_Array(Valid_Test_Drive);

    if (Test_Drive_Array != null) {

        Display_Element(DDL_Text_ID + '_tr');
        Display_Element(DDL_Text_ID);

        if (Any_Select_Test_Drive_Title != null) {
            Add_Item_To_DDL(DDL_Text_ID, Any_Select_Test_Drive_Title, '0');
        }

        for (var i = 0; i < Test_Drive_Array.length; i++) {

            Add_Item_To_DDL(DDL_Text_ID, Test_Drive_Array[i], Test_Drive_Array[i]);
        }
    }
}



function Creat_Sex_ddl(Sex_ddl_ID, Valid_Sex, Any_Select_Sex_Title) {

    Hide_Element(Sex_ddl_ID + '_tr');
    Hide_Element(Sex_ddl_ID);
    $('#' + Sex_ddl_ID).empty();

    if (Valid_Sex == null) {

        Display_Element(Sex_ddl_ID + '_tr');
        Display_Element(Sex_ddl_ID);

        if (Any_Select_Sex_Title != null) {
            Add_Item_To_DDL(Sex_ddl_ID, Any_Select_Sex_Title, '0');
        }

        Add_Item_To_DDL(Sex_ddl_ID, 'Nam', 1);
        Add_Item_To_DDL(Sex_ddl_ID, 'Nữ', 0);
    }
    else {

        Display_Element(Sex_ddl_ID + '_tr');
        Display_Element(Sex_ddl_ID);

        if (Any_Select_Sex_Title != null) {
            Add_Item_To_DDL(Sex_ddl_ID, Any_Select_Sex_Title, '0');
        }

        //
        if (Check_String_Contain(Valid_Sex, '#1#')) {
            Add_Item_To_DDL(Sex_ddl_ID, 'Nam', 1);
        }

        //
        if (Check_String_Contain(Valid_Sex, '#0#')) {
            Add_Item_To_DDL(Sex_ddl_ID, 'Nữ', 0);
        }
    }
}

function Creat_Job_ddl(Job_ddl_ID, Valid_Job_ID, Any_Select_Job_Title) {

    if (Check_Element_Is_Not_Null(Job_ddl_ID)) {

        Hide_Element(Job_ddl_ID + '_tr');
        Hide_Element(Job_ddl_ID);
        $('#' + Job_ddl_ID).empty();

        //
        PageMethods.set_path($('#PageMethods_Path_hdf').val());
        PageMethods.Creat_Job(Valid_Job_ID, Creat_Job_Sucessfull, Creat_Job_Error);
    }

    function Creat_Job_Sucessfull(Response) {

        //alert(Response);

        var JSON_Array = Parse_JSON_String_To_Array(Response);

        if (JSON_Array != null) {

            if (JSON_Array.length > 0) {

                Display_Element(Job_ddl_ID + '_tr');
                Display_Element(Job_ddl_ID);

                if (Any_Select_Job_Title != null) {
                    Add_Item_To_DDL(Job_ddl_ID, Any_Select_Job_Title, '0');
                }

                for (var i = 0; i < JSON_Array.length; i++) {
                    Add_Item_To_DDL(Job_ddl_ID, JSON_Array[i].Item_2, JSON_Array[i].Item_1);
                }
            }
        }
    }

    function Creat_Job_Error(Response) {

        if (Response != null) {
            Alert_Message_PageMethods_Error(Response);
        }
    }
}


function Check_Highslide_Is_Expanded() {

    var Result = false;

    for (var i = 0; i < hs.expanders.length; i++) {

        if (hs.expanders[i]
			&& !(this.last && this.transitions[1] == 'crossfade')) {
            Result = true;
            break;
        }
    }

    return Result;
}

function Get_Key_Pressed(event) {

    var Key_Pressed;

    if (event.keyCode) {
        Key_Pressed = event.keyCode;
    }
    else {
        Key_Pressed = event.which;
    }

    return Key_Pressed;
}

function Setup_TBX_Submit(TBX_ID, BTN_ID) {

    $('#' + TBX_ID).bind('keydown', function (event) {

        if (Get_Key_Pressed(event) == 13) {
            $('#' + BTN_ID).click(); return false;
        }
    });
}

function Disable_TBX_Auto_Submit(TBX_ID) {

    $('#' + TBX_ID).bind('keyup keydown', function (event) {

        if (Get_Key_Pressed(event) == 13) {
            return false;
        }
        else {
            return true;
        }
    });
}

function Shop_Search_tbx_OnChange(event) {

    var Valid = true;

    if (Get_Key_Pressed(event) == 13) {

        var Valid = false;

        var Keyword = $('#Shop_Search_tbx').val();

        if (Keyword == 'Tìm Kiếm Sản Phẩm, Khuyến Mại, Công Ty, Mã Số...') {
            Clear_Watermarked_TBX('Shop_Search_tbx');
            Keyword = '';
        }

        if ((Keyword.length >= 2) && (Keyword.length <= 128)) {

            Valid = true;
            __doPostBack('Shop_Search_btn', '');
        }
        else {
            Alert_Message('Phải nhập Từ Khóa Tìm Kiếm có ít nhất 2 ký tự !');

            if (Keyword.length == 0) {
                Clear_Watermarked_TBX('Shop_Search_tbx');
            }
        }
    }

    return Valid;
}



function Shop_Search_btn_OnClientClick() {

    var Valid = false;

    var Keyword = $('#Shop_Search_tbx').val();

    if (Keyword == 'Tìm Kiếm Sản Phẩm, Khuyến Mại, Công Ty, Mã Số...') {
        Clear_Watermarked_TBX('Shop_Search_tbx');
        Keyword = '';
    }

    if ((Keyword.length >= 2) && (Keyword.length <= 128)) {

        Valid = true;
        __doPostBack('Shop_Search_btn', '');
    }
    else {
        Alert_Message('Phải nhập Từ Khóa Tìm Kiếm có ít nhất 2 ký tự !');

        if (Keyword.length == 0) {
            Clear_Watermarked_TBX('Shop_Search_tbx');
        }
    }

    return Valid;
}


function Iframe_Onload(Iframe_For) {

    try {

        Iframe_Onload_Function(Iframe_For, $('#Page_Content_div').height());
    }
    catch (e) {
    }
}

function Iframe_Onload_Function(Iframe_For, Iframe_offsetHeight) {

    if (Check_Element_Is_Not_Null('Loading_' + Iframe_For + '_Iframe_tbl')) {

        Hide_Element('Loading_' + Iframe_For + '_Iframe_tbl');

        $('#' + Iframe_For + '_ifr').css('height', Iframe_offsetHeight + 'px');
    }
    else {
        parent.Iframe_Onload_Function(Iframe_For, Iframe_offsetHeight);
    }
}

function Facebook_Onload(d, s, id) {

    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;

    js = d.createElement(s); js.id = id;
    js.src = 'http://connect.facebook.net/vi_VN/all.js#xfbml=1';
    fjs.parentNode.insertBefore(js, fjs);
}


function Check_Loaded_PageMethods(PageMethods_For) {
    return Check_HDF('Loaded_PageMethods_' + PageMethods_For + '_hdf');
}

function Enable_Order_List(List_ID) {
    $('#' + List_ID).sortable();
    $('#' + List_ID).disableSelection();
}

function Add_innerHTML_To_Current(ID, HTML) {
    $('#' + ID).html($('#' + ID).html() + HTML);
}

function Replace_Content_From_HDF(Content_For) {
    if ((Check_Element_Is_Not_Null(Content_For + '_hdf')) && (Check_Element_Is_Not_Null(Content_For + '_lbl'))) {
        $('#' + Content_For + '_lbl').html($('#' + Content_For + '_hdf').val());
    }
}

function Creat_UUID() {
    var d = new Date().getTime();

    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
    });

    return uuid;
}

function Save_Player_Sound_On(value) {

    var c_name = 'Player_Sound_On';

    var exdate = new Date();
    exdate.setSeconds(exdate.getSeconds() + 2);

    var c_value = escape(value) + "; expires=" + exdate.toUTCString();

    document.cookie = c_name + "=" + c_value;
}

function Save_Last_Time_HDF(Last_Time_For) {

    var Current_Date = new Date();
    var Current_Time = Current_Date.getTime();

    Add_Hidden_Field('Last_Time_' + Last_Time_For + '_hdf', Current_Time);
}

function Read_Last_Time_HDF(Last_Time_For) {

    var Result = false;

    var Current_Date = new Date();
    var Current_Time = Current_Date.getTime();

    var Last_Time_HDF = $('#Last_Time_' + Last_Time_For + '_hdf').val();

    return (Current_Time - Last_Time_HDF);
}

function Check_Is_Chrome() {
    var Is_Chrome = /chrome/.test(navigator.userAgent.toLowerCase());

    return Is_Chrome;
}

function Add_Hidden_Field(key, value) {

    if (Check_Element_Is_Not_Null(key)) {
        $('#' + key).val(value);
    }
    else {
        var Element_To_Add = $('<input/>', {
            type: 'hidden',
            id: key,
            value: value
        });

        $('#Page_Form_HDF').append(Element_To_Add);
    }
}

function Add_Hidden_Field_For_Uploader(key, value) {

    if (Check_Element_Is_Not_Null(key)) {
        $('#' + key).val(value);
    }
    else {
        var Element_To_Add = $('<input/>', {
            type: 'hidden',
            id: key,
            value: value
        });

        $('#Page_Form_HDF_For_Uploader').append(Element_To_Add);
    }
}

function Split_List_To_Array(List) {

    var Result_List_Array = new Array();

    var List_Array = List.split('#');

    //
    if (List_Array != null) {

        for (var i = 0; i < List_Array.length; i++) {

            if ((List_Array[i] != '#') && (List_Array[i] != '')) {
                Result_List_Array.length = Result_List_Array.length + 1;
                Result_List_Array[Result_List_Array.length - 1] = List_Array[i];
            }
        }
    }

    return Result_List_Array;
}

function Read_File_Name_Extension(File_Name) {
    return "." + File_Name.substr(File_Name.lastIndexOf('.') + 1);
}

function Add_Silent_IMG(Holder_ID, IMG_src, UUID) {

    var Element_To_Add = $('<img/>', {
        src: IMG_src,
        onload: "Enable_Expand_IMG('" + UUID + "');",
        'width': '0px',
        'height': '0px'
    });

    $('#' + Holder_ID).append(Element_To_Add);
}

function Read_Curent_Web_URL() {

    var URL = window.location.href.split('?')[0];

    URL = URL.split('#')[0];

    return URL.toLowerCase().replace("friend.aspx", '').replace("shop.aspx", '').replace("fun.aspx", '');
}

function Read_Domain(URL) {

    URL = Crop_http_From_URL(URL);

    return URL = URL.split('/')[0];
}

function Add_Row_To_Table(Table_ID, innerHTML) {
    $('#' + Table_ID + ' >tbody:last-child').append("<tr><td align='center'>" + innerHTML + "</td></tr>");
}

function Add_List_Item(List_ID, List_Item_ID, InnerHTML) {

    if (Check_Element_Is_Not_Null(List_Item_ID)) {
        $('#' + List_Item_ID).html(InnerHTML);

    } else {
        var Element_To_Add = $('<li/>', {
            id: List_Item_ID,
            html: InnerHTML
        });

        $('#' + List_ID).append(Element_To_Add);
    }
}

function Set_Watermarked_TBX(TBX_ID, Value) {
    var TBX = $get(TBX_ID);
    var Wrapper = Sys.Extended.UI.TextBoxWrapper.get_Wrapper(TBX);

    //var oldValue = Wrapper.get_Value();
    Wrapper.set_Value(Value);
}

function Clear_Watermarked_TBX(TBX_ID) {
    $('#' + TBX_ID).val('');
    $('#' + TBX_ID).select();
}

function Set_Active_AJAX_Tab(Tab_Container_ID, Tab_Index) {

    var Tab_Container = $find(Tab_Container_ID);
    Tab_Container.set_activeTab(Tab_Container.get_tabs()[Tab_Index]);
}

function Creat_JSON_Array(Uploaded_obj) {
    return "[" + JSON.stringify(Uploaded_obj) + "]";
}

function Creat_JSON_one(Uploaded_obj) {
    return JSON.stringify(Uploaded_obj);
}

function Get_Query_String() {

    var vars = [], hash;

    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');

    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }

    return vars;
}

function Add_Item_To_DDL(DDL_ID, Text, Value) {

    $('#' + DDL_ID).append(new Option(Text, Value));
}

//???? Test lai
function Clear_ALL_Input(Holder_ID) {

    $('#' + Holder_ID).children().find('input[type=text], textarea').each(function () {
        $(this).val('');
        //$(this).attr('checked', false);
    });
}

function Set_Checked_CBXL(ID, Checked) {

    var list = $('#' + ID + ' input');

    list.each(function (index) {
        item = $(this);
        if (str.indexOf(item.val()) != -1) {
            item.attr('checked', Checked);
        }
    });
}

function Set_Checked_CBX(ID, Checked) {

    if (Check_Element_Is_Not_Null(ID)) {
        $('#' + ID).attr('checked', Checked);
    }
}

function Add_Item_To_CBXL(CBXL_ID, Text, Value, Checked) {

    var option = document.createElement('input');
    var label = document.createElement('label');

    //
    option.type = 'checkbox';
    label.innerHTML = Text;
    option.value = Value;
    option.setAttribute("checked", Checked);

    //
    document.getElementById(CBXL_ID).appendChild(option);
    document.getElementById(CBXL_ID).appendChild(label);

    /*
    var CBXL = document.getElementById(CBXL_ID);

    var CBXL_Item_Array = CBXL.getElementsByTagName("input");

    if (CBXL_Item_Array.length == 6) {
    document.getElementById(CBXL_ID).appendChild('<br/>');
    }*/
}


function Creat_Default_Friend_Index_1_cbxl() {

    $('#' + 'Default_Friend_Index_1_cbxl').empty();

    var JSON_Array = Parse_JSON_String_To_Array(document.getElementById('Default_Friend_Index_1_List_hdf').value);

    if (JSON_Array != null) {

        for (var i = 0; i < JSON_Array.length; i++) {

            Add_Item_To_CBXL("Default_Friend_Index_1_cbxl", JSON_Array[i].Item_1, JSON_Array[i].Item_1, true);
        }
    }
}

function Add_Item_To_RDOL(RDOL_ID, Text, Value, Checked) {

    var option = document.createElement('input');
    var label = document.createElement('label');

    //
    option.type = 'radio';
    option.value = Value;
    option.setAttribute("checked", Checked);
    option.setAttribute('name', RDOL_ID);

    label.innerHTML = Text;

    //
    document.getElementById(RDOL_ID).appendChild(option);
    document.getElementById(RDOL_ID).appendChild(label);
}

function Check_Loaded(Loaded_For) {
    return Check_HDF('Loaded_' + Loaded_For + '_hdf');
}

function Check_Tab_Is_Disabled(index) {
    return $.inArray(index, $("#ui-tabs").tabs("option", "disabled")) > -1;
}

function Creat_Marquee(Marquee_ID, Direction) {

    if (Check_Element_Is_Not_Null(Marquee_ID)) {

        var Valid_Exists = true;

        $('#' + Marquee_ID).find('*').each(function () {

            if ($(this).attr('class')) {
                if (($(this).attr('class') == 'js-marquee-wrapper') || ($(this).attr('class') == 'js-marquee')) {
                    Valid_Exists = false;
                }
            }
        });

        //
        var Valid_Tab = false;

        if (Marquee_ID == 'Friend_News_Focused_Marquee_1') {

            if ($('#View_Frame_1_Content_Active_Tab_Panel_hdf').val() == 'View_Frame_1_Content_Friend_Home_Tab_pnl') {
                Valid_Tab = true;
            }
        }
        else {
            Valid_Tab = true;
        }

        //
        if (((Valid_Tab) && (Valid_Exists)) && ($('#' + Marquee_ID).width() > 0)) {

            var $mwo = $('#' + Marquee_ID);
            //$('.marquee').marquee();

            //$('#' + Marquee_ID).marquee('destroy');

            //Left or Right
            if ((Direction == 'left') || (Direction == 'right')) {
                $('#' + Marquee_ID).marquee({
                    speed: 10000,
                    gap: 50,
                    delayBeforeStart: 0,
                    direction: Direction,
                    duplicated: false,
                    pauseOnHover: true
                });
            }
            else
                if ((Direction == 'up') || (Direction == 'down')) {
                    //Up or Down
                    $('#' + Marquee_ID).marquee({
                        direction: Direction,
                        speed: 1500
                    });
                }

            //pause and resume links
            $('.pause').click(function (e) {
                e.preventDefault();
                $mwo.trigger('pause');
            });

            $('.resume').click(function (e) {
                e.preventDefault();
                $mwo.trigger('resume');
            });

            //toggle
            $('.toggle').hover(function (e) {
                $mwo.trigger('pause');
            }, function () {
                $mwo.trigger('resume');
            }).click(function (e) {
                e.preventDefault();
            })
        }
    }
}

function Check_Last_Time_HDF(Last_Time_For, Min_Time) {

    var Result = false;

    var Current_Date = new Date();
    var Current_Time = Current_Date.getTime();

    if (Min_Time == null) {
        Add_Hidden_Field('Last_Time_' + Last_Time_For + '_hdf', null);
    }
    else
        if (Min_Time == 0) {
            Add_Hidden_Field('Last_Time_' + Last_Time_For + '_hdf', Current_Time);
        }
        else {
            var Last_Time_HDF = $('#Last_Time_' + Last_Time_For + '_hdf').val();

            if (!Check_Object_NOT_Null_Or_Empty(Last_Time_HDF)) {
                Result = true;
            }
            else {
                if ((Current_Time - Last_Time_HDF) >= Min_Time) {
                    Result = true;
                }
            }

            if (Last_Time_For == 'XXX') {

                Write_Message(Current_Time.toString());

                if (Check_Object_NOT_Null_Or_Empty(Last_Time_HDF)) {
                    Write_Message(' - ' + Last_Time_HDF.toString() + ' = ' + (Current_Time - Last_Time_HDF).toString() + ' : ' + Result.toString());
                }
            }

            if (Result) {
                Add_Hidden_Field('Last_Time_' + Last_Time_For + '_hdf', Current_Time);
            }
        }

    return Result;
}

function Check_HDF(HDF_ID) {

    var Result = false;

    if (Check_Element_Is_Not_Null(HDF_ID)) {
        if ($('#' + HDF_ID).val() == '1') {
            Result = true;
        }
    }

    return Result;
}

function Parse_JSON_String_To_Array(JSON_String) {

    var JSON_Array = new Array();

    try {
        JSON_Array = eval(JSON_String);
    } catch (e) {
        return null;
    }

    return JSON_Array;
}

function Page_Content_onmousemove(event) {
    var x = event.clientX;
    var y = event.clientY;

    //Write_Message(x + ' x ' + y);

    $('#Mouse_X_hdf').val(x);
    $('#Mouse_Y_hdf').val(y);
}



function Config_Highslide() {
    hs.graphicsDir = $('#Index_Host_hdf').val() + '/index/Java_Script/Highslide/graphics/';
    hs.align = 'center';
    hs.outlineType = 'rounded-white';
    hs.wrapperClassName = 'draggable-header';
    //hs.onDimmerClick = function () { return false; }
    hs.allowSizeReduction = true;
}

function Read_AD_Price() {
    return $('#AD_Price_hdf').val();
}

function Read_AD_Province_City() {
    return $('#AD_Province_City_hdf').val();
}

function Read_AD_Maker() {
    return $('#AD_Maker_hdf').val();
}

function Read_AD_Status() {
    return $('#AD_Status_hdf').val();
}

function Read_AD_Source() {
    return $('#AD_Source_hdf').val();
}

function Save_X_Viewed_To_Cookie() {
    //
    if (Check_Element_Is_Not_Null('Shop_X_Detail_td')) {
        //addCookie
        Set_Cookie('X_ID', '', 365);
    }
}

function Setup_Tooltip() {

    $(document).ready(function () {

        //
        //$('a[title]').tooltip({ html: true }); 

        $('a[title]').attr('title', '');

        /*
        $('.tooltip').tooltipster({
        theme: 'tooltipster-my-custom-theme',
        animation: 'swing',
        contentAsHTML: true,
        hideOnClick: true,
        position: 'right'
        });*/

        /*
        $('a[title]').tooltip({
        track: true,
        tooltipClass: 'my-tooltip-styling',
        content: function () { return $(this).attr('title'); }
        });*/
    });
}






function Creat_New_Shop_X_lnk_OnClientClick(URL) {

    $('#Creat_New_Shop_X_lnk_URL_hdf').val(URL);
    Active_View_Frame_1_Content_tab(6);
}


function Auto_Click_Link(ID) {

    if (Check_Element_Is_Not_Null(ID)) {

        if (Check_Element_Visible(ID)) {

            $('#' + ID).click();
        }
    }
}



function Scroll_Up(DIV_ID) {
    Write_Message(DIV_ID);

    $('#' + DIV_ID).scrollTo({ duration: 'slow', offsetTop: '50' });

    Write_Message(DIV_ID);
}

function Scroll_Down(DIV_ID) {
}

function Make_Button_Radio_Style(ID) {

    $('#' + ID).buttonset();

    $('#' + ID + ' label:first').removeClass('ui-corner-left');
    $('#' + ID + ' label:last').removeClass('ui-corner-right');
    $('#' + ID + ' label').addClass('ui-corner-all');
}