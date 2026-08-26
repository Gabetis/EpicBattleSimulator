namespace FSDK
{
    using System.Reflection;

    public interface IConfigureData
    {
        void           BindData();
        void           StoreData();
        PropertyInfo[] GetProperties();
        object         GetPropertyValue(string propertyName);
    }
}