
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using MDA.Infrastructure;

namespace MDA.Admin
{
    public class ModelServices
    {
        private static Primitive? _model;
        private readonly string path = Path.Combine(Environment.CurrentDirectory, @"Model\Model.Json");

        public Primitive Model
        {
            get {
                _model ??= GetModelFromFile();                
                return _model; 
            }            
        } 

        public Primitive UpdateModel(Primitive newModel)
        {
            SaveModelToFile(newModel);            
            _model = newModel;           

            return _model;
        }      
        

        private Primitive GetModelFromFile()
        {
            using FileStream openStream = File.OpenRead(path);

            JsonSerializerOptions options = new() { Converters = { new JsonStringEnumConverter() }};           
            Primitive? model = JsonSerializer.Deserialize<Primitive>(openStream, options);           

            if (model == null || model.Entities == null)
                throw new NullReferenceException("Model could not be loaded from file");
                    
            return model;
        }

        private void SaveModelToFile(Primitive newModel)
        {
            using FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, newModel);
            createStream.Dispose();
        }        
    }    
}