using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MEG.ElasticLogger.ContractResolvers;

public class IgnoringVirtualPropertiesContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (member is not PropertyInfo propertyInfo) 
            return property;
        
        var getterMethod = propertyInfo.GetGetMethod();
        var setterMethod = propertyInfo.GetSetMethod();

        bool isVirtual = getterMethod?.IsVirtual == true || setterMethod?.IsVirtual == true;
            
        if (isVirtual)
            property.ShouldSerialize = _ => false;
        
        return property;
    }
}