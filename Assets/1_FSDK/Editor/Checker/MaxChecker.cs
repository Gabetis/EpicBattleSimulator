namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class MaxChecker : APluginChecker
    {
        public override bool IsInstalled()
        {
#if EXISTED_MAX_SDK
            return true;
#else
            return false;
#endif
        }
        
        public override void Check()
        {
            bool hadMax = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Equals("MaxSdk.Scripts"))
                {
                    hadMax = true;
                    break;
                }
            }

            if (hadMax)
            {
                EditorServices.AddDefine(EditorServices.EXIST_MAX_SDK);
            }
            else
            {
                EditorServices.RemoveDefine(EditorServices.EXIST_MAX_SDK);
            }
        }
    }
}