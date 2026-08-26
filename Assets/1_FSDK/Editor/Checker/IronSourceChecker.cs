namespace FSDK
{
    using System;
    using System.Reflection;

    public class IronSourceChecker : APluginChecker
    {
        private Assembly _asEditor;
        public override bool IsInstalled()
        {
#if EXISTED_IRON_SOURCE
            return true;
#else
            return false;
#endif
        }

        public override void Check()
        {
            bool hadIronSource = false;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == "Assembly-CSharp-Editor")
                {
                    _asEditor = assembly;
                    break;
                }
            }
            
            if (_asEditor != null)
            {
                hadIronSource = _asEditor.GetType("IronSourceMenu") != null;
            }

            if (hadIronSource)
            {
                EditorServices.AddDefine(EditorServices.EXIST_IRON_SOURCE);
            }
            else
            {
                EditorServices.RemoveDefine(EditorServices.EXIST_IRON_SOURCE);
            }
        }
    }
}