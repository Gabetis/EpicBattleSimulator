namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class EasyMobileChecker : APluginChecker
    {
        public override bool IsInstalled()
        {
#if EXISTED_EASY_MOBILE
            return true;
#else
            return false;
#endif
        }
        
        public override void Check()
        {
            bool hadEasyMobile = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Equals("EasyMobile"))
                {
                    hadEasyMobile = true;
                    break;
                }
            }

            if (hadEasyMobile)
            {
                EditorServices.AddDefine(EditorServices.EXIST_EASY_MOBILE);
            }
            else
            {
                EditorServices.RemoveDefine(EditorServices.EXIST_EASY_MOBILE);
            }
        }
    }
}