import React from 'react';
import DynamicForm from './dynamicform';

const Form = ({entityName, id, onClose}) => {  
  const getResponse = useFetchById(entityName, id);
  const [updateResponse, doUpdate] = useUpdate();      
  const [entityModel, setEntityModel] = React.useState(undefined);   

  React.useEffect(() => { getEntityModel(entityName); }, []); 

  if (getResponse.loading || updateResponse.loading) return <p>Loading...</p>; 
  if (getResponse.error) return <p>{getResponse.error}</p>; 
  if (updateResponse.modelstate) return <p>{updateResponse.modelstate}</p>;  
  if (updateResponse.error) return <p>{updateResponse.error}</p>; 

  const getEntityModel = (name) => {
    const model = JSON.parse(sessionStorage.getItem("model"));
    if (model == undefined)
      throw Error("Model not found in sessionStorage", entity);

    const entity = model.entities.find(e => e.name == name);   
    if (entity == undefined)
      throw Error("Entity not found in Model", entity);
      
    setEntityModel(entity);
  }   

  const onSubmit = (properties) => {   
    doUpdate(entityName, id, properties);
    onClose();
  }  

  const onDelete = (id) => {    
    deleteMe(entityName, id);
    onClose();
  }    

  const onCancel = () => {    
    onClose();
  }  

  return (    
    <div className="form">
      <DynamicForm 
        id = { id }
        entityModel = { entityModel }  
        properties  = { getResponse.data }          
        onCancel    = { onCancel }
        onSubmit    = { onSubmit }
        onDelete    = { onDelete }   
      />
    </div>
  );     
}

export default Form;