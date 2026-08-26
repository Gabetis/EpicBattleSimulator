namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class AppsFlyerChecker : APluginChecker
    {
        public override bool IsInstalled()
        {
#if EXISTED_APPS_FLYER
            return true;
#else
            return false;
#endif
        }

        public override void Check()
        {
            bool hadAppsflyer = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Equals("AppsFlyer"))
                {
                    hadAppsflyer = true;
                    break;
                }
            }

            if (hadAppsflyer)
            {
                EditorServices.AddDefine(EditorServices.EXIST_APPS_FLYER);
            }
            else
            {
                EditorServices.RemoveDefine(EditorServices.EXIST_APPS_FLYER);
            }
        }
    }
}