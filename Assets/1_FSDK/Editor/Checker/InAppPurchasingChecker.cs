namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class InAppPurchasingChecker : APluginChecker
    {
        public override bool IsInstalled()
        {
#if EXISTED_PURCHASING
            return true;
#else
            return false;
#endif
        }
        
        public override void Check()
        {
            bool hadIAP = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Equals("UnityEngine.Purchasing"))
                {
                    hadIAP = true;
                    break;
                }
            }

            if (hadIAP)
            {
                EditorServices.AddDefine(EditorServices.EXIST_PURCHASING);
            }
            else
            {
                EditorServices.RemoveDefine(EditorServices.EXIST_PURCHASING);
            }
        }
    }
}