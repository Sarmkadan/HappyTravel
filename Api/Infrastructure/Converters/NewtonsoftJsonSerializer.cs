using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Infrastructure.Converters
{
    public class NewtonsoftJsonSerializer : IJsonSerializer
    {
        public string SerializeObject<T>(T serializingObject) 
            => JsonConvert.SerializeObject(serializingObject);


        public T DeserializeObject<T>(string serializedObject) 
            => JsonConvert.DeserializeObject<T>(serializedObject);
    }
}