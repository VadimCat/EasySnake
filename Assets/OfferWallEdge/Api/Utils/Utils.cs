using System.Diagnostics;

namespace FyberPlugin
{
    public class Utils
		{
			public static void printWarningMessage()
			{
				UnityEngine.Debug.Log( "WARNING: Fyber plugin is not available on this platform." );
				UnityEngine.Debug.Log( "WARNING: the \"" + GetMethodName() + "\" method does not do anything" );
			}
			
			private static string GetMethodName()
			{
				StackTrace st = new StackTrace ();
				StackFrame sf = st.GetFrame (2);
				
				return sf.GetMethod().Name;
			}
		}
}

