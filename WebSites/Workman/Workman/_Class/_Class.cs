using System;

namespace _IQwinwin
{
    public class _IQ
    {
        #region Global asax 
        public void Application_Start(object sender, EventArgs e)
        {
            Set_Static_Variable_Web();
        }
        public void Application_BeginRequest(object sender, EventArgs e)
        {
            lib.Application_BeginRequest(sender, e);
        }
        public void Application_Error(object sender, EventArgs e)
        {
            lib.Application_Error(sender, e);
        }
        #endregion


        #region --- --- --- Custom From Here --- --- ---

        #endregion


        #region Set_Static_Variable
        public void Set_Static_Variable_Web()
        {
            //lib.Sql_Connection_String = "Data Source='MAI-PHAM'; User ID='sa'; Password='PhamMai@12'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;";
            lib.Sql_Connection_String = "Data Source='10.15.40.17'; User ID='sa'; Password='123@123a'; MultipleActiveResultSets=true; Pooling=true; Max Pool Size=32767; Min Pool Size=1;";
        }
        #endregion        
    }
}
